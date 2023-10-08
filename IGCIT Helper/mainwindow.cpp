#include "mainwindow.h"

#include <winreg.h>
#include <sysinfoapi.h>
#include <Lmcons.h>
#include <QSysInfo>
#include <QList>
#include <QFileDialog>
#include <QStandardPaths>
#include <QMessageBox>
#include <QDir>

#define SECURITY_WIN32
#include <security.h>
#pragma comment(lib, "Secur32.lib")

#include "./ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent): QMainWindow(parent), ui(new Ui::MainWindow) {
	ui->setupUi(this);
	ui->tabWidget->setCurrentIndex(0);

	QObject::connect(ui->tabWidget, &QTabWidget::currentChanged, this, &MainWindow::onTabWidgetTabChange);
	QObject::connect(ui->applyTdrBtn, &QPushButton::clicked, this, &MainWindow::onApplyTdrBtnClicked);
	QObject::connect(ui->restoreTdrBtn, &QPushButton::clicked, this, &MainWindow::onRestoreTdrBtnClicked);
	QObject::connect(ui->dumpsEnableBtn, &QPushButton::clicked, this, &MainWindow::onDumpsEnableBtnClicked);
	QObject::connect(ui->dumpsFixWchdBtn, &QPushButton::clicked, this, &MainWindow::onDumpsFixWatchdogDumpsClicked);
	QObject::connect(ui->dumpsRestoreBtn, &QPushButton::clicked, this, &MainWindow::onDumpsRestoreDefaultsClicked);
	QObject::connect(ui->dumpsExtrBtn, &QPushButton::clicked, this, &MainWindow::onDumpsExtractDumpsClicked);
	QObject::connect(ui->dumpsAbortCompressBtn, &QPushButton::clicked, this, &MainWindow::onDumpsCancelExtractBtnClicked);
	QObject::connect(ui->dumpsClearBtn, &QPushButton::clicked, this, &MainWindow::onDumpsClearBtnClicked);
	QObject::connect(ui->ssuAnonBtn, &QPushButton::clicked, this, &MainWindow::onSsuAnonBtnClicked);

	setWindowsBuildLbl();
	setProcessorLbl();
	setMemoryLbl();
	setInfoFromBios();
	setGpusInfo();
}

MainWindow::~MainWindow() {
    delete ui;
}

void MainWindow::setWindowsBuildLbl() const {
	ui->winbuildLbl->setText(QString("%1 (build %2)").arg(QSysInfo::prettyProductName(), QSysInfo::kernelVersion()));
}

void MainWindow::setProcessorLbl() const {
	char procNameBuf[255] = {0};
	DWORD procNameBufLen = 255;
	LSTATUS ret;
	HKEY regKey;

	ret = RegOpenKeyExA(HKEY_LOCAL_MACHINE, R"(HARDWARE\DESCRIPTION\System\CentralProcessor\0)", 0, KEY_READ, &regKey);
	if (ret != ERROR_SUCCESS) {
		writeLog(QString("Failed to open processor registry key: %1").arg(ret));
		return;
	}

	ret = RegGetValueA(regKey, nullptr, "ProcessorNameString", RRF_RT_REG_SZ, nullptr, &procNameBuf, &procNameBufLen);
	if (ret != ERROR_SUCCESS) {
		writeLog(QString("Failed to get processor key value: %1").arg(ret));
		RegCloseKey(regKey);
		return;
	}

	ui->procNameLbl->setText(procNameBuf);
	RegCloseKey(regKey);
}

void MainWindow::setMemoryLbl() const {
	MEMORYSTATUSEX memStat {};

	memStat.dwLength = sizeof(memStat);

	if (GlobalMemoryStatusEx(&memStat) == 0) {
		writeLog(QString("Failed to query memory status: %1").arg(GetLastError()));
		return;
	}

	ui->memoryLbl->setText(getFormattedMemorySize(memStat.ullTotalPhys));
}

void MainWindow::setInfoFromBios() const {
	char sysManufBuf[255] = {0};
	char sysModelBuf[255] = {0};
	DWORD sysManufBufLen = 255;
	DWORD sysModelBufLen = 255;
	LSTATUS ret;
	HKEY regKey;

	ret = RegOpenKeyExA(HKEY_LOCAL_MACHINE, R"(HARDWARE\DESCRIPTION\System\BIOS)", 0, KEY_READ, &regKey);
	if (ret != ERROR_SUCCESS) {
		writeLog(QString("Failed to open bios registry key: %1").arg(ret));
		return;
	}

	ret = RegGetValueA(regKey, nullptr, "SystemManufacturer", RRF_RT_REG_SZ, nullptr, &sysManufBuf, &sysManufBufLen);
	if (ret != ERROR_SUCCESS) {
		writeLog(QString("Failed to get manufacturer key value: %1").arg(ret));
		RegCloseKey(regKey);
		return;
	}

	ret = RegGetValueA(regKey, nullptr, "SystemProductName", RRF_RT_REG_SZ, nullptr, &sysModelBuf, &sysModelBufLen);
	if (ret != ERROR_SUCCESS) {
		writeLog(QString("Failed to get model key value: %1").arg(ret));
		RegCloseKey(regKey);
		return;
	}

	ui->modelLbl->setText(sysModelBuf);
	ui->manufLbl->setText(sysManufBuf);
	RegCloseKey(regKey);
}

void MainWindow::setGpusInfo() const {
	QList<QPair<QString, QString>> gpusInfo;
	QSet<QString> gpusSet;
	DWORD maxValueNameinKeyLen;
	DWORD maxValueDataInKeyLen;
	LPBYTE valueDataBuf;
	LPSTR valueNameBuf;
	DWORD valuesCount;
	LSTATUS ret;
	int rowIdx;
	HKEY rkey;

	ret = RegOpenKeyExA(HKEY_LOCAL_MACHINE, R"(HARDWARE\DEVICEMAP\VIDEO)", 0, KEY_READ, &rkey);
	if (ret != ERROR_SUCCESS) {
		writeLog(QString("Failed to open devmap/video key: %1").arg(ret));
		return;
	}

	ret = RegQueryInfoKeyA(rkey, nullptr, nullptr, nullptr, nullptr, nullptr, nullptr, &valuesCount, &maxValueNameinKeyLen, &maxValueDataInKeyLen, nullptr, nullptr);
	if (ret != ERROR_SUCCESS) {
		writeLog(QString("Failed to query devmap/video: %1").arg(ret));
		RegCloseKey(rkey);
		return;
	}

	// add space for terminators
	++maxValueNameinKeyLen;
	++maxValueDataInKeyLen;

	valueDataBuf = new byte[maxValueDataInKeyLen];
	valueNameBuf = new char[maxValueNameinKeyLen];

	for (DWORD i=0; i<valuesCount; ++i) {
		DWORD valueNameWrittenLen = maxValueNameinKeyLen;
		DWORD valueDataWrittenLen = maxValueDataInKeyLen;
		char tmpBuf[255] = {0};
		DWORD tmpBufLen = 255;
		QPair<QString, QString> gpuInfo;
		std::string valueStr;
		HKEY videoKey;

		std::memset(valueNameBuf, 0, maxValueNameinKeyLen * sizeof(char));
		std::memset(valueDataBuf, 0, maxValueDataInKeyLen * sizeof(byte));

		ret = RegEnumValueA(rkey, i, valueNameBuf, &valueNameWrittenLen, nullptr, nullptr, valueDataBuf, &valueDataWrittenLen);
		if (ret != ERROR_SUCCESS || std::strstr(valueNameBuf, "Video") == nullptr)
			continue;

		valueStr.assign(LPCSTR(valueDataBuf));
		valueStr.erase(0, strlen(R"(\Registry\Machine\)")); // should be the same for all, so no parse but remove

		ret = RegOpenKeyExA(HKEY_LOCAL_MACHINE, valueStr.c_str(), 0, KEY_READ, &videoKey);
		if (ret != ERROR_SUCCESS)
			continue;

		ret = RegGetValueA(videoKey, nullptr, "DriverDesc", RRF_RT_REG_SZ, nullptr, &tmpBuf, &tmpBufLen);
		if (ret != ERROR_SUCCESS || std::strstr(tmpBuf, "Intel") == nullptr || gpusSet.contains(tmpBuf)) {
			RegCloseKey(videoKey);
			continue;
		}

		gpuInfo.first = tmpBuf;

		gpusSet.insert(tmpBuf);
		std::memset(tmpBuf, 0, sizeof(tmpBuf));
		tmpBufLen = 255;

		ret = RegGetValueA(videoKey, nullptr, "DriverVersion", RRF_RT_REG_SZ, nullptr, &tmpBuf, &tmpBufLen);
		if (ret != ERROR_SUCCESS) {
			RegCloseKey(videoKey);
			continue;
		}

		gpuInfo.second = tmpBuf;

		gpusInfo.append(gpuInfo);
		RegCloseKey(videoKey);
	}

	RegCloseKey(rkey);
	delete[] valueDataBuf;
	delete[] valueNameBuf;

	rowIdx = 0;
	for (const QPair<QString, QString> &info: gpusInfo)
		addGpuInfoRow(rowIdx++, info.first, info.second);
}

void MainWindow::updateToolsTab() const {
	DWORD tdrDdiDelaySz = sizeof(DWORD);
	DWORD tdrDelaySz = sizeof(DWORD);
	DWORD tdrDelay, tdrDdiDelay;
	LSTATUS ret;
	HKEY rkey;

	// defaults if no key is present or errors
    ui->tdrdVal->setValue(2);
	ui->tdrddiVal->setValue(5);

	ret = RegOpenKeyExA(HKEY_LOCAL_MACHINE, R"(SYSTEM\CurrentControlSet\Control\GraphicsDrivers)", 0, KEY_READ, &rkey);
	if (ret != ERROR_SUCCESS) {
		writeLog(QString("Failed to open graphicsdrivers key: %1").arg(ret));
		return;
	}

	ret = RegGetValueA(rkey, nullptr, "TdrDelay", RRF_RT_DWORD, nullptr, &tdrDelay, &tdrDelaySz);
	if (ret != ERROR_SUCCESS && ret != ERROR_FILE_NOT_FOUND) {
		writeLog(QString("Failed to get tdrDelay: %1").arg(ret));
		RegCloseKey(rkey);
		return;

	} else if (ret == ERROR_SUCCESS) {
		ui->tdrdVal->setValue(static_cast<int>(tdrDelay));
	}

	ret = RegGetValueA(rkey, nullptr, "TdrDdiDelay", RRF_RT_DWORD, nullptr, &tdrDdiDelay, &tdrDdiDelaySz);
	if (ret != ERROR_SUCCESS && ret != ERROR_FILE_NOT_FOUND) {
		writeLog(QString("Failed to get tdrDdiDelay: %1").arg(ret));
		RegCloseKey(rkey);
		return;

	} else if (ret == ERROR_SUCCESS) {
		ui->tdrddiVal->setValue(static_cast<int>(tdrDdiDelay));
	}

	RegCloseKey(rkey);
}

void MainWindow::writeLog(const QString &msg) const {
	if (ui->tabWidget->currentIndex() != Tabs::Logs)
		ui->tabWidget->setTabText(Tabs::Logs, "Logs*");

	ui->logTxBox->appendPlainText(msg);
}

QString MainWindow::getFormattedMemorySize(DWORDLONG bytes) const {
	char sizeStr[][8] = {"bytes", "KB", "MB", "GB"};
	double sz = static_cast<double>(bytes);
	int sizeIdx = 0;

	while (static_cast<int>(sz / 1024.f) > 0) {
		sz /= 1024.f;
		++sizeIdx;

		if (sizeIdx >= 4)
			return "Unknown";
	}

	return QString("%1 %2").arg(QString::number(sz, 'g', 3), sizeStr[sizeIdx]);
}

void MainWindow::addGpuInfoRow(int gpuIdx, const QString &name, const QString &driver) const {
	int layoutChildrenCount = ui->verticalLayout_4->count();
	QHBoxLayout *driverHlyt = new QHBoxLayout();
	QHBoxLayout *nameHlyt = new QHBoxLayout();
	QLabel *driverStrLbl = new QLabel(driver);
	QLabel *nameStrLbl = new QLabel(name);

	nameStrLbl->setAlignment(Qt::AlignRight);
	nameStrLbl->setTextInteractionFlags(Qt::TextSelectableByMouse);

	driverStrLbl->setAlignment(Qt::AlignRight);
	driverStrLbl->setTextInteractionFlags(Qt::TextSelectableByMouse);

	nameHlyt->addWidget(new QLabel(QString("GPU%1:").arg(gpuIdx)));
	nameHlyt->addWidget(nameStrLbl);

	driverHlyt->addWidget(new QLabel(QString("GPU%1 Driver:").arg(gpuIdx)));
	driverHlyt->addWidget(driverStrLbl);

	ui->verticalLayout_4->insertLayout(layoutChildrenCount - 1, nameHlyt);
	ui->verticalLayout_4->insertLayout(layoutChildrenCount, driverHlyt);
}

void MainWindow::tryReboot() const {
	QMessageBox::StandardButton ret = QMessageBox::question(ui->tabWidget, "Reboot required", "A reboot is required to apply the changes.\nDo you want to reboot now?");
	TOKEN_PRIVILEGES tkp;
	HANDLE hToken;
	DWORD dret;
	BOOL res;

	if (ret == QMessageBox::No)
		return;

	if (!OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, &hToken)) {
		writeLog("Failed to get process token, unable to reboot");
		return;
	}

	LookupPrivilegeValue(nullptr, SE_SHUTDOWN_NAME, &tkp.Privileges[0].Luid);

	tkp.PrivilegeCount = 1;
	tkp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;

	AdjustTokenPrivileges(hToken, FALSE, &tkp, 0, nullptr, nullptr);

	dret = GetLastError();
	if (dret != ERROR_SUCCESS) {
		writeLog(QString("Failed to set privileges, unable to reboot: %1").arg(dret));
		return;
	}

	res = ExitWindowsEx(EWX_REBOOT | EWX_FORCEIFHUNG, SHTDN_REASON_MAJOR_OPERATINGSYSTEM | SHTDN_REASON_MINOR_RECONFIG | SHTDN_REASON_FLAG_PLANNED);
	if (res == FALSE)
		writeLog(QString("Failed to start reboot: %1").arg(ret));
}

int MainWindow::isAdmin() const {
	SID_IDENTIFIER_AUTHORITY NtAuthority = SECURITY_NT_AUTHORITY;
	PSID AdministratorsGroup;
	BOOL isAdm, ret;

	ret = AllocateAndInitializeSid(&NtAuthority, 2, SECURITY_BUILTIN_DOMAIN_RID, DOMAIN_ALIAS_RID_ADMINS, 0, 0, 0, 0, 0, 0, &AdministratorsGroup);
	if (ret == FALSE) {
		writeLog("Failed to alloc and init sid");
		return -1;
	}

	ret = CheckTokenMembership(nullptr, AdministratorsGroup, &isAdm);
	if (ret == FALSE) {
		writeLog("Failed to check token membership");
		FreeSid(AdministratorsGroup);
		return -1;
	}

	FreeSid(AdministratorsGroup);
	return isAdm;
}

bool MainWindow::canPerformAdminOP() const {
	int ret = isAdmin();

	if (ret < 0) {
		writeLog("Failed to check Admin rights, cancelled");
		return false;

	} else if (ret == 0) {
		tryRunAsAdmin();
		return false;
	}

	return true;
}

void MainWindow::tryRunAsAdmin() const {
	QMessageBox::StandardButton bret {QMessageBox::question(this->window(), "Run as Admin", "This operation requires Admin rights, do you want to restart IGCIT Helper in Admin mode?")};
	SHELLEXECUTEINFO ShExecInfo {};
	WCHAR exePath[MAX_PATH] = {0};
	DWORD ret;

	if (bret == QMessageBox::No)
		return;

	ret = GetModuleFileNameW(nullptr, exePath, sizeof(exePath));
	if (ret == 0) {
		writeLog("Failed to get IGCIT Helper path");
		return;
	}

	ShExecInfo.cbSize = sizeof(SHELLEXECUTEINFO);
	ShExecInfo.fMask = 0;
	ShExecInfo.hwnd = nullptr;
	ShExecInfo.lpVerb = L"runas";
	ShExecInfo.lpFile = exePath;
	ShExecInfo.lpParameters = nullptr;
	ShExecInfo.lpDirectory = nullptr;
	ShExecInfo.nShow = SW_SHOWNORMAL;
	ShExecInfo.hInstApp = nullptr;

	ShellExecuteEx(&ShExecInfo);
	QApplication::quit();
}

void MainWindow::onTabWidgetTabChange(int idx) {
	switch (idx) {
		case Tabs::Tools:
			updateToolsTab();
			break;
		case Tabs::Logs:
			ui->tabWidget->setTabText(Tabs::Logs, "Logs");
			break;
		default:
			break;
	}
}

void MainWindow::onApplyTdrBtnClicked() {
	DWORD tdrV = ui->tdrdVal->value();
	DWORD tdrDdiV = ui->tdrddiVal->value();
	LSTATUS ret;
	HKEY rkey;

	if (!canPerformAdminOP())
		return;

	ret = RegOpenKeyExA(HKEY_LOCAL_MACHINE, R"(SYSTEM\CurrentControlSet\Control\GraphicsDrivers)", 0, KEY_SET_VALUE, &rkey);
	if (ret != ERROR_SUCCESS) {
		writeLog(QString("Failed to open graphicsdrivers key: %1").arg(ret));
		return;
	}

	ret = RegSetValueExA(rkey, "TdrDelay", 0, REG_DWORD, reinterpret_cast<BYTE *>(&tdrV), sizeof(DWORD));
	if (ret != ERROR_SUCCESS) {
		writeLog(QString("Failed to set tdr value: %1").arg(ret));
		RegCloseKey(rkey);
		return;
	}

	ret = RegSetValueExA(rkey, "TdrDdiDelay", 0, REG_DWORD, reinterpret_cast<BYTE *>(&tdrDdiV), sizeof(DWORD));
	if (ret != ERROR_SUCCESS) {
		writeLog(QString("Failed to set tdr ddi value: %1").arg(ret));
		RegCloseKey(rkey);
		return;
	}

	RegCloseKey(rkey);
	tryReboot();
}

void MainWindow::onRestoreTdrBtnClicked() {
	LSTATUS ret;
	HKEY rkey;

	if (!canPerformAdminOP())
		return;

	ret = RegOpenKeyExA(HKEY_LOCAL_MACHINE, R"(SYSTEM\CurrentControlSet\Control\GraphicsDrivers)", 0, KEY_SET_VALUE, &rkey);
	if (ret != ERROR_SUCCESS && ret != ERROR_FILE_NOT_FOUND) {
		writeLog(QString("Failed to open graphicsdrivers key: %1").arg(ret));
		return;
	}

	if (ret != ERROR_FILE_NOT_FOUND) {
		ret = RegDeleteValueA(rkey, "TdrDelay");
		if (ret != ERROR_SUCCESS && ret != ERROR_FILE_NOT_FOUND)
			writeLog(QString("Failed to restore tdr value: %1").arg(ret));

		ret = RegDeleteValueA(rkey, "TdrDdiDelay");
		if (ret != ERROR_SUCCESS && ret != ERROR_FILE_NOT_FOUND)
			writeLog(QString("Failed to restore tdr ddi value: %1").arg(ret));

		RegCloseKey(rkey);
	}

	ui->tdrdVal->setValue(2);
	ui->tdrddiVal->setValue(5);
	tryReboot();
}

void MainWindow::onDumpsEnableBtnClicked() {
	std::tuple<HKEY, LPCSTR, LPCSTR, REGSAM, REGSAM, QString> createRegKeys[] {
		std::make_tuple(HKEY_LOCAL_MACHINE, R"(Software\Microsoft\Windows\Windows Error Reporting)", "LocalDumps", KEY_CREATE_SUB_KEY | KEY_WOW64_64KEY, KEY_SET_VALUE | KEY_WOW64_64KEY, "local dumps"),
		std::make_tuple(HKEY_LOCAL_MACHINE, R"(SYSTEM\CurrentControlSet\Control\CrashControl)", "LiveKernelReports", KEY_CREATE_SUB_KEY, KEY_SET_VALUE, "live kernel reports"),
	};
	std::tuple<HKEY, LPCSTR, LPCSTR, REGSAM, DWORD, QString> regKeys[] {
		std::make_tuple(HKEY_LOCAL_MACHINE, R"(Software\Microsoft\Windows\Windows Error Reporting)", "Disabled", KEY_SET_VALUE, 1, "lm windows error report"),
		std::make_tuple(HKEY_CURRENT_USER, R"(Software\Microsoft\Windows\Windows Error Reporting)", "Disabled", KEY_SET_VALUE, 1, "user windows error report"),
		std::make_tuple(HKEY_LOCAL_MACHINE, R"(Software\Microsoft\Windows\Windows Error Reporting\LocalDumps)", "DumpType", KEY_SET_VALUE | KEY_WOW64_64KEY, 2, "local dumps"),
		std::make_tuple(HKEY_LOCAL_MACHINE, R"(SYSTEM\CurrentControlSet\Control\CrashControl\LiveKernelReports)", "DeleteLiveMiniDumps", KEY_SET_VALUE, 0, "live kernel reports"),
		std::make_tuple(HKEY_LOCAL_MACHINE, R"(SYSTEM\CurrentControlSet\Control\CrashControl)", "CrashDumpEnabled", KEY_SET_VALUE, 1, "crash control"),
		std::make_tuple(HKEY_LOCAL_MACHINE, R"(SYSTEM\CurrentControlSet\Control\CrashControl)", "FilterPages", KEY_SET_VALUE, 1, "filter pages")
	};
	const char dumpFolder[] {R"("C:\AppCrashDumps")"};
	LSTATUS ret;
	HKEY rkey;

	if (!canPerformAdminOP())
		return;

	for (const auto &regKey: createRegKeys) {
		HKEY ckey;

		ret = RegOpenKeyExA(std::get<0>(regKey), std::get<1>(regKey), 0, std::get<3>(regKey), &rkey);
		if (ret != ERROR_SUCCESS) {
			writeLog(QString("Failed to open %1 key: %2").arg(std::get<5>(regKey)).arg(ret));
			return;
		}

		ret = RegCreateKeyExA(rkey, std::get<2>(regKey), 0, nullptr, 0, std::get<4>(regKey), nullptr, &ckey, nullptr);
		if (ret != ERROR_SUCCESS) {
			writeLog(QString("Failed to create %1 key: %2").arg(std::get<4>(regKey)).arg(ret));
			RegCloseKey(rkey);
			return;
		}

		RegCloseKey(ckey);
	}

	for (const auto &regKey: regKeys) {
		DWORD val = std::get<4>(regKey);

		ret = RegOpenKeyExA(std::get<0>(regKey), std::get<1>(regKey), 0, std::get<3>(regKey), &rkey);
		if (ret != ERROR_SUCCESS) {
			writeLog(QString("Failed to open %1 key: %2").arg(std::get<5>(regKey)).arg(ret));
			return;
		}

		ret = RegSetValueExA(rkey, std::get<2>(regKey), 0, REG_DWORD, reinterpret_cast<BYTE *>(&val), sizeof(DWORD));
		if (ret != ERROR_SUCCESS) {
			writeLog(QString("Failed to set %1 value: %2").arg(std::get<5>(regKey)).arg(ret));
			RegCloseKey(rkey);
			return;
		}

		RegCloseKey(rkey);
	}

	ret = RegOpenKeyExA(HKEY_LOCAL_MACHINE, R"(Software\Microsoft\Windows\Windows Error Reporting\LocalDumps)", 0, KEY_SET_VALUE | KEY_WOW64_64KEY, &rkey);
	if (ret != ERROR_SUCCESS) {
		writeLog(QString("Failed to open dump folder key: %1").arg(ret));
		return;
	}

	ret = RegSetValueExA(rkey, "DumpFolder", 0, REG_EXPAND_SZ, reinterpret_cast<const BYTE *>(dumpFolder), static_cast<DWORD>(strlen(dumpFolder) + 1));
	if (ret != ERROR_SUCCESS) {
		writeLog(QString("Failed to set dump folder value: %1").arg(ret));
		RegCloseKey(rkey);
		return;
	}

	RegCloseKey(rkey);

	if (!ui->dumpsAdvPagefileChk->isChecked()) {
		const char pagefileVal[] {"C:\\pagefile.sys 17400 17400\0"};
		DWORD valSz = static_cast<DWORD>(strlen(pagefileVal) + 2);

		ret = RegOpenKeyExA(HKEY_LOCAL_MACHINE, R"(SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management)", 0, KEY_SET_VALUE, &rkey);
		if (ret != ERROR_SUCCESS) {
			writeLog(QString("Failed to open mem management key: %1").arg(ret));
			return;
		}

		ret = RegSetValueExA(rkey, "PagingFiles", 0, REG_MULTI_SZ, reinterpret_cast<const BYTE *>(pagefileVal), valSz);
		if (ret != ERROR_SUCCESS) {
			writeLog(QString("Failed to set pagefile value: %1").arg(ret));
			RegCloseKey(rkey);
			return;
		}

		RegCloseKey(rkey);
	}

	tryReboot();
}

void MainWindow::onDumpsFixWatchdogDumpsClicked() {
	LSTATUS ret;
	HKEY rkey;

	if (!canPerformAdminOP())
		return;

	ret = RegOpenKeyExA(HKEY_LOCAL_MACHINE, R"(SYSTEM\CurrentControlSet\Control\CrashControl\LiveKernelReports)", 0, KEY_SET_VALUE, &rkey);
	if (ret != ERROR_SUCCESS && ret != ERROR_FILE_NOT_FOUND) {
		writeLog(QString("Failed to open live kernel reports key: %1").arg(ret));
		return;
	}

	if (ret != ERROR_FILE_NOT_FOUND) {
		ret = RegDeleteTreeA(rkey, "WATCHDOG");

		if (ret != ERROR_SUCCESS)
			writeLog(QString("Failed to reset watchdog dumps: %1").arg(ret));

		RegCloseKey(rkey);
	}

	QMessageBox::information(this, "Success", "Done!");
}

void MainWindow::onDumpsRestoreDefaultsClicked() {
	std::tuple<HKEY, LPCSTR, LPCSTR, REGSAM, QString> regKeys[] {
		std::make_tuple(HKEY_LOCAL_MACHINE, R"(Software\Microsoft\Windows\Windows Error Reporting)", "Disabled", KEY_SET_VALUE, "lm windows error report"),
		std::make_tuple(HKEY_CURRENT_USER, R"(Software\Microsoft\Windows\Windows Error Reporting)", "Disabled", KEY_SET_VALUE, "user windows error report"),
		std::make_tuple(HKEY_LOCAL_MACHINE, R"(Software\Microsoft\Windows\Windows Error Reporting\LocalDumps)", "DumpType", KEY_SET_VALUE | KEY_WOW64_64KEY, "local dumps"),
		std::make_tuple(HKEY_LOCAL_MACHINE, R"(SYSTEM\CurrentControlSet\Control\CrashControl\LiveKernelReports)", "DeleteLiveMiniDumps", KEY_SET_VALUE, "live kernel reports"),
		std::make_tuple(HKEY_LOCAL_MACHINE, R"(SYSTEM\CurrentControlSet\Control\CrashControl)", "FilterPages", KEY_SET_VALUE, "filter pages")
	};
	DWORD three = 3;
	LSTATUS ret;
	HKEY rkey;

	if (!canPerformAdminOP())
		return;

	for (const auto &regKey: regKeys) {
		ret = RegOpenKeyExA(std::get<0>(regKey), std::get<1>(regKey), 0, std::get<3>(regKey), &rkey);
		if (ret != ERROR_SUCCESS) {
			if (ret != ERROR_FILE_NOT_FOUND)
				writeLog(QString("Failed to open %1 key: %2").arg(std::get<4>(regKey)).arg(ret));

			continue;
		}

		ret = RegDeleteValueA(rkey, std::get<2>(regKey));
		if (ret != ERROR_SUCCESS)
			writeLog(QString("Failed to set %1 value: %2").arg(std::get<4>(regKey)).arg(ret));

		RegCloseKey(rkey);
	}

	ret = RegOpenKeyExA(HKEY_LOCAL_MACHINE, R"(SYSTEM\CurrentControlSet\Control\CrashControl)", 0, KEY_SET_VALUE, &rkey);
	if (ret != ERROR_SUCCESS && ret != ERROR_FILE_NOT_FOUND) {
		writeLog(QString("Failed to open crash control key: %1").arg(ret));
		return;
	}

	if (ret != ERROR_FILE_NOT_FOUND) {
		ret = RegSetValueExA(rkey, "CrashDumpEnabled", 0, REG_DWORD, reinterpret_cast<BYTE *>(&three), sizeof(DWORD));
		if (ret != ERROR_SUCCESS) {
			writeLog(QString("Failed to set crash control value: %1").arg(ret));
			RegCloseKey(rkey);
			return;
		}

		RegCloseKey(rkey);
	}

	if (!ui->dumpsAdvPagefileChk->isChecked()) {
		const char pagefileVal[] {"?:\\pagefile.sys\0"};
		DWORD valSz = static_cast<DWORD>(strlen(pagefileVal) + 2);

		ret = RegOpenKeyExA(HKEY_LOCAL_MACHINE, R"(SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management)", 0, KEY_SET_VALUE, &rkey);
		if (ret != ERROR_SUCCESS && ret != ERROR_FILE_NOT_FOUND) {
			writeLog(QString("Failed to open mem management key: %1").arg(ret));
			return;
		}

		if (ret != ERROR_FILE_NOT_FOUND) {
			ret = RegSetValueExA(rkey, "PagingFiles", 0, REG_MULTI_SZ, reinterpret_cast<const BYTE *>(pagefileVal), valSz);
			if (ret != ERROR_SUCCESS) {
				writeLog(QString("Failed to set pagefile value: %1").arg(ret));
				RegCloseKey(rkey);
				return;
			}

			RegCloseKey(rkey);
		}
	}

	tryReboot();
}

void MainWindow::onDumpsExtractDumpsClicked() {
	QString outD {QFileDialog::getExistingDirectory(this, "Select output folder", QStandardPaths::writableLocation(QStandardPaths::DesktopLocation))};
	QString outPath {QString("%1/compressed_dumps.7z").arg(outD)};
	QFile outf(outPath);

	if (outD.isEmpty())
		return;

	extractDumpsCompressThd = new ExtractDumpsCompressThread(this);

	QObject::connect(extractDumpsCompressThd, &ExtractDumpsCompressThread::progressUpdated, this, &MainWindow::onProgressUpdated);
	QObject::connect(extractDumpsCompressThd, &ExtractDumpsCompressThread::logMessageWritten, this, &MainWindow::writeLog);
	QObject::connect(extractDumpsCompressThd, &ExtractDumpsCompressThread::resultReady, this, &MainWindow::onExtractDumpsCompressedThdResultready);
	QObject::connect(extractDumpsCompressThd, &ExtractDumpsCompressThread::finished, this, &MainWindow::onExtractDumpsCompressedFinished);

	if (outf.exists())
		outf.remove();

	ui->dumpsExtrBtn->setEnabled(false);
	ui->dumpsExtractProgbar->setValue(0);
	extractDumpsCompressThd->setOutputPath(outPath);
	extractDumpsCompressThd->start();
	ui->dumpsAbortCompressBtn->setEnabled(true);
}

void MainWindow::onExtractDumpsCompressedFinished() {
	if (extractDumpsCompressThd->isCancelled())
		QFile(extractDumpsCompressThd->getOutputPath()).remove();

	extractDumpsCompressThd->deleteLater();
	ui->dumpsExtractProgbar->setValue(0);
	ui->dumpsExtrBtn->setEnabled(true);
}

void MainWindow::onProgressUpdated(int progress) {
	ui->dumpsExtractProgbar->setValue(progress);
}

void MainWindow::onExtractDumpsCompressedThdResultready(bool res, const QString &outPath) {
	QFile outF(outPath);

	ui->dumpsAbortCompressBtn->setEnabled(false);
	ui->dumpsExtractProgbar->setValue(0);
	ui->dumpsExtrBtn->setEnabled(true);

	if (res && outF.exists())
		QMessageBox::information(this, "Success", QString("Dumps compressed to %1").arg(outPath));
	else if (res && !outF.exists())
		QMessageBox::information(this, "Success", "No dumps have been found");
	else
		QMessageBox::warning(this, "Error", "Failed to extract and compress dumps");
}

void MainWindow::onDumpsCancelExtractBtnClicked() {
	QObject::disconnect(extractDumpsCompressThd, &ExtractDumpsCompressThread::progressUpdated, this, &MainWindow::onProgressUpdated);
	QObject::disconnect(extractDumpsCompressThd, &ExtractDumpsCompressThread::logMessageWritten, this, &MainWindow::writeLog);
	QObject::disconnect(extractDumpsCompressThd, &ExtractDumpsCompressThread::resultReady, this, &MainWindow::onExtractDumpsCompressedThdResultready);

	extractDumpsCompressThd->quit();
	extractDumpsCompressThd->requestInterruption();
	ui->dumpsAbortCompressBtn->setEnabled(false);
}

void MainWindow::onDumpsClearBtnClicked() {
	if (!canPerformAdminOP())
		return;

	QDir appCrash {R"(C:\AppCrashDumps)"};
	QDir miniDump {R"(C:\Windows\Minidump)"};
	QDir watchdog {R"(C:\Windows\LiveKernelReports\WATCHDOG)"};

	if (appCrash.exists() && !appCrash.removeRecursively())
		writeLog(R"(Failed to clean C:\AppCrashDumps)");

	if (miniDump.exists() && !miniDump.removeRecursively())
		writeLog(R"(Failed to clean C:\Windows\Minidump)");

	if (watchdog.exists() && !watchdog.removeRecursively())
		writeLog(R"(Failed to clean C:\Windows\LiveKernelReports\WATCHDOG)");

	QDir().mkdir(R"(C:\AppCrashDumps)");
	QDir().mkdir(R"(C:\Windows\Minidump)");
	QDir().mkdir(R"(C:\Windows\LiveKernelReports\WATCHDOG)");

	QMessageBox::information(this, "Success", "done!");
}

void MainWindow::onSsuAnonBtnClicked() {
	QString ssu = QFileDialog::getOpenFileName(this, "Open SSU Report", QStandardPaths::writableLocation(QStandardPaths::DesktopLocation), "Text files (*.txt)");
	QFile ssuFile {ssu};
	QString outPath {QString("%1/igcit_ssu.txt").arg(QFileInfo(ssuFile).absolutePath())};
	QFile ssuOut {outPath};
	char username[UNLEN + 1] = {0};
	char samUname[UNLEN + 1] = {0};
	DWORD usernameLen = UNLEN + 1;
	DWORD samUnameLen = UNLEN + 1;
	QString ssuCont;
	BOOL bret;

	if (ssu.isEmpty())
		return;

	if (!ssuFile.open(QIODevice::ReadOnly)) {
		writeLog("Failed to read ssu report file");
		return;

	} else if (!ssuOut.open(QIODevice::WriteOnly)) {
		writeLog(QString("Failed to create output file: %1").arg(outPath));
		ssuFile.close();
		return;
	}

	ssuCont = ssuFile.readAll();

	for (const QRegularExpression &rex: ssuRexList)
		ssuCont.replace(rex, "");

	bret = GetUserNameExA(EXTENDED_NAME_FORMAT::NameSamCompatible, samUname, &samUnameLen);
	if (bret > 0)
		ssuCont.replace(samUname, "");

	bret = GetUserNameA(username, &usernameLen);
	if (bret > 0)
		ssuCont.replace(username, "");

	if (ssuOut.write(ssuCont.toUtf8()) == -1)
		writeLog(QString("Failed to write %1").arg(outPath));
	else
		QMessageBox::information(this, "Anonimyze SSU", QString("output: %1").arg(outPath));

	ssuOut.close();
	ssuFile.close();
}

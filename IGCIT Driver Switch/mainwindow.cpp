#include <cstring>
#include <Windows.h>
#include <winreg.h>
#include <QDir>
#include <QUrl>
#include <QDesktopServices>
#include <QMessageBox>

#define _WIN32_DCOM
#include <WbemIdl.h>
#include <wbemcli.h>
#pragma comment(lib, "wbemuuid.lib")

#include "mainwindow.h"
#include "./ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent): QMainWindow(parent), ui(new Ui::MainWindow) {
    ui->setupUi(this);
	setWindowIcon(QIcon(":/IGCITDriverSwitch/IGCIT-logo-dswitch.ico"));

	QList<QAction *> downloadMenu = ui->menuDownloads->actions();
	QList<QAction *> helpMenu = ui->menuHelp->actions();
	QPointer<QAction> menuDwnlSrc = downloadMenu.at(0);
	QPointer<QAction> menuDwnlStableArcXe = downloadMenu.at(2);
	QPointer<QAction> menuHelpWiki = helpMenu.at(0);
	QPointer<QAction> menuHelpAbout = helpMenu.at(1);
	QString driverVer;

	QObject::connect(menuDwnlSrc, &QAction::triggered, this, &MainWindow::onMenuDwnlSrcTriggered);
	QObject::connect(menuDwnlStableArcXe, &QAction::triggered, this, &MainWindow::onMenuDwnlStableArcXeTriggered);
	QObject::connect(menuHelpWiki, &QAction::triggered, this, &MainWindow::onMenuHelpWikiTriggered);
	QObject::connect(menuHelpAbout, &QAction::triggered, this, &MainWindow::onMenuHelpAboutTriggered);
	QObject::connect(ui->driversListCombo, &QComboBox::currentIndexChanged, this, &MainWindow::onDriverComboIndexChanged);
	QObject::connect(ui->restoreBtn, &QPushButton::clicked, this, &MainWindow::onRestoreBtnClicked);
	QObject::connect(ui->loadBtn, &QPushButton::clicked, this, &MainWindow::onLoadBtnClicked);

	writeLog("Getting device info..", Qt::blue);

	idList = getDeviceIDs();
	for (const QString &id: idList)
		writeLog(QString("Found device:\n%1").arg(id), Qt::magenta);

	if (idList.isEmpty()) {
		writeLog("No device found", Qt::red);
		return;
	}

	writeLog("Scanning drivers..", Qt::blue);
	runGetDriversListThread();

	driverVer = getCurrentDriverVersion();

	ui->curGpuDrvLbl->setText(driverVer.isEmpty() ? "Unknown" : driverVer);

	if (!QDir(driverDirPath).exists() && !QDir().mkdir(driverDirPath)) {
		writeLog(QString("Failed to create %1 folder, auto-scan disabled").arg(driverDirPath), Qt::red);

	} else {
		fsWatcher = QSharedPointer<QFileSystemWatcher>::create();

		writeLog(QString("%1: auto-scan enabled").arg(driverDirPath), Qt::magenta);

		fsWatcher->addPath(driverDirPath);
		QObject::connect(fsWatcher.get(), &QFileSystemWatcher::directoryChanged, this, &MainWindow::onDirectoryChanged);
	}

	unlockUI();
}

MainWindow::~MainWindow() {
    delete ui;
}

QString MainWindow::getCurrentDriverVersion() const {
	DWORD maxValueNameinKeyLen;
	DWORD maxValueDataInKeyLen;
	LPBYTE valueDataBuf;
	LPSTR valueNameBuf;
	DWORD valuesCount;
	QString versionStr;
	LSTATUS ret;
	HKEY rkey;

	ret = RegOpenKeyExA(HKEY_LOCAL_MACHINE, R"(HARDWARE\DEVICEMAP\VIDEO)", 0, KEY_READ, &rkey);
	if (ret != ERROR_SUCCESS) {
		writeLog(QString("Failed to open devmap/video key: %1").arg(ret), Qt::red);
		return {};
	}

	ret = RegQueryInfoKeyA(rkey, nullptr, nullptr, nullptr, nullptr, nullptr, nullptr, &valuesCount, &maxValueNameinKeyLen, &maxValueDataInKeyLen, nullptr, nullptr);
	if (ret != ERROR_SUCCESS) {
		writeLog(QString("Failed to query devmap/video: %1").arg(ret), Qt::red);
		RegCloseKey(rkey);
		return {};
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
		std::string valueStr;
		HKEY videoKey;

		std::memset(valueNameBuf, 0, maxValueNameinKeyLen * sizeof(char));
		std::memset(valueDataBuf, 0, maxValueDataInKeyLen * sizeof(byte));

		ret = RegEnumValueA(rkey, i, valueNameBuf, &valueNameWrittenLen, nullptr, nullptr, valueDataBuf, &valueDataWrittenLen);
		if (ret != ERROR_SUCCESS)
			continue;

		if (std::strstr(valueNameBuf, "Video") == nullptr)
			continue;

		valueStr.assign(LPCSTR(valueDataBuf));
		valueStr.erase(0, strlen(R"(\Registry\Machine\)")); // should be the same for all, so no parse but remove

		ret = RegOpenKeyExA(HKEY_LOCAL_MACHINE, valueStr.c_str(), 0, KEY_READ, &videoKey);
		if (ret != ERROR_SUCCESS)
			continue;

		ret = RegGetValueA(videoKey, nullptr, "DriverDesc", RRF_RT_REG_SZ, nullptr, &tmpBuf, &tmpBufLen);
		if (ret != ERROR_SUCCESS) {
			RegCloseKey(videoKey);
			continue;
		}

		if (std::strstr(tmpBuf, "Intel") == nullptr) {
			RegCloseKey(videoKey);
			continue;
		}

		std::memset(tmpBuf, 0, sizeof(tmpBuf));
		tmpBufLen = 255;

		ret = RegGetValueA(videoKey, nullptr, "DriverVersion", RRF_RT_REG_SZ, nullptr, &tmpBuf, &tmpBufLen);
		if (ret != ERROR_SUCCESS) {
			RegCloseKey(videoKey);
			continue;
		}

		versionStr = tmpBuf;

		RegCloseKey(videoKey);
		break;
	}

	RegCloseKey(rkey);
	delete[] valueDataBuf;
	delete[] valueNameBuf;

	return versionStr;
}

QList<QString> MainWindow::getDeviceIDs() const {
	IEnumWbemClassObject *queryEnum = nullptr;
	IWbemServices *wbemSvc = nullptr;
	IWbemLocator *wbemLoc = nullptr;
	QList<QString> devIdList;
	HRESULT ret;

	ret = CoCreateInstance(CLSID_WbemLocator, nullptr, CLSCTX_INPROC_SERVER, IID_IWbemLocator, (LPVOID*) &wbemLoc);
	if (FAILED(ret)) {
		writeLog(QString("Failed to create com instance: %1").arg(ret), Qt::red);
		return {};
	}

	ret = wbemLoc->ConnectServer(BSTR(L"ROOT\\cimv2"), nullptr, nullptr,  nullptr, 0, nullptr, nullptr, &wbemSvc);
	if (FAILED(ret)) {
		writeLog(QString("Failed to connect to com server: %1").arg(ret), Qt::red);
		wbemLoc->Release();
		return {};
	}

	ret = CoSetProxyBlanket(wbemSvc, RPC_C_AUTHN_WINNT, RPC_C_AUTHZ_NONE, nullptr, RPC_C_AUTHN_LEVEL_CALL, RPC_C_IMP_LEVEL_IMPERSONATE, nullptr, EOAC_NONE);
	if (FAILED(ret)) {
		writeLog(QString("Failed to set svc proxy security: %1").arg(ret), Qt::red);
		wbemSvc->Release();
		wbemLoc->Release();
		return {};
	}

	ret = wbemSvc->ExecQuery(BSTR(L"WQL"), BSTR(L"SELECT DeviceID FROM Win32_PnPSignedDriver WHERE DeviceName LIKE '%Intel%' AND DeviceClass = 'display'"), WBEM_FLAG_RETURN_IMMEDIATELY | WBEM_FLAG_FORWARD_ONLY, nullptr, &queryEnum);
	if (FAILED(ret)) {
		writeLog(QString("Failed to exec deviceId query: %1").arg(ret), Qt::red);
		wbemSvc->Release();
		wbemLoc->Release();
		return {};
	}

	if (queryEnum == nullptr) {
		writeLog(QString("Failed to enumerate deviceId query results: %1").arg(ret), Qt::red);
		wbemSvc->Release();
		wbemLoc->Release();
		return {};
	}

	while  (true) {
		IWbemClassObject *wbemObj = nullptr;
		char buf[256] = {0};
		VARIANT vaVar {};
		ULONG eret;

		ret = queryEnum->Next(WBEM_INFINITE, 1, &wbemObj, &eret);
		if (FAILED(ret)) {
			writeLog(QString("Failed to get next deviceId query obj: %1").arg(ret), Qt::red);
			break;
		}

		if (eret == 0)
			break;

		ret = wbemObj->Get(L"DeviceID", 0, &vaVar, nullptr, nullptr);
		if (FAILED(ret)) {
			writeLog(QString("Failed to get device id: %1").arg(ret), Qt::red);
			wbemObj->Release();
			break;
		}

		wcstombs_s(nullptr, buf, vaVar.bstrVal, sizeof(buf) - 1);
		VariantClear(&vaVar);
		wbemObj->Release();

		devIdList.append(buf);
	}

	queryEnum->Release();
	wbemSvc->Release();
	wbemLoc->Release();

	return devIdList;
}

void MainWindow::runGetDriversListThread() {
	ui->driversListCombo->setEnabled(false);

	if (!getDriversListThd.isNull() && getDriversListThd->isRunning()) {
		QObject::disconnect(getDriversListThd, &GetDriversListThread::resultReady, this, &MainWindow::onGetDriversListThdResultReady);
		QObject::disconnect(getDriversListThd, &GetDriversListThread::logMessageWritten, this, &MainWindow::writeLog);
		getDriversListThd->quit();
	}

	getDriversListThd = new GetDriversListThread(this);

	getDriversListThd->setPaths(driverDirPath);

	QObject::connect(getDriversListThd, &GetDriversListThread::resultReady, this, &MainWindow::onGetDriversListThdResultReady);
	QObject::connect(getDriversListThd, &GetDriversListThread::logMessageWritten, this, &MainWindow::writeLog);
	QObject::connect(getDriversListThd, &GetDriversListThread::finished, getDriversListThd, &QObject::deleteLater);

	getDriversListThd->start();
}

void MainWindow::runExtractDriverForInstallThread(const QString &fileName) {
	extractDriverForinstallThd = new ExtractDriverForInstallthread(this);

	extractDriverForinstallThd->setPaths(QString("%1/%2").arg(driverDirPath, fileName), tmpExtractDirPath);

	QObject::connect(extractDriverForinstallThd, &ExtractDriverForInstallthread::finished, extractDriverForinstallThd, &QObject::deleteLater);
	QObject::connect(extractDriverForinstallThd, &ExtractDriverForInstallthread::logMessageWritten, this, &MainWindow::writeLog);
	QObject::connect(extractDriverForinstallThd, &ExtractDriverForInstallthread::progressUpdated, this, &MainWindow::onExtractDriverForInstallThdProgressUpdated);
	QObject::connect(extractDriverForinstallThd, &ExtractDriverForInstallthread::resultReady, this, &MainWindow::onExtractDriverForInstallThdResultReady);

	extractDriverForinstallThd->start();
}

std::wstring MainWindow::getInstallerArgStr() const {
	std::wstring args {L"-s -o"};

	if (ui->argCleanInstChk->isChecked())
		args.append(L" -f");

	if (ui->argNoSigChk->isChecked())
		args.append(L" --unsigned");

	if (ui->argNoExtraChk->isChecked())
		args.append(L" --noExtras");

	if (ui->argRebootAfiChk->isChecked())
		args.append(L" -b");

	if (ui->argSaveReportChk->isChecked())
		args.append(L" --report ../installLog/drvinst.log");

	return args;
}

void MainWindow::writeLog(const QString &msg, const QColor &color) const {
	ui->logtxtEdit->setTextColor(color);
	ui->logtxtEdit->insertPlainText(QString("%1\n").arg(msg));
	ui->logtxtEdit->setTextColor(Qt::black);
}

void MainWindow::unlockUI() const {
	ui->driversListCombo->setEnabled(!idList.isEmpty());
	ui->restoreBtn->setEnabled(!idList.isEmpty());
}

void MainWindow::lockUI() const {
	ui->driversListCombo->setEnabled(false);
	ui->restoreBtn->setEnabled(false);
	ui->progressBar->setValue(0);
}

void MainWindow::onMenuDwnlSrcTriggered() {
	QDesktopServices::openUrl(QUrl("https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT"));
}

void MainWindow::onMenuDwnlStableArcXeTriggered() {
	QDesktopServices::openUrl(QUrl("https://www.intel.com/content/www/us/en/download/785597/intel-arc-iris-xe-graphics-windows.html"));
}

void MainWindow::onMenuHelpWikiTriggered() {
	QDesktopServices::openUrl(QUrl("https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT/wiki/IGCIT-Driver-Switch"));
}

void MainWindow::onMenuHelpAboutTriggered() {
	QMessageBox::information(this, "IGCIT Driver Switch", "v4.0.1, Author IGCIT - GPLv3", QMessageBox::Ok);
}

void MainWindow::onDirectoryChanged(const QString &path) {
	lockUI();
	ui->driversListCombo->setEnabled(false);
	writeLog("Directory changed, updating drivers list..", Qt::blue);
	runGetDriversListThread();
	unlockUI();
}

void MainWindow::onGetDriversListThdResultReady(const QList<QString> &list, const QList<QString> &versionList) {
	QSignalBlocker sblock {ui->driversListCombo};

	ui->driversListCombo->clear();

	if (list.isEmpty()) {
		writeLog("No driver found", Qt::red);
		return;
	}

	this->driverList = list;

	writeLog(QString("Loaded: %1 drivers").arg(list.size()), Qt::darkGreen);
	ui->driversListCombo->addItems(versionList);
	ui->driversListCombo->setCurrentIndex(0);
	ui->driversListCombo->setEnabled(true);
}

void MainWindow::onRestoreBtnClicked() {
	QMessageBox::StandardButton res = QMessageBox::question(this, "Restore driver", "Do you want to restore the driver?");
	SHELLEXECUTEINFO ShExecInfo {};
	QString removecmd;
	std::wstring args;

	if (res == QMessageBox::No)
		return;

	lockUI();
	ui->driversListCombo->setCurrentIndex(0);

	for (const QString &id: idList)
		removecmd.append(QString("pnputil /remove-device \"%1\" && ").arg(id));

	args = QString("/c %1 timeout 2 && pnputil /scan-devices").arg(removecmd).toStdWString();
	ShExecInfo.cbSize = sizeof(SHELLEXECUTEINFO);
	ShExecInfo.fMask = SEE_MASK_NOCLOSEPROCESS;
	ShExecInfo.hwnd = nullptr;
	ShExecInfo.lpVerb = L"runas";
	ShExecInfo.lpFile = L"cmd.exe";
	ShExecInfo.lpParameters = args.c_str();
	ShExecInfo.lpDirectory = nullptr;
	ShExecInfo.nShow = SW_SHOWNORMAL;
	ShExecInfo.hInstApp = nullptr;

	ShellExecuteEx(&ShExecInfo);

	if (ShExecInfo.hProcess != nullptr) {
		restoreDriverWaitThd = new RestoreDriverWaitThread(this);

		restoreDriverWaitThd->setProcessHandle(ShExecInfo.hProcess);
		QObject::connect(restoreDriverWaitThd, &RestoreDriverWaitThread::resultReady, this, &MainWindow::onRestoreDriverWaitThdResultReady);
		QObject::connect(restoreDriverWaitThd, &RestoreDriverWaitThread::finished, restoreDriverWaitThd, &QObject::deleteLater);
		restoreDriverWaitThd->start();
		ui->progressBar->setValue(50);

	} else {
		ui->progressBar->setValue(0);
		unlockUI();
	}
}

void MainWindow::onDriverComboIndexChanged(int idx) {
	QString selected = ui->driversListCombo->currentText();
	QList<QCheckBox *> advOpts {
			ui->argCleanInstChk,
			ui->argNoExtraChk,
			ui->argNoSigChk,
			ui->argRebootAfiChk,
			ui->argSaveReportChk
	};

	for (QCheckBox *chk: advOpts)
		chk->setCheckState(Qt::Unchecked);

	ui->loadBtn->setEnabled(!selected.isEmpty());
	ui->advOptsGroup->setEnabled(!selected.isEmpty());
}

void MainWindow::onLoadBtnClicked() {
	QString selected = ui->driversListCombo->currentText();
	QString fileName;
	int selectedIdx;

	if (selected.isEmpty() || QMessageBox::question(this, "Load driver", "Do you want to load the driver?") == QMessageBox::No)
		return;

	lockUI();

	if (QDir(tmpExtractDirPath).exists() && !QDir(tmpExtractDirPath).removeRecursively()) {
		writeLog("Failed to delete old tmp folder", Qt::red);
		unlockUI();
		return;
	}

	selectedIdx = ui->driversListCombo->currentIndex();
	if (selectedIdx == -1) {
		writeLog("Failed to get selected option index", Qt::red);
		unlockUI();
		return;
	}

	fileName = driverList.at(static_cast<qsizetype>(selectedIdx) - 1);

	writeLog(QString("Selected: %1 (%2)").arg(selected, fileName), Qt::darkBlue);
	runExtractDriverForInstallThread(fileName);
}

void MainWindow::onInstallerProcWaitResultReady(DWORD code) {
	QString driverVer = getCurrentDriverVersion();

	ui->curGpuDrvLbl->setText(driverVer.isEmpty() ? "Unknown" : driverVer);
	ui->progressBar->setValue(100);

	if (code != WAIT_OBJECT_0)
		writeLog("Installer exited early, may not be successful", Qt::red);
	else
		writeLog("Success", Qt::darkGreen);

	unlockUI();
}

void MainWindow::onRestoreDriverWaitThdResultReady() {
	QString driverVer = getCurrentDriverVersion();

	ui->curGpuDrvLbl->setText(driverVer.isEmpty() ? "Unknown" : driverVer);
	ui->progressBar->setValue(100);
	unlockUI();
}

void MainWindow::onExtractDriverForInstallThdProgressUpdated(int progress) {
	ui->progressBar->setValue(progress);
}

void MainWindow::onExtractDriverForInstallThdResultReady(bool res) {
	SHELLEXECUTEINFO ShExecInfo {};
	WCHAR *tmpDirPath = nullptr;
	DWORD bufLen = 256;
	std::wstring args;
	DWORD ret;

	if (!res) {
		unlockUI();
		return;
	}

	writeLog("Loading driver...\nthis may take some minutes and the screen may flicker", Qt::blue);
	ui->progressBar->setValue(0);

	tmpDirPath = new wchar_t[bufLen];
	ret = GetFullPathNameW(tmpExtractDirPath.toStdWString().c_str(), bufLen, tmpDirPath, nullptr);

	if (ret == 0) {
		writeLog("Failed to get tmp folder path", Qt::red);
		delete[] tmpDirPath;
		unlockUI();
		return;
	}

	args = getInstallerArgStr();
	ShExecInfo.cbSize = sizeof(SHELLEXECUTEINFO);
	ShExecInfo.fMask = SEE_MASK_NOCLOSEPROCESS;
	ShExecInfo.hwnd = nullptr;
	ShExecInfo.lpVerb = L"open";
	ShExecInfo.lpFile = L"Installer.exe";
	ShExecInfo.lpParameters = args.c_str();
	ShExecInfo.lpDirectory = tmpDirPath;
	ShExecInfo.nShow = SW_SHOWNORMAL;
	ShExecInfo.hInstApp = nullptr;

	if (args.find(L"report") != std::string::npos) {
		if (!QDir("installLog").exists() && !QDir().mkdir("installLog")) {
			writeLog("Failed to create install log directory", Qt::red);
			delete[] tmpDirPath;
			unlockUI();
			return;
		}

		writeLog("Install log output: installLog/drvinst.log", Qt::blue);
	}

	ui->driversListCombo->setCurrentIndex(0);
	ui->progressBar->setValue(50);
	ShellExecuteEx(&ShExecInfo);

	if (ShExecInfo.hProcess != nullptr) {
		installerProcWaitThd = new InstallerProcessWaitThread(this);

		installerProcWaitThd->setProcesshandle(ShExecInfo.hProcess);
		QObject::connect(installerProcWaitThd, &InstallerProcessWaitThread::resultReady, this, &MainWindow::onInstallerProcWaitResultReady);
		QObject::connect(installerProcWaitThd, &InstallerProcessWaitThread::finished, installerProcWaitThd, &QObject::deleteLater);
		installerProcWaitThd->start();

	} else {
		writeLog("Failed to run Installer", Qt::red);
		ui->progressBar->setValue(0);
		unlockUI();
	}

	delete[] tmpDirPath;
}

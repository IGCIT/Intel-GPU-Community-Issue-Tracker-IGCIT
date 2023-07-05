#pragma once

#include <Windows.h>
#include <QMainWindow>
#include <QPointer>
#include <QRegularExpression>

#include "ExtractDumpsCompressThread.h"

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow {
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private:
	enum Tabs {
		Device = 0,
		Tools,
		Logs,
		About
	};

	QList<QRegularExpression> ssuRexList {
		QRegularExpression {R"((\s+MAC\sAddress:\".*\"))", QRegularExpression::CaseInsensitiveOption | QRegularExpression::MultilineOption},
		QRegularExpression {R"((\s+IP\s(Address|Subnet):\".*\"))", QRegularExpression::CaseInsensitiveOption | QRegularExpression::MultilineOption},
		QRegularExpression {R"((\s+Default\sIP\sGateway:\".*\"))", QRegularExpression::CaseInsensitiveOption | QRegularExpression::MultilineOption},
		QRegularExpression {R"((\s+Access\sPoint:\".*\"))", QRegularExpression::CaseInsensitiveOption | QRegularExpression::MultilineOption},
		QRegularExpression {R"((\s+Serial\sNumber:\".*\"))", QRegularExpression::CaseInsensitiveOption | QRegularExpression::MultilineOption},
		QRegularExpression {R"((\s+Machine\sname:.*$))", QRegularExpression::CaseInsensitiveOption | QRegularExpression::MultilineOption},
		QRegularExpression {R"((\s+Machine\sId:.*$))", QRegularExpression::CaseInsensitiveOption | QRegularExpression::MultilineOption},
		QRegularExpression {R"((\s+Network\sName:\".*\"))", QRegularExpression::CaseInsensitiveOption | QRegularExpression::MultilineOption},
		QRegularExpression {R"((\s+Profile:\".*\"))", QRegularExpression::CaseInsensitiveOption | QRegularExpression::MultilineOption}
	};
	QPointer<ExtractDumpsCompressThread> extractDumpsCompressThd;
    Ui::MainWindow *ui;

	[[nodiscard]]
	QString getFormattedMemorySize(DWORDLONG bytes) const;

	[[nodiscard]]
	int isAdmin() const;

	[[nodiscard]]
	bool canPerformAdminOP() const;

	void setWindowsBuildLbl() const;
	void setProcessorLbl() const;
	void setMemoryLbl() const;
	void setInfoFromBios() const;
	void setGpusInfo() const;
	void writeLog(const QString &msg) const;
	void addGpuInfoRow(int gpuIdx, const QString &label, const QString &value) const;
	void updateToolsTab() const;
	void tryReboot() const;
	void tryRunAsAdmin() const;

private slots:
	void onTabWidgetTabChange(int idx);
	void onApplyTdrBtnClicked();
	void onRestoreTdrBtnClicked();
	void onDumpsEnableBtnClicked();
	void onDumpsFixWatchdogDumpsClicked();
	void onDumpsRestoreDefaultsClicked();
	void onDumpsExtractDumpsClicked();
	void onProgressUpdated(int progress);
	void onExtractDumpsCompressedThdResultready(bool res, const QString &outPath);
	void onExtractDumpsCompressedFinished();
	void onDumpsCancelExtractBtnClicked();
	void onDumpsClearBtnClicked();
	void onSsuAnonBtnClicked();
};

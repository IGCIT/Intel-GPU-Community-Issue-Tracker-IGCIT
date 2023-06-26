#pragma once

#include <QtWidgets/QMainWindow>
#include <QFileSystemWatcher>
#include <QSharedPointer>
#include <QPointer>
#include <QList>

#include "ui_IGCITDriverSwitch.h"
#include "GetDriversListThread.h"
#include "InstallerProcessWaitThread.h"
#include "ExtractDriverForInstallThread.h"
#include "RestoreDriverWaitThread.h"

class IGCITDriverSwitch : public QMainWindow {
    Q_OBJECT

public:
    IGCITDriverSwitch(QWidget *parent = nullptr);
    ~IGCITDriverSwitch() {}

private:    
    const QString driverDirPath {"intel_drivers"};
    const QString tmpExtractDirPath {"extracted_tmp"};
    QPointer<ExtractDriverForInstallthread> extractDriverForinstallThd;
    QPointer<InstallerProcessWaitThread> installerProcWaitThd;
    QPointer<RestoreDriverWaitThread> restoreDriverWaitThd;
    QPointer<GetDriversListThread> getDriversListThd;
    QSharedPointer<QFileSystemWatcher> fsWatcher;
    Ui::IGCITDriverSwitchClass ui;
    QList<QString> driverList;
    QList<QString> idList;

    [[nodiscard]]
    QString getCurrentDriverVersion() const;

    [[nodiscard]]
    QList<QString> getDeviceIDs() const;

    [[nodiscard]]
    std::wstring getInstallerArgStr() const;

    void runGetDriversListThread();
    void runExtractDriverForInstallThread(const QString &fileName);
    void writeLog(const QString &msg, const QColor &color) const;
    void unlockUI() const;
    void lockUI() const;

private slots:
    void onMenuDwnlSrcTriggered();
    void onMenuDwnlStableArcXeTriggered();
    void onMenuDwnlBetaArcXeTriggered();
    void onMenuDwnlStable7_10Triggered();
    void onMenuHelpWikiTriggered();
    void onMenuHelpAboutTriggered();
    void onDirectoryChanged(const QString &path);
    void onGetDriversListThdResultReady(const QList<QString> &driverList, const QList<QString> &versionList);
    void onRestoreBtnClicked();
    void onDriverComboIndexChanged(int idx);
    void onLoadBtnClicked();
    void onInstallerProcWaitResultReady(DWORD code);
    void onRestoreDriverWaitThdResultReady();
    void onExtractDriverForInstallThdProgressUpdated(int progress);
    void onExtractDriverForInstallThdResultReady(bool res);
};

#pragma once


#include <QThread>
#include <Windows.h>

class InstallerProcessWaitThread: public QThread {
	Q_OBJECT

private:
    HANDLE hproc = nullptr;

public:
    explicit InstallerProcessWaitThread(QObject *parent = nullptr): QThread(parent) {}

    void setProcesshandle(HANDLE handle) {
        hproc = handle;
    }

    void run() override {
        DWORD ret = WaitForSingleObject(hproc, 900000); // 15 minutes

        CloseHandle(hproc);
        emit resultReady(ret);
    }

signals:
    void resultReady(DWORD code);
};


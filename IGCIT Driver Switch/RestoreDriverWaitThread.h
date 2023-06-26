#pragma once

#include <QThread>
#include <Windows.h>

class RestoreDriverWaitThread: public QThread {
	Q_OBJECT

private:
	HANDLE hproc = nullptr;

public:
	explicit RestoreDriverWaitThread(QObject *parent = nullptr): QThread(parent) {}

	void setProcessHandle(HANDLE handle) {
		hproc = handle;
	}

	void run() override {
		WaitForSingleObject(hproc, 480000); // 8 minnutes
		CloseHandle(hproc);
		emit resultReady();
	}

signals:
	void resultReady();
};
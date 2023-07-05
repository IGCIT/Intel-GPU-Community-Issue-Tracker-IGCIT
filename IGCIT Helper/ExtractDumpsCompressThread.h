#pragma once

#include <QThread>
#include <QColor>
#include <QDir>
#include <bit7z/bitarchivewriter.hpp>
#include <bit7z/bitexception.hpp>

class ExtractDumpsCompressThread: public QThread {
	Q_OBJECT

private:
	QString outPath;
	bool cancelled = false;

public:
	explicit ExtractDumpsCompressThread(QObject *parent = nullptr): QThread(parent) {}

	[[nodiscard]]
	bool isCancelled() const { return cancelled; }

	[[nodiscard]]
	QString getOutputPath() const { return outPath; }

	void setOutputPath(const QString &path) { outPath = path; }

	void run() override {
        try {
            bit7z::Bit7zLibrary lib {"7z.dll"};
            bit7z::BitArchiveWriter bwriter {lib, bit7z::BitFormat::SevenZip};
            uint64_t totalSz = 1;

			bwriter.setTotalCallback([&totalSz](uint64_t total_size) {
                    totalSz = total_size;
                }
            );

			bwriter.setProgressCallback([this, &totalSz](uint64_t processed_size) {
					if (this->isInterruptionRequested()) {
						cancelled = true;
						return false;
					}

                    emit progressUpdated(static_cast<int>((100.f * processed_size) / totalSz));
                    return true;
                }
            );

			if (QDir(R"(C:\AppCrashDumps)").exists())
				bwriter.addDirectory(R"(C:\AppCrashDumps)");

			if (QDir(R"(C:\Windows\Minidump)").exists())
				bwriter.addDirectory(R"(C:\Windows\Minidump)");

			if (QDir(R"(C:\Windows\LiveKernelReports\WATCHDOG)").exists())
				bwriter.addDirectory(R"(C:\Windows\LiveKernelReports\WATCHDOG)");

			bwriter.setCompressionLevel(bit7z::BitCompressionLevel::Max);
			bwriter.compressTo(outPath.toStdString());

            emit resultReady(true, outPath);
            return;

        } catch (const bit7z::BitException &e) {
            emit logMessageWritten(e.what());
        }

        emit resultReady(false, outPath);
	}

signals:
	void resultReady(bool res, const QString &path);
    void logMessageWritten(const QString &msg);
    void progressUpdated(int progress);
};

#pragma once

#include <QThread>
#include <QColor>
#include <bit7z/bitfileextractor.hpp>
#include <bit7z/bitexception.hpp>

class ExtractDriverForInstallthread: public QThread {
	Q_OBJECT

private:
    QString exePath;
    QString extractPath;

public:
	explicit ExtractDriverForInstallthread(QObject *parent = nullptr): QThread(parent) {}

	void setPaths(const QString &filePath, const QString &extractDirPath) {
		exePath = filePath;
		extractPath = extractDirPath;
	}

	void run() override {
        try {
            bit7z::Bit7zLibrary lib {"7za.dll"};
            bit7z::BitFileExtractor extractor {lib, bit7z::BitFormat::SevenZip};
            uint64_t totalSz = 1;

            extractor.setTotalCallback([&totalSz](uint64_t total_size) {
                    totalSz = total_size;
                }
            );

            extractor.setProgressCallback([this, &totalSz](uint64_t processed_size) {
                    emit progressUpdated(static_cast<int>((100.f * static_cast<float>(processed_size)) / static_cast<float>(totalSz)));
                    return true;
                }
            );

            emit logMessageWritten("Extracting driver..", Qt::blue);
            extractor.extract(exePath.toStdString(), extractPath.toStdString());

            emit resultReady(true);
            return;

        } catch (const bit7z::BitException &e) {
            emit logMessageWritten(e.what(), Qt::red);
        }

        emit resultReady(false);
	}

signals:
	void resultReady(bool res);
	void logMessageWritten(const QString &msg, const QColor &color);
	void progressUpdated(int progress);
};

#pragma once

#include <QThread>
#include <QRegularExpression>
#include <QDir>
#include <QColor>
#include <bit7z/bitarchivereader.hpp>
#include <bit7z/bitfileextractor.hpp>
#include <bit7z/bitexception.hpp>

class GetDriversListThread: public QThread {
	Q_OBJECT

private:
    const QRegularExpression archiveVerStrRex {"SoftwareComponentId=\\\"([\\d\\.]+)\\\"", QRegularExpression::CaseInsensitiveOption};
    QString driverDirPath;

public:
    explicit GetDriversListThread(QObject *parent = nullptr): QThread(parent) {}

    void setPaths(const QString &driversDir) {
        driverDirPath = driversDir;
    }

	void run() override {
        QDir driversDir {driverDirPath};
        QStringList filesList = driversDir.entryList(QDir::Files);
        QList<QString> driverList;
        QList<QString> versionList;

        if (driverDirPath.isEmpty() || !QDir(driverDirPath).exists() || filesList.isEmpty()) {
            emit resultReady({}, {});
            return;
        }

        try {
            bit7z::Bit7zLibrary lib {"7za.dll"};
            
            versionList.append("");

            for (const QString &fileName : filesList) {
                QString filePath {QString("%1/%2").arg(driverDirPath).arg(fileName)};

                try {
                    bit7z::BitArchiveReader arc {lib, filePath.toStdString(), bit7z::BitFormat::SevenZip};
                    std::vector<bit7z::BitArchiveItemInfo> items = arc.items();

                    for (const bit7z::BitArchiveItemInfo &item : items) {
                        if (item.name().compare("igcc_dch.inf") != 0)
                            continue;

                        bit7z::BitFileExtractor extractor {lib, bit7z::BitFormat::SevenZip};
                        QRegularExpressionMatch match;
                        std::vector<byte> buf;
                        QString versionStr;
                        std::string bufStr;
                        QString fileCont;

                        extractor.extractMatching(filePath.toStdString(), item.path(), buf);

                        bufStr = std::string(buf.begin(), buf.end());
                        QTextStream ts {QByteArray(bufStr.c_str(), bufStr.length())};

                        fileCont = ts.readAll();
                        match = archiveVerStrRex.match(fileCont);

                        if (!match.hasMatch() || !match.hasCaptured(1)) {
                            emit logMessageWritten(QString("Unable to find driver version in %1").arg(item.name().c_str()), Qt::red);
                            break;
                        }

                        versionStr = match.captured(1);

                        if (!versionList.contains(versionStr)) {
                            versionList.append(versionStr);
                            driverList.append(fileName);
                        }
                    }
                } catch (const bit7z::BitException &e) {
                    emit logMessageWritten(e.what(), Qt::red);
                }
            }
        } catch (const bit7z::BitException &e) {
            emit logMessageWritten(e.what(), Qt::red);
        }

		emit resultReady(driverList, versionList);
	}

signals:
	void resultReady(const QList<QString> &driverList, const QList<QString> &versionList);
    void logMessageWritten(const QString &msg, const QColor &color);
};
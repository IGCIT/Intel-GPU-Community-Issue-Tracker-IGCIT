#include "IGCITDriverSwitch.h"
#include <QtWidgets/QApplication>

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    IGCITDriverSwitch w;
    w.show();
    return a.exec();
}

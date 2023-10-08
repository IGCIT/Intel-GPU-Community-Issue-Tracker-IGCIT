cmake_minimum_required(VERSION 3.5)

project(IGCITHelper VERSION 2.0.1 LANGUAGES CXX)

set(CMAKE_AUTOUIC ON)
set(CMAKE_AUTOMOC ON)
set(CMAKE_AUTORCC ON)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

enable_language("RC")
set (WIN32_RESOURCES ${CMAKE_CURRENT_SOURCE_DIR}/IGCITHelper.rc)

find_package(QT NAMES Qt6 Qt5 REQUIRED COMPONENTS Widgets)
find_package(Qt${QT_VERSION_MAJOR} REQUIRED COMPONENTS Widgets)

set(PROJECT_SOURCES
        main.cpp
        mainwindow.cpp
        mainwindow.h
        ExtractDumpsCompressThread.h

        mainwindow.ui
        igcithelper-resource.qrc
)

if(${QT_VERSION_MAJOR} GREATER_EQUAL 6)
    qt_add_executable(IGCITHelper
        MANUAL_FINALIZATION
        ${PROJECT_SOURCES}
        ${WIN32_RESOURCES}
    )
else()
    add_executable(IGCITHelper
        ${PROJECT_SOURCES}
        ${WIN32_RESOURCES}
    )
endif()

add_subdirectory(external/bit7z)

target_link_libraries(IGCITHelper PRIVATE Qt${QT_VERSION_MAJOR}::Widgets bit7z)

set_target_properties(IGCITHelper PROPERTIES
    WIN32_EXECUTABLE TRUE
)

install(TARGETS IGCITHelper
    BUNDLE DESTINATION .
    LIBRARY DESTINATION ${CMAKE_INSTALL_LIBDIR}
    RUNTIME DESTINATION ${CMAKE_INSTALL_BINDIR}
)

if(QT_VERSION_MAJOR EQUAL 6)
    qt_finalize_executable(IGCITHelper)
endif()

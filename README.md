## Intel GPU Community Issue Tracker (IGCIT)

![](img/IGCIT-logo-256.png)

**IGCIT** is a Community-driven issue tracker for Intel GPUs.

All the issues here will be reported directly to Intel.

We collect issues for **Windows only**, everything else, like emulators or Linux must be reported to the respective developers.

You can report Mesa issues [here](https://gitlab.freedesktop.org/mesa/mesa).

## How to report a bug

The following information are needed to help Intel to identify and fix the issue more quickly:

* Windows build
* Drivers build
* CPU (_example: i7-1065G7_)
* Rendering API (_DirectX, OpenGL, Vulkan_)
* Rendering API version (_example: DirectX 12_)
* Issue (_Crash, Performance, Freeze, Glitches_)
* Screenshot (_Where possible_)
* Steps to reproduce

# Help

## IGCIT Helper

This is a small utility to easily get your _**Windows build**_, _**CPU name**_ and _**GPU driver**_ version.

Download latest release [here]().

## How to get Windows build number

* Right click _**Windows start button**_

![](img/startBtn.jpg)

* Open _**Settings**_

![](img/windowsSettings.jpg)

* Open _**System**_ settings

![](img/sysSettings.jpg)

* Scroll down to _**About**_ and select it

![](img/about.jpg)

* Get your _Windows **Version**_ and _**OS Build**_ from here

![](img/winBuildVer.jpg)


## How to get GPU driver build number


##### DxDiag _[Method 1]_

* Right click _**Windows start button**_

![](img/startBtn.jpg)

* Open _**Run**_

![](img/run.jpg)

* Type _**dxdiag**_ and click _**OK**_

![](img/dxdiag.jpg)

* Press _**Yes**_ if asked
* Navigate to _**Display**_ tab

![](img/dxdiagDisplay.jpg)

* Get your GPU driver _**Version**_ from here

![](img/dxdiagDriverVer.jpg)


##### Intel Graphics Command Center _[Method 2]_

* Open the _**Intel Graphics Command Center**_

![](img/intelgpucenter.jpg)

* Click _**System**_

![](img/intelgpucenterSys.jpg)

* Navigate to _**GPUs**_ tab

![](img/intelgpucenterGpus.jpg)


* Get your GPU _**Graphics Driver**_ version from here

![](img/intelgpucenterDriverVer.jpg)

## How to get CPU name

* Right click _**Windows start button**_

![](img/startBtn.jpg)

* Open _**Settings**_

![](img/windowsSettings.jpg)

* Open _**System**_ settings

![](img/sysSettings.jpg)

* Scroll down to _**About**_ and select it

![](img/about.jpg)

* Get your _**Processor**_ name from here

![](img/cpuname.jpg)

## How to get in-game screenshots

_**Please remember to hide or cut any sensible data from your screenshots!**_

Some games or launchers may have a screenshot feature.

What we describe here, is a game independent way of taking a screenshot.

This is probably the easiest way, and it is supposed to work for everything.


#### 1. Disable Windows 10 screenshot shortcut

Windows screenshot app is not always reliable, so let's disable it!

* Right click _**Windows start button**_

![](img/startBtn.jpg)

* Open _**Settings**_

![](img/windowsSettings.jpg)

* Open _**Ease of Access**_ settings

![](img/windowsSettingsEOA.jpg)

* Scroll down to _**Keyboard**_ and select it

![](img/windowsSettingsKbd.jpg)

* Scroll down to _**Print Screen shortcut**_ and disable it

![](img/windowsSettingsShot.jpg)

#### 2. Install [Lightshot](https://app.prntscr.com/en/index.html)

This small utility will let you take a screenshot quickly and easily.

* Download the Windows setup [here](https://app.prntscr.com/build/setup-lightshot.exe)
* Run _Lightshot_ setup
* Follow on-screen instructions and complete the installation

You should now have a new icon in your _System tray_.

![](img/lightshotTray.jpg)

* Right click on _**Lightshot**_ icon
* Open _Lightshot_ _**Options**_

![](img/lightshotOpts.jpg)

* Set your options as follows:

![](img/lightshotOptsSet.jpg)

Windows screenshot shortcut will now run _Lightshot_.

# FAQs

> **Q**: How can I help?
>
> **A**: Test games and find issues!

> **Q**: Should I update my issue if it has been fixed by a driver update?
>
> **A**: Yes, if we do not close the issue, please let us know it has been fixed!

## Credits

A big thanks to _**BenjaminLSR**_ for reporting issues to Intel!
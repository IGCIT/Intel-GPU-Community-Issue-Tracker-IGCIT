## Intel GPU Community Issue Tracker (IGCIT)

![](img/IGCIT-logo-256.png)

**IGCIT** is a Community-driven issue tracker for Intel GPUs.

All the issues here will be reported directly to Intel.

We collect issues for **Windows only**, everything else, like Linux must be reported to the respective developers.
We also collect issues from **emulator developers**, if you find a bug in Intel **Windows drivers**, you can report it here.

You can report Mesa issues [here](https://gitlab.freedesktop.org/mesa/mesa).

---


## IGCIT Wiki

IGCIT Wiki contains how-tos, links to latest drivers and release notes, and more..

[Open IGCIT Wiki](https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT/wiki)

---

## How to report a bug

The following information are needed to help Intel to identify and fix the issue more quickly:

**If you want to report a bug in a Windows game**:

* Affected game
* CPU (_example: i7-1065G7_)
* Drivers build
* Rendering API (_DirectX, OpenGL, Vulkan_)
* Rendering API version (_example: DirectX 12_)
* Windows build
* Description
* Steps to reproduce
* Screenshot (_optional_)

**If you are a emulator developer**:

* Emulator name
* CPU or GPU (_example: i7-1065G7_)
* Drivers build
* Rendering API (_DirectX, OpenGL, Vulkan_)
* Rendering API version (_example: DirectX 12_)
* Description
* Steps to reproduce
* Screenshot (_optional_)
* Sample code / affected code (_optional_)
* Executable to reproduce the bug (_optional_)

Once you have collected all the required information, open a new issue describing your problem:

**You have a GitHub account _[Method 1]_**

 Open a new issue [here](https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT/issues).
 
 **You don't have a GitHub account _[Method 2]_**
 
 Submit a new issue [here](https://gitreports.com/issue/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT).
 
 _You do **not** need a GitHub account or any other account for this._
 
 _Just remember to follow IGCIT template that we provide in that page._


## IGCIT Tools


### IGCIT Helper

This is a small utility to easily get your _**Windows build**_, _**CPU name**_ and _**GPU driver build**_ version.

Download latest release [here](https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT/releases/latest).

### IGCIT Driver Switch

A tool to switch your _**Intel GPU driver**_ on the fly, based on Ciphray switch_driver.bat.

Its aim is to help to identify regressions quickly and easily.

See more in [IGCIT Wiki](https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT/wiki)

Download latest release [here](https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT/releases/latest).

---

## FAQs

> **Q**: How can I help?
>
> **A**: Test games and find issues!

> **Q**: Should I update my issue if it has been fixed by a driver update?
>
> **A**: Yes, if we do not close the issue, please let us know it has been fixed!

---


## Credits

* A big thanks to _**BenjaminLSR**_ for reporting issues to Intel!
* Zach Schneider: Git Reports developer.
* Ciphray: switch_driver.bat code.

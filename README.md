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

To open a new issue, some information are required.

Use [IGCIT Helper](https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT/releases) to extract all the device details required by _IGCIT_. (see [IGCIT Wiki](https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT/wiki/IGCIT-Helper)

**To open an issue about a Windows game / application**:

* Game / Application _[Required]_
* Processor _[Required] (example: i7-1065G7)_
* GPU driver version _[Required]_
* Rendering API _[Required] (example DirectX12, Vulkan, etc..)_
* Windows build number _[Required]_
* Description _[Required]_
* Steps to reproduce _[Required]_
* Device name _[Optional][Recommended]_
* Game settings _[Optional][Recommended]_
* Savegame _[Optional]_
* Screenshot _[Optional] (see [IGCIT Wiki](https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT/wiki/How-to-take-in-game-screenshots))_
* Game custom logs _[Optional] (see [IGCIT Wiki](https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT/wiki/Where-to-find-game-custom-logs))_

**To open an issue about your emulator [_Emulator Developers Only_]**:

* Emulator name
* Processor (_example: i7-1065G7_)
* GPU driver version
* Rendering API
* Description
* Steps to reproduce
* Screenshot _[Optional]_
* Sample code / affected code _[Optional]_
* Executable to reproduce the bug _[Optional]_

Once you have collected all the required information, open a new issue describing your problem:

**You have a GitHub account _[Method 1]_**

 Open a new issue [here](https://github.com/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT/issues).
 
 **You don't have a GitHub account _[Method 2]_**
 
 Submit a new issue [here](https://gitreports.com/issue/IGCIT/Intel-GPU-Community-Issue-Tracker-IGCIT).
 
 _You do **not** need a GitHub account or any other account for this._
 
 _Just remember to follow IGCIT template that we provide in that page._


## IGCIT Tools


### IGCIT Helper

This is a small utility to easily extract all the required information about your device.

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

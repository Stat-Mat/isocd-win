# ISOCD-Win

ISOCD-Win is a C#/.NET Windows replacement for the native Amiga ISOCD application written by [Carl Sassenrath](https://en.wikipedia.org/wiki/Carl_Sassenrath) and other developers at Commodore. It creates bootable ISO image files which are compatible with the Amiga CD32 and CDTV. It was written to avoid the need to run the original ISOCD application either on a real or emulated Amiga, thus greatly simplifying and speeding up the process for the creators of new CDs for Amiga computers and consoles.

![isocd-win-screenshot](https://github.com/Stat-Mat/isocd-win/blob/master/isocd-win.jpg)

![isocd-win-options-screenshot](https://github.com/Stat-Mat/isocd-win/blob/master/isocd-win-options.jpg)

![isocd-con-screenshot](https://github.com/Stat-Mat/isocd-win/blob/master/isocd-con.jpg)

## Features

* Has a simple, user-friendly GUI application
* Also includes a console (command-line) application which supports batch processing
* Creates ISO files compatible with the ISO 9660 file system specification, to be used on big-endian (like the Amiga) and little-endian architectures
* Supports injection of the original trademark files from Commodore to allow discs to be booted on the Amiga CD32 and CDTV
* Uses ISO-8859-1 encoding just like AmigaDOS
* Uses uppercase filenames in the generated ISO 9660 path table just like the original ISOCD (actual filenames are left intact) to make the ISO compatible with AmigaDOS
* Uses a case insensitive sort for the file system entries based on path to make the ISO compatible with AmigaDOS
* Supports image padding, which adds blank space at the start of the CDR-74 or CDR-80 image to improve the performance of double speed reading on the Amiga CD32 drive
* Building can be aborted mid-process if needed (multi-threaded)
* Supports launching of WinUAE to test built ISO files before burning
* The image building library is a self-contained assembly (DLL) and could easily be used in other .NET applications

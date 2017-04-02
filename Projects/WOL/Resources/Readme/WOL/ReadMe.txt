BUILDLet WOL
============

This application is the front end to send the magic packet utility 
built on WPF (Windows Presentation Foundation).  
Microsoft .NET Framework 4.5 is required to run this application.

"WOL.conf" is the configuration file for this application.
However, this file is not generated automatically.

If you would like to use history function, please put blank file, which name 
is "WOL.conf", in one of the following folders.
The application searches the configuration file in this order, at being launched.

  1. [My Documents]\BUILDLet\                        (C:\Users\[User Name]\Documents\BUILDLet\)
  2. [Program Folder]\BUILDLet Utilities\WOL\        (C:\Program Files\BUILDLet Utilities\WOL\)
  3. [Program Folder (x86)]\BUILDLet Utilities\WOL\  (C:\Program Files (x86)\BUILDLet Utilities\WOL\)
  4. [Current Directory]

Please note access rights of Program Files folder especially.

If the application finds configuration file successfully, 17 characters of the head 
of each of first 5 lines in the file is read and it is used for MAC address.
MAC address should be written like "FF:FF:FF:FF:FF:FF".

Please note that the configuration file in the program folder will be deleted 
when this application is uninstalled.


Install
-------
Run WOLSetup.exe.


Uninstall
---------
Uninstall "BUILDLet WOL" from "Programs and Features" in Control Panel.


License
-------
This software is released under the MIT License, see License.txt.


History
-------
* April 2, 2017    Version 2.1.1.0  
  Base class library BUILDLet.Utilities is updated from Version 1.x to 2.1.x
* July 20, 2015    Version 1.1.3.0    Minor Update  
  Icon image was a little changed.
* June 15, 2015    Version 1.1.2.0    Minor Update
* June 5, 2015    Version 1.1.1.0    Minor Update
* June 3, 2015    Version 1.1.0.0    Minor Update  
  The "Launch" button was added to the final step of the installation wizard 
  to launch WOL.exe.
* May 29, 2015    Version 1.0.8.0    Minor Update
* May 24, 2015    Version 1.0.7.0    Minor Update
* March 1, 2015    Version 1.0.6  
  Add shourcut in Start Menu, and remove Desktop shortcut.
* February 24, 2015    Version 1.0.5   Change executable file name.
* February 15, 2015    Version 1.0.4  
  File path of private configuration file in "My Documents" folder was changed.
* January 29, 2015    Version 1.0.3  
  MAC address input field was changed into ComboBox from TextBox.
* January 26, 2015    Version 1.0.2    Modify Readme
* January 25, 2015    Version 1.0.1    Modify Readme
* January 25, 2015    Version 1.0.0    1st Release

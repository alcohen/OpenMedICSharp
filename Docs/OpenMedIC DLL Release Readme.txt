This Zip file contains the DLLs necessary to build against OpenMedIC, including
OpenMedIC.dll itself.  It is recommended that you place them in your
Windows\System32\ directory to ensure they are always found by Windows.

These DLLs are already included in the main installation package, and placed by
that installer in your Windows\System32\ directory.

These DLLs are being distributed separately for those who don't want to go through 
the package installation and don't want the source code, but still want to be able
to use the OpenMedIC functionality in their projects.

To include the OpenMedIC functionality in your project:
 - Copy the DLLs in this Zip file in your Windows\System32\ directory
 - In your project, add a reference to Windows\System32\OpenMedIC.dll
 - [optional] In your code, add a line that says:
	using OpenMedIC;

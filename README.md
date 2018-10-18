# Dummy file based CMS

The aim of this repo is explore whether RazorPages can be used to offer a file based user editable CMS.

These RazorPages would live somewhere in a folder above the application root as a git repository (to allow revision control), and will enable the application to be
deployed separately to (some of) its content - without having to resort to using a database.

Unfortunately, RazorPages are currently expected to live under the application root.

The ideal outcome of this would be to present ASP.NET MVC with an enumeration of razor pages which would be indistinguishable from
ones which live under the application root

# Links
- Initial conversation https://twitter.com/sparkeh9/status/1052272110406647809
- The actual CMS attempt https://github.com/sparkeh9/Tapas

# Instructions
- Create a folder somewhere (the configuration is pointing to C:/CMSContent/Pages by default)
- Place a razor page in there [RazorPage.cshtml](README_FILES/RazorPage.cshtml)
- Run the app and navigate to the correct route 
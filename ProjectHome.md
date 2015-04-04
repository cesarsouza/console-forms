System.Console.Forms is a extension to .NET framework to “port” as much as possible of [System.Windows.Forms](http://en.wikipedia.org/wiki/Windows_Forms) to a Console environment.

# Introduction #
[System.Console.Forms](http://crsouza.blogspot.com/2009/03/systemconsoleforms.html) allows for easy creation of complex console user interfaces (CUIs) in a very similar way to a Windows.Forms environment. Applications starts with Application.Run(), Forms are  created inheriting from _Console.Forms.Form_ and layout is done by InitializeComponent().


<p align='center'>
<a href='http://www.comp.ufscar.br/~cesarsouza/resources/images/console/nano-sharp-1.png'><img src='http://www.comp.ufscar.br/~cesarsouza/resources/images/console/nano-sharp-1-thumb.png' /></a></p>


As you can see, it currently lacks some visual enhancements (such as drop-shadow borders and a better default form design), but this is something scheduled only for future releases. The first milestone is to replicate only the basic functionality and interface of [nano](http://en.wikipedia.org/wiki/Nano_(text_editor)), and the second is to fully reproduce MS-DOS [edit.exe](http://en.wikipedia.org/wiki/MS-DOS_Editor). When done, the library will be almost feature complete.

<br />

## Current State ##

Development is currently in very early stages. Much of the base code is almost done (it is already possible to create Forms and UserControls) but most of the default controls, like TextBoxes, RadioButtons and ToolStripMenus are still incomplete or missing.

<br />

If you found this project interesting and want to help, please contact me. All help is welcome!
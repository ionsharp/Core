# Imagin.NET ![](https://img.shields.io/badge/style-4.5-blue.svg?style=flat&label=version) ![](https://img.shields.io/badge/style-Stable-green.svg?style=flat&label=build) ![](https://img.shields.io/badge/style-4.6.1-red.svg?style=flat&label=.NET)
Imagin.NET is a library written in C# that provides common WPF controls and utilities.

## Nuget

There are currently two packages available that have not been updated in over a year. **Do not download these.** Updates to both packages will be noted here.

## Projects

### Common

#### Imagin.Common

Defines common utilities for use in a shared environment.

##### Dependencies

  Name  |  Version  |
--------|-----------|
NETStandard.Library | 2.0.1 |

#### Imagin.Common.WPF

Defines common user interface elements for use in WPF.

##### Dependencies

  Name  |  Version  |  Url  |
--------|-----------|-------|
Imagin.Common | | |
Microsoft.WindowsAPICodePack-Core | 1.1.0.2 | https://github.com/aybe/Windows-API-Code-Pack-1.1 |
Microsoft.WindowsAPICodePack-Shell | 1.1.0 | http://archive.msdn.microsoft.com/WindowsAPICodePack |
System.Reflection.TypeExtensions | 4.4.0 | |
System.Windows.Interactivity.WPF | 2.0.20525 | http://www.microsoft.com/en-us/download/details.aspx?id=10801 |
WpfLocalizeExtension | 3.1.0 | https://github.com/SeriousM/WPFLocalizationExtension/ |
XAMLMarkupExtensions | 1.3.0 | http://xamlmarkupextensions.codeplex.com/ |

##### Controls

  .  |  .  |  .  |  .  |  .  |
-----|-----|-----|-----|-----|
AlignableWrapPanel | AlphaNumericBox | AnglePicker | BasicWindow | ByteUpDown |
CheckerBoard | ColorChip | ColorComb | ColorDialog | ColorPicker |
ComboBox | DataGrid | DateTimeUpDown | DecimalUpDown | DirectionPad |
DoubleUpDown | DualColorChip | EditableLabel | FileBox | FlagCheckView |
FloatUpDown | FontFamilyBox | FontSizeBox | GradientChip | GradientDialog | 
GradientPicker | HexBox | Int16UpDown | Int32UpDown | Int64UpDown | 
Line | Link | MaskedButton | MaskedDropDownButton | MaskedImage | 
MaskedToggleButton | PasswordBox | PropertyGrid | RadioGroup | RegexBox | 
ResourceDictionaryEditor | RippleDecorator | SelectionCanvas | SplitView | StoragePicker | 
TabbedTree | TextBox | ThicknessBox | TimeSpanUpDown | ToggleSwitch | 
TokenView | TransitionalContentControl | TreeView | TreeViewComboBox | UriBox | 
UDoubleUpDown | UInt16UpDown | UInt32UpDown | UInt64UpDown | VersionBox

### Colour

#### Imagin.Colour

Defines utilities for managing color in a shared environment.

##### Dependencies

  Name  |  Version  |
--------|-----------|
Imagin.Common | | |
Microsoft.CSharp | 4.4.1 |
NETStandard.Library | 2.0.1 |

#### Imagin.Colour.WPF

Defines various user interface elements for managing color in WPF.

##### Dependencies

  Name  |  Version  |
--------|-----------|
Imagin.Colour | | |
Imagin.Common | | |
Imagin.Common.WPF | | |
NETStandard.Library | 2.0.1 |

##### Controls

  .  |  .  |  .  |
-----|-----|-----|
ColorChip | ColorComb | ColorDialog | 
ColorPicker | ColorView | DualColorChip | 
GradientChip | GradientDialog | GradientPicker | 

## Donate

[![](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=AJJG6PWLBYQNG)

## License
### Simplified BSD License (BSD)
**Copyright (c) 2017, Imagin**
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

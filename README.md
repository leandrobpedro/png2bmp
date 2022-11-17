# PNG2BMP

## Overview
This program converts one or more `".png"` files into `".bmp"` files. It is also possible to search for a certain color in the source file and replace it with the desired color in the output file.

## How to install
1. Download [png2bmp.exe](https://github.com/leandrobpedro/png2bmp/releases/download/v1.0.1/png2bmp.exe) file.
2. Move `exe` into `%windir%\System32\` or add to the `%PATH%`.

## How to use
```bat
rem for a single file...
png2bmp file.png

rem or for multiple files, use wildcard
png2bmp *.png
```
<table>
    <tr>
        <td>
            <img src="https://github.com/leandrobpedro/png2bmp/blob/media/checkboard.png?raw=true"  alt="png">
        </td>
        <td>
            <p>&#8594;</p>
        </td>
        <td>
            <img src="https://github.com/leandrobpedro/png2bmp/blob/media/solid.png?raw=true"  alt="bmp">
        </td>
    </tr>
</table>

## Convert file and replace color
```bat
rem searches for the color white and replaces it with magenta
png2bmp file.png #ffffff #ff00ff
```
<table>
    <tr>
        <td>
            <img src="https://github.com/leandrobpedro/png2bmp/blob/media/checkboard.png?raw=true"  alt="png">
        </td>
        <td>
            <p>&#8594;</p>
        </td>
        <td>
            <img src="https://github.com/leandrobpedro/png2bmp/blob/media/magenta.png?raw=true"  alt="bmp">
        </td>
    </tr>
</table>


## Help
```plain
png2bmp --help

Usage: png2bmp [options] <Files> <FromColor> <ToColor>

Arguments:
  Files         File path.
  FromColor     Color to find in hex format, e.g.: #ff00ff.
  ToColor       Color to replace in hex format, e.g.: #ff00ff.

Options:image.png
  -?|-h|--help  Show help information.
```
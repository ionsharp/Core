using System;

namespace Imagin.Common.Native
{
#pragma warning disable 0649
    struct IMAGELISTDRAWPARAMS
    {
        public int cbSize;
        public IntPtr himl;
        public int i;
        public IntPtr hdcDst;
        public int x;
        public int y;
        public int cx;
        public int cy;
        /// <summary>
        /// X offest from the upperleft of bitmap.
        /// </summary>
        public int xBitmap;
        /// <summary>
        /// Y offset from the upperleft of bitmap.
        /// </summary>
        public int yBitmap;
        public int rgbBk;
        public int rgbFg;
        public int fStyle;
        public int dwRop;
        public int fState;
        public int Frame;
        public int crEffect;
    }
#pragma warning restore 0649
}
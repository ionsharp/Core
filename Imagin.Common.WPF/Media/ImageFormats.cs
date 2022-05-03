using System.Collections.Generic;
using System.Linq;

namespace Imagin.Common.Media
{
    public struct FileFormat
    {
        readonly string extension;
        public string Extension
        {
            get
            {
                return extension;
            }
        }

        readonly bool readable;
        public bool Readable
        {
            get
            {
                return readable;
            }
        }

        readonly bool writable;
        public bool Writable
        {
            get
            {
                return writable;
            }
        }

        public FileFormat(string Extension, bool Readable, bool Writable)
        {
            extension = Extension;
            readable = Readable;
            writable = Writable;
        }
    }

    public class ImageFormats
    {
        static IEnumerable<FileFormat> formats;
        public static IEnumerable<FileFormat> Formats
        {
            get
            {
                formats = formats ?? Array<FileFormat>.New
                (
                    new FileFormat("image", true, true),
                    //...
                    new FileFormat("aai", true, true),
                    new FileFormat("art", true, true),
                    new FileFormat("arw", true, false),
                    new FileFormat("avi", true, false),
                    new FileFormat("avs", true, true),
                    new FileFormat("bpg", true, true),
                    new FileFormat("bmp", true, true),
                    new FileFormat("bmp2", true, true),
                    new FileFormat("bmp3", true, true),
                    new FileFormat("cals", true, false),   //10
                    new FileFormat("cin", true, true),
                    new FileFormat("cmyk", true, true),
                    new FileFormat("cmyka", true, true),
                    new FileFormat("cr2", true, false),
                    new FileFormat("crw", true, false),
                    new FileFormat("cur", true, false),
                    new FileFormat("cut", true, false),
                    new FileFormat("dcm", true, false),
                    new FileFormat("dcr", true, false),
                    new FileFormat("dcx", true, true),   //20
                    new FileFormat("dds", true, true),
                    new FileFormat("dib", true, true),
                    new FileFormat("djvu", true, false),
                    new FileFormat("dng", true, false),
                    new FileFormat("dot", true, false),
                    new FileFormat("dpx", true, true),
                    new FileFormat("emf", true, false),
                    new FileFormat("epdf", true, false),
                    new FileFormat("epdf", true, true),
                    new FileFormat("exr", true, true),   //30
                    new FileFormat("fax", true, true),
                    new FileFormat("fits", true, true),
                    new FileFormat("fpx", true, true),
                    new FileFormat("fig", true, false),
                    new FileFormat("gif", true, true),
                    new FileFormat("gray", true, true),
                    new FileFormat("hdr", true, true),
                    new FileFormat("hrz", true, true),
                    new FileFormat("ico", true, true),
                    new FileFormat("info", false, true),   //40
                    new FileFormat("inline", true, true),
                    new FileFormat("jp2", true, true),
                    new FileFormat("jpt", true, true),
                    new FileFormat("j2c", true, true),
                    new FileFormat("j2k", true, true),
                    new FileFormat("jpeg", true, true),
                    new FileFormat("jpg", true, true),
                    new FileFormat("json", false, true),
                    new FileFormat("mat", true, false),
                    new FileFormat("miff", true, true),   //50
                    new FileFormat("mono", true, true),
                    new FileFormat("mng", true, true),
                    new FileFormat("m2v", true, true),
                    new FileFormat("mpeg", true, true),
                    new FileFormat("mpc", true, true),
                    new FileFormat("mpr", true, true),
                    new FileFormat("mrw", true, false),
                    new FileFormat("mtv", true, true),
                    new FileFormat("mvg", true, true),
                    new FileFormat("nef", true, false),   //60
                    new FileFormat("orf", true, false),
                    new FileFormat("otb", true, true),
                    new FileFormat("p7", true, true),
                    new FileFormat("palm", false, true),
                    new FileFormat("clipboard", true, false),
                    new FileFormat("pbm", true, true),
                    new FileFormat("pcd", true, true),
                    new FileFormat("pcds", true, true),
                    new FileFormat("pcx", true, true),
                    new FileFormat("pdb", true, true),   //70
                    new FileFormat("pef", true, false),
                    new FileFormat("pfa", true, false),
                    new FileFormat("pfb", true, false),
                    new FileFormat("pfm", true, true),
                    new FileFormat("pgm", true, true),
                    new FileFormat("picon", true, true),
                    new FileFormat("pict", true, true),
                    new FileFormat("pix", true, false),
                    new FileFormat("png", true, true),
                    new FileFormat("pnm", true, true),
                    new FileFormat("ppm", true, true),
                    new FileFormat("psb", true, true),
                    new FileFormat("psd", true, true),
                    new FileFormat("ptif", true, true),   //90
                    new FileFormat("pwp", true, false),
                    new FileFormat("rad", true, false),
                    new FileFormat("raf", true, false),
                    new FileFormat("rgb", true, true),
                    new FileFormat("rgba", true, true),
                    new FileFormat("rfg", true, true),
                    new FileFormat("rla", true, false),
                    new FileFormat("rle", true, false),
                    new FileFormat("sct", true, false),
                    new FileFormat("sfw", true, false),   //100
                    new FileFormat("sgi", true, true),
                    new FileFormat("sun", true, true),
                    new FileFormat("svg", true, true),
                    new FileFormat("tga", true, true),
                    new FileFormat("tiff", true, true),
                    new FileFormat("tim", true, false),
                    new FileFormat("txt", true, true),
                    new FileFormat("uil", false, true),
                    new FileFormat("uyvy", true, true),
                    new FileFormat("vicar", true, true),   //110
                    new FileFormat("viff", true, true),
                    new FileFormat("wbmp", true, true),
                    new FileFormat("wpg", false, true),
                    new FileFormat("x", true, true),
                    new FileFormat("xbm", true, true),
                    new FileFormat("xcf", true, true),
                    new FileFormat("xpm", true, true),
                    new FileFormat("xwd", true, true),
                    new FileFormat("x3f", true, false),
                    new FileFormat("ycbcr", true, true),   //120
                    new FileFormat("ycbcra", true, true),
                    new FileFormat("yuv", true, true)
                );
                return formats;
            }
        }

        public static IEnumerable<FileFormat> Readable
            => Formats.Where(i => i.Readable);

        public static IEnumerable<FileFormat> Writable
            => Formats.Where(i => i.Writable);

        public static bool IsReadable(string extension)
            => Formats.Where(i => i.Extension.ToLower() == extension.ToLower()).FirstOrDefault().Readable;

        public static bool IsWritable(string extension)
            => Formats.Where(i => i.Extension.ToLower() == extension.ToLower()).FirstOrDefault().Writable;
    }
}
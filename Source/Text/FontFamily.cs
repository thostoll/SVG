using System;
using System.IO;
using System.Text;

namespace Svg.Text
{
    /// <summary>
    /// http://stackoverflow.com/questions/3633000/net-enumerate-winforms-font-styles
    /// </summary>
    public class FontFamily
    {
        #region InstalledFont Parameters

        #endregion

        #region InstalledFont Constructor

        private FontFamily(string fontName, string fontSubFamily, string fontPath)
        {
            FontName = fontName;
            FontSubFamily = fontSubFamily;
            FontPath = fontPath;
        }

        #endregion

        #region InstalledFont Properties

        public string FontName { get; set; }

        public string FontSubFamily { get; set; }

        public string FontPath { get; set; }

        #endregion

        public static FontFamily FromPath(string fontFilePath)
        {
            var fontName = string.Empty;
            var fontSubFamily = string.Empty;
            const string encStr = "UTF-8";

            using (var fs = new FileStream(fontFilePath, FileMode.Open, FileAccess.Read))
            {

                var ttOffsetTable = new TtOffsetTable()
                {
                    UMajorVersion = ReadUShort(fs),
                    UMinorVersion = ReadUShort(fs),
                    UNumOfTables = ReadUShort(fs),
                    USearchRange = ReadUShort(fs),
                    UEntrySelector = ReadUShort(fs),
                    URangeShift = ReadUShort(fs),
                };

                var tblDir = new TtTableDirectory();
                var found = false;
                for (var i = 0; i <= ttOffsetTable.UNumOfTables; i++)
                {
                    tblDir = new TtTableDirectory();
                    tblDir.Initialize();
                    fs.Read(tblDir.SzTag, 0, tblDir.SzTag.Length);
                    tblDir.UCheckSum = ReadULong(fs);
                    tblDir.UOffset = ReadULong(fs);
                    tblDir.ULength = ReadULong(fs);

                    var enc = Encoding.GetEncoding(encStr);
                    var s = enc.GetString(tblDir.SzTag);

                    if (string.Compare(s, "name", StringComparison.Ordinal) != 0) continue;
                    found = true;
                    break;
                }

                if (!found) return null;

                fs.Seek(tblDir.UOffset, SeekOrigin.Begin);

                var ttNtHeader = new TtNameTableHeader
                {
                    UfSelector = ReadUShort(fs),
                    UNrCount = ReadUShort(fs),
                    UStorageOffset = ReadUShort(fs)
                };

                for (var j = 0; j <= ttNtHeader.UNrCount; j++)
                {
                    var ttRecord = new TtNameRecord()
                    {
                        UPlatformId = ReadUShort(fs),
                        UEncodingId = ReadUShort(fs),
                        ULanguageId = ReadUShort(fs),
                        UNameId = ReadUShort(fs),
                        UStringLength = ReadUShort(fs),
                        UStringOffset = ReadUShort(fs)
                    };

                    if (ttRecord.UNameId > 2) { break; }

                    var nPos = fs.Position;
                    fs.Seek(tblDir.UOffset + ttRecord.UStringOffset + ttNtHeader.UStorageOffset, SeekOrigin.Begin);

                    var buf = new byte[ttRecord.UStringLength];
                    fs.Read(buf, 0, ttRecord.UStringLength);

                    Encoding enc;
                    if (ttRecord.UEncodingId == 3 || ttRecord.UEncodingId == 1)
                    {
                        enc = Encoding.BigEndianUnicode;
                    }
                    else
                    {
                        enc = Encoding.UTF8;
                    }

                    var strRet = enc.GetString(buf);
                    if (ttRecord.UNameId == 1) { fontName = strRet; }
                    if (ttRecord.UNameId == 2) { fontSubFamily = strRet; }

                    fs.Seek(nPos, SeekOrigin.Begin);
                }

                return new FontFamily(fontName, fontSubFamily, fontFilePath);
            }
        }

        [CLSCompliant(false)]
        public struct TtOffsetTable
        {
            public ushort UMajorVersion;
            public ushort UMinorVersion;
            public ushort UNumOfTables;
            public ushort USearchRange;
            public ushort UEntrySelector;
            public ushort URangeShift;
        }

        [CLSCompliant(false)]
        public struct TtTableDirectory
        {
            public byte[] SzTag;
            public uint UCheckSum;
            public uint UOffset;
            public uint ULength;
            public void Initialize()
            {
                SzTag = new byte[4];
            }
        }

        [CLSCompliant(false)]
        public struct TtNameTableHeader
        {
            public ushort UfSelector;
            public ushort UNrCount;
            public ushort UStorageOffset;
        }

        [CLSCompliant(false)]
        public struct TtNameRecord
        {
            public ushort UPlatformId;
            public ushort UEncodingId;
            public ushort ULanguageId;
            public ushort UNameId;
            public ushort UStringLength;
            public ushort UStringOffset;
        }

        // ReSharper disable once UnusedMember.Local
        private static ushort ReadChar(Stream fs, int characters)
        {
            var s = new string[characters];
            var buf = new byte[Convert.ToByte(s.Length)];

            buf = ReadAndSwap(fs, buf.Length);
            return BitConverter.ToUInt16(buf, 0);
        }

        // ReSharper disable once UnusedMember.Local
        private static ushort ReadByte(Stream fs)
        {
            var buf = new byte[11];
            buf = ReadAndSwap(fs, buf.Length);
            return BitConverter.ToUInt16(buf, 0);
        }

        private static ushort ReadUShort(Stream fs)
        {
            var buf = new byte[2];
            buf = ReadAndSwap(fs, buf.Length);
            return BitConverter.ToUInt16(buf, 0);
        }

        private static uint ReadULong(Stream fs)
        {
            var buf = new byte[4];
            buf = ReadAndSwap(fs, buf.Length);
            return BitConverter.ToUInt32(buf, 0);
        }

        private static byte[] ReadAndSwap(Stream fs, int size)
        {
            var buf = new byte[size];
            fs.Read(buf, 0, buf.Length);
            Array.Reverse(buf);
            return buf;
        }
    }

    class Program
    {
        //static void Main(string[] args)
        //{
        //    System.Drawing.FontFamily fam;
        //    var allInstalledFonts = from e in Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Fonts", false).GetValueNames()
        //                            select Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Fonts").GetValue(e);

        //    var ttfFonts = from e in allInstalledFonts.Where(e => (e.ToString().EndsWith(".ttf") || e.ToString().EndsWith(".otf"))) select e;
        //    var ttfFontsPaths = from e in ttfFonts.Select(e => (Path.GetPathRoot(e.ToString()) == "") ? Environment.GetFolderPath(Environment.SpecialFolder.Fonts) + "\\" + e.ToString() : e.ToString()) select e;
        //    var fonts = from e in ttfFontsPaths.Select(e => GetFontDetails(e.ToString())) select e;

        //    foreach (InstalledFont f in fonts)
        //    {
        //        if (f != null)
        //            Console.WriteLine("Name: " + f.FontName + ", SubFamily: " + f.FontSubFamily + ", Path: " + f.FontPath);
        //    }

        //    Console.ReadLine();
        //}

        
    }
}

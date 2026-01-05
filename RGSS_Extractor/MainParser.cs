using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RGSS_Extractor;

public class MainParser
{
    private Parser parser;

    public List<Entry> ParseFile(string path)
    {
        BinaryReader binaryReader = new BinaryReader(File.OpenRead(path));
        string @string = Encoding.UTF8.GetString(binaryReader.ReadBytes(6));
        if (@string != "RGSSAD")
        {
            return null;
        }

        binaryReader.ReadByte();
        int version = binaryReader.ReadByte();
        parser = CreateParser(version, binaryReader);
        if (parser == null)
        {
            return null;
        }

        parser.ParseFile();
        return parser.entries;
    }

    public byte[] GetFileData(Entry entry)
    {
        return parser.ReadData(entry.Offset, entry.Size, entry.DataKey);
    }

    public void ExportFile(Entry entry, string path)
    {
        parser.WriteFile(entry, path);
    }

    public void ExportArchive(string path)
    {
        if (parser == null)
        {
            return;
        }

        parser.WriteEntries(path);
    }

    [Obsolete]
    public void CloseFile()
    {
        parser.CloseFile();
    }

    private static Parser CreateParser(int version, BinaryReader inFile)
    {
        if (version == 1)
        {
            return new RGSSAD_Parser(inFile);
        }

        if (version == 3)
        {
            return new RGSS3A_Parser(inFile);
        }

        return null;
    }
}
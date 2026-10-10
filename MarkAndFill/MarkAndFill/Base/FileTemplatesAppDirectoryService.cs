using System;
using System.IO;

namespace MarkAndFill.Base;

public class FileTemplatesAppDirectoryService
{
    public static string TemplatesPath { get; private set; } = "";

    public static void Init()
    {
        var directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var fullDirPath = Path.Combine(directoryPath, "MarkAndFill", "Templates");
        if(!Directory.Exists(fullDirPath))
            Directory.CreateDirectory(fullDirPath);
        TemplatesPath = fullDirPath;
    }
}
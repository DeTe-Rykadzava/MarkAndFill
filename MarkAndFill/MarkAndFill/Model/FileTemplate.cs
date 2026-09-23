using System;
using System.Collections.Generic;
using System.Text;

namespace MarkAndFill.Model
{
    public class FileTemplate
    {
        public string Filename { get; set; }
        public string FilePath { get; set; }
        public string FileDirectoryPath { get; set; }
        public DateTime LastChange { get; set; }
        public FileTempGroup FileGroup { get; set; } 
        public FileType FileType { get; set; }
        public bool IsFavorite { get; set; } = false;

        public FileTemplate(string fileName, string filePath, string fileDirectoryPath, FileType fileType, bool isFaforite = false, DateTime? lastChange = null, FileTempGroup? fileGroup = null)
        {
            Filename = fileName;
            FilePath = filePath;
            FileDirectoryPath = fileDirectoryPath;
            LastChange = lastChange ?? DateTime.Now;
            FileGroup = fileGroup ?? FileTempGroup.BaseGroup;
            FileType = fileType;
            IsFavorite = isFaforite;
        }

        public FileTemplate() { }
    }
}

using Avalonia.Media;
using MarkAndFill.Base.Services;
using MarkAndFill.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkAndFill.ViewModels.ModelViewModels
{
    public class FileTemplateViewModel
    {
        private readonly FileTemplate _fileTemplate;

        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public string FilePath { get => _fileTemplate.FilePath; }
        public string FileDirectoryPath { get => _fileTemplate.FileDirectoryPath; }

        public FileGroupViewModel FileGroup { get; private set; }

        public DateTime LastChange => _fileTemplate.LastChange;

        public string FileTypeABS
        {
            get
            {
                if (_fileTemplate.FileType == FileType.Word)
                    return "W";
                else
                    return "M";
            }
        }

        public string FileTypeColor
        {
            get
            {
                if (_fileTemplate.FileType == FileType.Word)
                    return "#1E7BF5";
                else
                    return "#171719";
            }
        }

        public static List<FileTemplateViewModel> FileTemplates
        {
            get
            {
                var result = LastFilesService.GetAllPaths();
                return result.Select(s => new FileTemplateViewModel(s)).ToList();
            }
        }

        public FileTemplateViewModel(FileTemplate fileTemplate)
        {
            _fileTemplate = fileTemplate;
            FileGroup = new FileGroupViewModel(fileTemplate.FileGroup);
            _fileName = _fileTemplate.Filename;
        }

        public async Task<bool> MoveToGroup(FileGroupViewModel group) 
        {
            var groupModel = FileGroupService.GetAllGroups().Where(x => x.GroupName == group.GroupName).FirstOrDefault();
            if(groupModel == null)
                return false;
            LastFilesService.RemoveFile(_fileTemplate);
            _fileTemplate.FileGroup = groupModel;
            LastFilesService.SetFile(_fileTemplate);
            await LastFilesService.SaveAsync();
            return true;
        }

        public static async Task<bool> Remove(FileTemplateViewModel file) 
        {
            try
            {
                LastFilesService.RemoveFile(file._fileTemplate);
                await LastFilesService.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                Debug.WriteLine("Error while remove fileTemplate from list");
                return false;
            }
        }
    }
}

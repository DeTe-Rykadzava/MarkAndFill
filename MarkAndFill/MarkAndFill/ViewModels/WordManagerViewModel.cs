using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MarkAndFill.Base.Managers;
using ReactiveUI;
using MarkAndFill.ViewModels.ModelViewModels;
using DynamicData;
using System.Text.RegularExpressions;

namespace MarkAndFill.ViewModels
{
    public class WordManagerViewModel : ViewModelBase, IRoutableViewModel
    {
        private readonly WordManager _wordManager;
        private readonly FileTemplateViewModel _file;

        public string UrlPathSegment { get; } = "/word_manager";
        public IScreen HostScreen { get; }

        public string FileName { get => _file.FileName; }
        public string FileGroup { get => _file.FileGroup.GroupName; }

        public ObservableCollection<MarkViewModel> Marks { get; } = new();

        public ICommand ReplaceMarksCommand { get; }

        public WordManagerViewModel(IScreen hostScreen, FileTemplateViewModel file)
        {
            _wordManager = new WordManager();
            _file = file;
            HostScreen = hostScreen;
            ReplaceMarksCommand = ReactiveCommand.CreateFromTask(async () => 
            {
                List<MarkViewModel> marksForReplace = Marks.Where(s => !string.IsNullOrWhiteSpace(s.TagValue)).ToList();
                _wordManager.ReplaceTags(_file.FilePath, marksForReplace.ToDictionary(k => k.TagName, v => v.TagValue!));
            });
            Task.Run(Init);
        }

        private void Init()
        {
            try
            {
                var tags = _wordManager.GetUniqueTags(_file.FilePath);
                if (tags == null || !tags.Any())
                    return; // нужна обработка отсутствия тегов в файле
                var marks = new List<MarkViewModel>();
                foreach (var tag in tags)
                {
                    if (marks.Any(a => a.TagName == tag.TagName))
                        continue;
                    var count = tags.Count(c => c.TagName == tag.TagName);
                    var mark = new MarkViewModel(tag,count);
                    marks.Add(mark);
                }
                Marks.AddRange(marks);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Press F =)");
            }
        }
    }
}
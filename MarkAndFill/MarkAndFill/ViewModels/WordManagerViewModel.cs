using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using MarkAndFill.Base.Managers;
using MarkAndFill.Model;
using MarkAndFill.ViewModels.ModelViewModels;
using ReactiveUI;

namespace MarkAndFill.ViewModels;

public class WordManagerViewModel : ViewModelBase, IRoutableViewModel
{
    private readonly FileTemplateViewModel _file;
    private readonly WordManager _wordManager;

    public Interaction<Unit, FileInfo?> ReplaceMarksInteraction { get; } = new();

    public WordManagerViewModel(IScreen hostScreen, FileTemplateViewModel file)
    {
        _wordManager = new WordManager();
        _file = file;
        HostScreen = hostScreen;
        ReplaceMarksCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            try
            {
                var targetFileInfo = await ReplaceMarksInteraction.Handle(Unit.Default);
                if (targetFileInfo == null)
                    return;
                File.Copy(_file.FilePath, targetFileInfo.FullName, true);
                var marksForReplace = Marks.Where(s => !string.IsNullOrWhiteSpace(s.TagValue)).ToList();
                _wordManager.ReplaceTags(targetFileInfo.FullName,
                    marksForReplace.ToDictionary(k => k.TagName, v => v.TagValue!));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        GoBackCommand = ReactiveCommand.CreateFromTask(async () => { await HostScreen.Router.NavigateBack.Execute(); });
        // Task.Run(() => { Init();});
        Init();
    }

    public WordManagerViewModel()
    {
    }

    public string FileName => _file.FileName;
    public string FileGroup => _file.FileGroup.GroupName;

    public ObservableCollection<MarkViewModel> Marks { get; } = new();

    private int _countOfEcvivalentesTags;

    public int CountOfEcvivalentesTags
    {
        get => _countOfEcvivalentesTags;
        set => this.RaiseAndSetIfChanged(ref _countOfEcvivalentesTags, value);
    }

    public ICommand ReplaceMarksCommand { get; }

    public ICommand GoBackCommand { get; }

    public string UrlPathSegment { get; } = "/word_manager";
    public IScreen HostScreen { get; }

    private void Init()
    {
        try
        {
            var tags = _wordManager.GetUniqueTags(_file.FilePath);
            if (tags == null || !tags.Any()) return;

            var marks = tags.GroupBy(t => t.TagName)
                .Select(g => new MarkViewModel(g.First(), g.Count()))
                .ToList();

            Marks.Clear();
            Marks.AddRange(marks);

            CountOfEcvivalentesTags = 0;
            CountOfEcvivalentesTags = Marks.Count(c => c.Count > 1);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка инициализации тегов: {ex.Message}");
            // TODO: Здесь стоит добавить логику уведомления UI об ошибке
        }
    }
}
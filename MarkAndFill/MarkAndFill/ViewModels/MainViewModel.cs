using Avalonia.Controls;
using DynamicData;
using MarkAndFill.Base.Managers;
using MarkAndFill.Model;
using MarkAndFill.ViewModels.ModelViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

namespace MarkAndFill.ViewModels;

public class MainViewModel : ViewModelBase, IScreen
{
    public RoutingState Router { get; } = new RoutingState();

    //public ICommand SelectFileCommand { get; }

    //public Interaction<Unit, Uri?> SelectFileIteraction { get; }
    //private WordManager _wordManager;

    private List<FileTemplateViewModel> _fileTemplates = new();

    public ObservableCollection<FileTemplateViewModel> FileTemplates { get; } = new ObservableCollection<FileTemplateViewModel>();

    public ObservableCollection<FileGroupViewModel> Groups { get; } = new ObservableCollection<FileGroupViewModel>();

    public ICommand AllGroupsCommand { get; }

    public ReactiveCommand<FileGroupViewModel, Unit> ConcreteGroupCommand { get; }

    public ICommand MoveTemplateToGroupCommand { get; }

    public ICommand CreateNewGroupCommand { get; }

    public ICommand RemoveFileTemplateCommand { get; }
    public ICommand OpenInExplorerCommand { get; }
    public ICommand OpenFileCommand { get; }

    public MainViewModel()
    {
        AllGroupsCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            FileTemplates.Clear();
            FileTemplates.AddRange(_fileTemplates);
        });

        ConcreteGroupCommand = ReactiveCommand.CreateFromTask(async (FileGroupViewModel group) =>
        {
            var filesByGroup = _fileTemplates.Where(x => x.FileGroup.GroupName == group.GroupName).ToList();
            FileTemplates.Clear();
            FileTemplates.AddRange(filesByGroup);
            return Unit.Default;
        });

        CreateNewGroupCommand = ReactiveCommand.CreateFromTask(async () =>
        {

        });

        MoveTemplateToGroupCommand = ReactiveCommand.CreateFromTask<(FileGroupViewModel Group, FileTemplateViewModel File), Unit>(async args =>
        {
            try
            {
                await args.File.MoveToGroup(args.Group);
                LoadLastFiles();
                LoadFileGroups();
            }
            catch (Exception)
            {
                Debug.WriteLine("Error while move file to group");
            }
            return Unit.Default;
        });

        RemoveFileTemplateCommand = ReactiveCommand.CreateFromTask(async (FileTemplateViewModel file) =>
        {
            await FileTemplateViewModel.Remove(file);
            LoadLastFiles();
            LoadFileGroups();
        });

        OpenInExplorerCommand = ReactiveCommand.CreateFromTask(async (FileTemplateViewModel file) => 
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "explorer",
                Arguments = $"\"{file.FileDirectoryPath}\"",
                UseShellExecute = true,
            };

            Process.Start(processInfo);
        });

        OpenFileCommand = ReactiveCommand.CreateFromTask(async (FileTemplateViewModel file) => 
        {
            await Router.Navigate.Execute(new WordManagerViewModel(this, file));
            
        });

        //_wordManager = new WordManager();
        //SelectFileIteraction = new Interaction<Unit, Uri?>();
        //SelectFileCommand = ReactiveCommand.CreateFromTask(async () =>
        //{
        //    var result = await SelectFileIteraction.Handle(Unit.Default);
        //    if (result is not null)
        //    {
        //        Greeting = result.AbsolutePath;
        //        var tags = _wordManager.GetUniqueTags(result.AbsolutePath);
        //        Greeting += "\t Найденные теги: ";
        //        var TagMarks = new Dictionary<string, string>();
        //        foreach (var tag in tags) 
        //        {
        //            Greeting += tag;
        //            TagMarks.Add(tag, "{{Норм Текст}}");
        //        }
        //        _wordManager.ReplaceTags(result.AbsolutePath, TagMarks);
        //        Greeting += "\n Успешная замена";
        //    }
        //});
    }

    public void Activate()
    {
        LoadLastFiles();
        LoadFileGroups();
    }

    private async void LoadLastFiles()
    {
        FileTemplates.Clear();
        _fileTemplates.Clear();
        var vms = FileTemplateViewModel.FileTemplates;
        foreach (var item in vms)
        {
            FileTemplates.Add(item);
        }
        _fileTemplates.AddRange(FileTemplates);
    }

    private async void LoadFileGroups()
    {
        Groups.Clear();
        var vms = FileGroupViewModel.GetGroups();
        foreach (var item in vms)
        {
            Groups.Add(item);
        }
    }

}

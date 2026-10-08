using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using MarkAndFill.Base;
using MarkAndFill.Base.Services;
using MarkAndFill.Model;
using MarkAndFill.ViewModels.ModelViewModels;
using ReactiveUI;

namespace MarkAndFill.ViewModels;

public class MainViewModel : ViewModelBase, IScreen
{
    private readonly List<FileTemplateViewModel> _fileTemplates = new();

    public MainViewModel()
    {
        AllGroupsCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            FileTemplates.Clear();
            FileTemplates.AddRange(_fileTemplates);
        });

        ConcreteGroupCommand = ReactiveCommand.CreateFromTask(async (FileGroupViewModel group) =>
        {
            if (group.GroupName == "Последние файлы")
            {
                FileTemplates.Clear();
                LoadLastFiles();
            }
            else
            {
                var filesByGroup = _fileTemplates.Where(x => x.FileGroup.GroupName == group.GroupName).ToList();
                FileTemplates.Clear();
                FileTemplates.AddRange(filesByGroup);
            }
            return Unit.Default;
        });

        CreateNewGroupCommand = ReactiveCommand.CreateFromTask(async () => { });

        MoveTemplateToGroupCommand =
            ReactiveCommand.CreateFromTask<(FileGroupViewModel Group, FileTemplateViewModel File), Unit>(async args =>
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
                Arguments = $"/select,\"{file.FilePath}\"",
                UseShellExecute = true
            };

            Process.Start(processInfo);
        });

        LoadLastFilesCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            LoadLastFiles();
        });

        OpenFileCommand = ReactiveCommand.CreateFromTask(async (FileTemplateViewModel file) =>
        {
            await Router.Navigate.Execute(new WordManagerViewModel(this, file));
        });

        RemoveFileTemplateFromGroupsCommand = ReactiveCommand.CreateFromTask(async (FileTemplateViewModel  file) =>
        {
            try
            {
                await file.MoveToGroup(FileGroupViewModel.GetBaseGroup());
                LoadLastFiles();
                LoadFileGroups();
            }
            catch (Exception)
            {
                Debug.WriteLine("Error while move file to group");
            }

            return Unit.Default;
        });

        OpenTemplateCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            // TODO: сделать проверку а есть ли теги в файле и если нет то выслать ошибку
            var fileInfo = await OpenTemplateInteraction.Handle(Unit.Default);
            if(fileInfo == null)
                return;
            var newFilePath = Path.Combine(FileTemplatesAppDirectoryService.TemplatesPath,
                DateTime.Now.ToString("M-d-yy HH-mm"));
            fileInfo.CopyTo(newFilePath);
            var newFileTemp = new FileTemplate
            {
                FilePath = newFilePath,
                FileGroup = FileTempGroup.BaseGroup,
                LastChange = DateTime.Now,
                Filename = fileInfo.Name,
                IsFavorite = false
            };
            switch (fileInfo.Extension)
            {
                case ".docx":
                    newFileTemp.FileType = FileType.Word;
                    break;
                case ".md":
                    newFileTemp.FileType = FileType.MarkDown;
                    break;
                default:
                    newFileTemp.FileType = FileType.Word;
                    break;
            }
            
            LastFilesService.SetFile(newFileTemp);
            await LastFilesService.SaveAsync();
            OpenFileCommand.Execute(new FileTemplateViewModel(newFileTemp));
        });
    }

    public ObservableCollection<FileTemplateViewModel> FileTemplates { get; } = new();

    public ObservableCollection<FileGroupViewModel> Groups { get; } = new();

    public ICommand AllGroupsCommand { get; }

    public ReactiveCommand<FileGroupViewModel, Unit> ConcreteGroupCommand { get; }

    public ICommand MoveTemplateToGroupCommand { get; }

    public ICommand CreateNewGroupCommand { get; }

    public ICommand RemoveFileTemplateCommand { get; }
    public ICommand RemoveFileTemplateFromGroupsCommand { get; }
    public ICommand OpenInExplorerCommand { get; }
    public ICommand OpenFileCommand { get; }
    public ICommand OpenTemplateCommand { get; }

    public ICommand LoadLastFilesCommand { get; }
    public RoutingState Router { get; } = new();
    public IInteraction<Unit, FileInfo?> OpenTemplateInteraction { get; } = new Interaction<Unit, FileInfo?>();

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
        foreach (var item in vms) FileTemplates.Add(item);
        _fileTemplates.AddRange(FileTemplates);
    }

    private async void LoadFileGroups()
    {
        Groups.Clear();
        var vms = FileGroupViewModel.GetGroups();
        // Groups.Add(FileGroupViewModel.GetThumbnailAllFileGroup());
        foreach (var item in vms) Groups.Add(item);
    }
}
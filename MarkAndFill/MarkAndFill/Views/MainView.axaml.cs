using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MarkAndFill.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace MarkAndFill.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
        this.WhenActivated(action => { ViewModel!.Activate(); });
        this.WhenActivated(action =>
        {
            this.ViewModel!.OpenTemplateInteraction.RegisterHandler(OpenFileTemplate);
        });
    }

    private async Task OpenFileTemplate(IInteractionContext<Unit, FileInfo?> context)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null)
        {
            context.SetOutput(null);
            return;
        }
        var filePickerOptions = new FilePickerOpenOptions
        {
            AllowMultiple = false,
            FileTypeFilter = [new FilePickerFileType("Word") {Patterns = ["*.docx"] }, new FilePickerFileType("Markdown") {Patterns = ["*.md"]}]
        };
        var fileResult = await topLevel.StorageProvider.OpenFilePickerAsync(filePickerOptions);
        if(!fileResult.Any())
        {
            context.SetOutput(null);
            return;
        }
        var fileInfo = new FileInfo(fileResult[0].Path.AbsolutePath);
        context.SetOutput(fileInfo);
    }

    private void MainGrid_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if(sender is not Grid grid)
            return;
        if (grid.ColumnDefinitions.Count == 0)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(20)));
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(2, GridUnitType.Star)));
        }

        if (e.NewSize.Width >= 1450)
        {
            grid.ColumnDefinitions[2].Width = new GridLength(3, GridUnitType.Star);
        }
        else
        {
            grid.ColumnDefinitions[2].Width = new GridLength(2, GridUnitType.Star);
        }

    }
}
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MarkAndFill.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace MarkAndFill.Views.ManagersPages;

public partial class WordManagerView : ReactiveUserControl<WordManagerViewModel>
{
    public WordManagerView()
    {
        InitializeComponent();
        this.WhenActivated(action =>
        {
            ViewModel!.ReplaceMarksInteraction.RegisterHandler(ReplaceMarksInteractionHandler);
        });
    }

    private async Task ReplaceMarksInteractionHandler(IInteractionContext<Unit, FileInfo?> context)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null)
        {
            context.SetOutput(null);
            return;
        }
        var filePickerOptions = new FilePickerSaveOptions()
        {
           DefaultExtension = ".docx",
           FileTypeChoices = [new FilePickerFileType("Word") { Patterns = ["*.docx"]}, new FilePickerFileType("Markdown") {Patterns = ["*.md"]}],
           Title = "Путь к сохранению"
        };
        var fileResult = await topLevel.StorageProvider.SaveFilePickerAsync(filePickerOptions);
        if(fileResult == null)
        {
            context.SetOutput(null);
            return;
        }
        var fileInfo = new FileInfo(fileResult.Path.AbsolutePath);
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
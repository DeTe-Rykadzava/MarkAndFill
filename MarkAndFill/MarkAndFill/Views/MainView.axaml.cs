using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MarkAndFill.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;

namespace MarkAndFill.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
        this.WhenActivated(action =>
        {
            ViewModel!.Activate();
        });
    }

    // private async Task DoShowFilePicker(IInteractionContext<Unit, Uri?> context)
    // {
    //     // Get top level from the current control. Alternatively, you can use Window reference instead.
    //     var topLevel = TopLevel.GetTopLevel(this);
    //
    //     // Start async operation to open the dialog.
    //     var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
    //     {
    //         Title = "Open Text File",
    //         FileTypeFilter = new List<FilePickerFileType> { new FilePickerFileType("Word") { Patterns = new List<string> { "*.docx" } } },
    //         AllowMultiple = false
    //     });
    //
    //     if (files.Count >= 1)
    //     {
    //         var file = files[0];
    //         var path = file.Path;
    //         context.SetOutput(path);
    //         return;
    //     }
    //     context.SetOutput(null);
    // }
}
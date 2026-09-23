using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MarkAndFill.ViewModels;
using ReactiveUI.Avalonia;

namespace MarkAndFill.Views.ManagersPages;

public partial class WordManagerView : ReactiveUserControl<WordManagerViewModel>
{
    public WordManagerView()
    {
        InitializeComponent();
    }
}
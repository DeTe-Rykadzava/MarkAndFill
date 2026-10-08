using MarkAndFill.ViewModels;
using ReactiveUI.Avalonia;

namespace MarkAndFill.Views;

public partial class MainWindow : ReactiveWindow<MainViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}
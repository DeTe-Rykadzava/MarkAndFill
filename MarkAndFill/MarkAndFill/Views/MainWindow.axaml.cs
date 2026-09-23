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

public partial class MainWindow : ReactiveWindow<MainViewModel>
{
    public MainWindow()
    {
        InitializeComponent();

    }
}
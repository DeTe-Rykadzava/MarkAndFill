using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MarkAndFill.Base;
using MarkAndFill.Base.Services;
using MarkAndFill.ViewModels;
using MarkAndFill.Views;
using MarkAndFill.Views.ManagersPages;
using ReactiveUI;
using Splat;

namespace MarkAndFill;

public class App : Application
{
    public App()
    {
        Locator.CurrentMutable.Register(() => new WordManagerView(), typeof(IViewFor<WordManagerViewModel>));
        LastFilesService.InitAsync().GetAwaiter().GetResult();
        FileGroupService.InitAsync().GetAwaiter().GetResult();
        FileTemplatesAppDirectoryService.Init();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
#if DEBUG
        this.AttachDeveloperTools();
#endif
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        //else if (ApplicationLifetime is IActivityApplicationLifetime singleViewFactoryApplicationLifetime)
        //{
        //    singleViewFactoryApplicationLifetime.MainViewFactory = () => new PageNavigationHost()
        //    {
        //        Page = new MainView { DataContext = new MainViewModel() }
        //    };
        //}
        //else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        //{
        //    singleViewPlatform.MainView = new PageNavigationHost()
        //    {
        //        Page = new MainView { DataContext = new MainViewModel() }
        //    };
        //}

        base.OnFrameworkInitializationCompleted();
    }
}
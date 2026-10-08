using MarkAndFill.Model;
using ReactiveUI;

namespace MarkAndFill.ViewModels;

public class MarkViewModel : ViewModelBase
{
    private readonly Mark _mark;

    private int _count;

    private string _tagName;

    private string? _tagValue = string.Empty;

    public MarkViewModel(Mark mark)
    {
        _mark = mark;
        _tagName = _mark.TagName;
    }

    public MarkViewModel(Mark mark, int count)
    {
        _mark = mark;
        _tagName = _mark.TagName;
        _count = count;
    }

    public string TagName
    {
        get => _tagName;
        private set => this.RaiseAndSetIfChanged(ref _tagName, value);
    }

    public string DisplayName => _tagName?.Trim('{', '}') ?? string.Empty;

    public string? TagValue
    {
        get => _tagValue;
        set => this.RaiseAndSetIfChanged(ref _tagValue, value);
    }

    public int Count
    {
        get => _count;
        set => this.RaiseAndSetIfChanged(ref _count, value);
    }
}
using MarkAndFill.Model;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarkAndFill.ViewModels
{
    public class MarkViewModel : ViewModelBase
    {
        private readonly Mark _mark;

        private string _tagName;

        public string TagName
        {
            get { return _tagName; }
            private set
            {
                this.RaiseAndSetIfChanged(ref _tagName, value);
            }
        }

        private string? _tagValue = string.Empty;

        public string? TagValue
        {
            get { return _tagValue; }
            set
            {
                this.RaiseAndSetIfChanged(ref _tagValue, value);
            }
        }

        private int _count = 0;

        public int Count
        {
            get { return _count; }
            set
            {
                this.RaiseAndSetIfChanged(ref _count, value);
            }
        }

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
    }
}

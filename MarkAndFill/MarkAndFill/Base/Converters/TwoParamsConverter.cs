using Avalonia.Data.Converters;
using MarkAndFill.ViewModels.ModelViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MarkAndFill.Base.Converters
{
    public class TwoParamsConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            return (values[0] as FileGroupViewModel, values[1] as FileTemplateViewModel); // кортеж (ViewModel, ViewModel)
        }

        public object ConvertBack(IList<object> values, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}

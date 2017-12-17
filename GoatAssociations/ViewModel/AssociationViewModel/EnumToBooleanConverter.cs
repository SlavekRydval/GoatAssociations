using System;
using System.Windows.Data;

namespace GoatAssociations.ViewModel.AssociationViewModel
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        => value.Equals(parameter);

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
                => value.Equals(true) ? parameter : Binding.DoNothing;
    }
}

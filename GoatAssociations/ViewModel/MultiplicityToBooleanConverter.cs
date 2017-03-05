using System;
using System.Windows.Data;

namespace GoatAssociations.ViewModel
{
    class MultiplicityToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            bool result = value.Equals(parameter);
            return result ? result : "custom".Equals(parameter) && !value.Equals("0..1") && 
                                     !value.Equals("0..*") && !value.Equals("1..*") && 
                                     !value.Equals("1") && !value.Equals("*") && 
                                     !value.Equals(""); 
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
            //    => value.Equals(true) ? parameter : Binding.DoNothing;
    }
}

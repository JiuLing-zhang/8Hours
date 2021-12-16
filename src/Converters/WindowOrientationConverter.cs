using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using _8Hours.Enums;

namespace _8Hours.Converters
{
    public class WindowOrientationConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return false;
            }
            if (value.ToString() != parameter.ToString())
            {
                return false;
            }

            return true;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null || (bool)value == false || parameter == null)
            {
                return null;
            }
            if (!Enum.TryParse(parameter.ToString(), out WindowOrientationEnum orientation))
            {
                return null;
            }
            return orientation;
        }
    }
}

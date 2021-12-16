using System;
using System.Globalization;
using System.Windows.Data;
using _8Hours.Enums;
using JiuLing.CommonLibs.ExtensionMethods;

namespace _8Hours.Converters
{
    internal class DefaultJobTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                return false;
            }

            string uiParameter = parameter.ToString();
            if (value == null)
            {
                if (uiParameter.IsEmpty())
                {
                    return true;
                }
                return false;
            }

            if (value.ToString() != uiParameter)
            {
                return false;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (bool)value == false || parameter == null)
            {
                return null;
            }

            string uiParameter = parameter.ToString();
            if (uiParameter.IsEmpty())
            {
                return null;
            }
            if (!Enum.TryParse(uiParameter, out JobTypeEnum jobType))
            {
                return null;
            }
            return jobType;
        }
    }
}

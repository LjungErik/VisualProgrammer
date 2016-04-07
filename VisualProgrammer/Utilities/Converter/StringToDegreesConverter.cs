using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VisualProgrammer.Utilities.Converter
{
    public class StringToDegreesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string)
            {
                string degrees = (string)value;
                degrees += "°";
                return degrees;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
 	        if(value is string)
            {
                string degrees = (string)value;
                if(degrees.Length >= 2)
                {
                    return degrees.Substring(0, degrees.Length-1);
                }
            }
            return "";
        }
    }
}

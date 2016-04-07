using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VisualProgrammer.Utilities.Converter
{
    public class StringToServoNumberConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is string)
            {
                string number = (string)value;
                return "#" + number;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is string)
            {
                string servoNumber = (string)value;
                if(servoNumber.Length >= 2)
                {
                    return servoNumber.Substring(1);
                }
            }
            return "";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using VisualProgrammer.Enums;

namespace VisualProgrammer.Utilities.Converter
{
    public class WriteTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is WriteType)
            {
                WriteType type = (WriteType)value;
                switch (type)
                {
                    case WriteType.Info:
                        return Brushes.White;
                    case WriteType.Warning:
                        return Brushes.Yellow;
                    case WriteType.Error:
                        return Brushes.Red;
                }
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is SolidColorBrush)
            {
                SolidColorBrush color = (SolidColorBrush)value;
                if(color != null)
                {
                    if(color == Brushes.Yellow)
                            return WriteType.Warning;
                    else if(color == Brushes.Red)
                            return WriteType.Error;
                }
            }

            //Default is info
            return WriteType.Info;
        }
    }
}

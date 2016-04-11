using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VisualProgrammer.Enums;

namespace VisualProgrammer.Utilities.Converter
{
    public class StatusTypeToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is StatusType)
            {
                StatusType status = (StatusType)value;
                switch (status)
                {
                    case StatusType.Processing:
                        return false;
                    default:
                        return true;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

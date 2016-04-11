using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VisualProgrammer.Enums;

namespace VisualProgrammer.Utilities.Converter
{
    public class StatusTypeToStatusStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is StatusType)
            {
                StatusType status = (StatusType)value;
                switch (status)
                {
                    case StatusType.Canceled:
                        return "Canceled";
                    case StatusType.Error:
                        return "Error, compilation stopped";
                    case StatusType.Processing:
                        return "Compiling...";
                    case StatusType.Finished:
                        return "Completed";
                    case StatusType.Stopped:
                        return "Stopped";
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

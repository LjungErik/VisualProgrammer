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
    public class StatusTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is StatusType)
            {
                StatusType status = (StatusType)value;
                switch (status)
                {
                    case StatusType.Processing:
                    case StatusType.Finished:
                        return Brushes.LimeGreen;
                    case StatusType.Stopped:
                        return Brushes.Yellow;
                    case StatusType.Canceled:
                        return Brushes.Gray;
                    case StatusType.Error:
                        return Brushes.Red;
                }
            }
            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

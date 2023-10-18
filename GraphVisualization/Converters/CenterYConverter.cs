using System.Globalization;
using System.Windows.Data;
using System;

namespace GraphVisualization.Converters;

public class CenterYConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        int y = (int)values[0];
        int nodeRadius = (int)values[1];
        return (double)(y - nodeRadius / 2);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
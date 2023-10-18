using System.Globalization;
using System.Windows.Data;
using System;

namespace GraphVisualization.Converters;

public class CenterXConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        int x = (int)values[0];
        int nodeRadius = (int)values[1];
        return (double)(x - nodeRadius / 2);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
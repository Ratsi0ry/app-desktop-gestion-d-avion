using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Gestion_avion.Converters;

public class BoolToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool estReserve && estReserve)
        {
            return Brushes.LightGray; 
        }
        
        return Brushes.White; 
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
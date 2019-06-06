using System;
using System.Globalization;
using System.Windows.Data;

namespace SongsParser.Converters
{
    public class ImageConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string url)
            {
                return new Uri(url);
            }

            return "https://assets.billboard.com/assets/1559241114/images/charts/bb-placeholder-new.jpg?2a81dc25704f8e7feec7";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
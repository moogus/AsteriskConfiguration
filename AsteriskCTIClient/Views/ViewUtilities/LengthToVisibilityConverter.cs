using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AsteriskCTIClient.Views.ViewUtilities
{
  public sealed class LengthToVisibilityConverter : IValueConverter
  {
    public bool UseHidden { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      int val = System.Convert.ToInt32(value, CultureInfo.InvariantCulture);

      if (val == 0)
      {
        return Visibility.Visible;
      }
      return UseHidden ? Visibility.Hidden : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
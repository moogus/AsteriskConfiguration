using System;
using System.Globalization;
using System.Windows.Data;

namespace AsteriskCTIClient.Views.ViewUtilities
{
  public sealed class BoolToHeightConverter : IValueConverter
  {
    public bool UseHidden { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      bool val = System.Convert.ToBoolean(value, CultureInfo.InvariantCulture);

      return val ? "0,2,0,0" : "0";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
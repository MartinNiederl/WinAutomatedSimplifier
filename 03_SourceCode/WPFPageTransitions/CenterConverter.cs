using System;
using System.Windows.Data;

namespace WPFPageTransitions
{
	public class CenterConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => (double)value / 2;

	    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => throw new NotImplementedException();
	}
}

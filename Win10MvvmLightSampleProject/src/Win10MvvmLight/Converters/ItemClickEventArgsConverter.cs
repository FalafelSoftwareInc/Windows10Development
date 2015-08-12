using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win10MvvmLight.Portable.Model;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Win10MvvmLight.Converters
{
	public sealed class ItemClickEventArgsConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var args = value as ItemClickEventArgs;
			if (args == null)
				throw new ArgumentException("Value is not ItemClickEventArgs");
			if (args.ClickedItem is TestItem)
			{
				var selectedItem = args.ClickedItem as TestItem;
				return selectedItem;
			}
			else
				return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}

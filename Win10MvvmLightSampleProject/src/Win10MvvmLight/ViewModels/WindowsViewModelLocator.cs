using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win10MvvmLight.Portable.ViewModels;
using Win10MvvmLight.Views;

namespace Win10MvvmLight.ViewModels
{
	public class WindowsViewModelLocator : BaseViewModelLocator
	{
		public WindowsViewModelLocator() : base()
		{
			if (ViewModelBase.IsInDesignModeStatic)
			{
				SimpleIoc.Default.Register<INavigationService, NavigationService>();
			}
			else
			{
				var navigationService = InitNavigationService();
				SimpleIoc.Default.Register<INavigationService>(() => navigationService);
			}
		}

		protected INavigationService InitNavigationService()
		{
			var service = new NavigationService();

			service.Configure(typeof(MainPageViewModel).FullName, typeof(MainPage));
			service.Configure(typeof(SecondPageViewModel).FullName, typeof(SecondPage));

			return service;
		}
	}
}

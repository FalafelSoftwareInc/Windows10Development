using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win10MvvmLight.Portable.ViewModels
{
	public class BaseViewModelLocator
	{
		public BaseViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			SimpleIoc.Default.Register<MainPageViewModel>();
			SimpleIoc.Default.Register<SecondPageViewModel>();
		}

		public MainPageViewModel MainPage
		{
			get { return ServiceLocator.Current.GetInstance<MainPageViewModel>(); }
		}

		public SecondPageViewModel SecondPage
		{
			 get { return ServiceLocator.Current.GetInstance<SecondPageViewModel>(); }
		}
	}
}

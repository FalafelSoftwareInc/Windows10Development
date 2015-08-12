using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win10MvvmLight.Portable.ViewModels
{
	public class BaseViewModel : ViewModelBase
	{
        protected INavigationService NavigationService { get { return ServiceLocator.Current.GetInstance<INavigationService>(); } }

        public BaseViewModel()
		{
			if (this.IsInDesignMode)
			{
				LoadDesignTimeData();
			}
		}

		private bool isLoading;
		public virtual bool IsLoading
		{
			get { return isLoading; }
			set
			{
				isLoading = value;
				RaisePropertyChanged();
			}
		}

        public void Navigate<T>(object argument = null)
        {
            if (argument == null)
                NavigationService.NavigateTo(typeof(T).FullName);
            else
                NavigationService.NavigateTo(typeof(T).FullName, argument);
        }

        #region State Management

        public virtual void LoadState(object navParameter, Dictionary<string, object> state) { }

		public virtual void SaveState(Dictionary<string, object> state) { }

		protected virtual T RestoreStateItem<T>(Dictionary<string, object> state, string stateKey, T defaultValue = default(T))
		{
			return state != null && state.ContainsKey(stateKey) && state[stateKey] != null && state[stateKey] is T ? (T)state[stateKey] : defaultValue;
		}

		#endregion

		protected virtual void LoadDesignTimeData() { }
	}
}

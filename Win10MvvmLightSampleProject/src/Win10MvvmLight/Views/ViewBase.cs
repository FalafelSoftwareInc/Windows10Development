using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win10MvvmLight.Portable.ViewModels;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Win10MvvmLight.Views
{
	public class ViewBase : Page
	{
		private BaseViewModel PageViewModel
		{
			get { return this.DataContext as BaseViewModel; }
		}

		private String _pageKey;
        SystemNavigationManager currentView;

        public ViewBase()
		{
			this.LoadState += ViewBase_LoadState;
			this.SaveState += ViewBase_SaveState;
		}

		void ViewBase_SaveState(object sender, SaveStateEventArgs e)
		{
			if (PageViewModel != null) PageViewModel.SaveState(e.PageState);
		}

		void ViewBase_LoadState(object sender, LoadStateEventArgs e)
		{
			if (PageViewModel != null)
			{
				var view = this.GetType().Name;

				PageViewModel.LoadState(e.NavigationParameter, e.PageState);
			}
		}

		/// <summary>
		/// Register this event on the current page to populate the page
		/// with content passed during navigation as well as any saved
		/// state provided when recreating a page from a prior session.
		/// </summary>
		public event LoadStateEventHandler LoadState;

		/// <summary>
		/// Register this event on the current page to preserve
		/// state associated with the current page in case the
		/// application is suspended or the page is discarded from
		/// the navigaqtion cache.
		/// </summary>
		public event SaveStateEventHandler SaveState;

		protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var frameState = SuspensionManager.SessionStateForFrame(this.Frame);
			this._pageKey = "Page-" + this.Frame.BackStackDepth;

			if (e.NavigationMode == NavigationMode.New)
			{
				// Clear existing state for forward navigation when adding a new page to the
				// navigation stack
				var nextPageKey = this._pageKey;
				int nextPageIndex = this.Frame.BackStackDepth;
				while (frameState.Remove(nextPageKey))
				{
					nextPageIndex++;
					nextPageKey = "Page-" + nextPageIndex;
				}

				// Pass the navigation parameter to the new page
				if (this.LoadState != null)
				{
					this.LoadState(this, new LoadStateEventArgs(e.Parameter, null));
				}
			}
			else
			{
				// Pass the navigation parameter and preserved page state to the page, using
				// the same strategy for loading suspended state and recreating pages discarded
				// from cache
				if (this.LoadState != null)
				{
					this.LoadState(this, new LoadStateEventArgs(e.Parameter, (Dictionary<String, Object>)frameState[this._pageKey]));
				}
			}


            currentView = SystemNavigationManager.GetForCurrentView();

            if (!ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                currentView.AppViewBackButtonVisibility = this.Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            }

            
            currentView.BackRequested += SystemNavigationManager_BackRequested;
        }

        private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                e.Handled = true;
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);

			var frameState = SuspensionManager.SessionStateForFrame(this.Frame);
			var pageState = new Dictionary<String, Object>();
			if (this.SaveState != null)
			{
				this.SaveState(this, new SaveStateEventArgs(pageState));
			}
			frameState[_pageKey] = pageState;

            currentView.BackRequested -= SystemNavigationManager_BackRequested;
        }
	}

	/// <summary>
	/// Represents the method that will handle the <see cref="NavigationHelper.LoadState"/>event
	/// </summary>
	public delegate void LoadStateEventHandler(object sender, LoadStateEventArgs e);
	/// <summary>
	/// Represents the method that will handle the <see cref="NavigationHelper.SaveState"/>event
	/// </summary>
	public delegate void SaveStateEventHandler(object sender, SaveStateEventArgs e);

	/// <summary>
	/// Class used to hold the event data required when a page attempts to load state.
	/// </summary>
	public class LoadStateEventArgs : EventArgs
	{
		/// <summary>
		/// The parameter value passed to <see cref="Frame.Navigate(Type, Object)"/> 
		/// when this page was initially requested.
		/// </summary>
		public Object NavigationParameter { get; private set; }
		/// <summary>
		/// A dictionary of state preserved by this page during an earlier
		/// session.  This will be null the first time a page is visited.
		/// </summary>
		public Dictionary<string, Object> PageState { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="LoadStateEventArgs"/> class.
		/// </summary>
		/// <param name="navigationParameter">
		/// The parameter value passed to <see cref="Frame.Navigate(Type, Object)"/> 
		/// when this page was initially requested.
		/// </param>
		/// <param name="pageState">
		/// A dictionary of state preserved by this page during an earlier
		/// session.  This will be null the first time a page is visited.
		/// </param>
		public LoadStateEventArgs(Object navigationParameter, Dictionary<string, Object> pageState)
			: base()
		{
			this.NavigationParameter = navigationParameter;
			this.PageState = pageState;
		}
	}
	/// <summary>
	/// Class used to hold the event data required when a page attempts to save state.
	/// </summary>
	public class SaveStateEventArgs : EventArgs
	{
		/// <summary>
		/// An empty dictionary to be populated with serializable state.
		/// </summary>
		public Dictionary<string, Object> PageState { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SaveStateEventArgs"/> class.
		/// </summary>
		/// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
		public SaveStateEventArgs(Dictionary<string, Object> pageState)
			: base()
		{
			this.PageState = pageState;
		}
	}
}

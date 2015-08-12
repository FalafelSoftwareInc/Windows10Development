using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win10MvvmLight.Portable.Model;

namespace Win10MvvmLight.Portable.ViewModels
{
	public class MainPageViewModel : BaseViewModel
	{
		private ObservableCollection<TestItem> testItems = new ObservableCollection<TestItem>();
		public ObservableCollection<TestItem> TestItems
		{
			get { return testItems; }
			set
			{
				testItems = value;
				RaisePropertyChanged();
			}
		}

        #region Commands

        private RelayCommand<TestItem> selectItemCommand;
        public RelayCommand<TestItem> SelectItemCommand
        {
            get
            {
                return selectItemCommand ?? (selectItemCommand = new RelayCommand<TestItem>((selectedItem) =>
                {
                    if (selectedItem == null) return;

                    Navigate<SecondPageViewModel>(selectedItem.Id);
                }));
            }
        }

        #endregion

        #region State Management

        public override void LoadState(object navParameter, Dictionary<string, object> state)
		{
			base.LoadState(navParameter, state);

			if (!TestItems.Any())
			{
				for (var i = 1; i <= 5; i++)
				{
					var color = string.Join("", Enumerable.Repeat(i.ToString(), 6));
					var testItem = new TestItem() { Id = i, Title = "Runtime Item " + i, Subtitle = "Subtitle " + i, HexColor = string.Concat("#", color) };
					TestItems.Add(testItem);
				}
			}
		}

		#endregion

		protected override void LoadDesignTimeData()
		{
			base.LoadDesignTimeData();

			for (var i = 1; i < 10; i++)
			{
				var color = string.Join("", Enumerable.Repeat(i.ToString(), 6));
				var testItem = new TestItem() { Id = i, Title = "Test Item " + i, Subtitle = "Subtitle " + i, HexColor = string.Concat("#", color) };
                TestItems.Add(testItem);
			}
		}
	}
}

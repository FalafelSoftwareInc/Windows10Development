using System.Collections.Generic;
using System.Linq;
using Win10MvvmLight.Portable.Model;

namespace Win10MvvmLight.Portable.ViewModels
{
	public class SecondPageViewModel : BaseViewModel
	{
		private TestItem selectedItem;
		public TestItem SelectedItem
		{
			get { return selectedItem; }
			set
			{
				selectedItem = value;
				RaisePropertyChanged();
			}
		}

		private string stateText;
		public string StateText
		{
			get { return stateText; }
			set
			{
				stateText = value;
				RaisePropertyChanged();
			}
		}

		#region State Management

		public override void LoadState(object navParameter, Dictionary<string, object> state)
		{
			base.LoadState(navParameter, state);

			// load test items again; in production this would retrieve the live item by id or get it from a local data cache
			var items = GetFakeRuntimeItems();

			SelectedItem = items.FirstOrDefault(i => i.Id == (int)navParameter);

			if (state != null)
			{
				StateText = this.RestoreStateItem<string>(state, "STATETEXT");
			}
		}

		public override void SaveState(Dictionary<string, object> state)
		{
			base.SaveState(state);

			state["STATETEXT"] = StateText;
		}

		#endregion

		private List<TestItem> GetFakeRuntimeItems()
		{
			var items = new List<TestItem>();
			for (var i = 1; i <= 5; i++)
			{
				var color = string.Join("", Enumerable.Repeat(i.ToString(), 6));
				var testItem = new TestItem() { Id = i, Title = "Runtime Item " + i, Subtitle = "Subtitle " + i, HexColor = string.Concat("#", color) };
				items.Add(testItem);
			}

			return items;
		}

        protected override void LoadDesignTimeData()
		{
			base.LoadDesignTimeData();

			SelectedItem = new TestItem() { Title = "Design Time Selected Item", Subtitle = "Design subtitle", HexColor = "#333333" };
		}
	}
}

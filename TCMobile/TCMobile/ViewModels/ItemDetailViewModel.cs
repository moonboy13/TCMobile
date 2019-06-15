using System;

using TCMobile.Models;

namespace TCMobile.ViewModels
{
	public class ItemDetailViewModel : BaseViewModel<Item>
	{
		public Item Item { get; set; }
		public ItemDetailViewModel(Item item = null)
		{
			Title = item?.Text;
			Item = item;
		}
	}
}

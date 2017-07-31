using Cone;
using Cone.Helpers;
using System;

namespace PointOfSale
{
	[Feature("Sell one item")]
    public class SellOneItemFeature
    {
		class ItemAddedEventArgs : EventArgs
		{
			public readonly string Barcode;
			public readonly string ItemPrice;

			public ItemAddedEventArgs(string barcode, string itemPrice) { 
				this.Barcode = barcode;
				this.ItemPrice = itemPrice;
			}
		}

		class PosTerminal
		{
			public event EventHandler<ItemAddedEventArgs> ItemAdded;

			public void ProcessBarcode(string barcode) =>
				ItemAdded?.Invoke(this, new ItemAddedEventArgs(barcode, "$11.50"));
		}

		const string ExistingBarcode = "123456789";
		const string ExpectedPrice = "$11.50";

		public void scan_a_code_show_the_price() {
			var pos = new PosTerminal();
			var itemAdded = new EventSpy<ItemAddedEventArgs>((_, e) => 
				Check.That(
					() => e.Barcode == ExistingBarcode,
					() => e.ItemPrice == ExpectedPrice));

			pos.ItemAdded += itemAdded;
			pos.ProcessBarcode(ExistingBarcode);

			Check.That(() => itemAdded.HasBeenCalled);
		}
    }
}

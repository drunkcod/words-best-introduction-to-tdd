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

		class PriceRequiredEventArgs : EventArgs 
		{
			public readonly string Barcode;
			public string ItemPrice;

			public PriceRequiredEventArgs(string barcode) {
				this.Barcode = barcode;
			}
		}

		class PosTerminal
		{
			public event EventHandler<ItemAddedEventArgs> ItemAdded;
			public event EventHandler<PriceRequiredEventArgs> PriceRequired;

			public void ProcessBarcode(string barcode) {
				var priceCheck = new PriceRequiredEventArgs(barcode);
				PriceRequired?.Invoke(this, priceCheck);
				ItemAdded?.Invoke(this, new ItemAddedEventArgs(priceCheck.Barcode, priceCheck.ItemPrice));
			}
		}

		const string ExistingBarcode = "123456789";
		const string ExpectedPrice = "$11.50";

		public void scan_a_code_show_the_price() {
			var pos = new PosTerminal();
			var itemAdded = new EventSpy<ItemAddedEventArgs>((_, e) => 
				Check.That(
					() => e.Barcode == ExistingBarcode,
					() => e.ItemPrice == ExpectedPrice));

			pos.PriceRequired += (_, e) => e.ItemPrice = ExpectedPrice;
			pos.ItemAdded += itemAdded;
			pos.ProcessBarcode(ExistingBarcode);

			Check.That(() => itemAdded.HasBeenCalled);
		}

		public void does_price_lookup_on_scanned_item() { 
			var pos = new PosTerminal();

			var priceRequired = new EventHandler<PriceRequiredEventArgs>((_, e) => {
				switch(e.Barcode) { 
					case "12345": e.ItemPrice = "$12.34"; break;
					case "67890": e.ItemPrice = $"67.89"; break;
				}
			});
			pos.PriceRequired += priceRequired;
			pos.ItemAdded += (_, e) => {
				var priceCheck = new PriceRequiredEventArgs(e.Barcode);
				priceRequired(null, priceCheck);
				Check.That(() => e.ItemPrice == priceCheck.ItemPrice);
			};

			pos.ProcessBarcode("12345");
			pos.ProcessBarcode("67890");
		}
    }
}

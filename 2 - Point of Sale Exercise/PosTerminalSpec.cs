using Cone;
using Cone.Helpers;
using System;

namespace PointOfSale
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
		public event EventHandler<ItemAddedEventArgs> MissingItem;
		public event EventHandler<PriceRequiredEventArgs> PriceRequired;

		public void ProcessBarcode(string barcode) {
			var priceCheck = new PriceRequiredEventArgs(barcode);
			PriceRequired?.Invoke(this, priceCheck);
			var e = new ItemAddedEventArgs(priceCheck.Barcode, priceCheck.ItemPrice);
			(string.IsNullOrEmpty(e.ItemPrice) ? MissingItem : ItemAdded)?.Invoke(this, e);
		}
	}

	[Describe(typeof(PosTerminal))]
	public class PosTerminalSpec
	{
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

		public void signals_unknown_item() {
			var pos = new PosTerminal();
			var itemAdded = new EventSpy<ItemAddedEventArgs>();
			var itemMissing = new EventSpy<ItemAddedEventArgs>((_, e) => Check.That(() => e.Barcode == "No Such Item"));

			pos.ItemAdded += itemAdded;
			pos.MissingItem += itemMissing;

			pos.ProcessBarcode("No Such Item");

			Check.That(
				() => !itemAdded.HasBeenCalled,
				() => itemMissing.HasBeenCalled);
		}
	}
}

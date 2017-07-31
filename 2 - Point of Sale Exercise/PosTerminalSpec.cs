using Cone;
using Cone.Helpers;
using System;

namespace PointOfSale
{
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

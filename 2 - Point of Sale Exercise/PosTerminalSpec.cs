using Cone;
using Cone.Helpers;
using System;

namespace PointOfSale
{
	[Describe(typeof(PosTerminal))]
	public class PosTerminalSpec
	{
		PosTerminal pos;
		EventSpy<ItemAddedEventArgs> itemAdded;

		[BeforeEach]
		public void given_a_pos_terminal() { 
			pos = new PosTerminal();
			itemAdded = new EventSpy<ItemAddedEventArgs>();
			
			pos.ItemAdded += itemAdded;
		}

		public void does_price_lookup_on_scanned_item() {
			var priceRequired = new EventHandler<PriceRequiredEventArgs>((_, e) => {
				switch(e.Barcode.ToString()) { 
					case "12345": e.ItemPrice = "$12.34"; break;
					case "67890": e.ItemPrice = $"67.89"; break;
				}
			});
			var priceSpy = new EventSpy<PriceRequiredEventArgs>(priceRequired);			
			pos.PriceRequired += priceSpy;

			pos.ProcessBarcode(new Barcode("12345"));
			pos.ProcessBarcode(new Barcode("67890"));

			Assume.That(() => priceSpy.HasBeenCalled);
			itemAdded.Then((_, e) => {
				var priceCheck = new PriceRequiredEventArgs(e.Barcode);
				priceRequired(null, priceCheck);
				Check.That(() => e.ItemPrice != priceCheck.ItemPrice);
			});
		}

		public void signals_unknown_item() {
			var itemMissing = new EventSpy<ItemAddedEventArgs>();
			pos.MissingItem += itemMissing;

			pos.ProcessBarcode(new Barcode("Unknown Item"));

			Assume.That(() => !itemAdded.HasBeenCalled);
			itemMissing.Then((_, e) => Check.That(() => e.Barcode.ToString() == "No Such Item"));
		}
	}
}

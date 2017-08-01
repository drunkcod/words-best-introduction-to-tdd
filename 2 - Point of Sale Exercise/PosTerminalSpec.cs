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
			var productOne = new Barcode("12345");
			var productTwo = new Barcode("67890");
			var prices = new PriceLookup {
				{productOne, new Price(12.34m)},
				{productTwo, new Price(67.89m)},
			};

			prices.ConnectTo(pos);			
			var priceSpy = new EventSpy<PriceRequiredEventArgs>((_, e) => Check.That(() => e.ItemPrice.Value == prices[e.Barcode]));			
			pos.PriceRequired += priceSpy;

			pos.ProcessBarcode(productOne);
			pos.ProcessBarcode(productTwo);

			Assume.That(() => priceSpy.HasBeenCalled);
		}

		public void signals_unknown_item() {
			var itemMissing = new EventSpy<ItemAddedEventArgs>();
			pos.MissingItem += itemMissing;

			pos.ProcessBarcode(new Barcode("Unknown Item"));

			Assume.That(() => !itemAdded.HasBeenCalled);
			itemMissing.Then((_, e) => Check.That(() => e.Barcode.ToString() == "No Such Item"));
		}

		public void null_barcode() =>
			Check.Exception<ArgumentNullException>(() => pos.ProcessBarcode(null));
	}
}

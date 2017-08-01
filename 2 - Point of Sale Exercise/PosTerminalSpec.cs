using Cone;
using Cone.Helpers;
using System;
using System.Collections.Generic;

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

		class PriceLookup
		{
			readonly Dictionary<string, string> barcodeToPrice = new Dictionary<string, string>();

			public string this[Barcode barcode] => barcodeToPrice[barcode.ToString()];

			public void Add(Barcode item, string price) =>
				barcodeToPrice.Add(item.ToString(), price);

			public void ConnectTo(PosTerminal terminal) {
				terminal.PriceRequired += (_, e) => TryGetPrice(e.Barcode, out e.ItemPrice);
			}

			bool TryGetPrice(Barcode barcode, out string itemPrice) => 
				barcodeToPrice.TryGetValue(barcode.ToString(), out itemPrice);
		}

		public void does_price_lookup_on_scanned_item() {
			var prices = new PriceLookup();
			prices.Add(new Barcode("12345"), "$12.33");
			prices.Add(new Barcode("67890"), "$67.89");

			prices.ConnectTo(pos);
			var priceSpy = new EventSpy<PriceRequiredEventArgs>((_, e) => Check.That(() => e.ItemPrice == prices[e.Barcode]));			
			pos.PriceRequired += priceSpy;

			pos.ProcessBarcode(new Barcode("12345"));
			pos.ProcessBarcode(new Barcode("67890"));

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

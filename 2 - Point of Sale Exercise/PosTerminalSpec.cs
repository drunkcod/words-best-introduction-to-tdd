using Cone;
using Cone.Helpers;
using System;
using System.Collections.Generic;
using System.Collections;

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

		class PriceLookup : IEnumerable<KeyValuePair<Barcode, string>>
		{
			readonly Dictionary<string, string> barcodeToPrice = new Dictionary<string, string>();

			public string this[Barcode barcode] => barcodeToPrice[barcode.ToString()];

			public void Add(Barcode item, string price) =>
				barcodeToPrice.Add(item.ToString(), price);

			public void ConnectTo(PosTerminal terminal) {
				terminal.PriceRequired += (_, e) => TryGetPrice(e.Barcode, out e.ItemPrice);
			}

			public IEnumerator<KeyValuePair<Barcode, string>> GetEnumerator() {
				foreach(var item in barcodeToPrice)
					yield return new KeyValuePair<Barcode, string>(new Barcode(item.Key), item.Value);
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

			bool TryGetPrice(Barcode barcode, out string itemPrice) => 
				barcodeToPrice.TryGetValue(barcode.ToString(), out itemPrice);
		}

		public void does_price_lookup_on_scanned_item() {
			var productOne = new Barcode("12345");
			var productTwo = new Barcode("67890");
			var prices = new PriceLookup {
				{productOne, "$12.34"},
				{productTwo, "$67.89"},
			};

			prices.ConnectTo(pos);			
			var priceSpy = new EventSpy<PriceRequiredEventArgs>((_, e) => Check.That(() => e.ItemPrice == prices[e.Barcode]));			
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

using System.Collections.Generic;
using System.Collections;

namespace PointOfSale
{
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
}

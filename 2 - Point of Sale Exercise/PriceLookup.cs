using System.Collections.Generic;
using System.Collections;

namespace PointOfSale
{
	class PriceLookup : IEnumerable<KeyValuePair<Barcode, Price>>
	{
		readonly Dictionary<string, Price> barcodeToPrice = new Dictionary<string, Price>();

		public Price this[Barcode barcode] => barcodeToPrice[barcode.ToString()];

		public void Add(Barcode item, Price price) =>
			barcodeToPrice.Add(item.ToString(), price);

		public void ConnectTo(PosTerminal terminal) {
			terminal.PriceRequired += (_, e) => {
				if(TryGetPrice(e.Barcode, out Price foundPrice))
					e.ItemPrice = foundPrice;
			};
		}

		public IEnumerator<KeyValuePair<Barcode, Price>> GetEnumerator() {
			foreach(var item in barcodeToPrice)
				yield return new KeyValuePair<Barcode, Price>(new Barcode(item.Key), item.Value);
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		bool TryGetPrice(Barcode barcode, out Price itemPrice) => 
			barcodeToPrice.TryGetValue(barcode.ToString(), out itemPrice);
	}
}

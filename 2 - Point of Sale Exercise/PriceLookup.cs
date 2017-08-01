using System.Collections.Generic;
using System.Collections;

namespace PointOfSale
{
	class PriceLookup : IEnumerable<KeyValuePair<Barcode, Price>>
	{
		readonly Dictionary<Barcode, Price> barcodeToPrice = new Dictionary<Barcode, Price>();

		public Price this[Barcode barcode] => barcodeToPrice[barcode];

		public void Add(Barcode item, Price price) =>
			barcodeToPrice.Add(item, price);

		public void ConnectTo(PosTerminal terminal) {
			terminal.PriceRequired += (_, e) => {
				if(TryGetPrice(e.Barcode, out Price foundPrice))
					e.ItemPrice = foundPrice;
			};
		}

		public IEnumerator<KeyValuePair<Barcode, Price>> GetEnumerator() => 
			barcodeToPrice.GetEnumerator();
		
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		bool TryGetPrice(Barcode barcode, out Price itemPrice) => 
			barcodeToPrice.TryGetValue(barcode, out itemPrice);
	}
}

using Cone;
using Cone.Helpers;
using System;
using System.Text;

namespace PointOfSale
{
	[Feature("Sell one item")]
    public class SellOneItemFeature
    {
		const string ExistingBarcode = "123456789";
		const string ExpectedPrice = "$11.50";

		public void scan_a_code_show_the_price() {
			var pos = new PosTerminal();
			var result = new StringBuilder();

			pos.PriceRequired += (_, e) => e.ItemPrice = ExpectedPrice;
			pos.ItemAdded += (_, e) => result.Append(e.ItemPrice);
			pos.ProcessBarcode(ExistingBarcode);

			Check.That(() => result.ToString() == ExpectedPrice);
		}
    }
}

using Cone;
using Cone.Core;

namespace PointOfSale
{
	public class TerminalAndDisplayContext : ITestContext
	{
		Display display;
		PriceLookup prices;
		Sale sale;

		void ITestContext.Before() {
			var pos = new PosTerminal();

			display = new Display();
			display.ConnectTo(pos);

			prices = new PriceLookup();
			prices.ConnectTo(pos);

			sale = new Sale(pos, display);
		}

		void ITestContext.After(ITestResult result) {}

		public void AddPrice(string barcode, Price itemPrice) =>
			prices.Add(new Barcode(barcode), itemPrice);

		public void ProcessBarcode(string barcode) => sale.ProcessBarcode(barcode);
		public void PressTotal() => sale.PressTotal();

		public string DisplayText => display.Text;
	}
}

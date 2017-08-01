using Cone;

namespace PointOfSale
{
	class Display
	{
		public string Text;
	}

	[Feature("Sell one item")]
    public class SellOneItemFeature
    {
		const string ExistingBarcode = "123456789";
		const string ExpectedPrice = "$11.50";

		public void scan_a_code_show_the_price() {
			var pos = new PosTerminal();
			var display = new Display();

			pos.Connect(display);

			pos.PriceRequired += (_, e) => e.ItemPrice = ExpectedPrice;
			pos.ProcessBarcode(ExistingBarcode);

			Check.That(() => display.Text == ExpectedPrice);
		}

		public void display_missing_message_for_missing_product() {
			var pos = new PosTerminal();
			var display = new Display();

			pos.Connect(display);

			pos.ProcessBarcode("No Such Thing");

			Check.That(() => display.Text == "Missing Product <No Such Thing>");
		}
    }
}

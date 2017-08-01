using Cone;

namespace PointOfSale
{
	[Feature("Sell one item")]
    public class SellOneItemFeature
    {
		PosTerminal pos;
		Display display;

		[BeforeEach]
		public void given_a_terminal_with_attached_display() {
			pos = new PosTerminal();
			display = new Display();

			display.ConnectTo(pos);
		}

		public void scan_a_code_show_the_price() {
			var existingBarcode = new Barcode("123456789");
			var expectedPrice = "$11.50";

			pos.PriceRequired += (_, e) => e.ItemPrice = expectedPrice;
			pos.ProcessBarcode(existingBarcode);

			Check.That(() => display.Text == expectedPrice);
		}

		public void display_missing_message_for_missing_product() {
			pos.ProcessBarcode(new Barcode("No Such Thing"));

			Check.That(() => display.Text == "Missing Product <No Such Thing>");
		}
    }
}

using Cone;
using System;

namespace PointOfSale
{
	[Feature("Sell one item")]
    public class SellOneItemFeature
    {
		Display display;
		Sale sale;
		Action<PriceRequiredEventArgs> getPrice;

		[BeforeEach]
		public void given_a_terminal_with_attached_display() {
			var pos = new PosTerminal();
			pos.PriceRequired += (_, e) => getPrice?.Invoke(e);

			display = new Display();
			display.ConnectTo(pos);

			sale = new Sale(pos, display);
		}

		public void scan_a_code_show_the_price() {
			var existingBarcode = "123456789";
			var expectedPrice = "$11.50";
			getPrice = x => x.ItemPrice = expectedPrice;
			
			ProcessBarcode(existingBarcode);
			Check.That(() => display.Text == expectedPrice);
		}

		public void display_missing_message_for_missing_product() {
			ProcessBarcode("No Such Thing");
			Check.That(() => display.Text == "Missing Product <No Such Thing>");
		}

		public void display_scanning_error_for_empty_barcode() {
			ProcessBarcode(string.Empty);
			Check.That(() => display.Text == "Scanning Error: Empty Barcode");
		}

		void ProcessBarcode(string input) => sale.ProcessBarcode(input);
    }
}

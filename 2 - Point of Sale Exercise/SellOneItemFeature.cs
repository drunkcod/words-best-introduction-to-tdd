using Cone;
using System;

namespace PointOfSale
{
	[Feature("Sell one item")]
    public class SellOneItemFeature
    {
		class Sale
		{
			readonly PosTerminal terminal;
			readonly Display display;

			public Sale(PosTerminal terminal, Display display) { 
				this.terminal = terminal;
				this.display = display;
			}

			public void ProcessBarcode(string input) {
				try {
					terminal.ProcessBarcode(new Barcode(input));
				} catch(ArgumentException){
					display.Text = "Scanning Error: Empty Barcode";
				}
			}
		}

		PosTerminal pos;
		Display display;
		Sale sale;

		[BeforeEach]
		public void given_a_terminal_with_attached_display() {
			pos = new PosTerminal();
			display = new Display();

			display.ConnectTo(pos);

			sale = new Sale(pos, display);
		}

		public void scan_a_code_show_the_price() {
			var existingBarcode = "123456789";
			var expectedPrice = "$11.50";
			pos.PriceRequired += (_, e) => e.ItemPrice = expectedPrice;
			
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

using Cone;

namespace PointOfSale
{
	[Feature("Scanning an item")]
    public class ScanningAnItemFeature
    {
		public TerminalAndDisplayContext Pos = new TerminalAndDisplayContext();

		public void scan_a_code_show_the_price() {
			var existingBarcode = "123456789";
			Pos.AddPrice(existingBarcode, new Price(11.50m));
			Pos.ProcessBarcode(existingBarcode);
			Check.That(() => Pos.DisplayText == "$11.50");
		}

		public void display_missing_message_for_missing_product() {
			Pos.ProcessBarcode("No Such Thing");
			Check.That(() => Pos.DisplayText == "Missing Product <No Such Thing>");
		}

		public void display_scanning_error_for_empty_barcode() {
			Pos.ProcessBarcode(string.Empty);
			Check.That(() => Pos.DisplayText == "Scanning Error: Empty Barcode");
		}
    }
}

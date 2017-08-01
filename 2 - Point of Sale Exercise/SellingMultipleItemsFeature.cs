using Cone;

namespace PointOfSale
{
	[Feature("Selling items")]
	public class SellingMultipleItemsFeature
	{
		public TerminalAndDisplayContext Pos = new TerminalAndDisplayContext();

		public void total_when_no_items_added() {
			Pos.PressTotal();
			Check.That(() => Pos.DisplayText == "No Sale in Progress, Try Scanning a Product");
		}

		public void total_with_single_product() {
			Pos.AddPrice("A", new Price(1.23m));

			Pos.ProcessBarcode("A");
			Pos.PressTotal();

			Check.That(() => Pos.DisplayText == "Total: $1.23");
		}

		public void total_multiple_products() {
			Pos.AddPrice("A", new Price(8.50m));
			Pos.AddPrice("B", new Price(12.75m));
			Pos.AddPrice("C", new Price(3.30m));

			Pos.ProcessBarcode("A");
			Pos.ProcessBarcode("B");
			Pos.ProcessBarcode("C");
			Pos.PressTotal();

			Check.That(() => Pos.DisplayText == "Total: $24.55");
		}

		public void total_with_missing_product() {
			Pos.AddPrice("A", new Price(6.66m));

			Pos.ProcessBarcode("A");
			Pos.ProcessBarcode("B");
			Pos.ProcessBarcode("C");
			Pos.PressTotal();

			Check.That(() => Pos.DisplayText == "Total: $6.66");
		}
	}
}

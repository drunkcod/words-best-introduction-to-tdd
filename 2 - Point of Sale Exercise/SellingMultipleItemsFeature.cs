using Cone;

namespace PointOfSale
{
	[Feature("Selling multiple items")]
	public class SellingMultipleItemsFeature
	{
		Display display;
		PriceLookup prices;
		Sale sale;

		[BeforeEach]
		public void given_a_terminal_with_attached_display() {
			var pos = new PosTerminal();

			display = new Display();
			display.ConnectTo(pos);

			prices = new PriceLookup();
			prices.ConnectTo(pos);

			sale = new Sale(pos, display);
		}

		public void total_when_no_items_added() {
			sale.PressTotal();
			Check.That(() => display.Text == "No Sale in Progress, Try Scanning a Product");
		}

		public void total_with_single_product() {
			prices.Add(new Barcode("A"), new Price(1.23m));

			sale.ProcessBarcode("A");
			sale.PressTotal();

			Check.That(() => display.Text == "Total: $1.23");
		}
	}
}

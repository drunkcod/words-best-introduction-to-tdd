using System.Globalization;

namespace PointOfSale
{
	class Display
	{
		public string Text { get; private set; }

		public void DisplayError(string errorMessage) => Text = errorMessage;
		void DisplayPrice(Price itemPrice) => Text = string.Format("${0}", itemPrice.ToString(CultureInfo.InvariantCulture));
		void DisplayMissingProduct(Barcode missingItem) => Text = $"Missing Product <{missingItem}>";

		public void ConnectTo(PosTerminal terminal) {
			terminal.ItemAdded += (_, e) => DisplayPrice(e.ItemPrice);
			terminal.MissingItem += (_, e) => DisplayMissingProduct(e.Barcode);
		}
	}
}

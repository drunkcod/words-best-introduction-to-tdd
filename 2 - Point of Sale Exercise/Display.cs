using System.Globalization;

namespace PointOfSale
{
	class Display
	{
		public string Text { get; private set; }

		public void DisplayError(string errorMessage) => Text = errorMessage;
		public void DisplayTotal(Price totalPrice) => Text = $"Total: {Format(totalPrice)}";
		void DisplayPrice(Price itemPrice) => Text = Format(itemPrice);
		void DisplayMissingProduct(Barcode missingItem) => Text = $"Missing Product <{missingItem}>";

		public void ConnectTo(PosTerminal terminal) {
			terminal.ItemAdded += (_, e) => DisplayPrice(e.ItemPrice);
			terminal.MissingItem += (_, e) => DisplayMissingProduct(e.Barcode);
		}

		static string Format(Price price) => string.Format("${0}", price.ToString(CultureInfo.InvariantCulture));
	}
}

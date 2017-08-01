namespace PointOfSale
{
	class Display
	{
		public string Text;

		public void ConnectTo(PosTerminal terminal) {
			terminal.ItemAdded += (_, e) => Text = e.ItemPrice;
			terminal.MissingItem += (_, e) => Text = $"Missing Product <{e.Barcode}>";
		}
	}
}

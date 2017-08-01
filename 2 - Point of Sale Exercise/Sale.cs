using System;

namespace PointOfSale
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
}

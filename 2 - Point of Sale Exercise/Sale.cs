using System;

namespace PointOfSale
{
	class Sale
	{
		readonly PosTerminal terminal;
		readonly Display display;

		Price? total;

		public Sale(PosTerminal terminal, Display display) { 
			this.terminal = terminal;
			this.display = display;
			
			terminal.ItemAdded += (_, e) => total = e.ItemPrice;
		}

		public void ProcessBarcode(string input) {
			try {
				terminal.ProcessBarcode(new Barcode(input));
			} catch(ArgumentException){
				display.DisplayError("Scanning Error: Empty Barcode");
			}
		}

		public void PressTotal() {
			if(total.HasValue)
				display.DisplayTotal(total.Value);
			else
				display.DisplayError("No Sale in Progress, Try Scanning a Product");
		}
	}
}

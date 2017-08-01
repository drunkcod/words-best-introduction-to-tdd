using System;
using System.Collections.Generic;
using System.Linq;

namespace PointOfSale
{
	class Sale
	{
		readonly PosTerminal terminal;
		readonly Display display;

		readonly List<Price> total = new List<Price>();

		public Sale(PosTerminal terminal, Display display) { 
			this.terminal = terminal;
			this.display = display;
			
			terminal.ItemAdded += (_, e) => total.Add(e.ItemPrice);
		}

		public void ProcessBarcode(string input) {
			try {
				terminal.ProcessBarcode(new Barcode(input));
			} catch(ArgumentException){
				display.DisplayError("Scanning Error: Empty Barcode");
			}
		}

		public void PressTotal() {
			if(total.Count > 0)
				display.DisplayTotal(total.Aggregate((a, b) => a + b));
			else
				display.DisplayError("No Sale in Progress, Try Scanning a Product");
		}
	}
}

﻿using System;

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
				display.DisplayError("Scanning Error: Empty Barcode");
			}
		}

		public void PressTotal() {
			display.DisplayError("No Sale in Progress, Try Scanning a Product");
		}
	}
}

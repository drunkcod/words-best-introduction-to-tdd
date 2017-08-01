using System;

namespace PointOfSale
{
	class ItemAddedEventArgs : EventArgs
	{
		public readonly Barcode Barcode;
		public readonly string ItemPrice;

		public ItemAddedEventArgs(Barcode barcode, string itemPrice) { 
			this.Barcode = barcode;
			this.ItemPrice = itemPrice;
		}
	}

	class PriceRequiredEventArgs : EventArgs 
	{
		public readonly Barcode Barcode;
		public string ItemPrice;

		public PriceRequiredEventArgs(Barcode barcode) {
			this.Barcode = barcode;
		}
	}

	class PosTerminal
	{
		public event EventHandler<ItemAddedEventArgs> ItemAdded;
		public event EventHandler<ItemAddedEventArgs> MissingItem;
		public event EventHandler<PriceRequiredEventArgs> PriceRequired;

		public void ProcessBarcode(Barcode barcode) {
			var priceCheck = new PriceRequiredEventArgs(barcode);
			PriceRequired?.Invoke(this, priceCheck);
			var e = new ItemAddedEventArgs(priceCheck.Barcode, priceCheck.ItemPrice);
			(string.IsNullOrEmpty(e.ItemPrice) ? MissingItem : ItemAdded)?.Invoke(this, e);
		}
	}
}

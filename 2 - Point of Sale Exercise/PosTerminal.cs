using System;

namespace PointOfSale
{
	class ItemAddedEventArgs : EventArgs
	{
		public readonly string Barcode;
		public readonly string ItemPrice;

		public ItemAddedEventArgs(string barcode, string itemPrice) { 
			this.Barcode = barcode;
			this.ItemPrice = itemPrice;
		}
	}

	class PriceRequiredEventArgs : EventArgs 
	{
		public readonly string Barcode;
		public string ItemPrice;

		public PriceRequiredEventArgs(string barcode) {
			this.Barcode = barcode;
		}
	}

	class PosTerminal
	{
		public event EventHandler<ItemAddedEventArgs> ItemAdded;
		public event EventHandler<ItemAddedEventArgs> MissingItem;
		public event EventHandler<PriceRequiredEventArgs> PriceRequired;

		public void ProcessBarcode(string barcode) {
			var priceCheck = new PriceRequiredEventArgs(barcode);
			PriceRequired?.Invoke(this, priceCheck);
			var e = new ItemAddedEventArgs(priceCheck.Barcode, priceCheck.ItemPrice);
			(string.IsNullOrEmpty(e.ItemPrice) ? MissingItem : ItemAdded)?.Invoke(this, e);
		}
	}
}

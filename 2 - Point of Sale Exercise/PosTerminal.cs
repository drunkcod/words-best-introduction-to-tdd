using System;

namespace PointOfSale
{
	class ItemAddedEventArgs : EventArgs
	{
		public readonly Barcode Barcode;
		public readonly Price? ItemPrice;

		public ItemAddedEventArgs(Barcode barcode, Price? itemPrice) { 
			this.Barcode = barcode;
			this.ItemPrice = itemPrice;
		}
	}

	class PriceRequiredEventArgs : EventArgs 
	{
		public readonly Barcode Barcode;
		public Price? ItemPrice;

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
			if(barcode == null)
				throw new ArgumentNullException(nameof(barcode));
			var priceCheck = new PriceRequiredEventArgs(barcode);
			PriceRequired?.Invoke(this, priceCheck);
			var e = new ItemAddedEventArgs(priceCheck.Barcode, priceCheck.ItemPrice);
			(e.ItemPrice.HasValue ? ItemAdded : MissingItem )?.Invoke(this, e);
		}
	}
}

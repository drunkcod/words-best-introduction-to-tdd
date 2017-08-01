using System;

namespace PointOfSale
{
	class ItemAddedEventArgs : EventArgs
	{
		public readonly Barcode Barcode;
		public readonly Price ItemPrice;

		public ItemAddedEventArgs(Barcode barcode, Price itemPrice) { 
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

		public bool PriceFound => ItemPrice.HasValue;
	}

	class MissingItemEventArgs : EventArgs
	{
		public readonly Barcode Barcode;

		public MissingItemEventArgs(Barcode barcode) {
			this.Barcode = barcode;
		}
	}

	class PosTerminal
	{
		public event EventHandler<ItemAddedEventArgs> ItemAdded;
		public event EventHandler<MissingItemEventArgs> MissingItem;
		public event EventHandler<PriceRequiredEventArgs> PriceRequired;

		public void ProcessBarcode(Barcode barcode) {
			if(barcode == null)
				throw new ArgumentNullException(nameof(barcode));
			var priceCheck = new PriceRequiredEventArgs(barcode);
			PriceRequired?.Invoke(this, priceCheck);
			if(priceCheck.PriceFound)
				ItemAdded?.Invoke(this, new ItemAddedEventArgs(priceCheck.Barcode, priceCheck.ItemPrice.Value));
			else MissingItem?.Invoke(this, new MissingItemEventArgs(priceCheck.Barcode));
		}
	}
}

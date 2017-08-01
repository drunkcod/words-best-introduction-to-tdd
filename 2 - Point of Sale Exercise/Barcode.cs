using System;

namespace PointOfSale
{
	class Barcode 
	{
		readonly string value;

		public Barcode(string value) {
			if(string.IsNullOrEmpty(value)) 
				throw new ArgumentOutOfRangeException(nameof(value), "Barcode can't be empty");
			this.value = value;
		}

		public override string ToString() => value;
	}
}

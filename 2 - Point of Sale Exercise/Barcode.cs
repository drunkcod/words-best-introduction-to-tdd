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
		public override int GetHashCode() => value.GetHashCode();
		public override bool Equals(object obj) {
			var other = obj as Barcode;
			if(other == null)
				return false;
			return other.value == this.value;
		}
	}
}

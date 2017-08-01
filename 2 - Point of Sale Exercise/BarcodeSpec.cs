using Cone;
using System;

namespace PointOfSale
{
	[Describe(typeof(Barcode))]
	public class BarcodeSpec
	{
		public void cant_be_empty() => Check.Exception<ArgumentException>(() => new Barcode(string.Empty));
		public void cant_be_null() => Check.Exception<ArgumentException>(() => new Barcode(null));
	}
}

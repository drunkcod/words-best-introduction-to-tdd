using Cone;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PointOfSale
{
	[Describe(typeof(Barcode))]
	public class BarcodeSpec
	{
		public void cant_be_empty() => Check.Exception<ArgumentException>(() => new Barcode(string.Empty));
		public void cant_be_null() => Check.Exception<ArgumentException>(() => new Barcode(null));

		public void can_be_used_as_key() {
			var lookup = new Dictionary<Barcode, int> {
				{ new Barcode("123"), 1 }
			};
			
			Check.With(() => lookup.First())
				.That(x => lookup[x.Key] == lookup[new Barcode(x.Key.ToString())]);
		}
	}
}

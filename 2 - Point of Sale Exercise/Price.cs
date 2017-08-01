using System;

namespace PointOfSale
{
	public struct Price
	{
		readonly decimal value;

		public Price(decimal value) {
			this.value = value;
		}

		public override string ToString() => value.ToString();
		public string ToString(IFormatProvider formatProvider) => value.ToString(formatProvider);

		public static bool operator==(Price a, Price b) => a.value == b.value;
		public static bool operator!=(Price a, Price b) => !(a == b);

		public static Price operator+(Price a, Price b) => new Price(a.value + b.value);
	}
}

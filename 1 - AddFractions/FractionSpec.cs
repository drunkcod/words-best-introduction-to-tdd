using Cone;
using System;

namespace AddFractions
{
	struct Fraction
	{
		public Fraction(int n) : this(n, 1) { }

		public Fraction(int n, int d) {
			if(d == 0) throw new ArgumentOutOfRangeException(nameof(d));
			this.Numerator = n;
			this.Denominator = d;
		}

		public readonly int Numerator;
		public readonly int Denominator;

		public static Fraction operator+(Fraction lhs, Fraction rhs) =>
			lhs.Denominator == rhs.Denominator
			? new Fraction(lhs.Numerator + rhs.Numerator, lhs.Denominator)
			: new Fraction(lhs.Numerator * rhs.Denominator + rhs.Numerator * lhs.Denominator, lhs.Denominator * rhs.Denominator);
		
		public static bool operator==(Fraction a, Fraction b) => a.Numerator == b.Numerator && a.Denominator == b.Denominator;
		public static bool operator!=(Fraction a, Fraction b) => !(a == b);

		public override string ToString() => $"{Numerator}/{Denominator}";
		public override bool Equals(object obj) => obj is Fraction ? this == (Fraction)obj : base.Equals(obj);
		public override int GetHashCode() => Numerator << 16 | Denominator;

		public Fraction Reduce() {
			var gcd = Gcd(Numerator, Denominator);
			return new Fraction(Numerator / gcd, Denominator / gcd);
		}

		public static int Gcd(int a, int b) {
			if(a == 0)
				return b;
			while (b != 0) {
				if(a > b) {
					var c = a;
					a = b;
					b = c;
				}
				b = b % a;
			}
			return a;
		}
	}

	[Describe(typeof(Fraction))]
    public class FractionSpec
    {
		public void adding_zeros_yields_zero() => 
			Check.That(() => new Fraction(0) + new Fraction(0) == new Fraction(0));

		public void add_zero_to_something() {
			var something = new Fraction(3);
			Check.That(() => something + new Fraction(0) == something);
		}

		public void add_something_to_zero() {
			var something = new Fraction(5);
			Check.That(() => new Fraction(0) + something == something);
		}

		public void add_something_to_something_else() =>
			Check.That(() => new Fraction(-3) + new Fraction(1) == new Fraction(-2));

		public void add_with_common_denominator() =>
			Check.With(() => new Fraction(1, 3) + new Fraction(1, 3)).That(
				sum => sum == new Fraction(2, 3),
				sum => sum.Numerator == 2,
				sum => sum.Denominator == 3);

		public void add_with_different_denomoniators() =>
			Check.That(() => new Fraction(1, 2) + new Fraction(2, 5) == new Fraction(9, 10));

		public void denominator_cant_be_zero() => Check.Exception<ArgumentException>(() => new Fraction(1, 0));

		public void equality_checks_denomniator() => Check.That(() => !(new Fraction(1, 2) == new Fraction(1, 3)));

		public void reduce() => 
			Check.That(
				() => new Fraction(4, 4).Reduce() == new Fraction(1),
				() => new Fraction(4, 2).Reduce() == new Fraction(2),
				() => new Fraction(2, 4).Reduce() == new Fraction(1, 2));

		[Row(3, 9, 3)]
		[Row(63, 273, 21)]
		[Row(0, 7, 7)]
		public void gcd(int a, int b, int result) => Check.That(() => Fraction.Gcd(a, b) == result);
    }
}

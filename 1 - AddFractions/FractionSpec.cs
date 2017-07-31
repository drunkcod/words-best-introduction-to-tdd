using Cone;
using System;

namespace AddFractions
{
	struct Fraction
	{
		public Fraction(int n, int d) {
			if(d <= 0) throw new ArgumentOutOfRangeException(nameof(d));

			this.Numerator = n;
			this.Denominator = d;
		}

		public static Fraction Create(int numerator) => Create(numerator, 1);
		public static Fraction Create(int numerator, int denominator) =>
			(denominator > 0 ? new Fraction(numerator, denominator) : new Fraction(-numerator, -denominator))
			.Reduce();

		public readonly int Numerator;
		public readonly int Denominator;

		public static Fraction operator+(Fraction lhs, Fraction rhs) => 
			Create(lhs.Numerator * rhs.Denominator + rhs.Numerator * lhs.Denominator, lhs.Denominator * rhs.Denominator);
		
		public static bool operator==(Fraction a, Fraction b) => a.Numerator * b.Denominator == b.Numerator * a.Denominator;
		public static bool operator!=(Fraction a, Fraction b) => !(a == b);

		public override string ToString() => $"{Numerator}/{Denominator}";
		public override bool Equals(object obj) => obj is Fraction ? this == (Fraction)obj : base.Equals(obj);
		public override int GetHashCode() => Numerator << 16 | Denominator;

		public Fraction Reduce() {
			var gcd = Gcd(Numerator, Denominator);
			return new Fraction(Numerator / gcd, Denominator / gcd);
		}

		public static int Gcd(int a, int b) {
			while (b != 0) {
				var t = b;
				b = a % b;
				a = t;
			}
			return Math.Abs(a);
		}
	}

	[Describe(typeof(Fraction))]
    public class FractionSpec
    {
		public void denominator_cant_be_zero() => Check.Exception<ArgumentException>(() => new Fraction(1, 0));
		public void denominator_cant_be_sub_zero() => Check.Exception<ArgumentException>(() => new Fraction(1, -1));

		public void create_reduces() => Check.With(() => Fraction.Create(3, 6))
			.That(x => x.Numerator == 1, x => x.Denominator == 2);

		public void negative_fractions_have_positive_denominators() =>
			Check.With(() => Fraction.Create(1, -7)).That(
				x => x.Numerator == -1,
				x => x.Denominator == 7);

		public void reduce() =>
			Check.That(
				() => new Fraction(4, 4).Reduce() == Fraction.Create(1),
				() => new Fraction(4, 2).Reduce() == Fraction.Create(2),
				() => new Fraction(2, 4).Reduce() == new Fraction(1, 2));

		[Context("equality")]
		public class FractionEqualitySpec
		{
			public void a_equals_b() => Check.That(
				() => new Fraction(1, 4) == new Fraction(2, 8),
				() => new Fraction(1, 4) != new Fraction(4, 1));

			public void normalized_denominators() => Check.That(() => new Fraction(2, 4) == new Fraction(1, 2));

			public void checks_denomniator() => Check.That(() => !(new Fraction(1, 2) == new Fraction(1, 3)));
		}
    }

	[Feature("Number Theory")]
	public class GcdSpec
	{
		[DisplayAs("gcd({0}, {1}) == {2}")]
		[Row(3, 9, 3)]
		[Row(63, 273, 21)]
		[Row(0, 7, 7)]
		[Row(0, 1, 1)]
		[Row(-3, 1, 1)]
		[Row(3, -1, 1)]
		public void gcd(int a, int b, int result) => Check.That(() => Fraction.Gcd(a, b) == result);
	}

	[Describe(typeof(Fraction), "adding")]
	public class FractionAddSpec
	{
		public void zeros_and_zero_yields_zero() => 
			Check.That(() => Fraction.Create(0) + Fraction.Create(0) == Fraction.Create(0));

		public void zero_to_something() => Check.With(() => Fraction.Create(3))
			.That(something => something + Fraction.Create(0) == something);

		public void something_to_zero() => Check.With(() => Fraction.Create(5))
			.That(something => Fraction.Create(0) + something == something);

		public void something_to_something_else() =>
			Check.That(() => Fraction.Create(-3) + Fraction.Create(1) == Fraction.Create(-2));

		public void with_common_denominator() =>
			Check.With(() => new Fraction(1, 3) + new Fraction(1, 3)).That(
				sum => sum == new Fraction(2, 3),
				sum => sum.Numerator == 2,
				sum => sum.Denominator == 3);

		public void with_different_denomoniators() =>
			Check.That(() => new Fraction(1, 2) + new Fraction(2, 5) == new Fraction(9, 10));

		public void reduces_result() => Check.That(() => new Fraction(1, 4) + new Fraction(1, 4) == new Fraction(1, 2));
	}
}

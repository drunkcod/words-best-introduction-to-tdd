using Cone;

namespace AddFractions
{
	struct Fraction
	{
		
		public Fraction(int n) : this(n, 1) { }

		public Fraction(int n, int d) { 
			this.Numerator = n;
			this.Denominator = d;
		}

		public readonly int Numerator;
		public readonly int Denominator;

		public static Fraction operator+(Fraction lhs, Fraction rhs) =>
			lhs.Denominator == rhs.Denominator
			? new Fraction(lhs.Numerator + rhs.Numerator, lhs.Denominator)
			: new Fraction(lhs.Numerator * rhs.Denominator + rhs.Numerator * lhs.Denominator, lhs.Denominator * rhs.Denominator);
		

		public static bool operator==(Fraction lhs, Fraction rhs) => lhs.Numerator == rhs.Numerator;
		public static bool operator!=(Fraction lhs, Fraction rhs) => !(lhs == rhs);

		public override string ToString() => $"{Numerator}/{Denominator}";
		public override bool Equals(object obj) => obj is Fraction ? this == (Fraction)obj : base.Equals(obj);
		public override int GetHashCode() => Numerator << 16 | Denominator;
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
    }
}

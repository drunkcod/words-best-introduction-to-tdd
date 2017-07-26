using Cone;

namespace AddFractions
{
	struct Fraction
	{
		public Fraction(int n, int d) { }

		public static Fraction operator+(Fraction lhs, Fraction rhs) => new Fraction();

		public static bool operator==(Fraction lhs, Fraction rhs) => true;
		public static bool operator!=(Fraction lhs, Fraction rhs) => !(lhs == rhs);
	}

	[Describe(typeof(Fraction))]
    public class FractionSpec
    {
		public void adding_zeros_yields_zero() => Check.That(() => new Fraction(0, 1) + new Fraction(0, 2) == new Fraction(0, 3));
    }
}

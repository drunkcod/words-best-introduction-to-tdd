using Cone;

namespace AddFractions
{
	struct Fraction
	{
		readonly int n;
		public Fraction(int n) { this.n = n;}

		public static Fraction operator+(Fraction lhs, Fraction rhs) => new Fraction(lhs.n + rhs.n);

		public static bool operator==(Fraction lhs, Fraction rhs) => lhs.n == rhs.n;
		public static bool operator!=(Fraction lhs, Fraction rhs) => !(lhs == rhs);

		public override string ToString() => n.ToString();
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
    }
}

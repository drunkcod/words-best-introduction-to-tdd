using Cone;
using Cone.Helpers;
using PointOfSale;
using System;

namespace ClientFirstWithMocks
{
	class SalesController
	{ 
		readonly ICatalog catalog;
		readonly IDisplay display;

		public SalesController(ICatalog catalog, IDisplay display) {
			this.catalog = catalog;
			this.display = display;
		}

		public void OnBarcode(Barcode barcode) { 
			var price = catalog.GetPrice(barcode);
			if(price.HasValue)
				display.DisplayTotal(price.Value);
		}
	}

	interface ICatalog
	{
		Price? GetPrice(Barcode barcode);
	}

	interface IDisplay
	{ 
		void DisplayTotal(Price price);
	}

	class DisplayStub : IDisplay
	{ 
		public Action<Price> DisplayTotal;

		void IDisplay.DisplayTotal(Price price) => DisplayTotal?.Invoke(price);
	}

	class CatalogStub : ICatalog
	{
		public Func<Barcode, Price?> GetPrice;
		
		Price? ICatalog.GetPrice(Barcode barcode) => GetPrice?.Invoke(barcode);
	}


	[Describe(typeof(SalesController))]
    public class SalesControllerSpec
    {
		public void product_found() {
			var expectedPrice = new Price(7.95m);
			var theBarcode = new Barcode("12345");

			var display = new DisplayStub();
			var catalog = new CatalogStub {
				GetPrice = barcode => {
					Check.That(() => barcode == theBarcode);
					return expectedPrice;
				}
			};

			var displayTotal = MethodSpy.On(ref display.DisplayTotal);

			var sales = new SalesController(catalog, display);
			sales.OnBarcode(theBarcode);

			Check.That(() => displayTotal.HasBeenCalled);
			displayTotal.Then(price => Check.That(() => price == expectedPrice));
		}

		//public void product_missing() { }
		//public void empty() { }
    }
}

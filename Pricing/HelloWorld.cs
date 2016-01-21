
// The "Hello World!" program in C#.

using System;
using Pricing;
using System.Collections.Generic;

namespace HelloWorld
{
	public class Hello
	{
		public static void Main ()
		{
			//Console.WriteLine ("Hello World!");

			DataAccessor da = new DataAccessor ();
			List<PricingData> res = da.GetPricingForProduct ("sku", "1015600035");
			foreach (PricingData p in res) 
			{
				Console.WriteLine (p.Price + ", " + p.PriceDate);
			}

			PriceAnalyzer analyzer = new PriceAnalyzer (res, 5);
			analyzer.ProcessPricing ();
		}
	}
}
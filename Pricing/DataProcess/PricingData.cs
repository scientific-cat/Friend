using System;

namespace Pricing
{
	public class PricingData
	{
		public string OutletName { get; set;}
		public string SKU { get; set;}
		public DateTime PriceDate { get; set;}
		public decimal Price { get; set;}
		public decimal Volume {get;set;}

		public int? TPRWeekNumber { get; set;}
		public bool IsTPR { get; set;}
		public decimal? NonTPRVolume { get; set;}
		public decimal? NonTPRPrice { get; set;}
		public decimal? Discount {get;set;}
		public	decimal? Uplift { get; set;}
		public PricingData ()
		{
		}
	}
}


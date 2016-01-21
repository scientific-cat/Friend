using System;
using System.Collections.Generic;

namespace Pricing
{
	public class PriceAnalyzer
	{
		// this list contains each row of data in sorted order
		private List<PricingData> _pricingData;
		// this is a threshold to determine if price has changed significantly or not
	//	private decimal _priceChangeThreshold;

		public PriceAnalyzer (List<PricingData> pricingData, decimal priceChangeThreshold)
		{
			this._pricingData = pricingData;
	//		this._priceChangeThreshold = priceChangeThreshold;
		}

		public List<PricingData> ProcessPricing()
		{
			this.SetTPR ();
			this.SetTPRWeek ();
			this.SetVolume ();
			this.SetVolumeUplift ();

			return this._pricingData;
		}

		private decimal GetMaxPrice()
		{
			// TODO: write algorithm, or use LINQ
			return this._pricingData [0].Price;
		}

		/// <summary>
		/// Check if dates of sales are close enough
		/// </summary>
		/// <returns><c>true</c>, if dates are close was checked, <c>false</c> otherwise.</returns>
		/// <param name="prevPricingData">Previous pricing data.</param>
		/// /// <param name="currentPricingData">Current pricing data.</param>
		private bool DatesAreClose(PricingData prevPricingData, PricingData currentPricingData)
		{
			int prevDateDiff = (currentPricingData.PriceDate - prevPricingData.PriceDate).Days;
			bool result = prevDateDiff > 0 && prevDateDiff < 8;
			return result;
 
		}

		private void ProcessExclusions()
		{

		}

		private void SetVolumeUplift()
		{
			int i = 0; 

			while (i < this._pricingData.Count - 3) // we're checking 2 weeks ahead
			{
				i++;
				PricingData currentPricingData = this._pricingData [i]; 
				if (currentPricingData.IsTPR)
				{
					currentPricingData.Discount = currentPricingData.Price / currentPricingData.NonTPRPrice - 1;
					currentPricingData.Uplift = currentPricingData.Volume / currentPricingData.NonTPRVolume - 1;
				}

			}
		}

		private void SetVolume()
		{
			int i = 0;
			this._pricingData[i].NonTPRVolume = this._pricingData [i].Volume;

			while (i < this._pricingData.Count - 3) // we're checking 2 weeks ahead
			{
				i++;
				PricingData prevPricingData = this._pricingData [i-1]; 

				PricingData currentPricingData = this._pricingData [i]; 
				PricingData nextPricingData = this._pricingData[i+1];
				PricingData nextPricingData2 = this._pricingData[i+2];
				PricingData nextPricingData3 = this._pricingData[i+3];

				if (currentPricingData.TPRWeekNumber == -2)
				{
					currentPricingData.NonTPRVolume = (currentPricingData.Volume + nextPricingData.Volume + nextPricingData2.Volume) / 3;
				}
				else
				{
					if (currentPricingData.TPRWeekNumber == -1)
					{
						if (prevPricingData.TPRWeekNumber == -2)
						{
							currentPricingData.NonTPRVolume = prevPricingData.NonTPRVolume;
						}
						else
						{
							currentPricingData.NonTPRVolume = (currentPricingData.Volume + nextPricingData.Volume)/2;
						}
					}
					else
					{
						if (currentPricingData.TPRWeekNumber == 0 || !currentPricingData.TPRWeekNumber.HasValue)
						{
							if (prevPricingData.TPRWeekNumber == -1)
							{
								currentPricingData.NonTPRVolume = prevPricingData.NonTPRVolume;
							}
							else
							{
								currentPricingData.NonTPRVolume = currentPricingData.Volume;
							}
						}
						else
						{
							if (currentPricingData.TPRWeekNumber > 0)
							{
								currentPricingData.NonTPRVolume = prevPricingData.NonTPRVolume;
							}
							else
							{
								currentPricingData.NonTPRVolume = currentPricingData.Volume;
							}
						}
					}

				}
			}
		}

		private void SetTPRWeek()
		{
			int i = 0; 
			while (i < this._pricingData.Count - 3) // we're checking 2 weeks ahead
			{
				PricingData prevPricingData = this._pricingData [i-1]; 

				PricingData currentPricingData = this._pricingData [i]; 
				PricingData nextPricingData = this._pricingData[i+1];
				PricingData nextPricingData2 = this._pricingData[i+2];
				PricingData nextPricingData3 = this._pricingData[i+3];
				// if current price is not marked as TPR, check if following 3 prices are marked TPR and
				// assing TPR week accordingly
				if (!currentPricingData.IsTPR)
				{
					// if next pricing  is TPR and the date is close, this is 0-th week
					if (nextPricingData.IsTPR
					    && this.DatesAreClose (currentPricingData, nextPricingData)) 
					{
						currentPricingData.TPRWeekNumber = 0;
					} 
					else 
					{
						// if pricing after next is TPR and both pricing dates are close, then it's -1st week
						if(nextPricingData2.IsTPR 
							&& this.DatesAreClose (currentPricingData, nextPricingData)
								&& this.DatesAreClose (nextPricingData, nextPricingData2))
						{
							currentPricingData.TPRWeekNumber = -1;
						}
						else
						{
							// if pricing after next is TPR and both pricing dates are close, then it's -3rd week
							if(nextPricingData3.IsTPR 
							   && this.DatesAreClose (currentPricingData, nextPricingData)
							   && this.DatesAreClose (nextPricingData, nextPricingData2)
								&& this.DatesAreClose (nextPricingData2, nextPricingData3))

							{
								currentPricingData.TPRWeekNumber = -1;
							}
							else
							{
								currentPricingData.TPRWeekNumber = null;
							}
						}
					}
				}
				// if current price IS marked TPR, then assign TPR week to it based on previos price TPR week
				else
				{
					if (currentPricingData.IsTPR && prevPricingData.TPRWeekNumber != null)
					{
						currentPricingData.TPRWeekNumber = prevPricingData.TPRWeekNumber + 1;
					}
					else
					{
						currentPricingData.TPRWeekNumber = null;
					}
				}
			}
		}

		private void SetTPR()
		{
			int step1 = -5;
			int step2 = -5;
			decimal maxPrice = this.GetMaxPrice ();
			// search for max price in this store (? during which period?)


			// analyze data from same store,

			int i = 0;
			this._pricingData[i].NonTPRPrice = this._pricingData [i].Price;
			while (i < this._pricingData.Count - 1) 
			{
				i++;
				
				PricingData prevPricingData = this._pricingData[i-1];
				PricingData currentPricingData = this._pricingData [i]; 

				int prevDateDiff = (currentPricingData.PriceDate - prevPricingData.PriceDate).Days;

				// check if we have pricing data before this date in this store
				// and current price is less than 5% diffrent from current price
				// and smth I dont understand with max-price (??)
				if (
					prevDateDiff > 0 && prevDateDiff < 8 &&
					(currentPricingData.Price / prevPricingData.NonTPRPrice - 1 < step1) &&
					(maxPrice / prevPricingData.NonTPRPrice >= 1 + step2)
				    ) 
				{
					currentPricingData.NonTPRPrice = prevPricingData.NonTPRPrice;
				} 
				else 
				{
					currentPricingData.NonTPRPrice = currentPricingData.Price;
				}
				// set TPR flag
				currentPricingData.IsTPR = (currentPricingData.NonTPRPrice < currentPricingData.Price);				
			}
		}
	}
}


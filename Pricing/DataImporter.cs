using System;
using System.Collections;
using System.Collections.Generic;

namespace Pricing
{
	public interface IDataReader
	{ 
		SortedList<int, PricingData> ReadData ();
	}
}


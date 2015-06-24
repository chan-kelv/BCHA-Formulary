using System;
using System.Collections.Generic;

namespace BCHAFormulary
{
	public class BrandFormularyDrug : BrandDrug
	{
		public List<string> strengths;

		public BrandFormularyDrug (string genericName, string brandName, string strength) : base (genericName, brandName, UIProperties.Formulary)
		{
			strengths = new List<string> ();
			strengths.Add (strength);
		}

		public void addStrength(string strength){
			if (!string.IsNullOrEmpty (strength))
				strengths.Add (strength);
		}
	}
}


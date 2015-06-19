using System;
using System.Collections.Generic;

namespace BCHAFormulary
{
	public class GenericFormularyDrug :GenericDrug
	{
		public List<string> strengths;

		public GenericFormularyDrug (string genericName, string brandName, string strength) 
			: base (genericName, brandName, UIProperties.Formulary)
		{
			strengths = new List<string> () {
				strength
			};
		}

		public void AddStrength(string newStrength){
			strengths.Add (newStrength);
		}
	}
}


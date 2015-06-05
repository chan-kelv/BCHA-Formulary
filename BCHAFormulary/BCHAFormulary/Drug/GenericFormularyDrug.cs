using System;
using System.Collections.Generic;

namespace BCHAFormulary
{
	public class GenericFormularyDrug :GenericDrug
	{
		public List<string> strengths;

		public GenericFormularyDrug (string genericName, string brandName, string strength) 
			: base (genericName, brandName, "Formulary")
		{
			strengths = new List<string> () {
				strength
			};
		}
	}
}


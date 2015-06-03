using System;
using System.Collections.Generic;

namespace BCHAFormulary
{
	public class GenericDrug : Drug
	{
		public string genericName;
		public List<string> brandNames;

		public GenericDrug (string genericName, string brandName, string status) : base(status)
		{
			this.genericName = genericName;
			brandNames = new List<string> (){
				brandName
			};
		}

		public bool containsBrandName(string name){
			 return brandNames.Find (s => s.IndexOf (name, System.StringComparison.OrdinalIgnoreCase) >= 0);
		}
	}
}


using System;
using System.Collections.Generic;
using System.Linq;

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

		public void addBrandName(string name){
			if(!string.IsNullOrEmpty(name))
				brandNames.Add(name);
		}

		public bool containsBrandName(string name){
			return (brandNames.Any (s => s.Equals (name, StringComparison.OrdinalIgnoreCase)));
		}
	}
}


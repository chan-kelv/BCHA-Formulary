using System;
using System.Collections.Generic;
using System.Linq;

namespace BCHAFormulary
{
	public class BrandDrug : Drug
	{
		public string brandName;
		public List<string> genericNames;

		public BrandDrug (string genericName, string brandName, string status) : base(status)
		{
			genericNames = new List<string> ();
			genericNames.Add (genericName);
			this.brandName = brandName;
		}

		public void addGenericName(string name){
			if (!string.IsNullOrEmpty (name))
				genericNames.Add (name);
		}

		public bool containsGenericName(string name){
			return (genericNames.Any (s => s.Equals (name, StringComparison.OrdinalIgnoreCase)));
		}

	}
}


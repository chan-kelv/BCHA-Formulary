using System;
using System.Collections.Generic;

namespace BCHAFormulary
{
	public class BrandDrug : Drug
	{
		string brandName { get; set; }
		List<string> genericNames;

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
//
		// USELESS METHOD??
//		public bool containsGenericName(string name){
//			return genericNames.Contains (name);
//		}
	}
}


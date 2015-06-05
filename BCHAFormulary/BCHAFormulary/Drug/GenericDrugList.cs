using System;
using System.Collections.Generic;

namespace BCHAFormulary
{
	public class GenericDrugList
	{
		public Dictionary<string, GenericDrug> genericDrugList;
		public GenericDrugList ()
		{
			genericDrugList = new Dictionary<string, GenericDrug> ();
		}
	}
}


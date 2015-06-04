using System;
using System.Collections.Generic;

namespace BCHAFormulary
{
	public class BrandDrugList
	{
		//this whole class is not necessary if we can pass the dictionary back
		//TODO look into it
		public Dictionary <string, BrandDrug> brandDrugList;

		public BrandDrugList ()
		{
			brandDrugList = new Dictionary<string, BrandDrug> ();
		}

//		public void addBrandDrug(BrandDrug drug){
//			brandDrugList.Add(drug.brandName, drug);
//		}
//
//		public void removeBrandDrug(string drugName){
//			if (brandDrugList.ContainsKey (drugName))
//				brandDrugList.Remove (drugName);
//		}
//
//		public int drugListSize(){
//			return brandDrugList.Count;
//		}
//
//		public Drug getBrandDrug(string drugName){
//			Drug drug = null;
//			brandDrugList.TryGetValue (drugName, drug);
//			return drug;
//		}
//
//		public bool brandDrugListEmpty(){
//			return brandDrugList.Count == 0;
//		}
//
//		public bool containsBrandDrugObject(BrandDrug drug){
//			return brandDrugList.ContainsValue (drug);
//		}
//
//		public bool containsBrandDrugName(string drugName){
//			return brandDrugList.ContainsKey (drugName);
//		}
//
//		public List<string> getBrandNameList(){
//			return brandDrugList.Keys;
//		}
	}
}


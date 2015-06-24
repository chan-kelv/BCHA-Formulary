using System;
using System.IO;
using CsvHelper;
using System.Text;
using System.Collections.Generic;

namespace BCHAFormulary
{
	public class CSVParser
	{
		public Dictionary<string, GenericDrug> genericList;
		public Dictionary<string, BrandDrug> brandList;

		public CSVParser ()
		{
			genericList = new GenericDrugList ().genericDrugList;
			brandList = new BrandDrugList ().brandDrugList;
		}

		public void ParseFormulary(string csvFile){
			var stream = StringToStream (csvFile);
			var parser = new CsvParser (stream);
			try{
				
				string[] nextLine = parser.Read(); //skip the first title line
				while( (nextLine = parser.Read()) != null){
					string name = nextLine[0].ToUpper();
					string brandName = nextLine[2].ToUpper();

					if(!string.IsNullOrEmpty(name)){
						//generic list------------------------
						GenericDrug formularyDrug;
						if (genericList.TryGetValue(name, out formularyDrug)){
							((GenericFormularyDrug) formularyDrug).AddStrength(nextLine[1]);
							genericList[name] = formularyDrug;

							//if brandName cell is not empty, check if the brandNames are unique and add to drug if needed
							if(!string.IsNullOrEmpty(brandName))
								addBrandNameToExistingFormularyDrug(name, brandName);
							}
						else if (string.IsNullOrEmpty(brandName)) //if there is a drug with no brand name
							genericList.Add(name, new GenericFormularyDrug(name,string.Empty,nextLine[1]));
						else
							addGenericFormularyDrugWithBrandName(name, nextLine[1], brandName);
					}
					//brandList------------------------------------------------------------------------------
					if(!string.IsNullOrEmpty(brandName)){
						if(brandName.Contains(",")){
							var brandNameArray = brandName.Split(',');
							foreach(string brandNameSingle in brandNameArray){
								addBrandNameFormulary(name, brandNameSingle.Trim(), nextLine[1]);
							}
						}
						else //there is only 1 brand name
							addBrandNameFormulary(name, brandName, nextLine[1]);
					}
				}
				stream.Close();
			}
			catch(Exception e){
				Console.WriteLine ("Formulary parser failed due to {0}", e.Message);
			}
			finally{
				stream = null;
			}
		}

		public void ParseExcluded(string csvFile){
			//convert string to stream
			var stream = StringToStream(csvFile);
			var parser = new CsvParser (stream);

			try{
				string lastGenericDrug = null;
				string lastBrandDrug = null;
				List<string> excludedBrandNameList = new List<string>();

				string[] nextLine = parser.Read(); //skip title line
				while( (nextLine = parser.Read()) != null){
					string name = nextLine[0].ToUpper();
					string brandName = nextLine[1].ToUpper();
					string criteria = nextLine[2].ToUpper();
					//extraline for restricted criteria
					if(string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(criteria)){
						GenericDrug generic;
						if (genericList.TryGetValue(lastGenericDrug, out generic))
							((GenericExcludedDrug)generic).additionalCriteria(criteria);
						BrandDrug brand;
						if(brandList.TryGetValue(lastBrandDrug, out brand))
							((BrandExcludedDrug)brand).additionalCriteria(criteria);
					}
					else if(!string.IsNullOrEmpty(name)){ //no blank lines
						if(string.IsNullOrEmpty(brandName)){
							genericList.Add(name, new GenericExcludedDrug(name,"",criteria));
							lastGenericDrug = name;
						}
						else{
							if(brandName.Contains(",")){
								string[] brandNameArray = brandName.Split(',');
								brandList.Add(brandNameArray[0], new BrandExcludedDrug(name, brandNameArray[0], criteria));
								excludedBrandNameList.Add(brandNameArray[0]);
								foreach(string brandNameSingle in brandNameArray){
									excludedBrandNameList.Add(brandNameSingle);
									// if brand name already exists, add just the
									// generic name to the list
									if(excludedBrandNameList.Contains(brandNameSingle.Trim())){
										BrandDrug brand;
										if(brandList.TryGetValue(brandNameSingle.Trim(), out brand))
											((BrandExcludedDrug)brand).addGenericName(name);
									}
									else{
										brandList.Add(brandName, new BrandExcludedDrug(name, brandNameSingle.Trim(), criteria));
										excludedBrandNameList.Add(brandNameSingle.Trim());
										genericList.Add(name, new GenericExcludedDrug(name, brandNameSingle, criteria));
										lastGenericDrug = name; // sets the last
																// drug if next line
																// is extra criteria
									}
								}
								lastBrandDrug = brandNameArray[0];
							}
							else{
								if(brandList.ContainsKey(brandName)){
									BrandDrug brand;
									if(brandList.TryGetValue(brandName, out brand) && ((BrandExcludedDrug)brand).status.Equals(UIProperties.Excluded))
										((BrandExcludedDrug)brand).addGenericName(name);
								}
								else{
									brandList.Add(brandName, new BrandExcludedDrug(name, brandName, criteria));
									excludedBrandNameList.Add(brandName);
									lastBrandDrug = brandName;
									if(!genericList.ContainsKey(name)){
										genericList.Add(name, new GenericExcludedDrug(name, brandName, criteria));
										lastGenericDrug = name;
									}
									else{
										GenericDrug generic;
										if(genericList.TryGetValue(name, out generic) && (generic.status.Equals(UIProperties.Excluded)))
											generic.addBrandName(brandName);
									}
								}
							}
						}
					}
				}
				stream.Close();
			} catch(Exception e){
				Console.WriteLine ("Excluded parser failed due to {0}", e.Message);

			}
			finally{
				stream = null;
			}
		}

		public void ParseRestricted(string csvFile){
			//convert string to stream
			var stream = StringToStream(csvFile);
			var parser = new CsvParser (stream);

			try{
				string lastGenericDrug = null;
				string lastBrandDrug = null;
				List<string> restrictedBrandNameList = new List<string>();

				string[] nextLine = parser.Read(); //skip title line
				while((nextLine = parser.Read()) != null){
					string name = nextLine[0].ToUpper();
					string brandName = nextLine[1].ToUpper();
					string criteria = nextLine[2].ToUpper().Trim();
					//extraline for restricted criteria
					if(string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(criteria)){
						GenericDrug generic;
						if (genericList.TryGetValue(lastGenericDrug, out generic))
							((GenericRestrictedDrug)generic).additionalCriteria(criteria);
						BrandDrug brand;
						if(brandList.TryGetValue(lastBrandDrug, out brand))
							((BrandRestrictedDrug)brand).additionalCriteria(criteria);
					}
					else if(!string.IsNullOrEmpty(name)){ //no blank lines
						if(string.IsNullOrEmpty(brandName)){
							genericList.Add(name, new GenericRestrictedDrug(name,"",criteria));
							lastGenericDrug = name;
						}
						else{
							if(brandName.Contains(",")){
								string[] brandNameArray = brandName.Split(',');
								brandList.Add(brandNameArray[0], new BrandRestrictedDrug(name, brandNameArray[0], criteria));
								restrictedBrandNameList.Add(brandNameArray[0]);
								foreach(string brandNameSingle in brandNameArray){
									restrictedBrandNameList.Add(brandNameSingle);
									// if brand name already exists, add just the
									// generic name to the list
									if(restrictedBrandNameList.Contains(brandNameSingle.Trim())){
										BrandDrug brand;
										if(brandList.TryGetValue(brandNameSingle.Trim(), out brand))
											brand.addGenericName(name);
									}
									else{
										brandList.Add(brandName, new BrandRestrictedDrug(name, brandNameSingle.Trim(), criteria));
										restrictedBrandNameList.Add(brandNameSingle.Trim());
										genericList.Add(name, new GenericRestrictedDrug(name, brandNameSingle, criteria));
										lastGenericDrug = name; // sets the last
										// drug if next line
										// is extra criteria
									}
								}
								lastBrandDrug = brandNameArray[0];
							}
							else{
								if(brandList.ContainsKey(brandName)){
									BrandDrug brand;
									if(brandList.TryGetValue(brandName, out brand) && (brand.status.Equals(UIProperties.Restricted)))
										brand.addGenericName(name);
								}
								else{
									brandList.Add(brandName, new BrandRestrictedDrug(name, brandName, criteria));
									restrictedBrandNameList.Add(brandName);
									lastBrandDrug = brandName;
									if(!genericList.ContainsKey(name)){
										genericList.Add(name, new GenericRestrictedDrug(name, brandName, criteria));
										lastGenericDrug = name;
									}
									else{
										GenericDrug generic;
										if(genericList.TryGetValue(name, out generic) && (generic.status.Equals(UIProperties.Restricted)))
											generic.addBrandName(brandName);
									}
								}
							}
						}
					}
				}
				stream.Close();
			} catch(Exception e){
				Console.WriteLine ("Restricted parser failed due to {0}", e.Message);
			}
			finally{
				stream = null;
			}
		}

		private static StreamReader StringToStream(string rawString){
			byte[] byteArray = Encoding.ASCII.GetBytes(rawString);
			var stream = new MemoryStream(byteArray);
			return new StreamReader(stream);
		}

		private void addBrandNameToExistingFormularyDrug(string genericName, string brandName){
			GenericDrug genericDrug;
			if(genericList.TryGetValue(genericName, out genericDrug)){
				if (!genericDrug.containsBrandName (brandName))
					genericDrug.addBrandName (brandName);
			}
		}

		private void addGenericFormularyDrugWithBrandName(string genericName, string strength, string brandName){
			if (brandName.Contains (",")) {
				var brandNameArray = brandName.Split (',');
				genericList.Add (genericName, new GenericFormularyDrug (genericName, brandNameArray [0].Trim ().ToUpper (), strength));
				for (int i = 1; i < brandNameArray.Length; i++) {
					addBrandNameToExistingFormularyDrug (genericName, brandNameArray [i].Trim ());
				}
			} else {
				genericList.Add (genericName, new GenericFormularyDrug (genericName, brandName, strength));
			}
		}

		private void addBrandNameFormulary(string genericName, string brandName, string strength){
			if (brandList.ContainsKey (brandName)) {
				//add strengths
				BrandDrug brandDrug;
				brandList.TryGetValue (brandName, out brandDrug);
				((BrandFormularyDrug)brandDrug).addStrength (strength);

				//add generic name
				if(brandList.TryGetValue(brandName, out brandDrug)){
					if (!brandDrug.containsGenericName (genericName)) {
						brandList.TryGetValue (brandName, out brandDrug);
						brandDrug.addGenericName (genericName);
					}
				}
				} else {
				brandList.Add (brandName, new BrandFormularyDrug (genericName, brandName, strength));
			}
		}
	}
}


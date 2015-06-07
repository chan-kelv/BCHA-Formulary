using System;
using System.IO;
using CsvHelper;
using System.Text;
using System.Collections.Generic;

namespace BCHAFormulary
{
	public class CSVParser
	{
		Dictionary<string, GenericDrug> genericList;
		Dictionary<string, BrandDrug> brandList;

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

		public void ParseRestricted(string csvFile){
			// convert string to stream
			var stream = StringToStream(csvFile);

			var parser = new CsvParser (stream);
			while (parser.Read () != null) {
				try{
					Console.WriteLine ("row 0: {0}", parser.Read() [0]);
					Console.WriteLine ("row 1: {0}:", parser.Read() [1]);
					Console.WriteLine ("row 2: {0}", parser.Read() [2]);
				}
				catch(Exception e){
					Console.WriteLine ("Restriction Parser error {0}", e.Message);
				}
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


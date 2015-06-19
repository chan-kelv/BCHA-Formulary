using System;
using System.Text;

namespace BCHAFormulary
{
	public class GenericExcludedDrug : GenericDrug
	{
		public StringBuilder criteria;

		public GenericExcludedDrug (string genericName, string brandName, string criteria)
			:base (genericName, brandName, UIProperties.Excluded)
		{
			this.criteria = new StringBuilder (criteria);
		}

		public void additionalCriteria(string extraCriteria){
			StringBuilder extraAddition = new StringBuilder ();
			string punctuation = ".,':;<>/=()-";

			//Ignore beginning if first character is digit
			if (Char.IsDigit (extraCriteria [0])) {
				extraAddition.Append (extraCriteria [0]);
				extraAddition.Append (".  ");
			}

			//Add bullet
			else if (!(extraCriteria.Contains (":") || extraCriteria.Contains ("OR") && extraCriteria.Length <= 3)) {
				extraAddition.Append ("    -   ");
			}

			//find the first character
			int j = 0;
			bool foundFirstLetter = false;
			for (int i = 0; i < extraCriteria.Length; i++) {
				if (!(foundFirstLetter)) {
					if (!Char.IsLetter (extraCriteria [i])) {
						j++;
					} else
						foundFirstLetter = true;
				}
			}

			//append all from first letter to end
			char charToAdd;
			for (int k = j; k < extraCriteria.Length; k++) {
				charToAdd = extraCriteria [k];
				if (Char.IsLetterOrDigit (charToAdd) || Char.IsWhiteSpace (charToAdd) ||
					Char.IsSeparator (charToAdd) || punctuation.Contains (charToAdd.ToString()))
					extraAddition.Append (charToAdd);
				else
					extraAddition.Append ("'");
			}
			this.criteria.Insert (criteria.Length, ("\n\n" + extraAddition.ToString()));
		}
	}
}


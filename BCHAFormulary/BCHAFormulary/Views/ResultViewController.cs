using System;
using MonoTouch.Dialog;
using UIKit;
using Foundation;
using CoreGraphics;

namespace BCHAFormulary
{
	public class ResultViewController : DialogViewController
	{
		Drug drug;
		string drugName;

		public ResultViewController (Drug searchResult, string drugName) : base (UITableViewStyle.Grouped, null)
		{
			drug = searchResult;
			this.drugName = drugName;
		}

		public override void ViewDidLoad (){
			base.ViewDidLoad ();

			NavigationItem.LeftBarButtonItem = new UIBarButtonItem ("< Back", UIBarButtonItemStyle.Bordered, delegate {
				NavigationController.NavigationBar.Hidden = true;
				this.NavigationController.PopViewController (false);
			});
			NavigationItem.LeftBarButtonItem.TintColor = new UIColor (new CGColor (0, 0, 0));

			Root = new RootElement (drugName);

			#region formulary
			if(drug.status == "Formulary"){
				var alternateNamesString = string.Empty;
				string alternateType;
				if(drug.GetType() == typeof(GenericFormularyDrug)){
					alternateType = "Brand Name";

					var brandNameList = ((GenericFormularyDrug)drug).brandNames;
					foreach (string brand in brandNameList){
						alternateNamesString += string.Format("- {0} \n", brand);
					}
				}
				else{
					alternateType = "Generic Name";

					var genericNameList = ((BrandFormularyDrug)drug).genericNames;
					foreach(string grand in genericNameList){
						alternateNamesString += string.Format("- {0} \n", grand);
					}
				}
				if(!string.IsNullOrEmpty(alternateNamesString)){
					var alternateElement = new MultilineElement(alternateNamesString);
					var alternateSection = new Section(alternateType){
					};
					alternateSection.Add(alternateElement);

					Root.Add(alternateSection);
				}
			}
			#endregion
		}
	}
}


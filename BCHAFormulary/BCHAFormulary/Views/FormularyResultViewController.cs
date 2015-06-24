using System;
using MonoTouch.Dialog;
using UIKit;
using Foundation;
using System.Collections.Generic;

namespace BCHAFormulary
{
	[Register ("FormularyResultViewController")]
	public partial class FormularyResultViewController : DialogViewController
	{
		Drug formularyDrug;
		bool isGeneric = false;

		public FormularyResultViewController(IntPtr h) : base(h){}

		public FormularyResultViewController (Drug formularyDrug) :base(UITableViewStyle.Grouped, null, true)
		{
			this.formularyDrug = formularyDrug;
			if (formularyDrug.GetType () == typeof(GenericFormularyDrug))
				isGeneric = true;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			NavigationController.NavigationBar.TintColor = UIColor.Black;
			NavigationController.NavigationBar.BarTintColor = Colors.DarkBlue;
			NavigationController.NavigationBarHidden = false;
			string name;
			List<string> altNames;
			List<string> strenghts;

			if (isGeneric) {
				name = ((GenericFormularyDrug)formularyDrug).genericName;
				altNames = ((GenericFormularyDrug)formularyDrug).brandNames;
				strenghts = ((GenericFormularyDrug)formularyDrug).strengths;
			}
			else{
				name = ((BrandFormularyDrug)formularyDrug).brandName;
				altNames = ((BrandFormularyDrug)formularyDrug).genericNames;
				strenghts = ((BrandFormularyDrug)formularyDrug).strengths;
			}
			Root = new RootElement (name);
			var altSection = new Section ("Alternative names");
			foreach (string altName in altNames) {
				var altElement = new StringElement (altName);
				altSection.Add (altElement);
			}
			var statusSection = new Section ("Status") {
				new StringElement ("FORMULARY")
			};

			var strengthSection = new Section ("Strengths");
			foreach (string strength in strenghts) {
				var strengthString = "- " + strength;
				var strengthElement = new StringElement (strengthString);
				strengthSection.Add (strengthElement);
			}
			Root.Add (altSection);
			Root.Add (statusSection);
			Root.Add (strengthSection);
		}
	}
}


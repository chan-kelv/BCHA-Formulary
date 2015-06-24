using System;
using MonoTouch.Dialog;
using UIKit;
using Foundation;
using System.Collections.Generic;

namespace BCHAFormulary
{
	[Register("RestrictedResultViewController")]
	public partial class RestrictedResultViewController : DialogViewController
	{
		Drug restrictedDrug;
		bool isGeneric;

		public RestrictedResultViewController(IntPtr h) : base(h){}

		public RestrictedResultViewController (Drug restrictedDrug) :base(UITableViewStyle.Grouped, null, true)
		{
			this.restrictedDrug = restrictedDrug;
			if (restrictedDrug.GetType () == typeof(GenericRestrictedDrug))
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
			string criteria;

			if (isGeneric) {
				name = ((GenericRestrictedDrug)restrictedDrug).genericName;
				altNames = ((GenericRestrictedDrug)restrictedDrug).brandNames;
				criteria = ((GenericRestrictedDrug)restrictedDrug).criteria.ToString();
			}
			else{
				name = ((BrandRestrictedDrug)restrictedDrug).brandName;
				altNames = ((BrandRestrictedDrug)restrictedDrug).genericNames;
				criteria = ((BrandRestrictedDrug)restrictedDrug).criteria.ToString();
			}
			var rootElement = new RootElement (name);
			rootElement.UnevenRows = true;
			Root = rootElement;

			//name section
			if (altNames != null) {
				var altSection = new Section ("Alternative names");
				foreach (string altName in altNames) {
					var altElement = new StringElement (altName);
					altSection.Add (altElement);
				}
				Root.Add (altSection);
			}

			//Restricted tag section
			var statusElement = new StyledStringElement ("RESTRICTED");
			statusElement.TextColor = (Colors.Red);
			statusElement.BackgroundColor = UIColor.White;
			statusElement.Font = UIFont.BoldSystemFontOfSize (18);
			Root.Add (new Section ("Status"){ statusElement });

			//Reason section
			var excludedReasonElement = new StyledMultilineElement (UIProperties.ExcludedReason);
			excludedReasonElement.Font = UIFont.SystemFontOfSize (13);
			excludedReasonElement.BackgroundColor = UIColor.White;
			Root.Add (new Section ("Excluded Reason"){ excludedReasonElement });

			//var criteria
			if (!string.IsNullOrEmpty (criteria)) {
				var criteriaElement = new MultilineElement (criteria);
				var criteriaSection = new Section ("Reason For Exclusion"){ criteriaElement };
				Root.Add (criteriaSection);
			}
		}
	}
}


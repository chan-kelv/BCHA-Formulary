using System;
using MonoTouch.Dialog;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Linq;

namespace BCHAFormulary
{
	[Register("ExcludedResultViewController")]
	public partial class ExcludedResultViewController : DialogViewController
	{
		Drug excludedDrug;
		bool isGeneric = false;

		public ExcludedResultViewController (IntPtr h) : base(h){}

		public ExcludedResultViewController(Drug excludedDrug) : base(UITableViewStyle.Grouped, null, true)
		{
			this.excludedDrug = excludedDrug;
			if (excludedDrug.GetType () == typeof(GenericExcludedDrug))
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
				name = ((GenericExcludedDrug)excludedDrug).genericName;
				altNames = ((GenericExcludedDrug)excludedDrug).brandNames;
				criteria = ((GenericExcludedDrug)excludedDrug).criteria.ToString();
			}
			else{
				name = ((BrandExcludedDrug)excludedDrug).brandName;
				altNames = ((BrandExcludedDrug)excludedDrug).genericNames;
				criteria = ((BrandExcludedDrug)excludedDrug).criteria.ToString();
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

			//excluded label
//			var excludedReason = new ExcludedReason();
//			var excludedReasonElement = new UIViewElement (string.Empty, excludedReason, false){Flags = UIViewElement.CellFlags.DisableSelection};
//			var excludedSection = new Section ("Status"){ excludedReasonElement };
//			Root.Add(excludedSection);

			var statusElement = new StyledStringElement ("EXCLUDED");
			statusElement.TextColor = (Colors.Red);
			statusElement.BackgroundColor = UIColor.White;
			statusElement.Font = UIFont.BoldSystemFontOfSize (18);
			Root.Add (new Section ("Status"){ statusElement });

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
		public override void ViewWillLayoutSubviews()
		{
			if (TableView != null && TableView.VisibleCells.Any())
			{
				foreach (var cell in TableView.VisibleCells)
				{
					cell.SizeToFit();
				}
			}
			base.ViewWillLayoutSubviews();
		}
	}
}


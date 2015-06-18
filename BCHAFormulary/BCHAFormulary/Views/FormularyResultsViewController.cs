using System;
using MonoTouch.Dialog;
using UIKit;
using Foundation;

namespace BCHAFormulary
{
	public class FormularyResultsViewController : DialogViewController
	{
		Drug formularyDrug;
		bool isGeneric = false;

		public FormularyResultsViewController (Drug formularyDrug) : base(UITableViewStyle.Grouped, null)
		{
			this.formularyDrug = formularyDrug;

			if (formularyDrug. == typeof(GenericDrug))
				isGeneric = true;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


		}
	}
}


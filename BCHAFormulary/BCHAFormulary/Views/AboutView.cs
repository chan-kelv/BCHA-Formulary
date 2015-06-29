
using System;

using Foundation;
using UIKit;
using CoreGraphics;

namespace BCHAFormulary
{
	public partial class AboutView : UIViewController
	{
		public AboutView () : base ("AboutView", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Title = "About the App";
			NavigationController.NavigationBar.TintColor = UIColor.Black;
			NavigationController.NavigationBar.BarTintColor = Colors.DarkBlue;
			NavigationController.NavigationBarHidden = false;

			scrollView.ContentSize = new CGSize (320, 1350);

			txtAboutSearch.Text = "- Formulary drugs: all strengths and formulations available in FH \n- Restricted drugs: restriction criteria for approved indications, populations, patient care areas, and prescribers in FH \n- Excluded drugs: reason for BCHA provincial formulary exclusion";
			txtAboutSearch.Lines = 0;

			btnEmail.TouchUpInside += (object sender, EventArgs e) => {
				UIApplication.SharedApplication.OpenUrl(new NSUrl("mailto:chan.kelv@gmail.com"));
			};

			txtSearchTips.Text = "- Combination drug products can be found by searching for any individual component alone or for the brand name of the entire product. \n- A full list of all vaccines will appear when you search for \" vaccine\". Similarly for \"multivitamins\" and \"lipid\" \n- If you are unable to find a medication, use the \"Download Formulary\" button to browse a list of all formulary, restricted, and excluded drugs";
			txtSearchTips.Lines = 0;
		}
	}
}


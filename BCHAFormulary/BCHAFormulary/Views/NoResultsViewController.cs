
using System;

using Foundation;
using UIKit;

namespace BCHAFormulary
{
	public partial class NoResultsViewController : UIViewController
	{
		string drugInput;

		public NoResultsViewController (string input) : base ("NoResultsViewController", null)
		{
			drugInput = input.ToUpper();
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
			NavigationController.NavigationBar.TintColor = UIColor.Black;
			NavigationController.NavigationBar.BarTintColor = Colors.DarkBlue;
			NavigationController.NavigationBarHidden = false;

//			NavigationItem.LeftBarButtonItem = new UIBarButtonItem ("<", UIBarButtonItemStyle.Plain, delegate {
//				this.NavigationController.PopViewController (false);
//			});
			if (!string.IsNullOrEmpty (drugInput))
				txtDrugLabel.Text = string.Format ("Sorry, {0} could not be found", drugInput);

			btnViewFormulary.TouchUpInside += delegate {
//				UIApplication.SharedApplication.OpenUrl(new NSUrl("http://www.dropbox.com/s/3qdfzzfeucp83nt/formulary.csv?dl=0")); //TODO replace with real link
				var webView = new UIWebView(View.Bounds);
				View.AddSubview(webView);
				webView.LoadRequest(new NSUrlRequest(new NSUrl("http://www.dropbox.com/s/3qdfzzfeucp83nt/formulary.csv?dl=0")));
				webView.ScalesPageToFit = true;
			};
			// Perform any additional setup after loading the view, typically from a nib.
		}
	}
}


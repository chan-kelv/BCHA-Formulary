
using System;

using Foundation;
using UIKit;
using CoreGraphics;
using MBProgressHUD;
using System.Threading.Tasks;

namespace BCHAFormulary
{
	public partial class DrugSearchView : UIViewController
	{
		WebHelper webHelper = new WebHelper();
		FileAccessHelper updateFile = new FileAccessHelper(Environment.SpecialFolder.MyDocuments, "update.txt");

		public DrugSearchView () : base ("DrugSearchView", null)
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
			
			// Perform any additional setup after loading the view, typically from a nib.
			NavigationController.SetNavigationBarHidden(hidden:true, animated:false);
			txtDrugInput.Layer.BorderColor = new CGColor(255,165,0);

			#region update files

			var hud = new MTMBProgressHUD (View) {
				LabelText = "Updating formulary",
				RemoveFromSuperViewOnHide = true
			};
			View.AddSubview (hud);
			hud.Show (true);
			//do all updating here

			string data = null;


			Task.Factory.StartNew( delegate {
				data = webHelper.webGet(Uri.updateEndpoint);
			}).ContinueWith(task =>{
				if (data == null){
					new UIAlertView("Update error", "There was an error updating lists, please try again later", null, "OK");
				}
				else{
					//TODO parse data
					Console.WriteLine(data);
					bool saveStatus = updateFile.saveFile(data);
					if (!saveStatus)
						Console.WriteLine("An error has occured saving the file");
				}
			}, TaskScheduler.FromCurrentSynchronizationContext());
			hud.Hide (true);

			#endregion

			btnSearch.TouchUpInside += delegate {
				Console.WriteLine(updateFile.loadFile());
			};
		}
	}
}


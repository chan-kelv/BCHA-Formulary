
using System;

using Foundation;
using UIKit;
using CoreGraphics;
using MBProgressHUD;
using System.Threading.Tasks;
using System.Text;
using System.IO;


namespace BCHAFormulary
{
	public partial class DrugSearchView : UIViewController
	{
		WebHelper webHelper = new WebHelper();
		FileAccessHelper updateFile = new FileAccessHelper(Environment.SpecialFolder.MyDocuments, "update.txt");
		CSVParser masterList = new CSVParser();

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

			//make text 
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
			string restrictionData = null;
			string excludedData = null;
			string formularyData = null;
			Task.Factory.StartNew( delegate {
				data = webHelper.webGet(Uri.updateEndpoint);
				formularyData = webHelper.webGet(Uri.formularyEndpoint);
				excludedData = webHelper.webGet(Uri.excludedEndpoint);
				restrictionData = webHelper.webGet(Uri.restrictedEndpoint);
			}).ContinueWith(task =>{
				if (data == null){
					new UIAlertView("Update error", "There was an error updating lists, please try again later", null, "OK").Show();
				}
				else{
					//TODO parse data
					Console.WriteLine(data);
					bool saveStatus = updateFile.saveFile(data);
					if (!saveStatus)
						Console.WriteLine("An error has occured saving the file");
				}

				if(formularyData == null)
					new UIAlertView("Formulary update error", "There was an error updating lists, please try again later", null, "OK").Show();
				else{
					masterList.ParseFormulary(formularyData);
				}
				if (restrictionData == null)
					new UIAlertView("Restriction update error", "There was an error updating lists, please try again later", null, "OK").Show();
				else{
					masterList.ParseExcluded(excludedData);
				}
				if (restrictionData == null)
					new UIAlertView("Restriction update error", "There was an error updating lists, please try again later", null, "OK").Show();
				else{
					masterList.ParseRestricted(restrictionData);
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


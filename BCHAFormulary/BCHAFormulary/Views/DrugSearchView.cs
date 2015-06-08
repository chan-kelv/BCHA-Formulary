
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
		FileAccessHelper formularyFile = new FileAccessHelper(Environment.SpecialFolder.MyDocuments, "formulary.txt");
		FileAccessHelper excludedFile = new FileAccessHelper(Environment.SpecialFolder.MyDocuments, "excluded.txt");
		FileAccessHelper restrictedFile = new FileAccessHelper(Environment.SpecialFolder.MyDocuments, "restricted.txt");

		string updateData;
		string restrictionData;
		string excludedData;
		string formularyData;

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

			//try to load any saved data sets
			updateData = updateFile.loadFile();
			formularyData = formularyFile.loadFile();
			excludedData = excludedFile.loadFile();
			restrictionData = restrictedFile.loadFile();

			//check if network is available
			if(webHelper.isConnected()){//phone is connected to a network
				var currVersion = updateFile.loadFile(); //check if files need to be updated
				updateData = webHelper.webGet(Uri.updateEndpoint);
				if(string.IsNullOrEmpty(currVersion) || !currVersion.Equals(updateData)){ //needs to be updated
					Task.Factory.StartNew( delegate {
						formularyData = webHelper.webGet(Uri.formularyEndpoint);
						excludedData = webHelper.webGet(Uri.excludedEndpoint);
						restrictionData = webHelper.webGet(Uri.restrictedEndpoint);
					}).ContinueWith(task =>{
						if(formularyData == null)
							new UIAlertView("Formulary update error", "There was an error updating lists, please try again later", null, "OK").Show();
						else
							masterList.ParseFormulary(formularyData);
						if (restrictionData == null)
							new UIAlertView("Restriction update error", "There was an error updating lists, please try again later", null, "OK").Show();
						else
							masterList.ParseExcluded(excludedData);
						if (restrictionData == null)
							new UIAlertView("Restriction update error", "There was an error updating lists, please try again later", null, "OK").Show();
						else
							masterList.ParseRestricted(restrictionData);

						//if all 3 datasets are not null, files have correctly been updated, save the data set
						if(!string.IsNullOrEmpty(formularyData) && !string.IsNullOrEmpty(excludedData) && !string.IsNullOrEmpty(restrictionData)){
							updateFile.saveFile(updateData);
							formularyFile.saveFile(formularyData);
							excludedFile.saveFile(excludedData);
							restrictedFile.saveFile(restrictionData);
						}
						hud.Hide (true);
					}, TaskScheduler.FromCurrentSynchronizationContext());
				}
				else{ //no update is needed, parse files saved
					LoadListOffline();
					hud.Hide(true);
				}
			}

			else{ //phone is not connected to a network
				LoadListOffline();
				hud.Hide(true);
			}


			#endregion

			btnSearch.TouchUpInside += delegate {
				if(webHelper.isConnected())
					Console.WriteLine("phone is online");
				Console.WriteLine("Generic drug count total {0}", masterList.genericList.Count);
			};
		}

		private void LoadListOffline(){
			//datasets are either default/loaded files/new files at this point
			if(string.IsNullOrEmpty(formularyData)){ //use default if no saved file found
				formularyData = File.ReadAllText("formulary.csv");
			}
			if(string.IsNullOrEmpty(excludedData)){ //use default if no saved file found
				excludedData = File.ReadAllText("excluded.csv");
				//					masterList.ParseFormulary(excludedData);
			}
			if(string.IsNullOrEmpty(restrictionData)){ //use default if no saved file found
				restrictionData = File.ReadAllText("restricted.csv");
				//					masterList.ParseFormulary(restrictionData);
			}

			//parse data 			
			masterList.ParseFormulary(formularyData);
			masterList.ParseExcluded(excludedData);
			masterList.ParseRestricted(restrictionData);
		}
	}
}


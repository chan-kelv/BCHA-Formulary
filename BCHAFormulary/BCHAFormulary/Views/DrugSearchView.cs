
using System;

using Foundation;
using UIKit;
using CoreGraphics;
using MBProgressHUD;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading;


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
		string[] masterDrugNameList;

		UITableView autoCompleteTable;

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
//			NavigationController.SetNavigationBarHidden(hidden:true, animated:false);
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

							//turns all keys in the masterDrugList into a string[] of just the names
							generateAllDrugNames ();
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


			//dismiss focus when click outside the keyboard
			var tap = new UITapGestureRecognizer ();
			tap.AddTarget (() => View.EndEditing (true));
			View.AddGestureRecognizer (tap);
			tap.CancelsTouchesInView = false;

			//dismiss the keyboard when hit return
			txtDrugInput.ShouldReturn += (textField) => {
				textField.ResignFirstResponder();
				return true;
			};

			autoCompleteTable = new UITableView (new CGRect (8, 205, 320, 100));
			autoCompleteTable.ScrollEnabled = true;
			autoCompleteTable.Hidden = true;
			View.AddSubview (autoCompleteTable);

			NSNotificationCenter.DefaultCenter.AddObserver
			(UITextField.TextFieldTextDidChangeNotification, (notification) =>
				{
					Console.WriteLine ("Character received! {0}", notification.Object ==
						txtDrugInput);
					UpdateSuggestion();
				});

			//handle search button
			btnSearch.TouchUpInside += delegate {
				GenericDrug genericSearchDrug;
				BrandDrug brandSearchDrug;
				UIViewController resultView = null;
				if(masterList.genericList.TryGetValue(txtDrugInput.Text.ToUpper(), out genericSearchDrug)){
					var genericType = genericSearchDrug.GetType();
					if (genericType == typeof(GenericFormularyDrug))
						resultView = new FormularyResultViewController(genericSearchDrug);
					else if (genericType == typeof(GenericExcludedDrug))
						resultView = new ExcludedResultViewController(genericSearchDrug);
					else if(genericType == typeof(GenericRestrictedDrug))
						resultView = new RestrictedResultViewController(genericSearchDrug);
				}
				else if(masterList.brandList.TryGetValue(txtDrugInput.Text.ToUpper(), out brandSearchDrug)){
					var brandType = brandSearchDrug.GetType();
					if(brandType == typeof(BrandFormularyDrug))
						resultView = new FormularyResultViewController(brandSearchDrug);
					else if(brandType == typeof(BrandExcludedDrug))
						resultView = new ExcludedResultViewController(brandSearchDrug);
					else if(brandType == typeof(BrandRestrictedDrug))
						resultView = new RestrictedResultViewController(brandSearchDrug);
				}

				if(resultView == null)
					resultView = new NoResultsViewController(txtDrugInput.Text.ToUpper());

				this.NavigationController.PushViewController(resultView, true);
			};

			btnAbout.TouchUpInside += (object sender, EventArgs e) => {
				this.NavigationController.PushViewController(new AboutView(), true);
			};
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
			NavigationController.NavigationBarHidden = true;

		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			ReleaseDesignerOutlets ();
		}

		private void LoadListOffline(){
			//datasets are either default/loaded files/new files at this point

			//use default if no saved file found
			if(string.IsNullOrEmpty(formularyData)){ 
				formularyData = File.ReadAllText("formulary.csv");
			}
			//use default if no saved file found
			if(string.IsNullOrEmpty(excludedData)){ 
				excludedData = File.ReadAllText("excluded.csv");

			}
			//use default if no saved file found
			if(string.IsNullOrEmpty(restrictionData)){ 
				restrictionData = File.ReadAllText("restricted.csv");
			}

			//parse data 			
			masterList.ParseFormulary(formularyData);
			masterList.ParseExcluded(excludedData);
			masterList.ParseRestricted(restrictionData);
		}

		private void UpdateSuggestion(string inputText = null){
			string[] suggestions = null;
			var txtField = txtDrugInput.Text;
			try{
//				InvokeOnMainThread(()=>{
				Console.WriteLine ("Input is {0}", txtField);
				if(string.IsNullOrEmpty(txtField))
						suggestions = null;
					else{
					suggestions = masterDrugNameList.Where(x => x.ToUpperInvariant().Contains(txtField.ToUpper()))
						.OrderByDescending(x => x.ToUpperInvariant().StartsWith(txtField.ToUpper()))
							.Select (x => x).ToArray();
					}
				if (suggestions!= null && suggestions.Length != 0) {
						autoCompleteTable.Hidden = false;
						autoCompleteTable.Source = new AutoCompleteTableSource (suggestions, this);
						autoCompleteTable.ReloadData ();
				} else {
						autoCompleteTable.Hidden = true;
				}
			}
			catch(Exception e){
				Console.WriteLine ("No suggestions due to {0}", e.Message);
			}
		}

		public void SetAutoCompleteText(string finalString) {
			txtDrugInput.Text = finalString;
			txtDrugInput.ResignFirstResponder();
			autoCompleteTable.Hidden = true;
		}

		private void generateAllDrugNames(){
			var list1 = masterList.genericList.Keys.ToArray();
			var list2 = masterList.brandList.Keys.ToArray();
			if(list1 != null && list2 != null)
				masterDrugNameList = list1.Concat (list2).ToArray ();
		}

	}
}


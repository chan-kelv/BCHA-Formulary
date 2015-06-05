using System;

using UIKit;
using RestSharp;
using System.Threading.Tasks;
using MBProgressHUD;
using System.IO;

namespace BCHAFormulary
{
	public partial class ViewController : UIViewController
	{
		WebHelper webHelper = new WebHelper();
		FileAccessHelper updateFile = new FileAccessHelper(Environment.SpecialFolder.MyDocuments, "update.txt");

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.

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

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


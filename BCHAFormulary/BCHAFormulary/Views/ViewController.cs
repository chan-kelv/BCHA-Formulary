using System;

using UIKit;
using RestSharp;
using System.Threading.Tasks;

namespace BCHAFormulary
{
	public partial class ViewController : UIViewController
	{
		WebHelper webHelper = new WebHelper();

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			btnSearch.TouchUpInside += delegate {
				string data = null;

				Task.Factory.StartNew( delegate {
					data = webHelper.webGet(Uri.excludedEndpoint);
				}).ContinueWith(task =>{
					if (data == null){
						new UIAlertView("Update error", "There was an error updating lists, please try again later", null, "OK");
					}
					else{
						//TODO parse data
						Console.WriteLine(data);
					}
				}, TaskScheduler.FromCurrentSynchronizationContext());

			};
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


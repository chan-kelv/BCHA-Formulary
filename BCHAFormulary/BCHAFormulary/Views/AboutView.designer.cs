// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace BCHAFormulary
{
	[Register ("AboutView")]
	partial class AboutView
	{
		[Outlet]
		UIKit.UIButton btnEmail { get; set; }

		[Outlet]
		UIKit.UIScrollView scrollView { get; set; }

		[Outlet]
		UIKit.UILabel txtAboutSearch { get; set; }

		[Outlet]
		UIKit.UILabel txtSearchTips { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}

			if (txtAboutSearch != null) {
				txtAboutSearch.Dispose ();
				txtAboutSearch = null;
			}

			if (txtSearchTips != null) {
				txtSearchTips.Dispose ();
				txtSearchTips = null;
			}

			if (btnEmail != null) {
				btnEmail.Dispose ();
				btnEmail = null;
			}
		}
	}
}

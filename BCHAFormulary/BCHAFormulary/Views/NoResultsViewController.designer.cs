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
	[Register ("NoResultsViewController")]
	partial class NoResultsViewController
	{
		[Outlet]
		UIKit.UIButton btnViewFormulary { get; set; }

		[Outlet]
		UIKit.UILabel txtDrugLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txtDrugLabel != null) {
				txtDrugLabel.Dispose ();
				txtDrugLabel = null;
			}

			if (btnViewFormulary != null) {
				btnViewFormulary.Dispose ();
				btnViewFormulary = null;
			}
		}
	}
}

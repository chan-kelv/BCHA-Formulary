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
	[Register ("DrugSearchView")]
	partial class DrugSearchView
	{
		[Outlet]
		UIKit.UIButton btnAbout { get; set; }

		[Outlet]
		UIKit.UIButton btnSearch { get; set; }

		[Outlet]
		UIKit.UITextField txtDrugInput { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnSearch != null) {
				btnSearch.Dispose ();
				btnSearch = null;
			}

			if (btnAbout != null) {
				btnAbout.Dispose ();
				btnAbout = null;
			}

			if (txtDrugInput != null) {
				txtDrugInput.Dispose ();
				txtDrugInput = null;
			}
		}
	}
}

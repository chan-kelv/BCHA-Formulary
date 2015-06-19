using System;
using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace BCHAFormulary
{
	[Register("ExcludedReason")]
	public partial class ExcludedReason : UIView
	{
		public ExcludedReason(IntPtr h): base(h)
		{
		}

		public ExcludedReason ()
		{
			var arr = NSBundle.MainBundle.LoadNib("ExcludedReason", this, null);
			var v = Runtime.GetNSObject(arr.ValueAt(0)) as ExcludedReason;
			v.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);

			v.Frame = new CGRect (0, 0, 320, 345);
			this.Frame = new CGRect (0, 0, 320, 345);

			AddSubview(v);
		}
	}
}


using System;
using UIKit;

namespace BCHAFormulary
{
	public static class UIProperties
	{
		public static readonly string Formulary = "Formulary";
		public static readonly string Excluded = "Excluded";
		public static readonly string Restricted = "Restricted";
		public static readonly string ExcludedReason = "Excluded drugs have been assessed " +
			"by the B.C. Health Authorities Pharmacy and Therapeutics Committee and intentionally " +
			"excluded from the formulary based on the following reasons: the product has not been " +
			"shown to be more effective and/or safe than current formulary alternatives, is " +
			"unavailable or discontinued by the manufacturer, has unfavorable cost-benefit profile, or " +
			"other specific reason.\nPatients may be instructed to provide their own medication " +
			"in these cases as these drugs are not stocked in hospital. In exceptional cases, excluded drugs " +
			"may be supplied by pharmacy if deemed appropriate after assessment by pharmacy manager and/or " +
			"clinical pharmacist.";
   
	}

	public static class Colors{
		public static UIColor DarkBlue = UIColor.FromRGB(0,82,147);
		public static UIColor Red = UIColor.FromRGB(204,0,0);
	}
}


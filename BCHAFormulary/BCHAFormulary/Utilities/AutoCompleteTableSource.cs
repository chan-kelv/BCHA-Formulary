using System;
using Foundation;
using UIKit;

namespace BCHAFormulary {

	public class AutoCompleteTableSource : UITableViewSource {
		protected string[] suggestions;
		protected string cellIdentifier = "suggestionCell";

		DrugSearchView controllerContext;

		public AutoCompleteTableSource (string[] suggestions, DrugSearchView controllerContext) {
			this.suggestions = suggestions;
			this.controllerContext = controllerContext;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return (nint)suggestions.Length;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath) {
			controllerContext.SetAutoCompleteText(suggestions[indexPath.Row]);
			tableView.DeselectRow (indexPath, true);
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath) {
			UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);

			var cellStyle = UITableViewCellStyle.Default;
			if (cell == null) {
				cell = new UITableViewCell (cellStyle, cellIdentifier);
			}
			cell.TextLabel.Text = suggestions[indexPath.Row];
			return cell;
		}
	}
}

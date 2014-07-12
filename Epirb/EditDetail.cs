using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Epirb.Core;

namespace Epirb {

	[Activity (Label = "Edit Info")]			
	public class ViewAllDetails: Activity {
		Detail detail = new Detail();
		Button cancelButton;
		EditText valueTextEdit;
		TextView nameTextEdit;
		Button saveButton;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			int detailID = Intent.GetIntExtra("DetailID", 0);
			if(detailID > 0) {
				detail = DetailManager.GetDetail(detailID);
			}

			SetContentView(Resource.Layout.EditDetail);
			nameTextEdit = FindViewById<TextView>(Resource.Id.DetailName);
			valueTextEdit = FindViewById<EditText>(Resource.Id.DetailValue);

			saveButton = FindViewById<Button>(Resource.Id.SaveButton);

			cancelButton = FindViewById<Button>(Resource.Id.CancelButton);

			nameTextEdit.Text = detail.Name; 
			valueTextEdit.Text = detail.Value;

			cancelButton.Click += (sender, e) => { Cancel(); };
			saveButton.Click += (sender, e) => { Save(); };
		}

		void Save()
		{
			detail.Name = nameTextEdit.Text;
			detail.Value = valueTextEdit.Text;
			DetailManager.SaveDetail(detail);
			Finish();
		}

		void Cancel()
		{
			Finish();
		}

	}
}
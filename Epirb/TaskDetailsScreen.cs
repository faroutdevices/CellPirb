using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Epirb.Core;

namespace Epirb {

	[Activity (Label = "Edit Info")]			
	public class VesselDetailDetailsScreen : Activity {
		VesselDetail detail = new VesselDetail();
		Button cancelButton;
		EditText notesTextEdit;
		TextView nameTextEdit;
		Button saveButton;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			
			int detailID = Intent.GetIntExtra("VesselDetailID", 0);
			if(detailID > 0) {
				detail = VesselDetailManager.GetVesselDetail(detailID);
			}

			SetContentView(Resource.Layout.VesselDetailDetails);
			nameTextEdit = FindViewById<TextView>(Resource.Id.NameText);
			notesTextEdit = FindViewById<EditText>(Resource.Id.NotesText);

			saveButton = FindViewById<Button>(Resource.Id.SaveButton);

			cancelButton = FindViewById<Button>(Resource.Id.CancelButton);

			nameTextEdit.Text = detail.Name; 
			notesTextEdit.Text = detail.Notes;

			cancelButton.Click += (sender, e) => { Cancel(); };
			saveButton.Click += (sender, e) => { Save(); };
		}

		void Save()
		{
			detail.Name = nameTextEdit.Text;
			detail.Notes = notesTextEdit.Text;
			VesselDetailManager.SaveVesselDetail(detail);
			Finish();
		}

		void Cancel()
		{
			Finish();
		}

	}
}
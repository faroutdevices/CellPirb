using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Epirb.Core;

namespace Epirb {

	[Activity (Label = "Tap a Line to Edit",  Icon="@drawable/ic_launcher")]			
	public class Edit : Activity {
		VesselDetailListAdapter detailList;
		IList<VesselDetail> details;
		Button backButton;
		ListView detailListView;
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView(Resource.Layout.Edit);

			detailListView = FindViewById<ListView> (Resource.Id.VesselDetailList);
			backButton = FindViewById<Button> (Resource.Id.BackButton);

			if(backButton != null) {
				backButton.Click += (sender, e) => {
					StartActivity(typeof(Main));
				};
			}

			if(detailListView != null) {
				detailListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
					var detailDetails = new Intent (this, typeof (VesselDetailDetailsScreen));
					detailDetails.PutExtra ("VesselDetailID", details[e.Position].ID);
					StartActivity (detailDetails);
				};
			}
		}
		
		protected override void OnResume ()
		{
			base.OnResume ();

			details = VesselDetailManager.GetVesselDetails();
			detailList = new VesselDetailListAdapter(this, details);
			detailListView.Adapter = detailList;
		}
	}
}
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Epirb.Core;

namespace Epirb {

	[Activity (Label = "Tap a Line to Edit",  Icon="@drawable/ic_launcher")]			
	public class AllDetails : Activity {
		DetailListAdapter detailList;
		IList<Detail> details;
		Button backButton;
		ListView detailListView;
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView(Resource.Layout.AllDetails);

			detailListView = FindViewById<ListView> (Resource.Id.DetailList);
			backButton = FindViewById<Button> (Resource.Id.BackButton);

			if(backButton != null) {
				backButton.Click += (sender, e) => {
					StartActivity(typeof(Main));
				};
			}

			if(detailListView != null) {
				detailListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
					var detailDetails = new Intent (this, typeof (ViewAllDetails));
					detailDetails.PutExtra ("DetailID", details[e.Position].ID);
					StartActivity (detailDetails);
				};
			}
		}
		
		protected override void OnResume ()
		{
			base.OnResume ();

			details = DetailManager.GetDetails();
			detailList = new DetailListAdapter(this, details);
			detailListView.Adapter = detailList;
		}
	}
}
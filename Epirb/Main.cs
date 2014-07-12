using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Locations;
using Android.Telephony;
using Epirb.Core;
//using Mindscape.Raygun4Net;

namespace Epirb
{
	[Activity (Label = "CellPIRB", MainLauncher = true)]
	public class Main : Activity, ILocationListener
	{
		TextView _contacts;
		TextView _smsMessageText1;
		TextView _textHeader;
		Location _currentLocation;
		LocationManager _locationManager;
		String _locationProvider;
		Button _editMyInfoButton;
		Button _backtomainscreen;
		ListView detailListView;
		IList<Detail> details;
		bool latLongHasBeenSet = false;

		protected override void OnCreate (Bundle bundle)
		{
			//RaygunClient.Attach("Nw6oiU/EsZypSRkoYWy5kA==");

			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Main);
			_textHeader = FindViewById<TextView>(Resource.Id.textHeader);
			_smsMessageText1 = FindViewById<TextView>(Resource.Id.sms_message_text1);
			_contacts = FindViewById<TextView>(Resource.Id.contacts);
			_editMyInfoButton = FindViewById<Button> (Resource.Id.edit_my_info_button);
			_backtomainscreen = FindViewById<Button> (Resource.Id.back_to_main_screen);
			detailListView = FindViewById<ListView> (Resource.Id.DetailList);

			FindViewById<TextView>(Resource.Id.get_help_button).Click += HelpButton_OnClick;

				if(_editMyInfoButton != null) {
					_editMyInfoButton.Click += (sender, e) => {
					StartActivity(typeof(AllDetails));
					};
				}

				if(_backtomainscreen != null) {
					_backtomainscreen.Click += (sender, e) => {
							StartActivity(typeof(Main));
						};
					}

			_backtomainscreen.Visibility = ViewStates.Gone;
					
			InitializeLocationManager();
		}

		protected override void OnResume()
		{
			base.OnResume();
			_locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);

			//TODO: revisit
			SetContactText (_currentLocation);
		}

		protected override void OnPause()
		{
			base.OnPause();
			_locationManager.RemoveUpdates(this);
		}

		void InitializeLocationManager()
		{
			_locationManager = (LocationManager)GetSystemService(LocationService);
			Criteria criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.Fine
			};
			IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

			if (acceptableLocationProviders.Any())
			{
				_locationProvider = acceptableLocationProviders.First();
			}
			else
			{
				_locationProvider = String.Empty;
			}
		}

		//async void HelpButton_OnClick(object sender, EventArgs eventArgs)
		private void HelpButton_OnClick(object sender, EventArgs eventArgs)
		{			
			details = DetailManager.GetDetails();
			string contact1 = details [0].Value.ToString ();
			string contact2 = details [1].Value.ToString ();
			string contact3 = details [2].Value.ToString ();
			string boatname = details [3].Value.ToString ();
			string boattype = details [4].Value.ToString ();
			string boatlength = details [5].Value.ToString ();
			string boatcolor = details [6].Value.ToString ();
			string passengers = details [7].Value.ToString ();

			string _Latitude = String.Empty;
			string _Longitude = String.Empty;

			if (_currentLocation == null) {
				//_getHelpResultText.Text = "Can't determine your location yet.";
				///return;
			} else {			
				_Latitude = String.Format ("{0}", _currentLocation.Latitude);
				_Longitude = String.Format ("{0}", _currentLocation.Longitude);
			}

			StringBuilder _smsMessage1 = new StringBuilder();
			_smsMessage1.Append ("Vessel in distress at:").AppendLine ();
			_smsMessage1.Append ("Lat: " + _Latitude).AppendLine ();
			_smsMessage1.Append ("Long: " + _Longitude).AppendLine ();
			_smsMessage1.Append ("Just testing, do NOT call Coast Guard").AppendLine ();

			StringBuilder _smsMessage2 = new StringBuilder();
			_smsMessage2.Append ("Vessel Name: " + boatname).AppendLine ();
			_smsMessage2.Append ("Type: " + boattype).AppendLine ();
			_smsMessage2.Append ("Length: " + boatlength).AppendLine ();
			_smsMessage2.Append ("Color: " + boatcolor).AppendLine ();
			_smsMessage2.Append ("Passengers: " + passengers).AppendLine ();

			//test for less than 140 characters!!!
			if (contact1.Length > 7) {
				SmsManager.Default.SendTextMessage (contact1, null, _smsMessage1.ToString (), null, null);
				SmsManager.Default.SendTextMessage (contact1, null, _smsMessage2.ToString (), null, null);
			}

			if (contact2.Length > 7) {
				SmsManager.Default.SendTextMessage (contact2, null, _smsMessage1.ToString (), null, null);
				SmsManager.Default.SendTextMessage (contact2, null, _smsMessage2.ToString (), null, null);
			}

			if (contact3.Length > 7) {
				SmsManager.Default.SendTextMessage (contact3, null, _smsMessage1.ToString (), null, null);
				SmsManager.Default.SendTextMessage (contact3, null, _smsMessage2.ToString (), null, null);
			}

			_textHeader.Text = "Help SMS was sent successfully!!";
			_smsMessageText1.Text = String.Empty;
			_contacts.Text = string.Empty;
			_editMyInfoButton.Visibility = ViewStates.Gone;
			_backtomainscreen.Visibility = ViewStates.Visible;
		}			

		public void OnLocationChanged(Location location)
		{
			_currentLocation = location;
//			if (_currentLocation == null)
//			{
//				_locationText.Text = "Waiting to determine your location...";
//			}
//			else
//			{
//				_locationText.Text = String.Format("{0},{1}", _currentLocation.Latitude, _currentLocation.Longitude);
//			}

			//TODO: So we're just setting this once;
			if (_currentLocation != null && latLongHasBeenSet == false) {

				SetContactText (location);
				latLongHasBeenSet = true;
			}

		}

		public void OnProviderDisabled(string provider) {}

		public void OnProviderEnabled(string provider) {}

		public void OnStatusChanged(string provider, Availability status, Bundle extras) {}

		private void SetContactText(Location location)
		{
			details = DetailManager.GetDetails();
			string contact1 = details [0].Value.ToString ();
			string contact2 = details [1].Value.ToString ();
			string contact3 = details [2].Value.ToString ();
			string boatname = details [3].Value.ToString ();
			string boattype = details [4].Value.ToString ();
			string boatlength = details [5].Value.ToString ();
			string boatcolor = details [6].Value.ToString ();
			string passengers = details [7].Value.ToString ();

			_currentLocation = location;

			string _Latitude;
			string _Longitude;

			if (_currentLocation == null)
			{
				_Latitude = "Obtaining location..";
				_Longitude = "Obtaining locationo..";
			}
			else
			{
				_Latitude = String.Format ("{0}", _currentLocation.Latitude);
				_Longitude = String.Format ("{0}", _currentLocation.Longitude);
			}				
				
			StringBuilder _contactsText = new StringBuilder();
			_contactsText.Append ("Contact 1: " + contact1).AppendLine ();
			_contactsText.Append ("Contact 2: " + contact2).AppendLine ();
			_contactsText.Append ("Contact 3: " + contact3).AppendLine ();
							
			StringBuilder _smsMessage1 = new StringBuilder();
			//_smsMessage1.Append ("Vessel in distress:").AppendLine ();
			_smsMessage1.Append ("Vessel Name: " + boatname).AppendLine ();
			_smsMessage1.Append ("Latitude: " + _Latitude).AppendLine ();
			_smsMessage1.Append ("Longitude: " + _Longitude).AppendLine ();


			StringBuilder _smsMessage2 = new StringBuilder();

			_smsMessage2.Append ("Type: " + boattype).AppendLine ();
			_smsMessage2.Append ("Length: " + boatlength).AppendLine ();
			_smsMessage2.Append ("Color: " + boatcolor).AppendLine ();
			_smsMessage2.Append ("Passengers: " + passengers).AppendLine ();
			_smsMessage2.Append ("Call 911 for Coast Guard").AppendLine ();

			_contacts.Text = _contactsText.ToString ();

			_smsMessage1.Append (_smsMessage2);

			_smsMessageText1.Text = _smsMessage1.ToString ();
		}
	}
}



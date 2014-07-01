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
using Tasky.Core;

namespace Epirb
{
	[Activity (Label = "Epirb", MainLauncher = true)]
	public class Main : Activity, ILocationListener
	{
		TextView _contacts;
		TextView _smsMessageText1;
		TextView _smsMessageText2;
		Location _currentLocation;
		LocationManager _locationManager;
		String _locationProvider;
		Button _editMyInfoButton;
		ListView taskListView;
		IList<Task> tasks;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Main);
			_smsMessageText1 = FindViewById<TextView>(Resource.Id.sms_message_text1);
			_smsMessageText2 = FindViewById<TextView>(Resource.Id.sms_message_text2);
			_contacts = FindViewById<TextView>(Resource.Id.contacts);
			_editMyInfoButton = FindViewById<Button> (Resource.Id.edit_my_info_button);
			taskListView = FindViewById<ListView> (Resource.Id.TaskList);

			FindViewById<TextView>(Resource.Id.get_help_button).Click += HelpButton_OnClick;

			// wire up add edit button handler
				if(_editMyInfoButton != null) {
					_editMyInfoButton.Click += (sender, e) => {
					StartActivity(typeof(Edit));
					};
				}
				
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
			tasks = TaskManager.GetTasks();
			string contact1 = tasks [0].Notes.ToString ();
			string contact2 = tasks [1].Notes.ToString ();
			string contact3 = tasks [2].Notes.ToString ();
			string boatname = tasks [3].Notes.ToString ();
			string boattype = tasks [4].Notes.ToString ();
			string boatlength = tasks [5].Notes.ToString ();
			string boatcolor = tasks [6].Notes.ToString ();
			string passengers = tasks [7].Notes.ToString ();

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

			_smsMessageText1.Text = "Help SMS was sent successfully!!";

			_contacts.Text = string.Empty;
			_smsMessageText2.Text = string.Empty;
		}			

		public void OnLocationChanged(Location location)
		{
//			_currentLocation = location;
//			if (_currentLocation == null)
//			{
//				_locationText.Text = "Waiting to determine your location...";
//			}
//			else
//			{
//				_locationText.Text = String.Format("{0},{1}", _currentLocation.Latitude, _currentLocation.Longitude);
//			}

			//TODO: maybe shouldn't update this so frequently!!!;
			SetContactText (location);
		}

		public void OnProviderDisabled(string provider) {}

		public void OnProviderEnabled(string provider) {}

		public void OnStatusChanged(string provider, Availability status, Bundle extras) {}

		private void SetContactText(Location location)
		{
			tasks = TaskManager.GetTasks();
			string contact1 = tasks [0].Notes.ToString ();
			string contact2 = tasks [1].Notes.ToString ();
			string contact3 = tasks [2].Notes.ToString ();
			string boatname = tasks [3].Notes.ToString ();
			string boattype = tasks [4].Notes.ToString ();
			string boatlength = tasks [5].Notes.ToString ();
			string boatcolor = tasks [6].Notes.ToString ();
			string passengers = tasks [7].Notes.ToString ();

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
			_smsMessage1.Append ("Vessel in distress at:").AppendLine ();
			_smsMessage1.Append ("Lat: " + _Latitude).AppendLine ();
			_smsMessage1.Append ("Long: " + _Longitude).AppendLine ();
			_smsMessage1.Append ("Call 911 for Coast Guard").AppendLine ();

			StringBuilder _smsMessage2 = new StringBuilder();
			_smsMessage2.Append ("Vessel Name: " + boatname).AppendLine ();
			_smsMessage2.Append ("Type: " + boattype).AppendLine ();
			_smsMessage2.Append ("Length: " + boatlength).AppendLine ();
			_smsMessage2.Append ("Color: " + boatcolor).AppendLine ();
			_smsMessage2.Append ("Passengers: " + passengers).AppendLine ();

			_contacts.Text = _contactsText.ToString ();
			_smsMessageText1.Text = _smsMessage1.ToString ();
			_smsMessageText2.Text = _smsMessage2.ToString ();
		}
	}
}



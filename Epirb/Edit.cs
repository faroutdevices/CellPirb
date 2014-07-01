using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Tasky.Core;

namespace Epirb {
	/// <summary>
	/// Main ListView screen displays a list of tasks, plus an [Add] button
	/// </summary>
	[Activity (Label = "Edit Info",  Icon="@drawable/ic_launcher")]			
	public class Edit : Activity {
		TaskListAdapter taskList;
		IList<Task> tasks;
		Button backButton;
		ListView taskListView;
		
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// set our layout to be the home screen
			SetContentView(Resource.Layout.Edit);

			//Find our controls
			taskListView = FindViewById<ListView> (Resource.Id.TaskList);
			backButton = FindViewById<Button> (Resource.Id.BackButton);

			// wire up add task button handler
			if(backButton != null) {
				backButton.Click += (sender, e) => {
					StartActivity(typeof(Main));
				};
			}
			
			// wire up task click handler
			if(taskListView != null) {
				taskListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
					var taskDetails = new Intent (this, typeof (TaskDetailsScreen));
					taskDetails.PutExtra ("TaskID", tasks[e.Position].ID);
					StartActivity (taskDetails);
				};
			}
		}
		
		protected override void OnResume ()
		{
			base.OnResume ();

			tasks = TaskManager.GetTasks();
			
			// create our adapter
			taskList = new TaskListAdapter(this, tasks);

			//Hook up our adapter to our ListView
			taskListView.Adapter = taskList;
		}
	}
}
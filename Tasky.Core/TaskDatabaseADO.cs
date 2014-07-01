using System;
using System.Linq;
using System.Collections.Generic;

using Mono.Data.Sqlite;
using System.IO;
using System.Data;

namespace Tasky.Core
{
	/// <summary>
	/// TaskDatabase uses ADO.NET to create the [Items] table and create,read,update,delete data
	/// </summary>
	public class TaskDatabase 
	{
		static object locker = new object ();

		public SqliteConnection connection;

		public string path;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		public TaskDatabase (string dbPath) 
		{
			var output = "";
			path = dbPath;
			// create the tables
			bool exists = File.Exists (dbPath);

			if (!exists) {
				connection = new SqliteConnection ("Data Source=" + dbPath);

				connection.Open ();
				var commands = new[] {
					"CREATE TABLE [Items] (_id INTEGER PRIMARY KEY ASC, Name NTEXT, Notes NTEXT, Done INTEGER);"
				};
				foreach (var command in commands) {
					using (var c = connection.CreateCommand ()) {
						c.CommandText = command;
						var i = c.ExecuteNonQuery ();
					}
				}

				//Start Cj adding this here 6-22-2014
				int r;
				using (var command = connection.CreateCommand ()) {
					command.CommandText = "INSERT INTO [Items] ([Name], [Notes], [Done]) VALUES (? ,?, ?)";
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Contact #1" });
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "2067909610" });
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = false });
					r = command.ExecuteNonQuery ();
				}

				using (var command = connection.CreateCommand ()) {
					command.CommandText = "INSERT INTO [Items] ([Name], [Notes], [Done]) VALUES (? ,?, ?)";
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Contact #2" });
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "" });
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = false });
					r = command.ExecuteNonQuery ();
				}

				using (var command = connection.CreateCommand ()) {
					command.CommandText = "INSERT INTO [Items] ([Name], [Notes], [Done]) VALUES (? ,?, ?)";
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Contact #3" });
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "" });
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = false });
					r = command.ExecuteNonQuery ();
				}

				using (var command = connection.CreateCommand ()) {
					command.CommandText = "INSERT INTO [Items] ([Name], [Notes], [Done]) VALUES (? ,?, ?)";
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Vessel Name" });
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Yuan Fen" });
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = false });
					r = command.ExecuteNonQuery ();
				}

				using (var command = connection.CreateCommand ()) {
					command.CommandText = "INSERT INTO [Items] ([Name], [Notes], [Done]) VALUES (? ,?, ?)";
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Type" });
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Sailboat" });
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = false });
					r = command.ExecuteNonQuery ();
				}

				using (var command = connection.CreateCommand ()) {
					command.CommandText = "INSERT INTO [Items] ([Name], [Notes], [Done]) VALUES (? ,?, ?)";
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Length" });
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "28" });
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = false });
					r = command.ExecuteNonQuery ();
				}

				using (var command = connection.CreateCommand ()) {
					command.CommandText = "INSERT INTO [Items] ([Name], [Notes], [Done]) VALUES (? ,?, ?)";
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Color" });
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Blue" });
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = false });
					r = command.ExecuteNonQuery ();
				}

				using (var command = connection.CreateCommand ()) {
					command.CommandText = "INSERT INTO [Items] ([Name], [Notes], [Done]) VALUES (? ,?, ?)";
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Passengers" });
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "1" });
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = false });
					r = command.ExecuteNonQuery ();
				}

				//Done Cj adding this here 6-22-2014

			} else {
				// already exists, do nothing. 
			}
			Console.WriteLine (output);
		}

		/// <summary>Convert from DataReader to Task object</summary>
		Task FromReader (SqliteDataReader r) {
			var t = new Task ();
			t.ID = Convert.ToInt32 (r ["_id"]);
			t.Name = r ["Name"].ToString ();
			t.Notes = r ["Notes"].ToString ();
			t.Done = Convert.ToInt32 (r ["Done"]) == 1 ? true : false;
			return t;
		}

		public IEnumerable<Task> GetItems ()
		{
			var tl = new List<Task> ();

			lock (locker) {
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var contents = connection.CreateCommand ()) {
					contents.CommandText = "SELECT [_id], [Name], [Notes], [Done] from [Items]";
					var r = contents.ExecuteReader ();
					while (r.Read ()) {
						tl.Add (FromReader(r));
					}
				}
				connection.Close ();
			}
			return tl;
		}

		public Task GetItem (int id) 
		{
			var t = new Task ();
			lock (locker) {
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var command = connection.CreateCommand ()) {
					command.CommandText = "SELECT [_id], [Name], [Notes], [Done] from [Items] WHERE [_id] = ?";
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = id });
					var r = command.ExecuteReader ();
					while (r.Read ()) {
						t = FromReader (r);
						break;
					}
				}
				connection.Close ();
			}
			return t;
		}

		public int SaveItem (Task item) 
		{
			int r;
			lock (locker) {
				if (item.ID != 0) {
					connection = new SqliteConnection ("Data Source=" + path);
					connection.Open ();
					using (var command = connection.CreateCommand ()) {
						command.CommandText = "UPDATE [Items] SET [Name] = ?, [Notes] = ?, [Done] = ? WHERE [_id] = ?;";
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Name });
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Notes });
						command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = item.Done });
						command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = item.ID });
						r = command.ExecuteNonQuery ();
					}
					connection.Close ();
					return r;
				} else {
					connection = new SqliteConnection ("Data Source=" + path);
					connection.Open ();
					using (var command = connection.CreateCommand ()) {
						command.CommandText = "INSERT INTO [Items] ([Name], [Notes], [Done]) VALUES (? ,?, ?)";
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Name });
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Notes });
						command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = item.Done });
						r = command.ExecuteNonQuery ();
					}
					connection.Close ();
					return r;
				}

			}
		}

		public int DeleteItem(int id) 
		{
			lock (locker) {
				int r;
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var command = connection.CreateCommand ()) {
					command.CommandText = "DELETE FROM [Items] WHERE [_id] = ?;";
					command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = id});
					r = command.ExecuteNonQuery ();
				}
				connection.Close ();
				return r;
			}
		}
	}
}
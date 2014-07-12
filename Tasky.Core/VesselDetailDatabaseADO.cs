using System;
using System.Linq;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.IO;
using System.Data;

namespace Epirb.Core
{
	public class VesselDetailDatabase 
	{
		static object locker = new object ();

		public SqliteConnection connection;

		public string path;

		public VesselDetailDatabase (string dbPath) 
		{
			var output = "";
			path = dbPath;
			// create the tables
			bool exists = File.Exists (dbPath);

			if (!exists) {
				connection = new SqliteConnection ("Data Source=" + dbPath);

				connection.Open ();
				var commands = new[] {
					"CREATE TABLE [Items] (_id INTEGER PRIMARY KEY ASC, Name NTEXT, Notes NTEXT);"
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
					command.CommandText = "INSERT INTO [Items] ([Name], [Notes]) VALUES (? ,?)";
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Contact #1" });
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "2067909610" });
					r = command.ExecuteNonQuery ();
				}

				using (var command = connection.CreateCommand ()) {
					command.CommandText = "INSERT INTO [Items] ([Name], [Notes]) VALUES (? ,?)";
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Contact #2" });
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "" });
	
					r = command.ExecuteNonQuery ();
				}

				using (var command = connection.CreateCommand ()) {
					command.CommandText = "INSERT INTO [Items] ([Name], [Notes]) VALUES (? ,?)";
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Contact #3" });
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "" });
					r = command.ExecuteNonQuery ();
				}

				using (var command = connection.CreateCommand ()) {
					command.CommandText = "INSERT INTO [Items] ([Name], [Notes]) VALUES (? ,?)";
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Vessel Name" });
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Test Name" });
					r = command.ExecuteNonQuery ();
				}

				using (var command = connection.CreateCommand ()) {
					command.CommandText = "INSERT INTO [Items] ([Name], [Notes]) VALUES (? ,?)";
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Type" });
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Sailboat, Powerboat, other" });
					r = command.ExecuteNonQuery ();
				}

				using (var command = connection.CreateCommand ()) {
					command.CommandText = "INSERT INTO [Items] ([Name], [Notes]) VALUES (? ,?)";
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Length" });
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "0" });
					r = command.ExecuteNonQuery ();
				}

				using (var command = connection.CreateCommand ()) {
					command.CommandText = "INSERT INTO [Items] ([Name], [Notes]) VALUES (? ,?)";
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Color" });
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Test Color" });
					r = command.ExecuteNonQuery ();
				}

				using (var command = connection.CreateCommand ()) {
					command.CommandText = "INSERT INTO [Items] ([Name], [Notes]) VALUES (? ,?)";
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "Passengers" });
					command.Parameters.Add (new SqliteParameter (DbType.String) { Value = "0" });
					r = command.ExecuteNonQuery ();
				}

				//Done Cj adding this here 6-22-2014

			} else {
				// already exists, do nothing. 
			}
			Console.WriteLine (output);
		}
			
		VesselDetail FromReader (SqliteDataReader r) {
			var t = new VesselDetail ();
			t.ID = Convert.ToInt32 (r ["_id"]);
			t.Name = r ["Name"].ToString ();
			t.Notes = r ["Notes"].ToString ();
			t.Concat = r ["Name"].ToString () + ": " +  r ["Notes"].ToString ();
			return t;
		}

		public IEnumerable<VesselDetail> GetItems ()
		{
			var tl = new List<VesselDetail> ();

			lock (locker) {
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var contents = connection.CreateCommand ()) {
					contents.CommandText = "SELECT [_id], [Name], [Notes] from [Items]";
					var r = contents.ExecuteReader ();
					while (r.Read ()) {
						tl.Add (FromReader(r));
					}
				}
				connection.Close ();
			}
			return tl;
		}

		public VesselDetail GetItem (int id) 
		{
			var t = new VesselDetail ();
			lock (locker) {
				connection = new SqliteConnection ("Data Source=" + path);
				connection.Open ();
				using (var command = connection.CreateCommand ()) {
					command.CommandText = "SELECT [_id], [Name], [Notes] from [Items] WHERE [_id] = ?";
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

		public int SaveItem (VesselDetail item) 
		{
			int r;
			lock (locker) {
				if (item.ID != 0) {
					connection = new SqliteConnection ("Data Source=" + path);
					connection.Open ();
					using (var command = connection.CreateCommand ()) {
						command.CommandText = "UPDATE [Items] SET [Name] = ?, [Notes] = ? WHERE [_id] = ?;";
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Name });
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Notes });
						command.Parameters.Add (new SqliteParameter (DbType.Int32) { Value = item.ID });
						r = command.ExecuteNonQuery ();
					}
					connection.Close ();
					return r;
				} else {
					connection = new SqliteConnection ("Data Source=" + path);
					connection.Open ();
					using (var command = connection.CreateCommand ()) {
						command.CommandText = "INSERT INTO [Items] ([Name], [Notes]) VALUES (? ,?)";
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Name });
						command.Parameters.Add (new SqliteParameter (DbType.String) { Value = item.Notes });
						r = command.ExecuteNonQuery ();
					}
					connection.Close ();
					return r;
				}

			}
		}			
	}
}
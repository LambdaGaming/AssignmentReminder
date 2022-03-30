﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace AssignmentReminder
{
	public class AssignmentFile
	{
		public List<Assignment> AllAssignments = new List<Assignment>();
		public static readonly string JsonDir = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + @"\AssignmentReminder\assignments.json";

		public static void Load()
		{
			string dir = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + @"\AssignmentReminder";
			if ( !Directory.Exists( dir ) )
				Directory.CreateDirectory( dir );

			if ( !File.Exists( JsonDir ) )
				File.WriteAllText( JsonDir, "{ AllAssignments:[] }" );

			AssignmentReminder.MainFile = JsonConvert.DeserializeObject<AssignmentFile>( JsonDir );
		}

		public static void Save()
		{
			try
			{
				File.WriteAllText( JsonDir, JsonConvert.SerializeObject( AssignmentReminder.MainFile ) );
			}
			catch ( Exception e )
			{
				MessageBox.Show( e.Message, "Error" );
			}
		}

		public static void AddAssignment( string name, DateTime due )
		{
			Assignment assignment = new Assignment {
				Id = AssignmentReminder.MainFile.AllAssignments.Count + 1,
				Name = AssignmentList.FormatText( name ),
				DueDate = due
			};
			AssignmentReminder.MainFile.AllAssignments.Add( assignment );
		}

		public static void RemoveAssignment( int id )
		{
			Assignment removed = null;
			foreach ( Assignment assignment in AssignmentReminder.MainFile.AllAssignments )
			{
				if ( assignment.Id == id )
				{
					removed = assignment;
				}
			}

			if ( removed != null )
			{
				RemoveAssignment( removed );
			}
		}

		public static void RemoveAssignment( string name )
		{
			Assignment removed = null;
			foreach ( Assignment assignment in AssignmentReminder.MainFile.AllAssignments )
			{
				if ( assignment.Name == name )
				{
					removed = assignment;
				}
			}

			if ( removed != null )
			{
				RemoveAssignment( removed );
			}
		}

		public static void RemoveAssignment( Assignment assignment )
		{
			AssignmentReminder.MainFile.AllAssignments.Remove( assignment );
		}
	}

	public class Assignment
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime DueDate { get; set; }
	}
}

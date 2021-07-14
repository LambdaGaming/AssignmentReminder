using System;
using System.Windows.Forms;
using System.Diagnostics;
using AssignmentReminder.Properties;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Timers;

namespace AssignmentReminder
{
	static class AssignmentReminder
	{
		private static NotifyIcon notify;
		public static System.Timers.Timer closetimer = new System.Timers.Timer( 60000 );

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );

			notify = new NotifyIcon
			{
				Icon = Resources.icon,
				Text = "Assignment Reminder",
				ContextMenu = GetContextMenu(),
				Visible = true
			};
			DueNotify( notify );

			closetimer.Elapsed += TimerEnd;
			closetimer.Start();

			Application.ApplicationExit += delegate { notify.Dispose(); };
			Application.Run();
		}

		private static void TimerEnd( object obj, ElapsedEventArgs e )
		{
			Application.Exit();
		}

		private static ContextMenu GetContextMenu()
		{
			string path = AppDomain.CurrentDomain.BaseDirectory.ToString();
			Process process = new Process();
			process.StartInfo.FileName = path;

			ContextMenu menu = new ContextMenu();
			menu.MenuItems.Add( "Show Popup", delegate { DueNotify( notify ); } );
			menu.MenuItems.Add( "Show Assignments", delegate {
				AssignmentList list = new AssignmentList();
				list.ShowDialog();
			} );
			menu.MenuItems.Add( "Add Assignments", delegate {
				AssignmentManager add = new AssignmentManager();
				add.ShowDialog();
			} );
			menu.MenuItems.Add( "Exit", delegate { Application.Exit(); } );
			return menu;
		}

		private static void DueNotify( NotifyIcon notify )
		{
			string path = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + @"\AssignmentReminder\settings.xml";
			if ( !File.Exists( path ) ) return;
			XDocument settings = XDocument.Load( path );
			var assignment = from c in settings.Root.Descendants( "assignment" ) select c.Element( "due" ).Value;
			bool duetoday = false;
			int dueamount = 0;
			int duesoon = 0;
			int overdue = 0;
			int totalassignments = 0;

			foreach ( string dates in assignment )
			{
				DateTime date = DateTime.FromBinary( long.Parse( dates ) );
				TimeSpan daysleft = date.Date - DateTime.Today;

				if ( date.Date == DateTime.Today )
				{
					duetoday = true;
					dueamount++;
				}

				if ( daysleft.TotalDays <= 2 && daysleft.TotalDays > 0 )
					duesoon++;

				if ( daysleft.TotalDays < 0 )
					overdue++;

				totalassignments++;
			}

			if ( totalassignments <= 0 ) return; // Don't show popup if there aren't any assignments
			if ( duetoday && dueamount > 0 )
				notify.ShowBalloonTip( 1, "Assignments Due", "You have " + dueamount.ToString() + " assignment(s) due today. Click to view them.", ToolTipIcon.Info );
			else
			{
				if ( overdue > 0 )
					notify.ShowBalloonTip( 1, "No Assignments Due", "You have no assignments due today. You have " + overdue.ToString() + " overdue assignment(s).", ToolTipIcon.Info );
				else if ( duesoon > 0 )
					notify.ShowBalloonTip( 1, "No Assignments Due", "You have no assignments due today. You have " + duesoon.ToString() + " assignment(s) due soon.", ToolTipIcon.Info );
				else
					notify.ShowBalloonTip( 1, "No Assignments Due", "You have no assignments due today.", ToolTipIcon.Info );
			}
			notify.BalloonTipClicked += BalloonTipClicked;
			closetimer.Stop();
		}

		private static void BalloonTipClicked( object sender, EventArgs e )
		{
			AssignmentList list = new AssignmentList();
			list.ShowDialog();
		}
	}
}

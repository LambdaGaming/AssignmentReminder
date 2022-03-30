using System;
using System.Windows.Forms;
using System.Diagnostics;
using AssignmentReminder.Properties;
using System.Timers;

namespace AssignmentReminder
{
	static class AssignmentReminder
	{
		private static NotifyIcon notify;
		public static System.Timers.Timer CloseTimer = new System.Timers.Timer( 60000 );
		public static AssignmentList ListWindow = null;
		public static AssignmentManager ManagerWindow = null;
		public static AssignmentFile MainFile = null;

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );

			notify = new NotifyIcon {
				Icon = Resources.icon,
				Text = "Assignment Reminder",
				ContextMenu = GetContextMenu(),
				Visible = true
			};
			DueNotify( notify );

			CloseTimer.Elapsed += TimerEnd;
			CloseTimer.Start();
			AssignmentFile.Load();

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
				if ( ListWindow == null )
				{
					AssignmentList list = new AssignmentList();
					list.ShowDialog();
				}
				else
				{
					ListWindow.Visible = true;
					ListWindow.BringToFront();
				}
			} );
			menu.MenuItems.Add( "Add Assignments", delegate {
				if ( ManagerWindow == null )
				{
					AssignmentManager add = new AssignmentManager();
					add.ShowDialog();
				}
				else
				{
					ManagerWindow.Visible = true;
					ManagerWindow.BringToFront();
				}
			} );
			menu.MenuItems.Add( "Exit", delegate { Application.Exit(); } );
			return menu;
		}

		private static void DueNotify( NotifyIcon notify )
		{
			bool duetoday = false;
			int dueamount = 0;
			int duesoon = 0;
			int overdue = 0;
			int totalassignments = 0;

			foreach ( Assignment assignment in MainFile.AllAssignments )
			{
				DateTime date = assignment.DueDate;
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
			{
				notify.ShowBalloonTip( 1, "Assignments Due", "You have " + dueamount.ToString() + " assignment(s) due today. Click to view them.", ToolTipIcon.Info );
			}
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
			CloseTimer.Stop();
		}

		private static void BalloonTipClicked( object sender, EventArgs e )
		{
			if ( ListWindow == null )
			{
				AssignmentList list = new AssignmentList();
				list.ShowDialog();
			}
			else
			{
				ListWindow.Visible = true;
				ListWindow.BringToFront();
			}
		}
	}
}

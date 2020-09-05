using System;
using System.Windows.Forms;
using System.Diagnostics;
using AssignmentReminder.Properties;
using System.Xml.Linq;
using System.Linq;

namespace AssignmentReminder
{
	static class AssignmentReminder
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );

			NotifyIcon notify = new NotifyIcon
			{
				Icon = Resources.icon,
				ContextMenu = GetContextMenu(),
				Visible = true
			};
			DueNotify( notify );

			Application.ApplicationExit += delegate { notify.Dispose(); };
			Application.Run();
		}

		private static ContextMenu GetContextMenu()
		{
			string path = AppDomain.CurrentDomain.BaseDirectory.ToString();
			Process process = new Process();
			process.StartInfo.FileName = path;

			ContextMenu menu = new ContextMenu();
			menu.MenuItems.Add( "Show Assignments", delegate {
				AssignmentList list = new AssignmentList();
				list.ShowDialog();
			} );
			menu.MenuItems.Add( "Exit", delegate { Application.Exit(); } );
			return menu;
		}

		private static void DueNotify( NotifyIcon notify )
		{
			string path = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + @"\AssignmentReminder\settings.xml";
			XDocument settings = XDocument.Load( path );
			var assignment = from c in settings.Root.Descendants( "assignment" ) select c.Element( "due" ).Value;
			bool duetoday = false;
			foreach ( string dates in assignment )
			{
				DateTime date = DateTime.FromBinary( long.Parse( dates ) );
				if ( date.Date == DateTime.Today )
				{
					duetoday = true;
					break;
				}
			}
			if ( duetoday )
				notify.ShowBalloonTip( 1, "Assignments Due", "You have assignments due today. Click to view them.", ToolTipIcon.Info );
			else
				notify.ShowBalloonTip( 1, "Assignments Due", "You have no assignments due today.", ToolTipIcon.Info );
			notify.BalloonTipClicked += BalloonTipClicked;
		}

		private static void BalloonTipClicked( object sender, EventArgs e )
		{
			AssignmentList list = new AssignmentList();
			list.ShowDialog();
		}
	}
}

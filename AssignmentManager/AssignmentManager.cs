using System;
using System.Windows.Forms;

namespace AssignmentManager
{
	static class AssignmentManager
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new MainWindow() );
		}
	}
}

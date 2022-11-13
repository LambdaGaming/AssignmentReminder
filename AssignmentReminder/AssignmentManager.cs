using System;
using System.Windows.Forms;

namespace AssignmentReminder
{
	public partial class AssignmentManager : Form
	{
		public bool Editing = false;

		public AssignmentManager()
		{
			InitializeComponent();
			TimeChooser.CustomFormat = "hh:mm:ss tt";
			TimeChooser.Format = DateTimePickerFormat.Custom;
			DateTime now = DateTime.Now;
			TimeChooser.Value = new DateTime( now.Year, now.Month, now.Day, 23, 59, 59 );
			AssignmentReminder.CloseTimer.Stop();
			AssignmentReminder.ManagerWindow = this;
			FormClosed += delegate { AssignmentReminder.ManagerWindow = null; };
		}

		private void AddButton_Click( object sender, EventArgs e )
		{
			DateTime dayvalue = DateChooser.Value;
			DateTime timevalue = TimeChooser.Value;
			DateTime timeset = new DateTime( dayvalue.Year, dayvalue.Month, dayvalue.Day, timevalue.Hour, timevalue.Minute, timevalue.Second );

			if ( Editing )
			{
				ListViewItem selected = AssignmentReminder.ListWindow.listView.SelectedItems[0];
				AssignmentFile.RemoveAssignment( ( ( Assignment ) selected.Tag ).Id );
			}

			try
			{
				AssignmentFile.AddAssignment( NameBox.Text, timeset );
				AssignmentFile.Save();
				string message = Editing ? "updated" : "created";
				MessageBox.Show( "Successfully " + message + " assignment.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information );
			}
			catch( Exception ex )
			{
				MessageBox.Show( "Something went wrong while creating the assignment: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
		}
	}
}

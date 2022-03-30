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
			AssignmentReminder.CloseTimer.Stop();
			AssignmentReminder.ManagerWindow = this;
			FormClosed += delegate { AssignmentReminder.ManagerWindow = null; };
		}

		private void AddButton_Click( object sender, EventArgs e )
		{
			DateTime dayvalue = DateChooser.Value;
			DateTime timevalue = TimeChooser.Value;
			DateTime timeset = new DateTime( dayvalue.Year, dayvalue.Month, dayvalue.Day, timevalue.Hour, timevalue.Minute, timevalue.Second );

			if ( NameBox.Text.Contains( "!" ) )
			{
				MessageBox.Show( "The character '!' is not allowed. Please remove it and try again.", "Illegal character", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			if ( Editing )
			{
				ListViewItem selected = AssignmentReminder.ListWindow.listView.SelectedItems[0];
				selected.Remove();
				AssignmentFile.RemoveAssignment( AssignmentList.FormatText( selected.Text ) );
			}

			foreach ( Assignment assignment in AssignmentReminder.MainFile.AllAssignments )
			{
				if ( assignment.Name == NameBox.Text )
				{
					MessageBox.Show( "An assignment with this name already exists. Please choose a different name.", "Name already taken", MessageBoxButtons.OK, MessageBoxIcon.Error );
					return;
				}
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
				MessageBox.Show( "Something went wrong while creating the assignment: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
		}
	}
}

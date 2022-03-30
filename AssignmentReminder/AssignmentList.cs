using System;
using System.Drawing;
using System.Windows.Forms;

namespace AssignmentReminder
{
	public partial class AssignmentList : Form
	{
		private ListViewColumnSorter sort;

		public AssignmentList()
		{
			InitializeComponent();
			RegisterEvents();

			if ( AssignmentReminder.MainFile == null )
			{
				MessageBox.Show( "No assignments were found because the assignments.json file doesn't exist.", "No Assignments", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			if ( AssignmentReminder.MainFile.AllAssignments.Count == 0 )
			{
				MessageBox.Show( "No assignments were found.", "No Assignments", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			int count = 0;
			string textFormat = "MM/dd/yyyy hh:mm:ss tt";
			foreach ( Assignment assignment in AssignmentReminder.MainFile.AllAssignments )
			{
				ListViewItem item = listView.Items.Add( assignment.Name );
				item.UseItemStyleForSubItems = false;

				DateTime date = assignment.DueDate;
				TimeSpan daysleft = date.Date - DateTime.Today;
				if ( date.Date == DateTime.Today )
				{
					ListViewItem.ListViewSubItem color = new ListViewItem.ListViewSubItem();
					color.BackColor = Color.Red;
					color.Text = date.ToString( textFormat );
					listView.Items[count].BackColor = Color.Red;
					listView.Items[count].SubItems.Add( color );
				}
				else if ( daysleft.TotalDays <= 2 && daysleft.TotalDays > 0 )
				{
					ListViewItem.ListViewSubItem color = new ListViewItem.ListViewSubItem();
					Color lambdaorange = Color.FromArgb( 255, 255, 89, 0 ); // #FF5900
					color.BackColor = lambdaorange;
					color.Text = date.ToString( textFormat );
					listView.Items[count].BackColor = lambdaorange;
					listView.Items[count].SubItems.Add( color );
				}
				else if ( daysleft.TotalDays < 0 )
				{
					ListViewItem.ListViewSubItem color = new ListViewItem.ListViewSubItem();
					color.BackColor = Color.Red;
					color.Text = date.ToString( textFormat );
					listView.Items[count].Text = "!!! " + listView.Items[count].Text + " !!!";
					listView.Items[count].BackColor = Color.Red;
					listView.Items[count].SubItems.Add( color );
				}
				else
				{
					listView.Items[count].SubItems.Add( date.ToString( textFormat ) );
				}
				count++;

				AssignmentReminder.ListWindow = this;
				FormClosed += delegate { AssignmentReminder.ListWindow = null; };
			}

			sort = new ListViewColumnSorter();
			listView.ListViewItemSorter = sort;
			sort.SortColumn = 1; // Sorts the due date column to show nearest due dates at the top
			sort.Order = SortOrder.Ascending;
			listView.Sort();

			AssignmentReminder.CloseTimer.Stop();
		}

		private void RegisterEvents()
		{
			listView.ColumnClick += new ColumnClickEventHandler( ColumnClick );
			listView.DoubleClick += new EventHandler( DoubleClicked );
		}

		private void ColumnClick( object sender, ColumnClickEventArgs e )
		{
			if ( e.Column == sort.SortColumn )
			{
				if ( sort.Order == SortOrder.Ascending )
					sort.Order = SortOrder.Descending;
				else
					sort.Order = SortOrder.Ascending;
			}
			else
			{
				sort.SortColumn = e.Column;
				sort.Order = SortOrder.Ascending;
			}
			listView.Sort();
		}

		public static string FormatText( string text )
		{
			if ( text.StartsWith( "!!!" ) )
			{
				string trim = text.Trim( new char[] { '!' } );
				string nospace = trim.Trim();
				return nospace;
			}
			return text;
		}

		private void DeleteAssignment()
		{
			DialogResult confirm = MessageBox.Show( "Are you sure you want to delete this assignment?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
			if ( confirm == DialogResult.Yes )
			{
				ListViewItem selected = listView.SelectedItems[0];
				selected.Remove();
				AssignmentFile.RemoveAssignment( FormatText( selected.Text ) );
				AssignmentFile.Save();
			}
		}

		private void DoubleClicked( object sender, EventArgs e )
		{
			DeleteAssignment();
		}

		private void OnMouseClick( object sender, MouseEventArgs e )
		{
			if ( e.Button == MouseButtons.Right )
			{
				ContextMenu menu = new ContextMenu();
				menu.MenuItems.Add( "Delete", delegate {
					DeleteAssignment();
				} );
				menu.MenuItems.Add( "Edit Assignment", delegate {
					ListViewItem selected = listView.SelectedItems[0];
					AssignmentManager add = new AssignmentManager();
					add.Editing = true;
					add.Text = "Edit Assignment";
					add.AddButton.Text = "Confirm Changes";
					add.NameBox.Text = selected.Text;
					add.ShowDialog();
				} );
				menu.Show( this, PointToClient( Cursor.Position ) );
			}
		}
	}
}

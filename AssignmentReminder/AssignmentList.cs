using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace AssignmentReminder
{
	public partial class AssignmentList : Form
	{
		private ListViewColumnSorter sort;
		private string path = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + @"\AssignmentReminder\assignments.xml";

		public AssignmentList()
		{
			InitializeComponent();
			RegisterEvents();

			string xmldir = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + @"\AssignmentReminder";
			string settingsdir = xmldir + @"\assignments.xml";
			XmlDocument tempxml = new XmlDocument();

			if ( !Directory.Exists( xmldir ) )
				Directory.CreateDirectory( xmldir );

			if ( !File.Exists( settingsdir ) )
			{
				XmlElement init = tempxml.CreateElement( "settings" );
				tempxml.AppendChild( init );
				tempxml.Save( settingsdir );
				MessageBox.Show( "No assignments were found because the assignments.xml file didn't exist.", "No Assignments", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			XDocument settings = XDocument.Load( path );
			var name = from c in settings.Root.Descendants( "assignment" ) select c.Element( "name" ).Value;
			var due = from c in settings.Root.Descendants( "assignment" ) select c.Element( "due" ).Value;
			
			if ( name.Count() == 0 )
			{
				MessageBox.Show( "No assignments were found.", "No Assignments", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			foreach ( string names in name )
			{
				ListViewItem item = listView.Items.Add( names );
				item.UseItemStyleForSubItems = false;
			}

			int count = 0;
			string textFormat = "MM/dd/yyyy hh:mm:ss tt";
			foreach ( string dates in due )
			{
				DateTime date = DateTime.FromBinary( long.Parse( dates ) );
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

		private ListViewItem GetSelectedItem()
		{
			return listView.SelectedItems[0];
		}

		private void DeleteAssignment()
		{
			DialogResult confirm = MessageBox.Show( "Are you sure you want to delete this assignment?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
			if ( confirm == DialogResult.Yes )
			{
				ListViewItem selected = GetSelectedItem();
				XDocument settings = XDocument.Load( path );
				List<XElement> ancestors = settings.Descendants().Where( x => ( string ) x == FormatText( selected.Text ) ).Ancestors().ToList();
				for ( int i = 0; i <= ancestors.Count - 2; i++ )
					ancestors[i].Remove();
				selected.Remove();
				settings.Save( path );
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
					ListViewItem selected = GetSelectedItem();
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

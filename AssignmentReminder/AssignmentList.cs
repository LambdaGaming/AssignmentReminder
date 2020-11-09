using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AssignmentReminder
{
	public partial class AssignmentList : Form
	{
		private ListViewColumnSorter sort;
		private string path = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + @"\AssignmentReminder\settings.xml";

		public AssignmentList()
		{
			InitializeComponent();
			RegisterEvents();

			XDocument settings = XDocument.Load( path );
			var name = from c in settings.Root.Descendants( "assignment" ) select c.Element( "name" ).Value;
			var due = from c in settings.Root.Descendants( "assignment" ) select c.Element( "due" ).Value;
			
			foreach ( string names in name )
			{
				ListViewItem item = listView.Items.Add( names );
				item.UseItemStyleForSubItems = false;
			}

			int count = 0;
			foreach ( string dates in due )
			{
				DateTime date = DateTime.FromBinary( long.Parse( dates ) );
				TimeSpan daysleft = date.Date - DateTime.Today;
				if ( date.Date == DateTime.Today )
				{
					ListViewItem.ListViewSubItem color = new ListViewItem.ListViewSubItem();
					color.BackColor = Color.Red;
					color.Text = date.ToString();
					listView.Items[count].BackColor = Color.Red;
					listView.Items[count].SubItems.Add( color );
				}
				else if ( daysleft.TotalDays <= 2 && daysleft.TotalDays > 0 )
				{
					ListViewItem.ListViewSubItem color = new ListViewItem.ListViewSubItem();
					Color lambdaorange = Color.FromArgb( 255, 255, 89, 0 );
					color.BackColor = lambdaorange;
					color.Text = date.ToString();
					listView.Items[count].BackColor = lambdaorange;
					listView.Items[count].SubItems.Add( color );
				}
				else if ( daysleft.TotalDays < 0 )
				{
					ListViewItem.ListViewSubItem color = new ListViewItem.ListViewSubItem();
					color.BackColor = Color.Red;
					color.Text = date.ToString();
					listView.Items[count].Text = "!!! " + listView.Items[count].Text + " !!!";
					listView.Items[count].BackColor = Color.Red;
					listView.Items[count].SubItems.Add( color );
				}
				else
				{
					listView.Items[count].SubItems.Add( date.ToString() );
				}
				count++;
			}

			sort = new ListViewColumnSorter();
			listView.ListViewItemSorter = sort;
			listView.Sorting = SortOrder.Ascending;
			listView.Sort();
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

		private string FormatText( string text )
		{
			if ( text.StartsWith( "!!!" ) )
			{
				string trim = text.Trim( new char[] { '!' } );
				string nospace = trim.Trim();
				return nospace;
			}
			return text;
		}

		private void DoubleClicked( object sender, EventArgs e )
		{
			DialogResult confirm = MessageBox.Show( "Are you sure you want to delete this assignment?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question );
			if ( confirm == DialogResult.Yes )
			{
				foreach ( ListViewItem selected in listView.SelectedItems )
				{
					XDocument settings = XDocument.Load( path );
					List<XElement> ancestors = settings.Descendants().Where( x => ( string ) x == FormatText( selected.Text ) ).Ancestors().ToList();
					for ( int i=0; i <= ancestors.Count - 2; i++ )
						ancestors[i].Remove();
					selected.Remove();
					settings.Save( path );
				}
			}
		}
	}
}

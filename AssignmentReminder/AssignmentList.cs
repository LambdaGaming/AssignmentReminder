using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AssignmentReminder
{
	public partial class AssignmentList : Form
	{
		private ListViewColumnSorter sort;

		public AssignmentList()
		{
			InitializeComponent();

			string path = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + @"\AssignmentReminder\settings.xml";
			XDocument settings = XDocument.Load( path );
			var name = from c in settings.Root.Descendants( "assignment" ) select c.Element( "name" ).Value;
			var due = from c in settings.Root.Descendants( "assignment" ) select c.Element( "due" ).Value;
			
			foreach ( string names in name )
			{
				listView.Items.Add( names );
			}

			int count = 0;
			foreach ( string dates in due )
			{
				DateTime date = DateTime.FromBinary( long.Parse( dates ) );
				listView.Items[count].SubItems.Add( date.ToString() );
				count++;
			}

			sort = new ListViewColumnSorter();
			listView.ListViewItemSorter = sort;
			listView.Sorting = SortOrder.Ascending;
			listView.Sort();
		}

		private void listView_ColumnClick( object sender, ColumnClickEventArgs e )
		{
			if ( e.Column == sort.SortColumn )
			{
				if ( sort.Order == SortOrder.Ascending )
				{
					sort.Order = SortOrder.Descending;
				}
				else
				{
					sort.Order = SortOrder.Ascending;
				}
			}
			else
			{
				sort.SortColumn = e.Column;
				sort.Order = SortOrder.Ascending;
			}
			listView.Sort();
		}
	}
}

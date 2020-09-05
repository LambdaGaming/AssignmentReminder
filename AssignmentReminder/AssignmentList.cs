using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AssignmentReminder
{
	public partial class AssignmentList : Form
	{
		public AssignmentList()
		{
			InitializeComponent();

			string path = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + @"\AssignmentReminder\settings.xml";
			XDocument settings = XDocument.Load( path );
			var name = from c in settings.Root.Descendants( "assignment" ) select c.Element( "name" ).Value;
			var due = from c in settings.Root.Descendants( "assignment" ) select c.Element( "due" ).Value;
			
			foreach ( string names in name )
			{
				listView.Items.Add( new ListViewItem( names ) );
			}

			int count = 0;
			foreach ( string dates in due )
			{
				DateTime date = DateTime.FromBinary( long.Parse( dates ) );
				ListViewItem.ListViewSubItem newlist = new ListViewItem.ListViewSubItem();
				listView.Items[count].SubItems.Add( date.ToString() );
				count++;
			}
		}
	}
}

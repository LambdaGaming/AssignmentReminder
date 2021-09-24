using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

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
			string xmldir = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + @"\AssignmentReminder";
			string settingsdir = xmldir + @"\settings.xml";
			XmlDocument settings = new XmlDocument();
			DateTime dayvalue = DateChooser.Value;
			DateTime timevalue = TimeChooser.Value;
			DateTime timeset = new DateTime( dayvalue.Year, dayvalue.Month, dayvalue.Day, timevalue.Hour, timevalue.Minute, timevalue.Second );

			if ( !Directory.Exists( xmldir ) )
				Directory.CreateDirectory( xmldir );

			if ( !File.Exists( settingsdir ) )
			{
				XmlElement init = settings.CreateElement( "settings" );
				settings.AppendChild( init );
				settings.Save( settingsdir );
			}

			XDocument checkname = XDocument.Load( settingsdir );
			var names = from c in checkname.Root.Descendants( "assignment" ) select c.Element( "name" ).Value;

			if ( NameBox.Text.Contains( "!" ) )
			{
				MessageBox.Show( "The character '!' is not allowed. Please remove it and try again.", "Illegal character", MessageBoxButtons.OK, MessageBoxIcon.Error );
				return;
			}

			if ( Editing )
			{
				ListViewItem selected = AssignmentReminder.ListWindow.listView.SelectedItems[0];
				List<XElement> ancestors = checkname.Descendants().Where( x => ( string ) x == AssignmentList.FormatText( selected.Text ) ).Ancestors().ToList();
				for ( int i = 0; i <= ancestors.Count - 2; i++ )
					ancestors[i].Remove();
				selected.Remove();
				checkname.Save( settingsdir );
			}

			foreach ( string name in names )
			{
				if ( NameBox.Text == name )
				{
					MessageBox.Show( "An assignment with this name already exists. Please choose a different name.", "Name already taken", MessageBoxButtons.OK, MessageBoxIcon.Error );
					return;
				}
			}

			settings.Load( settingsdir );
			XmlElement newitem = settings.CreateElement( "assignment" );

			XmlElement itemname = settings.CreateElement( "name" );
			itemname.InnerText = NameBox.Text;

			XmlElement itemdue = settings.CreateElement( "due" );
			itemdue.InnerText = timeset.ToBinary().ToString();

			try
			{
				newitem.AppendChild( itemname );
				newitem.AppendChild( itemdue );
				settings.DocumentElement.AppendChild( newitem );
				settings.Save( settingsdir );
				MessageBox.Show( "Successfully created assignment.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information );
			}
			catch( Exception ex )
			{
				MessageBox.Show( "Something went wrong while creating the assignment: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
			}
		}
	}
}

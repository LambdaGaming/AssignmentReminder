using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace AssignmentReminder
{
	public partial class AssignmentManager : Form
	{
		public AssignmentManager()
		{
			InitializeComponent();
		}

		private void AddButton_Click( object sender, EventArgs e )
		{
			string xmldir = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ) + @"\AssignmentReminder";
			string settingsdir = xmldir + @"\settings.xml";
			XmlDocument settings = new XmlDocument();

			if ( !Directory.Exists( xmldir ) )
				Directory.CreateDirectory( xmldir );

			if ( !File.Exists( settingsdir ) )
			{
				XmlElement init = settings.CreateElement( "settings" );
				settings.AppendChild( init );
				settings.Save( settingsdir );
			}

			settings.Load( settingsdir );
			XmlElement newitem = settings.CreateElement( "assignment" );

			XmlElement itemname = settings.CreateElement( "name" );
			itemname.InnerText = NameBox.Text;

			XmlElement itemdue = settings.CreateElement( "due" );
			itemdue.InnerText = DateChooser.Value.ToBinary().ToString();

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

using System;
using System.Windows.Forms;
using WorkWithDataSource;

namespace GUI
{
	public partial class DataSource : Form
	{
		public DataSource()
		{
			InitializeComponent();
		}
		private void DataSource_Load(object sender, EventArgs e)
		{

		}


		private void button1_Click(object sender, EventArgs e)
		{
			labelConnnect.Visible = false;
			if (ConnectionTextBox.Text.Trim() != "" && LoginTextBox.Text.Trim() != "" && PasswordTextBox.Text.Trim() != "" )
			{
				DataBaseAutentification dataBaseAutentification = new DataBaseAutentification(ConnectionTextBox.Text, LoginTextBox.Text,PasswordTextBox.Text);
				new DataBaseAutentificationToXML(dataBaseAutentification);
				this.Close();
			}
			else
			{
				labelConnnect.Visible = true;
			}
		}

		
		private void CancelingButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}

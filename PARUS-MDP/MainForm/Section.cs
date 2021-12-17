using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DataTypes;
using WorkWithDataSource;
using WorkWithCatalog;


namespace GUI
{
	public partial class Section : Form
	{
		public Section()
		{
			InitializeComponent();
			PullData pullData = new PullData();
			var sections = pullData.Sections;
			SectionComboBox.DataSource = sections;
		}

		private void Section_Load(object sender, EventArgs e)
		{
			
		}

		private void DataSourceButton_Click(object sender, EventArgs e)
		{
			DataSource dataSource = new DataSource();
			dataSource.Show();
		}

		private void AcceptingButton_Click(object sender, EventArgs e)
		{
			PullData pullData = new PullData(SectionComboBox.Text);
			FactorsOrSchemesForm schemeForm = new FactorsOrSchemesForm(SectionComboBox.Text, EnumForGUI.Scheme, pullData);
			FactorsOrSchemesForm factorForm = new FactorsOrSchemesForm(SectionComboBox.Text, EnumForGUI.Factor, pullData);
			bool flag = true;
			this.Hide();
			var dialogresultScheme = schemeForm.ShowForm();
			while (flag)
			{
				if (dialogresultScheme == DialogResult.Cancel)
				{
					this.Show();
					flag = false;
				}
				if (dialogresultScheme == DialogResult.OK)
				{
					var dialogResultFactor = factorForm.ShowForm();

					if (dialogResultFactor == DialogResult.OK)
					{
						FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
						if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
						{
							CatalogCreator catalogCreator = 
								new CatalogCreator(folderBrowserDialog.SelectedPath,_section, factorForm.Factors, schemeForm.Schemes);
							catalogCreator.Create();
							this.Close();
						}
						break;
					}
					if (dialogResultFactor == DialogResult.Cancel)
					{
						dialogresultScheme = schemeForm.ShowForm();
					}
					if(dialogResultFactor == DialogResult.Abort)
					{
						this.Close();
						break;
					}
				}
				if(dialogresultScheme == DialogResult.Abort)
				{
					this.Close();
					break;
				}
			}
		}

		private void Section_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}
	}
}

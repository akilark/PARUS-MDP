using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DataTypes;
using WorkWithDataSource;


namespace GUI
{
	public partial class Section : Form
	{
		private string _section;
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

		private void SectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			_section = SectionComboBox.SelectedItem.ToString();
		}

		private void AcceptingButton_Click(object sender, EventArgs e)
		{
			PullData pullData = new PullData(_section);
			FactorsOrSchemesForm schemeForm = new FactorsOrSchemesForm(_section, EnumForGUI.Scheme, pullData);
			FactorsOrSchemesForm factorForm = new FactorsOrSchemesForm(_section, EnumForGUI.Factor, pullData);
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
						schemeForm.Close();
						factorForm.Close();
						this.Close();
						break;
					}
					if (dialogResultFactor == DialogResult.Cancel)
					{
						dialogresultScheme = schemeForm.ShowForm();
					}
				}
			}
		}

		private void Section_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}
	}
}

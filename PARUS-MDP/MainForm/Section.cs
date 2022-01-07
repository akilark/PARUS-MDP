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
using OutputFileStructure;


namespace GUI
{
	public partial class Section : Form
	{
		private List<string> _sections;
		private bool _dataSourceConnected;
		private PullData _nullPullData;
		private DataBaseAutentification _dataBaseAutentification;
		public Section()
		{
			InitializeComponent();
			DataBaseAutentificationToXML dataBaseAutentification = new DataBaseAutentificationToXML();
			_dataBaseAutentification = dataBaseAutentification.ReadFileInfo();
			ComboBoxFill();
		}

		private void Section_Load(object sender, EventArgs e)
		{
			
		}

		private void ComboBoxFill()
		{
			PullData pullData = new PullData(_dataBaseAutentification);
			_dataSourceConnected = pullData.IsConnected;
			if (_dataSourceConnected)
			{
				_sections = pullData.Sections;
			}
			else
			{
				dataSourceLabel.Visible = true;
				_nullPullData = pullData;
				_sections = new List<string>();
			}
			SectionComboBox.DataSource = _sections;
		}

		private void DataSourceButton_Click(object sender, EventArgs e)
		{
			DataSource dataSource = new DataSource();
			dataSource.ShowDialog();
			DataBaseAutentificationToXML dataBaseAutentification = new DataBaseAutentificationToXML();
			_dataBaseAutentification = dataBaseAutentification.ReadFileInfo();
			ComboBoxFill();
		}

		private void AcceptingButton_Click(object sender, EventArgs e)
		{
			PullData pullData;
			if(_dataSourceConnected)
			{
				pullData = new PullData(SectionComboBox.Text, _dataBaseAutentification);
				bool uniqueFlag = true;
				for(int i = 0; i < pullData.Sections.Count; i ++)
				{
					if(Comparator.CompareString(SectionComboBox.Text, pullData.Sections[i]))
					{
						uniqueFlag = false;
					}
				}
				if (uniqueFlag)
				{
					_sections.Add(SectionComboBox.Text);
				}
			}
			else
			{
				pullData = _nullPullData;
				_sections.Add(SectionComboBox.Text);
			}
			
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
								new CatalogCreator(folderBrowserDialog.SelectedPath, SectionComboBox.Text, factorForm.Factors, schemeForm.Schemes);
							catalogCreator.Create();

							SectionFromDataSource sectionFromDataSource = new SectionFromDataSource();
							sectionFromDataSource.SectionName = SectionComboBox.Text;
							sectionFromDataSource.Schemes = schemeForm.Schemes;
							sectionFromDataSource.Factors = factorForm.Factors;
							sectionFromDataSource.Imbalances = pullData.Imbalances;
							sectionFromDataSource.AOPOlist = pullData.AOPOlist;
							sectionFromDataSource.AOCNlist = pullData.AOCNlist;
							SectionInfoToXml pullDataToXml = new SectionInfoToXml(folderBrowserDialog.SelectedPath + @$"\{SectionComboBox.Text}", sectionFromDataSource);
							
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

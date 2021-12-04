using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WorkWithDataSource;

namespace GUI
{
	public partial class FactorsOrSchemesForm : Form
	{
		private EnumForGUI _enumforGUI;
		private string _section;
		private PullData _pullData;
		private DialogResult _dialogResult;


		public FactorsOrSchemesForm(string section, EnumForGUI enumForGUI, PullData pullData)
		{
			InitializeComponent();
			_section = section;
			FactorOrSchemeLabel.Text = enumForGUI.ToString();
			_pullData = pullData;
		}

		public DialogResult ShowForm()
		{
			this.ShowDialog();
			return _dialogResult;
		}

		private void FactorsOrSchemesForm_Load(object sender, EventArgs e)
		{
			SectionNameLabel.Text = _section;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			switch(_enumforGUI)
			{
				case EnumForGUI.Scheme:
					{
						AddScheme addScheme = new AddScheme();
						addScheme.Show();

						break;
					}
				case EnumForGUI.Factor:
					{

						break;
					}
			}
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{

		}

		private void SaveButton_Click(object sender, EventArgs e)
		{

		}

		private void DownloadButton_Click(object sender, EventArgs e)
		{

		}

		private void AcceptingButton_Click(object sender, EventArgs e)
		{
			_dialogResult = DialogResult.OK;
			switch (_enumforGUI)
			{
				case EnumForGUI.Scheme:
					{
						this.Hide();

						break;
					}
				case EnumForGUI.Factor:
					{
						this.Close();
						break;
					}
			}
			
		}

		private void BackButton_Click(object sender, EventArgs e)
		{
			_dialogResult = DialogResult.Cancel;
			this.Hide();
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{

		}
	}
}

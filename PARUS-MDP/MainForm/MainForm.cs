using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ExcelForParus;

namespace GUI
{
	public partial class MainForm : Form
	{
		private Section _section;
		private OutputFileGenerate _outputFileGenerate;
		private FolderBrowserDialog _folderBrowserDialog;
		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			_section = new Section();
			_outputFileGenerate = new OutputFileGenerate();
			_folderBrowserDialog = new FolderBrowserDialog();
		}

		private void FolderButton_Click(object sender, EventArgs e)
		{
			this.Hide();
			_section.ShowDialog();
		}

		private void OutputFileButton_Click(object sender, EventArgs e)
		{
			this.Hide();
			_outputFileGenerate.ShowDialog();
			this.Show();
		}

		private void PARUSsampleButton_Click(object sender, EventArgs e)
		{
			
			if(_folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				CreateExcelForParus createParusFile;
				createParusFile = new CreateExcelForParus(_folderBrowserDialog.SelectedPath);
				if (createParusFile.ErrorList.Count > 0)
				{
					ErrorWindow errorWindow = new ErrorWindow(createParusFile.ErrorList);
					errorWindow.ShowDialog();
				}
				
					
				
			}
			
		}
	}
}

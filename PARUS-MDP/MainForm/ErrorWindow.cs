using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GUI
{
	public partial class ErrorWindow : Form
	{
		private List<string> _errorList;
		public ErrorWindow(List<string> errorList)
		{
			InitializeComponent();
			_errorList = errorList;
			ErrorListBox.DataSource = errorList;
			ErrorListBox.HorizontalScrollbar = true;


		}

		private void ErrorWindow_Load(object sender, EventArgs e)
		{

		}

		private void CloseButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void TextButton_Click(object sender, EventArgs e)
		{
			var createFileDialog = new SaveFileDialog
			{
				Filter = "txt files (*.txt)|*.txt",
				RestoreDirectory = true,
			};
			if (createFileDialog.ShowDialog() == DialogResult.OK)
			{
				using (StreamWriter sw = new StreamWriter(Path.GetFullPath(createFileDialog.FileName), false, Encoding.Default))
				{
					foreach (string error in _errorList)
					{
						sw.WriteLine(error);
					}
				}
			}
		}
	}
}

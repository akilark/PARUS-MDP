using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using OfficeOpenXml;
using DataTypes;
using WorkWithDataSource;
using OutputFileStructure;

namespace GUI
{
	public partial class OutputFileGenerate : Form
	{
		private List<CheckBox> _winterCheckBox;
		private List<CheckBox> _summerCheckBox;
		public OutputFileGenerate()
		{
			InitializeComponent();
		}

		private void PathTextBox_TextChanged(object sender, EventArgs e)
		{

		}

		private void OutputFileGenerate_Load(object sender, EventArgs e)
		{
			_winterCheckBox = new List<CheckBox>();
			_summerCheckBox = new List<CheckBox>();
			_winterCheckBox.Add(checkBox_25);
			_winterCheckBox.Add(checkBox_20);
			_winterCheckBox.Add(checkBox_15);
			_winterCheckBox.Add(checkBox_10);
			_winterCheckBox.Add(checkBox_5);
			_winterCheckBox.Add(checkBox0);
			_winterCheckBox.Add(checkBox5);
			_winterCheckBox.Add(checkBox10);
			
			_summerCheckBox.Add(checkBox15);
			_summerCheckBox.Add(checkBox20);
			_summerCheckBox.Add(checkBox25);
			_summerCheckBox.Add(checkBox30);
			_summerCheckBox.Add(checkBox35);
			_summerCheckBox.Add(checkBox40);
			_summerCheckBox.Add(checkBox45);
		}

		private void BackButton_Click(object sender, EventArgs e)
		{
			this.Hide();
		}

		private void ExploreButton_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				PathTextBox.Text = folderBrowserDialog.SelectedPath;
			}
		}

		private void FormButton_Click(object sender, EventArgs e)
		{
			if (SamplePathtextBox.Text != "" && PathTextBox.Text != "")
			{
				var temperature = TemperatureArrayCreate();
				SampleSection sampleSection;
				if (TemperatureCheckBox.Checked)
				{
					sampleSection = new SampleSection(PathTextBox.Text, SamplePathtextBox.Text, temperature);
				}
				else
				{
					sampleSection = new SampleSection(PathTextBox.Text, SamplePathtextBox.Text);
				}
				string filePath = PathTextBox.Text + @"\Сформированная структура.xlsx";
				var openFileDialog = new SaveFileDialog
				{
					Filter = "txt files (*.xlsx)|*.xlsx",
					InitialDirectory = PathTextBox.Text,
					RestoreDirectory = true,
				};
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					filePath = openFileDialog.FileName;
				}
				sampleSection.SaveSampleWithStructure(filePath);

				
				FileInfo sampleFile = new FileInfo(SamplePathtextBox.Text);
				var excelSample = new ExcelPackage(sampleFile);
				FileInfo outputFile = new FileInfo(filePath);
				var excelOutputFile = new ExcelPackage(outputFile);
				PullData pullData = new PullData("Тест_2");
				WorkWithCellsGroup workWithCellsGroup;
				if (TemperatureCheckBox.Checked)
				{
					workWithCellsGroup = new WorkWithCellsGroup(PathTextBox.Text, excelOutputFile, sampleSection.FactorsInSample(),
					pullData.Schemes, temperature);
				}
				else
				{
					workWithCellsGroup = new WorkWithCellsGroup(PathTextBox.Text, excelOutputFile, sampleSection.FactorsInSample(),
					pullData.Schemes);
				}

				
				SampleControlActions sampleControlActions = new SampleControlActions(excelSample);

				var controlActionWithNeedDirection = sampleControlActions.ControlActionRows;

				var compare = new CompareControlActions(pullData.Imbalances, controlActionWithNeedDirection, pullData.AOPOlist, pullData.AOCNlist, true);

				progressBar.Visible = true;
				progressBar.Maximum = workWithCellsGroup.PathAndDislocation.Count;
				foreach (CellsGroup cellsGroupOneTemperature in workWithCellsGroup.PathAndDislocation)
				{
					InfoFromParusFile infoFromParusFile = new InfoFromParusFile(cellsGroupOneTemperature, compare.Imbalances, compare.AOPOlist, compare.AOCNlist, compare.LAPNYlist,
					false, ref excelOutputFile);
					progressBar.Value += 1;
				}
				
				FileInfo file = new FileInfo(filePath);
				excelOutputFile.SaveAs(file);
			}
			MessageBox.Show("Файл сформирован");
		}

		private string[] TemperatureArrayCreate()
		{
			var temperature = new string[0];
			foreach (CheckBox checkBox in _winterCheckBox)
			{
				if (checkBox.Checked)
				{
					Array.Resize(ref temperature, temperature.Length + 1);
					temperature[temperature.Length - 1] = checkBox.Text;
				}
			}

			foreach (CheckBox checkBox in _summerCheckBox)
			{
				if (checkBox.Checked)
				{
					Array.Resize(ref temperature, temperature.Length + 1);
					temperature[temperature.Length - 1] = checkBox.Text;
				}
			}
			return temperature;
		}

		private void SelectButton_Click(object sender, EventArgs e)
		{
			foreach(CheckBox checkBox in _winterCheckBox)
			{
				checkBox.Checked = true;
			}

			foreach(CheckBox checkBox in _summerCheckBox)
			{
				checkBox.Checked = true;
			}
		}

		private void AwayButton_Click(object sender, EventArgs e)
		{

			foreach (CheckBox checkBox in _winterCheckBox)
			{
				checkBox.Checked = false;
			}

			foreach (CheckBox checkBox in _summerCheckBox)
			{
				checkBox.Checked = false;
			}

		}

		private void WinterButton_Click(object sender, EventArgs e)
		{

			foreach (CheckBox checkBox in _winterCheckBox)
			{
				checkBox.Checked = true;
			}

			foreach (CheckBox checkBox in _summerCheckBox)
			{
				checkBox.Checked = false;
			}
		}

		private void SummerButton_Click(object sender, EventArgs e)
		{
			foreach (CheckBox checkBox in _winterCheckBox)
			{
				checkBox.Checked = false;
			}

			foreach (CheckBox checkBox in _summerCheckBox)
			{
				checkBox.Checked = true;
			}
		}

		private void TemperatureCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			TemperatureGroupBox.Enabled = TemperatureCheckBox.Checked;
		}

		private void EmergencyLineDisconnection_CheckedChanged(object sender, EventArgs e)
		{
			
		}

		private void SamplePathButton_Click(object sender, EventArgs e)
		{

			var createFileDialog = new OpenFileDialog
			{
				Filter = "Excel File (*.xlsx)|*.xlsx",
				InitialDirectory = PathTextBox.Text,
				RestoreDirectory = true,
			};
			if (createFileDialog.ShowDialog() == DialogResult.OK)
			{
				SamplePathtextBox.Text = createFileDialog.FileName;
			}
		}

		private void OutputFileGenerate_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}
	}
}

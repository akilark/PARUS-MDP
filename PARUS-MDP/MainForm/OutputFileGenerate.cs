using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using OfficeOpenXml;
using DataTypes;
using OutputFileStructure;
using WorkWithCatalog;

namespace GUI
{
	public partial class OutputFileGenerate : Form
	{
		private List<CheckBox> _winterCheckBox;
		private List<CheckBox> _summerCheckBox;
		private ErrorWindow _errorWindow;
		public OutputFileGenerate()
		{
			InitializeComponent();
		}

		private void PathTextBox_TextChanged(object sender, EventArgs e)
		{
			if(PathTextBox.Text.Length != 0 && SamplePathtextBox.Text.Length != 0)
			{
				FormButton.Enabled = true;
			}
			else
			{
				FormButton.Enabled = false;
			}
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
			errorButton.Visible = false;
			var temperature = TemperatureArrayCreate();
			SampleSection sampleSection;
			try
			{
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
				SectionInfoToXml sectionInfoToXml = new SectionInfoToXml(PathTextBox.Text + @"\configurationFile.kek");
				SectionFromDataSource sectionFromDataSource = sectionInfoToXml.ReadFileInfo();
				WorkWithCellsGroup workWithCellsGroup;
				if (TemperatureCheckBox.Checked)
				{
					workWithCellsGroup = new WorkWithCellsGroup(PathTextBox.Text, excelOutputFile, sampleSection.FactorsInSample(),
					sectionFromDataSource.Schemes, temperature);
				}
				else
				{
					workWithCellsGroup = new WorkWithCellsGroup(PathTextBox.Text, excelOutputFile, sampleSection.FactorsInSample(),
					sectionFromDataSource.Schemes);
				}


				SampleControlActions sampleControlActions = new SampleControlActions(excelSample);

				var controlActionWithNeedDirection = sampleControlActions.ControlActionRows;

				var compare = new CompareControlActions(sectionFromDataSource.Imbalances, controlActionWithNeedDirection, sectionFromDataSource.AOPOlist, sectionFromDataSource.AOCNlist, true);

				progressBar.Visible = true;
				progressBar.Value = 0;
				progressBar.Maximum = workWithCellsGroup.PathAndDislocation.Count;
				List<string> errorList = new List<string>();

				foreach (CellsGroup cellsGroupOneTemperature in workWithCellsGroup.PathAndDislocation)
				{
					InfoFromParusFile infoFromParusFile = new InfoFromParusFile(cellsGroupOneTemperature,
						ImbalancesWithRightDirection(compare.Imbalances, cellsGroupOneTemperature.Direction),
						AOPOwithRightDirection(compare.AOPOlist, cellsGroupOneTemperature.Direction),
						AOCNwithRightDirection(compare.AOCNlist, cellsGroupOneTemperature.Direction),
						compare.LAPNYlist, !EmergencyLineDisconnection.Checked, ref excelOutputFile);
					progressBar.Value += 1;
					foreach (string error in infoFromParusFile.ErrorList)
					{
						errorList.Add(error);
					}
				}

				FileInfo file = new FileInfo(filePath);
				excelOutputFile.SaveAs(file);

				if (errorList.Count > 0)
				{
					MessageBox.Show("Файл сформирован с ошибками", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error,
					MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
					_errorWindow = new ErrorWindow(errorList);
					_errorWindow.ShowDialog();
					errorButton.Visible = true;
				}
				else
				{
					MessageBox.Show("Файл сформирован", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.None,
					MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
				}
			}
			catch (IOException exception)
			{
				progressBar.Visible = false;
				progressBar.Value = 0;
				MessageBox.Show("Приложение пытается использовать файл, который открыт пользователем", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error,
					MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
			}
			
			
		}

		private List<Imbalance> ImbalancesWithRightDirection(List<Imbalance> imbalances, string direction)
		{
			var outputList = new List<Imbalance>();
			foreach(Imbalance imbalance in imbalances)
			{
				if(imbalance.ImbalanceValue == null)
				{
					continue;
				}
				if(imbalance.ImbalanceValue.Direction.ToLower() == direction.ToLower())
				{
					outputList.Add(imbalance);
				}
			}


			return outputList;
		}

		private List<AOPO> AOPOwithRightDirection(List<AOPO> aopoList, string direction)
		{
			var outputList = new List<AOPO>();
			foreach(AOPO aopo in aopoList)
			{
				if(aopo.Automatic == null)
				{
					continue;
				}
				if(aopo.Automatic.Direction.ToLower() == direction.ToLower())
				{
					outputList.Add(aopo);
				}
			}
			return outputList;
		}

		private List<AOCN> AOCNwithRightDirection(List<AOCN> aocnList, string direction)
		{
			var outputList = new List<AOCN>();
			foreach(AOCN aocn in aocnList)
			{
				if(aocn.Automatic == null)
				{
					continue;
				}
				if(aocn.Automatic.Direction.ToLower() == direction.ToLower())
				{
					outputList.Add(aocn);
				}
			}
			return outputList;
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
			if(TemperatureCheckBox.Checked)
			{
				bool checkFlag = false;
				foreach (CheckBox checkBox in _winterCheckBox)
				{
					if (checkBox.Checked)
					{
						checkFlag = true;
					}
				}
				foreach (CheckBox checkBox in _summerCheckBox)
				{
					if (checkBox.Checked)
					{
						checkFlag = true;
					}
				}
				
				if(checkFlag)
				{
					PathTextBox_TextChanged(sender, e);
				}
				else
				{
					FormButton.Enabled = false;
				}
			}
			else
			{
				if (!FormButton.Enabled)
				{
					PathTextBox_TextChanged(sender, e);
				}
			}
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

		private void ErrorButton_Click(object sender, EventArgs e)
		{
			this.Enabled = false;
			_errorWindow.ShowDialog();
			this.Enabled = true;
		}
	}
}

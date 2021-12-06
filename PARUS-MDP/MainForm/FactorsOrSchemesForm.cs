using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WorkWithDataSource;
using DataTypes;

namespace GUI
{
	public partial class FactorsOrSchemesForm : Form
	{
		private EnumForGUI _enumforGUI;
		private string _section;
		private PullData _pullData;
		private DialogResult _dialogResult;
		private List<Scheme> _schemes;
		private List<FactorsWithDirection> _factors;
		private List<Scheme> _deletedScheme;
		private List<Scheme> _editedScheme;
		private List<FactorsWithDirection> _deletedFactors;
		private List<FactorsWithDirection> _editedFactors;
		private bool _checkFlag;



		public FactorsOrSchemesForm(string section, EnumForGUI enumForGUI, PullData pullData)
		{
			InitializeComponent();
			_section = section;
			_pullData = pullData;
			_deletedScheme = new List<Scheme>();
			_editedScheme = new List<Scheme>();
			_factors = new List<FactorsWithDirection>();
			_deletedFactors = new List<FactorsWithDirection>();
			_editedFactors = new List<FactorsWithDirection>();
			_enumforGUI = enumForGUI;

			switch (_enumforGUI)
			{
				case EnumForGUI.Scheme:
					{
						_checkFlag = false;
						_schemes = pullData.Schemes;
						FactorOrSchemeTreeView.CheckBoxes = true;
						TreeViewSchemeFill();
						_checkFlag = true;
						break;
					}
				case EnumForGUI.Factor:
					{
						FactorOrSchemeTreeView.CheckBoxes = false;
						_factors = pullData.Factors;
						TreeViewFactorsFill();
						break;
					}
			}

		}

		public List<Scheme> Schemes => _schemes;

		public List<FactorsWithDirection> Factors => _factors;

		public DialogResult ShowForm()
		{
			this.ShowDialog();
			FactorOrSchemeTreeView.ExpandAll();
			return _dialogResult;
		}

		private void FactorsOrSchemesForm_Load(object sender, EventArgs e)
		{
			SectionNameLabel.Text = _section;

			switch (_enumforGUI)
			{
				case EnumForGUI.Scheme:
					{
						FactorOrSchemeLabel.Text = "схем";
						break;
					}
				case EnumForGUI.Factor:
					{
						FactorOrSchemeLabel.Text = "факторов";
						break;
					}
			}
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			switch(_enumforGUI)
			{
				case EnumForGUI.Scheme:
					{
						AddScheme addScheme = new AddScheme(_schemes);
						this.Enabled = false;
						addScheme.ShowDialog();
						this.Enabled = true;
						if (addScheme.schemeAdd != null)
						{
							AddSchemeToEditedSchemeList(addScheme.schemeAdd);
							AddSchemeToTreeView(addScheme.schemeAdd);
						}
						break;
					}
				case EnumForGUI.Factor:
					{
						AddFactor addFactor = new AddFactor();
						addFactor.Show();
						break;
					}
			}
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			switch (_enumforGUI)
			{
				case EnumForGUI.Scheme:
					{
						Scheme schemeTmp = new Scheme();
						if(FactorOrSchemeTreeView.SelectedNode.Level == 0)
						{
							
							schemeTmp.SchemeName = FactorOrSchemeTreeView.SelectedNode.Text;
							schemeTmp.Disturbance = new List<(string, bool)>();
							foreach(TreeNode treeNode in FactorOrSchemeTreeView.SelectedNode.Nodes)
							{
								schemeTmp.Disturbance.Add((treeNode.Text, treeNode.Checked));
							}
							_deletedScheme.Add(schemeTmp);
							FactorOrSchemeTreeView.SelectedNode.Remove();
							foreach (Scheme scheme in _schemes)
							{
								if (scheme.SchemeName == schemeTmp.SchemeName)
								{
									_schemes.Remove(scheme);
									break;
								}
							}
						}
						else
						{
							schemeTmp.SchemeName = FactorOrSchemeTreeView.SelectedNode.Parent.Text;
							schemeTmp.Disturbance = new List<(string, bool)>();
							schemeTmp.Disturbance.Add((FactorOrSchemeTreeView.SelectedNode.Text, FactorOrSchemeTreeView.SelectedNode.Checked));
							_deletedScheme.Add(schemeTmp);
							FactorOrSchemeTreeView.SelectedNode.Remove();
							
							foreach (Scheme scheme in _schemes)
							{
								if (scheme.SchemeName == schemeTmp.SchemeName)
								{
									foreach ((string, bool) disturbance in scheme.Disturbance)
									{
										if (schemeTmp.Disturbance[0].Item1 == disturbance.Item1)
										{
											scheme.Disturbance.Remove(disturbance);
											break;
										}

									}
								}
							}
							
						}
						
						
						break;
					}
				case EnumForGUI.Factor:
					{
						FactorsWithDirection factor = new FactorsWithDirection();
						factor.FactorNameAndValues = new List<(string, string[])>();
						switch(FactorOrSchemeTreeView.SelectedNode.Level)
						{
							case 0:
								{
									factor.Direction = FactorOrSchemeTreeView.SelectedNode.Text;
									foreach (TreeNode factorNode in FactorOrSchemeTreeView.SelectedNode.Nodes)
									{
										var factorValues = new string[factorNode.Nodes.Count];
										var factorName = factorNode.Text;
										for (int i= 0; i< factorNode.Nodes.Count;i++)
										{
											factorValues[i] = factorNode.Nodes[i].Text;
										}
										factor.FactorNameAndValues.Add((factorName, factorValues));
										_deletedFactors.Add(factor);
										FactorOrSchemeTreeView.SelectedNode.Remove();
									}
									break;
								}
							case 1:
								{
									factor.Direction = FactorOrSchemeTreeView.SelectedNode.Parent.Text;
									var factorName = FactorOrSchemeTreeView.SelectedNode.Text;
									var factorValues = new string[FactorOrSchemeTreeView.SelectedNode.Nodes.Count];
									for (int i = 0; i < FactorOrSchemeTreeView.SelectedNode.Nodes.Count; i++)
									{
										factorValues[i] = FactorOrSchemeTreeView.SelectedNode.Nodes[i].Text;
									}
									factor.FactorNameAndValues.Add((factorName, factorValues));
									_deletedFactors.Add(factor);
									FactorOrSchemeTreeView.SelectedNode.Remove();
									break;
								}
							case 2:
								{
									factor.Direction = FactorOrSchemeTreeView.SelectedNode.Parent.Parent.Text;
									var factorName = FactorOrSchemeTreeView.SelectedNode.Parent.Text;
									bool directionFlag = false;
									bool factorFlag = false;
									for(int i = 0; i< _deletedFactors.Count; i++)
									{
										
									}
									string[] factorValues = new string[] { FactorOrSchemeTreeView.SelectedNode.Text };  
									factor.FactorNameAndValues.Add((factorName, factorValues));
									_deletedFactors.Add(factor);
									FactorOrSchemeTreeView.SelectedNode.Remove();
									break;

								}
						}

						break;
					}
			}
			
		}

		private void SaveButton_Click(object sender, EventArgs e)
		{

		}

		private void DownloadButton_Click(object sender, EventArgs e)
		{
			_editedScheme = new List<Scheme>();
			_deletedScheme = new List<Scheme>();
			FactorOrSchemeTreeView.Nodes.Clear();
			switch (_enumforGUI)
			{
				case EnumForGUI.Scheme:
					{
						_checkFlag = false;
						FactorOrSchemeTreeView.CheckBoxes = true;
						TreeViewSchemeFill();
						_checkFlag = true;
						break;
					}
				case EnumForGUI.Factor:
					{

						break;
					}
			}
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
						this.Hide();
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


		private void TreeViewSchemeFill()
		{
			for(int i = 0; i < _pullData.Schemes.Count; i ++)
			{
				FactorOrSchemeTreeView.Nodes.Add(_pullData.Schemes[i].SchemeName);
				
				
				for (int j = 0; j < _pullData.Schemes[i].Disturbance.Count; j ++)
				{
					FactorOrSchemeTreeView.Nodes[i].Nodes.Add(_pullData.Schemes[i].Disturbance[j].Item1);
					FactorOrSchemeTreeView.Nodes[i].Nodes[j].Checked = _pullData.Schemes[i].Disturbance[j].Item2;
				}
			}
			FactorOrSchemeTreeView.ExpandAll();
		}

		private void TreeViewFactorsFill()
		{
			for (int i = 0; i < _pullData.Factors.Count; i++)
			{
				FactorOrSchemeTreeView.Nodes.Add(_pullData.Factors[i].Direction);


				for (int j = 0; j < _pullData.Factors[i].FactorNameAndValues.Count; j++)
				{
					FactorOrSchemeTreeView.Nodes[i].Nodes.Add(_pullData.Factors[i].FactorNameAndValues[j].Item1);
					for(int z = 0; z < _pullData.Factors[i].FactorNameAndValues[j].Item2.Length; z++)
					{
						FactorOrSchemeTreeView.Nodes[i].Nodes[j].Nodes.Add(_pullData.Factors[i].FactorNameAndValues[j].Item2[z]);
					}
				}
			}
			FactorOrSchemeTreeView.ExpandAll();
		}


		private void FactorOrSchemeTreeView_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if(_checkFlag && e.Node.Level == 1)
			{
				Scheme schemeTmp = new Scheme();
				schemeTmp.SchemeName = e.Node.Parent.Text;
				schemeTmp.Disturbance = new List<(string, bool)>();
				schemeTmp.Disturbance.Add((e.Node.Text, e.Node.Checked));
				AddSchemeToEditedSchemeList(schemeTmp);
			}
		}

		private void AddSchemeToEditedSchemeList(Scheme schemeTmp)
		{
			foreach (Scheme scheme in _editedScheme)
			{
				if (scheme.SchemeName == schemeTmp.SchemeName)
				{
					if (schemeTmp.Disturbance[0].Item1 == scheme.Disturbance[0].Item1)
					{
						_editedScheme.Remove(scheme);
						break;
					}
				}
			}
			_editedScheme.Add(schemeTmp);
		}
		
		private void AddSchemeToTreeView(Scheme schemeTmp)
		{
			if(schemeTmp != null)
			{
				bool schemeAppear = false;
				for (int i = 0; i < FactorOrSchemeTreeView.Nodes.Count; i++)
				{
					if (schemeTmp.SchemeName == FactorOrSchemeTreeView.Nodes[i].Text)
					{
						schemeAppear = true;
						bool disturbanceAppear = false;
						for (int j = 0; j < FactorOrSchemeTreeView.Nodes[i].Nodes.Count; j++)
						{
							if (schemeTmp.Disturbance[0].Item1 == FactorOrSchemeTreeView.Nodes[i].Nodes[j].Text)
							{
								FactorOrSchemeTreeView.Nodes[i].Nodes[j].Checked = schemeTmp.Disturbance[0].Item2;
								disturbanceAppear = true;
							}

						}
						if (!disturbanceAppear)
						{
							FactorOrSchemeTreeView.Nodes[i].Nodes.Add(schemeTmp.Disturbance[0].Item1);
							FactorOrSchemeTreeView.Nodes[i].Nodes[FactorOrSchemeTreeView.Nodes[i].Nodes.Count - 1].Checked = schemeTmp.Disturbance[0].Item2;
						}
					}

				}
				if (!schemeAppear)
				{
					FactorOrSchemeTreeView.Nodes.Add(schemeTmp.SchemeName);
					FactorOrSchemeTreeView.Nodes[FactorOrSchemeTreeView.Nodes.Count - 1].Nodes.Add(schemeTmp.Disturbance[0].Item1);
					FactorOrSchemeTreeView.Nodes[FactorOrSchemeTreeView.Nodes.Count - 1].
						Nodes[FactorOrSchemeTreeView.Nodes[FactorOrSchemeTreeView.Nodes.Count - 1].Nodes.Count - 1].Checked = schemeTmp.Disturbance[0].Item2;
				}

				FactorOrSchemeTreeView.ExpandAll();
			}
		}

		private void FactorsOrSchemesForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			_dialogResult = DialogResult.Abort;
		}

		private void FactorsOrSchemesForm_FormClosing(object sender, FormClosingEventArgs e)
		{

		}
	}
}

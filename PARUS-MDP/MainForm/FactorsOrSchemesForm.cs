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
						if (addScheme.SchemeAdd != null)
						{
							AddScheme(addScheme.SchemeAdd);
						}
						break;
					}
				case EnumForGUI.Factor:
					{
						AddFactor addFactor = new AddFactor(_factors);
						addFactor.ShowDialog();
						if(addFactor.FactorAdd !=null)
						{
							AddFactors(addFactor.FactorAdd);
						}
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
										for(int i= 0; i< _factors.Count; i ++)
										{
											if(_factors[i].Direction == factor.Direction)
											{
												_factors.Remove(_factors[i]);
											}
										}
									}
									FactorOrSchemeTreeView.SelectedNode.Remove();
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

									for (int i = 0; i < _factors.Count; i++)
									{
										if (_factors[i].Direction == factor.Direction)
										{
											for(int j = 0; j< _factors[i].FactorNameAndValues.Count; j++)
											{
												if(_factors[i].FactorNameAndValues[j].Item1 == factorName)
												{
													_factors[i].FactorNameAndValues.Remove(_factors[i].FactorNameAndValues[j]);
												}
											}
											
										}
									}

									FactorOrSchemeTreeView.SelectedNode.Remove();
									break;
								}
							case 2:
								{
									factor.Direction = FactorOrSchemeTreeView.SelectedNode.Parent.Parent.Text;
									var factorName = FactorOrSchemeTreeView.SelectedNode.Parent.Text;
									string[] factorValues = new string[] { FactorOrSchemeTreeView.SelectedNode.Text };
									bool factorFlag = false;
									for(int i = 0; i< _deletedFactors.Count; i++)
									{
										if (_deletedFactors[i].Direction == factor.Direction)
										{
											if(_deletedFactors[i].FactorNameAndValues == null)
											{
												_deletedFactors[i].FactorNameAndValues = new List<(string, string[])>();
												factor.FactorNameAndValues.Add((factorName, factorValues));
											}
											for (int j = 0; j < _deletedFactors[i].FactorNameAndValues.Count; j++)
											{
												if(_deletedFactors[i].FactorNameAndValues[j].Item1 == factorName)
												{
													string[] factorsValues = new string[_deletedFactors[i].FactorNameAndValues[j].Item2.Length + 1];
													for(int z = 0; z < _deletedFactors[i].FactorNameAndValues[j].Item2.Length; z ++)
													{
														factorsValues[z] = _deletedFactors[i].FactorNameAndValues[j].Item2[z];
													}
													factorsValues[_deletedFactors[i].FactorNameAndValues[j].Item2.Length] = FactorOrSchemeTreeView.SelectedNode.Text;
													_deletedFactors[i].FactorNameAndValues[j] = (factorName, factorsValues);
													factorFlag = true;
												}	
											}
										}
									}
									 
									if(!factorFlag)
									{
										factor.FactorNameAndValues.Add((factorName, factorValues));
										_deletedFactors.Add(factor);
									}

									for (int i = 0; i < _factors.Count; i++)
									{
										if (_factors[i].Direction == factor.Direction)
										{
											for (int j = 0; j < _factors[i].FactorNameAndValues.Count; j++)
											{
												if (_factors[i].FactorNameAndValues[j].Item1 == factorName)
												{
													for(int z = 0; z < _factors[i].FactorNameAndValues[j].Item2.Length; z++)
													{
														string[] factorsValues = new string[0];
														if (_factors[i].FactorNameAndValues[j].Item2[z] != factorValues[0])
														{
															Array.Resize(ref factorsValues, factorsValues.Length + 1);
															factorsValues[factorsValues.Length - 1] = _factors[i].FactorNameAndValues[j].Item2[z];
														}
														string factorNameTmp = _factors[i].FactorNameAndValues[j].Item1;

														_factors[i].FactorNameAndValues.Remove(_factors[i].FactorNameAndValues[j]);
														_factors[i].FactorNameAndValues.Add((factorNameTmp, factorsValues));
													}
													
												}
											}

										}
									}
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
			switch(_enumforGUI)
			{
				case EnumForGUI.Scheme:
					{
						foreach (Scheme scheme in _editedScheme)
						{
							foreach ((string, bool) disturbance in scheme.Disturbance)
							{
								ChangeData changeData = new ChangeData(_section, scheme.SchemeName, disturbance.Item1, disturbance.Item2);
								changeData.Insert();
							}
						}
						foreach (Scheme scheme in _deletedScheme)
						{
							foreach((string,bool) disturbance in scheme.Disturbance)
							{
								ChangeData changeData = new ChangeData(_section, scheme.SchemeName, disturbance.Item1, disturbance.Item2);
								changeData.Delete();
							}
						}						
						break;
					}
				case EnumForGUI.Factor:
					{
						foreach (FactorsWithDirection factor in _editedFactors)
						{
							foreach ((string, string[]) factorNameAndValues in factor.FactorNameAndValues)
							{
								foreach (string valueFactor in factorNameAndValues.Item2)
								{
									ChangeData changeData = new ChangeData(_section, factor.Direction, factorNameAndValues.Item1, valueFactor);
									changeData.Insert();
								}
							}
						}
						foreach (FactorsWithDirection factor in _deletedFactors)
						{
							foreach((string, string[]) factorNameAndValues in factor.FactorNameAndValues)
							{
								foreach(string valueFactor in factorNameAndValues.Item2)
								{
									ChangeData changeData = new ChangeData(_section, factor.Direction, factorNameAndValues.Item1, valueFactor);
									changeData.Delete();
								}
							}
						}
						break;
					}
			}
		}

		private void DownloadButton_Click(object sender, EventArgs e)
		{
			_editedScheme = new List<Scheme>();
			_deletedScheme = new List<Scheme>();
			_deletedFactors = new List<FactorsWithDirection>();
			_editedFactors = new List<FactorsWithDirection>();
			FactorOrSchemeTreeView.Nodes.Clear();
			switch (_enumforGUI)
			{
				case EnumForGUI.Scheme:
					{
						_checkFlag = false;
						FactorOrSchemeTreeView.CheckBoxes = true;
						TreeViewSchemeFill();
						_checkFlag = true;
						_schemes = _pullData.Schemes;
						break;
					}
				case EnumForGUI.Factor:
					{
						TreeViewFactorsFill();
						_factors = _pullData.Factors;
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
			_pullData.PullSchemes();
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
			_pullData.PullFactors();
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
			if(_checkFlag && e.Node.Level == 1 && _enumforGUI == EnumForGUI.Scheme)
			{
				Scheme schemeTmp = new Scheme();
				schemeTmp.SchemeName = e.Node.Parent.Text;
				schemeTmp.Disturbance = new List<(string, bool)>();
				schemeTmp.Disturbance.Add((e.Node.Text, e.Node.Checked));
				AddSchemeToSchemeList(schemeTmp, ref _schemes);
				AddSchemeToSchemeList(schemeTmp, ref _editedScheme);
			}
		}

		private void AddSchemeToSchemeList(Scheme schemeTmp, ref List<Scheme> schemes)
		{
			bool schemeNameUnique = true;
			foreach (Scheme scheme in schemes)
			{
				if (scheme.SchemeName == schemeTmp.SchemeName)
				{
					bool valueAppear = false;
					(string, bool) disturbanceTmp = ("", false);
					schemeNameUnique = false;
					foreach((string, bool) disturbance in scheme.Disturbance)
					{
						if (schemeTmp.Disturbance[0].Item1 == disturbance.Item1)
						{
							disturbanceTmp = disturbance;
							valueAppear = true;
						}
					}
					if(valueAppear)
					{
						scheme.Disturbance.Remove(disturbanceTmp);
						scheme.Disturbance.Add(schemeTmp.Disturbance[0]);
					}
					else
					{
						scheme.Disturbance.Add(schemeTmp.Disturbance[0]);
					}

				}
			}
			if(schemeNameUnique)
			{
				schemes.Add(schemeTmp);
			}
			
		}

		private void AddScheme(Scheme schemeTmp)
		{
			AddSchemeToSchemeList(schemeTmp, ref _schemes);
			AddSchemeToSchemeList(schemeTmp, ref _editedScheme);
			AddSchemeToTreeView(schemeTmp);
		}
		private void AddSchemeToTreeView(Scheme schemeTmp)
		{
			if (schemeTmp != null)
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

		public void AddFactors(FactorsWithDirection factor)
		{
			AddFactorToFactorList(factor, ref _factors);
			AddFactorToFactorList(factor, ref _editedFactors);
			
		}


		private void AddFactorToFactorList(FactorsWithDirection factor, ref List<FactorsWithDirection> factorsWithDirections)
		{
			bool uniqueDirection = true;
			for (int i = 0; i < factorsWithDirections.Count; i++)
			{
				if (factorsWithDirections[i].Direction == factor.Direction)
				{
					bool uniqueFactor = true;
					for (int j = 0; j < factorsWithDirections[i].FactorNameAndValues.Count; j++)
					{
						if (factorsWithDirections[i].FactorNameAndValues[j].Item1 == factor.FactorNameAndValues[0].Item1)
						{
							uniqueFactor = false;
							string[] factorValues = factorsWithDirections[i].FactorNameAndValues[j].Item2;
							bool valueFlag = true;
							for (int z = 0; z < factorsWithDirections[i].FactorNameAndValues[j].Item2.Length; z++)
							{
								if (factorsWithDirections[i].FactorNameAndValues[j].Item2[z] == factor.FactorNameAndValues[0].Item2[0])
								{
									valueFlag = false;
								}
							}
							if (valueFlag)
							{
								Array.Resize(ref factorValues, factorValues.Length + 1);
								factorValues[factorValues.Length - 1] = factor.FactorNameAndValues[0].Item2[0];
								factorsWithDirections[i].FactorNameAndValues.Remove(factorsWithDirections[i].FactorNameAndValues[j]);
								factorsWithDirections[i].FactorNameAndValues.Add((factor.FactorNameAndValues[0].Item1, factorValues));
								break;
							}
						}
					}
					if (uniqueFactor)
					{
						factorsWithDirections[i].FactorNameAndValues.Add(factor.FactorNameAndValues[0]);
					}
					uniqueDirection = false;
				}
			}
			if(uniqueDirection)
			{
				factorsWithDirections.Add(factor);
			}
			AddFactorToTreeView(factor);
		}

		private void AddFactorToTreeView(FactorsWithDirection factor)
		{
			if (factor != null)
			{
				bool directionAppear = false;
				bool factorAppear = false;
				for (int i = 0; i < FactorOrSchemeTreeView.Nodes.Count; i++)
				{
					if (factor.Direction == FactorOrSchemeTreeView.Nodes[i].Text)
					{
						directionAppear = true;
						
						for (int j = 0; j < FactorOrSchemeTreeView.Nodes[i].Nodes.Count; j++)
						{
							if (factor.FactorNameAndValues[0].Item1 == FactorOrSchemeTreeView.Nodes[i].Nodes[j].Text)
							{
								factorAppear = true;
								bool factorValueAppear = false;
								for(int z = 0; z < FactorOrSchemeTreeView.Nodes[i].Nodes[j].Nodes.Count; z++)
								{
									if(factor.FactorNameAndValues[0].Item2[0] == FactorOrSchemeTreeView.Nodes[i].Nodes[j].Nodes[z].Text)
									{
										factorValueAppear = true;
									}
								}
								if(!factorValueAppear)
								{
									FactorOrSchemeTreeView.Nodes[i].Nodes[j].Nodes.Add(factor.FactorNameAndValues[0].Item2[0]);
								}
							}
						}
						if (!factorAppear)
						{
							FactorOrSchemeTreeView.Nodes[i].Nodes.Add(factor.FactorNameAndValues[0].Item1);
							FactorOrSchemeTreeView.Nodes[i].Nodes[FactorOrSchemeTreeView.Nodes[i].Nodes.Count - 1].Nodes.Add(factor.FactorNameAndValues[0].Item2[0]);
						}
					}

				}
				if (!directionAppear)
				{
					FactorOrSchemeTreeView.Nodes.Add(factor.Direction);
					FactorOrSchemeTreeView.Nodes[FactorOrSchemeTreeView.Nodes.Count - 1].Nodes.Add(factor.FactorNameAndValues[0].Item1);
					FactorOrSchemeTreeView.Nodes[FactorOrSchemeTreeView.Nodes.Count - 1].
						Nodes[FactorOrSchemeTreeView.Nodes[FactorOrSchemeTreeView.Nodes.Count - 1].Nodes.Count - 1].Nodes.Add(factor.FactorNameAndValues[0].Item2[0]);
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

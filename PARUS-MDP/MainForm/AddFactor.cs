using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DataTypes;

namespace GUI
{
	public partial class AddFactor : Form
	{
		private FactorsWithDirection _newFactor;
		private List<FactorsWithDirection> _factors;
		
		public AddFactor(List<FactorsWithDirection> factors)
		{
			_factors = factors;
			InitializeComponent();
			List<string> directions = new List<string>();
			foreach(FactorsWithDirection factor in factors)
			{
				directions.Add(factor.Direction);
			}
			DirectionComboBox.DataSource = directions;
		}

		public FactorsWithDirection FactorAdd => _newFactor;
		private void AddFactor_Load(object sender, EventArgs e)
		{

		}

		private void AddingButton_Click(object sender, EventArgs e)
		{
			_newFactor = new FactorsWithDirection();
			List<(string, string[])> factorAndValue = new List<(string, string[])>();
			ErrorLabel.Visible = false;
			if (FactorComboBox.Text =="" || DirectionComboBox.Text =="" || FactorValueTextBox.Text =="")
			{
				ErrorLabel.Visible = true;
			}
			else
			{
				_newFactor.Direction = DirectionComboBox.Text;
				factorAndValue.Add((FactorComboBox.Text, new string[] { FactorValueTextBox.Text }));
				_newFactor.FactorNameAndValues = factorAndValue;
			}
			this.Close();
		}

		private void SectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			List<string> factors = new List<string>();
			for (int i = 0; i < _factors.Count; i++)
			{
				if (_factors[i].Direction == DirectionComboBox.Text)
				{
					foreach ((string,string[]) factor in _factors[i].FactorNameAndValues)
					{
						factors.Add(factor.Item1);
					}
					FactorComboBox.DataSource = factors;
				}
			}
		}

		private void SectionComboBox_TextChanged(object sender, EventArgs e)
		{
			FactorComboBox.ResetText();
		}
	}
}

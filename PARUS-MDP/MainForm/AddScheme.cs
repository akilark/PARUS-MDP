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
	public partial class AddScheme : Form
	{
		private Scheme _scheme;
		private List<string> _schemeName;
		private List<Scheme> _schemes;
		public AddScheme(List<Scheme> schemes)
		{
			_schemes = schemes;
			InitializeComponent();
			_schemeName = new List<string>();
			for(int i = 0; i < _schemes.Count; i ++ )
			{
				_schemeName.Add(_schemes[i].SchemeName);
			}
			SchemeComboBox.DataSource = _schemeName;
		}

		public Scheme SchemeAdd => _scheme;
		private void AddScheme_Load(object sender, EventArgs e)
		{
			
			
		}

		private void AddingButton_Click(object sender, EventArgs e)
		{
			_scheme = new Scheme();
			_scheme.Disturbance = new List<(string, bool)>();
			ErrorLabel.Visible = false;
			if (SchemeComboBox.Text != "" )
			{
				if (DisturbanceComboBox.Text != "")
				{
					bool raduoButtoResult = false;
					if (PAradioButtonYes.Checked)
					{
						raduoButtoResult =true;
					}
					
					_scheme.SchemeName = SchemeComboBox.Text;
					_scheme.Disturbance.Add((DisturbanceComboBox.Text, raduoButtoResult));
				}
				else
				{
					ErrorLabel.Visible = true;
				}

			}
			else
			{
				ErrorLabel.Visible = true;
			}
			this.Close();
		}

		private void SchemeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			List <string> disturbances = new List<string>();
			for (int i = 0; i < _schemes.Count; i++)
			{
				if (_schemes[i].SchemeName == SchemeComboBox.Text)
				{
					foreach((string,bool) disturbance in _schemes[i].Disturbance)
					{
						disturbances.Add(disturbance.Item1);
					}
					DisturbanceComboBox.DataSource = disturbances;
				}
			}
			
		}

		private void SchemeComboBox_TextChanged(object sender, EventArgs e)
		{
			DisturbanceComboBox.ResetText();
		}
	}
}

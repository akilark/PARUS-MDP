using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace GUI
{
	public partial class Section : Form
	{
		public Section()
		{
			InitializeComponent();
		}

		private void Section_Load(object sender, EventArgs e)
		{

		}

		private void CancelingButton_Click(object sender, EventArgs e)
		{
			this.Hide();
		}
	}
}

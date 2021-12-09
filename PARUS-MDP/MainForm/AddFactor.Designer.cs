
namespace GUI
{
	partial class AddFactor
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.DirectionComboBox = new System.Windows.Forms.ComboBox();
			this.FactorComboBox = new System.Windows.Forms.ComboBox();
			this.FactorValueTextBox = new System.Windows.Forms.TextBox();
			this.AddingButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.ErrorLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// DirectionComboBox
			// 
			this.DirectionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DirectionComboBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.DirectionComboBox.FormattingEnabled = true;
			this.DirectionComboBox.Location = new System.Drawing.Point(13, 35);
			this.DirectionComboBox.Name = "DirectionComboBox";
			this.DirectionComboBox.Size = new System.Drawing.Size(399, 25);
			this.DirectionComboBox.TabIndex = 0;
			this.DirectionComboBox.SelectedIndexChanged += new System.EventHandler(this.SectionComboBox_SelectedIndexChanged);
			this.DirectionComboBox.TextChanged += new System.EventHandler(this.SectionComboBox_TextChanged);
			// 
			// FactorComboBox
			// 
			this.FactorComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.FactorComboBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.FactorComboBox.FormattingEnabled = true;
			this.FactorComboBox.Location = new System.Drawing.Point(13, 85);
			this.FactorComboBox.Name = "FactorComboBox";
			this.FactorComboBox.Size = new System.Drawing.Size(399, 25);
			this.FactorComboBox.TabIndex = 1;
			// 
			// FactorValueTextBox
			// 
			this.FactorValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.FactorValueTextBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.FactorValueTextBox.Location = new System.Drawing.Point(12, 135);
			this.FactorValueTextBox.Name = "FactorValueTextBox";
			this.FactorValueTextBox.Size = new System.Drawing.Size(280, 25);
			this.FactorValueTextBox.TabIndex = 2;
			// 
			// AddingButton
			// 
			this.AddingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.AddingButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.AddingButton.Location = new System.Drawing.Point(329, 168);
			this.AddingButton.Name = "AddingButton";
			this.AddingButton.Size = new System.Drawing.Size(82, 25);
			this.AddingButton.TabIndex = 3;
			this.AddingButton.Text = "Добавить";
			this.AddingButton.UseVisualStyleBackColor = true;
			this.AddingButton.Click += new System.EventHandler(this.AddingButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label1.Location = new System.Drawing.Point(12, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(245, 19);
			this.label1.TabIndex = 4;
			this.label1.Text = "Введите или выберите направление :";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label2.Location = new System.Drawing.Point(12, 63);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(309, 19);
			this.label2.TabIndex = 5;
			this.label2.Text = "Введите или выберите наименование фактора:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label3.Location = new System.Drawing.Point(12, 113);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(183, 19);
			this.label3.TabIndex = 6;
			this.label3.Text = "Введите значение фактора:";
			// 
			// ErrorLabel
			// 
			this.ErrorLabel.AutoSize = true;
			this.ErrorLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.ErrorLabel.ForeColor = System.Drawing.Color.Red;
			this.ErrorLabel.Location = new System.Drawing.Point(12, 172);
			this.ErrorLabel.Name = "ErrorLabel";
			this.ErrorLabel.Size = new System.Drawing.Size(217, 19);
			this.ErrorLabel.TabIndex = 7;
			this.ErrorLabel.Text = "Необходимо заполнить все поля";
			this.ErrorLabel.Visible = false;
			// 
			// AddFactor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(423, 205);
			this.Controls.Add(this.ErrorLabel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.AddingButton);
			this.Controls.Add(this.FactorValueTextBox);
			this.Controls.Add(this.FactorComboBox);
			this.Controls.Add(this.DirectionComboBox);
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(439, 211);
			this.Name = "AddFactor";
			this.Text = "Добавить фактор";
			this.Load += new System.EventHandler(this.AddFactor_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox DirectionComboBox;
		private System.Windows.Forms.ComboBox FactorComboBox;
		private System.Windows.Forms.TextBox FactorValueTextBox;
		private System.Windows.Forms.Button AddingButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label ErrorLabel;
	}
}
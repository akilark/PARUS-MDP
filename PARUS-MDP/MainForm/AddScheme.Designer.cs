
namespace GUI
{
	partial class AddScheme
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
			this.SchemeComboBox = new System.Windows.Forms.ComboBox();
			this.DisturbanceComboBox = new System.Windows.Forms.ComboBox();
			this.PAgroupBox = new System.Windows.Forms.GroupBox();
			this.PAlabel = new System.Windows.Forms.Label();
			this.PAradioButtonNo = new System.Windows.Forms.RadioButton();
			this.PAradioButtonYes = new System.Windows.Forms.RadioButton();
			this.AddingButton = new System.Windows.Forms.Button();
			this.PAgroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// SchemeComboBox
			// 
			this.SchemeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SchemeComboBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SchemeComboBox.FormattingEnabled = true;
			this.SchemeComboBox.Location = new System.Drawing.Point(12, 12);
			this.SchemeComboBox.Name = "SchemeComboBox";
			this.SchemeComboBox.Size = new System.Drawing.Size(299, 25);
			this.SchemeComboBox.TabIndex = 0;
			// 
			// DisturbanceComboBox
			// 
			this.DisturbanceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.DisturbanceComboBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.DisturbanceComboBox.FormattingEnabled = true;
			this.DisturbanceComboBox.Location = new System.Drawing.Point(12, 62);
			this.DisturbanceComboBox.Name = "DisturbanceComboBox";
			this.DisturbanceComboBox.Size = new System.Drawing.Size(299, 25);
			this.DisturbanceComboBox.TabIndex = 1;
			// 
			// PAgroupBox
			// 
			this.PAgroupBox.Controls.Add(this.PAlabel);
			this.PAgroupBox.Controls.Add(this.PAradioButtonNo);
			this.PAgroupBox.Controls.Add(this.PAradioButtonYes);
			this.PAgroupBox.Location = new System.Drawing.Point(12, 92);
			this.PAgroupBox.Name = "PAgroupBox";
			this.PAgroupBox.Size = new System.Drawing.Size(88, 66);
			this.PAgroupBox.TabIndex = 2;
			this.PAgroupBox.TabStop = false;
			// 
			// PAlabel
			// 
			this.PAlabel.AutoSize = true;
			this.PAlabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.PAlabel.Location = new System.Drawing.Point(3, 25);
			this.PAlabel.Name = "PAlabel";
			this.PAlabel.Size = new System.Drawing.Size(28, 19);
			this.PAlabel.TabIndex = 2;
			this.PAlabel.Text = "ПА";
			// 
			// PAradioButtonNo
			// 
			this.PAradioButtonNo.AutoSize = true;
			this.PAradioButtonNo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.PAradioButtonNo.Location = new System.Drawing.Point(33, 36);
			this.PAradioButtonNo.Name = "PAradioButtonNo";
			this.PAradioButtonNo.Size = new System.Drawing.Size(50, 23);
			this.PAradioButtonNo.TabIndex = 1;
			this.PAradioButtonNo.TabStop = true;
			this.PAradioButtonNo.Text = "Нет";
			this.PAradioButtonNo.UseVisualStyleBackColor = true;
			// 
			// PAradioButtonYes
			// 
			this.PAradioButtonYes.AutoSize = true;
			this.PAradioButtonYes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.PAradioButtonYes.Location = new System.Drawing.Point(33, 12);
			this.PAradioButtonYes.Name = "PAradioButtonYes";
			this.PAradioButtonYes.Size = new System.Drawing.Size(53, 23);
			this.PAradioButtonYes.TabIndex = 0;
			this.PAradioButtonYes.TabStop = true;
			this.PAradioButtonYes.Text = "Есть";
			this.PAradioButtonYes.UseVisualStyleBackColor = true;
			// 
			// AddingButton
			// 
			this.AddingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.AddingButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.AddingButton.Location = new System.Drawing.Point(225, 133);
			this.AddingButton.Name = "AddingButton";
			this.AddingButton.Size = new System.Drawing.Size(86, 25);
			this.AddingButton.TabIndex = 3;
			this.AddingButton.Text = "Добавить";
			this.AddingButton.UseVisualStyleBackColor = true;
			// 
			// AddScheme
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(323, 170);
			this.Controls.Add(this.AddingButton);
			this.Controls.Add(this.PAgroupBox);
			this.Controls.Add(this.DisturbanceComboBox);
			this.Controls.Add(this.SchemeComboBox);
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(339, 209);
			this.Name = "AddScheme";
			this.Text = "Добавить схему";
			this.PAgroupBox.ResumeLayout(false);
			this.PAgroupBox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox SchemeComboBox;
		private System.Windows.Forms.ComboBox DisturbanceComboBox;
		private System.Windows.Forms.GroupBox PAgroupBox;
		private System.Windows.Forms.Label PAlabel;
		private System.Windows.Forms.RadioButton PAradioButtonNo;
		private System.Windows.Forms.RadioButton PAradioButtonYes;
		private System.Windows.Forms.Button AddingButton;
	}
}
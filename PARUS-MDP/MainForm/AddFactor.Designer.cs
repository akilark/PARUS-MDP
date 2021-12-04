
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
			this.SectionComboBox = new System.Windows.Forms.ComboBox();
			this.FactorComboBox = new System.Windows.Forms.ComboBox();
			this.FactorValueTextBox = new System.Windows.Forms.TextBox();
			this.AddingButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// SectionComboBox
			// 
			this.SectionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SectionComboBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SectionComboBox.FormattingEnabled = true;
			this.SectionComboBox.Location = new System.Drawing.Point(12, 12);
			this.SectionComboBox.Name = "SectionComboBox";
			this.SectionComboBox.Size = new System.Drawing.Size(337, 25);
			this.SectionComboBox.TabIndex = 0;
			// 
			// FactorComboBox
			// 
			this.FactorComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.FactorComboBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.FactorComboBox.FormattingEnabled = true;
			this.FactorComboBox.Location = new System.Drawing.Point(12, 60);
			this.FactorComboBox.Name = "FactorComboBox";
			this.FactorComboBox.Size = new System.Drawing.Size(337, 25);
			this.FactorComboBox.TabIndex = 1;
			// 
			// FactorValueTextBox
			// 
			this.FactorValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.FactorValueTextBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.FactorValueTextBox.Location = new System.Drawing.Point(12, 108);
			this.FactorValueTextBox.Name = "FactorValueTextBox";
			this.FactorValueTextBox.Size = new System.Drawing.Size(218, 25);
			this.FactorValueTextBox.TabIndex = 2;
			// 
			// AddingButton
			// 
			this.AddingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.AddingButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.AddingButton.Location = new System.Drawing.Point(267, 107);
			this.AddingButton.Name = "AddingButton";
			this.AddingButton.Size = new System.Drawing.Size(82, 25);
			this.AddingButton.TabIndex = 3;
			this.AddingButton.Text = "Добавить";
			this.AddingButton.UseVisualStyleBackColor = true;
			// 
			// AddFactor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(361, 144);
			this.Controls.Add(this.AddingButton);
			this.Controls.Add(this.FactorValueTextBox);
			this.Controls.Add(this.FactorComboBox);
			this.Controls.Add(this.SectionComboBox);
			this.MaximizeBox = false;
			this.Name = "AddFactor";
			this.Text = "Добавить фактор";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox SectionComboBox;
		private System.Windows.Forms.ComboBox FactorComboBox;
		private System.Windows.Forms.TextBox FactorValueTextBox;
		private System.Windows.Forms.Button AddingButton;
	}
}
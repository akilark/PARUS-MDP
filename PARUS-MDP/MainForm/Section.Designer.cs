
namespace GUI
{
	partial class Section
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
			this.DataSourceButton = new System.Windows.Forms.Button();
			this.CancelingButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// SectionComboBox
			// 
			this.SectionComboBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SectionComboBox.FormattingEnabled = true;
			this.SectionComboBox.Location = new System.Drawing.Point(12, 44);
			this.SectionComboBox.Name = "SectionComboBox";
			this.SectionComboBox.Size = new System.Drawing.Size(316, 25);
			this.SectionComboBox.TabIndex = 0;
			// 
			// DataSourceButton
			// 
			this.DataSourceButton.BackgroundImage = global::GUI.Properties.Resources.free_icon_settings_126472;
			this.DataSourceButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.DataSourceButton.Location = new System.Drawing.Point(414, 12);
			this.DataSourceButton.Name = "DataSourceButton";
			this.DataSourceButton.Size = new System.Drawing.Size(32, 32);
			this.DataSourceButton.TabIndex = 1;
			this.DataSourceButton.UseVisualStyleBackColor = true;
			// 
			// CancelButton
			// 
			this.CancelingButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.CancelingButton.Location = new System.Drawing.Point(357, 200);
			this.CancelingButton.Name = "CancelButton";
			this.CancelingButton.Size = new System.Drawing.Size(90, 25);
			this.CancelingButton.TabIndex = 2;
			this.CancelingButton.Text = "Назад";
			this.CancelingButton.UseVisualStyleBackColor = true;
			// 
			// Section
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(459, 237);
			this.Controls.Add(this.CancelingButton);
			this.Controls.Add(this.DataSourceButton);
			this.Controls.Add(this.SectionComboBox);
			this.MaximizeBox = false;
			this.Name = "Section";
			this.ShowIcon = false;
			this.Text = "Выбор сечения";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox SectionComboBox;
		private System.Windows.Forms.Button DataSourceButton;
		private System.Windows.Forms.Button CancelingButton;
	}
}
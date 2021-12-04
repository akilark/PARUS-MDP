
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
			this.AcceptingButton = new System.Windows.Forms.Button();
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
			this.SectionComboBox.SelectedIndexChanged += new System.EventHandler(this.SectionComboBox_SelectedIndexChanged);
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
			this.DataSourceButton.Click += new System.EventHandler(this.DataSourceButton_Click);
			// 
			// AcceptingButton
			// 
			this.AcceptingButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.AcceptingButton.Location = new System.Drawing.Point(356, 200);
			this.AcceptingButton.Name = "AcceptingButton";
			this.AcceptingButton.Size = new System.Drawing.Size(90, 25);
			this.AcceptingButton.TabIndex = 3;
			this.AcceptingButton.Text = "Принять";
			this.AcceptingButton.UseVisualStyleBackColor = true;
			this.AcceptingButton.Click += new System.EventHandler(this.AcceptingButton_Click);
			// 
			// Section
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(459, 237);
			this.Controls.Add(this.AcceptingButton);
			this.Controls.Add(this.DataSourceButton);
			this.Controls.Add(this.SectionComboBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(475, 276);
			this.Name = "Section";
			this.ShowIcon = false;
			this.Text = "Выбор сечения";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Section_FormClosed);
			this.Load += new System.EventHandler(this.Section_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox SectionComboBox;
		private System.Windows.Forms.Button DataSourceButton;
		private System.Windows.Forms.Button AcceptingButton;
	}
}
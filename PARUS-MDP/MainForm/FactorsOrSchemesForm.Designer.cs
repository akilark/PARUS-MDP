
namespace GUI
{
	partial class FactorsOrSchemesForm
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
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.SectionLabel = new System.Windows.Forms.Label();
			this.SectionNameLabel = new System.Windows.Forms.Label();
			this.GroupBox = new System.Windows.Forms.GroupBox();
			this.DownloadButton = new System.Windows.Forms.Button();
			this.SaveButton = new System.Windows.Forms.Button();
			this.AddButton = new System.Windows.Forms.Button();
			this.DeleteButton = new System.Windows.Forms.Button();
			this.AcceptingButton = new System.Windows.Forms.Button();
			this.BackButton = new System.Windows.Forms.Button();
			this.GeneralLabel = new System.Windows.Forms.Label();
			this.FactorOrSchemeLabel = new System.Windows.Forms.Label();
			this.GroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.Location = new System.Drawing.Point(12, 37);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(322, 376);
			this.treeView1.TabIndex = 0;
			// 
			// SectionLabel
			// 
			this.SectionLabel.AutoSize = true;
			this.SectionLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SectionLabel.Location = new System.Drawing.Point(12, 9);
			this.SectionLabel.Name = "SectionLabel";
			this.SectionLabel.Size = new System.Drawing.Size(66, 19);
			this.SectionLabel.TabIndex = 1;
			this.SectionLabel.Text = "Сечение:";
			// 
			// SectionNameLabel
			// 
			this.SectionNameLabel.AutoSize = true;
			this.SectionNameLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SectionNameLabel.Location = new System.Drawing.Point(81, 9);
			this.SectionNameLabel.Name = "SectionNameLabel";
			this.SectionNameLabel.Size = new System.Drawing.Size(124, 19);
			this.SectionNameLabel.TabIndex = 2;
			this.SectionNameLabel.Text = "Название сечения";
			// 
			// GroupBox
			// 
			this.GroupBox.Controls.Add(this.FactorOrSchemeLabel);
			this.GroupBox.Controls.Add(this.GeneralLabel);
			this.GroupBox.Controls.Add(this.DeleteButton);
			this.GroupBox.Controls.Add(this.AddButton);
			this.GroupBox.Controls.Add(this.SaveButton);
			this.GroupBox.Controls.Add(this.DownloadButton);
			this.GroupBox.Location = new System.Drawing.Point(346, 29);
			this.GroupBox.Name = "GroupBox";
			this.GroupBox.Size = new System.Drawing.Size(196, 190);
			this.GroupBox.TabIndex = 3;
			this.GroupBox.TabStop = false;
			this.GroupBox.Enter += new System.EventHandler(this.GroupBox_Enter);
			// 
			// DownloadButton
			// 
			this.DownloadButton.BackgroundImage = global::GUI.Properties.Resources.premium_icon_update_5126603;
			this.DownloadButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.DownloadButton.Location = new System.Drawing.Point(103, 62);
			this.DownloadButton.Name = "DownloadButton";
			this.DownloadButton.Size = new System.Drawing.Size(45, 45);
			this.DownloadButton.TabIndex = 0;
			this.DownloadButton.UseVisualStyleBackColor = true;
			// 
			// SaveButton
			// 
			this.SaveButton.BackgroundImage = global::GUI.Properties.Resources.premium_icon_save_data_5126891;
			this.SaveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.SaveButton.Location = new System.Drawing.Point(48, 62);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(45, 45);
			this.SaveButton.TabIndex = 1;
			this.SaveButton.UseVisualStyleBackColor = true;
			// 
			// AddButton
			// 
			this.AddButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.AddButton.Location = new System.Drawing.Point(51, 117);
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(94, 25);
			this.AddButton.TabIndex = 2;
			this.AddButton.Text = "Добавить";
			this.AddButton.UseVisualStyleBackColor = true;
			// 
			// DeleteButton
			// 
			this.DeleteButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.DeleteButton.Location = new System.Drawing.Point(51, 152);
			this.DeleteButton.Name = "DeleteButton";
			this.DeleteButton.Size = new System.Drawing.Size(94, 25);
			this.DeleteButton.TabIndex = 3;
			this.DeleteButton.Text = "Удалить";
			this.DeleteButton.UseVisualStyleBackColor = true;
			// 
			// AcceptingButton
			// 
			this.AcceptingButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.AcceptingButton.Location = new System.Drawing.Point(386, 388);
			this.AcceptingButton.Name = "AcceptingButton";
			this.AcceptingButton.Size = new System.Drawing.Size(75, 25);
			this.AcceptingButton.TabIndex = 4;
			this.AcceptingButton.Text = "Принять";
			this.AcceptingButton.UseVisualStyleBackColor = true;
			// 
			// BackButton
			// 
			this.BackButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.BackButton.Location = new System.Drawing.Point(467, 388);
			this.BackButton.Name = "BackButton";
			this.BackButton.Size = new System.Drawing.Size(75, 25);
			this.BackButton.TabIndex = 5;
			this.BackButton.Text = "Назад";
			this.BackButton.UseVisualStyleBackColor = true;
			// 
			// GeneralLabel
			// 
			this.GeneralLabel.AutoSize = true;
			this.GeneralLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.GeneralLabel.Location = new System.Drawing.Point(18, 10);
			this.GeneralLabel.Name = "GeneralLabel";
			this.GeneralLabel.Size = new System.Drawing.Size(159, 19);
			this.GeneralLabel.TabIndex = 4;
			this.GeneralLabel.Text = "Редактирование списка";
			// 
			// FactorOrSchemeLabel
			// 
			this.FactorOrSchemeLabel.AutoSize = true;
			this.FactorOrSchemeLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.FactorOrSchemeLabel.Location = new System.Drawing.Point(65, 29);
			this.FactorOrSchemeLabel.Name = "FactorOrSchemeLabel";
			this.FactorOrSchemeLabel.Size = new System.Drawing.Size(70, 19);
			this.FactorOrSchemeLabel.TabIndex = 5;
			this.FactorOrSchemeLabel.Text = "факторов";
			this.FactorOrSchemeLabel.Click += new System.EventHandler(this.label4_Click);
			// 
			// FactorsOrSchemesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(554, 425);
			this.Controls.Add(this.BackButton);
			this.Controls.Add(this.AcceptingButton);
			this.Controls.Add(this.GroupBox);
			this.Controls.Add(this.SectionNameLabel);
			this.Controls.Add(this.SectionLabel);
			this.Controls.Add(this.treeView1);
			this.MaximizeBox = false;
			this.Name = "FactorsOrSchemesForm";
			this.Text = "Редактирование данных";
			this.GroupBox.ResumeLayout(false);
			this.GroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Label SectionLabel;
		private System.Windows.Forms.Label SectionNameLabel;
		private System.Windows.Forms.GroupBox GroupBox;
		private System.Windows.Forms.Label FactorOrSchemeLabel;
		private System.Windows.Forms.Label GeneralLabel;
		private System.Windows.Forms.Button DeleteButton;
		private System.Windows.Forms.Button AddButton;
		private System.Windows.Forms.Button SaveButton;
		private System.Windows.Forms.Button DownloadButton;
		private System.Windows.Forms.Button AcceptingButton;
		private System.Windows.Forms.Button BackButton;
	}
}

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
			this.FactorOrSchemeTreeView = new System.Windows.Forms.TreeView();
			this.SectionLabel = new System.Windows.Forms.Label();
			this.SectionNameLabel = new System.Windows.Forms.Label();
			this.GroupBox = new System.Windows.Forms.GroupBox();
			this.FactorOrSchemeLabel = new System.Windows.Forms.Label();
			this.GeneralLabel = new System.Windows.Forms.Label();
			this.DeleteButton = new System.Windows.Forms.Button();
			this.AddButton = new System.Windows.Forms.Button();
			this.SaveButton = new System.Windows.Forms.Button();
			this.DownloadButton = new System.Windows.Forms.Button();
			this.AcceptingButton = new System.Windows.Forms.Button();
			this.BackButton = new System.Windows.Forms.Button();
			this.GroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// FactorOrSchemeTreeView
			// 
			this.FactorOrSchemeTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.FactorOrSchemeTreeView.Location = new System.Drawing.Point(12, 37);
			this.FactorOrSchemeTreeView.Name = "FactorOrSchemeTreeView";
			this.FactorOrSchemeTreeView.Size = new System.Drawing.Size(322, 376);
			this.FactorOrSchemeTreeView.TabIndex = 0;
			this.FactorOrSchemeTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.FactorOrSchemeTreeView_AfterCheck);
			this.FactorOrSchemeTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
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
			this.GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
			// DeleteButton
			// 
			this.DeleteButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.DeleteButton.Location = new System.Drawing.Point(51, 152);
			this.DeleteButton.Name = "DeleteButton";
			this.DeleteButton.Size = new System.Drawing.Size(94, 25);
			this.DeleteButton.TabIndex = 3;
			this.DeleteButton.Text = "Удалить";
			this.DeleteButton.UseVisualStyleBackColor = true;
			this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
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
			this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
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
			this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
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
			this.DownloadButton.Click += new System.EventHandler(this.DownloadButton_Click);
			// 
			// AcceptingButton
			// 
			this.AcceptingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.AcceptingButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.AcceptingButton.Location = new System.Drawing.Point(386, 388);
			this.AcceptingButton.Name = "AcceptingButton";
			this.AcceptingButton.Size = new System.Drawing.Size(75, 25);
			this.AcceptingButton.TabIndex = 4;
			this.AcceptingButton.Text = "Принять";
			this.AcceptingButton.UseVisualStyleBackColor = true;
			this.AcceptingButton.Click += new System.EventHandler(this.AcceptingButton_Click);
			// 
			// BackButton
			// 
			this.BackButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BackButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.BackButton.Location = new System.Drawing.Point(467, 388);
			this.BackButton.Name = "BackButton";
			this.BackButton.Size = new System.Drawing.Size(75, 25);
			this.BackButton.TabIndex = 5;
			this.BackButton.Text = "Назад";
			this.BackButton.UseVisualStyleBackColor = true;
			this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
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
			this.Controls.Add(this.FactorOrSchemeTreeView);
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(570, 464);
			this.Name = "FactorsOrSchemesForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Редактирование данных";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FactorsOrSchemesForm_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FactorsOrSchemesForm_FormClosed);
			this.Load += new System.EventHandler(this.FactorsOrSchemesForm_Load);
			this.GroupBox.ResumeLayout(false);
			this.GroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView FactorOrSchemeTreeView;
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

namespace GUI
{
	partial class MainForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.FolderButton = new System.Windows.Forms.Button();
			this.PARUSsampleButton = new System.Windows.Forms.Button();
			this.OutputFileButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// FolderButton
			// 
			this.FolderButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.FolderButton.Location = new System.Drawing.Point(180, 106);
			this.FolderButton.Name = "FolderButton";
			this.FolderButton.Size = new System.Drawing.Size(240, 72);
			this.FolderButton.TabIndex = 0;
			this.FolderButton.Text = "Формирование дерева папок";
			this.FolderButton.UseVisualStyleBackColor = true;
			this.FolderButton.Click += new System.EventHandler(this.FolderButton_Click);
			// 
			// PARUSsampleButton
			// 
			this.PARUSsampleButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.PARUSsampleButton.Location = new System.Drawing.Point(180, 214);
			this.PARUSsampleButton.Name = "PARUSsampleButton";
			this.PARUSsampleButton.Size = new System.Drawing.Size(240, 72);
			this.PARUSsampleButton.TabIndex = 1;
			this.PARUSsampleButton.Text = "Формирование шаблона для ПК \"ПАРУС\"";
			this.PARUSsampleButton.UseVisualStyleBackColor = true;
			this.PARUSsampleButton.Click += new System.EventHandler(this.PARUSsampleButton_Click);
			// 
			// OutputFileButton
			// 
			this.OutputFileButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.OutputFileButton.Location = new System.Drawing.Point(180, 322);
			this.OutputFileButton.Name = "OutputFileButton";
			this.OutputFileButton.Size = new System.Drawing.Size(240, 72);
			this.OutputFileButton.TabIndex = 2;
			this.OutputFileButton.Text = "Формирование приложения №6 ПУР";
			this.OutputFileButton.UseVisualStyleBackColor = true;
			this.OutputFileButton.Click += new System.EventHandler(this.OutputFileButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			this.ClientSize = new System.Drawing.Size(584, 461);
			this.Controls.Add(this.OutputFileButton);
			this.Controls.Add(this.PARUSsampleButton);
			this.Controls.Add(this.FolderButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ПАРУС- АФТП";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button FolderButton;
		private System.Windows.Forms.Button PARUSsampleButton;
		private System.Windows.Forms.Button OutputFileButton;
	}
}


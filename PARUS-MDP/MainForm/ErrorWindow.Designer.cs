
namespace GUI
{
	partial class ErrorWindow
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
			this.ErrorListBox = new System.Windows.Forms.ListBox();
			this.CloseButton = new System.Windows.Forms.Button();
			this.TextButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ErrorListBox
			// 
			this.ErrorListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ErrorListBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.ErrorListBox.FormattingEnabled = true;
			this.ErrorListBox.ItemHeight = 17;
			this.ErrorListBox.Location = new System.Drawing.Point(12, 12);
			this.ErrorListBox.Name = "ErrorListBox";
			this.ErrorListBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.ErrorListBox.ScrollAlwaysVisible = true;
			this.ErrorListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.ErrorListBox.Size = new System.Drawing.Size(486, 344);
			this.ErrorListBox.TabIndex = 0;
			// 
			// CloseButton
			// 
			this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CloseButton.Location = new System.Drawing.Point(504, 330);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(75, 26);
			this.CloseButton.TabIndex = 1;
			this.CloseButton.Text = "Закрыть";
			this.CloseButton.UseVisualStyleBackColor = true;
			this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
			// 
			// TextButton
			// 
			this.TextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.TextButton.BackgroundImage = global::GUI.Properties.Resources.free_icon_txt_file_3022200;
			this.TextButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.TextButton.Location = new System.Drawing.Point(504, 12);
			this.TextButton.Name = "TextButton";
			this.TextButton.Size = new System.Drawing.Size(75, 48);
			this.TextButton.TabIndex = 2;
			this.TextButton.UseVisualStyleBackColor = true;
			this.TextButton.Click += new System.EventHandler(this.TextButton_Click);
			// 
			// ErrorWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(585, 368);
			this.Controls.Add(this.TextButton);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(this.ErrorListBox);
			this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.MaximizeBox = false;
			this.Name = "ErrorWindow";
			this.Text = "Список ошибок";
			this.Load += new System.EventHandler(this.ErrorWindow_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox ErrorListBox;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.Button TextButton;
	}
}
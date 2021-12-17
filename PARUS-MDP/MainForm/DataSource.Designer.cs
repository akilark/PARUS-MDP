
namespace GUI
{
	partial class DataSource
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
			this.ConnectionTextBox = new System.Windows.Forms.TextBox();
			this.LoginTextBox = new System.Windows.Forms.TextBox();
			this.PasswordTextBox = new System.Windows.Forms.TextBox();
			this.ConnectionLabel = new System.Windows.Forms.Label();
			this.LoginLabel = new System.Windows.Forms.Label();
			this.PasswordLabel = new System.Windows.Forms.Label();
			this.ConnectButton = new System.Windows.Forms.Button();
			this.CancelingButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ConnectionTextBox
			// 
			this.ConnectionTextBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.ConnectionTextBox.Location = new System.Drawing.Point(19, 43);
			this.ConnectionTextBox.Name = "ConnectionTextBox";
			this.ConnectionTextBox.Size = new System.Drawing.Size(362, 25);
			this.ConnectionTextBox.TabIndex = 0;
			// 
			// LoginTextBox
			// 
			this.LoginTextBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.LoginTextBox.Location = new System.Drawing.Point(19, 116);
			this.LoginTextBox.Name = "LoginTextBox";
			this.LoginTextBox.Size = new System.Drawing.Size(362, 25);
			this.LoginTextBox.TabIndex = 0;
			// 
			// PasswordTextBox
			// 
			this.PasswordTextBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.PasswordTextBox.Location = new System.Drawing.Point(19, 189);
			this.PasswordTextBox.Name = "PasswordTextBox";
			this.PasswordTextBox.PasswordChar = '*';
			this.PasswordTextBox.Size = new System.Drawing.Size(362, 25);
			this.PasswordTextBox.TabIndex = 0;
			// 
			// ConnectionLabel
			// 
			this.ConnectionLabel.AutoSize = true;
			this.ConnectionLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.ConnectionLabel.Location = new System.Drawing.Point(19, 19);
			this.ConnectionLabel.Name = "ConnectionLabel";
			this.ConnectionLabel.Size = new System.Drawing.Size(148, 19);
			this.ConnectionLabel.TabIndex = 1;
			this.ConnectionLabel.Text = "Строка подключения:";
			// 
			// LoginLabel
			// 
			this.LoginLabel.AutoSize = true;
			this.LoginLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.LoginLabel.Location = new System.Drawing.Point(19, 87);
			this.LoginLabel.Name = "LoginLabel";
			this.LoginLabel.Size = new System.Drawing.Size(50, 19);
			this.LoginLabel.TabIndex = 1;
			this.LoginLabel.Text = "Логин:";
			// 
			// PasswordLabel
			// 
			this.PasswordLabel.AutoSize = true;
			this.PasswordLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.PasswordLabel.Location = new System.Drawing.Point(19, 160);
			this.PasswordLabel.Name = "PasswordLabel";
			this.PasswordLabel.Size = new System.Drawing.Size(59, 19);
			this.PasswordLabel.TabIndex = 1;
			this.PasswordLabel.Text = "Пароль:";
			// 
			// ConnectButton
			// 
			this.ConnectButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.ConnectButton.Location = new System.Drawing.Point(191, 233);
			this.ConnectButton.Name = "ConnectButton";
			this.ConnectButton.Size = new System.Drawing.Size(102, 25);
			this.ConnectButton.TabIndex = 2;
			this.ConnectButton.Text = "Подключить";
			this.ConnectButton.UseVisualStyleBackColor = true;
			this.ConnectButton.Click += new System.EventHandler(this.button1_Click);
			// 
			// CancelingButton
			// 
			this.CancelingButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.CancelingButton.Location = new System.Drawing.Point(299, 233);
			this.CancelingButton.Name = "CancelingButton";
			this.CancelingButton.Size = new System.Drawing.Size(82, 25);
			this.CancelingButton.TabIndex = 2;
			this.CancelingButton.Text = "Назад";
			this.CancelingButton.UseVisualStyleBackColor = true;
			this.CancelingButton.Click += new System.EventHandler(this.CancelingButton_Click);
			// 
			// DataSource
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(401, 270);
			this.Controls.Add(this.CancelingButton);
			this.Controls.Add(this.ConnectButton);
			this.Controls.Add(this.PasswordLabel);
			this.Controls.Add(this.LoginLabel);
			this.Controls.Add(this.ConnectionLabel);
			this.Controls.Add(this.PasswordTextBox);
			this.Controls.Add(this.LoginTextBox);
			this.Controls.Add(this.ConnectionTextBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "DataSource";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Подключение к базе данных";
			this.Load += new System.EventHandler(this.DataSource_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox ConnectionTextBox;
		private System.Windows.Forms.TextBox LoginTextBox;
		private System.Windows.Forms.TextBox PasswordTextBox;
		private System.Windows.Forms.Label ConnectionLabel;
		private System.Windows.Forms.Label LoginLabel;
		private System.Windows.Forms.Label PasswordLabel;
		private System.Windows.Forms.Button ConnectButton;
		private System.Windows.Forms.Button CancelingButton;
	}
}
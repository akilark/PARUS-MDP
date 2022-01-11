
namespace GUI
{
	partial class OutputFileGenerate
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
			this.PathTextBox = new System.Windows.Forms.TextBox();
			this.PathLabel = new System.Windows.Forms.Label();
			this.ExploreButton = new System.Windows.Forms.Button();
			this.TemperatureAllowGroupBox = new System.Windows.Forms.GroupBox();
			this.TemperatureCheckBox = new System.Windows.Forms.CheckBox();
			this.TemperatureGroupBox = new System.Windows.Forms.GroupBox();
			this.checkBox45 = new System.Windows.Forms.CheckBox();
			this.checkBox40 = new System.Windows.Forms.CheckBox();
			this.checkBox35 = new System.Windows.Forms.CheckBox();
			this.checkBox30 = new System.Windows.Forms.CheckBox();
			this.checkBox_25 = new System.Windows.Forms.CheckBox();
			this.checkBox_20 = new System.Windows.Forms.CheckBox();
			this.checkBox25 = new System.Windows.Forms.CheckBox();
			this.checkBox_15 = new System.Windows.Forms.CheckBox();
			this.checkBox20 = new System.Windows.Forms.CheckBox();
			this.checkBox15 = new System.Windows.Forms.CheckBox();
			this.SummerButton = new System.Windows.Forms.Button();
			this.WinterButton = new System.Windows.Forms.Button();
			this.AwayButton = new System.Windows.Forms.Button();
			this.checkBox_10 = new System.Windows.Forms.CheckBox();
			this.SelectButton = new System.Windows.Forms.Button();
			this.checkBox0 = new System.Windows.Forms.CheckBox();
			this.checkBox_5 = new System.Windows.Forms.CheckBox();
			this.checkBox10 = new System.Windows.Forms.CheckBox();
			this.checkBox5 = new System.Windows.Forms.CheckBox();
			this.FormButton = new System.Windows.Forms.Button();
			this.EmergencyLineDisconnection = new System.Windows.Forms.CheckBox();
			this.SamplePathLabel = new System.Windows.Forms.Label();
			this.SamplePathtextBox = new System.Windows.Forms.TextBox();
			this.SamplePathButton = new System.Windows.Forms.Button();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.errorButton = new System.Windows.Forms.Button();
			this.TemperatureAllowGroupBox.SuspendLayout();
			this.TemperatureGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// PathTextBox
			// 
			this.PathTextBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.PathTextBox.Location = new System.Drawing.Point(98, 21);
			this.PathTextBox.Name = "PathTextBox";
			this.PathTextBox.Size = new System.Drawing.Size(382, 25);
			this.PathTextBox.TabIndex = 0;
			this.PathTextBox.TextChanged += new System.EventHandler(this.PathTextBox_TextChanged);
			// 
			// PathLabel
			// 
			this.PathLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.PathLabel.Location = new System.Drawing.Point(-2, 10);
			this.PathLabel.Name = "PathLabel";
			this.PathLabel.Size = new System.Drawing.Size(100, 47);
			this.PathLabel.TabIndex = 1;
			this.PathLabel.Text = "Путь к дереву папок";
			this.PathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ExploreButton
			// 
			this.ExploreButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.ExploreButton.Location = new System.Drawing.Point(500, 21);
			this.ExploreButton.Name = "ExploreButton";
			this.ExploreButton.Size = new System.Drawing.Size(75, 25);
			this.ExploreButton.TabIndex = 2;
			this.ExploreButton.Text = "Обзор";
			this.ExploreButton.UseVisualStyleBackColor = true;
			this.ExploreButton.Click += new System.EventHandler(this.ExploreButton_Click);
			// 
			// TemperatureAllowGroupBox
			// 
			this.TemperatureAllowGroupBox.Controls.Add(this.TemperatureCheckBox);
			this.TemperatureAllowGroupBox.Controls.Add(this.TemperatureGroupBox);
			this.TemperatureAllowGroupBox.Location = new System.Drawing.Point(10, 131);
			this.TemperatureAllowGroupBox.Name = "TemperatureAllowGroupBox";
			this.TemperatureAllowGroupBox.Size = new System.Drawing.Size(581, 180);
			this.TemperatureAllowGroupBox.TabIndex = 3;
			this.TemperatureAllowGroupBox.TabStop = false;
			// 
			// TemperatureCheckBox
			// 
			this.TemperatureCheckBox.AutoSize = true;
			this.TemperatureCheckBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.TemperatureCheckBox.Location = new System.Drawing.Point(33, 22);
			this.TemperatureCheckBox.Name = "TemperatureCheckBox";
			this.TemperatureCheckBox.Size = new System.Drawing.Size(180, 23);
			this.TemperatureCheckBox.TabIndex = 1;
			this.TemperatureCheckBox.Text = "Учитывать температуру";
			this.TemperatureCheckBox.UseVisualStyleBackColor = true;
			this.TemperatureCheckBox.CheckedChanged += new System.EventHandler(this.TemperatureCheckBox_CheckedChanged);
			// 
			// TemperatureGroupBox
			// 
			this.TemperatureGroupBox.Controls.Add(this.checkBox45);
			this.TemperatureGroupBox.Controls.Add(this.checkBox40);
			this.TemperatureGroupBox.Controls.Add(this.checkBox35);
			this.TemperatureGroupBox.Controls.Add(this.checkBox30);
			this.TemperatureGroupBox.Controls.Add(this.checkBox_25);
			this.TemperatureGroupBox.Controls.Add(this.checkBox_20);
			this.TemperatureGroupBox.Controls.Add(this.checkBox25);
			this.TemperatureGroupBox.Controls.Add(this.checkBox_15);
			this.TemperatureGroupBox.Controls.Add(this.checkBox20);
			this.TemperatureGroupBox.Controls.Add(this.checkBox15);
			this.TemperatureGroupBox.Controls.Add(this.SummerButton);
			this.TemperatureGroupBox.Controls.Add(this.WinterButton);
			this.TemperatureGroupBox.Controls.Add(this.AwayButton);
			this.TemperatureGroupBox.Controls.Add(this.checkBox_10);
			this.TemperatureGroupBox.Controls.Add(this.SelectButton);
			this.TemperatureGroupBox.Controls.Add(this.checkBox0);
			this.TemperatureGroupBox.Controls.Add(this.checkBox_5);
			this.TemperatureGroupBox.Controls.Add(this.checkBox10);
			this.TemperatureGroupBox.Controls.Add(this.checkBox5);
			this.TemperatureGroupBox.Enabled = false;
			this.TemperatureGroupBox.Location = new System.Drawing.Point(6, 51);
			this.TemperatureGroupBox.Name = "TemperatureGroupBox";
			this.TemperatureGroupBox.Size = new System.Drawing.Size(567, 123);
			this.TemperatureGroupBox.TabIndex = 0;
			this.TemperatureGroupBox.TabStop = false;
			// 
			// checkBox45
			// 
			this.checkBox45.AutoSize = true;
			this.checkBox45.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox45.Location = new System.Drawing.Point(508, 22);
			this.checkBox45.Name = "checkBox45";
			this.checkBox45.Size = new System.Drawing.Size(54, 23);
			this.checkBox45.TabIndex = 11;
			this.checkBox45.Text = "+45";
			this.checkBox45.UseVisualStyleBackColor = true;
			this.checkBox45.CheckedChanged += new System.EventHandler(this.TemperatureCheckBox_CheckedChanged);
			// 
			// checkBox40
			// 
			this.checkBox40.AutoSize = true;
			this.checkBox40.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox40.Location = new System.Drawing.Point(448, 22);
			this.checkBox40.Name = "checkBox40";
			this.checkBox40.Size = new System.Drawing.Size(54, 23);
			this.checkBox40.TabIndex = 11;
			this.checkBox40.Text = "+40";
			this.checkBox40.UseVisualStyleBackColor = true;
			this.checkBox40.CheckedChanged += new System.EventHandler(this.TemperatureCheckBox_CheckedChanged);
			// 
			// checkBox35
			// 
			this.checkBox35.AutoSize = true;
			this.checkBox35.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox35.Location = new System.Drawing.Point(384, 22);
			this.checkBox35.Name = "checkBox35";
			this.checkBox35.Size = new System.Drawing.Size(54, 23);
			this.checkBox35.TabIndex = 10;
			this.checkBox35.Text = "+35";
			this.checkBox35.UseVisualStyleBackColor = true;
			this.checkBox35.CheckedChanged += new System.EventHandler(this.TemperatureCheckBox_CheckedChanged);
			// 
			// checkBox30
			// 
			this.checkBox30.AutoSize = true;
			this.checkBox30.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox30.Location = new System.Drawing.Point(320, 22);
			this.checkBox30.Name = "checkBox30";
			this.checkBox30.Size = new System.Drawing.Size(54, 23);
			this.checkBox30.TabIndex = 9;
			this.checkBox30.Text = "+30";
			this.checkBox30.UseVisualStyleBackColor = true;
			this.checkBox30.CheckedChanged += new System.EventHandler(this.TemperatureCheckBox_CheckedChanged);
			// 
			// checkBox_25
			// 
			this.checkBox_25.AutoSize = true;
			this.checkBox_25.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox_25.Location = new System.Drawing.Point(508, 51);
			this.checkBox_25.Name = "checkBox_25";
			this.checkBox_25.Size = new System.Drawing.Size(50, 23);
			this.checkBox_25.TabIndex = 8;
			this.checkBox_25.Text = "-25";
			this.checkBox_25.UseVisualStyleBackColor = true;
			this.checkBox_25.CheckedChanged += new System.EventHandler(this.TemperatureCheckBox_CheckedChanged);
			// 
			// checkBox_20
			// 
			this.checkBox_20.AutoSize = true;
			this.checkBox_20.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox_20.Location = new System.Drawing.Point(448, 51);
			this.checkBox_20.Name = "checkBox_20";
			this.checkBox_20.Size = new System.Drawing.Size(50, 23);
			this.checkBox_20.TabIndex = 7;
			this.checkBox_20.Text = "-20";
			this.checkBox_20.UseVisualStyleBackColor = true;
			this.checkBox_20.CheckedChanged += new System.EventHandler(this.TemperatureCheckBox_CheckedChanged);
			// 
			// checkBox25
			// 
			this.checkBox25.AutoSize = true;
			this.checkBox25.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox25.Location = new System.Drawing.Point(256, 22);
			this.checkBox25.Name = "checkBox25";
			this.checkBox25.Size = new System.Drawing.Size(54, 23);
			this.checkBox25.TabIndex = 8;
			this.checkBox25.Text = "+25";
			this.checkBox25.UseVisualStyleBackColor = true;
			this.checkBox25.CheckedChanged += new System.EventHandler(this.TemperatureCheckBox_CheckedChanged);
			// 
			// checkBox_15
			// 
			this.checkBox_15.AutoSize = true;
			this.checkBox_15.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox_15.Location = new System.Drawing.Point(384, 51);
			this.checkBox_15.Name = "checkBox_15";
			this.checkBox_15.Size = new System.Drawing.Size(50, 23);
			this.checkBox_15.TabIndex = 6;
			this.checkBox_15.Text = "-15";
			this.checkBox_15.UseVisualStyleBackColor = true;
			this.checkBox_15.CheckedChanged += new System.EventHandler(this.TemperatureCheckBox_CheckedChanged);
			// 
			// checkBox20
			// 
			this.checkBox20.AutoSize = true;
			this.checkBox20.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox20.Location = new System.Drawing.Point(192, 22);
			this.checkBox20.Name = "checkBox20";
			this.checkBox20.Size = new System.Drawing.Size(54, 23);
			this.checkBox20.TabIndex = 7;
			this.checkBox20.Text = "+20";
			this.checkBox20.UseVisualStyleBackColor = true;
			this.checkBox20.CheckedChanged += new System.EventHandler(this.TemperatureCheckBox_CheckedChanged);
			// 
			// checkBox15
			// 
			this.checkBox15.AutoSize = true;
			this.checkBox15.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox15.Location = new System.Drawing.Point(128, 22);
			this.checkBox15.Name = "checkBox15";
			this.checkBox15.Size = new System.Drawing.Size(54, 23);
			this.checkBox15.TabIndex = 6;
			this.checkBox15.Text = "+15";
			this.checkBox15.UseVisualStyleBackColor = true;
			this.checkBox15.CheckedChanged += new System.EventHandler(this.TemperatureCheckBox_CheckedChanged);
			// 
			// SummerButton
			// 
			this.SummerButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SummerButton.Location = new System.Drawing.Point(484, 94);
			this.SummerButton.Name = "SummerButton";
			this.SummerButton.Size = new System.Drawing.Size(75, 25);
			this.SummerButton.TabIndex = 5;
			this.SummerButton.Text = "Лето";
			this.SummerButton.UseVisualStyleBackColor = true;
			this.SummerButton.Click += new System.EventHandler(this.SummerButton_Click);
			// 
			// WinterButton
			// 
			this.WinterButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.WinterButton.Location = new System.Drawing.Point(403, 94);
			this.WinterButton.Name = "WinterButton";
			this.WinterButton.Size = new System.Drawing.Size(75, 25);
			this.WinterButton.TabIndex = 4;
			this.WinterButton.Text = "Зима";
			this.WinterButton.UseVisualStyleBackColor = true;
			this.WinterButton.Click += new System.EventHandler(this.WinterButton_Click);
			// 
			// AwayButton
			// 
			this.AwayButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.AwayButton.Location = new System.Drawing.Point(271, 94);
			this.AwayButton.Name = "AwayButton";
			this.AwayButton.Size = new System.Drawing.Size(126, 25);
			this.AwayButton.TabIndex = 3;
			this.AwayButton.Text = "Убрать отметки";
			this.AwayButton.UseVisualStyleBackColor = true;
			this.AwayButton.Click += new System.EventHandler(this.AwayButton_Click);
			// 
			// checkBox_10
			// 
			this.checkBox_10.AutoSize = true;
			this.checkBox_10.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox_10.Location = new System.Drawing.Point(320, 51);
			this.checkBox_10.Name = "checkBox_10";
			this.checkBox_10.Size = new System.Drawing.Size(50, 23);
			this.checkBox_10.TabIndex = 1;
			this.checkBox_10.Text = "-10";
			this.checkBox_10.UseVisualStyleBackColor = true;
			this.checkBox_10.CheckedChanged += new System.EventHandler(this.TemperatureCheckBox_CheckedChanged);
			// 
			// SelectButton
			// 
			this.SelectButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SelectButton.Location = new System.Drawing.Point(9, 94);
			this.SelectButton.Name = "SelectButton";
			this.SelectButton.Size = new System.Drawing.Size(109, 25);
			this.SelectButton.TabIndex = 2;
			this.SelectButton.Text = "Отметить всё";
			this.SelectButton.UseVisualStyleBackColor = true;
			this.SelectButton.Click += new System.EventHandler(this.SelectButton_Click);
			// 
			// checkBox0
			// 
			this.checkBox0.AutoSize = true;
			this.checkBox0.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox0.Location = new System.Drawing.Point(192, 51);
			this.checkBox0.Name = "checkBox0";
			this.checkBox0.Size = new System.Drawing.Size(36, 23);
			this.checkBox0.TabIndex = 0;
			this.checkBox0.Text = "0";
			this.checkBox0.UseVisualStyleBackColor = true;
			this.checkBox0.CheckedChanged += new System.EventHandler(this.TemperatureCheckBox_CheckedChanged);
			// 
			// checkBox_5
			// 
			this.checkBox_5.AutoSize = true;
			this.checkBox_5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox_5.Location = new System.Drawing.Point(256, 51);
			this.checkBox_5.Name = "checkBox_5";
			this.checkBox_5.Size = new System.Drawing.Size(42, 23);
			this.checkBox_5.TabIndex = 0;
			this.checkBox_5.Text = "-5";
			this.checkBox_5.UseVisualStyleBackColor = true;
			this.checkBox_5.CheckedChanged += new System.EventHandler(this.TemperatureCheckBox_CheckedChanged);
			// 
			// checkBox10
			// 
			this.checkBox10.AutoSize = true;
			this.checkBox10.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox10.Location = new System.Drawing.Point(64, 51);
			this.checkBox10.Name = "checkBox10";
			this.checkBox10.Size = new System.Drawing.Size(54, 23);
			this.checkBox10.TabIndex = 1;
			this.checkBox10.Text = "+10";
			this.checkBox10.UseVisualStyleBackColor = true;
			this.checkBox10.CheckedChanged += new System.EventHandler(this.TemperatureCheckBox_CheckedChanged);
			// 
			// checkBox5
			// 
			this.checkBox5.AutoSize = true;
			this.checkBox5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox5.Location = new System.Drawing.Point(128, 51);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new System.Drawing.Size(46, 23);
			this.checkBox5.TabIndex = 0;
			this.checkBox5.Text = "+5";
			this.checkBox5.UseVisualStyleBackColor = true;
			this.checkBox5.CheckedChanged += new System.EventHandler(this.TemperatureCheckBox_CheckedChanged);
			// 
			// FormButton
			// 
			this.FormButton.Enabled = false;
			this.FormButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.FormButton.Location = new System.Drawing.Point(462, 364);
			this.FormButton.Name = "FormButton";
			this.FormButton.Size = new System.Drawing.Size(121, 25);
			this.FormButton.TabIndex = 4;
			this.FormButton.Text = "Сформировать";
			this.FormButton.UseVisualStyleBackColor = true;
			this.FormButton.Click += new System.EventHandler(this.FormButton_Click);
			// 
			// EmergencyLineDisconnection
			// 
			this.EmergencyLineDisconnection.Checked = true;
			this.EmergencyLineDisconnection.CheckState = System.Windows.Forms.CheckState.Checked;
			this.EmergencyLineDisconnection.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.EmergencyLineDisconnection.Location = new System.Drawing.Point(10, 365);
			this.EmergencyLineDisconnection.Name = "EmergencyLineDisconnection";
			this.EmergencyLineDisconnection.Size = new System.Drawing.Size(310, 24);
			this.EmergencyLineDisconnection.TabIndex = 6;
			this.EmergencyLineDisconnection.Text = "Каждый небаланс учитывать как АДП- НК";
			this.EmergencyLineDisconnection.UseVisualStyleBackColor = true;
			this.EmergencyLineDisconnection.CheckedChanged += new System.EventHandler(this.EmergencyLineDisconnection_CheckedChanged);
			// 
			// SamplePathLabel
			// 
			this.SamplePathLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SamplePathLabel.Location = new System.Drawing.Point(-2, 72);
			this.SamplePathLabel.Name = "SamplePathLabel";
			this.SamplePathLabel.Size = new System.Drawing.Size(100, 47);
			this.SamplePathLabel.TabIndex = 7;
			this.SamplePathLabel.Text = "Путь к шаблону";
			this.SamplePathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.SamplePathLabel.Click += new System.EventHandler(this.SamplePathLabel_Click);
			// 
			// SamplePathtextBox
			// 
			this.SamplePathtextBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SamplePathtextBox.Location = new System.Drawing.Point(98, 83);
			this.SamplePathtextBox.Name = "SamplePathtextBox";
			this.SamplePathtextBox.Size = new System.Drawing.Size(382, 25);
			this.SamplePathtextBox.TabIndex = 8;
			this.SamplePathtextBox.TextChanged += new System.EventHandler(this.PathTextBox_TextChanged);
			// 
			// SamplePathButton
			// 
			this.SamplePathButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.SamplePathButton.Location = new System.Drawing.Point(499, 83);
			this.SamplePathButton.Name = "SamplePathButton";
			this.SamplePathButton.Size = new System.Drawing.Size(75, 25);
			this.SamplePathButton.TabIndex = 9;
			this.SamplePathButton.Text = "Обзор";
			this.SamplePathButton.UseVisualStyleBackColor = true;
			this.SamplePathButton.Click += new System.EventHandler(this.SamplePathButton_Click);
			// 
			// progressBar
			// 
			this.progressBar.ForeColor = System.Drawing.Color.ForestGreen;
			this.progressBar.Location = new System.Drawing.Point(462, 335);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(121, 23);
			this.progressBar.TabIndex = 2;
			this.progressBar.Visible = false;
			// 
			// errorButton
			// 
			this.errorButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.errorButton.Location = new System.Drawing.Point(335, 366);
			this.errorButton.Name = "errorButton";
			this.errorButton.Size = new System.Drawing.Size(121, 23);
			this.errorButton.TabIndex = 10;
			this.errorButton.Text = "Ошибки";
			this.errorButton.UseVisualStyleBackColor = true;
			this.errorButton.Visible = false;
			this.errorButton.Click += new System.EventHandler(this.ErrorButton_Click);
			// 
			// OutputFileGenerate
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(600, 401);
			this.Controls.Add(this.errorButton);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.SamplePathButton);
			this.Controls.Add(this.SamplePathtextBox);
			this.Controls.Add(this.SamplePathLabel);
			this.Controls.Add(this.EmergencyLineDisconnection);
			this.Controls.Add(this.FormButton);
			this.Controls.Add(this.TemperatureAllowGroupBox);
			this.Controls.Add(this.ExploreButton);
			this.Controls.Add(this.PathLabel);
			this.Controls.Add(this.PathTextBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "OutputFileGenerate";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Формирование приложения №6 ПУР";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OutputFileGenerate_FormClosed);
			this.Load += new System.EventHandler(this.OutputFileGenerate_Load);
			this.TemperatureAllowGroupBox.ResumeLayout(false);
			this.TemperatureAllowGroupBox.PerformLayout();
			this.TemperatureGroupBox.ResumeLayout(false);
			this.TemperatureGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox PathTextBox;
		private System.Windows.Forms.Label PathLabel;
		private System.Windows.Forms.Button ExploreButton;
		private System.Windows.Forms.GroupBox TemperatureAllowGroupBox;
		private System.Windows.Forms.CheckBox TemperatureCheckBox;
		private System.Windows.Forms.GroupBox TemperatureGroupBox;
		private System.Windows.Forms.CheckBox checkBox40;
		private System.Windows.Forms.CheckBox checkBox35;
		private System.Windows.Forms.CheckBox checkBox30;
		private System.Windows.Forms.CheckBox checkBox_25;
		private System.Windows.Forms.CheckBox checkBox_20;
		private System.Windows.Forms.CheckBox checkBox25;
		private System.Windows.Forms.CheckBox checkBox_15;
		private System.Windows.Forms.CheckBox checkBox20;
		private System.Windows.Forms.CheckBox checkBox15;
		private System.Windows.Forms.Button SummerButton;
		private System.Windows.Forms.Button WinterButton;
		private System.Windows.Forms.Button AwayButton;
		private System.Windows.Forms.CheckBox checkBox_10;
		private System.Windows.Forms.Button SelectButton;
		private System.Windows.Forms.CheckBox checkBox0;
		private System.Windows.Forms.CheckBox checkBox_5;
		private System.Windows.Forms.CheckBox checkBox10;
		private System.Windows.Forms.CheckBox checkBox5;
		private System.Windows.Forms.Button FormButton;
		private System.Windows.Forms.CheckBox EmergencyLineDisconnection;
		private System.Windows.Forms.CheckBox checkBox45;
		private System.Windows.Forms.Label SamplePathLabel;
		private System.Windows.Forms.TextBox SamplePathtextBox;
		private System.Windows.Forms.Button SamplePathButton;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Button errorButton;
	}
}
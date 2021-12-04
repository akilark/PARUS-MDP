
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
			this.checkBox_40 = new System.Windows.Forms.CheckBox();
			this.checkBox40 = new System.Windows.Forms.CheckBox();
			this.checkBox_35 = new System.Windows.Forms.CheckBox();
			this.checkBox35 = new System.Windows.Forms.CheckBox();
			this.checkBox_30 = new System.Windows.Forms.CheckBox();
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
			this.checkBox11 = new System.Windows.Forms.CheckBox();
			this.checkBox_5 = new System.Windows.Forms.CheckBox();
			this.checkBox10 = new System.Windows.Forms.CheckBox();
			this.checkBox5 = new System.Windows.Forms.CheckBox();
			this.FormButton = new System.Windows.Forms.Button();
			this.BackButton = new System.Windows.Forms.Button();
			this.EmergencyLineDisconnection = new System.Windows.Forms.CheckBox();
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
			this.PathLabel.Location = new System.Drawing.Point(-8, 8);
			this.PathLabel.Name = "PathLabel";
			this.PathLabel.Size = new System.Drawing.Size(100, 48);
			this.PathLabel.TabIndex = 1;
			this.PathLabel.Text = "Путь к каталогу";
			this.PathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.PathLabel.Click += new System.EventHandler(this.PathLabel_Click);
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
			// 
			// TemperatureAllowGroupBox
			// 
			this.TemperatureAllowGroupBox.Controls.Add(this.TemperatureCheckBox);
			this.TemperatureAllowGroupBox.Controls.Add(this.TemperatureGroupBox);
			this.TemperatureAllowGroupBox.Location = new System.Drawing.Point(7, 59);
			this.TemperatureAllowGroupBox.Name = "TemperatureAllowGroupBox";
			this.TemperatureAllowGroupBox.Size = new System.Drawing.Size(581, 175);
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
			// 
			// TemperatureGroupBox
			// 
			this.TemperatureGroupBox.Controls.Add(this.checkBox_40);
			this.TemperatureGroupBox.Controls.Add(this.checkBox40);
			this.TemperatureGroupBox.Controls.Add(this.checkBox_35);
			this.TemperatureGroupBox.Controls.Add(this.checkBox35);
			this.TemperatureGroupBox.Controls.Add(this.checkBox_30);
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
			this.TemperatureGroupBox.Controls.Add(this.checkBox11);
			this.TemperatureGroupBox.Controls.Add(this.checkBox_5);
			this.TemperatureGroupBox.Controls.Add(this.checkBox10);
			this.TemperatureGroupBox.Controls.Add(this.checkBox5);
			this.TemperatureGroupBox.Location = new System.Drawing.Point(6, 51);
			this.TemperatureGroupBox.Name = "TemperatureGroupBox";
			this.TemperatureGroupBox.Size = new System.Drawing.Size(567, 123);
			this.TemperatureGroupBox.TabIndex = 0;
			this.TemperatureGroupBox.TabStop = false;
			// 
			// checkBox_40
			// 
			this.checkBox_40.AutoSize = true;
			this.checkBox_40.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox_40.Location = new System.Drawing.Point(505, 51);
			this.checkBox_40.Name = "checkBox_40";
			this.checkBox_40.Size = new System.Drawing.Size(50, 23);
			this.checkBox_40.TabIndex = 11;
			this.checkBox_40.Text = "-40";
			this.checkBox_40.UseVisualStyleBackColor = true;
			// 
			// checkBox40
			// 
			this.checkBox40.AutoSize = true;
			this.checkBox40.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox40.Location = new System.Drawing.Point(505, 22);
			this.checkBox40.Name = "checkBox40";
			this.checkBox40.Size = new System.Drawing.Size(54, 23);
			this.checkBox40.TabIndex = 11;
			this.checkBox40.Text = "+40";
			this.checkBox40.UseVisualStyleBackColor = true;
			// 
			// checkBox_35
			// 
			this.checkBox_35.AutoSize = true;
			this.checkBox_35.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox_35.Location = new System.Drawing.Point(441, 51);
			this.checkBox_35.Name = "checkBox_35";
			this.checkBox_35.Size = new System.Drawing.Size(50, 23);
			this.checkBox_35.TabIndex = 10;
			this.checkBox_35.Text = "-35";
			this.checkBox_35.UseVisualStyleBackColor = true;
			// 
			// checkBox35
			// 
			this.checkBox35.AutoSize = true;
			this.checkBox35.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox35.Location = new System.Drawing.Point(441, 22);
			this.checkBox35.Name = "checkBox35";
			this.checkBox35.Size = new System.Drawing.Size(54, 23);
			this.checkBox35.TabIndex = 10;
			this.checkBox35.Text = "+35";
			this.checkBox35.UseVisualStyleBackColor = true;
			// 
			// checkBox_30
			// 
			this.checkBox_30.AutoSize = true;
			this.checkBox_30.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox_30.Location = new System.Drawing.Point(377, 51);
			this.checkBox_30.Name = "checkBox_30";
			this.checkBox_30.Size = new System.Drawing.Size(50, 23);
			this.checkBox_30.TabIndex = 9;
			this.checkBox_30.Text = "-30";
			this.checkBox_30.UseVisualStyleBackColor = true;
			// 
			// checkBox30
			// 
			this.checkBox30.AutoSize = true;
			this.checkBox30.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox30.Location = new System.Drawing.Point(377, 22);
			this.checkBox30.Name = "checkBox30";
			this.checkBox30.Size = new System.Drawing.Size(54, 23);
			this.checkBox30.TabIndex = 9;
			this.checkBox30.Text = "+30";
			this.checkBox30.UseVisualStyleBackColor = true;
			// 
			// checkBox_25
			// 
			this.checkBox_25.AutoSize = true;
			this.checkBox_25.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox_25.Location = new System.Drawing.Point(313, 51);
			this.checkBox_25.Name = "checkBox_25";
			this.checkBox_25.Size = new System.Drawing.Size(50, 23);
			this.checkBox_25.TabIndex = 8;
			this.checkBox_25.Text = "-25";
			this.checkBox_25.UseVisualStyleBackColor = true;
			// 
			// checkBox_20
			// 
			this.checkBox_20.AutoSize = true;
			this.checkBox_20.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox_20.Location = new System.Drawing.Point(249, 51);
			this.checkBox_20.Name = "checkBox_20";
			this.checkBox_20.Size = new System.Drawing.Size(50, 23);
			this.checkBox_20.TabIndex = 7;
			this.checkBox_20.Text = "-20";
			this.checkBox_20.UseVisualStyleBackColor = true;
			// 
			// checkBox25
			// 
			this.checkBox25.AutoSize = true;
			this.checkBox25.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox25.Location = new System.Drawing.Point(313, 22);
			this.checkBox25.Name = "checkBox25";
			this.checkBox25.Size = new System.Drawing.Size(54, 23);
			this.checkBox25.TabIndex = 8;
			this.checkBox25.Text = "+25";
			this.checkBox25.UseVisualStyleBackColor = true;
			// 
			// checkBox_15
			// 
			this.checkBox_15.AutoSize = true;
			this.checkBox_15.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox_15.Location = new System.Drawing.Point(185, 51);
			this.checkBox_15.Name = "checkBox_15";
			this.checkBox_15.Size = new System.Drawing.Size(50, 23);
			this.checkBox_15.TabIndex = 6;
			this.checkBox_15.Text = "-15";
			this.checkBox_15.UseVisualStyleBackColor = true;
			// 
			// checkBox20
			// 
			this.checkBox20.AutoSize = true;
			this.checkBox20.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox20.Location = new System.Drawing.Point(249, 22);
			this.checkBox20.Name = "checkBox20";
			this.checkBox20.Size = new System.Drawing.Size(54, 23);
			this.checkBox20.TabIndex = 7;
			this.checkBox20.Text = "+20";
			this.checkBox20.UseVisualStyleBackColor = true;
			// 
			// checkBox15
			// 
			this.checkBox15.AutoSize = true;
			this.checkBox15.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox15.Location = new System.Drawing.Point(185, 22);
			this.checkBox15.Name = "checkBox15";
			this.checkBox15.Size = new System.Drawing.Size(54, 23);
			this.checkBox15.TabIndex = 6;
			this.checkBox15.Text = "+15";
			this.checkBox15.UseVisualStyleBackColor = true;
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
			// 
			// checkBox_10
			// 
			this.checkBox_10.AutoSize = true;
			this.checkBox_10.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox_10.Location = new System.Drawing.Point(121, 51);
			this.checkBox_10.Name = "checkBox_10";
			this.checkBox_10.Size = new System.Drawing.Size(50, 23);
			this.checkBox_10.TabIndex = 1;
			this.checkBox_10.Text = "-10";
			this.checkBox_10.UseVisualStyleBackColor = true;
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
			// 
			// checkBox11
			// 
			this.checkBox11.AutoSize = true;
			this.checkBox11.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox11.Location = new System.Drawing.Point(9, 51);
			this.checkBox11.Name = "checkBox11";
			this.checkBox11.Size = new System.Drawing.Size(36, 23);
			this.checkBox11.TabIndex = 0;
			this.checkBox11.Text = "0";
			this.checkBox11.UseVisualStyleBackColor = true;
			// 
			// checkBox_5
			// 
			this.checkBox_5.AutoSize = true;
			this.checkBox_5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox_5.Location = new System.Drawing.Point(65, 51);
			this.checkBox_5.Name = "checkBox_5";
			this.checkBox_5.Size = new System.Drawing.Size(42, 23);
			this.checkBox_5.TabIndex = 0;
			this.checkBox_5.Text = "-5";
			this.checkBox_5.UseVisualStyleBackColor = true;
			// 
			// checkBox10
			// 
			this.checkBox10.AutoSize = true;
			this.checkBox10.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox10.Location = new System.Drawing.Point(121, 22);
			this.checkBox10.Name = "checkBox10";
			this.checkBox10.Size = new System.Drawing.Size(54, 23);
			this.checkBox10.TabIndex = 1;
			this.checkBox10.Text = "+10";
			this.checkBox10.UseVisualStyleBackColor = true;
			// 
			// checkBox5
			// 
			this.checkBox5.AutoSize = true;
			this.checkBox5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.checkBox5.Location = new System.Drawing.Point(65, 22);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new System.Drawing.Size(46, 23);
			this.checkBox5.TabIndex = 0;
			this.checkBox5.Text = "+5";
			this.checkBox5.UseVisualStyleBackColor = true;
			// 
			// FormButton
			// 
			this.FormButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.FormButton.Location = new System.Drawing.Point(370, 268);
			this.FormButton.Name = "FormButton";
			this.FormButton.Size = new System.Drawing.Size(121, 25);
			this.FormButton.TabIndex = 4;
			this.FormButton.Text = "Сформировать";
			this.FormButton.UseVisualStyleBackColor = true;
			// 
			// BackButton
			// 
			this.BackButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.BackButton.Location = new System.Drawing.Point(497, 268);
			this.BackButton.Name = "BackButton";
			this.BackButton.Size = new System.Drawing.Size(75, 25);
			this.BackButton.TabIndex = 5;
			this.BackButton.Text = "Назад";
			this.BackButton.UseVisualStyleBackColor = true;
			this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
			// 
			// EmergencyLineDisconnection
			// 
			this.EmergencyLineDisconnection.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.EmergencyLineDisconnection.Location = new System.Drawing.Point(22, 235);
			this.EmergencyLineDisconnection.Name = "EmergencyLineDisconnection";
			this.EmergencyLineDisconnection.Size = new System.Drawing.Size(262, 69);
			this.EmergencyLineDisconnection.TabIndex = 6;
			this.EmergencyLineDisconnection.Text = "Учет каждого аварийного небаланса выполнялся отключением соответствующей ветви";
			this.EmergencyLineDisconnection.UseVisualStyleBackColor = true;
			// 
			// OutputFileGenerate
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(600, 305);
			this.Controls.Add(this.EmergencyLineDisconnection);
			this.Controls.Add(this.BackButton);
			this.Controls.Add(this.FormButton);
			this.Controls.Add(this.TemperatureAllowGroupBox);
			this.Controls.Add(this.ExploreButton);
			this.Controls.Add(this.PathLabel);
			this.Controls.Add(this.PathTextBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "OutputFileGenerate";
			this.Text = "Формирование приложения №6 ПУР";
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
		private System.Windows.Forms.CheckBox checkBox_40;
		private System.Windows.Forms.CheckBox checkBox40;
		private System.Windows.Forms.CheckBox checkBox_35;
		private System.Windows.Forms.CheckBox checkBox35;
		private System.Windows.Forms.CheckBox checkBox_30;
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
		private System.Windows.Forms.CheckBox checkBox11;
		private System.Windows.Forms.CheckBox checkBox_5;
		private System.Windows.Forms.CheckBox checkBox10;
		private System.Windows.Forms.CheckBox checkBox5;
		private System.Windows.Forms.Button FormButton;
		private System.Windows.Forms.Button BackButton;
		private System.Windows.Forms.CheckBox EmergencyLineDisconnection;
	}
}
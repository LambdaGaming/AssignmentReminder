namespace AssignmentReminder
{
	partial class AssignmentManager
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.AddButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.NameBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.DateChooser = new System.Windows.Forms.DateTimePicker();
			this.label3 = new System.Windows.Forms.Label();
			this.TimeChooser = new System.Windows.Forms.DateTimePicker();
			this.SuspendLayout();
			// 
			// AddButton
			// 
			this.AddButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.AddButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.AddButton.Location = new System.Drawing.Point(0, 158);
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(264, 23);
			this.AddButton.TabIndex = 1;
			this.AddButton.Text = "Add Assignment";
			this.AddButton.UseVisualStyleBackColor = true;
			this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(12, 63);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Due Date:";
			// 
			// NameBox
			// 
			this.NameBox.Location = new System.Drawing.Point(12, 26);
			this.NameBox.Name = "NameBox";
			this.NameBox.Size = new System.Drawing.Size(240, 20);
			this.NameBox.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(12, 10);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(95, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Assignment Name:";
			// 
			// DateChooser
			// 
			this.DateChooser.Location = new System.Drawing.Point(12, 79);
			this.DateChooser.MinDate = new System.DateTime(2020, 9, 3, 0, 0, 0, 0);
			this.DateChooser.Name = "DateChooser";
			this.DateChooser.Size = new System.Drawing.Size(240, 20);
			this.DateChooser.TabIndex = 0;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(12, 116);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Due Time:";
			// 
			// TimeChooser
			// 
			this.TimeChooser.Location = new System.Drawing.Point(12, 132);
			this.TimeChooser.MinDate = new System.DateTime(2020, 9, 3, 0, 0, 0, 0);
			this.TimeChooser.Name = "TimeChooser";
			this.TimeChooser.ShowUpDown = true;
			this.TimeChooser.Size = new System.Drawing.Size(240, 20);
			this.TimeChooser.TabIndex = 5;
			// 
			// AssignmentManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
			this.ClientSize = new System.Drawing.Size(264, 181);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.TimeChooser);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.NameBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.AddButton);
			this.Controls.Add(this.DateChooser);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "AssignmentManager";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Add Assignment";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button AddButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox NameBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DateTimePicker DateChooser;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DateTimePicker TimeChooser;
	}
}


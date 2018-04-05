namespace SPA.UI.Tutorial
{
	partial class DatagridTest
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
			this.chkAll = new System.Windows.Forms.CheckBox();
			this.dgTest = new System.Windows.Forms.DataGridView();
			this.button1 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgTest)).BeginInit();
			this.SuspendLayout();
			// 
			// chkAll
			// 
			this.chkAll.AutoSize = true;
			this.chkAll.Location = new System.Drawing.Point(72, 43);
			this.chkAll.Margin = new System.Windows.Forms.Padding(4);
			this.chkAll.Name = "chkAll";
			this.chkAll.Size = new System.Drawing.Size(89, 22);
			this.chkAll.TabIndex = 0;
			this.chkAll.Text = "Check All";
			this.chkAll.UseVisualStyleBackColor = true;
			this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
			// 
			// dgTest
			// 
			this.dgTest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgTest.Location = new System.Drawing.Point(72, 75);
			this.dgTest.Margin = new System.Windows.Forms.Padding(4);
			this.dgTest.Name = "dgTest";
			this.dgTest.Size = new System.Drawing.Size(1140, 530);
			this.dgTest.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(1137, 42);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			//this.button1.Click += new System.EventHandler(this.button1_Click);
			this.button1.Click += new System.EventHandler(this.ButtonClick);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1261, 656);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.dgTest);
			this.Controls.Add(this.chkAll);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgTest)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox chkAll;
		private System.Windows.Forms.DataGridView dgTest;
		private System.Windows.Forms.Button button1;
	}
}


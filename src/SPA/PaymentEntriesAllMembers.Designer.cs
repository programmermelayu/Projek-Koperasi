namespace SPA
{
    partial class PaymentEntriesAllMembers
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaymentEntriesAllMembers));
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNoLarian = new System.Windows.Forms.TextBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.radDateEnd = new Telerik.WinControls.UI.RadDateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radDateStart = new Telerik.WinControls.UI.RadDateTimePicker();
            this.radGroupBox2 = new Telerik.WinControls.UI.RadGroupBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgPayments = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radDateEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDateStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).BeginInit();
            this.radGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPayments)).BeginInit();
            this.SuspendLayout();
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radGroupBox1.Controls.Add(this.label6);
            this.radGroupBox1.Controls.Add(this.txtNoLarian);
            this.radGroupBox1.Controls.Add(this.btnShow);
            this.radGroupBox1.Controls.Add(this.radDateEnd);
            this.radGroupBox1.Controls.Add(this.label2);
            this.radGroupBox1.Controls.Add(this.label1);
            this.radGroupBox1.Controls.Add(this.radDateStart);
            this.radGroupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.radGroupBox1.HeaderText = "Tapis Rekod";
            this.radGroupBox1.Location = new System.Drawing.Point(16, 1);
            this.radGroupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Padding = new System.Windows.Forms.Padding(3, 22, 3, 2);
            this.radGroupBox1.Size = new System.Drawing.Size(1656, 68);
            this.radGroupBox1.TabIndex = 1;
            this.radGroupBox1.Text = "Tapis Rekod";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 30);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 18);
            this.label6.TabIndex = 48;
            this.label6.Text = "No Larian:";
            // 
            // txtNoLarian
            // 
            this.txtNoLarian.Location = new System.Drawing.Point(107, 27);
            this.txtNoLarian.Margin = new System.Windows.Forms.Padding(4);
            this.txtNoLarian.Name = "txtNoLarian";
            this.txtNoLarian.Size = new System.Drawing.Size(223, 25);
            this.txtNoLarian.TabIndex = 47;
            // 
            // btnShow
            // 
            this.btnShow.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnShow.Location = new System.Drawing.Point(899, 21);
            this.btnShow.Margin = new System.Windows.Forms.Padding(4);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(100, 36);
            this.btnShow.TabIndex = 3;
            this.btnShow.Text = "Papar";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // radDateEnd
            // 
            this.radDateEnd.CustomFormat = "MMM - yyyy";
            this.radDateEnd.Font = new System.Drawing.Font("Arial", 9F);
            this.radDateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.radDateEnd.Location = new System.Drawing.Point(766, 28);
            this.radDateEnd.Margin = new System.Windows.Forms.Padding(4);
            this.radDateEnd.Name = "radDateEnd";
            this.radDateEnd.ShowUpDown = true;
            this.radDateEnd.Size = new System.Drawing.Size(125, 23);
            this.radDateEnd.TabIndex = 2;
            this.radDateEnd.TabStop = false;
            this.radDateEnd.Text = "Aug - 2014";
            this.radDateEnd.Value = new System.DateTime(2014, 8, 1, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F);
            this.label2.Location = new System.Drawing.Point(679, 31);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 17);
            this.label2.TabIndex = 16;
            this.label2.Text = "sehingga";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F);
            this.label1.Location = new System.Drawing.Point(367, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 17);
            this.label1.TabIndex = 15;
            this.label1.Text = "Penerimaan dari bulan";
            // 
            // radDateStart
            // 
            this.radDateStart.CustomFormat = "MMM - yyyy";
            this.radDateStart.Font = new System.Drawing.Font("Arial", 9F);
            this.radDateStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.radDateStart.Location = new System.Drawing.Point(547, 28);
            this.radDateStart.Margin = new System.Windows.Forms.Padding(4);
            this.radDateStart.Name = "radDateStart";
            this.radDateStart.ShowUpDown = true;
            this.radDateStart.Size = new System.Drawing.Size(120, 23);
            this.radDateStart.TabIndex = 1;
            this.radDateStart.TabStop = false;
            this.radDateStart.Text = "Sep - 2013";
            this.radDateStart.Value = new System.DateTime(2013, 9, 1, 0, 0, 0, 0);
            // 
            // radGroupBox2
            // 
            this.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radGroupBox2.Controls.Add(this.chkAll);
            this.radGroupBox2.Controls.Add(this.btnDelete);
            this.radGroupBox2.Controls.Add(this.btnClose);
            this.radGroupBox2.Controls.Add(this.dgPayments);
            this.radGroupBox2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.radGroupBox2.HeaderText = "";
            this.radGroupBox2.Location = new System.Drawing.Point(16, 76);
            this.radGroupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.radGroupBox2.Name = "radGroupBox2";
            this.radGroupBox2.Padding = new System.Windows.Forms.Padding(3, 22, 3, 2);
            this.radGroupBox2.Size = new System.Drawing.Size(1656, 810);
            this.radGroupBox2.TabIndex = 3;
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(22, 27);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(113, 22);
            this.chkAll.TabIndex = 41;
            this.chkAll.Text = "Pilih Semua";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(1412, 12);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(123, 36);
            this.btnDelete.TabIndex = 40;
            this.btnDelete.Text = "Padam";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(1542, 12);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(107, 36);
            this.btnClose.TabIndex = 39;
            this.btnClose.Text = "Tutup";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgPayments
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSteelBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgPayments.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgPayments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgPayments.Location = new System.Drawing.Point(19, 56);
            this.dgPayments.Margin = new System.Windows.Forms.Padding(4);
            this.dgPayments.Name = "dgPayments";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgPayments.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgPayments.Size = new System.Drawing.Size(1630, 740);
            this.dgPayments.TabIndex = 4;
            // 
            // PaymentEntriesAllMembers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1688, 901);
            this.Controls.Add(this.radGroupBox2);
            this.Controls.Add(this.radGroupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PaymentEntriesAllMembers";
            this.Text = "Padam Rekod Penerimaan";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PaymentEntries_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radDateEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDateStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).EndInit();
            this.radGroupBox2.ResumeLayout(false);
            this.radGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPayments)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox2;
        private System.Windows.Forms.DataGridView dgPayments;
        private Telerik.WinControls.UI.RadDateTimePicker radDateEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Telerik.WinControls.UI.RadDateTimePicker radDateStart;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNoLarian;
        private System.Windows.Forms.CheckBox chkAll;

    }
}
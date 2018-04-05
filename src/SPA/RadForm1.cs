using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace SPA
{
    public partial class RadForm1 : Telerik.WinControls.UI.RadForm
    {
        public RadForm1()
        {
            InitializeComponent();
        }

        private void RadForm1_Load(object sender, EventArgs e)
        {
            this.radDateTimePicker1.CustomFormat = "MMM - yyyy";
            this.radDateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.radDateTimePicker1.ShowUpDown = true;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            RadMessageBox.Show("Month Picked: " + this.radDateTimePicker1.Value.Month.ToString() + " Year Picked: " + this.radDateTimePicker1.Value.Year.ToString());
        }

    }
}

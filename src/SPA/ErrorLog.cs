using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SPA.Core;
using SPA.Control;

namespace SPA
{
    public partial class FormErrorLog : SPAForm
    {
        public FormErrorLog()
        {
            InitializeComponent();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            txtLog.Clear();
            var selectedFile = "error_" + String.Format("{0:yyyyMMdd}", dateTimePicker1.Value) + ".txt";
            string filePath = DirectoryHandler.GetDefaultDirectoryLog() + selectedFile;
            if (System.IO.File.Exists(filePath))
            {
                txtLog.ForeColor = Color.Red;
                foreach (var line in File.ReadAllLines(filePath))
                {
                    txtLog.Text += line;
                    txtLog.Text += Environment.NewLine;
                }
            }
            else
            {
                txtLog.ForeColor = Color.Green;
                txtLog.Text += "- No errors detected -";
            }
            this.Cursor = Cursors.Default;
        }

        private void ErrorLog_Load(object sender, EventArgs e)
        {
            this.btnRead_Click(sender, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}

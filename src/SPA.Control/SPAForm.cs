using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SPA.Control
{
    public class SPAForm : System.Windows.Forms.Form 
    {
    
        public enum MessageFor
        {
            Error,
            Information,
            Instruction,
            Restriction,
            Warning
        }

        public static void ShowMessage(string message, MessageFor messageFor)
        {
            switch (messageFor)
            {
                case MessageFor.Error:
                    MessageBox.Show(message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case MessageFor.Information:
                    MessageBox.Show(message, "Pemberitahuan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case MessageFor.Instruction:
                    MessageBox.Show(message, "Arahan", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                case MessageFor.Warning:
                    MessageBox.Show(message, "Amaran", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                case MessageFor.Restriction:
                    MessageBox.Show(message, "Amaran", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    break;
                default:
                    MessageBox.Show(message, "Pemberitahuan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        public static void ShowSaveSuccess()
        {
            ShowMessage("Rekod ini telah berjaya disimpan.", MessageFor.Information);
        }

        public static void ShowDeleteSuccess()
        {
            ShowMessage("Rekod ini telah berjaya dipadam.", MessageFor.Information);
        }

        public static void ShowUpdateSuccess()
        {
            ShowMessage("Rekod ini telah berjaya dikemaskini.", MessageFor.Information);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SPAForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "SPAForm";
            this.Load += new System.EventHandler(this.SPAForm_Load);
            this.ResumeLayout(false);

        }

        private void SPAForm_Load(object sender, EventArgs e)
        {

        }

    }
}

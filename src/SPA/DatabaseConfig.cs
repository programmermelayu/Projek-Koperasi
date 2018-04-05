using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using SPA.Config;
using SPA.Control;
using System.Data.SqlClient;
using SPA.Cache;

namespace SPA
{
    public partial class DatabaseConfig : SPAForm 
    {
        public DatabaseConfig()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (XmlWriter writer = XmlWriter.Create(DirectoryHandler.GetDefaultConfigPath() + "\\DbConfig.xml"))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Configuration");

                    writer.WriteStartElement("Database");

                    writer.WriteElementString("DbServer", txtDbServer.Text.Trim());
                    writer.WriteElementString("DbName", txtDbName.Text.Trim());

                    writer.WriteEndElement();

                    writer.WriteEndElement();
                    writer.WriteEndDocument();

                    ShowMessage("Tetapan fail telah berjaya dibina. Sila tutup dan buka semula aplikasi ini.", MessageFor.Information);
                    Close();
                }

            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageFor.Error);
            }

        }

        private void DatabaseConfig_Load(object sender, EventArgs e)
        {
            txtDbServer.Text = ConfigCache.DBServer;
            txtDbName.Text = ConfigCache.DBName;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnReconnect_Click(object sender, EventArgs e)
        {
            string connString = "Data Source=" + this.txtDbServer.Text.Trim() + ";Initial Catalog=" + txtDbName.Text.Trim() + ";User ID=sa;Password=#123456#;Max Pool Size=30000"; ;

            try
            {
                var testConnection = new SqlConnection(connString);
                if (testConnection.State != System.Data.ConnectionState.Open)
                {
                    testConnection.Open();
                }

                var reader = new SqlCommand("SELECT TOP 1 * FROM users", testConnection);
                SqlDataReader dr = reader.ExecuteReader();
                dr.Close();
                ShowMessage("Sambungan berjaya. Sila simpan tetapan ini.", MessageFor.Information);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, MessageFor.Error);
            }

        }
    }
}

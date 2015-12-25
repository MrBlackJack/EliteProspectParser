using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using System.IO;
using System.Xml;

namespace EliteProspectParser
{
    public partial class BDSetttingsForm : Form
    {
        private AppSettings _appSetttings = new AppSettings();

        public BDSetttingsForm()
        {
            InitializeComponent();
        }

        public BDSetttingsForm(AppSettings appSettings)
            : this()
        {
            _appSetttings = appSettings;
            _appSetttings.Load();
            PropertyBD.SelectedObject = _appSetttings;
            PropertyBD.PropertySort = PropertySort.NoSort;
        }

        private void btn_TestConnect_Click(object sender, EventArgs e)
        {
            Npgsql.NpgsqlConnection NpgConn = null;

            string connString = string.Format("Server={0};port={1};User Id={2};Password={3};Database={4}",
                _appSetttings.Server, _appSetttings.Port.ToString(), _appSetttings.UserName, _appSetttings.Password, _appSetttings.DBName);

            NpgConn = new Npgsql.NpgsqlConnection(connString);

            try
            {
                NpgConn.Open();
                NpgConn.Close();
                MessageBox.Show("Подключение прошло успешно!", "Тест подключения");
            }
            catch (Exception)
            {
                MessageBox.Show("Подключение не удалось!", "Тест подключения");
            }


        }

        private void btn_SaveSetting_Click(object sender, EventArgs e)
        {
            _appSetttings.Save();
            MessageBox.Show("Настройки были сохранены", "Сохранение");
        }
    }
}

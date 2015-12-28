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
    public class AppSettings
    {
        private string _server = "localhost";
        private int _port = 5432;
        private string _dbname = "";
        private string _userName = "postgres";
        private string _password = "";
        private XmlDocument xDoc = null;
        private string xmlpath = Application.StartupPath + @"\Settings.xml";
        private string setPath = Application.StartupPath + @"\Settings.txt";
        private StringBuilder settings = new StringBuilder();

        public AppSettings()
        {
            if (File.Exists(setPath))
            {
                foreach (string set in File.ReadAllLines(setPath))
                {
                    string[] paramValue = set.Split('=');
                    switch (paramValue[0])
                    {
                        case "sheduler_enabled":
                            sheduler_enabled = Convert.ToBoolean(paramValue[1]);
                            break;
                        case "CheckLeagues":
                            _checkLeagues = new List<string>((paramValue[1]).Split(';'));
                            break;
                        case "Server":
                            _server = paramValue[1];
                            break;
                        case "Port":
                            _port = Convert.ToInt16(paramValue[1]);
                            break;
                        case "UserName":
                            _userName = paramValue[1];
                            break;
                        case "Password":
                            _password = paramValue[1];
                            break;
                        case "DBName":
                            _dbname = paramValue[1];
                            break;
                        default:
                            break;
                    }
                }
            }

            xDoc = new XmlDocument();
            if (File.Exists(xmlpath))
            {
                xDoc.Load(xmlpath);
            }
            else
            {
                XmlDeclaration xmlDeclaration = xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = xDoc.DocumentElement;
                xDoc.InsertBefore(xmlDeclaration, root);

                XmlElement RootElement = xDoc.CreateElement(string.Empty, "Root", string.Empty);
                xDoc.AppendChild(RootElement);

                xDoc.Save(xmlpath);
            }

            if (File.Exists(Application.StartupPath + "/Scheduler.txt"))
                sheduler_enabled = Convert.ToBoolean(File.ReadAllText(Application.StartupPath + "/Scheduler.txt").Split('=')[1]);
        }

        [Browsable(false)]
        public readonly Paths paths = new Paths();
        
        [Browsable(false)]
        public readonly bool sheduler_enabled = false;

        [Browsable(false)]
        public string ConnectionString
        {
            get
            {
                return
                string.Format("Server={0};port={1};User Id={2};Password={3};Database={4}",
                _server, _port, _userName, _password, _dbname);
            }
        }

        private List<string> _checkLeagues = new List<string>();
        [Browsable(false)]
        public List<string> CheckLeagues
        {
            get { return _checkLeagues; }
            set { _checkLeagues = value; }
        }

        [Category("Сервер PostgreSQL"), DisplayName("Сервер")]
        public string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        [Category("Сервер PostgreSQL"), DisplayName("Порт")]
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        [Category("Сервер PostgreSQL"), DisplayName("База данных")]
        public string DBName
        {
            get { return _dbname; }
            set { _dbname = value; }
        }

        [Category("Сервер PostgreSQL"), DisplayName("Пользователь")]
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        [Category("Сервер PostgreSQL"), DisplayName("Пароль")]
        [PasswordPropertyText(true)]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public void Load()
        {
            //Блок с настройками подключения к БД
            if (xDoc.SelectSingleNode("Root/BDSettings") != null)
            {
                XmlNode bdSetting = xDoc.SelectSingleNode("Root/BDSettings");
                _server = bdSetting.Attributes["Server"].Value;
                _port = Convert.ToInt16(bdSetting.Attributes["Port"].Value);
                _dbname = bdSetting.Attributes["DBName"].Value;
                _userName = bdSetting.Attributes["UserName"].Value;
                _password = bdSetting.Attributes["Password"].Value;
            }

            //Блок с настройками лиг
            if (xDoc.SelectSingleNode("Root/Leagues") != null)
            {
                XmlNode lNode = xDoc.SelectSingleNode("Root/Leagues");
                _checkLeagues = new List<string>(lNode.Attributes["Value"].Value.Split(';'));
            }

            settings.AppendLine(string.Format("Server={0}", _server));
            settings.AppendLine(string.Format("Port={0}", _port));
            settings.AppendLine(string.Format("DBName={0}", _dbname));
            settings.AppendLine(string.Format("UserName={0}", _userName));
            settings.AppendLine(string.Format("Password={0}", _password));

            File.WriteAllText(setPath, settings.ToString(), Encoding.UTF8);
        }

        public void Save()
        {

            //Сохранение настроек БД
            if (xDoc.SelectSingleNode("Root/BDSettings") == null)
            {
                XmlElement bdElement = xDoc.CreateElement("BDSettings");
                xDoc.SelectSingleNode("Root").AppendChild(bdElement);

                bdElement.SetAttribute("Server", _server);
                bdElement.SetAttribute("Port", _port.ToString());
                bdElement.SetAttribute("DBName", _dbname);
                bdElement.SetAttribute("UserName", _userName);
                bdElement.SetAttribute("Password", _password);
            }
            else
            {
                XmlNode bdElement = xDoc.SelectSingleNode("Root/BDSettings");

                (bdElement as XmlElement).SetAttribute("Server", _server);
                (bdElement as XmlElement).SetAttribute("Port", _port.ToString());
                (bdElement as XmlElement).SetAttribute("DBName", _dbname);
                (bdElement as XmlElement).SetAttribute("UserName", _userName);
                (bdElement as XmlElement).SetAttribute("Password", _password);

                xDoc.Save(xmlpath);
            }

            //Сохранение настройки лиг
            if (_checkLeagues.Count > 0)
            {
                XmlElement leagueElement = null;
                if (xDoc.SelectSingleNode("Root/Leagues") == null)
                {
                    leagueElement = xDoc.CreateElement("Leagues");
                    xDoc.SelectSingleNode("Root").AppendChild(leagueElement);
                }
                else leagueElement = (xDoc.SelectSingleNode("Root/Leagues") as XmlElement);

                leagueElement.SetAttribute("Value", _checkLeagues.Aggregate((i, j) => i + ";" + j));
            }

            xDoc.Save(xmlpath);
        }
    }

    public class Paths
    {
        public Paths()
        {
            //Проверка директорий на существование
            if (!Directory.Exists(_output)) Directory.CreateDirectory(_output);
            if (!Directory.Exists(_teams)) Directory.CreateDirectory(_teams);
            if (!Directory.Exists(_players)) Directory.CreateDirectory(_players);
        }

        private string _output = Application.StartupPath + @"\Output";
        public string Output 
        {
            get { return _output; }
            set { _output = value; }
        }

        private string _teams = Application.StartupPath + @"\Output\TeamsLogo";
        public string TeamsLogo
        {
            get { return _teams; }
            set { _teams = value; }
        }

        private string _players = Application.StartupPath + @"\Output\PlayersLogo";
        public string PlayersLogo
        {
            get { return _teams; }
            set { _teams = value; }
        }
    }
}

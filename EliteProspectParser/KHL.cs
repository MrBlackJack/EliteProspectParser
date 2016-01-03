using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Threading;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
namespace EliteProspectParser
{
    public class KHL
    {
        private Label lblStatus = null;
        private ListBox log = null;
        private Label lblTValue = null;
        private DateTime start;

        public KHL(Label _status, ListBox _log, Label _lblTValue, DateTime _start)
        {
            lblStatus = _status;
            lblTValue = _lblTValue;
            log = _log;
            start = _start;
        }

        public List<Player> _listOfPlayers = new List<Player>();
        public List<Team> _listOfTeams = new List<Team>();

        public string upfirstletter(string str)
        {
            string[] words = str.Split('_');
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < words.Length; i++)
                result.Append(words[i].Substring(0, 1).ToUpper() + words[i].Substring(1) + " ");

            return result.ToString().Trim();
        }

        public List<Team> getTeams(string[] parts, HtmlAgilityPack.HtmlDocument hDoc)
        {
            string url = "http://www.khl.ru";
            for (int i = 0; i < parts.Length; i++)
            {
                var teamsNodes = hDoc.DocumentNode.SelectNodes(string.Format("//table[@id = '{0}']/tr/td[a and img]", parts[i]));

                foreach (var team in teamsNodes)
                {
                    Team _team = new Team();
                    _team.href = @"http://www.khl.ru" + team.SelectSingleNode("a").Attributes["href"].Value + "team/";
                    _team.urlLogo = url + team.SelectSingleNode("img").Attributes["src"].Value;
                    _team.nameRus = team.SelectSingleNode("img").Attributes["alt"].Value;
                    
                    string nm = _team.href.Substring(_team.href.IndexOf("clubs/") + 6);
                    _team.name = nm.Substring(0, nm.IndexOf('/')) == "cska" ? "CSKA" : upfirstletter(nm.Substring(0, nm.IndexOf('/')));

                    _listOfTeams.Add(_team);
                }
            }

            return _listOfTeams;
        }


        public string getcurdate(string khldate)
        {
            //19 Июля 1989
            string[] dt = khldate.Split(' ');
            string month = "";
            switch (dt[1])
            {
                case "Января":
                    month = "01";
                    break;
                case "Февраля":
                    month = "02";
                    break;
                case "Марта":
                    month = "03";
                    break;
                case "Апреля":
                    month = "04";
                    break;
                case "Мая":
                    month = "05";
                    break;
                case "Июня":
                    month = "06";
                    break;
                case "Июля":
                    month = "07";
                    break;
                case "Августа":
                    month = "08";
                    break;
                case "Сентября":
                    month = "09";
                    break;
                case "Октября":
                    month = "10";
                    break;
                case "Ноября":
                    month = "11";
                    break;
                case "Декабря":
                    month = "12";
                    break;
            }

            return string.Format("{0}-{1}-{2}", dt[2], month, dt[0].Length > 1?dt[0]:"0"+dt[0]);
        }

        public void getPlayers(Team team)
        {
            string url = team.href;
            string mainPage = "http://www.khl.ru";

            WebClient wb = new WebClient();
            wb.Encoding = Encoding.UTF8;
            HtmlAgilityPack.HtmlDocument hDoc = new HtmlAgilityPack.HtmlDocument();

            hDoc.LoadHtml(wb.DownloadString(url.Trim()));
            var players = hDoc.DocumentNode.SelectNodes("//tbody/tr/td/a");

            foreach (var item in players)
            {
                string playerHref = item.Attributes["href"].Value;


                WebClient wbAttrib = new WebClient();
                wbAttrib.Encoding = Encoding.UTF8;

                HtmlAgilityPack.HtmlDocument hDocAttrib = new HtmlAgilityPack.HtmlDocument();
                hDocAttrib.LoadHtml(wbAttrib.DownloadString((mainPage + playerHref).Trim()));

                string pNameRus = hDocAttrib.DocumentNode.SelectSingleNode("//h3[@class = 'e-player_name']").ChildNodes[0].InnerText;
                if (pNameRus.IndexOf("№") > 0) pNameRus = pNameRus.Substring(0, pNameRus.IndexOf("№") - pNameRus.Length);

                /*Загрузка фото игрока*/
                string pathPlayers = Application.StartupPath + @"\Output\PlayersLogo";
                string urlPhoto = mainPage + hDocAttrib.DocumentNode.SelectSingleNode("//dt[@class = 'e-details_img']/img").Attributes["src"].Value;
                
                //Загрузка файла
                /*WebClient Client = new WebClient();
                wb.Encoding = Encoding.UTF8;
                Client.DownloadFile(urlPhoto, pathPlayers + @"\" + pName.Trim() + ".jpg");*/

                string pName = hDocAttrib.DocumentNode.SelectSingleNode("//p[@class = 'e-translit_name']").InnerText;
                string pNumber = null;
                try
                {
                    pNumber = hDocAttrib.DocumentNode.SelectSingleNode("//span[@class = 'e-num']").InnerText;
                }
                catch (Exception)
                {
                    pNumber = "Отсутствует";
                }

                var playerInfo = hDocAttrib.DocumentNode.SelectNodes("//table[@class  = 'b-player_data']/tr/td");

                string pBirthDate = playerInfo[0].SelectSingleNode("h4").InnerText;
                string pHeight = playerInfo[1].SelectSingleNode("h4").InnerText;
                string pWeight = playerInfo[2].SelectSingleNode("h4").InnerText;

                var playerInfo_ = hDocAttrib.DocumentNode.SelectNodes("//span[@class  = 'e-club m-fl']/span/small");

                string pShoots = null;
                try
                {
                    pShoots = playerInfo[4].SelectSingleNode("h4").InnerText;
                }
                catch (Exception)
                {
                    pShoots = "Отсутствует";
                }


                string pPosition = null;
                try
                {
                    string[] PlayerSplit = null;
                    if (playerInfo_.Count == 2)
                        PlayerSplit = playerInfo_[1].InnerText.Replace("\n", "|").Split('|');
                    else
                        PlayerSplit = playerInfo_[2].InnerText.Replace("\n", "|").Split('|');

                    switch (PlayerSplit[1].Trim())
                    {
                        case "защитник":
                            pPosition = "DW";
                            break;
                        case "вратарь":
                            pPosition = "G";
                            break;
                        case "нападающий":
                            pPosition = "FW";
                            break;
                        default:
                            break;
                    };
                }
                catch (Exception)
                {
                    pPosition = "Отсутствует";
                }

                _listOfPlayers.Add(new Player()
                {
                    Name = pName,
                    NameRus = pNameRus,
                    Number = pNumber,
                    BirthDate = getcurdate(pBirthDate),
                    Height = pHeight,
                    Weight = pWeight,
                    Position = pPosition,
                    Shoots = pShoots,
                    team = team,
                    Photo = urlPhoto
                });

                try
                {
                    string logStr = string.Format("{0}->{1}->{2}->{3}", "khl.ru", "KHL", team.name.Trim(), pName.Trim());
                    if (log.InvokeRequired) log.Invoke(new Action<string>((s) => log.Items.Add(s)), logStr);
                    else log.Items.Add(logStr);


                    string status = (Convert.ToInt16(lblStatus.Text) + 1).ToString();
                    if (lblStatus.InvokeRequired) lblStatus.Invoke(new Action<string>((s) => lblStatus.Text = s), status);
                    else lblStatus.Text = status;

                    string time = (DateTime.Now - start).ToString("hh\\:mm\\:ss");
                    if (lblTValue.InvokeRequired) lblTValue.Invoke(new Action<string>((s) => lblTValue.Text = s), time);
                    else lblTValue.Text = time;
                }
                catch (Exception)
                {
                    string logStr = "Ошибка -> Не удалось обновить статус!";
                    if (log.InvokeRequired) log.Invoke(new Action<string>((s) => log.Items.Add(s)), logStr);
                    else log.Items.Add(logStr);
                }
            }
        }
    }
}

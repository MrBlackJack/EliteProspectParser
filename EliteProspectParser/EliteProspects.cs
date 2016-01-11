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
    public class EliteProspects
    {
        private Label lblStatus = null;
        private ListBox log = null;
        private Label lblTValue = null;
        private DateTime start;

        public EliteProspects(Label _status, ListBox _log, Label _lblTValue, DateTime _start)
        {
            lblStatus = _status;
            lblTValue = _lblTValue;
            log = _log;
            start = _start;
        }

        public List<Team> _listOfTeams = new List<Team>();
        public List<Player> _listOfPlayers = new List<Player>();
        public List<League> _listOfLeague = new List<League>();

        public void getPlayers(League league)
        {
            _listOfLeague.Add(league);

            string mainPage = "http://www.eliteprospects.com/";
            string url = "http://www.eliteprospects.com/" + league.href;

            WebClient wb = new WebClient();
            
            //Задаем кодировку
            wb.Encoding = Encoding.GetEncoding("ISO-8859-1");
            HtmlAgilityPack.HtmlDocument hDoc = new HtmlAgilityPack.HtmlDocument();
            try
            {
                hDoc.LoadHtml(wb.DownloadString(url.Trim()));
            }
            catch (Exception)
            {
                string logStr = string.Format("Ошибка -> Не удалось загрузить лигу -> {0}", league.Name);
                if (log.InvokeRequired) log.Invoke(new Action<string>((s) => log.Items.Add(s)), logStr);
                else log.Items.Add(logStr);

                return;
            }

            var teamsNodes = hDoc.DocumentNode.SelectNodes("//table[@class = 'tableborder']/tr/td/a[contains(@href, 'team.php?team=')]");

            if (teamsNodes == null) return;

            foreach (var t in teamsNodes)
            {
                if (_listOfTeams.Count > 1)
                {
                    try
                    {
                        var isDuplicates = _listOfTeams.First(c => c.name == t.InnerText);
                        if ((isDuplicates as Team) != null)
                            break;
                    }
                    catch (Exception)
                    {
                    }
                };

                Team team = new Team
                {
                    name = t.InnerText.Trim(),
                    href = t.Attributes["href"].Value.Substring(0, t.Attributes["href"].Value.IndexOf("&year")),
                    league = league
                };

                _listOfTeams.Add(team);
            }

            foreach (var t in _listOfTeams)
            {
                //if (t.name == "Reaktor Nizhnekamsk")
                //    MessageBox.Show("Угу");

                string urlTeam = "http://www.eliteprospects.com/" + t.href;

                HtmlAgilityPack.HtmlDocument hDocAttrib = new HtmlAgilityPack.HtmlDocument();
                try
                {
                    hDocAttrib.LoadHtml(wb.DownloadString(urlTeam));
                }
                catch (Exception)
                {
                    string logStr = string.Format("Ошибка -> не удалось загрузить команду -> {0}", t.name);
                    if (log.InvokeRequired) log.Invoke(new Action<string>((s) => log.Items.Add(s)), logStr);
                    else log.Items.Add(logStr);

                    return;
                }

                //Загружаем лого команды
                string pathTeams = Application.StartupPath + @"\Output\TeamsLogo";
                var img = hDocAttrib.DocumentNode.SelectSingleNode("//img[contains(@src, 'http://files.eliteprospects.com/layout/logos/')]");
                string urlLogo = (img == null ? "" : img.Attributes["src"].Value);

                t.urlLogo = urlLogo;

                //Загрузка файла
                /*if (urlLogo != "")
                    wb.DownloadFile(urlLogo, pathTeams + @"\" + t.name + ".png");*/

                //Получаем всех игроков команды
                var players = hDocAttrib.DocumentNode.SelectNodes("//tr[@bordercolor='white' and @bgcolor='white']");
                if (players == null) break;

                foreach (var item in players)
                {
                    string nm = item.SelectNodes("td")[1].InnerText.Trim().Replace("&nbsp;", "");
                    string playerHref = mainPage + item.SelectSingleNode("td/a").Attributes["href"].Value;

                    string pName = nm.Substring(0, nm.IndexOf("("));
                    string pNumber = item.SelectNodes("td")[0].InnerText.Trim().Replace("#", "");
                    string age = item.SelectNodes("td")[2].InnerText.Trim();
                    string pBirthDate = item.SelectNodes("td")[3].InnerText.Trim();
                    string pHeight = item.SelectNodes("td")[5].SelectNodes("span")[0].InnerText;
                    string pBirthPlace = item.SelectNodes("td")[3].InnerText.Trim();
                    string pWeight = item.SelectNodes("td")[6].SelectNodes("span")[0].InnerText;
                    string pShoots = item.SelectNodes("td")[7].InnerText.Trim();
                    string pPosition = nm.Substring(nm.IndexOf("(") + 1).Replace(")", "");

                    //Загружаем фото игрока
                    HtmlAgilityPack.HtmlDocument hDocPlayer = new HtmlAgilityPack.HtmlDocument();
                    hDocPlayer.LoadHtml(wb.DownloadString(playerHref));

                    string pathPlayers = Application.StartupPath + @"\Output\PlayersLogo";
                    var imgPhoto = hDocPlayer.DocumentNode.SelectSingleNode("//img[contains(@src, 'http://files.eliteprospects.com/layout/players/')]");
                    string urlPhoto = (imgPhoto == null ? "" : imgPhoto.Attributes["src"].Value);
                    
                    //Загрузка файла
                    /*if (urlPhoto != "")
                        wb.DownloadFile(urlPhoto, pathPlayers + @"\" + pName + ".png");*/

                    _listOfPlayers.Add(new Player()
                    {
                        Name = pName,
                        Number = pNumber,
                        BirthDate = pBirthDate,
                        Height = pHeight,
                        Weight = pWeight,
                        Position = pPosition,
                        Shoots = pShoots,
                        team = t,
                        Photo = urlPhoto,
                        EliteID = playerHref.Substring(playerHref.IndexOf('=') + 1)
                    });

                    try
                    {
                        string status = (Convert.ToInt16(lblStatus.Text) + 1).ToString();
                        if (lblStatus.InvokeRequired) lblStatus.Invoke(new Action<string>((s) => lblStatus.Text = s), status);
                        else lblStatus.Text = status;

                        string logStr = string.Format("{0}->{1}->{2}->{3}", "EliteProspects", league.Name.Trim(), t.name.Trim(), pName.Trim());
                        if (log.InvokeRequired) log.Invoke(new Action<string>((s) => log.Items.Add(s)), logStr);
                        else log.Items.Add(logStr);

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
}

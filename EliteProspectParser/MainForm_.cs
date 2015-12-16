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
    public partial class MainForm_ : Form
    {
        private List<League> leagues = new List<League>();
        private List<string> parts = new List<string>();

        public MainForm_()
        {
            InitializeComponent();
            MainFormLoad();
        }


        struct BackgroundWorkerParams
        {
            public int param1;
        }

        public void MainFormLoad()
        {
            string pathOutput = Application.StartupPath + @"\Output";
            string pathTeams = Application.StartupPath + @"\Output\TeamsLogo";
            string pathPlayers = Application.StartupPath + @"\Output\PlayersLogo";
            //Проверка директорий на существование
            if (!Directory.Exists(pathOutput)) Directory.CreateDirectory(pathOutput);
            if (!Directory.Exists(pathTeams)) Directory.CreateDirectory(pathTeams);
            if (!Directory.Exists(pathPlayers)) Directory.CreateDirectory(pathPlayers);

            //Главная страница сайта
            string MainPage = @"http://www.eliteprospects.com/";

            string url = @"http://www.eliteprospects.com/league_central.php#na";
            WebClient wb = new WebClient();
            wb.Encoding = Encoding.GetEncoding("ISO-8859-1");
            HtmlAgilityPack.HtmlDocument hDoc = new HtmlAgilityPack.HtmlDocument();
            hDoc.LoadHtml(wb.DownloadString(url.Trim()));

            HtmlAgilityPack.HtmlNodeCollection LeagueNodes = hDoc.DocumentNode.SelectNodes("//table[@class = 'tableborder']");

            int i = 0;
            foreach (HtmlAgilityPack.HtmlNode node in LeagueNodes)
            {
                foreach (var item in node.SelectNodes("tr[@class = 'trbackground']/td"))
                    parts.Add(item.InnerText);

                HtmlAgilityPack.HtmlNodeCollection teamNodes = node.SelectNodes("tr//td[@valign='top']");
                foreach (var items in teamNodes)
                {
                    var leaguesNodes = items.SelectNodes("a");
                    leaguesNodes = leaguesNodes == null ? items.SelectNodes("p/a") : items.SelectNodes("a");

                    if (leaguesNodes == null) continue;
                    foreach (var item in leaguesNodes)
                    {
                        leagues.Add(new League()
                        {
                            href = item.Attributes["href"].Value == null ? "" : item.Attributes["href"].Value,
                            Name = item.InnerText,
                            Part = parts[i]
                        });
                    }
                    i++;
                }
            }

            int group = 0;
            int cntPart = leagues.Count / 10;
            int cntOfGroups = 0;

            for (int j = 0; j < leagues.Count; j++)
            {
                group = j / cntPart + 1;
                leagues[j].group = j / cntPart + 1;
                cntOfGroups++;
            }

            lblStatus.Text = "0";
            int countOfThreads = group;

            List<Player> listOfPlayear = new List<Player>();
            Players PlayersObj = new Players(lblStatus, log_view);

            for (int l = 1; l <= group; l++)
            {
                BackgroundWorker bwOLD = new BackgroundWorker();

                //Задаем тело потока
                bwOLD.DoWork += (object sender, DoWorkEventArgs e) =>
                {
                    var groupNmr = (BackgroundWorkerParams)e.Argument;

                    foreach (var ls in leagues)
                    {
                        if (ls.group == groupNmr.param1 && (ls.Name.Trim() == "KHL" || ls.Name.Trim() == "VHL" || ls.Name.Trim() == "MHL"
                            || ls.Name.Trim() == "NHL"))
                            listOfPlayear = PlayersObj.getAllPlayer(ls, listOfPlayear);
                    }

                    e.Result = listOfPlayear;
                };

                //Поток завершил свою работу
                bwOLD.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e)
                {
                    if (e.Error != null)
                    {
                    }
                    else if (e.Cancelled)
                    {
                    }
                    else
                    {
                        listOfPlayear = (List<Player>)e.Result;

                        countOfThreads--;
                        if (countOfThreads == 0)
                        {
                            lblStatus.Text = listOfPlayear.Count.ToString();

                            string path = Application.StartupPath + @"\Output\Roster.xlsx";
                            File.Delete(path);

                            string pathCSV = Application.StartupPath + @"\Output\Roster.csv";
                            File.Delete(pathCSV);
                            var csv = new StringBuilder();
                            csv.AppendLine("Name;Number;Height;Weight;BirthDate;Shoots;Position;Team;League");

                            try
                            {
                                XLWorkbook xlTemplate = new XLWorkbook();
                                var xlWorkSheet = xlTemplate.AddWorksheet("Характеристики игроков");

                                xlWorkSheet.Cell("A1").Value = "Name";
                                xlWorkSheet.Cell("B1").Value = "Number";
                                xlWorkSheet.Cell("C1").Value = "Height";
                                xlWorkSheet.Cell("D1").Value = "Weight";
                                xlWorkSheet.Cell("E1").Value = "BirthDate";
                                xlWorkSheet.Cell("G1").Value = "Shoots";
                                xlWorkSheet.Cell("H1").Value = "Position";
                                xlWorkSheet.Cell("I1").Value = "Team";
                                xlWorkSheet.Cell("J1").Value = "League";
                                xlWorkSheet.Cell("K1").Value = "TeamLogo";
                                xlWorkSheet.Cell("L1").Value = "TeamLogo";

                                int numCell = 2;
                                foreach (Player player in listOfPlayear)
                                {
                                    xlWorkSheet.Cell("A" + numCell).Value = player.Name.Trim();
                                    xlWorkSheet.Cell("B" + numCell).Value = player.Number.Trim();
                                    xlWorkSheet.Cell("C" + numCell).Value = player.Height.Trim();
                                    xlWorkSheet.Cell("D" + numCell).Value = player.Weight.Trim();
                                    xlWorkSheet.Cell("E" + numCell).Value = player.BirthDate.Trim();
                                    xlWorkSheet.Cell("G" + numCell).Value = player.Shoots.Trim();
                                    xlWorkSheet.Cell("H" + numCell).Value = player.Position.Trim();
                                    xlWorkSheet.Cell("I" + numCell).Value = player.team.name.Trim();
                                    xlWorkSheet.Cell("J" + numCell).Value = player.team.league.Name.Trim();
                                    xlWorkSheet.Cell("L" + numCell).Value = player.Photo;
                                    numCell++;

                                    csv.AppendLine(player.ToString());
                                    File.WriteAllText(pathCSV, csv.ToString(), Encoding.UTF8);
                                }

                                xlTemplate.SaveAs(path);
                            }
                            catch (Exception exp)
                            {

                                MessageBox.Show(string.Format("Ошибка {0}!", exp.ToString()));
                            }

                            log_view.Items.Add("Все лиги загружены успешно...");
                            log_view.Items.Add("Создан файл Roster.xlsx...");
                            log_view.Items.Add("Создан файл Roster.csv...");
                        }
                    }
                };
                var parameters = new BackgroundWorkerParams();
                parameters.param1 = l;

                bwOLD.RunWorkerAsync(parameters);
            }
        }

        public class Players
        {
            private Label lblStatus;
            private ListBox log;

            public Players(Label _status, ListBox _log)
            {
                lblStatus = _status;
                log = _log;
            }

            public List<Player> getAllPlayer(League league, List<Player> _listOfPlayers)
            {
                string mainPage = "http://www.eliteprospects.com/";
                string url = "http://www.eliteprospects.com/" + league.href;

                WebClient wb = new WebClient();
                wb.Encoding = Encoding.GetEncoding("ISO-8859-1");
                HtmlAgilityPack.HtmlDocument hDoc = new HtmlAgilityPack.HtmlDocument();
                try
                {
                    hDoc.LoadHtml(wb.DownloadString(url.Trim()));
                }
                catch (Exception e)
                {
                    return _listOfPlayers;
                }

                var teamsNodes = hDoc.DocumentNode.SelectNodes("//table[@class = 'tableborder']/tr/td/a[contains(@href, 'team.php?team=')]");
                if (teamsNodes == null) return _listOfPlayers;

                List<Team> teams = new List<Team>();
                foreach (var t in teamsNodes)
                {
                    if (teams.Count > 0)
                    {
                        try
                        {
                            var isDuplicates = teams.First(c => c.name == t.InnerText);
                            if ((isDuplicates as Team) != null)
                                break;
                        }
                        catch (Exception e)
                        {
                        }
                    };

                    teams.Add(new Team
                    {
                        name = t.InnerText.Trim(),
                        href = t.Attributes["href"].Value,
                        league = league
                    });
                }

                foreach (var t in teams)
                {
                    string urlTeam = "http://www.eliteprospects.com/" + t.href;

                    WebClient wbAttrib = new WebClient();
                    wbAttrib.Encoding = Encoding.GetEncoding("ISO-8859-1");
                    HtmlAgilityPack.HtmlDocument hDocAttrib = new HtmlAgilityPack.HtmlDocument();
                    try
                    {
                        hDocAttrib.LoadHtml(wbAttrib.DownloadString(urlTeam));
                    }
                    catch (Exception e)
                    {
                        return _listOfPlayers;
                    }

                    //Загружаем лого команды
                    string pathTeams = Application.StartupPath + @"\Output\TeamsLogo";
                    var img = hDocAttrib.DocumentNode.SelectSingleNode("//img[contains(@src, 'http://files.eliteprospects.com/layout/logos/')]");
                    string urlLogo = (img == null ? "" : img.Attributes["src"].Value);
                    if (urlLogo != "")
                    wbAttrib.DownloadFile(urlLogo, pathTeams + @"\" + t.name+ ".png");

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
                        hDocPlayer.LoadHtml(wbAttrib.DownloadString(playerHref));

                        string pathPlayers = Application.StartupPath + @"\Output\PlayersLogo";
                        var imgPhoto = hDocPlayer.DocumentNode.SelectSingleNode("//img[contains(@src, 'http://files.eliteprospects.com/layout/players/')]");
                        string urlPhoto = (imgPhoto == null ? "" : imgPhoto.Attributes["src"].Value);
                        if (urlPhoto != "")
                            wbAttrib.DownloadFile(urlPhoto, pathPlayers + @"\" + pName + ".png");

                        _listOfPlayers.Add(new Player()
                        {
                            Name = pName,
                            Number = pNumber,
                            Age = age,
                            BirthDate = pBirthDate,
                            Height = pHeight,
                            Weight = pWeight,
                            Position = pPosition,
                            Shoots = pShoots,
                            team = t,
                            TeamLogo = urlLogo,
                            Photo = urlPhoto
                        });

                        try
                        {
                            string status = (Convert.ToInt16(lblStatus.Text) + 1).ToString();
                            if (lblStatus.InvokeRequired) lblStatus.Invoke(new Action<string>((s) => lblStatus.Text = s), status);
                            else lblStatus.Text = status;

                            string logStr = string.Format("{0}->{1}->{2}", league.Name.Trim(), t.name.Trim(), pName.Trim());
                            if (log.InvokeRequired) log.Invoke(new Action<string>((s) => log.Items.Add(s)), logStr);
                            else log.Items.Add(logStr);
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }

                return _listOfPlayers;
            }

            private void ChangeStatus(string status)
            {
                if (lblStatus.InvokeRequired)
                {
                    lblStatus.Invoke((MethodInvoker)delegate
                    {
                        ChangeStatus(status);
                    });
                }
                else
                {
                    lblStatus.Text = status;
                }
            }

            private void addInLog(string message)
            {
                if (log.InvokeRequired)
                {
                    log.Invoke((MethodInvoker)delegate
                    {
                        addInLog(message);
                    });
                }
                else
                {
                    log.Items.Add(message);
                }
            }

            public void ExportToXLSX(List<Player> listOfPlayers)
            {
            }
        }


        public class League
        {
            public string Part { get; set; }
            public string Name { get; set; }
            public string href { get; set; }
            public int group { get; set; }
        }

        public class Team
        {
            public string name { get; set; }
            public League league { get; set; }
            public string href { get; set; }
        }

        public class Player
        {
            //Ссылка
            public string href { get; set; }
            //Общие сведения
            public string Name { get; set; }
            public string BirthDate { get; set; }
            public string Age { get; set; }
            public string Height { get; set; }
            public string Weight { get; set; }
            //Информация о команде
            public string Team { get; set; }
            public string Number { get; set; }
            //Игровые характеристики
            public string Position { get; set; }
            public string Shoots { get; set; }
            public string Nation { get; set; }
            public Team team { get; set; }
            public string TeamLogo { get; set; }
            public string Photo { get; set; }

            public override string ToString()
            {
                return string.Join(";", Name, Number, Height, Weight, BirthDate, Shoots, Position, team.name, team.league.Name);
            }
        }
    }
}




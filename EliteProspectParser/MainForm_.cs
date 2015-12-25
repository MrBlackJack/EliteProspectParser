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
using System.Xml;
using Npgsql;

namespace EliteProspectParser
{
    public partial class MainForm_ : Form
    {
        private List<League> leagues = new List<League>();
        private List<Team> teams = new List<Team>();
        private List<string> parts = new List<string>();

        private DateTime start;

        private AppSettings _appSetttings = new AppSettings();

        public MainForm_()
        {
            //Инициализация компонетов
            InitializeComponent();

            //Загружаем сохранненые настройки
            _appSetttings.Load();

            //Показываем какие лиги были выбраны
            lblLeagues.Text = string.Format("Загружаемые лиги ({0})", (_appSetttings.CheckLeagues.Count > 0 ? 
                _appSetttings.CheckLeagues.Aggregate((i, j) => i + ";" + j) : ""));

            //Запускаем основной метод
            Loading();
        }

        public void Loading()
        {
            string url = @"http://www.eliteprospects.com/league_central.php#na";
            WebClient wb = new WebClient();
            wb.Encoding = Encoding.GetEncoding("ISO-8859-1");

            //Получаем страницу со списком лиг
            HtmlAgilityPack.HtmlDocument hDoc = new HtmlAgilityPack.HtmlDocument();
            hDoc.LoadHtml(wb.DownloadString(url.Trim()));

            //Выбираем все лиги
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
                            Name = item.InnerText.Trim(),
                            Part = parts[i],
                            isChecked = _appSetttings.CheckLeagues.Contains(item.InnerText.Trim()) ? true : false
                        });
                    }
                    i++;
                }
            }

            //Сортируем список
            leagues.Sort(delegate(League l1, League l2) { return l1.Name.CompareTo(l2.Name); });

            ListLeague.BeginUpdate();
            (ListLeague as ListBox).DataSource = leagues;
            (ListLeague as ListBox).DisplayMember = "Name";
            (ListLeague as ListBox).ValueMember = "Name";
            ListLeague.EndUpdate();

            for (int j = 0; j < ListLeague.Items.Count - 1; j++)
			{
                if ((ListLeague.Items[j] as League).isChecked)
                    ListLeague.SetItemChecked(j, true);
			}
        }


        struct BackgroundWorkerParams
        {
            public int grpNmr;
        }

        public void StartParser()
        {
            List<League> checkLeagues = new List<League>();
            foreach (League l in leagues)
                if (_appSetttings.CheckLeagues.Contains(l.Name.Trim()))
                    checkLeagues.Add(l);

            int group = 0;
            int cntPart = (checkLeagues.Count / 10 == 0 ? 1 : checkLeagues.Count / 10);
            int cntOfGroups = 0;

            for (int j = 0; j < checkLeagues.Count; j++)
            {
                group = /*j / cntPart + 1*/1;
                checkLeagues[j].group = j / cntPart + 1;
                cntOfGroups++;
            }

            lblStatus.Text = "0";
            int countOfThreads = group;

            List<Player> listOfPlayear = new List<Player>();
            Players PlayersObj = new Players(lblStatus, log_view, lblTValue, start);
            

            for (int l = 1; l <= group; l++)
            {
                BackgroundWorker bw = new BackgroundWorker();

                //Задаем тело потока
                bw.DoWork += (object sender, DoWorkEventArgs e) =>
                {
                    var bParams = (BackgroundWorkerParams)e.Argument;

                    foreach (var ls in checkLeagues)
                    {
                        if (ls.group == bParams.grpNmr && _appSetttings.CheckLeagues.Contains(ls.Name.Trim()))
                        {
                            PlayersObj.teams = teams;
                            listOfPlayear = PlayersObj.getAllPlayer(ls, listOfPlayear);
                        }
                    }

                    e.Result = listOfPlayear;
                };

                //Поток завершил свою работу
                bw.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
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

                            //Записываем файл статистики
                            string pathStat = _appSetttings.paths.Output + @"\stat.txt";
                            File.Delete(pathStat);
                            var stat = new StringBuilder();
                            stat.AppendLine("Всего игроков загружено :" + lblStatus.Text);
                            stat.AppendLine("Всего времени прошло :" + lblTValue.Text);
                            File.WriteAllText(pathStat, stat.ToString(), Encoding.UTF8);

                            //Инициализация класса подключения к базе
                            NpgsqlConnection NpgConn = new Npgsql.NpgsqlConnection(_appSetttings.ConnectionString);
                            bool ConnectionResult = true;

                            try
                            {
                                NpgConn.Open();
                            }
                            catch(Exception)
                            {
                                ConnectionResult = false;
                                log_view.Items.Add("Не удалось подключиться к базе!");
                            }

                            if (ConnectionResult)
                            {
                                //Обновление записей в таблице League
                                foreach (League league in checkLeagues)
                                {
                                    string updateSQL = string.Format("select leagues_ins('{0}');", league.Name.Trim());

                                    NpgsqlCommand SqlCommand = new NpgsqlCommand(updateSQL, NpgConn);
                                    int result = SqlCommand.ExecuteNonQuery();
                                }

                                //Обновление записей в таблице Teams
                                foreach (Team team in PlayersObj.teams)
                                {
                                    string updateSQL = string.Format("select Teams_ins('{0}', '{1}');", team.name.Trim(), team.league.Name.Trim());
                                    NpgsqlCommand SqlCommand = new NpgsqlCommand(updateSQL, NpgConn);
                                    try
                                    {
                                        int result = SqlCommand.ExecuteNonQuery();
                                    }
                                    catch (Exception)
                                    {
                                        log_view.Items.Add("Ошибка: не удалось обновить команду " + team.name.Trim());
                                    }
                                }
                            }

                            //Путь к excel файлу
                            File.Delete(_appSetttings.paths.Output + @"\Roster.xlsx");

                            //Путь к csv файлу
                            File.Delete(_appSetttings.paths.Output + @"\Roster.csv");

                            //Шапка csv файла
                            var csv = new StringBuilder();
                            csv.AppendLine("Name;Number;Height;Weight;BirthDate;Shoots;Position;Team;League;TeamLogo;Photo");


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
                            xlWorkSheet.Cell("L1").Value = "Photo";
                            xlWorkSheet.Cell("M1").Value = "EliteID";

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
                                xlWorkSheet.Cell("K" + numCell).Value = player.TeamLogo.Trim();
                                xlWorkSheet.Cell("L" + numCell).Value = player.Photo;
                                xlWorkSheet.Cell("M" + numCell).Value = player.EliteID;
                                numCell++;

                                csv.AppendLine(player.ToString());
                                File.WriteAllText(_appSetttings.paths.Output + @"\Roster.csv", csv.ToString(), Encoding.UTF8);

                                if (ConnectionResult)
                                {
                                    //Обновление записей в таблице Players
                                    string updateSQL = string.Format("select players_ins(cast('{0}' as varchar(255)), "+
                                                                     "                   cast('{1}' as varchar(255))," +
                                                                     "                   cast('{2}' as varchar(255)), " +
                                                                     "                   cast('{3}' as varchar(255)),   " +
                                                                     "                   cast('{4}' as varchar(255)),   " +
                                                                     "                   cast({5}   as integer),     " +
                                                                     "                   cast('{6}' as varchar(255)))", 
                                                                     player.Name.Trim(), player.Shoots, player.Height, player.Weight, 
                                                                     player.BirthDate, player.EliteID, player.Photo);

                                    NpgsqlCommand SqlCommand = new NpgsqlCommand(updateSQL, NpgConn);
                                    try
                                    {
                                        int result = SqlCommand.ExecuteNonQuery();
                                    }
                                    catch (Exception)
                                    {
                                        log_view.Items.Add("Ошибка: не удалось обновить игрока " + player.Name);
                                    }
                                }

                                //Обновление составов
                                if (ConnectionResult)
                                {
                                    //Обновление записей в таблице Players
                                    string updateSQL = string.Format("select rosters_ins({0}, '{1}', '{2}', '{3}'); ", 
                                        player.EliteID, player.team.name.Trim(), player.Position, player.Number
                                        );

                                    NpgsqlCommand SqlCommand = new NpgsqlCommand(updateSQL, NpgConn);
                                    try
                                    {
                                        int result = SqlCommand.ExecuteNonQuery();
                                    }
                                    catch (Exception)
                                    {
                                        log_view.Items.Add("Ошибка: не удалось обновить игрока " + player.Name);
                                    }
                                }
                            }

                            xlTemplate.SaveAs(_appSetttings.paths.Output + @"\Roster.xlsx");


                            log_view.Items.Add("Создан файл Roster.xlsx");
                            log_view.Items.Add("Создан файл Roster.csv");

                            btn_startPars.Enabled = true;
                            ListLeague.Enabled = true;

                            //Закрываем соединение
                            if(ConnectionResult)
                                NpgConn.Close();
                        }

                    }

                    int visibleItems = log_view.ClientSize.Height / log_view.ItemHeight;
                    log_view.TopIndex = Math.Max(log_view.Items.Count - visibleItems + 1, 0);
                };
                var parameters = new BackgroundWorkerParams();
                parameters.grpNmr = l;

                bw.RunWorkerAsync(parameters);
            }
        }

        private void btn_startPars_Click(object sender, EventArgs e)
        {
            start = DateTime.Now;
            log_view.Items.Clear();

            StartParser();
            btn_startPars.Enabled = false;
            ListLeague.Enabled = false;
        }

        private void Menu_BDSettings_Click(object sender, EventArgs e)
        {
            BDSetttingsForm bsSett = new BDSetttingsForm(_appSetttings);
            bsSett.Show();
        }

        private void ListLeague_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            List<string> checkedItems = new List<string>();
            foreach (var item in ListLeague.CheckedItems)
                checkedItems.Add((item as League).Name.ToString());

            if (e.NewValue == CheckState.Checked)
                checkedItems.Add((ListLeague.Items[e.Index] as League).Name.ToString());
            else
                checkedItems.Remove((ListLeague.Items[e.Index] as League).Name.ToString());

            _appSetttings.CheckLeagues = checkedItems;
            _appSetttings.Save();

            lblLeagues.Text = string.Format("Загружаемые лиги ({0})", _appSetttings.CheckLeagues.Aggregate((i, j) => i + ";" + j));
        }
    }
}




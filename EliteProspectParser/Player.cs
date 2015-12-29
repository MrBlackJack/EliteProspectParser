using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteProspectParser
{
    public class Player
    {
        //Ссылка
        public string href { get; set; }
        //Общие сведения
        public string Name { get; set; }
        public string NameRus { get; set; }
        public string BirthDate { get; set; }
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
        public string Photo { get; set; }
        public string EliteID { get; set; }

        public override string ToString()
        {
            return string.Join(";", Name, Number, Height, Weight, BirthDate, Shoots, Position, team.name, team.league.Name, Photo);
        }
    }
}

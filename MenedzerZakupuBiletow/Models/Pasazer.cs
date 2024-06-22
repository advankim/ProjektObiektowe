﻿using System.ComponentModel.DataAnnotations;

namespace MenedzerZakupuBiletow.Models
{
    public class Pasazer
    {
        [Key]
        public int Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public int Wiek { get; set; }
        public string Plec { get; set; }
        public string PESEL { get; set; }
        public string Bagaz { get; set; }
    }
}

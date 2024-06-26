﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektObiektowe.Models
{
    [Table("Rezerwacje")]
    public class Rezerwacja
    {
        public int Id { get; set; }
        public int Id_Pasazer { get; set; }
        public int Id_Bilet { get; set; }
        public string Data { get; set; }
        public int NumerMiejsca { get; set; } // Nowe pole do przechowywania numeru miejsca

        public Pasazer Pasazer { get; set; }
        public Bilet Bilet { get; set; }
    }
}

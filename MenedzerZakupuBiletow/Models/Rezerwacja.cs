using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenedzerZakupuBiletow.Models
{
    public class Rezerwacja
    {
        public int Id { get; set; }
        public int Id_Pasazer { get; set; }
        public int Id_Bilet { get; set; }
        public DateTime Data { get; set; }

        public Pasazer Pasazer { get; set; }
        public Bilet Bilet { get; set; }
    }
}
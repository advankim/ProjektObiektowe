using MenedzerZakupuBiletow.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenedzerZakupuBiletow.Models
{
    [Table("Rezerwacje")]
    public class Rezerwacja
    {
        public int Id { get; set; }
        public int Id_Pasazer { get; set; }
        public int Id_Bilet { get; set; }
        public string Data { get; set; }
        public string Status { get; set; }
        public string Cena { get; set; }
        public int Klasa { get; set; }
        public string Bagaz { get; set; }
        public string Numer_Miejsca { get; set; }
        public Pasazer Pasazer { get; set; }
        public Bilet Bilet { get; set; }
        public Lot Lot => Bilet?.Lot;
        public Lotnisko LotniskoWylot => Lot?.LotniskoWylot;
        public Lotnisko LotniskoPrzylot => Lot?.LotniskoPrzylot;
    }
}

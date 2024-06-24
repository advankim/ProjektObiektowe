namespace MenedzerZakupuBiletow.Models
{
    public class Bilet
    {
        public int Id { get; set; }
        public string Numer { get; set; }
        public int Id_Lot { get; set; }
        public decimal Cena_Klasa_1 { get; set; }
        public decimal Cena_Klasa_2 { get; set; }
        public int Dostepnych_Klasa_1 { get; set; }
        public int Dostepnych_Klasa_2 { get; set; }
        public Lot Lot { get; set; }
    }
}
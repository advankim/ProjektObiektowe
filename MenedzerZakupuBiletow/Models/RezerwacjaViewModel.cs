namespace MenedzerZakupuBiletow.Models
{
    public class RezerwacjaViewModel
    {
        public Bilet Bilet { get; set; }
        public int Klasa { get; set; }
        public string Bagaz { get; set; }
        public Pasazer Pasazer { get; set; }
    }
}
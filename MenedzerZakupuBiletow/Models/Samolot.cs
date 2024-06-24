namespace MenedzerZakupuBiletow.Models
{
    public class Samolot
    {
        public int Id { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string Numer { get; set; }
        public int Pojemnosc { get; set; }
        public int Rzedy_Klasa_1 { get; set; }
        public int Rzedy_Klasa_2 { get; set; }
        public string Linia { get; set; }
    }
}
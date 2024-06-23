namespace MenedzerZakupuBiletow.Models
{
	public class Lot
	{
		public int Id { get; set; }
		public string Numer { get; set; }
		public int Id_Samolot { get; set; }
		public int Id_Lotnisko_Wylot { get; set; }
		public int Id_Lotnisko_Przylot { get; set; }
		public DateTime Czas_Wylot { get; set; }
		public DateTime Czas_Przylot { get; set; }

		public Samolot Samolot { get; set; }
		public Lotnisko LotniskoWylot { get; set; }
		public Lotnisko LotniskoPrzylot { get; set; }
	}

}

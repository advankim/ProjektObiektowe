using System.ComponentModel.DataAnnotations;

namespace MenedzerZakupuBiletow.Models
{
    public class Rezerwacja
    {
        [Key]
        public int Id { get; set; }
        public int Id_Pasazer { get; set; }
        public int Id_Bilet { get; set; }
        public string Data { get; set; }
    }
}

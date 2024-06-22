using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektObiektowe.Models
{
    public class Lot
    {
        public int Id { get; set; }
       
        [Required(ErrorMessage = "Numer lotu jest wymagany.")]
        public string Numer { get; set; }

        [Required(ErrorMessage = "Czas wylotu jest wymagany.")]
        public DateTime Czas_Wylot { get; set; }

        [Required(ErrorMessage = "Czas przylotu jest wymagany.")]
        public DateTime Czas_Przylot { get; set; }

        [Required(ErrorMessage = "Lotnisko wylotu jest wymagane.")]
        public int Id_Lotnisko_Wylot { get; set; }

        [Required(ErrorMessage = "Lotnisko przylotu jest wymagane.")]
        public int Id_Lotnisko_Przylot { get; set; }

        [Required(ErrorMessage = "Samolot jest wymagany.")]
        public int Id_Samolot { get; set; }

        [ForeignKey("Id_Samolot")]
        public virtual Samolot Samolot { get; set; }

        [ForeignKey("Id_Lotnisko_Wylot")]
        public virtual Lotnisko Lotnisko_Wylot { get; set; }

        [ForeignKey("Id_Lotnisko_Przylot")]
        public virtual Lotnisko Lotnisko_Przylot { get; set; }
        public ICollection<Bilet> Bilety { get; set; }
    }


}


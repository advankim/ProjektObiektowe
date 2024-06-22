using System.ComponentModel.DataAnnotations;

namespace ProjektObiektowe.Models
{
    public class Samolot
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Numer lotu jest wymagany.")]
        public string Marka { get; set; }
        [Required(ErrorMessage = "Numer lotu jest wymagany.")]
        public string Model { get; set; }
        [Required(ErrorMessage = "Numer lotu jest wymagany.")]
        public string Numer { get; set; }
        [Required(ErrorMessage = "Numer lotu jest wymagany.")]
        public int Pojemnosc { get; set; }
        [Required(ErrorMessage = "Numer lotu jest wymagany.")]
        public int Pojemnosc_Klasa_1 { get; set; }
        public int Pojemnosc_Klasa_2 {  get; set; }
        public string Linia { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektObiektowe.Models
{
    public class Bilet
    {
        public int Id { get; set; }

        [Required]
        public string Numer {  get; set; }

        [Required(ErrorMessage = "Numer lotu jest wymagany.")]
        public int Id_Lot { get; set; }

        [Required(ErrorMessage = "Cena klasy 1 jest wymagana.")]
        public decimal Cena_Klasa_1 { get; set; }

        [Required(ErrorMessage = "Cena klasy 2 jest wymagana.")]
        public decimal Cena_Klasa_2 { get; set; }

        public int Dostepnych_Klasa_1 { get; set; }
        public int Dostepnych_Klasa_2 { get; set; }

        [ForeignKey("Id_Lot")]
        public virtual Lot Lot { get; set; }
    }
}

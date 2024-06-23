﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MenedzerZakupuBiletow.Models
{
    public class Pasazer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Imie jest wymagane.")]
        public string Imie { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane.")]
        public string Nazwisko { get; set; }

        [Range(1, 120, ErrorMessage = "Wiek musi być między 1 a 120.")]
        public int Wiek { get; set; }

        [Required(ErrorMessage = "Plec jest wymagana.")]
        public string Plec { get; set; }

        [Required(ErrorMessage = "PESEL jest wymagany.")]
        [StringLength(11, ErrorMessage = "PESEL musi mieć 11 znaków.")]
        public string PESEL { get; set; }

        public ICollection<Rezerwacja> Rezerwacje { get; set; }
    }
}
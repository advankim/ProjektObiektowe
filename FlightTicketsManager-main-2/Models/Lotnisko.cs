using System.ComponentModel.DataAnnotations;

namespace ProjektObiektowe.Models
{
    public class Lotnisko
    {
        [Key]
        public int Id {  get; set; }
        public string Kod {  get; set; }
        public string Nazwa {  get; set; }
        public string Miasto { get; set; }
        public string Kraj { get; set; }
    }
}

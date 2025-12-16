using System.ComponentModel.DataAnnotations;

namespace Asandului_Oana_Maria_Lab2.Models
{
    public class Author
    {
        public int ID { get; set; }

        [Required]
        public string FirstName { get; set; } = default!;

        [Required]
        public string LastName { get; set; } = default!;

        // Navigation
        public ICollection<Book>? Books { get; set; }
    }
}

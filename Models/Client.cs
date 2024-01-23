using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Client
    {
        
        public int Id { get; set; }
        [MaxLength(150)]
        public string Name { get; set; } = "";

        [MaxLength(150)]
        public string Email { get; set; } = "";

        [MaxLength(100)]
        public string Phone { get; set; } = "";
        [MaxLength(100)]
        public string Address { get; set; } = "";

        public string ImageFileName { get; set; } = "";

        public DateTime Created_at { get; set; }
    }

  
}

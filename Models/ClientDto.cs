using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class ClientDto
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = "";

        [Required, MaxLength(150)]
        public string Email { get; set; } = "";

        [Required, MaxLength(100)]
        public string Phone { get; set; } = "";
        [Required, MaxLength(100)]
        public string Address { get; set; } = "";

        public IFormFile? ImageFile { get; set; }
    }
}

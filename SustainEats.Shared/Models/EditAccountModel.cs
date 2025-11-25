using System.ComponentModel.DataAnnotations;

namespace SustainEats.Shared.Models
{
    public class EditAccountModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
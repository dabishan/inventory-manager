using System.ComponentModel.DataAnnotations;

namespace InventoryManager.ViewModel
{
    public class LoginData : ViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
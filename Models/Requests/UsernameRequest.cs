using System.ComponentModel.DataAnnotations;

namespace Models.Requests
{
    public class UsernameRequest
    {
        [Required]
        public string Username { get; set; }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Models.Models;

public class UserModel
{
    [DisplayName("User ID")]
    [Required]
    public int UserId { get; set; }

    [DisplayName("User Name")]
    [Required(AllowEmptyStrings = false)]
    public string? Name { get; set; }
}
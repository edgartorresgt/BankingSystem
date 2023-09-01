using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Models.Models;

public class UserAccountModel
{
    [DisplayName("Account ID")]
    [Required]
    public int AccountId { get; set; }

    [DisplayName("Amount")]
    [Required]
    [Range(0.01, 10000)]
    public decimal Amount { get; set; }
}
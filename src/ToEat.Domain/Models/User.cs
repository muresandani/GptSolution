using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ToEat.Domain.Models;

public class User : IdentityUser
{
    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }

    public Inventory? Inventory { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EnglishWordBot.Framework.Database.Entities.Users;
using EnglishWordBot.Framework.Database.Entities.Words;

namespace EnglishWordBot.Framework.Database.Entities.UserWordMappings;

public class UserWordMapping : BaseEntity
{
    [Required]
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    
    public User User { get; set; }

    [Required] 
    [ForeignKey("Word")]
    public Guid WordId { get; set; }

    public Word Word { get; set; }

    public bool Disabled { get; set; }

    public bool Learned { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace EnglishWordBot.Framework.Database.Entities;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
}
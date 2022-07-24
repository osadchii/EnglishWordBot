using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using EnglishWordBot.Framework.Common;
using Microsoft.EntityFrameworkCore;

namespace EnglishWordBot.Framework.Database.Entities.Words;

public class Word : BaseEntity
{
    [Required]
    [MaxLength(Limits.WordValueLength)]
    public string Value { get; set; }

    public Collection<Translation> Translations { get; set; }

    [Owned]
    public class Translation : BaseEntity
    {
        [Required]
        [MaxLength(Limits.WordTranslationLength)]
        public string Value { get; set; }
    }
}
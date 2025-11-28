using System.ComponentModel.DataAnnotations;

namespace kitaab.Model;

public class Book
{
    [Key] public Guid bookId { get; set; } = Guid.NewGuid();
    [Required] public string title { get; set; }
    [Required] public string description { get; set; }
    [Required] public int score { get; set; }
}
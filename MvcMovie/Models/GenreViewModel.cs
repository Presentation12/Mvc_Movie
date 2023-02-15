using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models
{
    public class GenreViewModel
    {
        public int Id { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$"), Required, StringLength(30)]
        public string? Name { get; set; }
    }
}

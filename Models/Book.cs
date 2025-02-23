using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        [Required]
        public int ViewsCount { get; set; }
        public bool IsDeleted { get; set; }
    }

}

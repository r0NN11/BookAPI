namespace BookAPI.Models
{
    public class UpdateBookDto
    {
        public string Title { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
    }
}

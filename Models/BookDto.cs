﻿namespace BookAPI.Models
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public int ViewsCount { get; set; }
        public double PopularityScore { get; set; }
    }
}

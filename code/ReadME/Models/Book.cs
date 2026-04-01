namespace ReadMe_perso.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string CoverImagePath { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<int> TagIds { get; set; } = new();
        public string Content { get; set; } = string.Empty;
    }
}

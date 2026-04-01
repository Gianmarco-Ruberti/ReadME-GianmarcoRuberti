namespace ReadMe.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime DateAdded { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Chapter> Chapters { get; set; }
        public string CoverImagePath { get; set; }
        public string FilePath { get; set; }
    }
}

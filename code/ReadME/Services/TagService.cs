using ReadMe_perso.Models;
using System.Text.Json;
using System.Diagnostics;

namespace ReadMe_perso.Services
{
    public class TagService
    {
        private List<Tag> _tags = new();
        private const string TagsFileName = "tags.json";

        public TagService()
        {
            LoadTags();
        }

        public List<Tag> GetAllTags()
        {
            return _tags;
        }

        public Tag GetTagById(int id)
        {
            return _tags.FirstOrDefault(t => t.Id == id) ?? new Tag();
        }

        public void AddTag(string name, string color = "#512BD4")
        {
            var newTag = new Tag
            {
                Id = _tags.Count > 0 ? _tags.Max(t => t.Id) + 1 : 1,
                Name = name,
                Color = color
            };
            _tags.Add(newTag);
            SaveTags();
        }

        public void DeleteTag(int id)
        {
            var tag = _tags.FirstOrDefault(t => t.Id == id);
            if (tag != null)
            {
                _tags.Remove(tag);
                SaveTags();
            }
        }

        public void UpdateTag(int id, string name, string color)
        {
            var tag = _tags.FirstOrDefault(t => t.Id == id);
            if (tag != null)
            {
                tag.Name = name;
                tag.Color = color;
                SaveTags();
            }
        }

        private async void SaveTags()
        {
            try
            {
                var appDataPath = FileSystem.AppDataDirectory;
                var filePath = Path.Combine(appDataPath, TagsFileName);
                var json = JsonSerializer.Serialize(_tags);
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving tags: {ex.Message}");
            }
        }

        private void LoadTags()
        {
            try
            {
                var appDataPath = FileSystem.AppDataDirectory;
                var filePath = Path.Combine(appDataPath, TagsFileName);

                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    var loadedTags = JsonSerializer.Deserialize<List<Tag>>(json);
                    _tags = loadedTags ?? new();
                }
                else
                {
                    // Create default tags
                    _tags = new()
                    {
                        new Tag { Id = 1, Name = "Fiction", Color = "#FF6B9D" },
                        new Tag { Id = 2, Name = "Science-Fiction", Color = "#4ECDC4" },
                        new Tag { Id = 3, Name = "Biographie", Color = "#95E1D3" },
                        new Tag { Id = 4, Name = "Thriller", Color = "#F38181" },
                        new Tag { Id = 5, Name = "Mystère", Color = "#AA96DA" }
                    };
                    SaveTags();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading tags: {ex.Message}");
                _tags = new();
            }
        }
    }
}

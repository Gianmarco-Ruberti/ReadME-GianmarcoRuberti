using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ReadMe.Models;

namespace ReadMe.Services
{
    public class TagService
    {
        private List<Tag> _tags = new();
        private readonly string _filePath;
        private int _nextId = 1;
        private readonly List<(string Name, string Color)> _defaultTags = new()
        {
            ("Fiction", "#FFB300"),
            ("Science-Fiction", "#1976D2"),
            ("Policier", "#388E3C"),
            ("Romance", "#D32F2F"),
            ("Fantasy", "#7B1FA2")
        };

        public TagService(string appDataDirectory)
        {
            _filePath = Path.Combine(appDataDirectory, "tags.json");
            LoadTags();
        }

        private void LoadTags()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _tags = JsonSerializer.Deserialize<List<Tag>>(json) ?? new();
                if (_tags.Count > 0)
                    _nextId = _tags.Max(t => t.Id) + 1;
            }
            else
            {
                foreach (var (name, color) in _defaultTags)
                {
                    _tags.Add(new Tag { Id = _nextId++, Name = name, Color = color });
                }
                SaveTags();
            }
        }

        private void SaveTags()
        {
            var json = JsonSerializer.Serialize(_tags);
            File.WriteAllText(_filePath, json);
        }

        public List<Tag> GetAllTags() => _tags;
        public Tag GetTagById(int id) => _tags.FirstOrDefault(t => t.Id == id);
        public void AddTag(Tag tag)
        {
            tag.Id = _nextId++;
            _tags.Add(tag);
            SaveTags();
        }
        public void UpdateTag(Tag tag)
        {
            var index = _tags.FindIndex(t => t.Id == tag.Id);
            if (index >= 0)
            {
                _tags[index] = tag;
                SaveTags();
            }
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
    }
}

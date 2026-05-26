using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ReadMe.Models;

namespace ReadMe.Services
{
    public class TagService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://10.0.2.2:7001/api/tags";

        public TagService()
        {
            _httpClient = new HttpClient();
        }


        public TagService(object fallbackArgument) : this()
        {

        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Tag>>(_baseUrl);
                return response ?? new List<Tag>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TagService GET Error]: {ex.Message}");
                return new List<Tag>();
            }
        }

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_baseUrl, tag);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TagService POST Error]: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteTagAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TagService DELETE Error]: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateTagAsync(Tag tag)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{tag.Id}", tag);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[TagService PUT Error]: {ex.Message}");
                return false;
            }
        }


        public List<Tag> GetAllTags()
        {
            return Task.Run(async () => await GetTagsAsync()).Result;
        }

        public Tag GetTagById(int id)
        {
            try
            {
                return Task.Run(async () => await _httpClient.GetFromJsonAsync<Tag>($"{_baseUrl}/{id}")).Result;
            }
            catch
            {
                return null;
            }
        }

        public void UpdateTag(Tag tag)
        {
            if (tag == null) return;
            Task.Run(async () => await UpdateTagAsync(tag)).Wait();
        }

        public void AddTag(Tag tag)
        {
            if (tag == null) return;
            Task.Run(async () => await CreateTagAsync(tag)).Wait();
        }

        public void DeleteTag(int id)
        {
            Task.Run(async () => await DeleteTagAsync(id)).Wait();
        }
    }
}
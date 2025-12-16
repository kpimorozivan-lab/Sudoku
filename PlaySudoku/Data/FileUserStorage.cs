using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using PlaySudoku.Models;

namespace PlaySudoku.Data
{
    public class FileUserStorage
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public FileUserStorage(string filePath = "users.json")
        {
            _filePath = filePath;
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }

        public List<User> LoadUsers()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<User>();
                }

                string json = File.ReadAllText(_filePath);
                
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<User>();
                }

                var users = JsonSerializer.Deserialize<List<User>>(json, _jsonOptions);
                return users ?? new List<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
                return new List<User>();
            }
        }

        public void SaveUsers(List<User> users)
        {
            try
            {
                string json = JsonSerializer.Serialize(users, _jsonOptions);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
        }
    }
}
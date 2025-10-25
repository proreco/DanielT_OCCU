using DanielT_OCCU.Data;
using System.Text.Json;
using Xunit;

namespace DanielT_OCCU.Tests
{
    public class DataSeederTests : IDisposable
    {
        private readonly string _statusFilePath;
        private readonly string _dataFilePath;

        public DataSeederTests()
        {
            string currentDirectory = AppContext.BaseDirectory;
            string projectDirectory = Path.GetFullPath(Path.Combine(currentDirectory, "..", "..", ".."));
            string dataStoragePath = Path.Combine(projectDirectory, "DataStorage");
            _statusFilePath = Path.Combine(dataStoragePath, "statusData.json");
            _dataFilePath = Path.Combine(dataStoragePath, "userData.json");
        }

        [Fact]
        public async Task SeedTestData()
        {
            // Arrange
            var dataStorage = new DataStorage();
            var statusStorage = new StatusStorage();

            // Act - Seed character data
            var characters = new List<DataObject>
            {
                new DataObject 
                { 
                    Name = "Gandalf", 
                    CharacterClass = "Wizard",
                    FavoredWeapon = "Staff",
                    Catchphrase = "You shall not pass!",
                    UpdatedAt = DateTime.Now
                },
                new DataObject 
                { 
                    Name = "Aragorn", 
                    CharacterClass = "Ranger",
                    FavoredWeapon = "Sword",
                    Catchphrase = "For Frodo",
                    UpdatedAt = DateTime.Now
                }
            };

            var statuses = new List<StatusObject>
            {
                new StatusObject 
                { 
                    Status = StatusType.Pass,
                    Message = "Character creation successful"
                },
                new StatusObject 
                { 
                    Status = StatusType.Warn,
                    Message = "Low health warning"
                }
            };

            await dataStorage.SaveData(characters);
            await statusStorage.SaveStatus(statuses);

            // Assert
            Assert.True(File.Exists(_dataFilePath));
            Assert.True(File.Exists(_statusFilePath));

            var loadedCharacters = await dataStorage.LoadData();
            var loadedStatuses = await statusStorage.LoadStatuses();

            Assert.Equal(2, loadedCharacters.Count);
            Assert.Equal(2, loadedStatuses.Count);
            Assert.Contains(loadedCharacters, c => c.Name == "Gandalf");
            Assert.Contains(loadedStatuses, s => s.Message == "Low health warning");
        }

        public void Dispose()
        {
            // Cleanup test files after running
            // if (File.Exists(_statusFilePath))
            //     File.Delete(_statusFilePath);
            // if (File.Exists(_dataFilePath))
            //     File.Delete(_dataFilePath);
        }
    }
}
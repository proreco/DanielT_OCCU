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
                    name = "Gandalf", 
                    characterClass = "Wizard",
                    favoredWeapon = "Staff",
                    catchphrase = "You shall not pass!",
                    updatedAt = DateTime.Now
                },
                new DataObject 
                { 
                    name = "Aragorn", 
                    characterClass = "Ranger",
                    favoredWeapon = "Sword",
                    catchphrase = "For Frodo",
                    updatedAt = DateTime.Now
                }
            };

            var statuses = new List<StatusObject>
            {
                new StatusObject 
                { 
                    status = StatusType.Pass,
                    message = "Character creation successful"
                },
                new StatusObject 
                { 
                    status = StatusType.Warn,
                    message = "Low health warning"
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
            Assert.Contains(loadedCharacters, c => c.name == "Gandalf");
            Assert.Contains(loadedStatuses, s => s.message == "Low health warning");
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
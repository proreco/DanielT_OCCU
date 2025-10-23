using System.Text.Json;
using System.IO;

namespace DanielT_OCCU.Data
{
    public class DataStorage
    {
        private readonly string _filePath;
        private List<DataObject> _dataList = new List<DataObject>();
        
        public DataStorage()
        {
            string currentDirectory = AppContext.BaseDirectory;
            string projectDirectory = Path.GetFullPath(Path.Combine(currentDirectory, "..", "..", ".."));
            string dataStoragePath = Path.Combine(projectDirectory, "DataStorage");
    
            if (!Directory.Exists(dataStoragePath))
            {
                Directory.CreateDirectory(dataStoragePath);
            }
            _filePath = Path.Combine(dataStoragePath, "userData.json");        }

        public async Task<bool> AddData(DataObject newData)
        {
            var dataList = await LoadData();
            var doesNameExist = dataList.FirstOrDefault(d => d.name == newData.name);
            
            if (doesNameExist != null)
                return false;

            dataList.Add(newData);
            await SaveData(dataList);
            return true;
        }
        
        public async Task<bool> DeleteData(string name)
        {
            var dataList = await LoadData();
            var dataToRemove = dataList.FirstOrDefault(d => d.name == name);

            if (dataToRemove == null)
                return false;

            dataList.Remove(dataToRemove);
            await SaveData(dataList);
            return true;
        }

        public async Task<bool> UpdateData(string name, DataObject updatedData)
        {
            var dataList = await LoadData();
            var existingData = dataList.FirstOrDefault(d => d.name == name);
            
            if (existingData == null)
                return false;

            existingData.name = updatedData.name;
            existingData.characterClass = updatedData.characterClass;
            existingData.favoredWeapon = updatedData.favoredWeapon;
            existingData.catchphrase = updatedData.catchphrase;
            existingData.updatedAt = DateTime.Now;

            await SaveData(dataList);
            return true;
        }

        public async Task SaveData(List<DataObject> dataList)
        {
            var jsonString = JsonSerializer.Serialize(dataList, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            await File.WriteAllTextAsync(_filePath, jsonString);
        }

        public async Task<List<DataObject>> LoadData()
        {
            if (!File.Exists(_filePath))
                return new List<DataObject>();

            var jsonString = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<DataObject>>(jsonString) ?? new List<DataObject>();
        }
    }
}
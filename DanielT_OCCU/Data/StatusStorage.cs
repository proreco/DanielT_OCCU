using System.Text.Json;

namespace DanielT_OCCU.Data
{
    public class StatusStorage
    {
        private readonly string _filePath;
        private List<StatusObject> _statusList = new List<StatusObject>();
        
        public StatusStorage()
        {
            string currentDirectory = AppContext.BaseDirectory;
            string projectDirectory = Path.GetFullPath(Path.Combine(currentDirectory, "..", "..", ".."));
            string dataStoragePath = Path.Combine(projectDirectory, "DataStorage");
    
    if (!Directory.Exists(dataStoragePath))
    {
        Directory.CreateDirectory(dataStoragePath);
    }
    
    _filePath = Path.Combine(dataStoragePath, "statusData.json");       
        }

        public async Task<bool> AddStatus(StatusObject updatedStatus)
        {
            var statuses = await LoadStatuses();
            var doesMessageExist = statuses.FirstOrDefault(d => d.message == updatedStatus.message);
            if (doesMessageExist != null)
                return false;
            statuses.Add(updatedStatus);
            await SaveStatus(statuses);
            return true;
        }

        public async Task<bool> DeleteStatus(string message)
        {
            var statuses = await LoadStatuses();
            var statusToRemove = statuses.FirstOrDefault(s => s.message == message);

            if (statusToRemove == null)
                return false;

            statuses.Remove(statusToRemove);
            await SaveStatus(statuses);
            return true;
        }

        public async Task<bool> UpdateStatus(string message, StatusObject updatedStatus)
        {
            var statuses = await LoadStatuses();
            var existingStatus = statuses.FirstOrDefault(s => s.message == message);
            
            if (existingStatus == null)
                return false;

            existingStatus.status = updatedStatus.status;
            existingStatus.message = updatedStatus.message;

            await SaveStatus(statuses);
            return true;
        }

        public async Task SaveStatus(List<StatusObject> statuses)
        {
            var jsonString = JsonSerializer.Serialize(statuses, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            await File.WriteAllTextAsync(_filePath, jsonString);
        }

        public async Task<List<StatusObject>> LoadStatuses()
        {
            if (!File.Exists(_filePath))
                return new List<StatusObject>();

            var jsonString = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<StatusObject>>(jsonString) ?? new List<StatusObject>();
        }
    }
}
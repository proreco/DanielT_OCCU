namespace DanielT_OCCU.Data
{
    public class StatusStorage : JsonFileRepository<StatusObject>
    {
        public StatusStorage()
            : base("statusData.json") { }

        protected override bool ItemExists(List<StatusObject> items, StatusObject item)
        {
            return items.Any(s => s.Message == item.Message);
        }

        protected override void UpdateItem(StatusObject existingItem, StatusObject updatedItem)
        {
            existingItem.Status = updatedItem.Status;
            existingItem.Message = updatedItem.Message;
        }

        // Convenience methods with specific signatures for backward compatibility
        public async Task<bool> AddStatus(StatusObject updatedStatus)
        {
            return await AddAsync(updatedStatus);
        }

        public async Task<bool> DeleteStatus(string message)
        {
            return await DeleteAsync(s => s.Message == message);
        }

        public async Task<bool> UpdateStatus(string message, StatusObject updatedStatus)
        {
            return await UpdateAsync(s => s.Message == message, updatedStatus);
        }

        public async Task<List<StatusObject>> LoadStatuses()
        {
            return await LoadAllAsync();
        }

        public async Task SaveStatus(List<StatusObject> statuses)
        {
            await SaveAllAsync(statuses);
        }
    }
}

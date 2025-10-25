// File: DanielT_OCCU/Data/DataStorage.cs
namespace DanielT_OCCU.Data
{
    public class DataStorage : JsonFileRepository<DataObject>
    {
        public DataStorage()
            : base("characterData.json") { }

        protected override bool ItemExists(List<DataObject> items, DataObject item)
        {
            return items.Any(d => d.Name == item.Name);
        }

        protected override void UpdateItem(DataObject existingItem, DataObject updatedItem)
        {
            existingItem.Name = updatedItem.Name;
            existingItem.CharacterClass = updatedItem.CharacterClass;
            existingItem.FavoredWeapon = updatedItem.FavoredWeapon;
            existingItem.Catchphrase = updatedItem.Catchphrase;
            existingItem.UpdatedAt = DateTime.Now;
        }

        // Convenience methods with specific signatures for backward compatibility
        public async Task<bool> AddData(DataObject newData)
        {
            return await AddAsync(newData);
        }

        public async Task<bool> DeleteData(string name)
        {
            return await DeleteAsync(d => d.Name == name);
        }

        public async Task<bool> UpdateData(string name, DataObject updatedData)
        {
            return await UpdateAsync(d => d.Name == name, updatedData);
        }

        public async Task<List<DataObject>> LoadData()
        {
            return await LoadAllAsync();
        }

        public async Task SaveData(List<DataObject> dataList)
        {
            await SaveAllAsync(dataList);
        }
    }
}

// File: DanielT_OCCU/Data/JsonFileRepository.cs
using System.Text.Json;

namespace DanielT_OCCU.Data
{
    public abstract class JsonFileRepository<T> : UniversalStorage<T>
        where T : class
    {
        protected readonly string FilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonFileRepository{T}"/> class.
        /// </summary>
        /// <param name="fileName"></param>
        protected JsonFileRepository(string fileName)
        {
            string currentDirectory = AppContext.BaseDirectory;
            string projectDirectory = Path.GetFullPath(
                Path.Combine(currentDirectory, "..", "..", "..")
            );
            string dataStoragePath = Path.Combine(projectDirectory, "DataStorage");

            if (!Directory.Exists(dataStoragePath))
            {
                Directory.CreateDirectory(dataStoragePath);
            }

            FilePath = Path.Combine(dataStoragePath, fileName);
        }

        /// <summary>
        /// Loads all items from the JSON file asynchronously.
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> LoadAllAsync()
        {
            if (!File.Exists(FilePath))
                return new List<T>();

            var jsonString = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<T>>(jsonString) ?? new List<T>();
        }

        /// <summary>
        /// Adds a new item to the JSON file asynchronously.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual async Task<bool> AddAsync(T item)
        {
            var items = await LoadAllAsync();

            // Check if item already exists - derived class can override this behavior
            if (ItemExists(items, item))
                return false;

            items.Add(item);
            await SaveAllAsync(items);
            return true;
        }

        /// <summary>
        /// Updates an existing item in the JSON file asynchronously.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="updatedItem"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(Func<T, bool> predicate, T updatedItem)
        {
            var items = await LoadAllAsync();
            var existingItem = items.FirstOrDefault(predicate);

            if (existingItem == null)
                return false;

            UpdateItem(existingItem, updatedItem);
            await SaveAllAsync(items);
            return true;
        }

        /// <summary>
        /// Deletes an item from the JSON file asynchronously.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(Func<T, bool> predicate)
        {
            var items = await LoadAllAsync();
            var itemToRemove = items.FirstOrDefault(predicate);

            if (itemToRemove == null)
                return false;

            items.Remove(itemToRemove);
            await SaveAllAsync(items);
            return true;
        }

        /// <summary>
        /// Saves all items to the JSON file asynchronously.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public virtual async Task SaveAllAsync(List<T> items)
        {
            var jsonString = JsonSerializer.Serialize(
                items,
                new JsonSerializerOptions { WriteIndented = true }
            );
            await File.WriteAllTextAsync(FilePath, jsonString);
        }

        // Abstract methods that derived classes must implement
        /// <summary>
        /// Checks if an item already exists in the list.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        protected abstract bool ItemExists(List<T> items, T item);

        /// <summary>
        /// Updates the existing item with values from the updated item.
        /// </summary>
        /// <param name="existingItem"></param>
        /// <param name="updatedItem"></param>
        protected abstract void UpdateItem(T existingItem, T updatedItem);
    }
}

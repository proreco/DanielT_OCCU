namespace DanielT_OCCU.Data
{
    public interface UniversalStorage<T>
        where T : class
    {
        Task<List<T>> LoadAllAsync();
        Task<bool> AddAsync(T item);
        Task<bool> UpdateAsync(Func<T, bool> predicate, T updatedItem);
        Task<bool> DeleteAsync(Func<T, bool> predicate);
        Task SaveAllAsync(List<T> items);
    }
}

using DanielT_OCCU.Data;

namespace DanielT_OCCU.Shared
{
    public static class SearchUtils
    {
        public static IEnumerable<DataObject> FilterCharacters(
            IEnumerable<DataObject> characters,
            string? searchName
        )
        {
            if (string.IsNullOrWhiteSpace(searchName))
                return characters ?? Enumerable.Empty<DataObject>();
            return (characters ?? Enumerable.Empty<DataObject>()).Where(c =>
                c.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}

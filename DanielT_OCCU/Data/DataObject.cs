using System.ComponentModel.DataAnnotations;

namespace DanielT_OCCU.Data
{
    public class DataObject
    {
        //[Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        public string? CharacterClass { get; set; }
        public string? FavoredWeapon { get; set; }
        public string? Catchphrase { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public DataObject()
        {
            UpdatedAt = DateTime.Now;
        }

        public DataObject(DataObject other)
        {
            Name = other.Name;
            CharacterClass = other.CharacterClass;
            FavoredWeapon = other.FavoredWeapon;
            Catchphrase = other.Catchphrase;
            UpdatedAt = DateTime.Now;
        }
    }
}

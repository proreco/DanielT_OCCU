using System.ComponentModel.DataAnnotations;

namespace DanielT_OCCU.Data
{
    public class DataObject
    {
        [Required(ErrorMessage = "Name is required")]
        public string name { get; set; } = string.Empty;
        public string? characterClass { get; set; }
        public string? favoredWeapon { get; set; }
        public string? catchphrase { get; set; }
        public DateTime updatedAt { get; set; }

        public DataObject()
        {
            updatedAt = DateTime.Now;
        }
    }
}
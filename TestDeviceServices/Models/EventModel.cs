using System.ComponentModel.DataAnnotations;

namespace TestDeviceServices.Models
{
    public class EventModel
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string EventDescription { get; set; }
        public Guid DeviceGuid { get; set; }
        [Required]
        public float ParamFloat { get; set; }
        public int ExternalId { get; set; }
        public DateTime DateCreate { get; set; } 
    }
}

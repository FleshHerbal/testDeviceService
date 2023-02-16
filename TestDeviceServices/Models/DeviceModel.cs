using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestDeviceServices.Models
{
    public class DeviceModel
    {
        [Key]
        public Guid Guid { get; set; }
        [Required(ErrorMessage = "The name of the transport is required!")]
        [MinLength(length: 3, ErrorMessage = "Name too short!")]
        public string DeviceName { get; set; }
        [Required(ErrorMessage = "External identifier is required")]
        public int ExternalId { get; set; }
        public DateTime DateCreate { get; set; }
    }

    public class DeviceTokens 
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public Guid DevicetGuid { get; set; }
        public DateTime LastAuth { get; set; }
    }
}

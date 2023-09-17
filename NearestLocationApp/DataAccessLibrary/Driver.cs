
using System.ComponentModel.DataAnnotations;

namespace DataAccessLibrary
{
    public class Driver
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string PlateNumber { get; set; } = string.Empty;
        [Required]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        public string TruckType { get; set; } = string.Empty;
        [Required]
        public float TruckLength { get; set; } = 0f;
        [Required]
        public float TruckWidth { get; set; } = 0f;
        [Required]
        public float TruckHeight { get; set; } = 0f;
        [Required]
        public float Payload { get; set; } = 0f;
    }
}
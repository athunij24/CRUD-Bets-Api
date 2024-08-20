using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BetsApi.Models
{
    public class User 
    {
        [Required]
        public long Id { get; set;}
        [Required]
        [StringLength(50)]
        public string UserName {get; set;}
        [Required]
        [StringLength(50)] 
        public string Password {get; set;}
        [Required]
        [StringLength(50)] 
        public string Name {get; set;}
        public float Balance {get; set;}
        
        public List<Bet> BetsSent { get; set; } = new List<Bet>();
        
        public List<Bet> BetsReceived { get; set; } = new List<Bet>();

    }
}
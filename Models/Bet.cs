using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BetsApi.Models 
{
    public class Bet
    {
        [Required]
        public long Id {get; set;}
        public string Wager{get; set;}
        public float Amount{get; set;}
        public State BetState{get; set;}
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        [JsonIgnore]
        public User? Sender { get; set; }
        [JsonIgnore]
        public User? Receiver { get; set; }
    }

    public enum State{
        Accepted,
        Declined,
        Pending,
    }
}
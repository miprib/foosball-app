using System;
using System.ComponentModel.DataAnnotations;

namespace MatchDetailsAPI.Models
{
    public class MatchDetailItem
    {
        [Required]
        public string ID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Team1 { get; set; }

        [Required]
        public string Team2 { get; set; }

        [Required]
        public int Team1Score { get; set; }

        [Required]
        public int Team2Score { get; set; }
    }
}
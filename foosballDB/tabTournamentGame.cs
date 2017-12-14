namespace foosballDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tabTournamentGame")]
    public partial class tabTournamentGame
    {
        [Key]
        public int GameID { get; set; }

        public DateTime Date { get; set; }

        public int TournamentID { get; set; }

        public virtual tabTournament tabTournament { get; set; }
    }
}

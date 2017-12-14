namespace foosballDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tabLeftTournamentPlayer")]
    public partial class tabLeftTournamentPlayer
    {
        [Key]
        public int GameID { get; set; }

        public int LeftPlayerID { get; set; }

        public int? Score { get; set; }

        public int TournamentID { get; set; }

        public virtual tabTournament tabTournament { get; set; }
    }
}

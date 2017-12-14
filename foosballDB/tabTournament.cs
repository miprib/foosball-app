namespace foosballDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tabTournament")]
    public partial class tabTournament
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tabTournament()
        {
            tabLeftTournamentPlayers = new HashSet<tabLeftTournamentPlayer>();
            tabRightTournamentPlayers = new HashSet<tabRightTournamentPlayer>();
            tabTournamentGames = new HashSet<tabTournamentGame>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TournamentID { get; set; }

        public int UserID { get; set; }

        [StringLength(50)]
        public string Winner { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tabLeftTournamentPlayer> tabLeftTournamentPlayers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tabRightTournamentPlayer> tabRightTournamentPlayers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tabTournamentGame> tabTournamentGames { get; set; }

        public virtual tabUser tabUser { get; set; }
    }
}

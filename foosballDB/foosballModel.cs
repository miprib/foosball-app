namespace foosballDB
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class foosballModel : DbContext
    {
        public foosballModel()
            : base("name=foosballModel")
        {
        }

        public virtual DbSet<tabLeftTournamentPlayer> tabLeftTournamentPlayers { get; set; }
        public virtual DbSet<tabRightTournamentPlayer> tabRightTournamentPlayers { get; set; }
        public virtual DbSet<tabTournament> tabTournaments { get; set; }
        public virtual DbSet<tabTournamentGame> tabTournamentGames { get; set; }
        public virtual DbSet<tabUser> tabUsers { get; set; }
        public virtual DbSet<viewResult> viewResults { get; set; }
        public virtual DbSet<database_firewall_rules> database_firewall_rules { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tabUser>()
                .HasMany(e => e.tabTournaments)
                .WithRequired(e => e.tabUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<database_firewall_rules>()
                .Property(e => e.start_ip_address)
                .IsUnicode(false);

            modelBuilder.Entity<database_firewall_rules>()
                .Property(e => e.end_ip_address)
                .IsUnicode(false);
        }
    }
}

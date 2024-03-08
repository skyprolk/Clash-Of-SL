namespace CSS.Database
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public class Mysql : DbContext
    {
        public Mysql() : base("name=mysql")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Clan> Clan { get; set; }
        public virtual DbSet<Player> Player { get; set; }
    }
}

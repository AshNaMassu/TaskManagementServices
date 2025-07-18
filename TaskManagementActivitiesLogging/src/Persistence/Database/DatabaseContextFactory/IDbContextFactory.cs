namespace Persistence.Database.Context
{
    public interface IDbContextFactory
    {
        public DataBaseContext CreateDbContext();
    }
}

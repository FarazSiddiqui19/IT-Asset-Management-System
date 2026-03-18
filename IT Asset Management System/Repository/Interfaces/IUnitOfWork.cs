namespace IT_Asset_Management_System.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync();
    }
}

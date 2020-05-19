namespace AuthDataAccess.Abstractions
{
    public interface IUnitOfWork
    {
        ITenantRepository TenantRepository { get; }
        IUserRepository UserRepository { get; }
        void Commit();
        void Rollback();
    }
}

using AuthDataAccess.Abstractions;

namespace AuthDataAccess.SQLImplementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private AuthManagementDbContext _context;
        private ITenantRepository _tenantRepository;
        private IUserRepository _userRepository;
        public UnitOfWork(AuthManagementDbContext context)
        {

            _context = context;
        }


        public ITenantRepository TenantRepository
        {
            get { return _tenantRepository = _tenantRepository ?? new TenantRepository(_context); }
        }

        public IUserRepository UserRepository
        {
            get { return _userRepository = _userRepository ?? new UserRepository(_context); }
        }

        public void Commit()
        { _context.SaveChanges(); }

        public void Rollback()
        { _context.Dispose(); }
    }
}

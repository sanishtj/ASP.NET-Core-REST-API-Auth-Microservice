using AuthDataAccess.Abstractions;

namespace AuthDataAccess.SQLImplementation
{
    public class UserRepository : IUserRepository
    {
        private AuthManagementDbContext _context;
        public UserRepository(AuthManagementDbContext context)
        {
            _context = context;
        }
        public string RepoCheck()
        {
            return "User Repo Check Success";
        }
    }
}

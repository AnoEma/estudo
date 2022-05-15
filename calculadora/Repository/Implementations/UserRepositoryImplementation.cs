using Calculadora.Data.VO;
using Calculadora.DTO;
using Calculadora.Model;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Calculadora.Repository.Implementations
{
    public class UserRepositoryImplementation : IUserRepository
    {
        private readonly DatabaseContext _context;
        public UserRepositoryImplementation(DatabaseContext context)
        {
            _context = context;
        }

        public User ValidateCredentials(UserVO user)
        {
            var pass = ComputeHash(user.Password, new SHA256CryptoServiceProvider());
            var result = _context
                            .Users
                            .FirstOrDefault(u => (u.UserName == user.UserName) && (u.Password == pass));
            return result;
        }
        public User ValidateCredentials(string username)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == username);
        }
        public bool RevokeToken(string username)
        {
           var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user != null) return false;

            user.RefreshToken = null;
            _context.SaveChanges();

            return true;
        }

        public User RefreshUserInfo(User user)
        {
            if (!_context.Users.Any(u => u.Id.Equals(user.Id)))
            {
                return null;
            }
            var result = _context.Users.SingleOrDefault(x => x.Id.Equals(user.Id));
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        private string ComputeHash(string password, SHA256CryptoServiceProvider algorithm)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }  
    }
}
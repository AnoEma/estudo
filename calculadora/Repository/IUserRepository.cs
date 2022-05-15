using Calculadora.Data.VO;
using Calculadora.Model;

namespace Calculadora.Repository
{
    public interface IUserRepository
    {
        User ValidateCredentials(UserVO user);
        User ValidateCredentials(string username); 
        bool RevokeToken(string username);
        User RefreshUserInfo(User user);
    }
}
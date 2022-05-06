using Calculadora.Data.VO;
using Calculadora.Model;

namespace Calculadora.Repository
{
    public interface IUserRepository
    {
        User ValidateCredentials(UserVO user);
        User RefreshUserInfo(User user);
    }
}
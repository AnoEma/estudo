using Calculadora.Data.VO;

namespace Calculadora.Service
{
    public interface ILoginService
    {
        TokenVO ValidateCredentials(UserVO user);
        TokenVO ValidateCredentials(TokenVO token);
        bool RevokeToken(string username);
    }
}
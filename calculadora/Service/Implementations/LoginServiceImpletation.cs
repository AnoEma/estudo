using Calculadora.Configurations;
using Calculadora.ConfigurationService;
using Calculadora.Data.VO;
using Calculadora.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Calculadora.Service.Implementations
{
    public class LoginServiceImpletation : ILoginService
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private TokenConfiguration _config;
        private IUserRepository _repository;
        private readonly ITokenService _service;

        public LoginServiceImpletation(TokenConfiguration config, IUserRepository repository, ITokenService service)
        {
            _config = config;
            _repository = repository;
            _service = service;
        }

        public TokenVO ValidateCredentials(UserVO userCredentials)
        {
            var user = _repository.ValidateCredentials(userCredentials);
            if (user == null) return null;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var accessToken = _service.GenerateAccessToken(claims);
            var refreshToken = _service.GenerateRefreshToken();

            return Save(user, accessToken, refreshToken);
        }
       
        public TokenVO ValidateCredentials(TokenVO token)
        {
            var accessToken = token.AccessToken;
            var refreshToken = token.RefreshToken;

            var principal = _service.GetPrincipalFromExpiredToken(accessToken);

            var username = principal.Identity.Name;

            var user = _repository.ValidateCredentials(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTipe <= DateTime.Now)
                return null;

            accessToken = _service.GenerateAccessToken(principal.Claims);
            refreshToken = _service.GenerateRefreshToken();

            return Save(user, accessToken, refreshToken);
        }

        public bool RevokeToken(string username)
        {
            return _repository.RevokeToken(username);
        }

        private TokenVO Save(Model.User user, string accessToken, string refreshToken)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTipe = DateTime.Now.AddMinutes(_config.Minutes);
            _repository.RefreshUserInfo(user);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_config.Minutes);

            return new TokenVO
                (
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken
                );
        }
    }
}
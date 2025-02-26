using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Ambev.DeveloperEvaluation.Common.Security
{
    public static class JwtKeyProvider
    {
        private static readonly string SecretKey = "sua_chave_super_secreta_com_pelo_menos_32_caracteres";
        private static SymmetricSecurityKey _securityKey;

        public static SymmetricSecurityKey GetSecurityKey()
        {
            if (_securityKey == null)
            {
                var keyBytes = Encoding.UTF8.GetBytes(SecretKey);
                _securityKey = new SymmetricSecurityKey(keyBytes);
                Console.WriteLine($"SecurityKey created with hash: {_securityKey.GetHashCode()}");
            }

            return _securityKey;
        }
    }
} 
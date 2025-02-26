using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class AuthenticateUserMapping : Profile
{
    public AuthenticateUserMapping()
    {
        CreateMap<AuthenticateUserRequest, AuthenticateUserCommand>();
    }
}

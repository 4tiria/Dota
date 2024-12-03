using Domain.Mongo.Auth.Models.Entities;
using Dota.Auth.Models.DTO.Requests;
using Dota.Auth.Models.DTO.Responses;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Dota.Auth.Auth;

public interface IAuthService
{
    AuthResponse Login(Account contextAccount);

    void Register(RegistrationRequest registrationRequest, Guid userGuid, string callbackUrl);

    Account AuthenticateUser(string email, string password);

    string CreateRefreshToken(Guid accountId);

    string GetRefreshedAccessToken(AuthResponse authResponse, string refreshToken);
}

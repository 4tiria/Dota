using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Domain.Mongo.Auth;
using Domain.Mongo.Auth.Models;
using Dota.Auth.Models.DTO.Requests;
using Dota.Auth.Models.DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Dota.Auth.Auth;
using Dota.Auth.Email;

namespace Dota.Auth.Controllers;

[ApiController, Route("auth")]
public class AccountController(MongoDbContext context,
    IOptions<AuthOptions> authOptions,
    TokenValidationParameters tokenValidationParameters,
    IConfiguration configuration,
    IAuthService authService,
    IEmailService emailService) : Controller
{
    private readonly MongoDbContext _context = context;
    private readonly IOptions<AuthOptions> _authOptions = authOptions;
    
    private readonly IConfiguration _configuration = configuration;
    private readonly IAuthService _authService = authService;
    private readonly IEmailService _emailService = emailService;

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        if (loginRequest is null)
            return BadRequest();

        var account = _authService.AuthenticateUser(loginRequest.Email, loginRequest.Password);
        if (account is null) 
            return Unauthorized();

        var authResponse = _authService.Login(account);

        Response.Cookies.Append(
            Cookie.REFRESH_TOKEN, 
            _authService.CreateRefreshToken(account.Id), 
            new CookieOptions() { HttpOnly = true });

        return Ok(authResponse);
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] AuthResponse request)
    {
        if (request is null)
            return BadRequest();

        if (!Request.Cookies.TryGetValue(Cookie.REFRESH_TOKEN, out var refreshToken) || refreshToken == null)
            return Unauthorized();

        var tokens = _authService.GetRefreshedAccessToken(request, refreshToken);
        Response.Cookies.Append(Cookie.REFRESH_TOKEN, refreshToken, new CookieOptions() { HttpOnly = true });
        return Ok(tokens);
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegistrationRequest registrationRequest)
    {
        var confirmationJwtToken = _emailService.GenerateEmailConfirmationJwtToken();
        var confirmationJwtTokenString = new JwtSecurityTokenHandler().WriteToken(confirmationJwtToken);
        var guid = new Guid();
        var callbackUrl = Url.Action(
            "ConfirmEmail",
            "Account",
            new { userId = guid, code = confirmationJwtTokenString },
            protocol: HttpContext.Request.Scheme);

        _authService.Register(registrationRequest, guid, callbackUrl);

        return Login(new LoginRequest()
        {
            Email = registrationRequest.Email,
            Password = registrationRequest.Password,
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout([FromBody] LogoutRequest logoutRequest)
    {
        var refreshToken = _context.RefreshTokens.FindOneAndDelete(x => x.AccountId.ToString() == logoutRequest.AccountId);

        if (refreshToken is null)
            return NotFound();

        if (Request.Cookies.TryGetValue(Cookie.REFRESH_TOKEN, out _))
            Response.Cookies.Delete(Cookie.REFRESH_TOKEN);

        return Ok();
    }


    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(Guid userId, string code)
    {
        if (code is null)
            return BadRequest();

        var updatedAccount = await _emailService.ConfirmEmail(userId, code);

        if (updatedAccount == null)
            return NotFound();

        return Redirect($"{_configuration["Client:Url"]}");
    }

}
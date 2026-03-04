using CriptoBank.Application.DTOs.User;
using CriptoBank.Application.DTOs.UserToken;
using CriptoBank.Application.Interfaces.Token;
using CriptoBank.Application.Repositories.Token;
using CriptoBank.Domain.Models;
using CriptoBank.Domain.Repositories;

namespace CriptoBank.Application.Repositories;
public class AuthService : IAuthService
{
    private readonly IUserTokenRepositories _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserTokenRepositories userRepository,
        IPortfolioRepository portfolioRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _portfolioRepository = portfolioRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDTO> RegisterAsync(
        RegisterRequestDTO request,
        CancellationToken ct)
    {
        var existing = await _userRepository
            .GetByEmailAsync(request.Email, ct);

        if (existing != null)
            throw new Exception("Usuário já existe");

        var hash = _passwordHasher.Hash(request.Password);

        var user = new User(request.Name, request.Email, hash);

        await _userRepository.AddAsync(user, ct);

        var portfolio = new Portfolio(
          user.Id,
          "Carteira Principal"
        );

        await _portfolioRepository.AddAsync(portfolio);

        var token = _tokenService.GenerateToken(user);

        return new AuthResponseDTO
        {
            Token = token,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<AuthResponseDTO> LoginAsync(
        LoginRequestDTO request,
        CancellationToken ct)
    {
        var user = await _userRepository
            .GetByEmailAsync(request.Email, ct);

        if (user is null)
            throw new Exception("Usuário inválido");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new Exception("Senha inválida");

        var token = _tokenService.GenerateToken(user);

        return new AuthResponseDTO
        {
            Token = token,
            Name = user.Name,
            Email = user.Email
        };
    }
}
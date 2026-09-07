using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Google.Apis.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServicesAbstraction;
using Shared.Constants;
using Shared.DTOs.Auth;
using Shared.Exceptions;
using Shared.Settings;

namespace Persistence.Services;

public class GoogleTokenValidator : IGoogleTokenValidator
{
    private readonly GoogleAuthSettings _settings;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GoogleTokenValidator> _logger;

    public GoogleTokenValidator(
        IOptions<GoogleAuthSettings> settings,
        IHttpClientFactory httpClientFactory,
        ILogger<GoogleTokenValidator> logger)
    {
        _settings = settings.Value;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public bool IsConfigured => _settings.IsConfigured;

    public string? ClientId => _settings.ClientId;

    public async Task<GoogleUserProfile> ValidateAsync(
        GoogleSignInRequest request, CancellationToken cancellationToken = default)
    {
        if (!_settings.IsConfigured)
        {
            _logger.LogError(
                "An external sign-in was attempted but {Section}:ClientId is not configured.",
                GoogleAuthSettings.SectionName);

            throw new BadRequestException(UserMessages.Auth.GoogleNotAvailable);
        }

        var idToken = string.IsNullOrWhiteSpace(request.IdToken)
            ? await ExchangeAuthorizationCodeAsync(request, cancellationToken)
            : request.IdToken.Trim();

        if (string.IsNullOrWhiteSpace(idToken))
            throw new UnauthorizedException(UserMessages.Auth.GoogleCredentialRequired);

        var payload = await VerifySignatureAsync(idToken);

        if (string.IsNullOrWhiteSpace(payload.Subject))
            throw new UnauthorizedException(UserMessages.Auth.GoogleCredentialInvalid);

        return new GoogleUserProfile(
            payload.Subject,
            payload.Email,
            payload.EmailVerified,
            payload.Name,
            payload.GivenName,
            payload.FamilyName,
            payload.Picture);
    }

    private async Task<GoogleJsonWebSignature.Payload> VerifySignatureAsync(string idToken)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = [_settings.ClientId!],
            IssuedAtClockTolerance = TimeSpan.Zero,
            ExpirationTimeClockTolerance = TimeSpan.Zero
        };

        try
        {
            return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        }
        catch (InvalidJwtException exception)
        {
            _logger.LogInformation(exception, "An external sign-in credential failed validation.");
            throw new UnauthorizedException(UserMessages.Auth.GoogleCredentialInvalid);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An external sign-in credential could not be validated.");
            throw new UnauthorizedException(UserMessages.Auth.GoogleCredentialInvalid);
        }
    }

    private async Task<string> ExchangeAuthorizationCodeAsync(
        GoogleSignInRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
            throw new UnauthorizedException(UserMessages.Auth.GoogleCredentialRequired);

        if (!_settings.CanExchangeAuthorizationCode)
        {
            _logger.LogError(
                "An authorization code was sent but {Section}:ClientSecret is not configured.",
                GoogleAuthSettings.SectionName);

            throw new BadRequestException(UserMessages.Auth.GoogleNotAvailable);
        }

        var redirectUri = string.IsNullOrWhiteSpace(request.RedirectUri)
            ? _settings.RedirectUri
            : request.RedirectUri.Trim();

        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["code"] = request.Code.Trim(),
            ["client_id"] = _settings.ClientId!,
            ["client_secret"] = _settings.ClientSecret!,
            ["redirect_uri"] = redirectUri,
            ["grant_type"] = "authorization_code"
        });

        try
        {
            var client = _httpClientFactory.CreateClient(GoogleAuthCatalog.ProviderName);

            using var response = await client.PostAsync(
                GoogleAuthCatalog.TokenEndpoint, form, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "The external token exchange was refused with status {Status}.",
                    (int)response.StatusCode);

                throw new UnauthorizedException(UserMessages.Auth.GoogleCredentialInvalid);
            }

            var payload = await response.Content
                .ReadFromJsonAsync<TokenExchangeResponse>(cancellationToken);

            if (payload is null || string.IsNullOrWhiteSpace(payload.IdToken))
                throw new UnauthorizedException(UserMessages.Auth.GoogleCredentialInvalid);

            return payload.IdToken;
        }
        catch (AppException)
        {
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "The external token exchange could not be completed.");
            throw new UnauthorizedException(UserMessages.Auth.GoogleCredentialInvalid);
        }
    }

    private sealed class TokenExchangeResponse
    {
        [JsonPropertyName("id_token")]
        public string? IdToken { get; set; }
    }
}

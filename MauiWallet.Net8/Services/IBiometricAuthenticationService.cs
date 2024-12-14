using MauiWallet.Models;

namespace MauiWallet.Services;

public interface IBiometricAuthenticationService
{
    public Task<bool> CheckIfBiometricsAreAvailableAsync();
    public Task<BiometricAuthenticationResult> AuthenticateAsync( string title, string message);
}
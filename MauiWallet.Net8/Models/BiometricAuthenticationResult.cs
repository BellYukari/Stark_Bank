using MauiWallet.Enums;

namespace MauiWallet.Models;

public class BiometricAuthenticationResult
{
    public BiometricAuthenticationStatus Status { get; set; }  
    public string ErrorMessage { get; set; }
}
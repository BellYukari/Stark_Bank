using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWallet.Models
{
    public class ApiLoginResponse
    {
        public Message Message { get; set; }
        public Data Data { get; set; }
    }

    public class Message
    {
        public List<string> Error { get; set; }
        public List<string> Success { get; set; }
    }

    public class Data
    {
        public string Token { get; set; }
        public User User { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string MobileCode { get; set; }
        public string Mobile { get; set; }
        public string FullMobile { get; set; }
        public Address Address { get; set; }
        public int EmailVerified { get; set; }
        public int SmsVerified { get; set; }
        public int KycVerified { get; set; }
        public string UserImage { get; set; }
        public string LastLogin { get; set; }
        public List<Wallet> Wallets { get; set; }
    }

    public class Address
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string AddressLine { get; set; }
    }

    public class Wallet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CurrencyId { get; set; }
        public decimal Balance { get; set; }
        public int Status { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MauiWallet.Services
{
    public class ApiResponseUserExist
    {
        public Data Data { get; set; }
    }


    public class Data
    {
        public exist_user exist_user { get; set; }
    }

    public class exist_user
    {
        public int Id { get; set; }
        public string id_device { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string mobile_code { get; set; }
        public string Mobile { get; set; }
        public string full_mobile { get; set; }
        public object refferal_user_id { get; set; }
        public string Image { get; set; }
        public int Status { get; set; }
        public Address address { get; set; }
        public int email_verified { get; set; }
        public int sms_verified { get; set; }
        public int kyc_verified { get; set; }
        public object ver_code { get; set; }
        public object ver_code_send_at { get; set; }
        public int two_factor_verified { get; set; }
        public int two_factor_status { get; set; }
        public object two_factor_secret { get; set; }
        public object device_id { get; set; }
        public object email_verified_at { get; set; }
        public object deleted_at { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public object sudo_customer { get; set; }
        public object sudo_account { get; set; }
        public object stripe_card_holders { get; set; }
        public object stripe_connected_account { get; set; }
        public object strowallet_customer { get; set; }
        public string Fullname { get; set; }
        public string UserImage { get; set; }
        public StringStatus StringStatus { get; set; }
        public string LastLogin { get; set; }
        public KycStringStatus KycStringStatus { get; set; }
    }

    public class Address
    {
        public string address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
    }

    public class StringStatus
    {
        public string Class { get; set; }
        public string Value { get; set; }
    }

    public class KycStringStatus
    {
        public string Class { get; set; }
        public string Value { get; set; }
    }


}

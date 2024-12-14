using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MauiWallet.Models;

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace MauiWallet.Services
{
    public class ApiResponse
    {
        public object Data { get; set; }
        public Message Message { get; set; }
    }

    public class Message
    {
        public List<string> Success { get; set; }
        public List<string> Error { get; set; }
    }


}

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
    public class ApiResponseResultR1
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

}

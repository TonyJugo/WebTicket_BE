using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTicket.Infrastructure.Options
{
    public class VnPayOptions
    {
        public const string VnPayOptionsKey = "VnPayOptions";
        public string ReturnUrl { get; set; }
        public string Url { get; set; }
        public string TmnCode { get; set; }
        public string HashSecret { get; set; }
    }
}

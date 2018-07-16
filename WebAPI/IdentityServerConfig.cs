using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class IdentityServerConfig
    {
        public string Authority { get; set; }
        public string ApiName { get; set; }

        public string ApiSecret { get; set; }
        public bool RequireHttpsMetadata { get; set; }
    }
}

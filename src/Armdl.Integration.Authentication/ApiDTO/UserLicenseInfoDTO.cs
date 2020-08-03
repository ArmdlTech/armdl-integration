using System;
using System.Collections.Generic;
using System.Text;

namespace Armdl.Integration.ApiDTO
{
    internal class UserLicenseInfoDTO
    {
        public string Status { get; set; }

        public UserLicenseInfoDetailsDTO Info { get; set; }
    }
}

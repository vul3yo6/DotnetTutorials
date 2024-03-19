using System;

namespace DotnetLibrariesTests.Models
{
    internal class License
    {
        public string SysId { get; set; }
        public string SysType { get; set; }
        public string CompanyName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Hardware { get; set; }
        public DateTime modifyDate { get; set; }
        public string Rnd { get; set; }
    }
}

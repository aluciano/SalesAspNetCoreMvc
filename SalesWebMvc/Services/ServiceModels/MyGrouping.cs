using SalesWebMvc.Models;
using System.Collections.Generic;

namespace SalesWebMvc.Services.ServiceModels
{
    public class MyGrouping
    {
        public Department Key { get; set; }
        public IEnumerable<SalesRecord> Sales { get; set; }
    }
}

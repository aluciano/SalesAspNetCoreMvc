using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services.ServiceModels
{
    public class SalesRecordByDateIncluding
    {
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public List<String> IncludeList { get; set; }
        public bool GroupBySellerDepartment { get; set; }
    }
}

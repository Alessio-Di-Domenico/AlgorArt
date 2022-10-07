using System.Collections.Generic;

namespace AlgorArt.Models
{
    public class ViewModel
    {
        public RequestFunds RequestFunds { get; set; }
        public Funders Funders { get; set; }
        public IEnumerable<RequestFunds> RequestFundsList { get; set; }
        public IEnumerable<Funders> FundersList { get; set; }
    }
}

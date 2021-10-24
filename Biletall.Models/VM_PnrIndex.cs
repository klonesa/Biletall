using System.Collections.Generic;

namespace Biletall.Models
{
    public class VM_PnrIndex
    {
        public IEnumerable<PnrModel> PnrModels { get; set; }
    }

    public class PnrModel
    {
        public string PnrNo { get; set; }
        public string FullName { get; set; }
        public string Nereden { get; set; }
        public string Nereye { get; set; }
        public string Tarih { get; set; }
    }
}

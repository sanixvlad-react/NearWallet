
using static NearWallet.Core.Enums;

namespace NearWallet.Core.Models
{
    public class TranzactionModel
    {
        public int Id { get; set; }
        //public string Name { get; set; }
        public string WalletPath { get; set; }
        public string Description { get; set; }
        public string DateCreated { get; set; }
        public string DateTimePassed { get; set; }
        public double Amount { get; set; }
        public string TokenName { get; set; }
        public TranzactionType Type { get; set; }
        public StatusType Status { get; set; }
    }
}

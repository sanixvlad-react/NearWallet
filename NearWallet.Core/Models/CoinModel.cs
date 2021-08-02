
namespace NearWallet.Core.Models
{
    public class CoinModel
    {
        public string Name { get; set; } = string.Empty;
        public string Wallet { get; set; } = string.Empty;
        public string Balance { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsAdded { get; set; }
    }
}

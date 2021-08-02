using System;
namespace NearWallet.Core.Models
{
    public class NftModel
    {
        public int Id { get; set; }
        public int MerketId { get; set; } = 0;
        public string Url { get; set; }
        public string Title { get; set; }
    }
}

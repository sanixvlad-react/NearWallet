using System;
namespace NearWallet.Core.Models
{
    public class ValidatorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public int Procent { get; set; }
        public double StakedNear { get; set; }
        public string Status { get; set; }
    }
}

using System;
namespace NearWallet.Core
{
    public class Enums
    {
        public enum NftItemType
        {
            Header = 0,
            Data = 1
        }

        public enum TranzactionType
        {
            Receive = 0,
            Send = 1,
            AccessKeyAdded = 2,
            MethodCalled = 3
        }

        public enum StatusType
        {
            Succeeded = 0,
            StatusNotAvailable = 1
        }
    }
}

using System.Collections.Generic;

using NearWallet.Core.Models;

namespace NearWallet.Core
{
    public class Data
    {
        public static List<WalletModel> Wallets = new List<WalletModel>
        {
            new WalletModel
            {
                Path = "sanix.near"
            }
        };

        public static List<CoinModel> AllCoins = new List<CoinModel>
        {
            new CoinModel
            {
                Wallet = "AURORA",
                Name = "aurora.tkn.near",
                IsAdded = true,
            },
            new CoinModel
            {
                Wallet = "Skyward Future",
                Name = "jusdao.near",
                IsAdded = false,
            },
            new CoinModel
            {
                Wallet = "Ref Finance",
                Name = "ref-finance.sputnik-dao.near",
                IsAdded = false,
            },
            new CoinModel
            {
                Wallet = "BANANA",
                Name = "berryclub.ek.near",
                IsAdded = true,
            },
            new CoinModel
            {
                Wallet = "Bitcoin",
                Name = "bitcoinminer.near",
                IsAdded = false,
            },
            new CoinModel
            {
                Wallet = "INFINITY",
                Name = "infinity.tkn.near",
                IsAdded = true,
            },
            new CoinModel
            {
                Wallet = "nDAI",
                Name = "6b175474e89094c44da98b954eedeac495271d0f.factory.bridge.near",
                IsAdded = true,
            },
            new CoinModel
            {
                Wallet = "wNEAR",
                Name = "wrap.near",
                IsAdded = true,
            },
        };

        public static List<CoinModel> AddedCoins = new List<CoinModel>
        {
            new CoinModel
            {
                Wallet = "AURORA",
                Balance = "0",
                Name = "aurora.tkn.near"
            },
            new CoinModel
            {
                Wallet = "BANANA",
                Balance = "0.05208",
                Name = "berryclub.ek.near"
            },
            new CoinModel
            {
                Wallet = "INFINITY",
                Balance = "0",
                Name = "infinity.tkn.near"
            },
            new CoinModel
            {
                Wallet = "nDAI",
                Balance = "0",
                Name = "6b175474e89094c44da98b954eedeac495271d0f.factory.bridge.near"
            },
            new CoinModel
            {
                Wallet = "wNEAR",
                Balance = "0",
                Name = "wrap.near"
            },
        };

        public static List<NftModel> Nfts = new List<NftModel>
        {
            new NftModel
            {
                 Id = 1,
                 Title = "nearverse",
            },
            new NftModel
            {
                 Id = 1,
                 MerketId = 1,
                 Title = "Near to the MOON!",
            },
            new NftModel
            {
                 Id = 2,
                 MerketId = 1,
                 Title = "Near to the MOON!",
            },
            new NftModel
            {
                 Id = 3,
                 MerketId = 1,
                 Title = "Near to the MOON!",
            },
            new NftModel
            {
                 Id = 2,
                 Title = "Pluminite",
            },
            new NftModel
            {
                 Id = 1,
                 MerketId = 2,
                 Title = "Near to the MOON!",
            },
            new NftModel
            {
                 Id = 2,
                 MerketId = 2,
                 Title = "Near to the MOON!",
            },
            new NftModel
            {
                 Id = 3,
                 Title = "Mintbase",
            },
            new NftModel
            {
                 Id = 1,
                 MerketId = 3,
                 Title = "Near to the MOON!",
            },

        };

        public static List<ValidatorModel> MyValidators = new List<ValidatorModel>
        {
            new ValidatorModel
            {
                Id = 1,
                Name = "test1.validator.near",
                Procent = 1,
                StakedNear = 10,
                Status = "active"
            },
            new ValidatorModel
            {
                Id = 2,
                Name = "test2.validator.near",
                Procent = 10,
                StakedNear = 10,
                Status = "inactive"
            },
        };

        public static List<ValidatorModel> AllValidators = new List<ValidatorModel>
        {
            new ValidatorModel
            {
                Id = 1,
                Name = "test1.validator.near",
                Procent = 1,
                StakedNear = 10,
                Status = "active"
            },
            new ValidatorModel
            {
                Id = 2,
                Name = "test2.validator.near",
                Procent = 1,
                StakedNear = 10,
                Status = "active"
            },
            new ValidatorModel
            {
                Id = 1,
                Name = "test3.validator.near",
                Procent = 1,
                StakedNear = 10,
                Status = "active"
            },
            new ValidatorModel
            {
                Id = 1,
                Name = "test4.validator.near",
                Procent = 1,
                StakedNear = 10,
                Status = "active"
            },
            new ValidatorModel
            {
                Id = 1,
                Name = "test5.validator.near",
                Procent = 1,
                StakedNear = 10,
                Status = "active"
            },
            new ValidatorModel
            {
                Id = 1,
                Name = "test6.validator.near",
                Procent = 1,
                StakedNear = 10,
                Status = "active"
            },
            new ValidatorModel
            {
                Id = 1,
                Name = "test7.validator.near",
                Procent = 1,
                StakedNear = 10,
                Status = "active"
            },
            new ValidatorModel
            {
                Id = 1,
                Name = "test8.validator.near",
                Procent = 1,
                StakedNear = 10,
                Status = "active"
            },
            new ValidatorModel
            {
                Id = 1,
                Name = "test9.validator.near",
                Procent = 10,
                StakedNear = 10,
                Status = "inactive"
            },
            new ValidatorModel
            {
                Id = 1,
                Name = "test10.validator.near",
                Procent = 10,
                StakedNear = 10,
                Status = "inactive"
            },
            new ValidatorModel
            {
                Id = 1,
                Name = "test11.validator.near",
                Procent = 10,
                StakedNear = 10,
                Status = "inactive"
            },
            new ValidatorModel
            {
                Id = 1,
                Name = "test12.validator.near",
                Procent = 10,
                StakedNear = 10,
                Status = "inactive"
            },
            new ValidatorModel
            {
                Id = 1,
                Name = "test13.validator.near",
                Procent = 10,
                StakedNear = 10,
                Status = "inactive"
            },
            new ValidatorModel
            {
                Id = 1,
                Name = "test14.validator.near",
                Procent = 10,
                StakedNear = 10,
                Status = "inactive"
            },
            new ValidatorModel
            {
                Id = 1,
                Name = "test15.validator.near",
                Procent = 10,
                StakedNear = 10,
                Status = "inactive"
            },
            new ValidatorModel
            {
                Id = 1,
                Name = "test16.validator.near",
                Procent = 10,
                StakedNear = 10,
                Status = "inactive"
            },
            new ValidatorModel
            {
                Id = 1,
                Name = "test17.validator.near",
                Procent = 10,
                StakedNear = 10,
                Status = "inactive"
            },
            new ValidatorModel
            {
                Id = 1,
                Name = "test18.validator.near",
                Procent = 10,
                StakedNear = 10,
                Status = "inactive"
            },
        };

        public static List<AuthorizationAppModel> AuthorizationApps = new List<AuthorizationAppModel>
        {
            new AuthorizationAppModel
            {
                PublicKey = "ed25519:36abRPVucHnYuKjwa1rxAnF8QL2TMQuEe5Zm6Bfkarx2",
                PaymentForStorage = "0.25",
                WalletPath = "tkn.near"
            },
            new AuthorizationAppModel
            {
                PublicKey = "ed25519:3vUQYdQRbkmsDeRPCf4peSDBmM5H5BN1ae21NTnhn3fS",
                PaymentForStorage = "0.25",
                WalletPath = "mintbase1.near"
            },
            new AuthorizationAppModel
            {
                PublicKey = "ed25519:3vqf1JL1fD6A98p3h8GT9oaG2z78hAiSXnGUaxsW4Aio",
                PaymentForStorage = "0.25",
                WalletPath = "berryclub.ek.near"
            },
            new AuthorizationAppModel
            {
                PublicKey = "ed25519:3vqf1JL1fD6A98p3h8GT9oaG2z78hAiSXnGUaxsW4Aio",
                PaymentForStorage = "0.25",
                WalletPath = "pixelparty.near"
            },
            new AuthorizationAppModel
            {
                PublicKey = "ed25519:3vqf1JL1fD6A98p3h8GT9oaG2z78hAiSXnGUaxsW4Aio",
                PaymentForStorage = "0.25",
                WalletPath = "farm.berryclub.ek.near"
            },
            new AuthorizationAppModel
            {
                PublicKey = "ed25519:3vqf1JL1fD6A98p3h8GT9oaG2z78hAiSXnGUaxsW4Aio",
                PaymentForStorage = "0.25",
                WalletPath = "learnclub.near"
            },
            new AuthorizationAppModel
            {
                PublicKey = "ed25519:3vqf1JL1fD6A98p3h8GT9oaG2z78hAiSXnGUaxsW4Aio",
                PaymentForStorage = "0.25",
                WalletPath = "ref-finance.near"
            },
            new AuthorizationAppModel
            {
                PublicKey = "ed25519:3vqf1JL1fD6A98p3h8GT9oaG2z78hAiSXnGUaxsW4Aio",
                PaymentForStorage = "0.25",
                WalletPath = "app.nearcrowd.near"
            },
            new AuthorizationAppModel
            {
                PublicKey = "ed25519:3vqf1JL1fD6A98p3h8GT9oaG2z78hAiSXnGUaxsW4Aio",
                PaymentForStorage = "0.25",
                WalletPath = "skyward.near"
            },
        };

        public static List<TranzactionModel> Tranzactions = new List<TranzactionModel>
        {
            new TranzactionModel
            {
                 WalletPath = "skyward.near",
                 DateTimePassed = "1d",
                 DateCreated = "7/20/21, 12:01 AM",
                 Type = Enums.TranzactionType.AccessKeyAdded,
                 Status = Enums.StatusType.StatusNotAvailable
            },
            new TranzactionModel
            {
                 Amount = 0,
                 Description="deposit_and_stake",
                 WalletPath = "skyward.near",
                 DateTimePassed = "1d",
                 DateCreated = "7/20/21, 12:01 AM",
                 Type = Enums.TranzactionType.MethodCalled,
                 Status = Enums.StatusType.Succeeded
            },
            new TranzactionModel
            {
                 Amount = 14,
                 TokenName = "NEAR",
                 WalletPath = "aura.near",
                 DateTimePassed = "1d",
                 DateCreated = "7/20/21, 12:01 AM",
                 Type = Enums.TranzactionType.Receive,
                 Status = Enums.StatusType.StatusNotAvailable
            },
            new TranzactionModel
            {
                 Amount = 14,
                 TokenName = "NEAR",
                 WalletPath = "skyward.near",
                 DateTimePassed = "1d",
                 DateCreated = "7/20/21, 12:01 AM",
                 Type = Enums.TranzactionType.Send,
                 Status = Enums.StatusType.Succeeded
            },
            new TranzactionModel
            {
                 Amount = 14,
                 TokenName = "NEAR",
                 WalletPath = "aura.near",
                 DateTimePassed = "1d",
                 DateCreated = "7/20/21, 12:01 AM",
                 Type = Enums.TranzactionType.Receive,
                 Status = Enums.StatusType.StatusNotAvailable
            },
            new TranzactionModel
            {
                 Amount = 14,
                 TokenName = "NEAR",
                 WalletPath = "aura.near",
                 DateTimePassed = "1d",
                 DateCreated = "7/20/21, 12:01 AM",
                 Type = Enums.TranzactionType.Receive,
                 Status = Enums.StatusType.Succeeded
            },
            new TranzactionModel
            {
                 Amount = 14,
                 TokenName = "NEAR",
                 WalletPath = "aura.near",
                 DateTimePassed = "1d",
                 DateCreated = "7/20/21, 12:01 AM",
                 Type = Enums.TranzactionType.Receive,
                 Status = Enums.StatusType.StatusNotAvailable
            },
        };

        public static List<PlaceModel> PlaceModels = new List<PlaceModel>
        {
            new PlaceModel
            {
                 Latitude = 48.4418557,
                 Longitude = 22.7224308,
                 Title = "CAFE AVGUSTINA",
                 SubTitle = "∞You can have coffee and cake here. Lovely cafe !!!!∞"
            },
            new PlaceModel
            {
                 Latitude = 48.4410378,
                 Longitude = 22.7210479,
                 Title = "NEAR ∞TAXI∞",
                 SubTitle = "∞ We ride all over Europe for NEAR !!! Vitalij +380661963252; +380683891718 ∞"
            },
            new PlaceModel
            {
                 Latitude = 48.4427675,
                 Longitude = 22.7258172,
                 Title = "FLORI COSMETIC",
                 SubTitle = "∞ Natural handmade cosmetics! Worldwide delivery!!! We accept NEAR. Try us flori.ua ∞"
            },
            new PlaceModel
            {
                 Latitude = 48.4420136,
                 Longitude = 22.7190902,
                 Title = "TRANSFORM",
                 SubTitle = "This is start here! https://transform.red, https://t.me/infinitynft, https://discord.gg/9dR3jZdgsW, https://twitter.com/TRANSFORMNFT"
            },
            new PlaceModel
            {
                 Latitude = 48.442842,
                 Longitude = 22.7178663,
                 Title = "NEWSSTAND",
                 SubTitle = "∞You can buy newspapers and souvenirs. bernfiona.near∞"
            },
            new PlaceModel
            {
                 Latitude = 46.64995,
                 Longitude = 32.6114133,
                 Title = "CAFE DONER",
                 SubTitle = "You can buy Turkish fast food here🍺 0638057435 doner.near https://instagram.com/doner_kherson?utm_medium=copy_link"
            },
        };
    }
}

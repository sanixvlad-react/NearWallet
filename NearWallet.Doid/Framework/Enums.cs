
using AndroidX.AppCompat.App;

namespace NearWallet.Doid.Framework
{
    public enum StakedFragmentType
    {
        AllValidators = 0,
        Validator = 1,
        YourValidators = 2
    }

    public enum StakeType
    {
        Staked = 0,
        UnStaked = 1
    }

    public enum ThemeType
    {
        System = AppCompatDelegate.ModeNightFollowSystem,
        Day = AppCompatDelegate.ModeNightNo,
        Night = AppCompatDelegate.ModeNightYes
    }

}

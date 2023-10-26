namespace Authentication
{
    [System.Serializable]
    public enum SocialAuthType
    {
        Google,
        GooglePlay,
        Facebook,
        Apple,
        Guest,
    }

    public interface ISocialAuth
    {
        void Initialize();
        void Update();
        void SignUp();
        void SignIn();
        void SignOut();
        void LinkAccount();
        void DisableAccount();
        void DeleteAccount();
    }
}
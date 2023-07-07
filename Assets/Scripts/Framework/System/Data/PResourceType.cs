namespace Framework
{
    [System.Serializable]
    public enum PResourceType
    {
        Consumable,
        Nonconsumable,
    }
    [System.Serializable]
    public enum PConsumableType
    {
        GEM = 1,
        BERI = 2,
    }
    [System.Serializable]
    public enum PNonConsumableType
    {
        AVATAR = 3,
        AVATAR_FRAME = 4,
        BATTLE_FIELD = 5,
        SKIN_SHIP = 6,
        ELITE = 7,
    }
}
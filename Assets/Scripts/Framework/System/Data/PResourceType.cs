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
        SKIN_SHIP = 5,
    }
}
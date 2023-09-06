namespace Framework
{
    public enum ServerRequest
    {
        _CONFIG = 4002,
        _CHECK_RANK = 4003,
        _RANK_REWARD = 4004,
        _CANCEL_FIND_MATCH = 4103,
        _CHANGE_AVATAR = 4010,
        _CHANGE_FRAME = 4011,
        _CHANGE_BATTLE_FIELD = 4012,
        _CHANGE_ACHIEVEMENT = 4013,
        _CHANGE_NAME = 4014,
        _CONFIG_ACHIEVEMENT = 4020,
        _ACHIEVEMENT_REWARD = 4021,
        _CONFIG_SHOP = 4030,
        _TRANSACTION = 4031,
        _LUCKYSHOT_EARN = 4040,
        _LUCKYSHOT_FIRE = 4041,
        _CONFIG_RP = 4050,
        _RP_DAILYQUEST_REWARD = 4052,
        _RP_SEASONQUEST_REWARD = 4053,
        _RP_REWARD = 4054,
        _RP_CHANGE_QUEST = 4055,
        _RP_UPGRADE = 4056,
        _RP_DATA = 4057,
        _CONFIG_ADS = 4060,
        _GIFT_CONFIG = 4070,
        _GIFT = 4071,
        //_PVE_ATTACK = 4081,

        _FIND_MATCH = 4101,
        _SUBMIT_SHIP = 4110,
        _REMATCH = 4114,

        _CONFIG_TREASURE = 4201,
        _DATA_TREASURE = 4202,
        _NEWGAME_TREASURE = 4203,
        _FIRE_TREASURE = 4204,
        _END_GAME_TREASURE = 4205,
    }
    public enum ServerResponse
    {
        // Default
        CheckLoginConnection = 1,
        //---------


        _PROFILE = 4001,
        _CONFIG = 4002,
        _CHECK_RANK = 4003,
        _RANK_REWARD = 4004,
        _CHANGE_AVATAR = 4010,
        _CHANGE_FRAME = 4011,
        _CHANGE_BATTLE_FIELD = 4012,
        _CHANGE_ACHIEVEMENT = 4013,
        _CHANGE_NAME = 4014,
        _CONFIG_ACHIEVEMENT = 4020,
        _ACHIEVEMENT_REWARD = 4021,
        _CONFIG_SHOP = 4030,
        _TRANSACTION = 4031,
        _LUCKYSHOT_EARN = 4040,
        _LUCKYSHOT_FIRE = 4041,
        _CONFIG_RP = 4050,
        _RP_DAILYQUEST_REWARD = 4052,
        _RP_SEASONQUEST_REWARD = 4053,
        _RP_REWARD = 4054,
        _RP_CHANGE_QUEST = 4055,
        _RP_UPGRADE = 4056,
        _RP_DATA = 4057,
        _CONFIG_ADS = 4060,
        _REWARD_ADS = 4061,
        _GIFT_CONFIG = 4070,
        _GIFT = 4071,
        _PVE_ATTACK = 4081,

        _QUIT_SEARCH = 4103,
        _MATCH = 4104,
        _GAME_START = 4105,
        _ATTACK = 4111,
        _END_TURN = 4112,
        _TURN_MISS = 4113,
        _REMATCH = 4114,
        _QUIT_GAME = 4115,
        _GAME_DESTROY = 4116,
        _GAME_RECONNECT = 4117,
        _REMATCH_ACCEPT = 4118,

        _CONFIG_TREASURE = 4201,
        _DATA_TREASURE = 4202,
        _NEWGAME_TREASURE = 4203,
        _FIRE_TREASURE = 4204,
        _END_GAME_TREASURE = 4205,

        REQUEST_TREASURE_CONFIG = 4101,
        REQUEST_JOIN_TREASURE_ROOM = 4102,
        REQUEST_SHOOT_TREASURE = 4103,
        REQUEST_EXIT_TREASURE_ROOM = 4104,

        RECIEVE_TREASURE_CONFIG = 4601,
        RECIEVE_JOIN_TREASURE_ROOM = 4602,
        RECIEVE_TREASURE_SHOT = 4603,


    }
}
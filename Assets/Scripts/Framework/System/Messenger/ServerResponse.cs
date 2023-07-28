﻿namespace Framework
{
    public enum ServerRequest
    {
        _CONFIG = 4002,
        _CHECK_RANK = 4003,
        _RANK_REWARD = 4004,
        _CONFIG_SHOP = 4030,
        _TRANSACTION = 4031,
        _CANCEL_FIND_MATCH = 4103,
        _CHANGE_AVATAR = 4010,
        _CHANGE_FRAME = 4011,
        _CHANGE_BATTLE_FIELD = 4012,
        _CHANGE_ACHIEVEMENT = 4013,
        _CHANGE_NAME = 4014,

        _FIND_MATCH = 4101,
        _SUBMIT_SHIP = 4110,
        _REMATCH = 4114,
    }
    public enum ServerResponse
    {
        _PROFILE = 4001,
        _CONFIG = 4002,
        _CHECK_RANK = 4003,
        _RANK_REWARD = 4004,
        _CHANGE_AVATAR = 4010,
        _CHANGE_FRAME = 4011,
        _CHANGE_BATTLE_FIELD = 4012,
        _CHANGE_ACHIEVEMENT = 4013,
        _CHANGE_NAME = 4014,
        _CONFIG_SHOP = 4030,
        _TRANSACTION = 4031,

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

        GetLuckyShot,
        SEARCH_OPPONENT = 9001,
        REQUEST_RECONNECT = 4002,
        QUIT_GAME_REQUEST = 4003,
        ATTACK = 4004,
        QUIT_SEARCH = 4005,
        REQUEST_RANK_CONFIG = 4006,
        REQUEST_BET_CONFIG = 4008,
        REQUEST_OBTAIN_ACHIEVEMENT = 4014,
        REQUEST_ACHIEVEMENT = 4015,
        REQUEST_LUCKY_SHOT_CONFIG = 4016,
        REQUEST_LUCKY_SHOT = 4017,
        REQUEST_RANK = 4018,
        REQUEST_GIFT = 4020,
        REQUEST_GIFT_CONFIG = 4021,
        REQUEST_CHANGE_NAME = 4022,
        REQUEST_CHANGE_AVATAR = 4023,
        REQUEST_SHOP_CONFIG = 4024,
        REQUEST_TRANSACTION = 4025,
        REQUEST_TRANSACTION_MONEY = 4026,
        REQUEST_ACHIEVEMENT_CHANGE = 4028,
        REQUEST_CHANGE_FRAME = 4029,
        REQUEST_CHANGE_BATTLEFIELD = 4030,
        REQUEST_CHANGE_SKIN_SHIP = 4031,
        REQUEST_COUNTDOWN_CONFIG = 4032,
        REQUEST_ROYAL_CONFIG = 4033,
        REQUEST_RECEIVE_ROYALPASS_QUEST = 4034,
        REQUEST_RECEIVE_ROYALPASS_SEASON_QUEST = 4035,
        REQUEST_RECEIVE_ROYALPASS = 4036,
        REQUEST_CHANGE_QUEST = 4038,
        REQUEST_CLAIM_ALL_ROYALPASS = 4039,
        REQUEST_TREASURE_CONFIG = 4101,
        REQUEST_JOIN_TREASURE_ROOM = 4102,
        REQUEST_SHOOT_TREASURE = 4103,
        REQUEST_EXIT_TREASURE_ROOM = 4104,

        START = 4501,
        RECIEVE_RECONNECT = 4502,
        ENEMY_OUT_GAME = 4503,
        BEINGATTACKED = 4504,
        RECEIVE_RANK_CONFIG = 4506,
        NEW_TURN = 4507,
        RECIEVE_BET_CONFIG = 4508,
        ENDGAME = 4510,
        RECIEVE_OBTAIN_ACHIEVEMENT = 4514,
        RECIEVE_ACHIEVEMENT = 4515,
        RECIEVE_LUCKY_SHOT_CONFIG = 4516,
        RECIEVE_LUCKY_SHOT = 4517,
        RECIEVE_RANK = 4518,
        RECIEVE_GIFT = 4520,
        RECIEVE_GIFT_CONFIG = 4521,
        RECIEVE_CHANGE_NAME = 4522,
        RECIEVE_CHANGE_AVATAR = 4523,
        RECIEVE_SHOP_CONFIG = 4524,
        RECIEVE_TRANSACTION = 4525,
        RECIEVE_TRANSACTION_MONEY = 4526,
        RECIEVE_ACHIEVEMENT_CHANGE = 4528,
        RECIEVE_CHANGE_FRAME = 4529,
        RECIEVE_CHANGE_BATTLEFIELD = 4530,
        RECIEVE_CHANGE_SKIN_SHIP = 4531,
        RECEIVE_COUNTDOWN_CONFIG = 4532,
        RECIEVE_ROYAL_CONFIG = 4533,
        RECIEVE_RECEIVE_ROYALPASS_QUEST = 4534,
        RECIEVE_RECEIVE_ROYALPASS_SEASON_QUEST = 4535,
        RECIEVE_RECEIVE_ROYALPASS = 4536,
        RECIEVE_CHANGE_QUEST = 4538,
        RECIEVE_CLAIM_ALL_ROYALPASS = 4539,
        RECIEVE_REWARD_ROCKET = 4540,
        RECIEVE_TREASURE_CONFIG = 4601,
        RECIEVE_JOIN_TREASURE_ROOM = 4602,
        RECIEVE_TREASURE_SHOT = 4603,
        LOGIN = 4999,


    }
}
﻿namespace Framework
{
    public enum GameServerEvent
    {
        SEARCH_OPPONENT = 4001,
        REQUEST_RECONNECT = 4002,
        QUIT_GAME_REQUEST = 4003,
        ATTACK = 4004,
        QUIT_SEARCH = 4005,
        REQUEST_RANK_CONFIG = 4006,
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
        REQUEST_ACHIEVEMENT_CHANGE = 4028,
        REQUEST_AVATAR_FRAME = 4029,
        REQUEST_TREASURE_CONFIG = 4101,
        REQUEST_JOIN_TREASURE_ROOM = 4102,
        REQUEST_EXIT_TREASURE_ROOM = 4104,

        START = 4501,
        RECIEVE_RECONNECT = 4502,
        ENEMY_OUT_GAME = 4503,
        BEINGATTACKED = 4504,
        RECEIVE_RANK_CONFIG = 4506,
        NEW_TURN = 4507,
        COUNTDOWN = 4508,
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
        RECIEVE_ACHIEVEMENT_CHANGE = 4528,
        RECIEVE_AVATAR_FRAME = 4529,
        RECIEVE_TREASURE_CONFIG = 4601,
        RECIEVE_JOIN_TREASURE_ROOM = 4602,
        LOGIN = 4999,

    }
}
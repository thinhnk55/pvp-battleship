using DG.Tweening;
using Framework;
using Lean.Touch;
using SimpleJSON;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Pre,
    Search,
    Turn,
    Anim,
    Out,
}
public class CoreGame : SingletonMono<CoreGame>
{
    public static JSONNode reconnect;
    public static int timeInit;
    public static int bet;
    public static List<int> bets;
    public int roomId;
    public int playerChair;
    public static List<List<Vector2Int>> shipConfigs = new List<List<Vector2Int>>()
    {
        new List<Vector2Int>() { new Vector2Int(0, 0),  },
        new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(-1,0), },
        new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(-1,0), new Vector2Int(-2,0), },
        new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(-1,0), new Vector2Int(-2,0),new Vector2Int(-3,0), },
    };

    [SerializeField] public Board player;
    [SerializeField] Board opponent;
    [SerializeField] GameObject lineRoot;
    public GameObject shipListPlayer;
    [SerializeField] GameObject shipListOpponent;
    [SerializeField] List<Ship> shipsPlayer;
    [SerializeField] List<Ship> shipsOpponent;
    List<List<GameObject>> lines;
    public Camera cam;

    [SerializeField] GameObject preUI;
    [SerializeField] GameObject ingameUI;
    [SerializeField] SearchUI searchUI;
    [SerializeField] PostUI postUI;
    [SerializeField] TextMeshProUGUI betAmountSearch;
    public StateMachine<GameState> stateMachine;

    bool playerTurn;
    float turnTime; float TurnTime { get { return turnTime; } set { turnTime = Mathf.Clamp(value, 0, timeInit); turnTimeText.text = ((int)turnTime).ToString(); } }
    [SerializeField] TextMeshProUGUI turnTimeText;
    [SerializeField] Image turnImage;


    [OnValueChanged("OnRevealed")][SerializeField] private bool reveal;
    public bool Reveal
    {
        get { return reveal; }
        set
        {
            reveal = value;
            OnRevealed();
        }
    }
    void OnRevealed()
    {
        for (int i = 0; i < shipsOpponent.Count; i++)
        {
            shipsOpponent[i].renderer.enabled = reveal;
        }
        foreach (var os in player.octiles) {
            foreach (var octile in os)
            {
                octile.textOccupied.enabled = reveal;
            }
        }
        foreach (var os in opponent.octiles)
        {
            foreach (var octile in os)
            {
                octile.textOccupied.enabled = reveal;
            }
        }
    }

    #region Mono
    protected override void Awake()
    {
        reveal = true;
        cam = Camera.main;
        lines = new List<List<GameObject>>();
        Instance.shipsPlayer = shipListPlayer.GetComponentsInChildren<Ship>().ToList();
        Instance.shipsPlayer.Reverse();
        Instance.player.battleFieldSprite.sprite = SpriteFactory.ResourceIcons[5].sprites[GameData.Player.BattleField.Data];
        Instance.opponent.battleFieldSprite.sprite = SpriteFactory.ResourceIcons[5].sprites[GameData.Opponent.BattleField.Data];
        Instance.shipsOpponent = shipListOpponent.GetComponentsInChildren<Ship>().ToList();
        stateMachine = new StateMachine<GameState>();
        stateMachine.AddState(GameState.Pre, Instance.StartPregame, null, Instance.EndPregame);
        stateMachine.AddState(GameState.Search, Instance.StartSearch, Instance.UpdateSearch, Instance.EndSearch);
        stateMachine.AddState(GameState.Turn, Instance.StartTurn, Instance.UpdateTurn, Instance.EndTurn);
        stateMachine.AddState(GameState.Out, null, null, null);
        stateMachine.CurrentState = GameState.Pre;
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.START, Instance.GameStart);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.ENEMY_OUT_GAME, Instance.EnemyOutGame);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.ENDGAME, Instance.EndGame);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.NEW_TURN, Instance.EndTurn);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.COUNTDOWN, Instance.CountDown);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.BEINGATTACKED, Instance.HandleBeingAttacked);
        if (reconnect!=null)
        {
            Instance.Reconnect(reconnect);
            reconnect = null;
        }
    }
    void Start()
    {


        //ServerMessenger.AddListener<JSONNode>(GameServerEvent.BEINGATTACKED, Attack);
    }

    void Update()
    {
        stateMachine.Update();
    }
    protected override void OnDestroy()
    {
        LeanTouch.OnFingerUp -= Instance.opponent.BeingAttacked;
        LeanTouch.OnFingerUpdate -= Instance.opponent.SelectingTarget;
        Instance.opponent.horzLine.gameObject.SetActive(false);
        Instance.opponent.vertLine.gameObject.SetActive(false);
        stateMachine.CurrentState = GameState.Out;
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.START, Instance.GameStart);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.ENEMY_OUT_GAME, Instance.EnemyOutGame);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.ENDGAME, Instance.EndGame);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.NEW_TURN, Instance.EndTurn);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.COUNTDOWN, Instance.CountDown);
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.BEINGATTACKED, Instance.HandleBeingAttacked);
        Debug.Log("Destroyed");
        base.OnDestroy();
    }
    #endregion

    #region StateMachine
    void StartPregame()
    {
        float sizeWidth = cam.orthographicSize * cam.aspect * 2;
        Instance.preUI.SetActive(true);
        Instance.postUI.gameObject.SetActive(false);
        Instance.searchUI.gameObject.SetActive(false);
        Instance.ingameUI.gameObject.SetActive(false);
        Instance.shipListPlayer.gameObject.SetActive(true);
        Instance.opponent.gameObject.SetActive(false);
        Instance.player.InitBoard(10, 10);
        Instance.player.Position = Vector3.right * (-sizeWidth * 1 / 32 - player.width / 2);
        Instance.opponent.Position = Vector3.right * (sizeWidth * 15 / 32 - opponent.width / 2);
    }
    void EndPregame()
    {

    }
    Tween searchTween;
    void StartSearch()
    {
        WSClient.SearchOpponent(bet, player.ships);
        Instance.opponent.gameObject.SetActive(true);
        Instance.opponent.InitBoard(10, 10);
        Instance.opponent.RandomShip(shipsOpponent);
        Instance.preUI.SetActive(false);
        Instance.searchUI.gameObject.SetActive(true);
        var profile = new ProfileInfo()
        {
            Avatar = -1,
        };
        Instance.searchUI.opponentProfile.BuildUI(profile);
        Instance.shipListPlayer.gameObject.SetActive(false);
    }
    void UpdateSearch()
    {

    }
    void EndSearch()
    {
        searchUI.gameObject.SetActive(false);
    }
    void StartTurn()
    {
        TurnTime = timeInit;
        if (playerTurn)
        {
            Debug.Log("Player Turn");
            LeanTouch.OnFingerUp += Instance.opponent.BeingAttacked;
            LeanTouch.OnFingerUpdate += Instance.opponent.SelectingTarget;
            Instance.turnImage.sprite = SpriteFactory.PlayerTurn;
        }
        else
        {
            Debug.Log("Opponent Turn");
            Instance.turnImage.sprite = SpriteFactory.OpponentTurn;
        }
    }
    void UpdateTurn()
    {
        TurnTime -= Time.deltaTime;
    }
    void EndTurn()
    {
        LeanTouch.OnFingerUp -= Instance.opponent.BeingAttacked;
        LeanTouch.OnFingerUpdate -= Instance.opponent.SelectingTarget;
        Instance.opponent.horzLine.gameObject.SetActive(false);
        Instance.opponent.vertLine.gameObject.SetActive(false);
    }
    #endregion

    #region Public
    public void StartSearchGame()
    {
        if (shipListPlayer.transform.childCount == 0)
        {
            Instance.searchUI.gameObject.SetActive(true);
            Instance.stateMachine.CurrentState = GameState.Search;
        }
    }
    public void RandomShip()
    {
        Instance.player.RandomShip(Instance.shipsPlayer);
    }
    public void QuitSearch()
    {
        Instance.stateMachine.CurrentState = GameState.Pre;
        Instance.searchUI.gameObject.SetActive(false);
        Instance.preUI.SetActive(true);
        Instance.searchTween.Kill();
        WSClient.QuitSearch(bet);
    }
    public void QuitAfter()
    {
        Instance.postUI.gameObject.SetActive(false);
    }
    public void QuitGame()
    {
        SceneTransitionHelper.Load(ESceneName.Home);
        WSClient.QuitGame(roomId);
    }
    public void GoHome()
    {
        SceneTransitionHelper.Load(ESceneName.Home);
    }
    public void Rematch()
    {

    }
    public void NewMatch()
    {
        SceneTransitionHelper.Reload();
    }
    #endregion

    #region CallBackServer
    void GameStart(JSONNode json)
    {
        GameData.Opponent = ProfileData.FromJsonOpponent(GameData.Opponent, json);
        Debug.Log(GameData.Opponent.Username);
        Instance.ingameUI.SetActive(true);
        Instance.roomId = int.Parse(json["r"]);
        Instance.playerChair = int.Parse(json["c"]);
        CoinVFX.CoinVfx(Instance.searchUI.tresure.transform, Instance.searchUI.avatar1.transform.position, Instance.searchUI.avatar2.transform.position);
        PConsumableType.BERI.AddValue(-bets[bet]);
        //opponent.diamond.text = json["d"];
        //opponent.beri.text = json["b"];
        //opponent.point.text = json["p"];
        DOVirtual.DelayedCall(1.5f, () =>
        {
            Instance.playerTurn = int.Parse(json["turn"]) == playerChair;
            Instance.stateMachine.CurrentState = GameState.Turn;
        });
    }

    void HandleBeingAttacked(JSONNode json)
    {
        Instance.playerTurn = playerChair == int.Parse(json["c"]);
        Board board = playerTurn ? Instance.opponent : Instance.player;
        int status = int.Parse(json["s"]);
        int x = int.Parse(json["x"]);
        int y = int.Parse(json["y"]);
        if (status == 2)
        {
            Ship ship;
            int type = int.Parse(json["ship"]["type"]);
            if (Instance.playerTurn)
            {
                ship = Instantiate(PrefabFactory.Ships[type]).GetComponent<Ship>();
                ship.board = board;
                ship.FromJson(json["ship"]);
            }
            else
            {
                ship = board.octiles[y][x].ship;
            }
            ship.BeingDestroyed();
        }
        else if (status == 1)
        {
            board.octiles[y][x].BeingAttacked(true);
        }
        else if (status == 0)
        {
            board.octiles[y][x].BeingAttacked(false);
        }
        //board.HandleAttacked(x,y,status, ship);
    }

    public void EndTurn(JSONNode json)
    {
        Instance.playerTurn = playerChair == int.Parse(json["c"]);
        Instance.stateMachine.CurrentState = GameState.Turn;
    }
    void CountDown(JSONNode json)
    {
        TurnTime = float.Parse(json["c"]);
    }
    void EndGame(JSONNode json)
    {
        DOVirtual.DelayedCall(3, () =>
        {
            Debug.Log("End");
            Instance.stateMachine.CurrentState = GameState.Out;
            Instance.ingameUI.SetActive(false);
            Instance.postUI.gameObject.SetActive(true);
            Instance.postUI.amount.text = json["w"];
            if (int.Parse(json["c"]) == playerChair)
            {
                PConsumableType.BERI.AddValue(int.Parse(json["w"]));
                Instance.postUI.ResultPlayer.sprite = SpriteFactory.Win;
                Instance.postUI.ResultOpponent.sprite = SpriteFactory.Lose;
                CoinVFX.CoinVfx(searchUI.avatar1.transform, searchUI.tresure.transform.position, searchUI.tresure.transform.position);
            }
            else
            {
                PConsumableType.BERI.AddValue(-int.Parse(json["w"]));
                Instance.postUI.ResultPlayer.sprite = SpriteFactory.Lose;
                Instance.postUI.ResultOpponent.sprite = SpriteFactory.Win;
                CoinVFX.CoinVfx(searchUI.avatar2.transform, searchUI.tresure.transform.position, searchUI.tresure.transform.position);
            }
        });
    }
    void EnemyOutGame(JSONNode json)
    {
        SceneTransitionHelper.Load(ESceneName.Home);
    }
    public void Reconnect(JSONNode data)
    {
        int[,] arr = new int[Instance.player.octiles.Count, Instance.player.octiles.Count];
        Instance.opponent.gameObject.SetActive(true);
        Instance.opponent.InitBoard(10, 10);
        for (int i = 0; i < data["ship"].Count; i++)
        {
            Ship ship = Instance.shipsPlayer.Find((ship) => { 
                return ship.poses.Count == int.Parse(data["ship"][i]["type"]) + 1
                && !Instance.player.ships.Contains(ship); 
            });
            ship.board = Instance.player;
            ship.FromJson(data["ship"][i]);
            for (int j = 0; j < ship.octilesComposition.Count; j++)
            {
                arr[ship.octilesComposition[j].pos.y, ship.octilesComposition[j].pos.x] = 3;
            }
        }
        for (int i = 0; i < data["fired"].Count; i++)
        {
            if (arr[int.Parse(data["fired"][i]["y"]), int.Parse(data["fired"][i]["x"])] == 3)
            {
                Instance.player.octiles[int.Parse(data["fired"][i]["y"])][int.Parse(data["fired"][i]["x"])].BeingAttacked(true);
                arr[int.Parse(data["fired"][i]["y"]), int.Parse(data["fired"][i]["x"])] = 2;
            }
            else if (arr[int.Parse(data["fired"][i]["y"]), int.Parse(data["fired"][i]["x"])] == 0)
            {
                Instance.player.octiles[int.Parse(data["fired"][i]["y"])][int.Parse(data["fired"][i]["x"])].BeingAttacked(false);
                arr[int.Parse(data["fired"][i]["y"]), int.Parse(data["fired"][i]["x"])] = 1;
            }
        }
        arr = new int[Instance.opponent.octiles.Count, Instance.opponent.octiles.Count];
        for (int i = 0; i < data["efship"].Count; i++)
        {
            Ship ship = Instantiate(PrefabFactory.Ships[int.Parse(data["efship"][i]["type"])]).GetComponent<Ship>();
            ship.board = Instance.opponent;
            ship.FromJson(data["efship"][i]);
            ship.BeingDestroyed();
            for (int j = 0; j < ship.octilesComposition.Count; j++)
            {
                arr[ship.octilesComposition[j].pos.y, ship.octilesComposition[j].pos.x] = 3;
            }
        }
        for (int i = 0; i < data["sf"].Count; i++)
        {
            if (arr[int.Parse(data["sf"][i]["y"]), int.Parse(data["sf"][i]["x"])] == 0)
            {
                Instance.opponent.octiles[int.Parse(data["sf"][i]["y"])][int.Parse(data["sf"][i]["x"])].BeingAttacked(true);
                arr[int.Parse(data["sf"][i]["y"]), int.Parse(data["sf"][i]["x"])] = 2;
            }
        }
        for (int i = 0; i < data["efired"].Count; i++)
        {
            if (arr[int.Parse(data["efired"][i]["y"]), int.Parse(data["efired"][i]["x"])] == 0)
            {
                Instance.opponent.octiles[int.Parse(data["efired"][i]["y"])][int.Parse(data["efired"][i]["x"])].BeingAttacked(false);
                arr[int.Parse(data["efired"][i]["y"]), int.Parse(data["efired"][i]["x"])] = 1;
            }
        }
        CoreGame.bet = int.Parse(data["bet"]);
        CoreGame.Instance.roomId = int.Parse(data["r"]);
        CoreGame.Instance.playerChair = int.Parse(data["c"]);
        Instance.playerTurn = int.Parse(data["turn"]) == Instance.playerChair;
        Instance.turnTime = int.Parse(data["count"]);
        Instance.ingameUI.SetActive(true);
        Instance.preUI.SetActive(false);
        CoreGame.Instance.stateMachine.CurrentState = GameState.Turn;
        CoreGame.Instance.player.ships = new List<Ship> { };

        Debug.Log("Reconnect");

    }
    #endregion
}

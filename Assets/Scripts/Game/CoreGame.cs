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
public class CoreGame : Singleton<CoreGame>
{
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
        shipsPlayer = shipListPlayer.GetComponentsInChildren<Ship>().ToList();
        shipsPlayer.Reverse();
        shipsOpponent = shipListOpponent.GetComponentsInChildren<Ship>().ToList();
    }
    void Start()
    {
        stateMachine = new StateMachine<GameState>();
        stateMachine.AddState(GameState.Pre, StartPregame, null, EndPregame);
        stateMachine.AddState(GameState.Search, StartSearch, UpdateSearch, EndSearch);
        stateMachine.AddState(GameState.Turn, StartTurn, UpdateTurn, EndTurn);
        stateMachine.AddState(GameState.Out, null, null, null);
        stateMachine.CurrentState = GameState.Pre;
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.START, Instance.GameStart);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.ENEMY_OUT_GAME, Instance.EnemyOutGame);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.ENDGAME, Instance.EndGame);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.NEW_TURN, Instance.EndTurn);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.COUNTDOWN, Instance.CountDown);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.BEINGATTACKED, Instance.HandleBeingAttacked);
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
        preUI.SetActive(true);
        postUI.gameObject.SetActive(false);
        searchUI.gameObject.SetActive(false);
        ingameUI.gameObject.SetActive(false);
        shipListPlayer.gameObject.SetActive(true);
        opponent.gameObject.SetActive(false);
        player.Position = Vector3.right * (-sizeWidth * 1 / 32 - player.width / 2);
        opponent.Position = Vector3.right * (sizeWidth * 15 / 32 - opponent.width / 2);
    }
    void EndPregame()
    {

    }
    Tween searchTween;
    void StartSearch()
    {
        WSClient.SearchOpponent(bet, player.ships);
        opponent.gameObject.SetActive(true);
        opponent.InitBoard(10, 10);
        opponent.RandomShip(shipsOpponent);
        preUI.SetActive(false);
        searchUI.gameObject.SetActive(true);
        var profile = new ProfileData();
        profile.Avatar = -1;
        searchUI.opponentProfile.BuildUI(profile);
        shipListPlayer.gameObject.SetActive(false);
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
            searchUI.gameObject.SetActive(true);
            stateMachine.CurrentState = GameState.Search;
        }
    }
    public void RandomShip()
    {
        player.RandomShip(shipsPlayer);
    }
    public void QuitSearch()
    {
        stateMachine.CurrentState = GameState.Pre;
        searchUI.gameObject.SetActive(false);
        preUI.SetActive(true);
        searchTween.Kill();
        WSClient.QuitSearch(bet);
    }
    public void QuitAfter()
    {
        Instance.postUI.gameObject.SetActive(false);
    }
    public void QuitGame()
    {
        ingameUI.gameObject.SetActive(false);
        SceneTransitionHelper.Load(ESceneName.Home);
        WSClient.QuitGame(roomId);
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
        ProfileData profile = GameData.Opponent;
        GameData.Opponent = ProfileData.FromJson(ref profile, json);
        Debug.Log(GameData.Opponent.Username);
        Instance.ingameUI.SetActive(true);
        roomId = int.Parse(json["r"]);
        playerChair = int.Parse(json["c"]);
        Instance.searchUI.opponentProfile.BuildUI(GameData.Opponent);
        CoinVFX.CoinVfx(Instance.searchUI.tresure.transform, Instance.searchUI.avatar1.transform.position, Instance.searchUI.avatar2.transform.position);
        PResourceType.BERI.AddValue(-bets[bet]);
        //opponent.diamond.text = json["d"];
        //opponent.beri.text = json["b"];
        //opponent.point.text = json["p"];
        DOVirtual.DelayedCall(1.5f, () =>
        {
            playerTurn = int.Parse(json["turn"]) == playerChair;
            stateMachine.CurrentState = GameState.Turn;
        });
    }

    void HandleBeingAttacked(JSONNode json)
    {
        playerTurn = playerChair == int.Parse(json["c"]);
        Board board = playerTurn ? Instance.opponent : Instance.player;
        int status = int.Parse(json["s"]);
        int x = int.Parse(json["x"]);
        int y = int.Parse(json["y"]);
        if (status == 2)
        {
            Ship ship;
            int type = int.Parse(json["ship"]["type"]);
            if (playerTurn)
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
        playerTurn = playerChair == int.Parse(json["c"]);
        stateMachine.CurrentState = GameState.Turn;
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
            stateMachine.CurrentState = GameState.Out;
            Instance.ingameUI.SetActive(false);
            Instance.postUI.gameObject.SetActive(true);
            postUI.amount.text = json["w"];
            if (int.Parse(json["c"]) == playerChair)
            {
                PResourceType.BERI.AddValue(int.Parse(json["w"]));
                postUI.ResultPlayer.sprite = SpriteFactory.Win;
                postUI.ResultOpponent.sprite = SpriteFactory.Lose;
                CoinVFX.CoinVfx(searchUI.avatar1.transform, searchUI.tresure.transform.position, searchUI.tresure.transform.position);
            }
            else
            {
                PResourceType.BERI.AddValue(-int.Parse(json["w"]));
                postUI.ResultPlayer.sprite = SpriteFactory.Lose;
                postUI.ResultOpponent.sprite = SpriteFactory.Win;
                CoinVFX.CoinVfx(searchUI.avatar2.transform, searchUI.tresure.transform.position, searchUI.tresure.transform.position);
            }
        });
    }
    void EnemyOutGame(JSONNode json)
    {
        SceneTransitionHelper.Load(ESceneName.Home);
    }
    #endregion
}

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
    [SerializeField] GameObject searchUI;
    [SerializeField] PostUI postUI;
    [SerializeField] TextMeshProUGUI betAmountSearch;
    public StateMachine<GameState> stateMachine;

    bool playerTurn;
    float turnTime; float TurnTime { get { return turnTime; } set { turnTime = Mathf.Clamp(value,0,timeInit); turnTimeText.text = ((int)turnTime).ToString(); } }
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
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.START, GameStart);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.ENEMY_OUT_GAME, EnemyOutGame);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.ENDGAME, EndGame);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.NEW_TURN, EndTurn);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.COUNTDOWN, CountDown);
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.BEINGATTACKED, HandleBeingAttacked);
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
        base.OnDestroy();
    }
    #endregion

    #region StateMachine
    void StartPregame()
    {
        float sizeWidth = cam.orthographicSize * cam.aspect * 2;
        preUI.SetActive(true);
        shipListPlayer.gameObject.SetActive(true);
        opponent.gameObject.SetActive(false);
        player.Position = Vector3.right * ( sizeWidth * 7 / 16 - player.width/2 - sizeWidth/2);
        opponent.Position = Vector3.right * ( sizeWidth * 14 / 16 - opponent.width/2 - sizeWidth / 2);
        for (int i = 0; i < lines.Count; i++)
        {
            for (int j = 0; j < lines[i].Count; j++)
            {

            }
        }
    }
    void EndPregame()
    {

    }
    Tween searchTween;
    void StartSearch()
    {
        WSClient.SearchOpponent(bet, player.ships);
        opponent.gameObject.SetActive(true);
        opponent.InitBoard(10,10);
        opponent.RandomShip(shipsOpponent);
        preUI.SetActive(false);
        searchUI.SetActive(true);
        shipListPlayer.gameObject.SetActive(false);
    }
    void UpdateSearch()
    {
        
    }
    void EndSearch()
    {
        searchUI.SetActive(false);
        ingameUI.SetActive(true);
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
        TurnTime-= Time.deltaTime;
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
        ingameUI.SetActive(false);
        preUI.SetActive(true);

        searchTween.Kill();
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
    #endregion

    #region CallBackServer
    void GameStart(JSONNode json)
    {
        roomId = int.Parse(json["r"]);
        playerChair = int.Parse(json["c"]);
        opponent.name.text = json["n"];
        //opponent.diamond.text = json["d"];
        //opponent.beri.text = json["b"];
        //opponent.point.text = json["p"];
        playerTurn = int.Parse(json["turn"]) == playerChair;
        stateMachine.CurrentState = GameState.Turn;
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
    void EndTurn(JSONNode json)
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
        DOVirtual.DelayedCall(1, () =>
        {
            Debug.Log("End");
            stateMachine.CurrentState = GameState.Out;
            Instance.ingameUI.SetActive(false);
            Instance.postUI.gameObject.SetActive(true);
            postUI.amount.text = json["w"];
        });
    }
    void EnemyOutGame(JSONNode json)
    {
        SceneTransitionHelper.Load(ESceneName.Home);
    }
    #endregion
}

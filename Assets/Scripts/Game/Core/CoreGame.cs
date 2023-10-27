using Authentication;
using DG.Tweening;
using FirebaseIntegration;
using Framework;
using Lean.Touch;
using SimpleJSON;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Pre,
    PreRematch,
    Search,
    SearchRematch,
    Turn,
    Anim,
    Out,
}
public class CoreGame : SingletonMono<CoreGame>
{
    public static JSONNode reconnect;
    public static int timeInit;
    public static int bet;
    public static bool rematch = false;
    public static int roomId;
    public static int playerChair;
    public int missturn;

    public static List<List<Vector2Int>> shipConfigs = new List<List<Vector2Int>>()
    {
        new List<Vector2Int>() { new Vector2Int(0, 0),  },
        new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(-1,0), },
        new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(-1,0), new Vector2Int(-2,0), },
        new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(-1,0), new Vector2Int(-2,0),new Vector2Int(-3,0), },
    };

    [SerializeField] public Board player;
    [SerializeField] Board opponent;
    [SerializeField] ProfileCollection playerProfile;
    [SerializeField] ProfileCollection opponentProfile;
    [SerializeField] GameObject lineRoot;
    public GameObject shipListPlayer;
    [SerializeField] GameObject shipListOpponent;
    public List<Ship> shipsPlayer;
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

    //rematch
    [SerializeField] TextMeshProUGUI counterRematchObj;
    static float counterRematch;
    float currentCounterRematch;
    [SerializeField] GameObject btnBackPreGame;
    [SerializeField] GameObject btnBackSearchGame;
    [SerializeField] GameObject btnBattle;
    [SerializeField] GameObject btnReady;
    [SerializeField] TextMeshProUGUI rematchChatA;
    [SerializeField] TextMeshProUGUI rematchChatB;
    [SerializeField] Button buttonAuto;
    [SerializeField] Button buttonRematch;

    //
    public int consecutiveKill;
    static public PDataUnit<int> consecutiveKillMax = new(0);

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
            shipsOpponent[i].shipRenderer.enabled = reveal;
        }
        foreach (var os in player.octiles)
        {
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
        AudioHelper.StopMusic();
        consecutiveKill = 0;
        reveal = true;
        cam = Camera.main;
        lines = new List<List<GameObject>>();
        Instance.shipsPlayer = shipListPlayer.GetComponentsInChildren<Ship>().ToList();
        Instance.shipsPlayer.Reverse();
        if (GameData.Player.BattleField != null)
        {
            Instance.player.battleFieldSprite.sprite = SpriteFactory.BattleFields[GameData.Player.BattleField.Data];
        }
        Instance.shipsOpponent = shipListOpponent.GetComponentsInChildren<Ship>().ToList();
        Instance.stateMachine = new StateMachine<GameState>();
        Instance.stateMachine.AddState(GameState.Pre, Instance.StartPregame, Instance.UpdatePregame, Instance.EndPregame);
        Instance.stateMachine.AddState(GameState.PreRematch, Instance.StartPregameReMatch, Instance.UpdatePregameRematch, Instance.EndPregameRematch);
        Instance.stateMachine.AddState(GameState.Search, Instance.StartSearch, Instance.UpdateSearch, Instance.EndSearch);
        Instance.stateMachine.AddState(GameState.SearchRematch, Instance.StartSearchRematch, Instance.UpdateSearchRematch, Instance.EndSearchRematch);
        Instance.stateMachine.AddState(GameState.Turn, Instance.StartTurn, Instance.UpdateTurn, Instance.EndTurn);
        Instance.stateMachine.AddState(GameState.Out, null, null, null);
        if (rematch)
        {
            Instance.stateMachine.CurrentState = GameState.PreRematch;
        }
        else
        {
            Instance.stateMachine.CurrentState = GameState.Pre;
        }
        ServerMessenger.AddListener<JSONNode>(ServerResponse._MATCH, Instance.Match);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._GAME_START, Instance.GameStart);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._GAME_DESTROY, Instance.EnemyOutGame);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._END_TURN, Instance.EndTurn);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._TURN_MISS, Instance.TurnMiss);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._REMATCH, Instance.Rematch);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._REMATCH_ACCEPT, Instance.AcceptRematch);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._QUIT_SEARCH, Instance.QuitSearch);
        if (reconnect != null)
        {
            Instance.Reconnect(reconnect);
        }
    }
    void Start()
    {
    }

    void Update()
    {
        Instance.stateMachine.Update();
    }
    protected override void OnDestroy()
    {
        if (AudioManager.Instance)
        {
            AudioHelper.StopMusic();
        }
        LeanTouch.OnFingerUp -= Instance.opponent.BeingAttacked;
        LeanTouch.OnFingerUpdate -= Instance.opponent.SelectingTarget;
        Instance.opponent.horzLine.SetActive(false);
        Instance.opponent.vertLine.SetActive(false);
        Instance.stateMachine.CurrentState = GameState.Out;
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._MATCH, Instance.Match);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._GAME_START, Instance.GameStart);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._GAME_DESTROY, Instance.EnemyOutGame);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._END_TURN, Instance.EndTurn);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._TURN_MISS, Instance.TurnMiss);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._REMATCH, Instance.Rematch);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._REMATCH_ACCEPT, Instance.AcceptRematch);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._QUIT_SEARCH, Instance.QuitSearch);
        Debug.Log("Destroyed");
        base.OnDestroy();
    }


    #endregion

    #region StateMachine
    void StartPregame()
    {
        if (GameData.Tutorial[2] == 0)
        {
            btnBackPreGame.SetActive(false);
        }
        AudioHelper.StopMusic();
        float sizeWidth = cam.orthographicSize * cam.aspect * 2;
        Instance.preUI.SetActive(true);
        Instance.postUI.gameObject.SetActive(false);
        Instance.searchUI.gameObject.SetActive(false);
        Instance.ingameUI.gameObject.SetActive(false);
        Instance.shipListPlayer.gameObject.SetActive(true);
        Instance.opponent.gameObject.SetActive(false);
        Instance.player.InitBoard(10, 10);
        float margin = Mathf.Lerp(1f / 32, 1f / 14, (cam.aspect - 1.77f) / (2.2f - 1.77f));
        Instance.player.Position = Vector3.right * (sizeWidth * (-1f / 2 + margin) + player.width / 2);
        Instance.opponent.Position = Vector3.right * (sizeWidth * (+1f / 2 - margin) - opponent.width / 2);

        Instance.counterRematchObj.transform.parent.gameObject.SetActive(false);
        Instance.btnReady.gameObject.SetActive(false);
        Instance.btnBattle.gameObject.SetActive(true);
    }
    void UpdatePregame()
    {

    }
    void EndPregame()
    {

    }
    void StartPregameReMatch()
    {
        float sizeWidth = cam.orthographicSize * cam.aspect * 2;
        Instance.preUI.SetActive(true);
        Instance.postUI.gameObject.SetActive(false);
        Instance.searchUI.gameObject.SetActive(false);
        Instance.ingameUI.gameObject.SetActive(false);
        Instance.shipListPlayer.gameObject.SetActive(true);
        Instance.opponent.gameObject.SetActive(false);
        Instance.player.InitBoard(10, 10);
        float margin = Mathf.Lerp(1f / 32, 1f / 14, (cam.aspect - 1.77f) / (2.2f - 1.77f));
        Instance.player.Position = Vector3.right * (sizeWidth * (-1f / 2 + margin) + player.width / 2);
        Instance.opponent.Position = Vector3.right * (sizeWidth * (+1f / 2 - margin) - opponent.width / 2);

        Instance.currentCounterRematch = counterRematch;
        Instance.counterRematchObj.transform.parent.gameObject.SetActive(true);
        Instance.btnReady.gameObject.SetActive(true);
        Instance.btnBattle.gameObject.SetActive(false);

        Instance.playerProfile.UpdateUIs();
        Instance.opponentProfile.UpdateUIs();
    }
    void UpdatePregameRematch()
    {
        Instance.currentCounterRematch -= Time.deltaTime;
        if (Instance.currentCounterRematch < 0)
        {
            Instance.currentCounterRematch = 0;
        }
        Instance.counterRematchObj.SetText(Instance.currentCounterRematch.ToString("F1"));
        if (Instance.currentCounterRematch <= 1 && Instance.currentCounterRematch + Time.deltaTime >= 1)
        {
            if (Instance.player.ships.Count < 10)
            {
                Instance.RandomShip();
                Instance.buttonAuto.enabled = false;
                Instance.buttonAuto.GetComponent<Image>().color = Color.gray;
            }
            DOVirtual.DelayedCall(1, () => Instance.Ready());
        }
    }
    void EndPregameRematch()
    {

    }
    Tween searchTween;
    void StartSearch()
    {
        if (GameData.Tutorial[2] == 0)
        {
            btnBackSearchGame.SetActive(false);
            GameData.Tutorial[2] = 1;
        }
        Instance.opponent.gameObject.SetActive(true);
        Instance.preUI.SetActive(false);
        Instance.searchUI.gameObject.SetActive(true);
        Instance.shipListPlayer.SetActive(false);
        WSClientHandler.SearchOpponent(bet, player.ships);
        var profile = new ProfileInfo()
        {
            Avatar = -1,
            Frame = -1,
        };
        Instance.searchUI.opponentProfile.BuildUI(profile);
    }
    void UpdateSearch()
    {

    }
    void EndSearch()
    {
        Instance.opponent.InitBoard(10, 10);
        Instance.searchUI.gameObject.SetActive(false);
    }
    void StartSearchRematch()
    {
        Instance.opponent.gameObject.SetActive(true);
        Instance.opponent.InitBoard(10, 10);
        Instance.preUI.SetActive(false);
        Instance.searchUI.gameObject.SetActive(true);
        Instance.shipListPlayer.SetActive(false);
        Instance.searchUI.opponentProfile.UpdateUIs();
        Instance.opponent.battleFieldSprite.sprite = SpriteFactory.BattleFields[GameData.Opponent.BattleField.Data];
        DOVirtual.DelayedCall(3, () => { Instance.stateMachine.CurrentState = GameState.Turn; });
    }
    void UpdateSearchRematch()
    {

    }
    void EndSearchRematch()
    {
        Instance.searchUI.gameObject.SetActive(false);
    }
    void StartTurn()
    {
        Instance.ingameUI.SetActive(true);
        Instance.TurnTime = timeInit;
        if (Instance.playerTurn)
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
        Instance.TurnTime -= Time.deltaTime;
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
            Instance.searchUI.amount.text = (GameData.Bets[bet].Bet * 1.95f).ToString();
            Instance.searchUI.gameObject.SetActive(true);
            Instance.stateMachine.CurrentState = GameState.Search;
        }
        else
        {
            PopupHelper.CreateMessage(PrefabFactory.PopupInvalidFormation, "Invalid Formation", "Please get all your ship in position", null);
        }
    }
    public void RandomShip()
    {
        Instance.player.RandomShip(Instance.shipsPlayer);
    }
    public void QuitSearch()
    {
        WSClientHandler.QuitSearch(bet);
    }
    public void QuitSearch(JSONNode data)
    {
        if (data["e"].AsInt == 0)
        {
            Instance.stateMachine.CurrentState = GameState.Pre;
            Instance.searchUI.gameObject.SetActive(false);
            Instance.preUI.SetActive(true);
            Instance.searchTween.Kill();
        }
    }
    public void QuitAfter()
    {
        Instance.postUI.gameObject.SetActive(false);
    }
    public void QuitGame()
    {
        WSClientHandler.QuitGame(roomId);
        rematch = false;
    }
    public void GoHome()
    {
        if (rematch)
        {
            QuitGame();
        }
        else
        {
            SceneTransitionHelper.Load(ESceneName.Home);
        }
    }
    public void Rematch()
    {
        rematch = true;
        Instance.rematchChatA.transform.parent.gameObject.SetActive(true);
        WSClientHandler.RequesRematch(roomId);
    }
    public void NewMatch()
    {
        rematch = false;
        SceneTransitionHelper.Reload();
        WSClientHandler.QuitGame(roomId);
    }
    public void Ready()
    {
        if (shipListPlayer.transform.childCount == 0)
        {
            rematch = false;
            Instance.searchUI.amount.text = (GameData.Bets[bet].Bet * 1.95f).ToString();
            CoinVFX.CoinVfx(Instance.searchUI.tresure.transform, Instance.searchUI.avatar1.transform.position, Instance.searchUI.avatar2.transform.position);
            WSClientHandler.SubmitShip(roomId, player.ships);
            btnReady.GetComponent<Button>().enabled = false;
            btnReady.GetComponent<Image>().color = Color.gray;
            buttonAuto.enabled = false;
            buttonAuto.GetComponent<Image>().color = Color.gray;
        }
    }
    #endregion

    #region CallBackServer
    public void Match(JSONNode json)
    {
        roomId = int.Parse(json["d"]["r"]);
        playerChair = int.Parse(json["d"]["p1"]["u"]) == DataAuth.AuthData.userId ? int.Parse(json["d"]["p1"]["c"]) : int.Parse(json["d"]["p2"]["c"]);
        Debug.Log(DataAuth.AuthData.userId + "_" + int.Parse(json["d"]["p1"]["u"]) + "_" + int.Parse(json["d"]["p2"]["u"]));
        bet = int.Parse(json["d"]["t"]);
        WSClientHandler.SubmitShip(roomId, player.ships);
        GameData.Opponent = int.Parse(json["d"]["p1"]["u"]) == DataAuth.AuthData.userId ? ProfileData.FromJsonOpponent(GameData.Opponent, json["d"]["p2"]["p"]) : ProfileData.FromJsonOpponent(GameData.Opponent, json["d"]["p1"]["p"]);
        Instance.searchUI.opponentProfile.UpdateUIs();
        Instance.opponent.battleFieldSprite.sprite = SpriteFactory.BattleFields[GameData.Opponent.BattleField.Data];
        Instance.opponentProfile.UpdateUIs();
        CoinVFX.CoinVfx(Instance.searchUI.tresure.transform, Instance.searchUI.avatar1.transform.position, Instance.searchUI.avatar2.transform.position);
        AnalyticsHelper.SpendVirtualCurrency(PConsumableType.BERRY.ToString().ToLower(), "classic" + bet.ToString());
    }
    void GameStart(JSONNode json)
    {
        Analytics.Log("startgame", new List<KeyValuePair<string, object>>());
        timeInit = json["d"]["c"].AsInt;
        Instance.playerTurn = int.Parse(json["d"]["f"]) == playerChair;
        Debug.Log(json["d"]["f"].AsInt + "_" + playerChair);
        if (Instance.stateMachine.CurrentState == GameState.PreRematch)
        {
            Instance.stateMachine.CurrentState = GameState.SearchRematch;
        }
        else
        {
            Instance.stateMachine.CurrentState = GameState.Turn;
        }
    }

    void EndTurn(JSONNode json)
    {
        Instance.playerTurn = playerChair == int.Parse(json["d"]["c"]);
        Board board = Instance.playerTurn ? Instance.opponent : Instance.player;
        int status = int.Parse(json["d"]["r"]);
        if (status != 5)
        {
            var missle = ObjectPoolManager.SpawnObject<Missle>(PrefabFactory.Missle, Vector3.zero);
            missle.Init(board.octiles[int.Parse(json["d"]["y"])][int.Parse(json["d"]["x"])].Position);
        }
        if (status == 1)
        {
            Instance.playerTurn = !Instance.playerTurn;
        }
        LeanTouch.OnFingerUp -= Instance.opponent.BeingAttacked;
        LeanTouch.OnFingerUpdate -= Instance.opponent.SelectingTarget;
        Instance.opponent.horzLine.gameObject.SetActive(false);
        Instance.opponent.vertLine.gameObject.SetActive(false);
        DOVirtual.DelayedCall(Octile.timeAttackAnim, () =>
        {
            int x = int.Parse(json["d"]["x"]);
            int y = int.Parse(json["d"]["y"]);
            Ship ship;
            int type;
            switch (status)
            {
                case 0:
                    break;
                case 1:
                    board.octiles[y][x].BeingAttacked(false);
                    break;
                case 2:
                    board.octiles[y][x].BeingAttacked(true);
                    break;
                case 3:
                    type = int.Parse(json["d"]["d"][0]);
                    if (Instance.playerTurn)
                    {
                        ship = Instantiate(PrefabFactory.Ships[type]).GetComponent<Ship>();
                        ship.board = board;
                        ship.FromJson(json["d"]["d"]);
                    }
                    else
                    {
                        ship = board.octiles[y][x].ship;
                    }
                    ship.BeingDestroyed();
                    break;
                case 4:
                    type = int.Parse(json["d"]["d"][0]);
                    if (Instance.playerTurn)
                    {
                        ship = Instantiate(PrefabFactory.Ships[type]).GetComponent<Ship>();
                        ship.board = board;
                        ship.FromJson(json["d"]["d"]);
                    }
                    else
                    {
                        ship = board.octiles[y][x].ship;
                    }
                    ship.BeingDestroyed();
                    Instance.EndGame(json);
                    break;
                case 5:
                    Debug.Log("Miss turn");
                    Instance.EndGame(json);
                    break;
                case 6:
                    Instance.EndGame(json);
                    break;
                default:
                    break;
            }
            Instance.stateMachine.CurrentState = GameState.Turn;
        });
    }
    private void TurnMiss(JSONNode data)
    {
        if (playerTurn)
        {
            Analytics.Log("turnmiss", new List<KeyValuePair<string, object>>());
            missturn++;
        }
        Instance.playerTurn = !Instance.playerTurn;
        Instance.stateMachine.CurrentState = GameState.Turn;
        if (missturn == 2)
        {
            PopupHelper.Create(PrefabFactory.PopupMissTurn);
        }
    }
    void EndGame(JSONNode json)
    {
        Instance.opponent.DestroyTutorIngame();
        Instance.rematchChatB.transform.parent.gameObject.SetActive(false);
        Instance.rematchChatA.transform.parent.gameObject.SetActive(false);
        for (int i = 0; i < json["d"]["s"].Count; i++)
        {
            int type = int.Parse(json["d"]["s"][i][0]);
            if (!Instance.opponent.destroyedShips.Find((ship) => { return ship.octilesComposition[0].pos == new Vector2Int(int.Parse(json["d"]["s"][i][2]), int.Parse(json["d"]["s"][i][3])); }))
            {
                Ship ship = Instantiate(PrefabFactory.Ships[type]).GetComponent<Ship>();
                ship.board = Instance.opponent;
                ship.FromJson(json["d"]["s"][i]);
                ship.Reveal();
            }
        }
        DOVirtual.DelayedCall(4, () =>
        {
            MusicType.END.PlayMusic();
            Debug.Log("End");
            Instance.stateMachine.CurrentState = GameState.Out;
            Instance.ingameUI.SetActive(false);
            Instance.postUI.gameObject.SetActive(true);
            Instance.postUI.amount.text = (json["d"]["e"].AsInt / 0.95f * 1.95f).ToString();
            Instance.postUI.profile1.UpdateUIs();
            Instance.postUI.profile2.UpdateUIs();

            Messenger.Broadcast(GameEvent.GAME_END, int.Parse(json["d"]["w"]) == playerChair);
            if (int.Parse(json["d"]["w"]) == playerChair)
            {
                PConsumableType.BERRY.SetValue(int.Parse(json["d"]["gw"]));
                Instance.postUI.ResultPlayer.sprite = SpriteFactory.Win;
                Instance.postUI.ResultOpponent.sprite = SpriteFactory.Lose;
                CoinVFX.CoinVfx(postUI.avatar1.transform, postUI.treasure.transform.position, postUI.treasure.transform.position);
                AnalyticsHelper.EarnVirtualCurrency(PConsumableType.BERRY.ToString().ToLower(), "classic" + bet.ToString());
                GameData.Player.Point += GameData.Bets[bet].BetRankPoint;
            }
            else
            {
                PConsumableType.BERRY.SetValue(int.Parse(json["d"]["gl"]));
                Instance.postUI.ResultPlayer.sprite = SpriteFactory.Lose;
                Instance.postUI.ResultOpponent.sprite = SpriteFactory.Win;
                CoinVFX.CoinVfx(postUI.avatar2.transform, postUI.treasure.transform.position, postUI.treasure.transform.position);
            }
        });


    }
    void EnemyOutGame(JSONNode json)
    {
        Instance.buttonRematch.GetComponent<Image>().sprite = SpriteFactory.DisableButton;
        Instance.buttonRematch.enabled = false;
        if (rematch)
        {
            rematch = false;
            if (Instance.stateMachine.CurrentState == GameState.PreRematch || Instance.stateMachine.CurrentState == GameState.SearchRematch)
            {
                SceneTransitionHelper.Load(ESceneName.Home);
            }
            else if (Instance.stateMachine.CurrentState == GameState.Out)
            {
                Instance.rematchChatB.transform.parent.gameObject.SetActive(true);
                Instance.rematchChatB.text = "SORRY I HAVE TO GO!";
            }

        }
        else
        {
            Instance.rematchChatB.transform.parent.gameObject.SetActive(true);
            Instance.rematchChatB.text = "SORRY I HAVE TO GO!";
        }

    }
    public void Reconnect(JSONNode data)
    {
        Analytics.Log("reconnect", new List<KeyValuePair<string, object>>());
        reconnect = null;
        int[,] arr = new int[Instance.player.octiles.Count, Instance.player.octiles.Count];
        Instance.opponent.gameObject.SetActive(true);
        Instance.opponent.InitBoard(10, 10);
        if (data["p1"]["u"].AsInt == DataAuth.AuthData.userId)
        {
            GameData.Opponent = ProfileData.FromJsonOpponent(GameData.Opponent, data["p2"]["p"]);
            consecutiveKill = data["p1"]["p"]["s"]["kn"].AsInt;
            if (data["p1"]["p"]["a"]["r"]["d2"].AsInt == 0)
            {
                consecutiveKillMax = new(1);
            }
            else
            {
                if (data["p1"]["p"]["a"]["r"]["d3"].AsInt == 1)
                {
                    consecutiveKillMax = new(3);
                }
                else
                {
                    consecutiveKillMax = new(2);
                }
            }
        }
        else
        {
            GameData.Opponent = ProfileData.FromJsonOpponent(GameData.Opponent, data["p1"]["p"]);
            consecutiveKill = data["p2"]["p"]["s"]["kn"].AsInt;
            if (data["p2"]["p"]["a"]["r"]["d2"].AsInt == 0)
            {
                consecutiveKillMax = new(1);
            }
            else
            {
                if (data["p2"]["p"]["a"]["r"]["d3"].AsInt == 1)
                {
                    consecutiveKillMax = new(3);
                }
                else
                {
                    consecutiveKillMax = new(2);
                }
            }
        }
        //Debug.Log("Consecutive :" + consecutiveKill + "Max " + consecutiveKillMax.Data);

        Instance.searchUI.opponentProfile.UpdateUIs();
        Instance.opponent.battleFieldSprite.sprite = SpriteFactory.BattleFields[GameData.Opponent.BattleField.Data];
        for (int i = 0; i < data["s"].Count; i++)
        {
            Ship ship = Instance.shipsPlayer.Find((ship) =>
            {
                return ship.poses.Count == int.Parse(data["s"][i][0]) + 1
                && !Instance.player.ships.Contains(ship);
            });
            ship.board = Instance.player;
            ship.FromJson(data["s"][i]);
            for (int j = 0; j < ship.octilesComposition.Count; j++)
            {
                arr[ship.octilesComposition[j].pos.y, ship.octilesComposition[j].pos.x] = 3;
            }
        }
        for (int i = 0; i < data["d"].Count; i++)
        {
            Ship ship = Instantiate(PrefabFactory.Ships[int.Parse(data["d"][i][0])]).GetComponent<Ship>();
            ship.board = Instance.opponent;
            ship.FromJson(data["d"][i]);
            ship.BeingDestroyed();
        }
        JSONNode boardPlayer;
        JSONNode boardOpponent;
        if (int.Parse(data["p1"]["u"]) == DataAuth.AuthData.userId)
        {
            boardPlayer = data["b1"];
            boardOpponent = data["b2"];
        }
        else
        {
            boardOpponent = data["b1"];
            boardPlayer = data["b2"];
        }
        //consecutiveKillMax = boardPlayer["b1"][""];
        for (int i = 0; i < boardPlayer.Count; i++)
        {
            for (int j = 0; j < boardPlayer[i].Count; j++)
            {
                if (int.Parse(boardPlayer[i][j]) == 2)
                {
                    Instance.player.octiles[j][i].BeingAttacked(false);
                }
                else if (int.Parse(boardPlayer[i][j]) == 1 || int.Parse(boardPlayer[i][j]) == 4)
                {
                    Instance.player.octiles[j][i].BeingAttacked(true);
                }
            }
        }
        for (int i = 0; i < boardOpponent.Count; i++)
        {
            for (int j = 0; j < boardOpponent[i].Count; j++)
            {
                if (int.Parse(boardOpponent[j][i]) == 2)
                {
                    Instance.opponent.octiles[i][j].BeingAttacked(false);
                }
                else if (int.Parse(boardOpponent[i][j]) == 1)
                {
                    Instance.opponent.octiles[j][i].BeingAttacked(true);
                }
            }
        }

        bet = int.Parse(data["t"]);
        roomId = int.Parse(data["r"]);
        playerChair = int.Parse(data["p1"]["u"]) == DataAuth.AuthData.userId ? int.Parse(data["p1"]["c"]) : int.Parse(data["p2"]["c"]);
        Instance.playerTurn = int.Parse(data["n"]) == playerChair;
        Instance.turnTime = int.Parse(data["c"]);
        timeInit = int.Parse(data["a"]);
        Instance.ingameUI.SetActive(true);
        Instance.preUI.SetActive(false);
        Instance.stateMachine.CurrentState = GameState.Turn;
        Debug.Log("Reconnect");
    }
    void Rematch(JSONNode data)
    {
        rematch = true;
        Instance.rematchChatB.transform.parent.gameObject.SetActive(true);
        Instance.rematchChatB.text = "I WANT TO PLAY AGAINN";
        Instance.buttonRematch.GetComponentInChildren<TextMeshProUGUI>().text = "Ready";
    }
    void AcceptRematch(JSONNode data)
    {
        rematch = true;
        counterRematch = data["d"]["c"].AsFloat;
        SceneTransitionHelper.Reload();
        Analytics.Log("rematch", new List<KeyValuePair<string, object>>());
    }
    #endregion
}

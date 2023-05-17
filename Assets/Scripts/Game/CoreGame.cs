using DG.Tweening;
using Framework;
using Lean.Touch;
using Sirenix.OdinInspector;
using System.Collections.Generic;
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
    [SerializeField] public Board player;
    [SerializeField] Board opponent;
    [SerializeField] GameObject lineRoot;
    [SerializeField] GameObject shipListPlayer;
    [SerializeField] GameObject shipListOpponent;
    [SerializeField] List<Ship> shipsPlayer;
    [SerializeField] List<Ship> shipsOpponent;
    List<List<GameObject>> lines;
    Camera cam;

    [SerializeField] GameObject preUI;
    [SerializeField] GameObject ingameUI;
    [SerializeField] GameObject searchUI;
    [SerializeField] GameObject postUI;
    public StateMachine<GameState> stateMachine;

    bool playerTurn;
    float turnTime; float TurnTime { get { return turnTime; } set { turnTime = value; turnTimeText.text = turnTime.ToString("F1"); } }
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
        base.Awake();
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
        Messenger.AddListener<bool>(GameEvent.Attack, Attack);
    }

    void Update()
    {
        stateMachine.Update();
    }
    protected override void OnDestroy()
    {
        LeanTouch.OnFingerTap -= Instance.opponent.BeingAttacked;
        LeanTouch.OnFingerTap -= Instance.player.BeingAttacked;
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
        opponent.gameObject.SetActive(true);
        opponent.RandomShip(shipsOpponent);
        preUI.SetActive(false);
        searchUI.SetActive(true);
        shipListPlayer.gameObject.SetActive(false);
        playerTurn = true;
        searchTween = DOVirtual.DelayedCall(2, () =>
        {
            stateMachine.CurrentState = GameState.Turn;
            Messenger.Broadcast(GameEvent.Game_Start);
        });
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
        Debug.Log(stateMachine.CurrentState);
        turnTime = 30;
        if (playerTurn)
        {
            Debug.Log("Player Turn");
            LeanTouch.OnFingerTap += Instance.opponent.BeingAttacked;
            Instance.turnImage.sprite = SpriteFactory.PlayerTurn;
        }
        else
        {
            Debug.Log("Opponent Turn");
            LeanTouch.OnFingerTap += Instance.player.BeingAttacked;
            Instance.turnImage.sprite = SpriteFactory.OpponentTurn;
        }
    }
    void UpdateTurn()
    {
        TurnTime -= Time.deltaTime;
        if (TurnTime<=0)
        {
            playerTurn = !playerTurn;
            stateMachine.CurrentState = GameState.Turn;
        }
    }
    void EndTurn()
    {
        Debug.Log("EndTurn");
        turnTime = 30;
        LeanTouch.OnFingerTap -= Instance.opponent.BeingAttacked;
        LeanTouch.OnFingerTap -= Instance.player.BeingAttacked;
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
    public void Attack(bool attackShip)
    {
        List<Ship> shipList;
        if (playerTurn)
        {
            shipList = Instance.opponent.ships;
            Debug.Log(opponent.ships.Count);
        }
        else
        {
            shipList = player.ships;
            Debug.Log(player.ships.Count);
        }
        if (!attackShip)
        {
            playerTurn = !playerTurn;
        }

        if (shipList.Count == 0)
        {
            Debug.Log("End");
            stateMachine.CurrentState = GameState.Out;
            Messenger.Broadcast(GameEvent.Game_End);
            Instance.ingameUI.SetActive(false);
            Instance.postUI.SetActive(true);
        }
        else
            stateMachine.CurrentState = GameState.Turn;
    }
    public void QuitSearch()
    {
        stateMachine.CurrentState = GameState.Pre;
        searchTween.Kill();
    }
    public void QuitAfter()
    {
        Instance.postUI.SetActive(false);
    }
    #endregion


}

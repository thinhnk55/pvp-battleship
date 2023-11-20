using DG.Tweening;
using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : SingletonMono<Bot>
{
    private int CountShotPlayer = 0;
    public static bool replayTutorial = false;
    [SerializeField] GameObject tutorialInGame;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        ServerMessengerFake.AddListener<JSONNode>(ServerRequest._SUBMIT_SHIP, RequestSubmitShip);
        ServerMessengerFake.AddListener<JSONNode>(ServerRequest._ATTACK, EndTurn);
        yield return new WaitForSeconds(3f);
        Match();
    }

    private void OnDestroy()
    {
        ServerMessengerFake.RemoveListener<JSONNode>(ServerRequest._SUBMIT_SHIP, RequestSubmitShip);
        ServerMessengerFake.RemoveListener<JSONNode>(ServerRequest._ATTACK, EndTurn);
    }


    #region ServerRequest
    private void RequestSubmitShip(JSONNode json)
    {
        DOVirtual.DelayedCall(2f, () =>
        {
            JSONNode startGameJson = JSONNode.Parse(GameConfig.StartJsonTuto);
            ServerMessenger.Broadcast<JSONNode>(ServerResponse._GAME_START, startGameJson);
        });
    }

    #endregion

    #region ServerRespone
    private void Match()
    {
        JSONNode json = JSONNode.Parse(GameConfig.MatchJsonTuto);
        ServerMessenger.Broadcast<JSONNode>(ServerResponse._MATCH, json);
    }

    private void EndTurn(JSONNode json)
    {
        if(CoreGame.Instance.playerTurn == true)
        {
            JSONNode jsonNode = new JSONClass();
            jsonNode = JSONNode.Parse(GameConfig.ListEndTurnJsonTuto[CountShotPlayer++]);

            if(CountShotPlayer == 4)
            {
                if (replayTutorial == false)
                {
                    int earn = (int)(GameData.Bets[0].Bet);
                    int goldWiner = PConsumableType.BERRY.GetValue() + earn;
                    //jsonNode["d"]["e"] = earn.ToString();
                    jsonNode["d"]["gw"] = goldWiner.ToString();
                }
                else
                {
                    int earn = 0;
                    int goldWiner = PConsumableType.BERRY.GetValue();
                    jsonNode["d"]["e"] = earn.ToString();
                    jsonNode["d"]["gw"] = goldWiner.ToString();
                }
            }
            ServerMessenger.Broadcast<JSONNode>(ServerResponse._END_TURN, jsonNode);
            if(CountShotPlayer == 4)
            {
                ServerMessenger.Broadcast<JSONNode>(ServerResponse._GAME_DESTROY, GameConfig.GameDestroyTuto);
            }
            //else
            //{
            //    int row = jsonNode["d"]["x"].AsInt;
            //    int collum = jsonNode["d"]["y"].AsInt;
            //    CreatePopupTutoInGame(row, collum);
            //}
        }
        else
        {
            int x = json["x"].AsInt;
            int y = json["y"].AsInt;

            JSONNode d = new JSONClass();
            d.Add("c", 1.ToJson());
            d.Add("x", json["x"]);
            d.Add("y", json["y"]);
            d.Add("r", 1.ToJson());
            JSONNode jsonEndTurn = new JSONClass()
            {
                {"id", ServerResponse._END_TURN.ToJson() },
                {"e", 0.ToJson() },
                {"d", d },
            };
            Debug.Log(jsonEndTurn);
            ServerMessenger.Broadcast<JSONNode>(ServerResponse._END_TURN, jsonEndTurn);
        }
    }

    public void CreatePopupTutoInGame()
    {
        JSONNode jsonNode = new JSONClass();
        jsonNode = JSONNode.Parse(GameConfig.ListEndTurnJsonTuto[CountShotPlayer]);
        int row = jsonNode["d"]["x"].AsInt;
        int collum = jsonNode["d"]["y"].AsInt;
        tutorialInGame = PopupHelper.Create(PrefabFactory.PopupTuTorPlay).gameObject;
        tutorialInGame.transform.position = CoreGame.Instance.opponent.octiles[collum][row].transform.position;
    }

    public void DestroyTutorialInGame()
    {
        if (tutorialInGame)
        {
            Destroy(tutorialInGame);
        }
    }
    #endregion

}

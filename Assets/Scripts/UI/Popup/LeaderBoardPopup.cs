using Framework;
using SimpleJSON;
using TMPro;
using UnityEngine;

public class LeaderBoardPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer;
    void Start()
    {
        ServerMessenger.AddListener<JSONNode>(ServerResponse._LEADER_REWARD_RECEIVE, LeaderBoardReceive);
        Timer<LeaderBoard>.Instance.Init(OnTrigger, OnElapsed);
    }
    private void OnDestroy()
    {
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._LEADER_REWARD_RECEIVE, LeaderBoardReceive);
    }
    public void OnTrigger()
    {
        timer.text = "Ended";
    }
    public void OnElapsed()
    {
        timer.text = "End in: " + Timer<LeaderBoard>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
    }
    void Update()
    {
        Timer<LeaderBoard>.Instance.Elasping();
    }
    public void LeaderBoardReceive()
    {

    }
    public void LeaderBoardReceive(JSONNode data)
    {

    }
}

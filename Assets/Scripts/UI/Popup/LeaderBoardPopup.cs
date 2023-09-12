using Framework;
using TMPro;
using UnityEngine;

public class LeaderBoardPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer;
    void Start()
    {
        Timer<LeaderBoard>.Instance.Init(OnTrigger, OnElapsed);
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
}

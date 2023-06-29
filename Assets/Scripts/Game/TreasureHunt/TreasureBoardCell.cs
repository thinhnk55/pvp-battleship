using UnityEngine;
using UnityEngine.UI;

public class TreasureBoardCell : MonoBehaviour
{
    [SerializeField] Image cellImage;
    [SerializeField] Image cellImage2;

    int x, y;

    bool shot;

    public bool IsShot { get => shot; }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(TryShoot);
    }

    public void InitCell(int _x, int _y)
    {
        x = _x;
        y = _y;
        SetIsShot(false);
    }

    public void SetIsShot(bool shot)
    {
        this.shot = shot;
        if (cellImage != null) cellImage.gameObject.SetActive(!shot);
        if (cellImage2 != null) cellImage2.gameObject.SetActive(shot);
    }

    public void PlayShootAnim()
    {
        
    }

    public void TryShoot()
    {
        if (!IsShot)
            TreasureHuntManager.Instance.TryShootCell(x, y);
    }
}

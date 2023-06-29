using UnityEngine;
using UnityEngine.UI;

public class TreasureBoardCell : MonoBehaviour
{
    [SerializeField] Image cellImage;

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
        cellImage.gameObject.SetActive(!shot);
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

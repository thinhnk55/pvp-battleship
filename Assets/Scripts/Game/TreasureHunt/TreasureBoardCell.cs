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
        SetNotShot();
    }

    public void SetNotShot()
    {
        shot = false;
    }

    public void Shoot()
    {
        shot = true;
    }

    public void TryShoot()
    {
        TreasureHuntManager.Instance.TryShootCell(x, y);
    }
}

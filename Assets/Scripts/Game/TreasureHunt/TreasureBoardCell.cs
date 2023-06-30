using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
        if (shootAnimCoroutine != null) StopCoroutine(shootAnimCoroutine);
        if (cellImage != null) cellImage.gameObject.SetActive(!shot);
        if (cellImage2 != null) cellImage2.gameObject.SetActive(shot);
    }

    public void PlayShootAnim()
    {
        this.shot = true;
        shootAnimCoroutine = StartCoroutine(RunShootAnim());
    }

    Coroutine shootAnimCoroutine;

    IEnumerator RunShootAnim()
    {
        if (cellImage != null) cellImage.gameObject.SetActive(false);
        if (cellImage2 != null)
        {
            cellImage2.gameObject.SetActive(false);
            yield return new WaitForSeconds(.2f);
            cellImage2.gameObject.SetActive(true);
            cellImage2.transform.localScale = Vector3.zero;
            cellImage2.transform.DOScale(Vector2.one, .6f).SetEase(Ease.OutQuart);
        }
    }

    public void TryShoot()
    {
        if (!IsShot)
            TreasureHuntManager.Instance.TryShootCell(x, y);
    }
}

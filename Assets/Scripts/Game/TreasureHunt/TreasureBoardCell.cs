using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Framework;

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

    public void PlayShootAnim(bool treasureHit)
    {
        this.shot = true;
        shootAnimCoroutine = StartCoroutine(RunShootAnim());
        cellImage2.sprite = treasureHit ? SpriteFactory.ShipLuckyShot : SpriteFactory.X;
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

    public void ResetCell()
    {
        if (shot)
        {
            shot = false;
            StartCoroutine(PlayResetAnim(1.1f));
        }
    }

    IEnumerator PlayResetAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (cellImage != null) cellImage.gameObject.SetActive(false);
        if (cellImage2 != null)
        {
            cellImage2.gameObject.SetActive(true);
            cellImage2.transform.DOScale(Vector2.zero, .3f).SetEase(Ease.InQuart);
            yield return new WaitForSeconds(.3f);
            cellImage2.gameObject.SetActive(false);
        }
        if (cellImage != null)
        {
            cellImage.gameObject.SetActive(true);
            cellImage.transform.localScale = Vector3.zero;
            cellImage.transform.DOScale(Vector2.one, .6f).SetEase(Ease.OutQuart);
        }
    }

    public void TryShoot()
    {
        if (!IsShot)
            TreasureHuntManager.Instance.TryShootCell(x, y);
    }
}

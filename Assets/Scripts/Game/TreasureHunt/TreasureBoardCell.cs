using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Framework;

public class TreasureBoardCell : MonoBehaviour
{
    [SerializeField] Image cellImage;
    [SerializeField] Image cellIcon;

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
        //if (cellImage != null) cellImage.gameObject.SetActive(!shot);
        if (cellIcon != null) cellIcon.gameObject.SetActive(shot);
        if (cellIcon != null) cellIcon.sprite = shot ? SpriteFactory.X : SpriteFactory.QuestionMark;
    }

    public void PlayShootAnim(bool treasureHit)
    {
        this.shot = true;
        shootAnimCoroutine = StartCoroutine(RunShootAnim(treasureHit));
        
    }

    Coroutine shootAnimCoroutine;

    IEnumerator RunShootAnim(bool treasureHit)
    {
        if (cellImage != null) 
            cellImage.transform.DORotate(new Vector3(0, -90f, 0), 0.4f, RotateMode.FastBeyond360).OnComplete(() => 
            {
                cellImage.transform.localEulerAngles = new Vector3(0, 90f, 0);
                cellImage.transform.DORotate(Vector3.zero, 0.4f, RotateMode.FastBeyond360);
            }
            );
        if (cellIcon != null)
        {
            yield return new WaitForSeconds(.4f);
            //cellIcon.transform.localScale = Vector3.zero;
            //cellIcon.transform.DOScale(Vector2.one, .6f).SetEase(Ease.OutQuart);
            cellIcon.sprite = treasureHit ? SpriteFactory.ShipLuckyShot : SpriteFactory.X;
            cellIcon.gameObject.SetActive(true);
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
        //if (cellImage != null) cellImage.gameObject.SetActive(false);
        //if (cellIcon != null)
        //{
        //    cellIcon.gameObject.SetActive(true);
        //    cellIcon.transform.DOScale(Vector2.zero, .3f).SetEase(Ease.InQuart);
        //    yield return new WaitForSeconds(.3f);
        //    cellIcon.gameObject.SetActive(false);
        //}
        //if (cellImage != null)
        //{
        //    cellImage.gameObject.SetActive(true);
        //    cellImage.transform.localScale = Vector3.zero;
        //    cellImage.transform.DOScale(Vector2.one, .6f).SetEase(Ease.OutQuart);
        //}

        if (cellImage != null)
            cellImage.transform.DORotate(new Vector3(0, -90f, 0), 0.4f, RotateMode.FastBeyond360).OnComplete(() =>
            {
                cellImage.transform.localEulerAngles = new Vector3(0, 90f, 0);
                cellImage.transform.DORotate(Vector3.zero, 0.4f, RotateMode.FastBeyond360);
            }
            );
        if (cellIcon != null)
        {
            yield return new WaitForSeconds(.4f);
            //cellIcon.transform.localScale = Vector3.zero;
            //cellIcon.transform.DOScale(Vector2.one, .6f).SetEase(Ease.OutQuart);
            cellIcon.sprite = SpriteFactory.QuestionMark;
            cellIcon.gameObject.SetActive(false);
        }
    }

    public void TryShoot()
    {
        if (!IsShot)
            TreasureHuntManager.Instance.TryShootCell(x, y);
    }
}

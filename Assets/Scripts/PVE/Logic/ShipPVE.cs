using DG.Tweening;
using Framework;
using Lean.Common;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShipPVE : MonoBehaviour
{
    [SerializeField] TextMeshPro pointTxt;
    [SerializeField] GameObject targetImgage; //huong dan nguoi choi nhap vao tau dich de ban, chi xuat hien turn dau
    public LeanSelectableByFinger leanSelectable;
    public PDataUnit<int> point;
    public int index;
    Tween tweenPoint;
    [SerializeField] ParticleSystem destroyedVFX;

    private void Awake()
    {
        leanSelectable = GetComponent<LeanSelectableByFinger>();
        point = new PDataUnit<int>(0);
        point.OnDataChanged += (o,n) => 
        {
            if(index == -1) // Tau player
            {
                tweenPoint = DOTween.To(() => int.Parse(pointTxt.text),
                                        (value) => pointTxt.text = value.ToString(), n, 1)
                                            .OnComplete(() => pointTxt.text = n.ToString());
            }
            else
            {
                pointTxt.text = n.ToString();
            }
        };
    }
    public void HidePoint()
    {
        pointTxt.text = "????";
    }
    public void ShowPoint()
    {
        pointTxt.text = point.Data.ToString();
    }
    public void OnSelectShip()
    {
        PVE.Instance.SetDisableLeanSelectableShipEnemy(false);
        PVE.Instance.selectedEnemy = index;
        PVE.Instance.Attack();

        if (PVE.Instance.CurrentStep.Data == 0)
        {
            scaleTargetTween.Kill();
            targetImgage.SetActive(false);
        }
    }
    public IEnumerator BeingDestroyed()
    {
        ObjectPoolManager.SpawnObject<Missle>(PrefabFactory.Missle, Vector3.zero).Init(transform.position);
        yield return new WaitForSeconds(1);
        destroyedVFX.Play();
        yield return new WaitForSeconds(2f);
        if(index != -1)  // Khong phai tau player
        {
            Destroy(destroyedVFX.gameObject);
        }
        destroyedVFX.transform.parent = null;
    }

    Tween scaleTargetTween;
    public void ScaleTargetImage()
    {
        targetImgage.SetActive(true);
        scaleTargetTween = targetImgage.transform.DOScale(Vector3.one * 0.8f, 0.75f).OnComplete(() =>
        {
            Tween sequence = DOTween.Sequence()
                .Insert(0, targetImgage.transform.DOScale(Vector3.one * 1.0f, 0.75f))
                .Insert(0.75f, targetImgage.transform.DOScale(Vector3.one * 0.8f, 0.75f))
                .SetLoops(-1).Play();
        });
    }
}

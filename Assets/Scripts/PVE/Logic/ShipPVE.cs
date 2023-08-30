using Framework;
using Lean.Common;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class ShipPVE : MonoBehaviour
{
    [SerializeField] TextMeshPro pointTxt;
    public LeanSelectableByFinger leanSelectable;
    public PDataUnit<int> point;
    public int index;


    [SerializeField] ParticleSystem destroyedVFX;

    private void Awake()
    {
        leanSelectable = GetComponent<LeanSelectableByFinger>();
        point = new PDataUnit<int>(0);
        point.OnDataChanged += (o,n) => pointTxt.text = n.ToString();
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
        leanSelectable.enabled = false;
        PVE.Instance.selectedEnemy = index;
        PVE.Instance.Attack();
    }
    public IEnumerator BeingDestroyed()
    {
        ObjectPoolManager.SpawnObject<Missle>(PrefabFactory.Missle).Init(transform.position);
        yield return new WaitForSeconds(1);
        destroyedVFX.Play();
        destroyedVFX.transform.parent = null;
    }

    private void OnDestroy()
    {
        //Destroy(destroyedVFX.gameObject);
    }
}

using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class SetTabOpenPopup : MonoBehaviour
    {
        public int i;
        private void Awake()
        {
            GetComponent<ButtonOpenPopup>().OnSpawnPopup.AddListener(SetTab);
        }
        private void OnDestroy()
        {
            GetComponent<ButtonOpenPopup>().OnSpawnPopup.RemoveListener(SetTab);
        }
        public void SetTab(PopupBehaviour popup)
        {
            popup.GetComponentInChildren<Tabs>().Activate(i);
        }
    }
}

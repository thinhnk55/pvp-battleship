using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class SetTabPopup : MonoBehaviour
    {

        public int i;
        public void SetTab(PopupBehaviour popup)
        {
            popup.GetComponentInChildren<Tabs>().Activate(i);
        }
    }
}

using UnityEngine;

namespace Framework
{
    public class SetTabOpenPopup : MonoBehaviour
    {
        public int tab;
        public PopupBehaviour popup;
        private void Awake()
        {
            popup = GetComponent<PopupBehaviour>();
            GetComponent<ButtonOpenPopup>().OnSpawnPopup.AddListener(SetTab);
        }
        private void OnDestroy()
        {
            GetComponent<ButtonOpenPopup>().OnSpawnPopup.RemoveListener(SetTab);
        }
        public void SetTab(PopupBehaviour popup)
        {
            popup?.GetComponentInChildren<Tabs>().Activate(tab);
        }

        public void SetTab(PopupBehaviour popup, int tab)
        {
            popup?.GetComponentInChildren<Tabs>().Activate(tab);
        }
    }
}

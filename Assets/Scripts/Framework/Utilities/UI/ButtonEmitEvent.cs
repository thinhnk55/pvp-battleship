using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class ButtonEmitEvent : ButtonBase
    {
        [SerializeField] GameEvent _event;
        [SerializeField] string args;

        protected override void Button_OnClicked()
        {
            base.Button_OnClicked();

            Messenger.Broadcast<JSONNode>(_event, JSONNode.Parse(args));
        }

        protected virtual void HandleSpawnPopup(PopupBehaviour popupBehaviour)
        {

        }
    }
}


using TMPro;
using UnityEngine;

namespace Framework
{
    public class TextBase : MonoBehaviour
    {
        public TMP_Text text;
        protected virtual void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
            if ( text == null)
            {
                text = GetComponent<TextMeshPro>();
            }
        }
    }
}

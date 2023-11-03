using Framework;
using UnityEngine;

namespace FirebaseIntegration
{
    public class FirebaseData : PDataBlock<FirebaseData>
    {
        [SerializeField] public bool isNewTokenCloudMessage; public static bool IsNewTokenCloudMessage { get { return Instance.isNewTokenCloudMessage; } set { Instance.isNewTokenCloudMessage = value; } }
        [SerializeField] public string tokenCloudMessage; public static string TokenCloudMessage { get { return Instance.tokenCloudMessage; } set { Instance.tokenCloudMessage = value; } }
        // Start is called before the first frame update
        protected override void Init()
        {
            base.Init();
        }
    }
}

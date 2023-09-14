using Framework;
using UnityEngine;

namespace FirebaseIntegration
{
    public class FirebaseData : PDataBlock<FirebaseData>
    {
        [SerializeField] private string tokenCloudMessage; public static string TokenCloudMessage { get { return Instance.tokenCloudMessage; } set { Instance.tokenCloudMessage = value; } }
        // Start is called before the first frame update
        protected override void Init()
        {
            base.Init();
        }
    }
}

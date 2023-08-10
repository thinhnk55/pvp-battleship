using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLogin : MonoBehaviour
{
    /* Flow auto login
     * 
     * B1: Send access token -> server
     * B2: if (token expire)
     *     then 
     *          Send refresh token -> server for get access token
     *              if(server return access token -> B1)
     *              else
     *                  return login screen
     *     else
     *          -> Main Screen
     */
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

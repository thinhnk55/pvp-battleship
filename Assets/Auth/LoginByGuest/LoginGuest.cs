using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;



public  class LoginGuest : ISocialAuth
{
    public const long TOKEN_EXPIRED_TIME = 1 * 24 * 60 * 60 * 1000;
    public const string SECRET_KEY = "WeDonate10000$HzieKL";


    public string GenerateToken(string device)
    {
        long expiredTime = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + TOKEN_EXPIRED_TIME) / 1000;
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "sub", device },
            { "exp", expiredTime }
        };
        string signature = Sign(device, expiredTime);
        data.Add("kid", signature);
        
        string id_token = UnityEngine.Purchasing.MiniJSON.Json.Serialize(data);
        string endcode = Convert.ToBase64String(Encoding.UTF8.GetBytes(id_token));
        return endcode;
    }

/*    public  string VerifyToken(string token)
    {
        string decode = Encoding.UTF8.GetString(Convert.FromBase64String(token));
        Dictionary<string, object> data = MiniJSON.Json.Deserialize(decode) as Dictionary<string, object>;
        string device = data["sub"] as string;
        long expiredTime = Convert.ToInt64(data["exp"]);
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        if (now > expiredTime)
        {
            return null;
        }
        string mySignature = Sign(device, expiredTime);
        string signature = data["kid"] as string;
        if (mySignature != signature)
        {
            return null;
        }
        return device;
    }*/

    private string Sign(string device, long expiredTime)
    {
        string data = SECRET_KEY + device + expiredTime;
        string signature = SHA256Hash.GetSHA256Hash(data);
        return signature;
    }

    public void Initialize()
    {

    }

    public void Update()
    {
        throw new NotImplementedException();
    }

    public void SignUp()
    {
        throw new NotImplementedException();
    }

    public void SignIn()
    {
        Debug.Log("Login By Guest");
        HTTPClient.Instance.LoginByGuest(GenerateToken(SystemInfo.deviceUniqueIdentifier));
    }

    public void SignOut()
    {
        throw new NotImplementedException();
    }

    /*    private static string ComputeSHA256Hash(string rawData)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(rawData);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }*/
}
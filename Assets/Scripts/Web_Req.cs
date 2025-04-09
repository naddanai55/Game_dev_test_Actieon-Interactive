using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Web_Req : MonoBehaviour
{
    public class Logindata
    {
        public string status;
        public string message;
        public int diamond;
        public int heart;
    }
    void Start()
    {
        // StartCoroutine(Signup("nainai123", "nai123"));
    }

    public IEnumerator Login(string username, string password, System.Action<string, int, int> onLoginResult = null)
    {
        Debug.Log($"Logging in with username: {username} and password: {password}");
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using UnityWebRequest www = UnityWebRequest.Post("http://localhost/signup_test/login.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            onLoginResult?.Invoke("Network error", 0, 0);
        }
    
        else
        {
            Debug.Log(www.downloadHandler.text);
            Logindata response = JsonUtility.FromJson<Logindata>(www.downloadHandler.text);
            if (response.status == "success")
            {
                onLoginResult?.Invoke(response.message, response.diamond, response.heart);
            }
            else if (response.message == "User not found")
            {
                onLoginResult?.Invoke("User not found. Please check your username.", 0, 0);
            }
            else if (response.message == "Invalid password")
            {
                onLoginResult?.Invoke("Invalid password. Please try again.", 0, 0);
            }
            else
            {
                // Handle any other status that might occur
                onLoginResult?.Invoke("Login failed: " + response.message, 0, 0);
            }
        }
    }
    public IEnumerator Signup(string username, string password, System.Action<string> onMessage)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using UnityWebRequest www = UnityWebRequest.Post("http://localhost/signup_test/signup.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            onMessage?.Invoke("Signup failed!");
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            onMessage?.Invoke("Signup successful!");

        }
    }
}

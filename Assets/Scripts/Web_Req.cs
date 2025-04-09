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
        public int user_id;
    }

    public class AddDiamondResponse
    {
        public string status;
        public string message;
        public int diamond;
    }

    void Start()
    {
        // StartCoroutine(Signup("nainai123", "nai123"));
    }

    public IEnumerator Login(string username, string password, System.Action<string, int, int, int> onLoginResult = null)
    {
        Debug.Log($"Logging in with username: {username} and password: {password}");
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using UnityWebRequest www = UnityWebRequest.Post("https://test-piggy.codedefeat.com/worktest/dev01/login.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            onLoginResult?.Invoke("Network error", 0, 0, 0);
        }
    
        else
        {
            Debug.Log(www.downloadHandler.text);
            Logindata response = JsonUtility.FromJson<Logindata>(www.downloadHandler.text);
            if (response.status == "success")
            {
                int user_id = response.user_id;
                onLoginResult?.Invoke(response.message, response.diamond, response.heart, user_id);
            }
            else if (response.message == "User not found")
            {
                onLoginResult?.Invoke("User not found. Please check your username.", 0, 0, 0);
            }
            else if (response.message == "Invalid password")
            {
                onLoginResult?.Invoke("Invalid password. Please try again.", 0, 0, 0);
            }
            else
            {
                // Handle any other status that might occur
                onLoginResult?.Invoke("Login failed: " + response.message, 0, 0, 0);
            }
        }
    }

    
    public IEnumerator Signup(string username, string password, System.Action<string> onMessage)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        using UnityWebRequest www = UnityWebRequest.Post("https://test-piggy.codedefeat.com/worktest/dev01/signup.php", form);
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

    public IEnumerator AddDiamond(int user_id, System.Action<string, int> onResult = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", user_id);

        using UnityWebRequest www = UnityWebRequest.Post("https://test-piggy.codedefeat.com/worktest/dev01/add_diamond.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            onResult?.Invoke("Network error", 0);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            var result = JsonUtility.FromJson<AddDiamondResponse>(www.downloadHandler.text);
            if (result.status == "success")
            {
                onResult?.Invoke("Diamond added!", result.diamond);
            }
            else
            {
                onResult?.Invoke("Error: " + result.message, 0);
            }
        }
    }
}

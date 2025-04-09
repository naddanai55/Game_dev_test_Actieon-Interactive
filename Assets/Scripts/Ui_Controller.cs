using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class Ui_Controller : MonoBehaviour
{
    public GameObject loginPanel, signupPanel, msgPanel, lobbyPanel;

    public TMP_InputField usernameInput, passwordInput, signupUsernameInput, signupPasswordInput, confirmPasswordInput;

    public Button loginButton, signupButton_onLogin, signupButton_onSignup, OkButton, startButton, addDiamondButton;

    public TMP_Text msgText, diamondText;

    public Slider HeartSlider;

    private int user_id;

    void Start()
    {
        signupButton_onLogin.onClick.AddListener(() =>
        {
            OpenSignupPanel();
        });

        signupButton_onSignup.onClick.AddListener(() =>
        {
            OpenLoginPanel();
        });

        OkButton.onClick.AddListener(() =>
        {
            if (msgText.text == "Passwords do not match.")
            {
                OpenSignupPanel();
            }
            else
            {
                OpenLoginPanel();
            }
        });

        loginButton.onClick.AddListener(() =>
        {
            StartCoroutine(Main.Instance.webReq.Login(usernameInput.text, passwordInput.text, (message, diamond, heart, user_id) =>
            {
                msgText.text = message;

                if (message == "Login successful")
                {
                    this.user_id = user_id;
                    OpenLobbyPanel(diamond, heart);
                }
                else
                {
                    OpenMsgPanel();
                }
            }));
        });

        signupButton_onSignup.onClick.AddListener(() =>
        {
            if (signupPasswordInput.text == confirmPasswordInput.text)
            {
                StartCoroutine(Main.Instance.webReq.Signup(signupUsernameInput.text, signupPasswordInput.text, (message) =>
                {
                    msgText.text = message;
                    OpenMsgPanel();
                }));
            }
            else
            {
                msgText.text = "Passwords do not match.";
                OpenMsgPanel();
            }
        });

        addDiamondButton.onClick.AddListener(() =>
        {
            // Call AddDiamond method when the Add Diamond button is clicked
            StartCoroutine(Main.Instance.webReq.AddDiamond(this.user_id, (message, newDiamondCount) =>
            {
                Debug.Log("Logged in with user ID: " + this.user_id);
                diamondText.text = $"{newDiamondCount}";
            }));
        });
    }

    public void OpenLoginPanel()
    {

        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        msgPanel.SetActive(false);
        lobbyPanel.SetActive(false);
    }
    public void OpenSignupPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(true);
        msgPanel.SetActive(false);
        lobbyPanel.SetActive(false);
    }
    public void OpenMsgPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        msgPanel.SetActive(true);
        lobbyPanel.SetActive(false);
    }
    public void OpenLobbyPanel(int diamond = 1000, int heart = 100)
    {
        diamondText.text = $"{diamond}";
        HeartSlider.value = heart;

        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        msgPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }
}
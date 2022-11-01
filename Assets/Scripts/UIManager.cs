using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using Firebase.Extensions;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject PasswordMenu;
    public GameObject SignUpMenu;
    public GameObject SettingsMenu;
    //Settings

    //MainMenu
    public TMP_InputField UsernameTextfield;
    public TMP_InputField PasswordTextfield;
    //PasswordMenu
    //SignUpMenu
    public TMP_InputField CreateUsernameTextfield;
    public TMP_InputField CreatePasswordTextfield;
    //FireBase
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    protected FirebaseAuth auth;
    void Start()
    {
        PasswordMenu.SetActive(false);
        MainMenu.SetActive(true);
        SignUpMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        PasswordTextfield.contentType = TMP_InputField.ContentType.Password;
        CreatePasswordTextfield.contentType = TMP_InputField.ContentType.Password;
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });

    }

    public void OnOpenSettingsMenu()
    {
        CloseAllMenus();
        SettingsMenu.SetActive(true);
    }
    public void OnOpenSignUpMenu()
    {
        CloseAllMenus();
        SignUpMenu.SetActive(true);
    }
    public void OnOpenPasswordMenu()
    {
        CloseAllMenus();
        PasswordMenu.SetActive(true);
    }
    public void OnBacktoMainMenu()
    {
        CloseAllMenus();
        MainMenu.SetActive(true);
    }
    public void CloseAllMenus()
    {
        PasswordMenu.SetActive(false);
        MainMenu.SetActive(false);
        SignUpMenu.SetActive(false);
        SettingsMenu.SetActive(false);
    }
    protected void InitializeFirebase()
    {

        auth = FirebaseAuth.DefaultInstance;

    }
    public void onSignUp()
    {
        string email = CreateUsernameTextfield.text;
        email = email.Replace(" ", "");
        string password = CreatePasswordTextfield.text;
        password = password.Replace(" ", "");
        bool works = false;
        Firebase.Auth.FirebaseUser newUser = null;
        Task x = auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
             newUser = task.Result;
            works = true;
        
        });
       
        CreateUsernameTextfield.text = "";
        CreatePasswordTextfield.text = "";

        OnBacktoMainMenu();
    }
    public void OnSignIn()
    {
        string email = UsernameTextfield.text;//pgat3000@gmail.com
        email = email.Replace(" ", "");


        string password = PasswordTextfield.text;//123456
        password = password.Replace(" ", "");
        Task x = auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using Huawei.Agconnect;
using Huawei.Agconnect.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleFileBrowser;
public class AGC : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text email, password;
    void Start()
    {
        Stream inputStream = new MemoryStream(Resources.Load<TextAsset>("agconnect-services").bytes);
        AGConnectInstance.Initialize(new AGConnectOptionsBuilder()
            .SetInputStream(inputStream).SetPersistPath(new DirectoryInfo(Application.persistentDataPath)));
        if (AGConnectAuth.Instance.GetCurrentUser() != null)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
   

    public void LoadScene()
    {
        if (AGConnectAuth.Instance.GetCurrentUser() != null)
        {
           AGConnectAuth.Instance.SignOut();
        }
        SceneManager.LoadScene("SampleScene");
    }
    
    public async void Login()
    {
        if (AGConnectAuth.Instance.GetCurrentUser() == null)
        {
            try
            {
                IAGConnectAuthCredential credential = EmailAuthProvider.CredentialWithPassword(email.text, password.text);
                var result = await AGConnectAuth.Instance.SignInAsync(credential);
                SceneManager.LoadScene("SampleScene");

            }
            catch (AGCAuthException ex)
            {
                Debug.LogError("Login Failed: " + ex.ErrorMessage);
              
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Login Failed: " + ex.Message);
               
            }
        }
        else
        {
            Debug.LogError("User Already Logged In");
            AGConnectAuth.Instance.SignOut();
        }
    }
    public async void LoginAnon()
    {
        if (AGConnectAuth.Instance.GetCurrentUser() == null)
        {
            try
            {
                await AGConnectAuth.Instance.SignInAnonymouslyAsync();
                SceneManager.LoadScene("SampleScene");

            }
            catch (AGCAuthException ex)
            {
                Debug.LogError("Login Failed: " + ex.ErrorMessage);
              
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Login Failed: " + ex.Message);
               
            }
           
        }
        else
        {
            Debug.LogError("User Already Logged In");
            AGConnectAuth.Instance.SignOut();
        }
    }
    

}

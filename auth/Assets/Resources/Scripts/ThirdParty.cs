/*
 * Copyright 2022. Huawei Technologies Co., Ltd. All rights reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Facebook.Unity;
using Huawei.Agconnect.Auth;
using System.Threading.Tasks;
using Huawei.Agconnect.AGCException;
using UnityEngine.UI;

public class ThirdParty : MonoBehaviour
{
    public Text userText;
    public GameObject menupanel;

    // Activate this part after adding facebook unity sdk
    //void Awake()
    //{
    //    if (!FB.IsInitialized)
    //    {
    //        // Initialize the Facebook SDK
    //        FB.Init(InitCallback, OnHideUnity);
    //    }
    //    else
    //    {
    //        // Already initialized, signal an app activation App Event
    //        FB.ActivateApp();
    //    }
    //}

    //public void FacebookLogin()
    //{
    //    var perms = new List<string>() { "public_profile", "email" };
    //    FB.LogInWithReadPermissions(perms, AuthCallback);
    //}

    //private async void AuthCallback(ILoginResult result)
    //{
    //    if (FB.IsLoggedIn)
    //    {
    //        // AccessToken class will have session details
    //        var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
    //        // Print current access token's User ID

          
    //        IAGConnectAuthCredential credential = FacebookAuthProvider.CredentialWithToken(aToken.TokenString);
    //        Task<ISignInResult> signInResult = AGConnectAuth.Instance.SignInAsync(credential);
    //        try
    //        {
    //            await signInResult;
    //            userText.text=signInResult.Result.GetUser().GetDisplayName();

    //        }
    //        catch (System.Exception)
    //        {

    //            if (signInResult.Exception.InnerException is AGCException exception)
    //                Debug.Log(exception.ErrorMessage);
    //            else
    //                Debug.Log(signInResult.Exception.ToString());
    //        }
           
    //    }
    //    else
    //    {
    //        Debug.Log("User cancelled login");
    //    }

       
    //    menupanel.SetActive(true);
    //}

    //private void InitCallback()
    //{
    //    if (FB.IsInitialized)
    //    {
    //        // Signal an app activation App Event
    //        FB.ActivateApp();
    //        // Continue with Facebook SDK
    //        // ...
    //    }
    //    else
    //    {
    //        Debug.Log("Failed to Initialize the Facebook SDK");
    //    }
    //}

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public async void HwIdSignIn()
    {
        if (AGConnectAuth.Instance.GetCurrentUser() != null)
        {
            string accessToken = "accessToken";
            IAGConnectAuthCredential credential = HwIdAuthProvider.CredentialWithToken(accessToken);
            Task<ISignInResult> hwSignInTast = AGConnectAuth.Instance.SignInAsync(credential);
            try
            {
                await hwSignInTast;
            }
            catch (System.Exception)
            {

                if (hwSignInTast.Exception.InnerException is AGCException exception)
                    Debug.Log(exception.ErrorMessage);
                else
                    Debug.Log(hwSignInTast.Exception.InnerException.ToString());
            }
        }
    }

    public async void HwGameSignIn()
    {
        if (AGConnectAuth.Instance.GetCurrentUser() != null)
        {
            string accessToken = "accessToken";
            IAGConnectAuthCredential credential = HWGameAuthProvider.CredentialWithToken(accessToken);
            Task<ISignInResult> hwSignInTask = AGConnectAuth.Instance.SignInAsync(credential);
            try
            {
                await hwSignInTask;
            }
            catch (System.Exception)
            {

                if (hwSignInTask.Exception.InnerException is AGCException exception)
                    Debug.Log(exception.ErrorMessage);
                else
                    Debug.Log(hwSignInTask.Exception.InnerException.ToString());
            }
        }
    }

    public async void WeChatSignIn()
    {
        if (AGConnectAuth.Instance.GetCurrentUser() != null)
        {
            string accessToken = "accessToken";
            string openId = "";
            IAGConnectAuthCredential credential = WeixinAuthProvider.CredentialWithToken(accessToken, openId);
            Task<ISignInResult> hwSignInTask = AGConnectAuth.Instance.SignInAsync(credential);
            try
            {
                await hwSignInTask;
            }
            catch (System.Exception)
            {

                if (hwSignInTask.Exception.InnerException is AGCException exception)
                    Debug.Log(exception.ErrorMessage);
                else
                    Debug.Log(hwSignInTask.Exception.InnerException.ToString());
            }
        }
    }

    public async void LoginWithTwitter()
    {
        string accessToken = "accessToken";
        string secret = "secret";
        IAGConnectAuthCredential credential = TwitterAuthProvider.CredentialWithToken(accessToken, secret);
        Task<ISignInResult> twSignInTask = AGConnectAuth.Instance.SignInAsync(credential);
        try
        {
            await twSignInTask;
        }
        catch (System.Exception)
        {

            if (twSignInTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(twSignInTask.Exception.InnerException.ToString());
        }
    }

    public async void LoginWithWeibo()
    {
        string accessToken = "accessToken";
        string uuid = "uuid";
        IAGConnectAuthCredential credential = WeiboAuthProvider.CredentialWithToken(accessToken, uuid);
        Task<ISignInResult> weiboSignInTask = AGConnectAuth.Instance.SignInAsync(credential);
        try
        {
            await weiboSignInTask;
        }
        catch (System.Exception)
        {

            if (weiboSignInTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(weiboSignInTask.Exception.InnerException.ToString());
        }
    }

    public async void LoginWithQQ()
    {
        string accessToken = "accessToken";
        string openId = "openid";
        IAGConnectAuthCredential credential = QQAuthProvider.CredentialWithToken(accessToken, openId);
        Task<ISignInResult> qqSignInTask = AGConnectAuth.Instance.SignInAsync(credential);
        try
        {
            await qqSignInTask;
        }
        catch (System.Exception)
        {

            if (qqSignInTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(qqSignInTask.Exception.InnerException.ToString());
        }
    }

    public async void LoginWithGoogle()
    {
        string accessToken ="token";
        IAGConnectAuthCredential credential = GoogleAuthProvider.CredentialWithToken(accessToken);
        Task<ISignInResult> googleSignInTask = AGConnectAuth.Instance.SignInAsync(credential);
        try
        {
            await googleSignInTask;
        }
        catch (System.Exception)
        {

            if (googleSignInTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(googleSignInTask.Exception.InnerException.ToString());
        }
    }

    public async void LoginWithGoogleGame()
    {
        string serverAuthCode = "serverauthcode";
        IAGConnectAuthCredential credential = GoogleGameAuthProvider.CredentialWithToken(serverAuthCode);
        Task<ISignInResult> googleSignInTask = AGConnectAuth.Instance.SignInAsync(credential);
        try
        {
            await googleSignInTask;
        }
        catch (System.Exception)
        {

            if (googleSignInTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(googleSignInTask.Exception.InnerException.ToString());
        }
    }
}

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

using Huawei.Agconnect;
using Huawei.Agconnect.AGCException;
using Huawei.Agconnect.Auth;
using Huawei.Agconnect.Auth.Internal.Storage;
using Huawei.Agconnect.Core.Service.Auth;
using Huawei.Agconnect.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Text codeText, passwordText, emailText, userText;
    public Text passwordLoginText, emailLoginText;
    public Text passwordResetText, emailResetText, codeResetText;
    public Text passwordChangeText, emailChangeText, codeChangeText;
    public Text newEmailText, codeEmailChangeText;
    public Text passwordLoginCodeText, emailLoginCodeText, codeLoginCodeText;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            Stream inputStream = new MemoryStream(Resources.Load<TextAsset>("agconnect-services").bytes);
            AGConnectInstance.Initialize(new AGConnectOptionsBuilder()
                .SetInputStream(inputStream).SetPersistPath(new DirectoryInfo(Application.persistentDataPath)));
            AGConnectAuth.Instance.AddTokenListener(new OnTokenListener());
        }
        catch (System.Exception ex)
        {

            Debug.LogException(ex);
        }

    }


    public async void RequestVerifyCode()
    {

        VerifyCodeSettings settings = new VerifyCodeSettings.Builder()
               .SetAction(VerifyCodeSettings.ActionRegisterLogin)
               .SendInterval(30)
               .SetLang("en-US")
               .Build();

        Task<VerifyCodeResult> verifyCodeResultTask =
            AGConnectAuth.Instance.RequestVerifyCodeAsync(emailText.text, settings);
        try
        {
            await verifyCodeResultTask;
            VerifyCodeResult results = verifyCodeResultTask.Result;
        }
        catch (System.Exception)
        {

            if (verifyCodeResultTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(verifyCodeResultTask.Exception.InnerException.ToString());
        }


    }



    public async void RegisterUser()
    {
        EmailUser emailUser = new EmailUser.Builder()
              .SetEmail(emailText.text)
              .SetVerifyCode(codeText.text)
              .SetPassword(passwordText.text)
              .Build();

        Task<ISignInResult> createUserTask = AGConnectAuth.Instance.CreateUserAsync(emailUser);
        try
        {
            await createUserTask;
            var result = createUserTask.Result;
            var user = AGConnectAuth.Instance.GetCurrentUser();
            userText.text = user.GetEmail();
        }
        catch (System.Exception)
        {
            
            if (createUserTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(createUserTask.Exception.InnerException.ToString());
        }
    }


    public async void SignInEmailUser()
    {
        AGConnectUser user = AGConnectAuth.Instance.GetCurrentUser();
        if (user == null)
        {
            var xxxxx = Huawei.Agconnect.Version.LibraryInfos.Instance.GetVersion();
            IAGConnectAuthCredential credential = EmailAuthProvider.CredentialWithPassword(emailLoginText.text, passwordLoginText.text);
            Task<ISignInResult> signInTask = AGConnectAuth.Instance.SignInAsync(credential);
            try
            {
                await signInTask;
                ISignInResult signInResult = signInTask.Result;
                user = AGConnectAuth.Instance.GetCurrentUser();
                userText.text = user.GetEmail();
            }
            catch (System.Exception ex)
            {
                if (signInTask.Exception.InnerException is AGCException exception)
                    Debug.Log(exception.ErrorMessage);
                else
                    Debug.Log(signInTask.Exception.InnerException.ToString());
            }


        }
        else
        {
            AGConnectAuth.Instance.SignOut();
        }

    }

    public async void RequestVerifyCodeActionChangePassword()
    {
        VerifyCodeSettings settings = new VerifyCodeSettings.Builder()
            .SetAction(VerifyCodeSettings.ActionResetPassword)
            .SendInterval(30)
            .SetLang("en-US")
            .Build();
        Task<VerifyCodeResult> verifyCodeResultTask =
                AGConnectAuth.Instance.RequestVerifyCodeAsync(emailResetText.text, settings);
        try
        {
            await verifyCodeResultTask;

        }
        catch (System.Exception ex)
        {
            if (verifyCodeResultTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(verifyCodeResultTask.Exception.InnerException.ToString());
        }
    }
    public async void RequestVerifyCodeActionResetPassword()
    {
        VerifyCodeSettings settings = new VerifyCodeSettings.Builder()
            .SetAction(VerifyCodeSettings.ActionResetPassword)
            .SendInterval(30)
            .SetLang("en-US")
            .Build();
        Task<VerifyCodeResult> verifyCodeResultTask =
                AGConnectAuth.Instance.RequestVerifyCodeAsync(emailChangeText.text, settings);
        try
        {
            await verifyCodeResultTask;
            VerifyCodeResult verifyCodeResult = verifyCodeResultTask.Result;

        }
        catch (System.Exception ex)
        {

            if (verifyCodeResultTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(verifyCodeResultTask.Exception.InnerException.ToString());
        }


    }

    public async void ChangePassword()
    {
        var provider = 12;
        Task updatePasswordTask = AGConnectAuth.Instance.GetCurrentUser()
            .UpdatePasswordAsync(passwordResetText.text, codeResetText.text, provider);
        try
        {
            await updatePasswordTask;
        }
        catch (System.Exception ex)
        {
            if (updatePasswordTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(updatePasswordTask.Exception.InnerException.ToString());
        }

    }

    public async void ResetPassword()
    {
        Task resetPasswordTask = AGConnectAuth.Instance.ResetPasswordAsync(newEmailText.text, passwordChangeText.text, codeChangeText.text);
        try
        {
            await resetPasswordTask;
            Debug.Log("Ba?ar?l?");

        }
        catch (System.Exception ex)
        {

            if (resetPasswordTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(resetPasswordTask.Exception.InnerException.ToString());
        }

    }

    public async void RequestVerifyCodeActionChangeEmail()
    {
        VerifyCodeSettings settings = new VerifyCodeSettings.Builder()
            .SetAction(VerifyCodeSettings.ActionRegisterLogin)
            .SendInterval(30)
            .SetLang("en-US")
            .Build();
        Task<VerifyCodeResult> verifyCodeResultTask =
                AGConnectAuth.Instance.RequestVerifyCodeAsync(newEmailText.text, settings);
        try
        {
            await verifyCodeResultTask;

        }
        catch (System.Exception ex)
        {
            if (verifyCodeResultTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(verifyCodeResultTask.Exception.InnerException.ToString());
        }
    }


    public async void ChangeEmailAddress()
    {
        //Firstly run RequestVerifyCodeActionRegister method to receive verify code.
        //User should be logged in.

        if (AGConnectAuth.Instance.GetCurrentUser() != null)
        {
            Task updateEmail = AGConnectAuth.Instance.GetCurrentUser().UpdateEmailAsync(newEmailText.text, codeEmailChangeText.text);
            try
            {
                await updateEmail;
                var user = AGConnectAuth.Instance.GetCurrentUser();
                userText.text = user.GetEmail();
            }
            catch (System.Exception)
            {

                if (updateEmail.Exception.InnerException is AGCException exception)
                    Debug.Log(exception.ErrorMessage);
                else
                    Debug.Log(updateEmail.Exception.InnerException.ToString());
            }
        }
    }


    public async void RequestVerifyCodeActionSignVerifyCode()
    {
        VerifyCodeSettings settings = new VerifyCodeSettings.Builder()
            .SetAction(VerifyCodeSettings.ActionRegisterLogin)
            .SendInterval(30)
            .SetLang("en-US")
            .Build();
        Task<VerifyCodeResult> verifyCodeResultTask =
                AGConnectAuth.Instance.RequestVerifyCodeAsync(emailLoginCodeText.text, settings);
        try
        {
            await verifyCodeResultTask;

        }
        catch (System.Exception ex)
        {

            if (verifyCodeResultTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(verifyCodeResultTask.Exception.InnerException.ToString());
        }
    }

    public async void SignInWithVerifyCode()
    {
        //Firstly run RequestVerifyCodeActionRegister method to receive verify code.
        AGConnectUser user = AGConnectAuth.Instance.GetCurrentUser();
        if (user == null)
        {
            IAGConnectAuthCredential credential =
                EmailAuthProvider.CredentialWithVerifyCode(emailLoginCodeText.text, passwordLoginCodeText.text, codeLoginCodeText.text);

            Task<ISignInResult> signInTask = AGConnectAuth.Instance.SignInAsync(credential);
            try
            {
                await signInTask;
                user = AGConnectAuth.Instance.GetCurrentUser();
                userText.text = user.GetEmail();
            }
            catch (System.Exception)
            {

                if (signInTask.Exception.InnerException is AGCException exception)
                    Debug.Log(exception.ErrorMessage);
                else
                    Debug.Log(signInTask.Exception.InnerException.ToString());
            }

        }
    }

    public void SignOut()
    {
        AGConnectUser user = AGConnectAuth.Instance.GetCurrentUser();
        if (user != null)
        {
            AGConnectAuth.Instance.SignOut();
        }
    }

}
class OnTokenListener : IOnTokenListener
{
    public void OnChanged(ITokenSnapshot snapshot)
    {
        TokenSnapshotState state = snapshot.State;

        if (state == TokenSnapshotState.TokenUpdated)
        {
            string token = snapshot.Token;

        }
        else if (state == TokenSnapshotState.TokenInvalid)
        {
            string token = snapshot.Token;
        }
        else if (state == TokenSnapshotState.SignedIn)
        {
            string token = snapshot.Token;
        }
        else if (state == TokenSnapshotState.SignedOut)
        {
            string token = snapshot.Token;
        }
    }
}

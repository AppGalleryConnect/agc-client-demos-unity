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

using Huawei.Agconnect.AGCException;
using Huawei.Agconnect.Auth;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MobileNumber : MonoBehaviour
{
    public Text codeText, countryCodeText,  phoneText, userText;
    public Text  phoneLoginText, countryCodeLoginText, verifyCodeLoginText;
    public Text phoneChangeNumberText, countryCodeChangeNumberText, verifyCodeChangeNumberText;
    public Text passwordChangePassText, verifyCodeChangePassText;
    public Text passwordResetPassText, verifyCodeResetPassText, phoneResetPassText, countryCodeResetPassText;

    public InputField passwordText,passwordLoginText;
    // Start is called before the first frame update
    void Start()
    {

    }



    public async void RequestVerifyCode()
    {
        VerifyCodeSettings settings = new VerifyCodeSettings.Builder()
                 .SetAction(VerifyCodeSettings.ActionRegisterLogin)
                 .SendInterval(30)
                 .SetLang("en-US")
                 .Build();

        Task<VerifyCodeResult> verifyCodeResultTask =
            AGConnectAuth.Instance.RequestVerifyCodeAsync(countryCodeText.text, phoneText.text, settings);
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
        PhoneUser phoneUser = new PhoneUser.Builder()
               .SetPhoneNumber(phoneText.text)
               .SetCountryCode(countryCodeText.text)
               .SetVerifyCode(codeText.text)
               .SetPassword(passwordText.text)
               .Build();

        Task<ISignInResult> createUserTask = AGConnectAuth.Instance.CreateUserAsync(phoneUser);
        try
        {
            await createUserTask;
            var result = createUserTask.Result;
            var user = AGConnectAuth.Instance.GetCurrentUser();
            userText.text = user.GetPhone();
        }
        catch (System.Exception)
        {
            if (createUserTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(createUserTask.Exception.InnerException.ToString());
        }
    }



    public async void SignInMobile()
    {
        AGConnectUser user = AGConnectAuth.Instance.GetCurrentUser();
        if (user == null)
        {
            IAGConnectAuthCredential credential = PhoneAuthProvider.CredentialWithPassword(countryCodeLoginText.text, phoneLoginText.text, passwordLoginText.text);
            Task<ISignInResult> signInTask = AGConnectAuth.Instance.SignInAsync(credential);
            try
            {
                await signInTask;
                ISignInResult signInResult = signInTask.Result;
                user = AGConnectAuth.Instance.GetCurrentUser();
                userText.text = user.GetPhone();
            }
            catch (System.Exception)
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

    public async void RequestVerifyCodeReset()
    {
        VerifyCodeSettings settings = new VerifyCodeSettings.Builder()
                 .SetAction(VerifyCodeSettings.ActionResetPassword)
                 .SendInterval(30)
                 .SetLang("en-US")
                 .Build();

        Task<VerifyCodeResult> verifyCodeResultTask =
            AGConnectAuth.Instance.RequestVerifyCodeAsync(countryCodeText.text, phoneText.text, settings);
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

    public async void ChangeMobileNumber()
    {
        if (AGConnectAuth.Instance.GetCurrentUser() != null)
        {

            Task updatePhoneTask = AGConnectAuth.Instance.GetCurrentUser()
                .UpdatePhoneAsync(countryCodeChangeNumberText.text, phoneChangeNumberText.text, verifyCodeChangeNumberText.text);
            try
            {
                await updatePhoneTask;
            }
            catch (System.Exception)
            {

                if (updatePhoneTask.Exception.InnerException is AGCException exception)
                    Debug.Log(exception.ErrorMessage);
                else
                    Debug.Log(updatePhoneTask.Exception.InnerException.ToString());
            }
        }
    }
    public async void ChangePassword()
    {
        if (AGConnectAuth.Instance.GetCurrentUser() != null)
        {

            Task updatePassTask = AGConnectAuth.Instance.GetCurrentUser()
                .UpdatePasswordAsync(passwordChangePassText.text, verifyCodeChangePassText.text,
                    (int)AGConnectAuthCredentialEnums.PhoneProvider);
            try
            {
                await updatePassTask;
            }
            catch (System.Exception)
            {

                if (updatePassTask.Exception.InnerException is AGCException exception)
                    Debug.Log(exception.ErrorMessage);
                else
                    Debug.Log(updatePassTask.Exception.InnerException.ToString());
            }
        }
    }

    public async void ResetPassword()
    {
        if (AGConnectAuth.Instance.GetCurrentUser() != null)
        {

            Task resetPassTask = AGConnectAuth.Instance
                .ResetPasswordAsync(countryCodeResetPassText.text,phoneResetPassText.text,passwordResetPassText.text,verifyCodeResetPassText.text);
            try
            {
                await resetPassTask;
            }
            catch (System.Exception)
            {

                if (resetPassTask.Exception.InnerException is AGCException exception)
                    Debug.Log(exception.ErrorMessage);
                else
                    Debug.Log(resetPassTask.Exception.InnerException.ToString());
            }
        }
    }
}

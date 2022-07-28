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

using System;
using Huawei.Agconnect;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Huawei.Agconnect.Function;
using UnityEngine;
using UnityEngine.UI;

public class CloudFunction : MonoBehaviour
{
    AGConnectFunction function;
    public Text animalresultText,usernameresultText,resultText;

    public Text yearText, usernameText;
    // Start is called before the first frame update
    void Start()
    {
        Stream inputStream = new MemoryStream(Resources.Load<TextAsset>("agconnect-services").bytes);
        AGConnectInstance.Initialize(new AGConnectOptionsBuilder()
            .SetInputStream(inputStream).SetPersistPath(new DirectoryInfo(Application.persistentDataPath)));
        function = AGConnectFunction.Instance;
    }

    public async void CallFunction()
    {
        IFunctionCallable callable = function.Wrap("hello-$latest");
        Task<IFunctionResult> task = callable.Call();
        try
        {
            await task;
            resultText.text = task.Result.GetValue<UserInfo>().Username;

        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }
    }
    public async void CallFunctionWithUser()
    {
        IFunctionCallable callable = function.Wrap("get-account-info-$latest");
        UserInfo userInfo = new UserInfo
        {
            Username = "Jack"
        };
        Task<IFunctionResult> task = callable.Call(userInfo);
        try
        {
            await task;
            usernameresultText.text = task.Result.GetValue<UserInfo>().Username;

        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }
    }

    public async void CallFunctionAnimalYear()
    {
        IFunctionCallable callable = function.Wrap("china-year-animals-$latest");
        AnimalYear animalYear = new AnimalYear();
        animalYear.year = Convert.ToInt32(yearText.text);
        Task<IFunctionResult> task = callable.Call(animalYear);
        try
        {
            await task;
            animalresultText.text = task.Result.GetValue<AnimalYear>().result;

        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);
        }
    }

}

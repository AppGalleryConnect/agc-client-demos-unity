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


using Assets.Scripts.Models;
using Huawei.Agconnect;
using Huawei.Agconnect.Auth;
using Huawei.Agconnect.Core.Service.Auth;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Huawei.Agconnect.CloudDB;
using UnityEngine;

public class DBConnect
{
    static DBConnect Instance = null;
    AGConnectCloudDB CloudDB;
    public CloudDBZone Zone { get; private set; }

    private DBConnect()
    {
    }

    public static DBConnect GetInstance()
    {
        return Instance ??= new DBConnect();
    }

    public void InitDB()
    {
        try
        {
            Stream inputStream = new MemoryStream(Resources.Load<TextAsset>("agconnect-services").bytes);
            AGConnectInstance.Initialize(new AGConnectOptionsBuilder()
                .SetInputStream(inputStream).SetPersistPath(new DirectoryInfo(Application.persistentDataPath)));
            AGConnectAuth.Instance.AddTokenListener(new OnTokenListener());
            CloudDB = AGConnectCloudDB.GetInstance(AGConnectInstance.Instance,AGConnectAuth.Instance);
            Debug.Log("Database Initialized");
        }
        catch (AGCAuthException ex)
        {
            Debug.LogError("Database could not be initialized: " + ex.ErrorMessage);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Database could not be initialized: " + ex.Message);
        }
    }

    public async Task<bool> Login(string email, string password)
    {
        if (AGConnectAuth.Instance.GetCurrentUser() == null)
        {
            try
            {
                IAGConnectAuthCredential credential = EmailAuthProvider.CredentialWithPassword(email, password);
                var result = await AGConnectAuth.Instance.SignInAsync(credential);
                Debug.Log("Login Successful");
                return true;
            }
            catch (AGCAuthException ex)
            {
                Debug.LogError("Login Failed: " + ex.ErrorMessage);
                return false;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Login Failed: " + ex.Message);
                return false;
            }
        }
        else
        {
            Debug.LogError("User Already Logged In");
            return true;
        }
    }

    public bool InitJsonObjectTypes()
    {
        try
        {
            Debug.Log("Start Create Schema");
            string json = Resources.Load<TextAsset>("UnityCloudDB").text;
            ObjectTypeInfo objectTypeInfo = JsonConvert.DeserializeObject<ObjectTypeInfo>(json);
            try
            {
                CloudDB.CreateObjectType(objectTypeInfo);
                Debug.Log("Schema Create Success");
                return true;
            }
            catch (AGCAuthException ex)
            {
                Debug.LogError("Schema Create Failed: " + ex.ErrorMessage);
                return false;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Schema Create Failed: " + ex.Message);
                return false;
            }
        }
        catch (AGCAuthException ex)
        {
            Debug.LogError("Schema Create Failed: " + ex.ErrorMessage);
            return false;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Could Not Read CloudDB Json File: " + ex.Message);
            return false;
        }
    }

    public async Task<bool> OpenCloudZone()
    {
        try
        {
            Debug.Log("Try To Open Zone");
            Zone = await CloudDB.OpenCloudDBZone(new CloudDBZoneConfig("Demo"));
            Debug.Log("Open Zone is Successful");
            return true;
        }
        catch (AGCAuthException ex)
        {
            Debug.LogError("Open Zone Failed: " + ex.ErrorMessage);
            return false;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Open Zone Failed: " + ex.Message);
            return false;
        }
    }

    public async Task<List<Board>> GetAllBoards()
    {
        try
        {
            var query = CloudDBZoneQuery<Board>.Where(typeof(Board));
            var snapshot = await Zone.ExecuteQuery(query);
            List<Board> boards = snapshot.GetSnapshotObjects();
            return boards;
        }
        catch (AGCAuthException ex)
        {
            Debug.LogError(ex.ErrorMessage);
            return null;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
            return null;
        }
    }

    public async Task<Board> GetBoard(string id)
    {
        try
        {
            var query = CloudDBZoneQuery<Board>.Where(typeof(Board)).EqualTo("Id", id);
            var snapshot = await Zone.ExecuteQuery(query);
            List<Board> boards = snapshot.GetSnapshotObjects();
            if (boards.Count != 0)
                return boards[0];
        }
        catch (AGCAuthException ex)
        {
            Debug.LogError(ex.ErrorMessage);
            return null;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
            return null;
        }

        return null;
    }

    public async Task ExecuteUpsert(Board board)
    {
        try
        {
            await Zone.ExecuteUpsert(board);
            Debug.Log("upsert is successful");
        }
        catch (AGCAuthException ex)
        {
            Debug.LogError(ex.ErrorMessage);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public async Task ExecuteDelete(Board board)
    {
        try
        {
            await Zone.ExecuteDelete(board);
            Debug.Log("delete is successful");
        }
        catch (AGCAuthException ex)
        {
            Debug.LogError(ex.ErrorMessage);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public void CloseZone()
    {
        CloudDB.CloseCloudDBZone(Zone);
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
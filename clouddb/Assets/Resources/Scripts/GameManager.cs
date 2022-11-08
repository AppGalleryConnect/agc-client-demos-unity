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


using Assets.Scripts;
using Assets.Scripts.Models;
using Huawei.Agconnect.AGCException;
using Huawei.Agconnect.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;
using Huawei.Agconnect.CloudDB;
using Huawei.Agconnect.CloudDB.Exceptions;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    string GameID;
    public Gap[] Gaps;
    public Board Board;
    public static GameManager instance;
    public GameObject GameBoard;
    public GameObject MainMenu;
    public GameObject Email;
    public GameObject Password;
    public GameObject LoginPanel;
    public GameObject LoginSuccessful;
    public GameObject LoginUnsuccessful;
    public GameObject Verify;
    public GameObject VerifyCode;
    public GameObject GameJoinPanel;
    public GameObject GameIDJoin;
    public GameObject GameNotFoundPanel;
    public GameObject GameCode;
    public GameObject WaitPlayerPanel;
    public GameObject WinnerPanel;
    public GameObject WinnerInform;
    public GameObject PlayerTypeText;

    public bool IsGameOver;
    public string PlayerType;
    bool NeedUpdate;
    private DBConnect _dbConnect;

    void Awake()
    {
        if (instance==null)
        {
            DontDestroyOnLoad(this);
            instance = this;
            IsGameOver = false;
            _dbConnect=DBConnect.GetInstance();
            _dbConnect.InitDB();
            GameCode.GetComponent<TMP_Text>().text = "Game Code\n";
           
            
        }
        else
        {
            Destroy(this);
        }
       
    }

    //private async void Start()
    //{
    //    if (AGConnectAuth.Instance.GetCurrentUser() != null)
    //    {
    //        bool isInitSuccessful = _dbConnect.InitJsonObjectTypes();
    //        if (isInitSuccessful)
    //        {
    //            bool isCloudZoneOpened = await _dbConnect.OpenCloudZone();
    //            if (isCloudZoneOpened)
    //            {
    //                LoginPanel.SetActive(false);
    //                LoginSuccessful.SetActive(true);
    //            }
    //        }
    //    }
    //}

    void Update()
    {
        if (NeedUpdate)
        {
            Gaps[0].Text.text = Board.Gap0;
            Gaps[1].Text.text = Board.Gap1;
            Gaps[2].Text.text = Board.Gap2;
            Gaps[3].Text.text = Board.Gap3;
            Gaps[4].Text.text = Board.Gap4;
            Gaps[5].Text.text = Board.Gap5;
            Gaps[6].Text.text = Board.Gap6;
            Gaps[7].Text.text = Board.Gap7;
            Gaps[8].Text.text = Board.Gap8;

            GameOverCheck(Board.LastIndex, Board.Turn);
            if (!WaitPlayerPanel.activeSelf && !Board.HasPlayerO)
            {
                Debug.Log("O Disconnected Listener");
                WinnerPanel.SetActive(true);
                WinnerInform.GetComponent<TMP_Text>().text = "O Disconncted. X Won !";
            }

            if (WaitPlayerPanel.activeSelf && Board.HasPlayerO) WaitPlayerPanel.SetActive(false);
            if (!Board.HasPlayerX)
            {
                Debug.Log("X Disconnected Listener");
                WinnerPanel.SetActive(true);
                WinnerInform.GetComponent<TMP_Text>().text = "X Disconncted. O Won !";
            }

            NeedUpdate = false;
        }
    }

    public async void Login()
    {
        bool isLoggedIn = await _dbConnect.Login(Email.GetComponent<TMP_InputField>().text,
            Password.GetComponent<TMP_InputField>().text);

        if (isLoggedIn)
        {
            bool isInitSuccessful = _dbConnect.InitJsonObjectTypes();
            if (isInitSuccessful)
            {
                bool isCloudZoneOpened = await _dbConnect.OpenCloudZone();
                if (isCloudZoneOpened)
                {
                    LoginPanel.SetActive(false);
                    LoginSuccessful.SetActive(true);
                    return;
                }
            }
        }

        LoginPanel.SetActive(false);
        LoginUnsuccessful.SetActive(true);
    }

    public async void JoinGame()
    {
        Board = await _dbConnect.GetBoard(GameIDJoin.GetComponent<TMP_InputField>().text);

        if (Board == null)
        {
            GameNotFoundPanel.SetActive(true);
            return;
        }

        GameID = Board.Id;
        GameCode.GetComponent<TMP_Text>().text += GameID;
        Board.HasPlayerO = true;
        PlayerType = "O";
        PlayerTypeText.GetComponent<TMP_Text>().text += "\nO";
        MainMenu.SetActive(false);
        GameBoard.SetActive(true);
        GameJoinPanel.SetActive(false);

        await _dbConnect.ExecuteUpsert(Board);

        BoardChangeListener boardListener = new BoardChangeListener(instance);
        CloudDBZoneQuery<Board> boardQuery = CloudDBZoneQuery<Board>.Where(typeof(Board)).EqualTo("Id", Board.Id);
        await _dbConnect.Zone.SubscribeSnapshot(boardQuery, boardListener);
    }

    public async void CreateGame()
    {
        const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
        NewGameId:
        GameID = "";
        for (int i = 0; i < 6; i++)
        {
            GameID += glyphs[Random.Range(0, glyphs.Length)];
        }

        foreach (Board board in await _dbConnect.GetAllBoards())
        {
            if (board.Id.Equals(GameID)) goto NewGameId;
        }

        GameCode.GetComponent<TMP_Text>().text += GameID;
        Board = new Board
        {
            Id = GameID
        };
        PlayerType = "X";
        PlayerTypeText.GetComponent<TMP_Text>().text += "\nX";
        Board.HasPlayerX = true;
        MainMenu.SetActive(false);
        GameBoard.SetActive(true);
        WaitPlayerPanel.SetActive(true);

        await _dbConnect.ExecuteUpsert(Board);

        BoardChangeListener boardListener = new BoardChangeListener(instance);
        CloudDBZoneQuery<Board> boardQuery = CloudDBZoneQuery<Board>.Where(typeof(Board)).EqualTo("Id", GameID);
        await _dbConnect.Zone.SubscribeSnapshot(boardQuery, boardListener);
    }

    public async void GameOverCheck(int index, string Mark)
    {
        IsGameOver = true;
        for (int i = 0; i < 9; i++)
        {
            if (Gaps[i].Text.text != "X" && Gaps[i].Text.text != "O")
            {
                IsGameOver = false;
                break;
            }
        }

        if (IsGameOver)
        {
            WinnerPanel.SetActive(true);
            WinnerInform.GetComponent<TMP_Text>().text = "Draw !";
            await _dbConnect.ExecuteDelete(Board);
        }

        bool RowCheck()
        {
            int rowNum = index / 3;

            for (int i = 0; i < 3; i++)
            {
                if (Gaps[rowNum * 3 + i].Text.text != Mark) return false;
            }

            return true;
        }

        bool ColumnCheck()
        {
            int columnNum = index % 3;

            for (int i = 0; i < 3; i++)
            {
                if (Gaps[i * 3 + columnNum].Text.text != Mark) return false;
            }

            return true;
        }

        bool CrossCheck()
        {
            if (index % 2 == 1) return false;

            bool LeftToRightCheck()
            {
                for (int i = 0; i < 9; i += 4)
                {
                    if (Gaps[i].Text.text != Mark) return false;
                }

                return true;
            }

            bool RightToLeftCheck()
            {
                for (int i = 2; i < 7; i += 2)
                {
                    if (Gaps[i].Text.text != Mark) return false;
                }

                return true;
            }

            if (index == 2 || index == 6) return RightToLeftCheck();
            if (index == 0 || index == 8) return LeftToRightCheck();
            return LeftToRightCheck() || RightToLeftCheck();
        }

        IsGameOver = RowCheck() || ColumnCheck() || CrossCheck();

        if (IsGameOver)
        {
            WinnerPanel.SetActive(true);
            WinnerInform.GetComponent<TMP_Text>().text = Board.Turn + " Won !";
            if (await _dbConnect.GetBoard(GameID) != null)
                await _dbConnect.ExecuteDelete(Board);
        }
    }

    // UI Functions

    public async void Register()
    {
        EmailUser emailUser = new EmailUser.Builder()
            .SetEmail(Email.GetComponent<TMP_InputField>().text)
            .SetVerifyCode(VerifyCode.GetComponent<TMP_InputField>().text)
            .SetPassword(Password.GetComponent<TMP_InputField>().text)
            .Build();

        Task<ISignInResult> createUserTask = AGConnectAuth.Instance.CreateUserAsync(emailUser);
        try
        {
            await createUserTask;
            var result = createUserTask.Result;
            var user = AGConnectAuth.Instance.GetCurrentUser();
            Verify.SetActive(false);
            LoginSuccessful.SetActive(true);
        }
        catch (System.Exception)
        {
            if (createUserTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else
                Debug.Log(createUserTask.Exception.InnerException.ToString());

            Verify.SetActive(false);
            LoginUnsuccessful.SetActive(true);
        }
    }

    public void CloseGameNotFoundPanel()
    {
        GameNotFoundPanel.SetActive(false);
    }

    public void CloseGameJoinPanel()
    {
        GameJoinPanel.SetActive(false);
    }

    public async void OpenVerifyPanel()
    {
        VerifyCodeSettings settings = new VerifyCodeSettings.Builder()
            .SetAction(VerifyCodeSettings.ActionRegisterLogin)
            .SendInterval(30)
            .SetLang("en-US")
            .Build();

        Task<VerifyCodeResult> verifyCodeResultTask =
            AGConnectAuth.Instance.RequestVerifyCodeAsync(Email.GetComponent<TMP_InputField>().text, settings);
        try
        {
            await verifyCodeResultTask;
        }
        catch (System.Exception)
        {
            if (verifyCodeResultTask.Exception.InnerException is AGCException exception)
                Debug.Log(exception.ErrorMessage);
            else Debug.Log(verifyCodeResultTask.Exception.InnerException.ToString());
        }

        LoginPanel.SetActive(false);
        Verify.SetActive(true);
    }

    public void ReturnLoginPanel()
    {
        LoginUnsuccessful.SetActive(false);
        LoginPanel.SetActive(true);
        
    }

    public void EndGame()
    {
        Board = new Board();
        Gaps[0].Text.text = Board.Gap0;
        Gaps[1].Text.text = Board.Gap1;
        Gaps[2].Text.text = Board.Gap2;
        Gaps[3].Text.text = Board.Gap3;
        Gaps[4].Text.text = Board.Gap4;
        Gaps[5].Text.text = Board.Gap5;
        Gaps[6].Text.text = Board.Gap6;
        Gaps[7].Text.text = Board.Gap7;
        Gaps[8].Text.text = Board.Gap8;
        GameCode.GetComponent<TMP_Text>().text = "";
        PlayerTypeText.GetComponent<TMP_Text>().text = "Player";
        GameBoard.SetActive(false);
        MainMenu.SetActive(true);
        WinnerPanel.SetActive(false);
        WinnerInform.SetActive(false);
    }

    public void OpenJoinGame()
    {
        GameJoinPanel.SetActive(true);
    }

    public async void CloseGame()
    {
        if (await _dbConnect.GetBoard(GameID) != null)
        {
            if (PlayerType == "X") Board.HasPlayerX = false;
            else if (PlayerType == "O") Board.HasPlayerO = false;
            await _dbConnect.ExecuteUpsert(Board);
        }

        AGConnectAuth.Instance.SignOut();
        Application.Quit();
    }

    public void SignOut()
    {
        _dbConnect.CloseZone();
        AGConnectAuth.Instance.SignOut();
        LoginSuccessful.SetActive(false);
        LoginPanel.SetActive(true);
    }


    // Listeners
    class BoardChangeListener : IOnSnapshotListener<Board>
    {
        GameManager GM;

        public BoardChangeListener(GameManager gameManager)
        {
            GM = gameManager;
        }

        public void OnSnapshot(CloudDBZoneSnapshot<Board> snapshot, AGConnectCloudDBException e)
        {
            if (e != null)
            {
                Debug.LogError("Occured Error at Board Listener: " + e.Message);
                return;
            }

            Debug.Log("Board Listener");
            List<Board> boards = snapshot.GetSnapshotObjects();
            GM.Board = boards[boards.Count - 1];
            GM.NeedUpdate = true;
        }
    }
}
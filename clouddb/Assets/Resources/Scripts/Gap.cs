using UnityEngine;
using TMPro;

namespace Assets.Scripts
{
    public class Gap : MonoBehaviour
    {
        public int index;

        public TextMeshProUGUI Text;

        GameManager GM;

        void Start() { Text = GetComponentInChildren<TextMeshProUGUI>(); GM = GameObject.Find("GameManager").GetComponent<GameManager>(); }

        public async void Mark() 
        { 
            if (!GM.IsGameOver && Text.text != "X" && Text.text != "O" && GM.PlayerType == GM.Board.Turn) 
            { 
                Text.text = GM.Board.Turn; GM.GameOverCheck(index, Text.text); 
                if (!GM.IsGameOver)  { if (GM.Board.Turn == "X") GM.Board.Turn = "O"; else GM.Board.Turn = "X"; } else Debug.Log("Game Over");

                GM.Board.LastIndex = index;
                GM.Board.Gap0 = GM.Gaps[0].Text.text;
                GM.Board.Gap1 = GM.Gaps[1].Text.text;
                GM.Board.Gap2 = GM.Gaps[2].Text.text;
                GM.Board.Gap3 = GM.Gaps[3].Text.text;
                GM.Board.Gap4 = GM.Gaps[4].Text.text;
                GM.Board.Gap5 = GM.Gaps[5].Text.text;
                GM.Board.Gap6 = GM.Gaps[6].Text.text;
                GM.Board.Gap7 = GM.Gaps[7].Text.text;
                GM.Board.Gap8 = GM.Gaps[8].Text.text;

                await DBConnect.GetInstance().ExecuteUpsert(GM.Board);
            } 
        }
    }
}

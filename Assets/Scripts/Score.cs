using UnityEngine;
using UnityEngine.UI;
public class Score : MonoBehaviour
{
    private Board board;
    public Text score;
  
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        score.text = "Score : 0";
    }
    void Update()
    {
        score.text = "Score : " + board.score.ToString("0");
    }
}

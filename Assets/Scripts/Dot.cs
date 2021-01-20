using UnityEngine;

public class Dot : MonoBehaviour
{
    private Board board;

    [Header("Dependant Variables")]
    public bool isSelected = false;
    public bool isMatched = false;
    public bool isPrev = false;
    Vector2 tempPosition;
    
    SpriteRenderer mySprite;
    Color currentColor;

    public int column;
    public int row;
    private int targetY;
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        mySprite = GetComponent<SpriteRenderer>();
        currentColor = mySprite.color;

        column = (int)transform.position.x;
        row = (int)transform.position.y;
        targetY = row;
    }

    void Update()
    {
        targetY = row;

        if (Mathf.Abs(targetY - transform.position.y) != 0)
        {
            //Move Towards the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }

        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            touchPos.z = 0;

            if (Vector3.Distance(touchPos, transform.position) < 0.35)
            {
                isSelected = true;
            }
            else
            {
                isSelected = false;
            }

            
        }

        SelectingDots();
    }
    void SelectingDots()
    {
        if (isSelected && board.matchedDots.Count > 0 )
        {

            //Adding a matched dot
            if (!board.matchedDots.Contains(this.gameObject))
            {
                if (this.gameObject.tag == board.matchedDots.Peek().tag && Vector3.Distance(transform.position, board.matchedDots.Peek().transform.position) < 1.5)
                {
                    board.matchedDots.Peek().GetComponent<Dot>().isPrev = true;
                    isMatched = true;                    
                    board.matchedDots.Push(this.gameObject);
                }
            }

            //Taking back a move
            else if (board.matchedDots.Contains(this.gameObject))
            {
                if (isPrev && board.matchedDots.Count > 1)
                {
                    board.matchedDots.Pop().GetComponent<Dot>().ResetDot();
                }
            }

            if(isMatched)
                ChangeColor();

        }
    }

    void ChangeColor()
    {
        mySprite.color = new Color(1f,1f,1f, .5f);
    }
    public void ResetDot()
    {
        isMatched = false;
        isSelected = false;
        isPrev = false;
        mySprite.color = currentColor;

    }
}



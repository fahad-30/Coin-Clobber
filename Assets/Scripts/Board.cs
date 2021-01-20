using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public int height;
    public int width;
    public GameObject[] dots;
    public GameObject[,] allDots;
    public Stack<GameObject> matchedDots = new Stack<GameObject>();
    public int prevCount;
    Vector3 startPos;
    Vector3 direction;
    public int score;
 
    void Start()
    {
        allDots = new GameObject[width, height];
        prevCount = matchedDots.Count;
        score = 0;
        Setup();
    }

    void Setup()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                Vector2 tempPos = new Vector2(i, j);
                int dotToUse = Random.Range(0, dots.Length);
                GameObject dot_c = Instantiate(dots[dotToUse], tempPos, Quaternion.identity);
                dot_c.transform.parent = this.transform;
                dot_c.name = "( " + i + ", " + j + " )";
                allDots[i,j] = dot_c;
                dot_c.GetComponent<Dot>().row = j;
                dot_c.GetComponent<Dot>().column = i;

            }
        }
    }

    void Update()
    {
        MovingTouch();
    }

    void MovingTouch()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            touchPos.z = 0;
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touchPos;
                    InitialDot(touchPos);
                    break;

                //case TouchPhase.Moved:
                //   break;

                case TouchPhase.Ended:
                    StartCoroutine(DestroyAllMatches());
                    break;
            }
        }
    }

    public IEnumerator DestroyAllMatches()
    {
        yield return new WaitForSeconds(.5f);

        if (matchedDots.Count > 2)
        {
            score += matchedDots.Count;

            foreach (GameObject item in matchedDots)
            {
                Destroy(item);
            }
        }
        else if(matchedDots.Count > 0 && matchedDots.Count <=2)
            ResetStack();

        matchedDots = new Stack<GameObject>();

        StartCoroutine(DecreaseRowCo());
    }

    void ResetStack()
    {
        foreach (GameObject item in matchedDots)
        {
            item.GetComponent<Dot>().ResetDot();
        }
       
    }

    void InitialDot(Vector3 pos)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    
                    if (Vector3.Distance(pos, allDots[i, j].transform.position) < 0.35)
                    {
                        matchedDots.Push(allDots[i, j]);
                        matchedDots.Peek().GetComponent<Dot>().isMatched = true;

                    }
                }
            }
        }
    }

    private IEnumerator DecreaseRowCo()
    {
        yield return new WaitForSeconds(.2f);
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allDots[i, j].GetComponent<Dot>().row -= nullCount;
                    allDots[i, j] = null;

                }
            }
            nullCount = 0;
        }
        

        StartCoroutine(RefillBoard());
    }

    private IEnumerator RefillBoard()
    {
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    Vector2 finalPos = new Vector2(i,j);
                    Vector2 tempPosition = new Vector2(i, height+j);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                    allDots[i,j].transform.position = Vector2.Lerp(allDots[i, j].transform.position, finalPos, .6f);
                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;

                }
            }
        }
    }

    

}


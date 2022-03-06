using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    [SerializeField] Transform bottomLeft;
    [SerializeField] Transform topRight;
    [SerializeField] Transform completedPieces;

    const int width = 10, height = 25;
    bool[,] stackArray;
    GameObject[,] goStackArray;

    Spawner spawner;
    ScoreKeeper scoreKeeper;
    Movement movement;

    private void Start()
    {
        InitStack();
        spawner = FindObjectOfType<Spawner>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        movement = FindObjectOfType<Movement>();
    }

    private void InitStack()
    {
        stackArray = new bool[height,width];
        goStackArray = new GameObject[height,width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                stackArray[y,x] = false;
                goStackArray[y, x] = null;
            }
        }
    }

    public bool isColliding(List<Vector2> newPositions)
    {
        foreach (Vector2 newPosition in newPositions)
        {
            if (newPosition.y <= GetBottomLeft().y) 
                return true;
            if (newPosition.x >= GetTopRight().x || newPosition.x < GetBottomLeft().x)
                return true;
            if(newPosition.y < topRight.transform.position.y)
            {
                if (stackArray[(int)(newPosition.y - GetBottomLeft().y), (int)(newPosition.x - GetBottomLeft().x)] == true)
                    return true;
            }
        }
        return false;
    }


    public void AddShapeToStack(List<GameObject> shape)
    {
        foreach (GameObject piece in shape)
        {
            Vector2 basePos = piece.transform.position - bottomLeft.transform.position;
            stackArray[(int)basePos.y,(int)basePos.x] = true;
            goStackArray[(int)basePos.y,(int)basePos.x] = piece;
            piece.transform.parent = gameObject.transform;
        }
    }

    public void CheckCompletedRows()
    {
        int completedRowCount = 0;
        for(int y=height-1; y>=0; y--)
        {
            bool completedRow = true;
            for(int x=0; x<width; x++)
            {
                if(stackArray[y, x] == false)
                {
                    completedRow = false;
                    break;
                }
            }
            if (completedRow)
            {
                movement.SetPaused(true);
                ClearRow(y);
                completedRowCount++;
            }
        }

        scoreKeeper.AddScore(1000 * completedRowCount * completedRowCount * (1.0f / movement.GetSlideSpeed()) );
    }

    private void ClearRow(int row)
    {
        for (int x = 0; x < width; x++)
        {
            if (goStackArray[row, x] != null)
            {
                goStackArray[row, x].GetComponent<PieceScript>().StartAnimation(completedPieces.transform.position);
            }
        }

        float blinkDelay = goStackArray[row, 0].GetComponent<PieceScript>().GetTotalBlinkDelay();

        StartCoroutine(RepositionStackElements(row, blinkDelay));
    }

    IEnumerator RepositionStackElements(int row, float blinkDelay)
    {
        yield return new WaitForSeconds(blinkDelay);
        for (int y = row; y < height - 1; y++)
        {
            for (int x = 0; x < width; x++)
            {
                stackArray[y, x] = stackArray[y + 1, x];
                goStackArray[y, x] = goStackArray[y + 1, x];
                if (goStackArray[y, x] != null)
                {
                    goStackArray[y, x].transform.position = (Vector2)goStackArray[y, x].transform.position + Vector2.down;
                }
            }
        }
    }

    public Vector2 GetBottomLeft()
    {
        return bottomLeft.transform.position;
    }

    public Vector2 GetTopRight()
    {
        return topRight.transform.position;
    }
}

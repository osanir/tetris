using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    List<GameObject> pieceList;

    private void Start()
    {
        if(pieceList == null)
        {
            pieceList = new List<GameObject>();
        }
    }

    public void ClearPieceList()
    {
        if(pieceList.Count > 0)
        {
            foreach(GameObject piece in pieceList)
            {
                // POOL
                //Destroy(piece);
            }
            pieceList.Clear();
        }
    }

    public void AddPiece(GameObject piece)
    {
        pieceList.Add(piece);
    }

    public void SetPieceList(List<GameObject> newPieceList)
    {
        pieceList = newPieceList;
    }

    public List<GameObject> GetPieceList()
    {
        return pieceList;
    }

    public int[] GetShapeMatrix(string shapeType)
    {
        switch (shapeType){
            case "L":
                {
                    return new int[6] { 0, 2, 4, 5, 0, -2};
                }
            case "J":
                {
                    return new int[6] { 1, 3, 4, 5, 0, -2};
                }
            case "O":
                {
                    return new int[6] { 2, 3, 4, 5, 0, -1};
                }
            case "T":
                {
                    return new int[6] { 0, 2, 3, 4, 0, -1};
                }
            case "Z":
                {
                    return new int[6] { 1, 2, 3, 4, 0, -2};
                }
            case "S":
                {
                    return new int[6] { 0, 2, 3, 5, 0, -2};
                }
            case "I":
                {
                    return new int[6] { 0, 2, 4, 6, 0, -2};
                }
        }
        return null;
    }
}

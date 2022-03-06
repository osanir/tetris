using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject piecePrefab;
    [SerializeField] List<Sprite> spriteList;
    [SerializeField] TextMeshProUGUI nextShapeText;

    GameObject movementController;
    Stack stack;
    Shape shape;

    string[] shapeTypeList = { "L", "J", "O", "T", "Z", "S", "I" };
    string[] shapeTypeFontEquivalent = { "J", "M", "B", "E", "I", "L", "A" };
    int nextShapeIndex = 0;
    private void Awake()
    {
        movementController = FindObjectOfType<Movement>().gameObject;
        stack = FindObjectOfType<Stack>();
        shape = FindObjectOfType<Shape>();
    }

    private void Start()
    {
        CreateRandomShape();
        nextShapeIndex = Random.Range(0, shapeTypeList.Length);
    }

    public void CreateRandomShape()
    {
        shape.SetPieceList(CreateShape(shapeTypeList[nextShapeIndex]));
        nextShapeIndex = Random.Range(0, shapeTypeList.Length);
        nextShapeText.text = shapeTypeFontEquivalent[nextShapeIndex];
    }

    public List<GameObject> CreateShape(string shapeType)
    {
        movementController.transform.position = transform.position;
        if(shape.GetPieceList() != null)
            shape.ClearPieceList();
        List<GameObject> pieceList = new List<GameObject>();

        int[] shapeMatrix = shape.GetShapeMatrix(shapeType);
        int spriteIndex = Random.Range(0, spriteList.Count);
        for (int i=0; i<4; i++)
        {
            Vector2 offset = new Vector2(shapeMatrix[i] % 2, -shapeMatrix[i] / 2) 
                - new Vector2(shapeMatrix[4], shapeMatrix[5]) 
                - new Vector2(0.5f, 0.5f);
            pieceList.Add(CreatePiece(offset, spriteIndex));

            Debug.Log("Created piece[" + i.ToString() + "]: " + shapeType);
            Debug.Log(offset);
            Debug.Log(pieceList[i].transform.position);
        }

        return pieceList;
    }

    GameObject CreatePiece(Vector2 offset, int spriteIndex)
    {
        GameObject newPiece = Instantiate(piecePrefab,
                                    (Vector2)transform.position + offset,
                                    Quaternion.identity,
                                    movementController.transform);
        newPiece.GetComponent<SpriteRenderer>().sprite = spriteList[spriteIndex];
        return newPiece;
    }

    Color GenerateRandomColor()
    {
        float red = Random.Range(0.0f, 1.0f);
        float green = Random.Range(0.0f, 1.0f);
        float blue = Random.Range(0.0f, 1.0f);
        return new Color(red, green, blue);
    }

    public List<Sprite> GetSpriteList()
    {
        return spriteList;
    }
}

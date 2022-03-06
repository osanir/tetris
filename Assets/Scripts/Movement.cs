using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [SerializeField] float slideSpeed = 1;
    [SerializeField] float difficultyLevel = 0.99f;

    bool isPaused = false;
    AudioPlayer audioPlayer;
    CameraShake cameraShake;
    Spawner spawner;
    Shape shape;
    Stack stack;
    float elapsedTime = 0f;
    bool isSpeedUp = false;
    List<Vector2> predeterminedStuckMovement = new List<Vector2>() {
        Vector2.left,
        Vector2.right,
        Vector2.up,
        Vector2.up * 2,
        Vector2.up * 3,
        Vector2.up * 4,
    };

    private void Start()
    {
        stack = FindObjectOfType<Stack>();
        spawner = FindObjectOfType<Spawner>();
        shape = FindObjectOfType<Shape>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    void Update()
    {
        if(!isPaused) 
            elapsedTime += Time.deltaTime;
        
        if(elapsedTime > (isSpeedUp ? slideSpeed/20 : slideSpeed))
        {
            elapsedTime = 0;
            if (CanMove(Vector2.down))
            {
                MoveDown();
            } else
            {
                if(transform.position == spawner.transform.position)
                {
                    SceneManager.LoadScene("Game");
                    // GAMEOVER
                    // return;
                }
                slideSpeed = slideSpeed * difficultyLevel;
                stack.AddShapeToStack(shape.GetPieceList());
                transform.rotation = Quaternion.EulerRotation(Vector3.zero);
                stack.CheckCompletedRows();
                spawner.CreateRandomShape();
            }
        }
    }

    private bool CanMove(Vector2 direction)
    {
        return !stack.isColliding(GetNewPositionList(direction));
    }

    List<Vector2> GetNewPositionList(Vector2 direction)
    {
        List<Vector2> newPositionList = new List<Vector2>();
        foreach (GameObject piece in shape.GetPieceList())
        {
            Vector2 newPosition = (Vector2)piece.transform.position + direction;
            newPositionList.Add(newPosition);
        }

        return newPositionList;
    }

    private void MoveGivenDirection(Vector2 direction)
    {
        transform.position = (Vector2)transform.position + direction;
    }

    private void MoveDown()
    {
        transform.position = (Vector2)transform.position + Vector2.down;
    }
    private void MoveLeft()
    {
       transform.position = (Vector2)transform.position + Vector2.left;
    }

    private void MoveRight()
    {
        transform.position = (Vector2)transform.position + Vector2.right;
    }

    private void RotateClockwise()
    {
        transform.Rotate(new Vector3(0, 0, 90));
        foreach(GameObject piece in shape.GetPieceList())
        {
            piece.transform.Rotate(new Vector3(0, 0, -90));
        }
    }

    private void RotateCounterClockwise()
    {
        transform.Rotate(new Vector3(0, 0, -90));
        foreach (GameObject piece in shape.GetPieceList())
        {
            piece.transform.Rotate(new Vector3(0, 0, -90));
        }
    }

    void OnFire(InputValue value)
    {
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("piece[" + i.ToString() + "]: " + shape.GetPieceList()[i].transform.localPosition.x + " " + shape.GetPieceList()[i].transform.localPosition.y);
            Debug.Log(shape.GetPieceList()[i].transform.position);
        }
        //spawner.CreateRandomShape();
    }

    void OnMove(InputValue value)
    {
        Vector2 rawInput = value.Get<Vector2>();
        if(rawInput.x == 1)
        {
            if (CanMove(Vector2.right))
            {
                MoveRight();
            } else
            {
                audioPlayer.PlayHitClip();
                cameraShake.Play();
            }
        } else if( rawInput.x == -1)
        {
            if (CanMove(Vector2.left))
            {
                MoveLeft();
            } else
            {
                audioPlayer.PlayHitClip();
                cameraShake.Play();
            }
        } else if( rawInput.y == 1)
        {
            RotateClockwise();
            int nextMoveIndex = 0;
            Vector2 initialPosition = transform.position;
            while (!CanMove(Vector2.zero))
            {
                transform.position = initialPosition + predeterminedStuckMovement[nextMoveIndex];
                nextMoveIndex++;
                if(nextMoveIndex > predeterminedStuckMovement.Count - 1)
                {
                    transform.position = initialPosition;
                    RotateCounterClockwise();
                }
            }
        } 
        
        if(rawInput.y == -1)
        {
            isSpeedUp = true;
        } else
        {
            isSpeedUp = false;
        }

    }

    public float GetSlideSpeed()
    {
        return slideSpeed;
    }

    public void SetPaused(bool newState)
    {
        isPaused = newState;
    }

    public bool GetPaused()
    {
        return isPaused;
    }
}

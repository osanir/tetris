using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceScript : MonoBehaviour
{
    [SerializeField] Sprite blinkSpriteDark;
    [SerializeField] Sprite blinkSpriteLight;
    [SerializeField] float blinkDelay = 0.25f;
    [SerializeField] int blinkCount = 4;

    Movement movement;

    private void Start()
    {
        movement = FindObjectOfType<Movement>();    
    }

    public void StartAnimation(Vector2 targetPosition)
    {
        StartCoroutine(Blink(targetPosition));
    }

    public IEnumerator FloatToTargetPosition(Vector2 targetPosition)
    {
        while(Mathf.Abs(transform.position.x - targetPosition.x) > Mathf.Epsilon)
        {
            transform.position = Vector2.Lerp(transform.position, targetPosition, 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator Blink(Vector2 targetPosition)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "CompletedTiles";
        for(int i=0; i<blinkCount; i++)
        {
            spriteRenderer.sprite = blinkSpriteLight;
            yield return new WaitForSeconds(blinkDelay);
            spriteRenderer.sprite = blinkSpriteDark;
            yield return new WaitForSeconds(blinkDelay);
        }
        StartCoroutine(FloatToTargetPosition(targetPosition));
        movement.SetPaused(false);
    }

    public float GetTotalBlinkDelay()
    {
        return blinkCount * blinkDelay * 2;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public delegate void ElementClick(byte elementID);
    public delegate void ElementMove(byte elementID, bool isMoving);

    public static event ElementClick elementClick;
    public static event ElementMove elementMove;

    private Color initColor = Color.white;
    public byte elementID { get; set; } = 0;

    public Color InitColor
    {
        get
        {
            return initColor;
        }
        set
        {
            initColor = value;
            this.GetComponent<SpriteRenderer>().color = initColor;
        }
    }
    
    public void SetColor(Color color)
    {
        this.GetComponent<SpriteRenderer>().color = color;
    }

    public void OnMouseUp()
    {
        elementClick(elementID);
    }

    public void MoveTo(Vector2 pos)
    {
        StartCoroutine(Move(transform.localPosition, pos));
    }

    private IEnumerator Move(Vector2 startPos, Vector2 desPos)
    {
        elementMove(elementID, true);
        float totalMovementTime = 1.0f;
        float currentMovementTime = 0.0f;

        while (transform.position.y > 0.0f)
        {
            currentMovementTime += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, new Vector2(transform.localPosition.x, 0.0f), currentMovementTime / totalMovementTime);
            yield return null;
        }

        totalMovementTime = 1.0f;
        currentMovementTime = 0.0f;

        startPos = transform.localPosition;

        while (transform.position.y != desPos.y)
        {
            currentMovementTime += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, new Vector2(startPos.x, desPos.y), currentMovementTime / totalMovementTime);
            yield return null;
        }
        elementMove(elementID, false);
    }
}
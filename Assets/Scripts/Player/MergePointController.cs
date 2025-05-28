using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergePointController : MonoBehaviour
{
    public Vector2 mergePointOffset = new Vector2(0.784f, 0.907f);
    public Transform mergePoint;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        Vector2 offset = mergePointOffset;

        if (spriteRenderer.flipX)
        {
            offset.x = -offset.x;
        }
        Vector2 worldPos = (Vector2)transform.position + offset;
        mergePoint.position = worldPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private SurfaceEffector2D surfaceEffector;

    private void Awake() => Init();

    private void Init()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void HorizontalMove(float inputX, float speed)
    {
        if (surfaceEffector == null)
        {
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(inputX * speed + surfaceEffector.speed, rb.velocity.y);
        }
    }

    public void PlayerJump(float power)
    {
        rb.AddForce(Vector2.up * power, ForceMode2D.Impulse);
    }
}

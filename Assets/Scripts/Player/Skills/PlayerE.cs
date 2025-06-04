using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerE : Skill
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float dashSpeed;
    private Coroutine cantControllTime;

    protected override void Init()
    {
        base.Init();
        rb = player.GetComponent<Rigidbody2D>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void UseSkill()
    {
        cantControllTime = StartCoroutine(CoCantControllTime());

        if (cantControllTime != null)
        {
            if (!spriteRenderer.flipX)
            {
                rb.velocity = new Vector2(dashSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-dashSpeed, rb.velocity.y);
            }
        }
    }

    IEnumerator CoCantControllTime()
    {
        controller.isControllActivated = false;
        yield return new WaitForSeconds(0.3f);
        controller.isControllActivated = true;
    }
}

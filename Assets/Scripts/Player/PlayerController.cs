using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private IView view;
    private PlayerMovement movement;
    private PlayerModel model;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isOnGround = true;

    private void OnEnable() => SubscribeEvents();
    private void Awake() => Init();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            movement.PlayerJump(model.JumpPower);
            isOnGround = false;
            model.IsJumping.Value = true;
        }
    }
    private void FixedUpdate() 
    {
        movement.HorizontalMove(model.MoveSpd);
        if (movement.inputX > 0)
        {
            model.IsRunning.Value = true;
            spriteRenderer.flipX = false;
        }
        else if(movement.inputX < 0)
        {
            model.IsRunning.Value = true;
            spriteRenderer.flipX = true;
        }
        else
        {
            model.IsRunning.Value = false;
        }
    }
    private void OnDisable() => UnsubscribeEvents();

    private void Init()
    {
        model = GetComponent<PlayerModel>();
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isOnGround = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            model.IsJumping.Value = false;
        }
    }

    public void SubscribeEvents()
    {
        model.IsRunning.Subscribe(SetRunAnimation);
    }

    public void UnsubscribeEvents()
    {
        model.IsRunning.Unsubscribe(SetRunAnimation);
    }

    private void SetRunAnimation(bool value) => animator.SetBool("IsRun", value);
}

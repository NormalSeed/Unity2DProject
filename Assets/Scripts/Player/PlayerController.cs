using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IView view;
    private PlayerMovement movement;
    private PlayerModel model;

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
        
        //Test---
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(1);
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
        view = GetComponent<PlayerView>();
        movement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        model.CurHp.Value = model.MaxHp;

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

    public void TakeDamage(int damage)
    {
        // TODO: 공격을 받으면 model의 CurHp가 감소하게 하고 UI의 HP 갯수를 하나 줄여야 함
        if (model.CurHp.Value > 0) model.CurHp.Value -= damage;
    }

    public void SubscribeEvents()
    {
        model.IsRunning.Subscribe(view.PlayRunAnimation);
        model.CurHp.Subscribe(view.UpdateHpUI);
    }

    public void UnsubscribeEvents()
    {
        model.IsRunning.Unsubscribe(view.PlayRunAnimation);
        model.CurHp.Unsubscribe(view.UpdateHpUI);
    }
}

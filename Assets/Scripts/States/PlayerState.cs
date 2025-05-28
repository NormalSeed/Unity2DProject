using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : BaseState
{
    protected PlayerController controller;

    public PlayerState(PlayerController _controller)
    {
        controller = _controller;
    }
    public override void Enter()
    {
        
    }
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && controller.isOnGround)
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Jump]);
    }
    public override void Exit()
    {

    }
}

public class Player_Idle : PlayerState
{
    public Player_Idle(PlayerController _controller) : base(_controller)
    {
        HasPhysics = false;
    }
    public override void Enter()
    {
        controller.view.PlayAnimation(controller.IDLE_HASH);
        controller.movement.rb.velocity = Vector2.zero;
    }
    public override void Update()
    {
        base.Update();

        if (Mathf.Abs(controller.inputX) > 0.01f)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Run]);
        }
    }
}
public class Player_Run : PlayerState
{
    public Player_Run(PlayerController _controller) : base(_controller)
    {
        HasPhysics = true;
    }

    public override void Enter()
    {
        controller.view.PlayAnimation(controller.RUN_HASH);
    }
    public override void Update()
    {
        base.Update();

        if (Mathf.Abs(controller.inputX) < 0.01f)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Idle]);
        }

        if (controller.inputX > 0)
        {
            controller.spriteRenderer.flipX = false;
        }
        else if (controller.inputX < 0)
        {
            controller.spriteRenderer.flipX = true;
        }
    }
    public override void FixedUpdate()
    {
        controller.movement.HorizontalMove(controller.inputX, controller.model.MoveSpd);
    }
}
public class Player_Jump : PlayerState
{
    public Player_Jump(PlayerController _controller) : base(_controller)
    {
        HasPhysics = true;
    }

    public override void Enter()
    {
        //TODO: 점프 애니메이션 필요
        //controller.view.PlayAnimation(controller.JUMP_HASH);
        controller.movement.PlayerJump(controller.model.JumpPower);
        controller.isOnGround = false;
    }

    public override void Update()
    {
        if (controller.isOnGround)
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Idle]);

        if (controller.inputX > 0)
        {
            controller.spriteRenderer.flipX = false;
        }
        else if (controller.inputX < 0)
        {
            controller.spriteRenderer.flipX = true;
        }
    }
    public override void FixedUpdate()
    {
        controller.movement.HorizontalMove(controller.inputX, controller.model.MoveSpd);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherState : BaseState
{
    protected DasherController controller;

    public DasherState(DasherController _controller)
    {
        controller = _controller;
    }
    public override void Enter()
    {

    }

    public override void Update()
    {
        if (controller.isDetect)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Detect]);
        }
    }

    public override void Exit()
    {
        
    }
}

public class Dasher_Idle : DasherState
{
    public Dasher_Idle(DasherController _controller) : base(_controller)
    {
        HasPhysics = false;
    }

    public override void Enter()
    {
        controller.view.PlayAnimation(controller.IDLE_HASH);
    }

    public override void Update()
    {
        base.Update();
        if (controller.nextMove != 0)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Run]);
        }
    }
}

public class Dasher_Run : DasherState
{
    public Dasher_Run(DasherController _controller) : base(_controller)
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
        if (controller.nextMove == 0)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Idle]);
        }
        if (controller.nextMove != 0)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Run]);
        }
    }

    public override void FixedUpdate()
    {
        if (controller.isRight)
        {
            controller.spriteRenderer.flipX = false;
        }
        else
        {
            controller.spriteRenderer.flipX = true;
        }
        controller.movement.HorizontalMove(controller.isFlip, controller.model.MoveSpd);
    }
}

public class Dasher_Detect : DasherState
{
    public Dasher_Detect(DasherController _controller) : base(_controller)
    {
        HasPhysics = true;
    }
    public override void Enter()
    {
        controller.view.PlayAnimation(controller.RUN_HASH);
    }

    public override void Update()
    {
        if (!controller.isDetect)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Idle]);
        }

        if (controller.isDetect && 
            controller.movement.distance <= controller.movement.attackRange &&
            !controller.isAttack)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Attack]);
        }
    }

    public override void FixedUpdate()
    {
        if (controller.isRight)
        {
            controller.spriteRenderer.flipX = false;
        }
        else
        {
            controller.spriteRenderer.flipX = true;
        }
        controller.movement.FollowPlayer(controller.targetTransform, controller.model.MoveSpd);
    }

    public override void Exit()
    {
        controller.movement.distance = controller.movement.detectRange;
        controller.Invoke("MovingIntelligence", 0);
    }
}

public class Dasher_Attack : DasherState
{
    public Dasher_Attack(DasherController _controller) : base(_controller)
    {
        HasPhysics = false;
    }

    private float attackTime;

    public override void Enter()
    {
        controller.view.PlayAnimation(controller.ATTACK_HASH);
        controller.movement.attackDir = controller.movement.followDir;
        attackTime = 1;
        controller.AttackSkill();
    }

    public override void Update()
    {
        attackTime -= Time.deltaTime;
        if (attackTime < 0)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Idle]);
        }
    }

    public override void Exit()
    {
        controller.isAttack = false;
        controller.movement.attackDir = 0;
        controller.movement.rb.velocity = new Vector2(0, controller.movement.rb.velocity.y);
        controller.Invoke("MovingIntelligence", 0);
    }
}

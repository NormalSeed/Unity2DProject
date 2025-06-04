using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1State : BaseState
{
    protected Boss1Controller controller;

    public Boss1State(Boss1Controller _controller)
    {
        controller = _controller;
    }

    public override void Enter()
    {
        
    }

    public override void Update()
    {
        if (controller.isTerrorized)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Terrorized]);
        }

        if (controller.isDetect && !controller.isTerrorized && !controller.isHalf)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Detect]);
        }

        if (controller.isHalf)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.SpAttack1]);
        }
    }

    public override void Exit()
    {
        
    }
}

public class Boss1_Idle : Boss1State
{
    public Boss1_Idle(Boss1Controller _controller) : base(_controller)
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
        if (controller.nextMove != 0 && !controller.isHalf)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Run]);
        }
    }
}

public class Boss1_Run : Boss1State
{
    public Boss1_Run(Boss1Controller _controller) : base(_controller)
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

public class Boss1_Detect : Boss1State
{
    public Boss1_Detect(Boss1Controller _controller) : base(_controller)
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
        if (!controller.isDetect)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Idle]);
        }

        if (controller.isDetect &&
            controller.movement.distance <= controller.movement.attackRange &&
            !controller.isAttack &&
            !controller.isTerrorized)
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

public class Boss1_Attack : Boss1State
{
    public Boss1_Attack(Boss1Controller _controller) : base(_controller)
    {
        HasPhysics = false;
    }

    private float attackTime;

    public override void Enter()
    {
        controller.view.PlayAnimation(controller.ATTACK_HASH);
        controller.movement.attackDir = controller.movement.followDir;
        attackTime = 0.6f;
        controller.AttackSkill();
    }

    public override void Update()
    {
        attackTime -= Time.deltaTime;

        if (attackTime < 0)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Idle]);
        }

        if (controller.isTerrorized)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Terrorized]);
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

public class Boss1_Terrorized : Boss1State
{
    public Boss1_Terrorized(Boss1Controller _controller) : base(_controller)
    {
        HasPhysics = true;
    }

    private float duration;

    public override void Enter()
    {
        duration = 1.5f;
        controller.view.animator.speed = 0.5f;
        controller.view.PlayAnimation(controller.RUN_HASH);
    }

    public override void Update()
    {
        duration -= Time.deltaTime;
        if (duration < 0)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Idle]);
        }
    }

    public override void FixedUpdate()
    {
        controller.movement.TerrorMovement(controller.targetTransform, controller.model.MoveSpd);
    }

    public override void Exit()
    {
        controller.view.animator.speed = 1;
        controller.isTerrorized = false;
        controller.Invoke("MovingIntelligence", 0);
    }
}

public class Boss1_SpAttack1 : Boss1State
{
    private float markCooldown = 2f;

    public Boss1_SpAttack1(Boss1Controller _controller) : base(_controller)
    {
        HasPhysics = false;
    }

    public override void Enter()
    {
        controller.view.PlayAnimation(controller.IDLE_HASH);
        controller.SpAttack1();
    }

    public override void Update()
    {
        markCooldown -= Time.deltaTime;
        if (markCooldown < 0 && !controller.isBerserk)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Idle]);
        }
        else if (markCooldown < 0 && controller.isBerserk)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.SpAttack2]);
        }
        if (controller.isTerrorized)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Terrorized]);
        }
    }

    public override void Exit()
    {
        markCooldown = 2f;
    }
}

public class Boss1_SpAttack2 : Boss1State
{
    private float cooldown = 3f;
    public Boss1_SpAttack2(Boss1Controller _controller) : base(_controller)
    {
        HasPhysics = false;
    }

    public override void Enter()
    {
        controller.view.PlayAnimation(controller.IDLE_HASH);
        controller.SpAttack2();
    }

    public override void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown < 0)
        {
            controller.stateMachine.ChangeState(controller.stateMachine.stateDic[EState.Idle]);
        }
    }

    public override void Exit()
    {
        controller.isBerserk = false;
        controller.isDamagable = true;
    }
}
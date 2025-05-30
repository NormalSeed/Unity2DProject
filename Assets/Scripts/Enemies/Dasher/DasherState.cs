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
        controller.movement.HorizontalMove(controller.isFlip, controller.model.MoveSpd);
    }
}

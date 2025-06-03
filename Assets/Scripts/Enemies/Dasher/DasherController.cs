using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherController : Enemy
{
    public DasherModel dasherModel;
    public readonly int IDLE_HASH = Animator.StringToHash("DasherIdle");
    public readonly int RUN_HASH = Animator.StringToHash("DasherRun");
    public readonly int ATTACK_HASH = Animator.StringToHash("DasherAttack");

    protected override void Init()
    {
        base.Init();
        //DasherModel에 EnemyModel에 없는 DashSpd가 있으므로 dasherModel을 새로 정의할 필요가 있음
        dasherModel = model as DasherModel;
    }

    protected override void StateMachineInit()
    {
        stateMachine = new StateMachine();
        stateMachine.stateDic.Add(EState.Idle, new Dasher_Idle(this));
        stateMachine.stateDic.Add(EState.Run, new Dasher_Run(this));
        stateMachine.stateDic.Add(EState.Detect, new Dasher_Detect(this));
        stateMachine.stateDic.Add(EState.Attack, new Dasher_Attack(this));
        stateMachine.stateDic.Add(EState.Terrorized, new Dasher_Terrorized(this));

        stateMachine.CurState = stateMachine.stateDic[EState.Idle];
    }

    public override void TakeDamage(int damage)
    {
        if (model.CurHp.Value > 0) model.CurHp.Value -= damage;
        if (model.CurHp.Value <= 0) gameObject.SetActive(false);
    }

    public override void AttackSkill()
    {
        if (model is DasherModel)
        {
            movement.rb.AddForce(new Vector2(movement.attackDir * dasherModel.DashSpd * 1.5f, movement.rb.velocity.y), ForceMode2D.Impulse);
            isAttack = true;
        }
    }
}

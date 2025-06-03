using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : Enemy
{
    public Boss1Model boss1Model;

    public readonly int IDLE_HASH = Animator.StringToHash("Boss1Idle");
    public readonly int RUN_HASH = Animator.StringToHash("Boss1Run");
    public readonly int ATTACK_HASH = Animator.StringToHash("Boss1Attack");
    public readonly int SPATTACK1_HASH = Animator.StringToHash("Bos1SpAttack1");
    public readonly int SPATTACK2_HASH = Animator.StringToHash("Bos1SpAttack2");

    protected void OnEnable()
    {
        model.MaxHp = 30;
    }
    protected override void Init()
    {
        base.Init();
        boss1Model = model as Boss1Model;
    }
    protected override void StateMachineInit()
    {
        stateMachine = new StateMachine();
        stateMachine.stateDic.Add(EState.Idle, new Boss1_Idle(this));
        stateMachine.stateDic.Add(EState.Run, new Boss1_Run(this));
        stateMachine.stateDic.Add(EState.Detect, new Boss1_Detect(this));
        stateMachine.stateDic.Add(EState.Attack, new Boss1_Attack(this));
        //stateMachine.stateDic.Add(EState.SpAttack1, new Boss1_SpAttack1(this));
        //stateMachine.stateDic.Add(EState.SpAttack2, new Boss1_SpAttack2(this));
        stateMachine.stateDic.Add(EState.Terrorized, new Boss1_Terrorized(this));

        stateMachine.CurState = stateMachine.stateDic[EState.Idle];
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    protected override void MovingIntelligence()
    {
        base.MovingIntelligence();
    }

    public override void TakeDamage(int damage)
    {
        if (model.CurHp.Value > 0) model.CurHp.Value -= damage;
        if (model.CurHp.Value <= 0) gameObject.SetActive(false);
    }

    public override void AttackSkill()
    {
        if (model is Boss1Model)
        {
            movement.rb.AddForce(new Vector2(movement.attackDir * boss1Model.DashSpd, movement.rb.velocity.y), ForceMode2D.Impulse);
            isAttack = true;
        }
    }

    public void SpAttack1()
    {

    }

    public void SpAttack2()
    {

    }
}

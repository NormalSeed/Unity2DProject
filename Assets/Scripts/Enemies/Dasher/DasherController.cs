using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherController : Enemy
{
    public DasherModel dasherModel;
    public readonly int IDLE_HASH = Animator.StringToHash("DasherIdle");
    public readonly int RUN_HASH = Animator.StringToHash("DasherRun");
    public readonly int ATTACK_HASH = Animator.StringToHash("DasherAttack");

    private void Awake() => Init();

    private void Update()
    {
        if (nextMove == -1)
        {
            isRight = false;
        }
        else if (nextMove == 1)
        {
            isRight = true;
        }
        isFlip = spriteRenderer.flipX;

        Patrol();
        DetectPlayer();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    protected override void Init()
    {
        base.Init();
        //DasherModel�� EnemyModel�� ���� DashSpd�� �����Ƿ� dasherModel�� ���� ������ �ʿ䰡 ����
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAttack && collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb == null) return;
            rb.velocity = Vector2.zero;

            rb.AddForce(new Vector2(movement.attackDir, 0) * attackSpeed * 4f, ForceMode2D.Impulse);
        }
        else if (!isAttack && collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb == null) return;

            Vector2 normal = collision.contacts[0].normal;
            Vector2 reflectedDir = Vector2.Reflect(rb.velocity.normalized, normal);
            rb.velocity = Vector2.zero;

            rb.AddForce(reflectedDir * 10f, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("TerrorTrigger"))
        {
            Debug.Log("���� �߻�");
            InTerror();
        }
    }

    private void MovingIntelligence()
    {
        nextMove = Random.Range(-1, 2);
        float time = Random.Range(2f, 4f);
        Invoke("MovingIntelligence", time);
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

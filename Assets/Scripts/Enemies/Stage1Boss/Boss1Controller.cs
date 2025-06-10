using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss1Controller : Enemy
{
    public Boss1Model boss1Model;

    [SerializeField] GameObject[] swordMarks;
    private List<GameObject> enabledMarks = new();
    public bool isHalf;
    public bool isBerserk;
    public bool isBerserkFirst;
    public bool isDamagable;
    private new Collider2D collider;
    public Coroutine coSpawnMark;
    public Coroutine coUltimate;
    private int prevSpawnedMark;

    public readonly int IDLE_HASH = Animator.StringToHash("Boss1Idle");
    public readonly int RUN_HASH = Animator.StringToHash("Boss1Run");
    public readonly int ATTACK_HASH = Animator.StringToHash("Boss1Attack");
    public readonly int SPATTACK1_HASH = Animator.StringToHash("Bos1SpAttack1");
    public readonly int SPATTACK2_HASH = Animator.StringToHash("Bos1SpAttack2");

    protected override void Init()
    {
        base.Init();
        collider = GetComponent<Collider2D>();
        boss1Model = model as Boss1Model;
        boss1Model.MaxHp = 60;
        boss1Model.CurHp.Value = boss1Model.MaxHp;
        isHalf = false;
        isBerserk = false;
        isBerserkFirst = true;
        isDamagable = true;
        prevSpawnedMark = -1;
    }
    protected override void StateMachineInit()
    {
        stateMachine = new StateMachine();
        stateMachine.stateDic.Add(EState.Idle, new Boss1_Idle(this));
        stateMachine.stateDic.Add(EState.Run, new Boss1_Run(this));
        stateMachine.stateDic.Add(EState.Detect, new Boss1_Detect(this));
        stateMachine.stateDic.Add(EState.Attack, new Boss1_Attack(this));
        stateMachine.stateDic.Add(EState.SpAttack1, new Boss1_SpAttack1(this));
        stateMachine.stateDic.Add(EState.SpAttack2, new Boss1_SpAttack2(this));
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
        if (boss1Model.CurHp.Value > 0 && isDamagable) boss1Model.CurHp.Value -= damage;
        if (boss1Model.CurHp.Value <= 0) gameObject.SetActive(false);
        if ((float)boss1Model.CurHp.Value / boss1Model.MaxHp <= 0.5f)
        {
            isHalf = true;
        }
        if ((float)boss1Model.CurHp.Value / boss1Model.MaxHp <= 0.1f && isBerserkFirst)
        {
            isBerserk = true;
            isBerserkFirst = false;
        }

        if (boss1Model.CurHp.Value <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        GameManager.Instance.AddScore(500);
        GameManager.Instance.AddScore(Convert.ToInt32(GameManager.Instance.timeRemain * 100));
        GameManager.Instance.isGameCleared = true;
        GameManager.Instance.isStageStarted = false;
        GameManager.Instance.LoadScene("StageClear");
    }

    public override void AttackSkill()
    {
        if (model is Boss1Model)
        {
            movement.rb.AddForce(new Vector2(movement.attackDir * boss1Model.DashSpd, movement.rb.velocity.y), ForceMode2D.Impulse);
            isAttack = true;
        }
    }

    private void DetectTargetInAttackZone(Vector2 start, Vector2 end)
    {
        RaycastHit2D hit = Physics2D.Raycast(start, (end - start).normalized, Vector2.Distance(start, end), LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            PlayerController player = hit.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
        }
    }

    public void SpAttack1()
    {
        coSpawnMark = StartCoroutine(CoSpawnMark());
    }

    private IEnumerator CoSpawnMark()
    {
        yield return new WaitForSeconds(0.5f);

        int rand;
        do
        {
            rand = UnityEngine.Random.Range(0, 6);
        } 
        while 
            (rand == prevSpawnedMark);
        prevSpawnedMark = rand;

        swordMarks[rand].SetActive(true);
        enabledMarks.Add(swordMarks[rand]);

        yield return new WaitForSeconds(0.5f);

        Vector2 originPos = new Vector2(transform.position.x + 1.1f, transform.position.y);
        if (enabledMarks.Count > 0)
        {
            Vector2 targetPos = enabledMarks[0].transform.position;

            if (targetPos.x - originPos.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }

            collider.enabled = false;

            transform.position = targetPos;
            DetectTargetInAttackZone(originPos, targetPos);
            enabledMarks[0].gameObject.SetActive(false);
            enabledMarks.RemoveAt(0);

            collider.enabled = true;
        }
        coSpawnMark = null;
    }

    public void SpAttack2()
    {
        isDamagable = false;
        coUltimate = StartCoroutine(CoUltimateSkill());
    }

    private IEnumerator CoUltimateSkill()
    {
        for (int i = 0; i < 6; i++)
        {
            swordMarks[i].SetActive(true);
            enabledMarks.Add(swordMarks[i]);
        }
        yield return new WaitForSeconds(0.5f);

        int j = 5;

        while (j >= 0)
        {
            yield return new WaitForSeconds(0.3f);

            Vector2 originPos = new Vector2 (transform.position.x + 1.1f, transform.position.y);
            Vector2 targetPos = enabledMarks[j].transform.position;

            if (targetPos.x - originPos.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }

            collider.enabled = false;

            transform.position = targetPos;
            DetectTargetInAttackZone(originPos, targetPos);
            enabledMarks[j].gameObject.SetActive(false);
            enabledMarks.RemoveAt(j);

            collider.enabled = true;

            j--;
        }
        coUltimate = null;
    }
}

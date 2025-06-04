using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class PlayerController : MonoBehaviour
{
    public PlayerView view;
    public PlayerMovement movement;
    public PlayerModel model;
    public StateMachine stateMachine;

    public SpriteRenderer spriteRenderer;

    public float inputX;
    public bool isOnGround = true;
    public float generalSkillCool = 0.5f;
    public bool isDamagable = true;
    public Coroutine coDamagable;
    public bool isControllActivated = true;

    private Coroutine coReactivate;

    public readonly int IDLE_HASH = Animator.StringToHash("idle");
    public readonly int RUN_HASH = Animator.StringToHash("run");
    // TODO: 점프 애니메이션 필요
    public readonly int JUMP_HASH = Animator.StringToHash("jump");


    private void OnEnable() => SubscribeEvents();
    private void Awake() => Init();
    private void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        stateMachine.Update();
    }
    private void FixedUpdate() 
    {
        stateMachine.FixedUpdate();
        if (movement.rb.velocity.y < 0)
        {
            Debug.DrawRay(movement.rb.position, Vector3.down, Color.blue);
            RaycastHit2D hit = Physics2D.Raycast(movement.rb.position, Vector3.down, 1, LayerMask.GetMask("Ground"));
            if (hit.collider != null)
            {
                if (hit.distance < 0.5f)
                {
                    isOnGround = true;
                }
            }
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
        StateMachineInit();

        isOnGround = true;
    }

    private void StateMachineInit()
    {
        stateMachine = new StateMachine();
        stateMachine.stateDic.Add(EState.Idle, new Player_Idle(this));
        stateMachine.stateDic.Add(EState.Run, new Player_Run(this));
        stateMachine.stateDic.Add(EState.Jump, new Player_Jump(this));
        stateMachine.stateDic.Add(EState.CantControll, new Player_CantControll(this));

        stateMachine.CurState = stateMachine.stateDic[EState.Idle];

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isControllActivated = false;
            coReactivate = StartCoroutine(CoReactivate());
            TakeDamage(1);
        }
    }

    private IEnumerator CoReactivate()
    {
        yield return new WaitForSeconds(0.5f);
        isControllActivated = true;
    }

    public void TakeDamage(int damage)
    {
        // TODO: 공격을 받으면 model의 CurHp가 감소하게 하고 UI의 HP 갯수를 하나 줄여야 함
        if (model.CurHp.Value > 0 && isDamagable) model.CurHp.Value -= damage;
        coDamagable = StartCoroutine(CoDamagable());

        if (model.CurHp.Value <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        GameManager.Instance.isGameOver = true;
        GameManager.Instance.LoadScene("GameOver");
    }

    public IEnumerator CoDamagable()
    {
        isDamagable = false;
        yield return new WaitForSeconds(1f);
        isDamagable = true;
    }

    public void SubscribeEvents()
    {
        model.CurHp.Subscribe(view.UpdateHPUI);
    }

    public void UnsubscribeEvents()
    {
        model.CurHp.Unsubscribe(view.UpdateHPUI);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool isControllActivated = true;

    private Coroutine coReactivate;

    public readonly int IDLE_HASH = Animator.StringToHash("idle");
    public readonly int RUN_HASH = Animator.StringToHash("run");
    // TODO: ���� �ִϸ��̼� �ʿ�
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
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
        // TODO: ������ ������ model�� CurHp�� �����ϰ� �ϰ� UI�� HP ������ �ϳ� �ٿ��� ��
        if (model.CurHp.Value > 0) model.CurHp.Value -= damage;
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

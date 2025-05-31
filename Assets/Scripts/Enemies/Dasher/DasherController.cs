using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherController : Enemy
{
    public DasherModel model;
    public DasherView view;
    public DasherMovement movement;
    public StateMachine stateMachine;
    public SpriteRenderer spriteRenderer;
    public int nextMove;
    public bool isFlip;
    public bool isDetect;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float detectRadius;
    [SerializeField] private LayerMask playerLayer;
    public Collider2D detectedTarget;
    public Transform targetTransform;

    public readonly int IDLE_HASH = Animator.StringToHash("DasherIdle");
    public readonly int RUN_HASH = Animator.StringToHash("DasherRun");
    public readonly int ATTACK_HASH = Animator.StringToHash("DasherAttack");

    private void Awake() => Init();

    private void Update()
    {
        if (nextMove == -1)
        {
            spriteRenderer.flipX = true;
        }
        else if (nextMove == 1)
        {
            spriteRenderer.flipX = false;
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

    private void Init()
    {
        model = GetComponent<DasherModel>();
        view = GetComponent<DasherView>();
        movement = GetComponent<DasherMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        model.CurHp.Value = model.MaxHp;

        StateMachineInit();

        Invoke("MovingIntelligence", 0.1f);
    }

    private void StateMachineInit()
    {
        stateMachine = new StateMachine();
        stateMachine.stateDic.Add(EState.Idle, new Dasher_Idle(this));
        stateMachine.stateDic.Add(EState.Run, new Dasher_Run(this));
        stateMachine.stateDic.Add(EState.Detect, new Dasher_Detect(this));
        //stateMachine.stateDic.Add(EState.Dash, new Dasher_Attack(this));

        stateMachine.CurState = stateMachine.stateDic[EState.Idle];
    }

    private void MovingIntelligence()
    {
        nextMove = Random.Range(-1, 2);
        float time = Random.Range(2f, 4f);
        Invoke("MovingIntelligence", time);
    }

    public void Patrol()
    {
        Vector2 rayOrigin = transform.position + new Vector3(movement.patrolVec.x, 1);
        Debug.DrawRay(rayOrigin, Vector2.down * 2f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 2f, groundLayer);
        if (hit.collider == null) // 앞에 Ground가 없으면
        {
            CancelInvoke();
            Invoke("MovingIntelligence", 0);
        }
    }

    public override void TakeDamage(int damage)
    {
        if (model.CurHp.Value > 0) model.CurHp.Value -= damage;
        if (model.CurHp.Value <= 0) gameObject.SetActive(false);
    }

    public override void DetectPlayer()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, detectRadius, playerLayer);
        
        if (collider != null && collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("탐지 성공");
            CancelInvoke();
            detectedTarget = collider;
            isDetect = true;
            targetTransform = collider.transform;
        }
        else if (detectedTarget != null)
        {
            detectedTarget = null;
            // Invoke("MoveIntelligence", 0);
            isDetect = false;
            targetTransform = null;
        }
    }

    public override void AttackSkill()
    {
        
    }
}

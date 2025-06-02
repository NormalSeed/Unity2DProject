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
    public bool isAttack;
    public bool isRight;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float detectRadius;
    [SerializeField] private LayerMask playerLayer;
    public Collider2D detectedTarget;
    public Transform targetTransform;
    public float attackSpeed = 5f;

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
        stateMachine.stateDic.Add(EState.Attack, new Dasher_Attack(this));

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
        Vector2 rayOriginRight = transform.position + new Vector3(1, 1);
        Debug.DrawRay(rayOriginRight, Vector2.right * movement.detectRange, Color.red);
        Vector2 rayOriginLeft = transform.position + new Vector3(-1, 1);
        Debug.DrawRay(rayOriginLeft, -Vector2.right * movement.detectRange, Color.yellow);
        RaycastHit2D hitRight = Physics2D.Raycast(rayOriginRight, Vector2.right, movement.detectRange, playerLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(rayOriginLeft, -Vector2.right, movement.detectRange, playerLayer);
        
        if (hitRight.collider != null)
        {
            Debug.Log("탐지 성공");
            CancelInvoke();
            detectedTarget = hitRight.collider;
            isDetect = true;
            targetTransform = hitRight.transform;
            isRight = true;
        }
        else if (hitLeft.collider != null)
        {
            Debug.Log("탐지 성공");
            CancelInvoke();
            detectedTarget = hitLeft.collider;
            isDetect = true;
            targetTransform = hitLeft.transform;
            isRight = false;
        }
        else if (hitRight.collider == null && hitLeft.collider == null && detectedTarget != null)
        {
            detectedTarget = null;
            isDetect = false;
            targetTransform = null;
        }
    }

    public override void AttackSkill()
    {
        movement.rb.AddForce(new Vector2(movement.attackDir * attackSpeed * 1.5f, movement.rb.velocity.y), ForceMode2D.Impulse);
        isAttack = true;
    }
}

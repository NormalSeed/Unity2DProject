using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public EnemyModel model;
    public EnemyView view;
    public EnemyMovement movement;
    public StateMachine stateMachine;
    public SpriteRenderer spriteRenderer;
    public int nextMove;
    public float movingLatency;
    public bool isFlip;
    public bool isDetect;
    public bool isAttack;
    public bool isRight;
    public bool isTerrorized;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerLayer;
    public Collider2D detectedTarget;
    public Transform targetTransform;
    public float attackSpeed = 5f;

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

    protected virtual void Init()
    {
        model = GetComponent<EnemyModel>();
        view = GetComponent<EnemyView>();
        movement = GetComponent<EnemyMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        model.CurHp.Value = model.MaxHp;
        isTerrorized = false;

        StateMachineInit();

        if (!IsInvoking("MovingIntelligence"))
        {
            this.Invoke(nameof(MovingIntelligence), 0.1f);
        }
    }

    protected abstract void StateMachineInit();

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
            Debug.Log("공포 발생");
            InTerror();
        }
    }

    protected virtual void MovingIntelligence()
    {
        this.CancelInvoke("MovingIntelligence");
        Debug.Log($"Invoke 실행됨 - 호출된 위치: {GetType().Name}");
        nextMove = Random.Range(-1, 2);
        movingLatency = Random.Range(2f, 4f);
        this.Invoke(nameof(MovingIntelligence), movingLatency);
    }

    public void Patrol()
    {
        Vector2 rayOrigin = transform.position + new Vector3(movement.patrolVec.x, 1);
        Debug.DrawRay(rayOrigin, Vector2.down * 2f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 2f, groundLayer);
        if (hit.collider == null) // 앞에 Ground가 없으면
        {
            this.CancelInvoke("MovingIntelligence");
            this.Invoke(nameof(MovingIntelligence), 0);
        }
    }

    public void DetectPlayer()
    {
        Vector2 rayOriginRight = transform.position + new Vector3(1, 1);
        Debug.DrawRay(rayOriginRight, Vector2.right * movement.detectRange, Color.red);
        Vector2 rayOriginLeft = transform.position + new Vector3(-1, 1);
        Debug.DrawRay(rayOriginLeft, -Vector2.right * movement.detectRange, Color.yellow);

        int layerMask = LayerMask.GetMask("Player", "Ground");

        RaycastHit2D hitRight = Physics2D.Raycast(rayOriginRight, Vector2.right, movement.detectRange, layerMask);
        RaycastHit2D hitLeft = Physics2D.Raycast(rayOriginLeft, -Vector2.right, movement.detectRange, layerMask);

        if (hitRight.collider != null && hitRight.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("탐지 성공");
            this.CancelInvoke("MovingIntelligence");
            detectedTarget = hitRight.collider;
            isDetect = true;
            targetTransform = hitRight.transform;
            isRight = true;
        }
        else if (hitLeft.collider != null && hitLeft.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("탐지 성공");
            this.CancelInvoke("MovingIntelligence");
            detectedTarget = hitLeft.collider;
            isDetect = true;
            targetTransform = hitLeft.transform;
            isRight = false;
        }
        else
        {
            detectedTarget = null;
            isDetect = false;
            targetTransform = null;
        }
    }

    public void InTerror()
    {
        Debug.Log("공포 발생");
        TakeDamage(5);
        isTerrorized = true;
        spriteRenderer.flipX = !spriteRenderer.flipX;
        this.CancelInvoke("MovingIntelligence");
    }
    public abstract void TakeDamage(int damage);

    public abstract void AttackSkill();
}

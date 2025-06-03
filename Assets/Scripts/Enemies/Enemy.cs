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
    public bool isFlip;
    public bool isDetect;
    public bool isAttack;
    public bool isRight;
    public bool isTerrorized;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float detectRadius;
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

        Invoke("MovingIntelligence", 0.1f);
    }

    protected abstract void StateMachineInit();

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
            Debug.Log("공포 발생");
            InTerror();
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

    public void DetectPlayer()
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

    public void InTerror()
    {
        Debug.Log("공포 발생");
        TakeDamage(3);
        isTerrorized = true;
        spriteRenderer.flipX = !spriteRenderer.flipX;
        CancelInvoke();
    }
    public abstract void TakeDamage(int damage);

    public abstract void AttackSkill();
}

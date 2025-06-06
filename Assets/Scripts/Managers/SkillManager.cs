using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public bool IsUsingSkill;

    public PlayerQ playerQ;
    public PlayerW playerW;
    public PlayerE playerE;
    public PlayerR playerR;

    public Animator animator;
    public SpriteRenderer spriteRenderer;
    [SerializeField] public GameObject rSkillTerritory;
    [SerializeField] public GameObject SkillWindow;
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        playerQ = GetComponent<PlayerQ>();
        playerW = GetComponent<PlayerW>();
        playerE = GetComponent<PlayerE>();
        playerR = GetComponent<PlayerR>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        IsUsingSkill = false;
    }

    private void Update()
    {
        
    }
}

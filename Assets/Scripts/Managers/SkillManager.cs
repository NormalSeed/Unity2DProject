using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkillManager : Singleton<SkillManager>
{
    public bool IsUsingSkill;

    public GameObject player;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Init()
    {
        base.SingletonInit();
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

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public virtual void FindPlayer()
    {
        player = GameObject.Find("Player");
        if (player != null)
        {
            foreach (Skill skill in FindObjectsOfType<Skill>())
            {
                skill.player = player;
                skill.controller = player.GetComponent<PlayerController>();
                skill.bulletController = player.GetComponentInChildren<BulletController>();
                skill.rb = player.GetComponent<Rigidbody2D>();
                skill.playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayer();
    }
}

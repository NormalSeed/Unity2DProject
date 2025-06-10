using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Skill : MonoBehaviour
{
    public GameObject player;
    public PlayerController controller;
    public BulletController bulletController;
    public Rigidbody2D rb;
    public SpriteRenderer playerSpriteRenderer;
    [SerializeField] public float cooldown;
    public float cooldownTimer;

    protected virtual void Awake() => Init();

    protected virtual void Init()
    {

    }

    protected virtual void Update()
    {
        if(cooldownTimer >= 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        return false;
    }

    public virtual void UseSkill()
    {

    }
}

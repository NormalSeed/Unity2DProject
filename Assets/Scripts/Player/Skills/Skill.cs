using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public GameObject player;
    public PlayerController controller;
    [SerializeField] public float cooldown;
    public float cooldownTimer;

    protected virtual void Awake() => Init();

    protected virtual void Init()
    {
        player = GameObject.Find("Player");
        controller = player.GetComponent<PlayerController>();
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

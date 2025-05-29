using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    public bool IsUsingSkill;

    public PlayerQ playerQ;
    public PlayerW playerW;
    public PlayerE playerE;
    public PlayerR playerR;
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        base.SingletonInit();
        playerQ = GetComponent<PlayerQ>();
        playerW = GetComponent<PlayerW>();
        playerE = GetComponent<PlayerE>();
        playerR = GetComponent<PlayerR>();

        IsUsingSkill = false;
    }
}

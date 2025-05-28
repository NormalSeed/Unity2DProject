using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    [SerializeField] private PlayerController controller;
    private void Awake() => Init();

    private void Init()
    {
        base.SingletonInit();
    }
}

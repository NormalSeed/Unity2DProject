using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    private PlayerController controller;
    public bool canUseSkill;
    private Coroutine coGeneralSkillCool;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        canUseSkill = true;
    }
    private void Update()
    {
        if (canUseSkill && controller.isControllActivated)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SkillManager.Instance.playerQ.CanUseSkill();
                canUseSkill = false;
                coGeneralSkillCool = StartCoroutine(CoGeneralSkillCool());
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                SkillManager.Instance.playerW.CanUseSkill();
                canUseSkill = false;
                coGeneralSkillCool = StartCoroutine(CoGeneralSkillCool());
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                SkillManager.Instance.playerE.CanUseSkill();
                canUseSkill = false;
                coGeneralSkillCool = StartCoroutine(CoGeneralSkillCool());
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                SkillManager.Instance.playerR.CanUseSkill();
                canUseSkill = false;
                coGeneralSkillCool = StartCoroutine(CoGeneralSkillCool());
            }
        }
    }

    IEnumerator CoGeneralSkillCool()
    {
        yield return new WaitForSeconds(0.7f);
        canUseSkill = true;
    }
}

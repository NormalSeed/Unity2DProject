using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    private PlayerController controller;
    private SkillManager skillManager;
    private SkillView view;

    public bool canUseSkill;
    private Coroutine coGeneralSkillCool;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        skillManager = GetComponentInChildren<SkillManager>();
        view = GetComponent<SkillView>();
        canUseSkill = true;
    }
    private void Update()
    {
        if (canUseSkill && controller.isControllActivated)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                skillManager.playerQ.CanUseSkill();
                canUseSkill = false;
                coGeneralSkillCool = StartCoroutine(CoGeneralSkillCool());
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                skillManager.playerW.CanUseSkill();
                canUseSkill = false;
                coGeneralSkillCool = StartCoroutine(CoGeneralSkillCool());
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                skillManager.playerE.CanUseSkill();
                canUseSkill = false;
                coGeneralSkillCool = StartCoroutine(CoGeneralSkillCool());
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                skillManager.playerR.CanUseSkill();
                canUseSkill = false;
                coGeneralSkillCool = StartCoroutine(CoGeneralSkillCool());
            }
        }

        view.UpdateUI();
    }

    IEnumerator CoGeneralSkillCool()
    {
        yield return new WaitForSeconds(0.7f);
        canUseSkill = true;
    }
}

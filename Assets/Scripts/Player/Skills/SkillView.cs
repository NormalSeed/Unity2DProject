using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillView : MonoBehaviour, IView
{
    [SerializeField] private Image QDisable;
    [SerializeField] private Image WDisable;
    [SerializeField] private Image EDisable;
    [SerializeField] private Image RDisable;
    private GameObject skillManager;
    private PlayerQ playerQ;
    private PlayerW playerW;
    private PlayerE playerE;
    private PlayerR playerR;

    private void Awake() => Init();

    private void Init()
    {
        skillManager = GameObject.Find("SkillManager");
        playerQ = skillManager.GetComponent<PlayerQ>();
        playerW = skillManager.GetComponent<PlayerW>();
        playerE = skillManager.GetComponent<PlayerE>();
        playerR = skillManager.GetComponent<PlayerR>();
    }

    public void PlayAnimation(int hash)
    {
        // TODO: 스킬 애니메이션 필요
    }

    public void UpdateUI()
    {
        UpdateQSkillUI();
        UpdateWSkillUI();
        UpdateESkillUI();
        UpdateRSkillUI();
    }

    public void UpdateQSkillUI()
    {
        float ratio = playerQ.cooldownTimer / playerQ.cooldown;
        QDisable.fillAmount = ratio;
    }

    public void UpdateWSkillUI()
    {
        float ratio = playerW.cooldownTimer / playerW.cooldown;
        WDisable.fillAmount = ratio;
    }

    public void UpdateESkillUI()
    {
        float ratio = playerE.cooldownTimer / playerE.cooldown;
        EDisable.fillAmount = ratio;
    }

    public void UpdateRSkillUI()
    {
        float ratio = playerR.cooldownTimer / playerR.cooldown;
        RDisable.fillAmount = ratio;
    }
}

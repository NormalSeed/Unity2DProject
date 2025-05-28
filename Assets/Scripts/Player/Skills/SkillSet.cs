using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSet : MonoBehaviour
{
    public Dictionary<eSkill, PlayerSkill> skillDic;
    public PlayerSkill selectedSkill;

    public SkillSet()
    {
        skillDic = new Dictionary<eSkill, PlayerSkill>();
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        skillDic.Add(eSkill.Q, GetComponent<Q_Skill>());
        skillDic.Add(eSkill.W, GetComponent<W_Skill>());
        skillDic.Add(eSkill.E, GetComponent<E_Skill>());
        skillDic.Add(eSkill.R, GetComponent<R_Skill>());
    }

    public PlayerSkill SelectSkill(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Q:
                selectedSkill = skillDic[eSkill.Q];
                return selectedSkill;
            case KeyCode.W:
                selectedSkill = skillDic[eSkill.W];
                return selectedSkill;
            case KeyCode.E:
                selectedSkill = skillDic[eSkill.E];
                return selectedSkill;
            case KeyCode.R:
                selectedSkill = skillDic[eSkill.R];
                return selectedSkill;
        }

        return selectedSkill;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerR : Skill
{
    private GameObject player;
    private PlayerController controller;
    private Coroutine cantControllTime;

    protected override void Init()
    {
        player = GameObject.Find("Player");
        controller = player.GetComponent<PlayerController>();
    }
    public override void UseSkill()
    {
        cantControllTime = StartCoroutine(CoCantControllTime());
        // 1�ʵ��� �������� �� BoxCollider2D(Trigger) ���� ������ �������� �� ��
        // �����̻�(����)�� �ɾ �÷��̾�Լ� õõ�� �־����� �ϴ� ��ų.
        // TODO: �� �̵��� �����̻�(����) ���� �� �۾� �ʿ���
    }

    public void SkillFinished()
    {
        SkillManager.Instance.animator.SetBool("IsRSkill", false);
        SkillManager.Instance.spriteRenderer.enabled = false;
    }

    IEnumerator CoCantControllTime()
    {
        controller.isControllActivated = false;
        controller.movement.rb.drag = 1000;
        controller.coDamagable = StartCoroutine(controller.CoDamagable());
        yield return new WaitForSeconds(0.5f);
        SkillManager.Instance.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 0.9f);
        SkillManager.Instance.spriteRenderer.enabled = true;
        SkillManager.Instance.animator.SetBool("IsRSkill", true);
        SkillManager.Instance.rSkillTerritory.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        controller.movement.rb.drag = 1;
        controller.isControllActivated = true;
        SkillManager.Instance.rSkillTerritory.SetActive(false);
    }
}

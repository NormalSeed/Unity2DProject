using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerR : Skill
{
    private Coroutine cantControllTime;

    protected override void Init()
    {
        base.Init();
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
        skillManager.animator.SetBool("IsRSkill", false);
        skillManager.spriteRenderer.enabled = false;
    }

    IEnumerator CoCantControllTime()
    {
        controller.isControllActivated = false;
        controller.movement.rb.drag = 1000;
        controller.coDamagable = StartCoroutine(controller.CoDamagable());
        yield return new WaitForSeconds(0.5f);
        skillManager.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 0.9f);
        skillManager.spriteRenderer.enabled = true;
        skillManager.animator.SetBool("IsRSkill", true);
        skillManager.rSkillTerritory.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        controller.movement.rb.drag = 1;
        controller.isControllActivated = true;
        skillManager.rSkillTerritory.SetActive(false);
    }
}

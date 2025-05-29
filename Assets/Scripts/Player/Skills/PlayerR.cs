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
        // 1�ʵ��� �������� �� OverlapBox ���� ������ �������� �� ��
        // �����̻�(����)�� �ɾ �÷��̾�Լ� õõ�� �־����� �ϴ� ��ų.
        // TODO: �� �̵��� �����̻�(����) ���� �� �۾� �ʿ���.
    }

    IEnumerator CoCantControllTime()
    {
        controller.isControllActivated = false;
        yield return new WaitForSeconds(1f);
        controller.isControllActivated = true;
    }
}

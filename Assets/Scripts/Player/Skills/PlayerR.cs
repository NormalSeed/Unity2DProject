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
        // 1초동안 정신집중 후 OverlapBox 안의 적에게 데미지를 준 후
        // 상태이상(공포)를 걸어서 플레이어에게서 천천히 멀어지게 하는 스킬.
        // TODO: 적 이동과 상태이상(공포) 구현 후 작업 필요함.
    }

    IEnumerator CoCantControllTime()
    {
        controller.isControllActivated = false;
        yield return new WaitForSeconds(1f);
        controller.isControllActivated = true;
    }
}

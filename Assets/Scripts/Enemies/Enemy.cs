using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract void TakeDamage(int damage);

    public abstract void DetectPlayer();

    public abstract void AttackSkill();
}

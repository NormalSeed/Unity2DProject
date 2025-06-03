using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour, IView
{
    public Animator animator;

    private void Awake() => Init();

    private void Init()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation(int hash)
    {
        animator.Play(hash);
    }
}

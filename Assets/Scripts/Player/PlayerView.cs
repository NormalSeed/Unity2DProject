using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PlayerView : MonoBehaviour, IView
{
    [SerializeField] private RawImage[] hp;

    private Animator animator;

    private void Awake() => Init();
    private void Init()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayRunAnimation(bool value) => animator.SetBool("IsRun", value);
    public void PlayJumpAnimation(bool value) => animator.SetBool("IsJump", value);
    public void UpdateHpUI(int value)
    {
        for (int i = 0; i < hp.Length; i++)
        {
            hp[i].enabled = i < value;
        }
    }
}

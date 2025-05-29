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

    public void PlayAnimation(int hash)
    {
        animator.Play(hash);
    }
    public void UpdateUI(int value)
    {
        UpdateHPUI(value);
    }

    public void UpdateHPUI(int value)
    {
        for (int i = 0; i < hp.Length; i++)
        {
            hp[i].enabled = i < value;
        }
    }
}

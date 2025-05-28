using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IView
{
    public void UpdateHpUI(int value);
    public void PlayAnimation(int hash);
    public virtual void PlayJumpAnimation(){ }

}

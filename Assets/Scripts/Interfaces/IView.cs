using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IView
{
    public virtual void UpdateUI(int value){ }
    public void PlayAnimation(int hash);
}

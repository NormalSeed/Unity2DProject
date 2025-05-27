using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IView
{
    public void UpdateHp(int hp);
    public void PlayRunAnimation();
    public virtual void PlayJumpAnimation(){ }

}

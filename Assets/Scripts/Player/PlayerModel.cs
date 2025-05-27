using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [field : SerializeField] public int Hp { get; set; } = 5;
    [field : SerializeField] public float MoveSpd { get; set; } = 5f;
    [field : SerializeField] public float JumpPower { get; set; } = 5f;

    public ObservableProperty<bool> IsJumping { get; private set; } = new();
    public ObservableProperty<bool> IsRunning { get; private set; } = new();
}

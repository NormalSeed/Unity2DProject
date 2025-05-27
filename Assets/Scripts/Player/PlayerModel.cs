using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] public int Hp { get; set; } = 5;
    [SerializeField] public float MoveSpd { get; set; } = 5f;
    [SerializeField] public float JumpPower { get; set; } = 5f;

    public ObservableProperty<bool> IsJumping { get; private set; } = new();
}

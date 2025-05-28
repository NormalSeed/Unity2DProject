using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [field : SerializeField] public float MoveSpd { get; set; } = 5f;
    [field : SerializeField] public float JumpPower { get; set; } = 5f;

    [field: SerializeField] public int MaxHp { get; set; } = 5;
    [field: SerializeField] public ObservableProperty<int> CurHp { get; private set; } = new();
}

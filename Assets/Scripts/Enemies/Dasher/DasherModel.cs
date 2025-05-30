using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherModel : MonoBehaviour
{
    [field: SerializeField] public float MoveSpd { get; set; } = 2f;
    [field: SerializeField] public int MaxHp { get; set; } = 2;
    [field: SerializeField] public ObservableProperty<int> CurHp { get; private set; } = new();
}

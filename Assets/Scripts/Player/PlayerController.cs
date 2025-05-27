using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private IView view;
    private PlayerMovement movement;
    private PlayerModel model;

    private void Awake() => Init();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.PlayerJump(model.JumpPower);
        }
    }
    private void FixedUpdate() => movement.HorizontalMove(model.MoveSpd);

    private void Init()
    {
        model = GetComponent<PlayerModel>();
        movement = GetComponent<PlayerMovement>();
    }

    
}

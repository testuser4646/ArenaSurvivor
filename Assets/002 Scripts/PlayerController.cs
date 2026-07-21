using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Character
{
    Camera cam;

    protected override void Awake()
    {
        base.Awake();

        cam = Camera.main;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (isDead)
            return;

        InputMove();
        InputLook();
        InputAction();

        RotateHand();
    }

    void InputMove()
    {
        dir = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            dir += Vector2.up;

        if (Keyboard.current.sKey.isPressed)
            dir += Vector2.down;

        if (Keyboard.current.aKey.isPressed)
            dir += Vector2.left;

        if (Keyboard.current.dKey.isPressed)
            dir += Vector2.right;

        dir = dir.normalized;
    }

    void InputLook()
    {
        Vector3 mouse =
            cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        mouse.z = 0;

        lookDir = (mouse - transform.position).normalized;
    }

    void InputAction()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
            PickupWeapon();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (currentWeapon != null)
                currentWeapon.Attack();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (currentWeapon != null)
            {
                currentWeapon.Throw(lookDir, col);
                currentWeapon = null;
            }
        }
    }

    protected override void Init()
    {
        maxHP = 10;
        nowHP = maxHP;

        moveSpeed = 10f;
        handRotateSpeed = 400f;

        isDead = false;

        rb.linearVelocity = Vector2.zero;
    }

    protected override void Die()
    {
        isDead = true;
        dir = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
    }
}
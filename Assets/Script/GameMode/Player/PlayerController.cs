using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] private float rotateSpeed;
    private PlayerKeybinds inputActions;
    private CharacterController controller;
    private Animator anim;
    private Vector2 movementInput;
    private Vector3 currentMovement;
    private Quaternion RotateDir;
    private bool isWalk;
    private bool isRun;
    private bool isCrouched;
    private PhotonView pv;

    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
        anim = gameObject.GetComponent<Animator>();
        inputActions = new PlayerKeybinds();

        inputActions.PlayerController.Movement.started += OnMovementAction;
        inputActions.PlayerController.Movement.performed += OnMovementAction;
        inputActions.PlayerController.Movement.canceled += OnMovementAction;

        inputActions.PlayerController.Run.started += OnRun;
        inputActions.PlayerController.Run.canceled += OnRun;

        pv = gameObject.GetComponentInParent<PhotonView>();
    }
    private void OnEnable()
    {
        inputActions.PlayerController.Enable();
    }
    private void OnDisable()
    {
        inputActions.PlayerController.Disable();
    }
    private void OnMovementAction(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();

        currentMovement.x = movementInput.x;
        currentMovement.z = movementInput.y;

        isWalk = movementInput.x != 0 || movementInput.y != 0;
    }
    private void OnRun(InputAction.CallbackContext context)
    {
        isRun = context.ReadValueAsButton();
    }
    private void PlayerRotate()
    {
        if (!isWalk) return;
        RotateDir = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentMovement), Time.deltaTime * rotateSpeed);

        transform.rotation = RotateDir;
    }
    private void AnimationController()
    {
        anim.SetBool("IsWalk", isWalk);
        anim.SetBool("IsRun", isRun);
    }
    private void Update()
    {
        if(!pv.IsMine) return;

        AnimationController();
        PlayerRotate();
    }
    private void FixedUpdate()
    {
        if (!pv.IsMine) return;

        controller.Move(currentMovement * Time.fixedDeltaTime);
    }
}







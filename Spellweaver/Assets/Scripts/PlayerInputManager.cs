using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    PlayerInputActions playerControls;
    public PlayerManager player;

    [Header("Aim Controls")]
    [SerializeField] public Vector2 aim_Input;
    public float sensitivity;
    public float aimVecticalInput;
    public float aimHorizontalInput;
    public float mouseSensitivity = 2.0f;
    public float controllerSensitivity = 0.1f;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerInputActions();
        }
        
        
        playerControls.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    private void Update()
    {
        HandleAiming();
        
    }
    private void HandleAiming()
    {
        aim_Input = playerControls.AimingControls.Aim.ReadValue<Vector2>();

        aim_Input *= sensitivity;

        aimVecticalInput = aim_Input.y;
        aimHorizontalInput = aim_Input.x;




    }
}

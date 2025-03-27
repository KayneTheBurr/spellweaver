using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    [Header("Ability Inputs")]
    [SerializeField] bool basicAttack_Input = false;
    [SerializeField] bool ability1_Input = false;
    [SerializeField] bool ability2_Input = false;
    [SerializeField] bool ability3_Input = false;
    [SerializeField] bool ability4_Input = false;


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
            return;
        }
        playerControls = new PlayerInputActions();
    }
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerInputActions();
        }

        playerControls.PlayerActions.BasicAttack.performed += _ => basicAttack_Input = true;
        playerControls.PlayerActions.BasicAttack.canceled += _ => basicAttack_Input = false;

        playerControls.PlayerActions.Ability1.performed += _ => ability1_Input = true;
        playerControls.PlayerActions.Ability1.canceled += _ => ability1_Input = false;

        playerControls.PlayerActions.Ability2.performed += _ => ability2_Input = true;
        playerControls.PlayerActions.Ability2.canceled += _ => ability2_Input = false;

        playerControls.PlayerActions.Ability3.performed += _ => ability3_Input = true;
        playerControls.PlayerActions.Ability3.canceled += _ => ability3_Input = false;

        playerControls.PlayerActions.Ability4.performed += _ => ability4_Input = true;
        playerControls.PlayerActions.Ability4.canceled += _ => ability4_Input = false;



        playerControls.Enable();

        SceneManager.activeSceneChanged += OnSceneChanged;

        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Disable();
        }

        SceneManager.activeSceneChanged -= OnSceneChanged;
    }
    private void OnSceneChanged(Scene previousScene, Scene newScene)
    {
        UpdateCursorState(newScene.buildIndex);
    }
    private void UpdateCursorState(int sceneIndex)
    {
        if (sceneIndex == 2) // DPS Testing Scene
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2
            && DamageTimeManager.instance.canAim)
        {
            HandleAiming();
            HandleBasicAttackInput();
            HandleAbilityInput();
        }
    }
    private void HandleAiming()
    {
        aim_Input = playerControls.AimingControls.Aim.ReadValue<Vector2>();

        aim_Input *= sensitivity;

        aimVecticalInput = aim_Input.y;
        aimHorizontalInput = aim_Input.x;




    }
    public void HandleBasicAttackInput()
    {
        if(basicAttack_Input)
        {
            basicAttack_Input = false;
            player.playerCombatManager.AttemptBasicAttack();
        }
    }
    public void HandleAbilityInput()
    {
        if (ability1_Input)
        {
            Debug.Log("used ability 1");
            ability1_Input = false;
            player.playerCombatManager.AttemptAbility(1);
        }
        if (ability2_Input)
        {
            Debug.Log("used ability 2");
            ability2_Input = false;
            player.playerCombatManager.AttemptAbility(2);
        }
        if (ability3_Input)
        {
            Debug.Log("used ability 3");
            ability3_Input = false;
            player.playerCombatManager.AttemptAbility(3);
        }
        if (ability4_Input)
        {
            Debug.Log("used ability 4");
            ability4_Input = false;
            player.playerCombatManager.AttemptAbility(4);
        }
    }
}

using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public Camera cam;
    public PlayerManager player;

    [Header("Camera Settings")]
    [SerializeField] float sensitivity = 1.0f;
    [SerializeField] float maxXRotation = 45f;
    [SerializeField] float minXRotation = -45f;
    [SerializeField] float maxYRotation = 45f;
    [SerializeField] float minYRotation = -45f;

    private float xRot = 0f;
    private float yRot = 0f;

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
    public void HandleAllCameraAction()
    {
        Vector2 aimInput = PlayerInputManager.instance.aim_Input;

        yRot += aimInput.x * sensitivity;
        xRot -= aimInput.y * sensitivity;


        xRot = Mathf.Clamp(xRot, minXRotation, maxXRotation);
        yRot = Mathf.Clamp(yRot, minYRotation, maxYRotation);

        transform.localRotation = Quaternion.Euler(xRot, yRot, 0f);
    }
}

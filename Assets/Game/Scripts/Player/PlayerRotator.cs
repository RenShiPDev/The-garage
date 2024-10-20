using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    public static PlayerRotator Instance = null;

    [SerializeField] private GameObject _rotatingCamera;
    [SerializeField] private GameObject _rotatingObject;

    [Range(0.1f, 9f)][SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;

    public bool RotEnabled;

    private void Awake()
    {
        RotEnabled = true;

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = value; }
    }

    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X";
    const string yAxis = "Mouse Y";

    void Update()
    {
        if (RotEnabled)
        {
            rotation.x += Input.GetAxis(xAxis) * sensitivity;
            rotation.y += Input.GetAxis(yAxis) * sensitivity;

            if (PlayerMover.Instance.IsEnabled)
            {
                rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
            }
            var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
            var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

            _rotatingCamera.transform.localRotation = yQuat;
            _rotatingObject.transform.localRotation = xQuat;

            if (yQuat.eulerAngles.y != 0)
            {
                rotation.x -= Input.GetAxis(xAxis) * sensitivity;
                rotation.x -= Input.GetAxis(xAxis) * sensitivity;
            }
        }
    }

    private void LateUpdate()
    {
        if (!RotEnabled && PlayerMover.Instance.gameObject.GetComponent<Collider>().enabled)
        {
            var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
            var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);
            _rotatingCamera.transform.localRotation = yQuat;
            _rotatingObject.transform.localRotation = xQuat;
        }
    }

    public void SetRotation(Vector2 rot)
    {
        rotation = rot;
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);
        _rotatingCamera.transform.localRotation = yQuat;
        _rotatingObject.transform.localRotation = xQuat;
    }

    public void DisableRot(Quaternion quatP)
    {
        RotEnabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerRaycastActivator : MonoBehaviour
{
    public static PlayerRaycastActivator Instance = null;

    [SerializeField] public Transform GrabPos;

    private TakingObject _takingObject;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Ray rayd = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hitd;
        if (Physics.Raycast(rayd, out hitd)) 
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitd.distance, Color.red);

        /*
        RaycastHit hitd1;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitd1, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitd.distance, Color.green);
        }*/

        bool pressed = false;

        float valAxis = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetKeyDown(KeyCode.E))
        {
            pressed = true;

            RaycastHit hit;
            int layerMask = 1 << 8;
            layerMask = ~layerMask;

            if (_takingObject != null)
            {
                _takingObject.DropMe();
                _takingObject = null;
            }
            else
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.distance < 4f)
                    {
                        if (_takingObject == null)
                        {
                            if (hit.collider.gameObject.GetComponent<TakingObject>() != null)
                            {
                                _takingObject = hit.collider.gameObject.GetComponent<TakingObject>();
                                _takingObject.TakeMe();
                                if (valAxis == 1)
                                {
                                    _takingObject.GetComponent<Rigidbody>().AddForce((_takingObject.transform.position - transform.position).normalized * 15f);
                                }
                            }
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButton(1))
        {
            valAxis = -1;
        }
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.E))
        {
            RaycastHit hit;
            int layerMask = 1 << 8;
            layerMask = ~layerMask;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                if (hit.distance < 4f)
                {
                    if (hit.collider.gameObject.GetComponent<InGameButton>() != null)
                    {
                        var comp = hit.collider.gameObject.GetComponent<InGameButton>();
                        comp.PressMouse();
                    }
                }
            }
        }

        if (pressed || valAxis != 0)
        {
            RaycastHit hit;
            int layerMask = 1 << 8;
            layerMask = ~layerMask;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                if(hit.distance < 4f)
                {
                    if (hit.collider.gameObject.GetComponent<InGameButton>() != null)
                    {
                        var comp = hit.collider.gameObject.GetComponent<InGameButton>();

                        if (pressed)
                        {
                            comp.PressE();
                        }

                        if(valAxis != 0)
                        {
                            comp.Roll(valAxis);
                        }
                    }
                }
            }
        }
    }

    public void Drop()
    {
        if (_takingObject != null)
        {
            _takingObject.DropMe();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour
{
    public UnityEvent Step;

    public static PlayerMover Instance = null;

    [SerializeField] private List<Collider> _curCollisions = new List<Collider>();

    [SerializeField] private float _speed;
    [SerializeField] private float _stepTime;

    [SerializeField] private GameObject _camera;

    private Rigidbody _rb;
    private CapsuleCollider _col;
    private Vector3 _Ffwd;
    private Vector3 _cameraPos;

    private float _timer;
    private float _startDrag;
    private float _colSizeY;

    private bool stepped;

    public bool OnFloor;
    public bool InCollision;
    public bool IsEnabled;

    private void Awake()
    {
        IsEnabled = true;

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _colSizeY = _col.height;
        _startDrag = _rb.drag;
        _cameraPos = _camera.transform.localPosition;
    }

    public void ChangeKinematic()
    {
        _rb.isKinematic = false;
        _col.enabled = true;
    }

    Vector3 targetF;
    Vector3 targetR;

    private void LateUpdate()
    {
        if (IsEnabled)
        {
            targetF = _camera.transform.forward;
            targetF.y = 0;

            targetR = _camera.transform.right;
            targetR.y = 0;

            targetF = targetF.normalized;
            targetR = targetR.normalized;
        }
    }

    private void FixedUpdate()
    {
        if (IsEnabled)
        {
            var localSpeed = _speed;

            if(_rb.velocity.y < 0)
            {
                _rb.velocity -= Vector3.up/4f;
            }

            if (Input.GetKey(KeyCode.Space) && OnFloor)
            {
                OnFloor = false;
                targetF = new Vector3(0, -1, 0);
                _rb.AddForce(-targetF * localSpeed/8f, ForceMode.Impulse);
                stepped = false;
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                _col.height = _colSizeY/2f;
                _camera.transform.position = _col.transform.position + new Vector3(0, _colSizeY/2.2f, 0);
                localSpeed /= 2;
            }
            else
            {
                if (Input.GetKey(KeyCode.C))
                {
                    _col.height = _colSizeY / 4f;
                    _camera.transform.position = _col.transform.position + new Vector3(0, _colSizeY / 4.4f, 0);
                    localSpeed /= 4;
                }
                else
                {
                    if (_col.height != _colSizeY)
                    {
                        _col.height = _colSizeY;
                        _camera.transform.localPosition = _cameraPos;
                    }
                }
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                    localSpeed *= 1.5f;
            }

            if (Input.GetKey(KeyCode.W))
            {
                _rb.AddForce(targetF * localSpeed);
                stepped = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                _rb.AddForce(-targetF * localSpeed);
                stepped = true;
            }

            if (Input.GetKey(KeyCode.A))
            {
                _rb.AddForce(-targetR * localSpeed);
                stepped = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                _rb.AddForce(targetR * localSpeed);
                stepped = true;
            }
        }

        if (stepped)
        {
            _timer += Time.deltaTime;

            Vector3 targPos = _cameraPos;
            float speed = 2f;
            if (_timer <= _stepTime / 2f)
            {
                targPos.y += 0.1f;
            }

            if ((_camera.transform.localPosition - targPos).magnitude >= 0.01f)
            {
                _camera.transform.localPosition = Vector3.Slerp(_camera.transform.localPosition, targPos, Time.deltaTime * speed);
            }

            if (_timer >= _stepTime)
            {
                Step?.Invoke();
                _timer = 0;
                stepped = false;
            }
        }
        stepped = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach(var col in _curCollisions)
        {
            if(col.gameObject == collision.gameObject)
            {
                return;
            }
        }
        _curCollisions.Add(collision.collider);

        foreach (var col in _curCollisions)
        {
            var colPos = col.bounds.center.y + col.bounds.size.y/2;
            if(colPos < transform.position.y)
            {
                OnFloor = true; 
                break;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        foreach (var col in _curCollisions)
        {
            if (col.gameObject == collision.gameObject)
            {
                _curCollisions.Remove(collision.collider);
                break;
            }
        }
    }
}

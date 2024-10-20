using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectHider : MonoBehaviour
{
    public UnityEvent OnPos;
    public UnityEvent Opening;
    public UnityEvent Closening;

    public UnityEvent OnClosed;
    public UnityEvent OnOpened;

    [SerializeField] private GameObject _object;

    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;

    [SerializeField] private float _speed;

    [SerializeField] private bool _isHide;
    [SerializeField] public bool InPos;

    private void Start()
    {
        if (_isHide)
        {
            Opening?.Invoke();
        }
        else
        {
            Closening?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if (!InPos && _object != null)
        {
            if (_isHide)
            {
                if ((_object.transform.position - _endPos.position).magnitude > 0.01f)
                {
                    _object.transform.position = Vector3.Lerp(_object.transform.position, _endPos.position, _speed * Time.deltaTime);
                    _object.transform.rotation = Quaternion.Lerp(_object.transform.rotation, _endPos.rotation, _speed * Time.deltaTime);
                }
                else
                {
                    OnPos?.Invoke();
                    _object.transform.position = _endPos.position;
                    _object.transform.rotation = _endPos.rotation;
                    InPos = true;

                    OnClosed?.Invoke();
                }
            }
            else
            {
                if ((_object.transform.position - _startPos.position).magnitude > 0.01f)
                {
                    _object.transform.position = Vector3.Lerp(_object.transform.position, _startPos.position, _speed * Time.deltaTime);
                    _object.transform.rotation = Quaternion.Lerp(_object.transform.rotation, _startPos.rotation, _speed * Time.deltaTime);
                }
                else
                {
                    OnPos?.Invoke();
                    _object.transform.position = _startPos.position;
                    _object.transform.rotation = _startPos.rotation;
                    InPos = true;

                    OnOpened?.Invoke();
                }
            }
        }
    }

    public void Show()
    {
        InPos = false;
        _isHide = false;
        Closening?.Invoke();
    }
    public void Hide()
    {
        InPos = false;
        _isHide = true;
        Opening?.Invoke();
    }

    [ContextMenu("ChangeVisibility")]
    public void ChangeVisibility()
    {
        InPos = false;
        _isHide = !_isHide;
        if (_isHide)
        {
            Opening?.Invoke();
        }
        else
        {
            Closening?.Invoke();
        }
    }

    public void HideObj(GameObject obj)
    {
        _object = obj;
        if(obj.TryGetComponent<Rigidbody>(out Rigidbody rbobj))
        {
            rbobj.isKinematic = true;
        }
        if (obj.TryGetComponent<Collider>(out Collider colobj))
        {
            colobj.enabled = false;
        }
        Hide();
    }

    public void ResetHider()
    {
        if( _isHide )
        {
            InPos = true;
            _isHide = false;
            if (_object.TryGetComponent<Rigidbody>(out Rigidbody rbobj))
            {
                rbobj.isKinematic = false;
            }
            if (_object.TryGetComponent<Collider>(out Collider colobj))
            {
                colobj.enabled = true;
            }
            _object = null;
        }
    }

    public bool CheckHide()
    {
        return _isHide;
    }
}

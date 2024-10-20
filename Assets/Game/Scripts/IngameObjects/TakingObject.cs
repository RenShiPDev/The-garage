using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class TakingObject : MonoBehaviour
{
    public UnityEvent OnTake;
    public UnityEvent OnDrop;
    public UnityEvent OnPosChanged;
    public UnityEvent OnRbCollide;

    public int Price;

    private Rigidbody _rb;

    [SerializeField] private string _name;
    [SerializeField] private int _id;

    private float _distPosition;
    private float _timer;

    private bool _timerEnabled;

    public bool IsTaked;
    public bool IsSaving;
    public bool Killed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        OnPosChanged.Invoke();
        _timerEnabled = true;
    }

    public string GetName()
    {
        return _name;
    }
    public int GetId()
    {
        return _id;
    }
    public void SetId(int id)
    {
        _id += id;
    }

    public void SetName(string name)
    {
        _name = name;
    }

    public void TakeMe()
    {
        IsTaked = true;
        _timerEnabled = true;
        
        //_distPosition = (transform.position - PlayerRaycastActivator.Instance.GrabPos.position).magnitude;
        _distPosition = 0;

        OnTake?.Invoke();
    }
    public void DropMe()
    {
        IsTaked = false;
        OnDrop?.Invoke();
    }

    public void CheckGravity()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void LateUpdate()
    {
        if (IsTaked)
        {
            float scrl = Input.GetAxis("Mouse ScrollWheel");
            if (scrl != 0)
            {
                _distPosition += scrl*(_distPosition);
            }

            var targetPos = PlayerRaycastActivator.Instance.GrabPos.position;

            var distance = (PlayerRaycastActivator.Instance.GrabPos.position - transform.position).magnitude;

            if(distance >= 4.1f)
            {
                DropMe();
            }

            if(!_rb.isKinematic)
            {
                _rb.velocity = (targetPos - transform.position) * 10 / _rb.mass;
                _rb.angularVelocity = Vector3.zero;
            }

            OnPosChanged.Invoke();
        }        

        if (_timerEnabled)
        {
            _timer += Time.deltaTime;
            OnPosChanged.Invoke();
            if (_timer > 2f)
            {
                _timer = 0;
                _timerEnabled = false;
            }
        }

        if(transform.position.y < -10f)
        {
            transform.position = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(_rb != null)
        {
            if (_rb.velocity.magnitude > 0.9f)
            {
                OnRbCollide?.Invoke();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLight : MonoBehaviour
{
    public UnityEvent OnLightOn;
    public UnityEvent OnLightOff;

    [SerializeField] private Light _light;
    [SerializeField] private List<GameObject> _enemies = new List<GameObject>();

    [SerializeField] private float _checkTimeout;

    private float _timer;

    private bool _isOn;
    private bool _isEnemy;

    private void Start()
    {
        _light.enabled = _isOn;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(_isOn)
            {
                OnLightOff?.Invoke();
                _light.enabled = false;
            }
            else 
            { 
                OnLightOn?.Invoke();
                _light.enabled = true;
            }


            _isOn = !_isOn;
        }

        _timer += Time.deltaTime;

        if(_timer > _checkTimeout) 
        {
            _isEnemy = false;
            foreach (GameObject go in _enemies)
            {
                if ( (transform.position - go.transform.position).magnitude < 6f )
                {
                    _isEnemy = true;
                    _isOn = Random.Range(0, 2) == 1;
                    if (_isOn)
                    {
                        OnLightOff?.Invoke();
                        _light.enabled = false;
                    }
                    else
                    {
                        OnLightOn?.Invoke();
                        _light.enabled = true;
                    }
                    break;
                }
            }
            _timer = 0;
        }

        if(_timer > _checkTimeout / 3f && _isEnemy)
        {
            if (!_isOn) 
            {
                _isOn = Random.Range(0, 2) == 1;
                if(_isOn)
                {
                    OnLightOn?.Invoke();
                    _light.enabled = true;
                }
            } 
        }
    }
}

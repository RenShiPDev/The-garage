using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InGameButton : MonoBehaviour
{
    private int _step = 100;

    [SerializeField] private TMP_Text _stepText;

    [DoNotSerialize] public UnityEvent<int> OnRollUp;
    [DoNotSerialize] public UnityEvent<int> OnRollDown;
    public UnityEvent OnPressE;
    public UnityEvent<GameObject> OnPressEObj;

    public int Id;

    public bool IsDisabled;
    public bool Mouse;

    private void Start()
    {
        if(_stepText != null)
        {
            _stepText.text = "Step: " + _step;
        }
    }

    public void Roll(float value)
    {
        if(value > 0)
        {
            OnRollUp?.Invoke(_step);
        }
        else
        {
            OnRollDown?.Invoke(_step);
        }
    }
    public void PressE()
    {
        if (!IsDisabled)
        {
            OnPressE?.Invoke();
            OnPressEObj?.Invoke(gameObject);
        }
    }
    public void PressMouse()
    {
        if (Mouse)
        {
            PressE();
        }
    }

    public void UpChangeStep()
    {
        _step += 100;
        _stepText.text = "Step: " + _step;
    }
    public void DownChangeStep()
    {
        if(_step > 100)
        {
            _step -= 100;
            _stepText.text = "Step: " + _step;
        }
    }

    public void Enable()
    {
        IsDisabled = false;
    }
    public void Disable()
    {
        IsDisabled = true;
    }
}

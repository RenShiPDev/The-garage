using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ObjectTaker : MonoBehaviour
{
    [SerializeField] private string _targetName;

    [SerializeField] private ObjectHider _hider;

    public TakingObject Taked;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<TakingObject>(out TakingObject takingobj))
        {
            if (takingobj.GetName() == _targetName)
            {
                Taked = takingobj;

                Taked.DropMe();
                _hider.HideObj(Taked.gameObject);

                if (Taked.TryGetComponent<Rigidbody>(out Rigidbody rbobj))
                {
                    rbobj.isKinematic = true;
                }
                if (Taked.TryGetComponent<Collider>(out Collider colobj))
                {
                    colobj.enabled = false;
                }
            }
        }
    }

    public void Drop()
    {
        if(Taked != null)
        {
            if (Taked.TryGetComponent<Rigidbody>(out Rigidbody rbobj))
            {
                rbobj.isKinematic = false;
            }
            if (Taked.TryGetComponent<Collider>(out Collider colobj))
            {
                colobj.enabled = true;
            }
        }
    }
}

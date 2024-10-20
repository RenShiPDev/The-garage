using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLegsTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        //PlayerMover.Instance.OnFloor = false;
        var colPos = other.bounds.center.y + other.bounds.size.y / 2;
        if (colPos < transform.position.y)
        {
            PlayerMover.Instance.OnFloor = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //PlayerMover.Instance.OnFloor = false;
        var colPos = other.bounds.center.y + other.bounds.size.y / 2;
        if (colPos < transform.position.y)
        {
            PlayerMover.Instance.OnFloor = false;
        }
    }
}

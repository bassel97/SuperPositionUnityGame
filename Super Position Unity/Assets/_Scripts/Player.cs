using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{



    private void OnTriggerEnter(Collider other)
    {
        string otherTag = other.tag;

        if (otherTag.Equals("Goal"))
        {
            Debug.Log("Win");
        }
        else if (otherTag.Equals("Trap"))
        {
            Debug.Log("Lose");
        }
    }

}

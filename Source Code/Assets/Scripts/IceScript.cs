using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceScript : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "iceEnemy")
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
        else if((col.gameObject.tag == "floor") || (col.gameObject.tag == "wall"))
        {
            Destroy(gameObject);
        }
        
    }
}

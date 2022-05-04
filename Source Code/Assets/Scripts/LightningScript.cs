using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningScript : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "lightningEnemy")
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

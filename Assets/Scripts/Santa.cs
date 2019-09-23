using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santa : MonoBehaviour
{

    public Transform target;
    public float speed;
    public Transform startpos;
    public bool movingtotarget = true; 

    private void Update() //Checks if the santa should move towards the player or from the player
    {
        if (movingtotarget)
        {
            MoveToTarget();
        }
        else
        {
            MoveFromTarget();
        }
    }

    void MoveToTarget()//Moves santa towards player
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);        
    }

    void MoveFromTarget()//Moves santa from player
    {
        transform.position = Vector3.MoveTowards(transform.position, startpos.position, speed * Time.deltaTime); 
    }

}

    
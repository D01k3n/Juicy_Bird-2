using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralaxeffect : MonoBehaviour
{
    public GameObject[] levels;
    Camera cam;
    Vector2 screenbounds;

    public float backlayerspeed;
    public float midlayerspeed;
    public float frontlayerspeed; 
    public float offset;

    private void Start() //Defines variables/components, initializes childobjects function 
    {
        cam = GetComponent<Camera>();
        screenbounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
        foreach (GameObject obj in levels)
        {
            ChildObjects(obj);
        }

    }

    void ChildObjects(GameObject obj) //Measures screensize and calculates how big the background needs to be to cover the whole screen 
    {
        float objectwidth = obj.GetComponent<SpriteRenderer>().bounds.size.x;
        int childsneeded = (int)Mathf.Ceil(screenbounds.x * 2 / objectwidth) - 1;
        GameObject clone = Instantiate(obj) as GameObject;
        for (int i = 0; i <= -childsneeded; i++)
        {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector3(objectwidth * i, c.transform.position.y, c.transform.position.z);
            c.name = obj.name + i;
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }

    void repositionChildObjects(GameObject obj) //Changes position of background to the back (Looping the background)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if (children.Length > 1)
        {
            GameObject firstchild = children[1].gameObject;
            GameObject lastchild = children[children.Length - 1].gameObject;

            float halfobjectswidth = firstchild.GetComponent<SpriteRenderer>().bounds.extents.x;
            if (transform.position.x - screenbounds.x - offset > lastchild.transform.position.x + halfobjectswidth)
            {
                firstchild.transform.SetAsLastSibling();
                firstchild.transform.position = new Vector3(lastchild.transform.position.x + halfobjectswidth * 2, lastchild.transform.position.y, lastchild.transform.position.z);
            }
        }

        foreach (Transform child in children)
        {
            if (!child.gameObject.GetComponent<Rigidbody>())
            {
                child.gameObject.AddComponent<Rigidbody>();                
            }            
            Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            if(child.gameObject.tag == "Back")
            {
                rb.velocity -= Vector3.right * backlayerspeed * Time.deltaTime;
            }
            else if(child.gameObject.tag == "Mid")
            {
                rb.velocity -= Vector3.right * midlayerspeed * Time.deltaTime;
            }
            else
            {
                rb.velocity -= Vector3.right * frontlayerspeed * Time.deltaTime;
            }
        }
    }

    private void LateUpdate()
    {
        foreach (GameObject obj in levels)
        {
            repositionChildObjects(obj);
        }
    }

}

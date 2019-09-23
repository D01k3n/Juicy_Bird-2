using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Animations; 
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    bool started = false;
    float spawntimer = 2;
    public bool lost = false;

    public Animator uianimator;    
    GameObject gameovertext;
    GameObject pressbuttontext;

    Animator playeranimator; 

    public GameObject[] pipe_prefabs;
    public GameObject[] penguin_bodyparts;
    List<GameObject> bodypartsinscene = new List<GameObject>();

    GameManager gm;
    public AudioManager am; 
    private void Start() //Defining some variables/components
    {
        lost = false; 
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameovertext = GameObject.Find("GameOver_Text");        
        gameovertext.SetActive(false);
        pressbuttontext = GameObject.Find("PressButton_Text");
        playeranimator = GetComponentInChildren<Animator>();       
    }

    private void Update()
    {
        Movement();
        if (started && !lost) // Spawns pipes if not lost and has started
        {
            spawntimer -= Time.deltaTime; // when reaching 0, spawns pipe
            if(spawntimer <= 0)
            {
                int rand = Random.Range(0, 3); //randomizes which pipe to spawn
                GameObject npipe; 
                if(rand == 0)
                {
                    npipe = Instantiate(pipe_prefabs[0], new Vector3(7, 3.45f, 0), Quaternion.identity); 
                    Rigidbody rb2 = npipe.GetComponent<Rigidbody>();
                    rb2.velocity -= Vector3.right * 100 * Time.deltaTime;
                    gm.pipes.Add(npipe);
                }
                if(rand == 1)
                {
                    npipe = Instantiate(pipe_prefabs[1], new Vector3(7, .5f, 0), Quaternion.Euler(0, 0, 180));
                    Rigidbody rb2 = npipe.GetComponent<Rigidbody>();
                    rb2.velocity -= Vector3.right * 100 * Time.deltaTime;
                    gm.pipes.Add(npipe);
                }
                if (rand == 2)
                {
                    float r = Random.Range(1.25f, 3.1f); //Randomizes hight on obstacle unlike the other normal pipes
                    npipe = Instantiate(pipe_prefabs[2], new Vector3(7, r, 0), Quaternion.identity);
                    Rigidbody rb2 = npipe.GetComponent<Rigidbody>();
                    rb2.velocity -= Vector3.right * 100 * Time.deltaTime;
                    gm.pipes.Add(npipe);
                }
                spawntimer = 2; //resets spawntimer
            }
        }
    }

    private void Movement() //Starts game and controlls character 
    {
        if (Input.GetKeyDown(KeyCode.Space) && !lost && !started)
        {
            started = true;
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            uianimator.SetBool("PressButton", false);
            pressbuttontext.SetActive(false);
            gm.score = 0;
            gm.score_text.text = "Score: " + gm.score.ToString();
            playeranimator.SetTrigger("Flap");
            FindObjectOfType<Santa>().movingtotarget = false;            
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 300);
            playeranimator.SetTrigger("Flap");
        }
    }

    void GameOver() //Resets character, resumes music, pauses the game, runs ui animations, removes bodyparts from scene, resets score
    {
        playeranimator.SetBool("lost", false);
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);       
        started = false;
        lost = false;
        am.ResumeMusic();
        gameovertext.SetActive(false);
        pressbuttontext.SetActive(true);
        uianimator.SetBool("PressButton", true);        
        gm.score = 0;
        gm.score_text.text = "Score: " + gm.score.ToString();
        FindObjectOfType<Santa>().movingtotarget = true;
        foreach(GameObject part in bodypartsinscene)
        {
            Destroy(part);
        }
        bodypartsinscene.Clear();
    }

    void submit() //runs submit score function in GameManager
    {
        if (gm.score > 0 && lost)
        {
            gm.SetScore();
        }
        Invoke("GameOver", 1);
    }

    private void OnCollisionEnter(Collision collision) //Stops the game when touching pipe/roof/floor, triggers all animations(Death, UI etc.) 
    {
        if (collision.collider.tag == "DONTTOUCH" && !lost)
        {
            lost = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;            
            gameovertext.SetActive(true);
            uianimator.SetTrigger("GameOver");
            playeranimator.SetTrigger("GameOver");
            playeranimator.SetBool("lost", true);
            Invoke("Explode", .9f);
            am.PauseMusic();
            am.PlayAudioEffect(0);
            gm.DestroyPipes();
            Invoke("submit", 7);
        }
    }

    void Explode() //Spawns bodyparts on death(GORE!) 
    {
        foreach(GameObject bodypart in penguin_bodyparts)
        {
            GameObject part = Instantiate(bodypart, transform.position, Quaternion.identity);
            Rigidbody rb = part.AddComponent<Rigidbody>();
            rb.useGravity = false;
            float rx = Random.Range(-2000, 2000);
            float ry = Random.Range(-2000, 2000);
            float rr = Random.Range(-1000, 1000);
            rb.AddForce(new Vector2(rx, ry) * Time.deltaTime);
            rb.AddTorque(new Vector3(0, 0, rr));
            bodypartsinscene.Add(part);
        }
    }
}

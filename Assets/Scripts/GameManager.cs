using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public List<GameObject> pipes = new List<GameObject>();
    public List<GameObject> submissions = new List<GameObject>();
    GameObject player;

    public Text score_text;
    public int score;

    GameObject scoreinput;
    public Text nameinput;
    public GameObject submitprefab;
    GameObject submitcontent;
    GameObject scoreboard;

    Score _scorescript;

    public GameObject PauseMenu;
    public bool isPaused; 

    private void Start()//Defines variables/components
    {
        player = GameObject.Find("Player");
        score_text = GameObject.Find("Points_Text").GetComponent<Text>();
        nameinput = GameObject.Find("Name_Input_Input").GetComponent<Text>();
        scoreinput = GameObject.Find("Score_Input");
        scoreinput.SetActive(false);
        submitcontent = GameObject.Find("ScoreList_Content");
        scoreboard = GameObject.Find("Scoreboard");
        scoreboard.SetActive(false);
        _scorescript = GetComponent<Score>();
    }

    private void Update() //Opens scoreboard if pressing tab
    {
        if (!player.GetComponent<PlayerController>().lost) // checks that player hasn't died, if so: won't give more points
            for (int i = 0; i < pipes.Count; i++) // gives +1 score for each pipe passed, destroys pipe after passed
            {
                GameObject pipe = pipes[i];
                if (pipe.transform.position.x < player.transform.position.x && pipe != null)
                {
                    score += 1;
                    score_text.text = "Score: " + score.ToString();
                    pipes.Remove(pipe);
                    Destroy(pipe, 3);
                }
            }

        if (Input.GetKey(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }
        else
        {
            scoreboard.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) //Paus menyn öppnas när man klickar på Escape knappen på tangentbordet.
        {
            if (isPaused) //Om det redan är pausat och man klickar på Resume eller Escape startas spelet igen. 
            {
                Resume();
            }
            else //Om det inte är pausat pausas spelet när man går in på paus menyn på Escape knappen. 
            {
                isPaused = true;
                PauseMenu.SetActive(true);
                Time.timeScale = 0f; 
            }

        }
    }

    public void DestroyPipes() //Destroys pipes a few seconds after passing player
    {
        for (int i = 0; i < pipes.Count; i++)
        {
            GameObject pipe = pipes[i];
            if (pipe != null)
            {
                pipes.Remove(pipe);
                Destroy(pipe);
            }
        }
    }

    public void SetScore() //Opens submission menu in ordet to submit score if player wants to 
    {
        Time.timeScale = 0;
        scoreinput.SetActive(true);
    }

    public void Submit() //Takes information from submission and sends it to the upload function 
    {
        string name = nameinput.text;
        Time.timeScale = 1;
        scoreinput.SetActive(false);

        if (_scorescript.highscoreslist.Length < 7) //Checks that scoreboard isn't full
        {
            GameObject submit = Instantiate(submitprefab, Vector3.zero, Quaternion.identity);
            submit.transform.SetParent(submitcontent.transform);
            submit.GetComponent<Text>().text = name + " " + score.ToString();
            submissions.Add(submit);
            _scorescript.score(name, score);
        }

        else //If scoreboard is full, 
        {
            Refresh();
        }

    }

    public void Refresh()//resets scoreboard, downloads it again, reorganizes scoreboard after score (when has uploaded newest score to website)
    {
        if (!_scorescript.hasuploaded)
        {
            _scorescript.score(name, score);
        }

        else
        {
            for (int i = 0; i < submissions.Count; i++)
            {
                GameObject submission = submissions[i];
                Destroy(submission);
            }
            submissions.Clear();

            _scorescript.Download();
        }

    }

    public void UpdateScoreboard(string _name, int _score) //Updates the scoreboard ui with the latest/highest highscores 
    {
        if (submissions.Count < 7)
        {
            GameObject submit = Instantiate(submitprefab, Vector3.zero, Quaternion.identity);
            submit.transform.SetParent(submitcontent.transform);
            submit.GetComponent<Text>().text = _name + " " + _score.ToString();
            submissions.Add(submit);
        }
    }

    public void Skip() //Closes submission menu if player pressed skip 
    {
        scoreinput.SetActive(false);
        Time.timeScale = 1;
    }

    public void Resume() //Spelet startas igen när man klickar på Resume knappen i pause menyn.
    {
        isPaused = false;
        PauseMenu.SetActive(false); 
        Time.timeScale = 1f; 
    }

    public void Options() //Options menyn öppnas när man klickar på Options knappen i paus menyn. 
    {


    }

    public void Quit() //Spelet avslutas när man klickar på Quit knappen i paus menyn. 
    {
       //Application.Quit;
    }



}

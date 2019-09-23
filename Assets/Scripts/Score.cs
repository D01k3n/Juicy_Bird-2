using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Score : MonoBehaviour
{
    const string privatecode = "yWCgoW53d022GrWa9uM0oQAe24Tq2NaEmRMSP2McCmZQ";
    const string publiccode = "5d7f4313d1041303eca72949";
    const string webURL = "http://dreamlo.com/lb/";

    public HighScore[] highscoreslist;
    public bool hasuploaded = false; 

    GameManager gm; 

    private void Start()//Defines variables/components, Initializes download function
    {
        StartCoroutine(DownloadScore());
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();        
    }

    public void score(string username, int score)//Gets score and name of player from submission and initializes the upload function 
    {
        StartCoroutine(UploadScore(username, score));
    }

    public void Download() //Initializes download function (from other scripts) 
    {
        StartCoroutine(DownloadScore());
    }

    IEnumerator UploadScore(string username, int score) //Uploads score to the website 
    {
        WWW www = new WWW(webURL + privatecode + "/add/" + WWW.EscapeURL(username) + "/" + score);
        yield return www; 

        if (string.IsNullOrEmpty(www.error))
        {
            hasuploaded = true;
            gm.Refresh(); 
        }
        else
        {
            print("Error: Didn't Upload Shit!");
        }
    }

    IEnumerator DownloadScore() //Downloads score from website and sends the information to the formating function 
    {
        WWW www = new WWW(webURL + publiccode + "/pipe/");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            FormatHighScores(www.text);
            hasuploaded = false;
        }
        else
        {
            print("Error: Didn't download shit!");
        }
    }

    void FormatHighScores(string text) //Formats the information from the website and initializes updatescoreboard function
    {
        string[] entries = text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscoreslist = new HighScore[entries.Length];

        for(int i = 0; i < entries.Length; i++)
        {          
                string[] entry_info = entries[i].Split(new char[] { '|' });
                string name = entry_info[0];
                int score = int.Parse(entry_info[1]);
                highscoreslist[i] = new HighScore(name, score);

                gm.UpdateScoreboard(highscoreslist[i].name, highscoreslist[i].score);                       
        }
    }

    public struct HighScore //Creates a highscore struct in order to store all highscores in a list 
    {
        public string name;
        public int score; 

        public HighScore(string _name, int _score)
        {
            name = _name;
            score = _score; 
        }
    }

}

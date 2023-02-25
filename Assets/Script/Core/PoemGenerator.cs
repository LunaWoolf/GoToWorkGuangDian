using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text.RegularExpressions;

public class PoemGenerator : MonoSingleton<PoemGenerator>
{
    [Header("Word Bank")]
    public TextAsset nounRef;
    public TextAsset verbRef;
    public TextAsset adjRef;
    public TextAsset lineRef;

    public TextAsset nounRef_controversial;
    public TextAsset verbRef_controversial;
    public TextAsset adjRef_controversial;
 


    [Header("WordList")]
    List<string> nounsList = new List<string>();
    List<string> verbList = new List<string>();
    List<string> adjList =  new List<string>();

    [Header("UI Reference")]
    public GameObject PoemParent;
    public GameObject PoemPaper;
    Animator PoemPaperAnimator;

    [Header("Prefab Reference")]
    public GameObject PoemLine;

    string[] nouns;
    string[] verbs;
    string[] adjs;
    string[] lines;

    string[] nouns_controversial;
    string[] verbs_controversial;
    string[] adjs_controversial;

    public PoemPaperController poemPaperController;

    bool waitForNextPoem = false;
    

    void Awake()
    {
        ParseWorkList();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PoemPaper != null) PoemPaperAnimator = PoemPaper.GetComponent<Animator>();
        poemPaperController.OnPaperExitFinish.AddListener(TearPoem);
        //GeneratorPoem(5);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TearPoem();
            GeneratorPoem(5);
        }
    }

    public void ParseWorkList()
    {
        if(nounRef != null)
            nouns = nounRef.text.Split("\n");
        if (verbRef != null)
            verbs = verbRef.text.Split("\n");
        if (adjRef != null)
            adjs = adjRef.text.Split("\n");
        if (lineRef != null)
            lines = lineRef.text.Split("\n");
        if (nounRef_controversial != null)
            nouns_controversial = nounRef_controversial.text.Split("\n");
        if (verbRef_controversial != null)
            verbs_controversial = verbRef_controversial.text.Split("\n");
        if (adjRef_controversial != null)
            adjs_controversial = adjRef_controversial.text.Split("\n");
    }

    public void GeneratorPoem(int line)
    {
        string[] poem = new string[line];

        for (int i = 0; i < line; i++)
        {
            int rand = Random.Range(0, lines.Length);
            string line_tem = lines[rand];
            line_tem = ReplaceVerb(line_tem);
            line_tem = ReplaceNoun(line_tem);
            line_tem = ReplaceAdj(line_tem);
            poem[i] = line_tem;
            Debug.Log(line_tem);

            GameObject p = Instantiate(PoemLine, PoemParent.transform, false);
            p.GetComponent<PoemLine>().SetLine(line_tem);


        }
        
    }

    public string ReplaceVerb(string line)
    {
        string[] list = Regex.Split(line," ");
        string result = "";
        if (list.Length > 0)
        {
            foreach (string s in list)
            {
                string r = s;
                if (s.Contains("<v>"))
                {
                    int rand = Random.Range(0, verbs.Length);
                    r = r.Replace("<v>", verbs[rand]);
                }

                result += r + " ";
            }

            result = result.Substring(0, result.Length - 1);
        }
        else
        {
            result = line;
        }
     
        return result;
    }

    public string ReplaceNoun(string line)
    {
        string[] list = Regex.Split(line, " ");
        string result = "";
        if (list.Length > 0)
        {
            foreach (string s in list)
            {
                string r = s;
                if (s.Contains("<n>"))
                {
                    int rand = Random.Range(0, nouns.Length);
                    r = r.Replace("<n>", nouns[rand]);
                }

                result += r + " ";
            }
            result = result.Substring(0, result.Length - 1);
        }
        else
        {
            result = line;
        }
        return result;
    }

    public string ReplaceAdj(string line)
    {
        string[] list = Regex.Split(line, " ");
        string result = "";
        if (list.Length > 0)
        {
            foreach (string s in list)
            {
                string r = s;
                if (s.Contains("<adj>"))
                {
                    int rand = Random.Range(0, adjs.Length);
                    r = r.Replace("<adj>", adjs[rand]);
                }

                result +=  r + " ";
            }

            result = result.Substring(0, result.Length - 1);
        }
        else
        {
            result = line;
        }
        return result;
    }

    public void TearPoem()
    {
        foreach (Transform child in PoemParent.transform)
        {
            Destroy(child.gameObject);
        }

        if (waitForNextPoem)
        {
            GeneratorPoem(5);
            LoadPoemPaper();
            waitForNextPoem = false;
        }

    }

    public void NextPoem()
    {
        UnloadPoemPaper();
        waitForNextPoem = true;
    }

    public void UnloadPoemPaper() { PoemPaperAnimator.SetTrigger("Exit"); }
   

    public void LoadPoemPaper(){ PoemPaperAnimator.SetTrigger("Enter"); }


}

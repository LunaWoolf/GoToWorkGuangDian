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

    [Header("UI Reference_Read")]
    public GameObject PoemParent_Read;
    public GameObject PoemPaper_Read;
    Animator PoemPaperAnimator_Read;
    public PoemPaperController poemPaperController_Read;

    [Header("UI Reference_Write")]
    public GameObject PoemParent_Write;
    public GameObject PoemPaper_Write;
    Animator PoemPaperAnimator_Write;
    public PoemPaperController poemPaperController_Write;

    [Header("Prefab Reference")]
    public GameObject PoemLine;

    [Header("Generate Param")]
    [Range(0, 100)]
    public int ValidPrecentag = 70;
    [Range(0, 100)]
    public int ControversialPrecentage = 30;

    string[] nouns;
    string[] verbs;
    string[] adjs;
    string[] lines;

    string[] nouns_controversial;
    string[] verbs_controversial;
    string[] adjs_controversial;


    bool waitForNextPoem = false;

    string[] currentPoem;

    void Awake()
    {
        var objs = FindObjectsOfType<PoemGenerator>();

        if (objs.Length > 1)
        {
            foreach (var v in objs)
            {
                if (v.gameObject != this.gameObject)
                    Destroy(v.gameObject);
            }
        }

        DontDestroyOnLoad(this.gameObject);
        ParseWorkList();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PoemPaper_Read != null) PoemPaperAnimator_Read = PoemPaper_Read.GetComponent<Animator>();
        poemPaperController_Read.OnPaperExitFinish.AddListener(TearPoem);
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

    public string[] GeneratorPoem(int line)
    {
        string[] poem = new string[line];
        int rand = Random.Range(0, 100);
        bool isValid = (rand < ValidPrecentag) ? true : false;

        for (int i = 0; i < line; i++)
        {
            int randLine = Random.Range(0, lines.Length);
            string line_tem = lines[randLine];

            line_tem = ReplaceVerb(line_tem, isValid);
            line_tem = ReplaceNoun(line_tem, isValid);
            line_tem = ReplaceAdj(line_tem,  isValid);
            poem[i] = line_tem;
            Debug.Log(line_tem);

            GameObject p = Instantiate(PoemLine, PoemParent_Read.transform, false);
            p.GetComponent<PoemLine>().SetLine(line_tem);
        }
        currentPoem = poem;
        return poem;
    }

    public string ReplaceVerb(string line, bool isvalid)
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

                    int c = Random.Range(0, 100);
                    bool controversial = (c < ControversialPrecentage) ? true : false;
                    string w = "Default";
                    int rand = 0;
                    int i = 0;

                    if (!isvalid && controversial)
                    {
                        rand = Random.Range(0, verbs_controversial.Length);
                        w = "?" + verbs_controversial[rand];

                    }
                    else
                    {
                        rand = Random.Range(0, verbs.Length);
                        w = verbs[rand];

                    }

                    //Debug.Log("verb w =  " + w);
                    r = r.Replace("<v>", w);
                }
                result += r + " ";
            }

            result = result.Substring(0, result.Length - 1);
        }
        else
        {
            return line;
        }
     
        return result;
    }

    public string ReplaceNoun(string line, bool isvalid)
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
                    int c = Random.Range(0, 100);
                    bool controversial = (c < ControversialPrecentage) ? true : false;
                    string w = "Default";
                    int rand = 0;
                    int i = 0;

                    if (!isvalid && controversial)
                    {
                        rand = Random.Range(1, nouns_controversial.Length);
                        w = "?" + nouns_controversial[rand];

                    }
                    else
                    {
                        rand = Random.Range(1, nouns.Length);
                        w = nouns[rand];

                    }

                    r = r.Replace("<n>", w);

                }

                result += r + " ";
            }
            result = result.Substring(0, result.Length - 1);
        }
        else
        {
            return line;
        }
        return result;
    }

    public string ReplaceAdj(string line, bool isvalid)
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
                    int c = Random.Range(0, 100);
                    bool controversial = (c < ControversialPrecentage) ? true : false;
                    string w = "Default";
                    int rand = 0;
                    int i = 0;

                    if (!isvalid && controversial)
                    {
                        rand = Random.Range(1, adjs_controversial.Length);
                        w = "?" + adjs_controversial[rand];

                    }
                    else
                    {
                        rand = Random.Range(1, adjs.Length);
                        w = adjs[rand];
                    }


                    r = r.Replace("<adj>", w);


                }
                result +=  r + " ";
            }

            result = result.Substring(0, result.Length - 1);
        }
        else
        {
            return line;
        }

        return result;
    }

    public void TearPoem()
    {
        foreach (Transform child in PoemParent_Read.transform)
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

    public void OnPoemPass()
    {

        PropertyManager.instance.PassedPoem.Add(currentPoem);
    }

    public void OnPoemDeny()
    {
        PropertyManager.instance.DeniedPoem.Add(currentPoem);
    }

    public void UnloadPoemPaper() { if (PoemPaper_Read != null) PoemPaperAnimator_Read = PoemPaper_Read.GetComponent<Animator>(); PoemPaperAnimator_Read.SetTrigger("Exit"); }
   

    public void LoadPoemPaper() { if (PoemPaper_Read != null) PoemPaperAnimator_Read = PoemPaper_Read.GetComponent<Animator>(); PoemPaperAnimator_Read.SetTrigger("Enter"); }

    ///////////////////////////
    //////Writing Mode/////////
    ///////////////////////////


    [Header("Writing Mode")]
    PoemLine currentLine;
    public void AddWordToPoem(string word)
    {
        if (currentLine == null)
        {
            GameObject line = Instantiate(PoemLine, PoemParent_Write.transform, false);
            currentLine = line.GetComponent<PoemLine>();
        }

        currentLine.GetComponent<PoemLine>().SetLine(word);

        // p.GetComponent<PoemLine>().SetLine(line_tem);
    }

    public void AddNewLineToPoem()
    {
        GameObject line = Instantiate(PoemLine, PoemParent_Write.transform, false);
        currentLine = line.GetComponent<PoemLine>();
    }

    public void MoveWritePoemToReadPoem()
    {
        //Parent of poem -> paragraph
        PoemParent_Write.transform.parent = PoemPaper_Read.transform;
        PoemParent_Write.GetComponent<RectTransform>().position = PoemParent_Read.GetComponent<RectTransform>().position;
        PoemParent_Write.GetComponent<RectTransform>().localScale = PoemParent_Read.GetComponent<RectTransform>().localScale;
        PoemParent_Write.GetComponent<RectTransform>().rotation = PoemParent_Read.GetComponent<RectTransform>().rotation;
        
    }
}

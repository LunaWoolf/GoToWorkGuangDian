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

    public PoemPaperController poemPaperController;

    bool waitForNextPoem = false;

    string[] currentPoem;

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

            GameObject p = Instantiate(PoemLine, PoemParent.transform, false);
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
                        w = verbs_controversial[rand];

                    }
                    else
                    {
                        rand = Random.Range(0, verbs.Length);
                        w = verbs[rand];

                    }

                    Debug.Log("verb w =  " + w);
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
                    while (GameManager.instance.personalBannedWordMap.ContainsKey(w) || w == "BLANK")
                    {
                        i++;
                        if (GameManager.instance.denyPoemCount > 5 || GameManager.instance.personalBannedWordMap.Keys.Count > 10)
                        {
                            rand = Random.Range(0, 3);
                            if (rand % 2 == 0)
                            {
                                w = "BLANK";
                                //break;
                            }
                        }
                        else if (!isvalid && controversial)
                        {
                            rand = Random.Range(1, nouns_controversial.Length);
                            w = nouns_controversial[rand];

                        }
                        else
                        {
                            rand = Random.Range(1, nouns.Length);
                            w = nouns[rand];

                        }

                        if (i == 2)
                        {
                            rand = 0;
                            w = "BLANK";
                        }
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
                    while (GameManager.instance.personalBannedWordMap.ContainsKey(w) || w == "BLANK" )
                    {
                        i++;
                        if (GameManager.instance.denyPoemCount > 5 || GameManager.instance.personalBannedWordMap.Keys.Count > 10)
                        {
                            rand = Random.Range(0, 3);
                            if (rand % 2 == 0)
                            {
                                w = "BLANK";
                                //break;
                            }


                        }
                        else if (!isvalid && controversial)
                        {
                            rand = Random.Range(1, adjs_controversial.Length);
                            w = adjs_controversial[rand];

                        }
                        else
                        {
                            rand = Random.Range(1, adjs.Length);
                            w = adjs[rand];
                        }

                        if (i == 2)
                        {
                            rand = 0;
                            w = "BLANK";
                        }
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

    public void OnPoemPass()
    {

        PropertyManager.instance.PassedPoem.Add(currentPoem);
    }

    public void OnPoemDeny()
    {
        PropertyManager.instance.DeniedPoem.Add(currentPoem);
    }

    public void UnloadPoemPaper() { if (PoemPaper != null) PoemPaperAnimator = PoemPaper.GetComponent<Animator>(); PoemPaperAnimator.SetTrigger("Exit"); }
   

    public void LoadPoemPaper() { if (PoemPaper != null) PoemPaperAnimator = PoemPaper.GetComponent<Animator>(); PoemPaperAnimator.SetTrigger("Enter"); }

    ///////////////////////////
    //////Writing Mode/////////
    ///////////////////////////


    [Header("Writing Mode")]
    PoemLine currentLine;
    public void AddWordToPoem(Word Word)
    {
        if (currentLine == null)
        {
            GameObject line = Instantiate(PoemLine, PoemParent.transform, false);
            currentLine = line.GetComponent<PoemLine>();
        }
        
       // p.GetComponent<PoemLine>().SetLine(line_tem);
    }

}

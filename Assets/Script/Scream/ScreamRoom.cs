using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamRoom : MonoBehaviour
{
    float screamHoldingTime;
    public string WordToSpawn = "AHHHHHHHH";
    char[] CharToSpawn;
    public GameObject PlayerMouthPosition;
    public GameObject ScreamWordBullet;
    int currentCharToSpawnIndex = 0;
    public PlayerMovement playerMovement;

    List<GameObject> ScreamWordBulletList = new List<GameObject>();
    int currentScreamWordBulletListIndex = 0;

    void Start()
    {
        CharToSpawn = WordToSpawn.ToCharArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            OnScreamKeyHold();
        }
        else
        {
            OnScreamKeyReleased();
        }

    }

    void OnScreamKeyHold()
    {
        screamHoldingTime += 1f;

        if (screamHoldingTime % 4f == 0)
        {
            GameObject screamWord;
            if (ScreamWordBulletList.Count < 50)
            {
                screamWord = Instantiate(ScreamWordBullet, PlayerMouthPosition.transform.position, PlayerMouthPosition.transform.rotation);
                ScreamWordBulletList.Add(screamWord);
            }
            else
            {
                screamWord = ScreamWordBulletList[currentScreamWordBulletListIndex];
                currentScreamWordBulletListIndex++;
                if (currentScreamWordBulletListIndex >= ScreamWordBulletList.Count)
                {
                    currentScreamWordBulletListIndex = 0;
                }

                screamWord.GetComponent<ScreamWordBullet>().isActive = false;
                screamWord.transform.position = PlayerMouthPosition.transform.position;
                screamWord.transform.rotation = PlayerMouthPosition.transform.rotation;
                screamWord.GetComponent<ScreamWordBullet>().ResetScreamWordBullet();

            }

            screamWord.GetComponent<ScreamWordBullet>().SetText(CharToSpawn[currentCharToSpawnIndex].ToString());
            screamWord.GetComponent<ScreamWordBullet>().direction = PlayerMouthPosition.transform.right.normalized * playerMovement.isFliped;
            screamWord.GetComponent<ScreamWordBullet>().isActive = true;

            currentCharToSpawnIndex++;
            if (currentCharToSpawnIndex >= CharToSpawn.Length)
            {
                currentCharToSpawnIndex = 0;
            }
           
        }

     
    }


    void OnScreamKeyReleased()
    {
        if (screamHoldingTime <= 0)
            return;

        currentCharToSpawnIndex = 0;

        screamHoldingTime -= 1f;

    }

}

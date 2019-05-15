using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    public string winText;
    bool gameover;
    public GameObject clue1;
    public GameObject clue2;
    public GameObject clue3;
    int spawnPointX1;
    int spawnPointX2;
    int spawnPointX3;
    int spawnPointZ1;
    int spawnPointZ2;
    int spawnPointZ3;

    Vector3 spawnPosition1;

    Vector3 spawnPosition2;

    Vector3 spawnPosition3;

    // Start is called before the first frame update
    void Start()
    {
        winText = "";
        gameover = false;
        spawnPointX1 = Random.Range(3, 50);
        spawnPointX2 = Random.Range(3, 50);
        spawnPointX3 = Random.Range(3, 50);
        spawnPointZ1 = Random.Range(-30, 5);
        spawnPointZ2 = Random.Range(-30, 5);
        spawnPointZ3 = Random.Range(-30, 5);
        spawnPosition1 = new Vector3(spawnPointX1, 1, spawnPointZ1);
        spawnPosition2 = new Vector3(spawnPointX2, 1, spawnPointZ2);
        spawnPosition3 = new Vector3(spawnPointX3, 1, spawnPointZ3);
        clue1 = GameObject.FindGameObjectWithTag("clue1");
        clue2 = GameObject.FindGameObjectWithTag("clue2");
        clue3 = GameObject.FindGameObjectWithTag("clue3");
        clue1.transform.position = spawnPosition1;
        clue2.transform.position = spawnPosition2;
        clue3.transform.position = spawnPosition3;
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetText()
    {
        if (ClueController.Clue1 && ClueController.Clue2 && ClueController.Clue3)
        {
            winText = "User Won";
            gameover = true;
        }
    }
}

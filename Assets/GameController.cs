using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    public string winText;
    bool gameover;
    // Start is called before the first frame update
    void Start()
    {
        winText = "";
        gameover = false;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class Clue3 : MonoBehaviour
{
    [SerializeField]
    public InteractionObject clue;

    public GameObject pickup;

    [SerializeField]
    public InteractionSystem pIS;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {

            pIS.StartInteraction(FullBodyBipedEffector.RightHand, clue, false);
            if (pIS.GetInteractionObject(FullBodyBipedEffector.RightHand) == clue)
            {
                Destroy(this.pickup, 5);
                ClueController.Clue3 = true;
                Debug.Log("Clue3:"+ClueController.Clue3);
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;
using RootMotion.FinalIK;

public class Clue1 : MonoBehaviour
{
    [SerializeField]
    public InteractionObject clue;

    public GameObject pickup;
    public GameObject player;

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

        Val<Vector3> playerpos = Val.V(() => player.transform.position);
        Val<Vector3> cluepos = Val.V(() => pickup.transform.position);
        Val<Vector3> diffpos = Val.V(() => playerpos.Value - cluepos.Value);
        Val<float> distaway = Val.V(() => diffpos.Value.sqrMagnitude); 

        if (Input.GetKeyDown(KeyCode.E) && distaway.Value < 2.0)
        {

            pIS.StartInteraction(FullBodyBipedEffector.RightHand, clue, false);
            
            if(pIS.GetInteractionObject(FullBodyBipedEffector.RightHand) == clue)
            {
                Destroy(this.pickup, 5);
                ClueController.Clue1 = true;
                Debug.Log(ClueController.Clue1);
            }

        }
    }
}

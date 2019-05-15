using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;
using RootMotion.FinalIK;

public class Clue3 : MonoBehaviour
{
    [SerializeField]
    public InteractionObject clue;

    public GameObject pickup;

    [SerializeField]
    public InteractionSystem pIS;

    public GameObject player;

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
            if (pIS.GetInteractionObject(FullBodyBipedEffector.RightHand) == clue)
            {
                Destroy(this.pickup, 5);
                ClueController.Clue3 = true;
                Debug.Log("Clue3:"+ClueController.Clue3);
            }

        }
    }
}

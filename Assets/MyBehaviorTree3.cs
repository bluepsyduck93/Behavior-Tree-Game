using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class MyBehaviorTree3 : MonoBehaviour
{
	public Transform wander1;
	public Transform wander2;
	public Transform wander3;
    GameObject staff;
    GameObject cult1;
    GameObject cult2;
    GameObject cult3;


	private BehaviorAgent behaviorAgent;
	// Use this for initialization
	void Start ()
	{
        staff = GameObject.FindGameObjectWithTag("staff");
        cult1 = GameObject.FindGameObjectWithTag("cult1");
        cult2 = GameObject.FindGameObjectWithTag("cult2");
        cult3 = GameObject.FindGameObjectWithTag("cult3");

        behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	protected Node ST_ApproachAndWait(GameObject participant, Transform target)
	{
		Val<Vector3> position = Val.V (() => target.position);
        Val<float> offset = Val.V(() => 1.0f);
        Debug.Log(offset);
		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(position,offset), new LeafWait(1000));
	}

    protected Node WalkTo(GameObject currentPerson)
    {
        Vector3 position = new Vector3(staff.transform.position.x, 0f, staff.transform.position.z);
        return new Sequence(new LeafInvoke(() => currentPerson.GetComponent<SteeringController>().Target = (position)));
    }

    protected Node BuildTreeRoot()
	{
        //Val<float> pp = Val.V (() => police.transform.position.z);
        //Func<bool> act = () => (police.transform.position.z > 10);
        Node roaming =
            new DecoratorLoop(
                new SequenceParallel(
                    this.WalkTo(cult1),
                    this.WalkTo(cult2),
                    this.WalkTo(cult3)
                )
            );
        //Node root = new DecoratorLoop
        return roaming;
	}
}

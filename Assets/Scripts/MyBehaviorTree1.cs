using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

public class MyBehaviorTree1 : MonoBehaviour
{
    public GameObject cube1;
    public GameObject cube2;
    //public GameObject participant1;
    //public GameObject participant2;
    //public GameObject participant3;
    GameObject staff;
    GameObject cult1;
    GameObject cult2;
    GameObject cult3;
    GameObject flash;
    GameObject vampire;
    GameObject offscreen;
    bool summonactive;
    float _t = 0f;

    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
        offscreen = GameObject.FindGameObjectWithTag("offscreen");
        vampire = GameObject.FindGameObjectWithTag("vampire");
        vampire.SetActive(false);
        flash = GameObject.FindGameObjectWithTag("light");
        summonactive = false;
        staff = GameObject.FindGameObjectWithTag("staff");
        cult1 = GameObject.FindGameObjectWithTag("cult1");
        cult2 = GameObject.FindGameObjectWithTag("cult2");
        cult3 = GameObject.FindGameObjectWithTag("cult3");
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }

    // Update is called once per frame
    void Update()
    {
        if (summonactive)
        {
            //flash
            flash.GetComponent<Flash>().CameraFlash();
            //swap in staff and demon models
            vampire.SetActive(true);
            staff.SetActive(false);

        }
        else if(!summonactive)
        {
            //have staff active, demon away
            vampire.SetActive(false);
            staff.SetActive(true);
        }
    }
    protected Node BuildTreeRoot()
    {
        return new DecoratorLoop(
                new Sequence(
                    this.MoveCultRoot()
                )
            );
    }
    protected Node AssertAndDie(GameObject currentPerson)
    {
        return new DecoratorLoop(new Sequence(new DecoratorInvert(new DecoratorLoop((new DecoratorInvert(new Sequence(this.CheckDead(currentPerson, cube2)))))),
                                              currentPerson.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("DYING",10000)));
    }

    protected Node CheckDead(GameObject p, GameObject cube)
    {
        Val<Vector3> ppos = Val.V(() => p.transform.position);
        Val<Vector3> cubepos = Val.V(() => cube.transform.position);
        Val<Vector3> diffpos = Val.V(() => ppos.Value-cubepos.Value);
        Val<float> distaway = Val.V(() => ppos.Value.sqrMagnitude);
        return new LeafAssert(() => (distaway.Value < 5.0));


    }

    protected Node MoveCultRoot()
    {
        return new DecoratorLoop(new DecoratorInvert(
                                 new DecoratorLoop(
                                (new SequenceParallel(WalkToCube(cult1), 
                                                      WalkToCube(cult2), 
                                                      WalkToCube(cult3))))));
    }
    protected Node PraiseCultRoot()
    {
        return new SequenceParallel(new LeafWait(1000),
                                                      ParticipantPraise(cult1),
                                                      ParticipantPraise(cult2),
                                                      ParticipantPraise(cult3));
    }
    
    protected Node FearCultRoot()
    {
        return new DecoratorLoop(new DecoratorInvert(
                                 new DecoratorLoop(
                                 new SequenceParallel(new LeafWait(1000),
                                                      ParticipantFear(cult1),
                                                      ParticipantFear(cult2),
                                                      ParticipantFear(cult3)))));
    }

    protected Node CubeFloats(GameObject cube)
    {
        Vector3 lowestPosition = cube.transform.position;

        Vector3 highestPosition = lowestPosition + new Vector3 (0, 3, 0);

        return new DecoratorLoop(new Sequence(new LeafInvoke(()=> StartCoroutine(ApproachObj(cube1,lowestPosition, highestPosition))),
                                              new LeafWait(10500),
                                              new LeafInvoke(() => StartCoroutine(ApproachObj(cube1, highestPosition, lowestPosition))),
                                              new LeafWait(10500)));

    }
    protected Node WalkToCube(GameObject currentPerson)
    {
        Vector3 cubeposition = new Vector3(staff.transform.position.x, 0f, staff.transform.position.z);
        return new Sequence(new LeafInvoke(()=> currentPerson.GetComponent<SteeringController>().Target = (cubeposition)));
    }
    protected Node RunAway(GameObject currentPerson)
    {
        Vector3 cubeposition = new Vector3(offscreen.transform.position.x, 0f, offscreen.transform.position.z);
        return new Sequence(new LeafInvoke(() => currentPerson.GetComponent<SteeringController>().Target = (cubeposition)));
    }
    protected Node ParticipantPraise(GameObject currentPerson)
    {
        return new Sequence(currentPerson.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CLAP", 10000), new LeafWait(1000));
    }
    protected Node ParticipantFear(GameObject currentPerson)
    {
        return new Sequence(currentPerson.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("DUCK", 10000), new LeafWait(1000));
    }
    protected Node summon(GameObject cult1, GameObject cult2, GameObject cult3)
    {
        return new Sequence(
                //participantpraise,
                ParticipantPraise(cult1),
                ParticipantPraise(cult2),
                ParticipantPraise(cult3),
                //appearance,
                //staffSwap(),
                //create new sequenceshuffle
                new SequenceParallel(
                        ParticipantFear(cult1),
                        RunAway(cult2),
                        AssertAndDie(cult3)
                    )
            );
    }
    /*
    protected Node desummon()
    {
        summonactive = false;
    }
    protected Node staffSwap()
    {
        summonactive = true;
    }
    */
    IEnumerator ApproachObj(GameObject cube, Vector3 start, Vector3 end)
    {
        float startTime = Time.time;
        while (Time.time < startTime + 10f)
        {
            cube.transform.position = Vector3.Lerp(start, end, (Time.time - startTime)/10f);
            yield return null;
        }
        cube.transform.position = end;
        

    }


}
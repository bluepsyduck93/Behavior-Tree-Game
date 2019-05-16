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

    public GameObject cult4;
    public GameObject cult5;
    public GameObject cult6;

    public GameObject cult7;
    public GameObject cult8;
    public GameObject cult9;

    GameObject flash;
    GameObject vampire;
    GameObject fairy;
    GameObject offscreen;
    GameObject position1;
    GameObject position2;
    bool summonactive;
    bool demonactive;
    bool fairyactive;
    float _t = 0f;

    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
        position1 = GameObject.FindGameObjectWithTag("point1");
        position2 = GameObject.FindGameObjectWithTag("point2");
        offscreen = GameObject.FindGameObjectWithTag("offscreen");
        vampire = GameObject.FindGameObjectWithTag("vampire");
        vampire.SetActive(false);
        fairy = GameObject.FindGameObjectWithTag("fairy");
        fairy.SetActive(false);
        flash = GameObject.FindGameObjectWithTag("light");
        flash.GetComponent<Flash>().doCameraFlash = false;
        summonactive = true;
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

    }
    protected Node BuildTreeRoot()
    {
        int rand = UnityEngine.Random.Range(0, 2);
        //Debug.Log(rand);
        return
                new Sequence(
                    //PraiseCultRoot(),
                    new SequenceParallel(
                    MoveCrowdRoot(),
                    MoveCultRoot()
                    ),
                    new LeafWait(1800),
                    summon(cult1,cult2,cult3, rand),
                    //new LeafWait(300),
                    desummon(rand)
                    //summon(cult1, cult2, cult3)
                //this.MoveCultRoot()
                );
    }
    protected Node EveryoneDeadRoot()
    {
        return new SequenceParallel(Die(cult7), Die(cult8), Die(cult9), Die(cult3));
    }
    protected Node Die(GameObject currentPerson)
    {
        return new Sequence(currentPerson.GetComponent<BehaviorMecanim>().Node_BodyAnimation("DYING", true), new LeafWait(1000));
    }

    protected Node MoveCultRoot()
    {
        return new SequenceParallel(new LeafWait(1000),
                                        RunAway(cult1, staff,2.0f),
                                        WalkToCube(cult2, position1),
                                        WalkToCube(cult3, position2)
                                    );
        /*
        return new DecoratorLoop(new DecoratorInvert(
                                 new DecoratorLoop(
                                (new SequenceParallel(WalkToCube(cult1, staff),
                                                      WalkToCube(cult2, position2),
                                                      WalkToCube(cult3, position1))))));
                                                      */
    }
    protected Node MoveCrowdRoot()
    {
        return new  SequenceParallel(new LeafWait(1000),
                                        RunAway(cult4, staff, 7.0f),
                                        RunAway(cult5, staff, 7.0f),
                                        RunAway(cult6, staff, 7.0f),
                                        facetowards(cult7,cult8),
                                        facetowards(cult8,cult9),
                                        facetowards(cult9,cult7)
                                    );
 
    }
    void _summon(bool setactive, int setsummon)
    {
        //flash
        //Debug.Log(setsummon);
        if (setactive)
        {
            cult1.GetComponent<SteeringController>().maxSpeed = 50.0f;
            flash.GetComponent<Flash>().doCameraFlash = true;
            if (setsummon == 0)
            {
                //swap in staff and demon models
                vampire.SetActive(true);
                staff.SetActive(false);
            }
            else if(setsummon == 1)
            {
                fairy.SetActive(true);
                staff.SetActive(false);
            }
            

        }
        else
        {
            flash.GetComponent<Flash>().doCameraFlash = true;
            //have staff active, demon away
            vampire.SetActive(false);
            fairy.SetActive(false);
            staff.SetActive(true);
            cult1.SetActive(false);
            cult2.SetActive(false);
            cult3.SetActive(false);
            cult4.SetActive(false);
            cult5.SetActive(false);
            cult6.SetActive(false);
            cult7.SetActive(false);
            cult8.SetActive(false);
            cult9.SetActive(false);
        }
    }
    protected Node staffSwap(bool setSummon, int type)
    {
        return new Sequence(
                //new LeafWait(200),
                new LeafInvoke(() => _summon(setSummon, type))
            );
    }
    protected Node summon(GameObject cult1, GameObject cult2, GameObject cult3, int summontype)
    {
        if (summontype == 0)
        {
            //summoned demon
            return new Sequence(
                        PraiseCultRoot(),
                        //new LeafWait(500),
                        staffSwap(true, summontype),
                        new SequenceParallel(
                            //ParticipantFear(cult2),
                            FearCultRoot(),
                            RunAway(cult1,offscreen, 1.0f),
                            EveryoneDeadRoot()
                        )
                   );
        }
        return new Sequence(
                PraiseCultRoot(),
                //new LeafWait(500),
                staffSwap(true, summontype),
                new SequenceParallel(
                    facetowards(cult2,cult3),
                    facetowards(cult3,cult2)
                ),
                new SequenceParallel
                (
                    fight(cult2,cult3),
                    fight(cult3,cult2),
                    ParticipantFear(cult7),
                    Die(cult8),
                    Die(cult4),
                    ParticipantFear(cult5),
                    Die(cult6),
                    ParticipantFear(cult9),
                    RunAway(cult1,offscreen,1.0f)
                )

            );
    }
  
    protected Node desummon(int summontype)
    {
        return new Sequence(
                //new LeafWait(200),
                staffSwap(false, summontype)
            );
    }

    protected Node facetowards(GameObject cultFrom, GameObject cultTo)
    {
        BehaviorMecanim behaviormec = cultFrom.GetComponent<BehaviorMecanim>();
        return behaviormec.Node_OrientTowards(cultTo.transform.position);
    }

    protected Node fight(GameObject cultFrom, GameObject cultTo)
    {
        BehaviorMecanim behaviormec = cultFrom.GetComponent<BehaviorMecanim>();
        return behaviormec.Node_Fight(cultTo.transform.position, true);
    }

    #region b3code
    protected Node PraiseCultRoot()
    {
        return new SequenceParallel(new LeafWait(1000),
                                                      ParticipantPraise(cult1),
                                                      ParticipantPraise(cult2),
                                                      ParticipantPraise(cult3));
    }

    protected Node FearCultRoot()
    {
        return
            new SequenceParallel(//new LeafWait(1000),
                                ParticipantFear(cult4),
                                ParticipantFear(cult2),
                                ParticipantFear(cult5),
                                ParticipantFear(cult6));
    }

    protected Node CubeFloats(GameObject cube)
    {
        Vector3 lowestPosition = cube.transform.position;

        Vector3 highestPosition = lowestPosition + new Vector3(0, 3, 0);

        return new DecoratorLoop(new Sequence(new LeafInvoke(() => StartCoroutine(ApproachObj(cube1, lowestPosition, highestPosition))),
                                              new LeafWait(10500),
                                              new LeafInvoke(() => StartCoroutine(ApproachObj(cube1, highestPosition, lowestPosition))),
                                              new LeafWait(10500)));

    }
    protected Node WalkToCube(GameObject currentPerson, GameObject walkTo)
    {
        Vector3 cubeposition = new Vector3(walkTo.transform.position.x, 0f, walkTo.transform.position.z);
        return new Sequence(new LeafInvoke(() => currentPerson.GetComponent<SteeringController>().Target = (cubeposition)));
    }
    protected Node RunAway(GameObject currentPerson, GameObject positionTo, float radius)
    {
        Vector3 cubeposition = new Vector3(positionTo.transform.position.x, 0f, positionTo.transform.position.z);
        
        return new Sequence(
            currentPerson.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(cubeposition,radius)
            );
    }
    protected Node ParticipantPraise(GameObject currentPerson)
    {
        return new Sequence(currentPerson.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CLAP", 10000), new LeafWait(1000));
    }
    protected Node ParticipantFear(GameObject currentPerson)
    {
        return new Sequence(currentPerson.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("DUCK", 10000), new LeafWait(1000));
    }
    /*
protected Node CheckDead(GameObject p, GameObject cube)
{
    Val<Vector3> ppos = Val.V(() => p.transform.position);
    Val<Vector3> cubepos = Val.V(() => cube.transform.position);
    Val<Vector3> diffpos = Val.V(() => ppos.Value - cubepos.Value);
    Val<float> distaway = Val.V(() => ppos.Value.sqrMagnitude);
    return new LeafAssert(() => (distaway.Value < 5.0));


}*/
    #endregion
    IEnumerator ApproachObj(GameObject cube, Vector3 start, Vector3 end)
    {
        float startTime = Time.time;
        while (Time.time < startTime + 10f)
        {
            cube.transform.position = Vector3.Lerp(start, end, (Time.time - startTime) / 10f);
            yield return null;
        }
        cube.transform.position = end;


    }


}
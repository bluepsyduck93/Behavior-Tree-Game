using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;

public class MyBehaviorTree : MonoBehaviour
{
    GameObject staff;
    GameObject cult1;
    GameObject cult2;
    GameObject cult3;
    GameObject flash;
    GameObject vampire;
    GameObject player;
    GameObject clue1;
    GameObject cube;
    bool summonactive;
    bool evil;
    float _t = 0f;

    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
        /*Intializing the game Objects*/
        vampire = GameObject.FindGameObjectWithTag("vampire");
        vampire.SetActive(false);
        flash = GameObject.FindGameObjectWithTag("light");
        summonactive = false;
        staff = GameObject.FindGameObjectWithTag("staff");
        cult1 = GameObject.FindGameObjectWithTag("cult1");
        cult2 = GameObject.FindGameObjectWithTag("cult2");
        cult3 = GameObject.FindGameObjectWithTag("cult3");
        player = GameObject.FindGameObjectWithTag("player");
        cube = GameObject.FindGameObjectWithTag("cube");
        clue1 = GameObject.FindGameObjectWithTag("clue1");
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
        /*Evil Timer Start*/
        _t += Time.deltaTime;
        if (_t >= 10.0)
        {
            _t = 0f;
            changeCubeColor();
        }
        /*Evil Timer End*/
    }
    protected Node BuildTreeRoot()
    {
        /*DO NOT EDIT START*/
        return
            new Sequence(
            new SequenceParallel(
            MoveCultRoot(), ObjFloats(staff),
            PraiseCultRoot()),
            AssertFearCultRoot(),

        /*DO NOT EDIT END*/
          /* this.staffSwap(),*/
           startPlayer()
        /*ADD OTHER NODES BELOW LIKE THIS -> , MyNode1(), MyNode2() */   
           );

    }

    /*Death Functions (Will Implement Later)*/

    /*protected Node AssertAndDie(GameObject currentPerson)
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


    }*/

    /*DO NOT EDIT START*/

    /*Cultists are Praising Here*/
    protected Node MoveCultRoot()
    {
        return new SequenceParallel(WalkToObj(cult1), 
                                                      WalkToObj(cult2), 
                                                      WalkToObj(cult3));
    }
    protected Node PraiseCultRoot()
    {
        return new SequenceParallel(new LeafWait(100),
                                                      ParticipantPraise(cult1),
                                                      ParticipantPraise(cult2),
                                                      ParticipantPraise(cult3));
    }
    protected Node WalkToObj(GameObject currentPerson)
    {
        Vector3 staffposition = new Vector3(staff.transform.position.x, 0f, staff.transform.position.z);
        return new Sequence(currentPerson.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(staffposition, 1.0f));
    }
    protected Node ParticipantPraise(GameObject currentPerson)
    {
        return new Sequence(currentPerson.GetComponent<BehaviorMecanim>().ST_PlayGesture("CLAP", AnimationLayer.Hand, 10000));
    }

    /* Staff Floating */
    protected Node ObjFloats(GameObject obj)
    {
        Vector3 lowestPosition = obj.transform.position;

        Vector3 highestPosition = lowestPosition + new Vector3 (0, 3, 0);

        return new Sequence(new LeafInvoke(()=> StartCoroutine(ApproachObj(obj,lowestPosition, highestPosition))),
                                              new LeafWait(1000),
                                              new LeafInvoke(() => StartCoroutine(ApproachObj(obj, highestPosition, lowestPosition))),
                                              new LeafWait(1000));

    }
    IEnumerator ApproachObj(GameObject obj, Vector3 start, Vector3 end)
    {
        float startTime = Time.time;
        while (Time.time < startTime + 10f)
        {
            obj.transform.position = Vector3.Lerp(start, end, (Time.time - startTime) / 10f);
            yield return null;
        }
        obj.transform.position = end;


    }
    /*Evil Starts Here*/
    void changeCubeColor()
    {
        cube.transform.GetComponent<Renderer>().material.color = Color.red;

        evil = true;
    }

    protected Node AssertFearCultRoot()
    {
        return
               new Sequence(
               new DecoratorInvert(
               new DecoratorLoop((new DecoratorInvert(new Sequence(this.CheckEvil()))))),
               new SequenceParallel(ParticipantFear(cult1), ParticipantFear(cult2), ParticipantFear(cult3)));
    }

    protected Node CheckEvil()
    {
        return new LeafAssert(() => evil);
    }

    protected Node ParticipantFear(GameObject currentPerson)
    {
        return new Sequence(currentPerson.GetComponent<BehaviorMecanim>().ST_PlayGesture("DUCK", AnimationLayer.Body, 10000));
    }
    /*DO NOT EDIT END*/

    /*Demon + Staff Mechanism*/
    protected Node desummon()
    {
        return new LeafAssert(() => summonactive = false);
    }
    protected Node staffSwap()
    {
        return new LeafAssert(()=>summonactive = true);
    }


    protected Node startPlayer()
    {
        return new Sequence(playerPickUp());
    }
   protected Node playerPickUp()
    {
        Vector3 clueposition = new Vector3(clue1.transform.position.x, clue1.transform.position.y, clue1.transform.position.z);
        Val<InteractionObject> clue = Val.V(() => { return this.clue1.GetComponent<InteractionObject>(); });
        Val<FullBodyBipedEffector> right = FullBodyBipedEffector.RightHand;

       return new Sequence(player.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(clueposition, 1.0f)
           /*,
           player.GetComponent<BehaviorMecanim>().Node_StartInteraction(right, clue)*/);
    }

}
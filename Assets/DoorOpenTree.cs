/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;
using RootMotion.FinalIK;
using UnityEngine.UI;

public class DoorOpenTree : MonoBehaviour
{
    public Transform doorEntryLocation;
    public GameObject participant;
    public GameObject door;
    public Transform doorCloseLocation;
    public List<GameObject> marketCrowd;
    public List<Transform> wanderingPoints;
    public GameObject dialogBox;
    //public GameObject textBox;
    //IK related interface
    
    //public InteractionObject ikBall;
    public FullBodyBipedEffector hand;


    private BehaviorAgent behaviorAgent;
    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
        //textBox.SetActive(false);
    }

    #region IK related function
    *//*protected Node PickUp(GameObject p)
    {
        return new Sequence(this.Node_BallStop(),
                            p.GetComponent<BehaviorMecanim>().Node_StartInteraction(hand, ikBall),
                            new LeafWait(1000),
                            p.GetComponent<BehaviorMecanim>().Node_StopInteraction(hand));
    }*//*
    protected Node walkThroughDoor(GameObject p)
    {
        
        return new Sequence(p.GetComponent<BehaviorMecanim>().Node_HandAnimation("Wave", true),new LeafWait(1500));
    }

    protected Node openTrigger()
    {
        return new Sequence(new LeafInvoke(() => door.GetComponent<Animator>().SetTrigger("open")));
    }

    protected Node closeTrigger()
    {
                return new Sequence(new LeafInvoke(() => door.GetComponent<Animator>().SetTrigger("close")));
    }

   *//* protected virtual RunStatus closeGate()
    {
        
        new LeafWait(1000);
        return RunStatus.Success;
    }*/
    /*public Node Node_BallStop()
    {
        return new LeafInvoke(() => this.BallStop());
    }
    public virtual RunStatus BallStop()
    {
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        return RunStatus.Success;
    }

    protected Node PutDown(GameObject p)
    {
        return new Sequence(p.GetComponent<BehaviorMecanim>().Node_StartInteraction(hand, ikBall),
                            new LeafWait(300),
                            this.Node_BallMotion(),
                            new LeafWait(500), p.GetComponent<BehaviorMecanim>().Node_StopInteraction(hand));
    }

    public Node Node_BallMotion()
    {
        return new LeafInvoke(() => this.BallMotion());
    }

    public virtual RunStatus BallMotion()
    {
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.isKinematic = false;
        ball.transform.parent = null;
        return RunStatus.Success;
    }
    
*//*
    #endregion
    protected Node ST_ApproachAndWait(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_GoTo(position),new LeafWait(1000));
    }

    protected Node BuildTreeRoot()
    {
        Node roaming = new SequenceParallel(new Sequence(this.ST_ApproachAndWait(this.doorEntryLocation), 
            this.walkThroughDoor(participant),new LeafWait(1300), this.openTrigger(), new LeafWait(1000),
            this.ST_ApproachAndWait(this.doorCloseLocation),
            new LeafWait(100), this.closeTrigger(),participant.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave",100),
            new LeafWait(1000),this.talk(participant),new LeafWait(1000)));
       //,new DecoratorLoop(mobLoop()));
        return roaming;
    }

    protected Node talk(GameObject p)
    {
        return new LeafInvoke(() => this.charInteract(p));
        Debug.Log("Talk Method");
        
        //charClass.text.SetActive(false);

        //AndroidDialogAndToastBinding.instance.toastShort("Hello,How you doing?");
        return new LeafWait(1000);
    }

    protected virtual RunStatus charInteract(GameObject p)
    {
        dialogueBoxScript dial = dialogBox.GetComponent<dialogueBoxScript>();
        //dial.transform.position = p.transform.position;
        dial.show(true);
        Text t= dialogBox.GetComponentInChildren<Text>();
        t.text = "Hey!!";
        Debug.Log("Talk");
        new LeafWait(50000);
        return RunStatus.Success;

    }
  *//*  protected Node crowdBehavior(GameObject p)
    {
       *//* foreach (var p in marketCrowd)
        {*//*

            System.Random r = new System.Random();
            Val<Vector3> position = Val.V(() => wanderingPoints[r.Next(0, wanderingPoints.Count)].position);
            Node talk = new SequenceParallel(p.GetComponent<BehaviorMecanim>().Node_GoTo(position), p.GetComponent<BehaviorMecanim>().Node_HandAnimation("Talk on Phone", true));
        //      }
        return talk;

        }

    protected Node mobLoop()
        {
        var list = new List<Node>();
            for (int i = 0; i < marketCrowd.Count; i++)
            {
                list.Add(crowdBehavior(marketCrowd[i]));
            }
        //list.ToArray();
            return new SequenceParallel(list.ToArray());
        }
  *//*
}

*/
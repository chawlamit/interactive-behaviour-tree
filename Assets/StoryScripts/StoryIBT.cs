using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK.Demos;
using UnityEngine;
using UnityEngine.UI;
using TreeSharpPlus;


public class StoryIBT : MonoBehaviour
{
	public GameObject hero;
	public GameObject enemy;
	public GameObject sideCharactersParent;
	public static int clue_Count;
	public static Dictionary<String, bool> blackboard = new Dictionary<String, bool>();
	public static Dictionary<GameObject,bool> blackboardTrigger = new Dictionary<GameObject, bool>();
	public static Dictionary<GameObject,bool> alreadytalked = new Dictionary<GameObject, bool>();
	public static List<GameObject> people = new List<GameObject>();
	private static Vector3 targetPosition;
	
	private BehaviorAgent behaviorAgent;
	private List<GameObject> sideCharacters = new List<GameObject>();
	
    // Start is called before the first frame update
    void Start()
    {

	    clue_Count = 0;
	    for (int i = 0; i < sideCharactersParent.transform.childCount; i++)
	    {
		    var childObject = sideCharactersParent.transform.GetChild(i).gameObject;
		    if (childObject.CompareTag("Group"))
		    {
			    var child1 = childObject.transform.GetChild(0).gameObject;
			    var child2 = childObject.transform.GetChild(1).gameObject;
			    blackboardTrigger[child1] = false;
			    alreadytalked[child1] = false;
			    blackboardTrigger[child2] = false;
			    alreadytalked[child2] = false;
		    }
		    else
		    {
			    blackboardTrigger[childObject] = false;
			    alreadytalked[childObject] = false;
		    }
		    sideCharacters.Add(childObject);
	    }
	    
	    // initalizing blackborad
	    blackboard.Add("move", false); //mouse left click
	    blackboard.Add("talk", false); // right mouse click to talk
	    
	    behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
	    BehaviorManager.Instance.Register (behaviorAgent);
	    behaviorAgent.StartBehavior ();
    }

    private void Update()
    {
	    Debug.Log("b: "+ blackboard["talk"]);
	    Debug.Log("c: "+ clue_Count);
    }

    public static bool CheckTrigger(GameObject go)
    {
	    return blackboardTrigger[go];
    }
    
    public static bool ComeBack(GameObject go)
    {
	    return alreadytalked[go];
    }

    protected Node BuildTreeRoot()
    {
	    Node ibt = new SequenceParallel(
		    StoryTree(), 
		    UserInput(),
			Move(),
		    SideCharactersAffordances()
		    );
	    return ibt;
    }
    
    private Node Move()
    {
	    Val<Vector3> position = Val.V (() => targetPosition);
	    return new DecoratorLoop( new Selector(new LeafAssert(()=> blackboard["move"] == false),
		    new Sequence(hero.GetComponent<BehaviorMecanim>().Node_GoTo(position),
			    new LeafInvoke(()=>blackboard["move"] = false))
	    ));
    }
    

    #region userinput
    private Node UserInput()
    {
	    return new DecoratorLoop(  			    
			new LeafInvoke(() => GetUserInput())
	    );
    }
    private RunStatus GetUserInput()
    {
	    RaycastHit hit;
	    if (Input.GetMouseButton(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
	    {
			
			if (hit.transform.parent == null)
		    {
			    return RunStatus.Running;
		    }
		    if (hit.transform.parent.name.Equals("TerrainGroup_0"))
		    {
				
				blackboard["move"] = true;
			    targetPosition = hit.point; 
			} 
	    }

	    if (Input.GetKeyDown(KeyCode.T))
	    {
		     blackboard["talk"] = true;
		    
	    }
	    return RunStatus.Running;
	    }
	    #endregion

	    private Node StoryTree()
	    {
		    return new Sequence(
			    InitialStory.Get(hero),
			    middleStory.Get(hero,enemy));
	    }
	    private Node SideCharactersAffordances()
	    {
		    return new Sequence(new LeafWait( 1000),
			    new DecoratorLoop(sideCharacter.Get2(this.hero,this.sideCharacters)));
	    }


	    #region dialogues

	    
	    public static Node inquireAndRespond(GameObject player, string text)
	    {
		    return new Sequence(talk(player, text), new LeafWait(1000),
		    stopTalk(player));
	    }

	    public static Node talk(GameObject player, string text)
	    {
		    var p = Val.V(() => player);
		    return new Sequence( 
			    p.Value.GetComponent<playerScript>().dialogBox.GetComponent<dialogBoxScript>().Node_show(true),
			    new LeafInvoke(() =>
				    p.Value.GetComponent<playerScript>().dialogBox.GetComponent<dialogBoxScript>()
					    .GetComponentInChildren<Text>().text = text));
	    }



	    public static Node stopTalk(GameObject player)
	    {

		    var p = Val.V(() => player);
		    return new Sequence( new LeafInvoke(()=>print("talk3")),
			    p.Value.GetComponent<playerScript>().dialogBox.GetComponent<dialogBoxScript>().Node_show(false),
			    new LeafInvoke(() =>
				    p.Value.GetComponent<playerScript>().dialogBox.GetComponent<dialogBoxScript>()
					    .GetComponentInChildren<Text>().text = ""));
	    }
	    

	    #endregion
}



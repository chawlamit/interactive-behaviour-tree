using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK.Demos;
using UnityEngine;
using TreeSharpPlus;


public class StoryIBT : MonoBehaviour
{
	public GameObject hero;
	public GameObject enemy;
	public GameObject sideCharactersParent;

	public static Dictionary<String, bool> blackboard = new Dictionary<String, bool>();
	private static Vector3 targetPosition;

	private BehaviorAgent behaviorAgent;
	private List<GameObject> sideCharacters = new List<GameObject>();
	
    // Start is called before the first frame update
    void Start()
    {
	    behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
	    BehaviorManager.Instance.Register (behaviorAgent);
	    behaviorAgent.StartBehavior ();
	    
	    // initalize all character in the game
	    for (int i = 0; i < sideCharactersParent.transform.childCount; i++)
	    {
		    sideCharacters.Add(sideCharactersParent.transform.GetChild(i).gameObject);
	    }
	    
	    // initalizing blackborad
	    blackboard.Add("move", false); //mouse left click
	    blackboard.Add("talk", false); // right mouse click to talk
		
    }
	
    protected Node BuildTreeRoot()
    {
	    Node ibt = new SequenceParallel(
		    StoryTree(), 
			UserInput()
		    // sideCharactersAffordances()
		    );
	    return ibt;
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
		    if (hit.transform.parent.name.Equals("TerrainGroup_0"))
		    {
			    blackboard["move"] = true;
			    targetPosition = hit.transform.position;
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
			StartStoryArc.Get());
	}
}



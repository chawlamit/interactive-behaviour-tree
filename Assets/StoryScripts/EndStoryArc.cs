using RootMotion.FinalIK;
using UnityEngine;
using TreeSharpPlus;

public class EndStoryArc
{
    private static GameObject hero;
    private static GameObject enemy;
    
    
    public static Node Get(GameObject heroObj, GameObject enemyObj, GameObject apartment_door)
    {
        hero = heroObj;
        enemy = enemyObj;

        // door
        
        return new Sequence(GotoEnemyHouse(apartment_door.transform.position),
            new LeafWait(1000),
            OpenDoorIkAnim(apartment_door: apartment_door)
        );
    }
    
    private static Node GotoEnemyHouse(Vector3 enemyDoor)
    {
        // enemyDoor.y = 0;
        enemyDoor += new Vector3(30f, 0f, -50f); // stand 2m away from the door
        Val<Vector3> pos = Val.V(() => enemyDoor);
        return hero.GetComponent<BehaviorMecanim>().Node_GoTo(pos);
    }
    
    private static Node OpenDoorIkAnim(GameObject apartment_door)
    {
        Animation doorAnim = apartment_door.GetComponent<Animation>();
        
        Val<InteractionObject> doorHandle = Val.V(() => apartment_door.transform.GetChild(3).GetChild(0).GetComponent<InteractionObject>());
        return new Sequence(
            hero.GetComponent<BehaviorMecanim>().Node_StartInteraction(FullBodyBipedEffector.RightHand, doorHandle),
            new LeafWait(1000),
            new SequenceParallel(
            new LeafInvoke(() => PlayOpenDoorAnim(doorAnim)),
                hero.GetComponent<BehaviorMecanim>().Node_StopInteraction(FullBodyBipedEffector.RightHand)   
            )
        );
        
    }

    private static RunStatus PlayOpenDoorAnim(Animation doorAnim)
    {
        doorAnim.Play("Door_Open");
        return RunStatus.Success;
    }
}
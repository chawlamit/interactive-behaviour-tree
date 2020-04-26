using UnityEngine;
using TreeSharpPlus;

public class EndStoryArc
{
    private static GameObject hero;
    private static GameObject enemy;
    private static GameObject apartment_door;
    
    
    public static Node Get(GameObject heroObj, GameObject enemyObj, GameObject standHere)
    {
        hero = heroObj;
        enemy = enemyObj;

        // door
        apartment_door = standHere.transform.parent.gameObject; 
        Animation doorAnim = apartment_door.GetComponent<Animation>();
        
        return new Sequence(GotoEnemyHouse(standHere.transform.position),
            new LeafWait(1000),
            OpenDoorIKAnim(doorAnim)
        );
    }
    
    private static Node GotoEnemyHouse(Vector3 enemyDoor)
    {
        enemyDoor.y = 0;
        Val<Vector3> pos = Val.V(() => enemyDoor);
        return hero.GetComponent<BehaviorMecanim>().Node_GoTo(pos);
    }
    
    private static Node OpenDoorIKAnim(Animation doorAnim)
    {
        return null;
    }
}
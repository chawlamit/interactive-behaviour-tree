using RootMotion.FinalIK;
using TreeSharpPlus;
using UnityEngine;

namespace StoryScripts
{
    public class EndStoryArc
    {
        private static GameObject hero;
        private static GameObject enemy;
        private static GameObject gun;
    
    
        public static Node Get(GameObject heroObj, GameObject enemyObj, GameObject apartment_door, GameObject gunObj)
        {
            hero = heroObj;
            enemy = enemyObj;
            gun = gunObj;

            // door
        
            return new Sequence(
                // GotoEnemyHouse(apartment_door.transform.position),
                new LeafWait(2000),
                // OpenDoorIkAnim(apartment_door: apartment_door),
                // new LeafWait(5000),
                TakeOutGun()
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
            Val<FullBodyBipedEffector> leftHand = Val.V(() => FullBodyBipedEffector.LeftHand);
            return new Sequence(
                hero.GetComponent<BehaviorMecanim>().Node_StartInteraction(leftHand, doorHandle),
                new LeafWait(1000),
                new SequenceParallel(
                    new LeafInvoke(() => PlayOpenDoorAnim(doorAnim)),
                    hero.GetComponent<BehaviorMecanim>().Node_StopInteraction(leftHand)   
                )
            );
        
        }

        private static RunStatus PlayOpenDoorAnim(Animation doorAnim)
        {
            doorAnim.Play("Door_Open");
            return RunStatus.Success;
        }

        private static Node TakeOutGun()
        {
            Val<FullBodyBipedEffector> rightHand = Val.V(() => FullBodyBipedEffector.RightHand);
            Val<InteractionObject> gunIK = Val.V(() => gun.GetComponent<InteractionObject>());

            return new Sequence(
                hero.GetComponent<BehaviorMecanim>().Node_StartInteraction(rightHand, gunIK),
                new LeafWait(500),
                hero.GetComponent<BehaviorMecanim>().Node_StopInteraction(rightHand),
                new LeafInvoke(() => PickUpGun()),
                hero.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Pistolaim", 10000)
            );
        }

        private static RunStatus PickUpGun()
        {
            gun.transform.localPosition = new Vector3(-0.1484f, 0.0336f, 0f);
            gun.transform.localRotation = Quaternion.Euler(0, 180, 0);

            return RunStatus.Success;
        }

        private static RunStatus PutGunBack()
        {
            gun.transform.parent = hero.transform;
            gun.transform.localPosition = new Vector3(0.216f, 1.065f, -0.05100011f);
            gun.transform.localRotation = Quaternion.Euler(4.736f, 4.736f, -96.55801f);

            return RunStatus.Success;
        }
    }
}


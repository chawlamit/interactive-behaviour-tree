using RootMotion.FinalIK;
using RootMotion.FinalIK.Demos;
using TreeSharpPlus;
using UnityEngine;

namespace StoryScripts
{
    public class EndStoryArc
    {
        private static GameObject hero;
        private static GameObject enemy;
    
    
        public static Node Get(GameObject heroObj, GameObject enemyObj, GameObject apartment_door)
        {
            hero = heroObj;
            enemy = enemyObj;
            
            // door
            return new Sequence(
                GotoEnemyHouse(apartment_door.transform.position),
                new LeafWait(500),
                TakeOutGun(hero),
                new LeafWait(200),
                OpenDoorIkAnim(apartment_door: apartment_door),
                new LeafWait(500),
                new
                    SelectorParallel( //go inside the house with piston holding up and stop whenever either of them succeeds
                        hero.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Pistolaim", 10000),
                        GoInsideHouse(apartment_door)
                    ),
                hero.GetComponent<BehaviorMecanim>().ST_TurnToFace(enemy.transform.position),
                new LeafWait(400),
                enemy.GetComponent<BehaviorMecanim>().ST_TurnToFace(hero.transform.position),
                new LeafWait(400),
                // focus camera on the enemy
                // Random selection, who kills whom
                new RandomSelector(
                    HeroKillsEnemy(),
                    EnemyKillsHero()
                ));
        }
      
        #region enemyHouse
        private static Node GotoEnemyHouse(Vector3 enemyDoor)
        {
            // enemyDoor.y = 0;
            enemyDoor += new Vector3(30f, 0f, -30f); // stand 2m away from the door
            Val<Vector3> pos = Val.V(() => enemyDoor);
            return hero.GetComponent<BehaviorMecanim>().Node_GoTo(pos);
        }
    
        private static Node OpenDoorIkAnim(GameObject apartment_door)
        {
            Animation doorAnim = apartment_door.GetComponent<Animation>();
            Transform handle = apartment_door.transform.GetChild(3).GetChild(0);
            
            // Val<InteractionObject> doorHandle = Val.V(() => handle.GetComponent<InteractionObject>());
            // Val<FullBodyBipedEffector> leftHand = Val.V(() => FullBodyBipedEffector.LeftHand);
            
            return new Sequence(
                hero.GetComponent<BehaviorMecanim>().ST_TurnToFace(new Val<Vector3>(handle.position)),
                new LeafWait(1000),
                hero.GetComponent<BehaviorMecanim>().Node_StartInteraction(FullBodyBipedEffector.LeftHand, handle.GetComponent<InteractionObject>()),
                new LeafWait(800),
                new SequenceParallel(
                    hero.GetComponent<BehaviorMecanim>().Node_StopInteraction(FullBodyBipedEffector.LeftHand),
                    new LeafInvoke(() => PlayOpenDoorAnim(doorAnim))
                )
            );
        
        }

        private static RunStatus PlayOpenDoorAnim(Animation doorAnim)
        {
            doorAnim.Play("Door_Open");
            return RunStatus.Success;
        }

        private static Node GoInsideHouse(GameObject apartment_door)
        {
            var go_to = apartment_door.transform.position;
            go_to += new Vector3(60f, -go_to.y, 100);
            Val<Vector3> pos = Val.V(() => go_to);
            return hero.GetComponent<BehaviorMecanim>().Node_GoTo(pos);
        }
        #endregion

        #region Gun
        private static Node TakeOutGun(GameObject participant)
        {
            InteractionObject gun = participant.GetComponentInChildren<InteractionObject>();
            
            return new Sequence(
                participant.GetComponent<BehaviorMecanim>().Node_StartInteraction(FullBodyBipedEffector.RightHand, gun),
                new LeafWait(500),
                new LeafInvoke(() => PickUpGun(gun.gameObject)),
                participant.GetComponent<BehaviorMecanim>().Node_StopInteraction(FullBodyBipedEffector.RightHand)
            );
        }

        private static RunStatus PickUpGun(GameObject gun)
        {
            gun.transform.localPosition = new Vector3(-0.1484f, 0.0336f, 0f);
            gun.transform.localRotation = Quaternion.Euler(0, 180, 0);

            return RunStatus.Success;
        }

        private static RunStatus PutGunBack(GameObject gun)
        {
            gun.transform.parent = hero.transform;
            gun.transform.localPosition = new Vector3(0.216f, 1.065f, -0.05100011f);
            gun.transform.localRotation = Quaternion.Euler(4.736f, 4.736f, -96.55801f);

            return RunStatus.Success;
        }

        private static RunStatus Shoot(GameObject gun)
        {
            
            return RunStatus.Success;
        }
        #endregion

        #region kill

        private static Node HeroKillsEnemy()
        {
            return new Sequence(
                new LeafInvoke(() => Debug.Log("Hero Wins")),
                new SequenceParallel(
                        hero.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Pistolaim", 6000),
                                        enemy.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("HANDSUP", 3000)
                    ),
                
                // hero says - f**k you
                enemy.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("DYING", 20000)
                );
        }
        
        private static Node EnemyKillsHero()
        {
            return new Sequence(
                new LeafInvoke(() => Debug.Log("Hero Wins")),

                TakeOutGun(enemy),
                new SequenceParallel(
                enemy.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Pistolaim", 3000),
                    // enemy says - too slow, f**k you
                    hero.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("DYING", 10000)
                )
                // sad hero cries and dies
            );

        }

        
        #endregion
    }
}


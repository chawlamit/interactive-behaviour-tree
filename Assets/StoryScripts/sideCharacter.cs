using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;
using Random = UnityEngine.Random;

public class sideCharacter : MonoBehaviour
{
    private static string[] affordances = new string[5]{"Think","MouthWipe","SatNightFever","Texting","ChestPumpSalute"};
    private static System.Random random = new System.Random();

    public static Node Get(GameObject hero, List<GameObject> sideCharacters)
    {
        SequenceParallel sp = new SequenceParallel();
        foreach (var character in sideCharacters)
        {
            Val<Vector3> position = Val.V (() => hero.transform.position);
            Node interaction =
                new Selector(
                    new Sequence(new LeafAssert(() => character.CompareTag("Group") == true),
                        new Selector(
                            
                            // The Sequence when hero is nearby : Currently just look at the hero
                            new Sequence(new LeafAssert(() => StoryIBT
                                                                  .blackboardTrigger[
                                                                      character.transform.GetChild(0).gameObject] ==
                                                              true),
                                character.transform.GetChild(0).gameObject.GetComponent<BehaviorMecanim>()
                                    .ST_TurnToFace(Val.V(() => (hero.transform.position))),
                                character.transform.GetChild(1).gameObject.GetComponent<BehaviorMecanim>()
                                    .ST_TurnToFace(Val.V(() => (hero.transform.position))),
                                hero.GetComponent<BehaviorMecanim>().ST_TurnToFace(Val.V(() =>
                                    character.transform.GetChild(0).transform.position)),
                                new LeafInvoke(()=>print("Group Trigger"))
                            ),
                            
                            // The sequence when hero not nearby : Currently do Animations one after another
                            new Sequence(
                            new SequenceParallel(
                                character.transform.GetChild(0).gameObject.GetComponent<BehaviorMecanim>()
                                    .ST_TurnToFace(Val.V(() =>
                                        (character.transform.GetChild(1).gameObject.transform.position))),
                                character.transform.GetChild(1).gameObject.GetComponent<BehaviorMecanim>()
                                    .ST_TurnToFace(Val.V(() =>
                                        (character.transform.GetChild(0).gameObject.transform.position)))),
                        
                            new SequenceParallel(
                                new Sequence(character.transform.GetChild(0).gameObject
                                        .GetComponent<BehaviorMecanim>()
                                        .ST_PlayHandGesture("cheer", 2500),
                                    character.transform.GetChild(0).gameObject.GetComponent<BehaviorMecanim>()
                                        .ST_PlayHandGesture("Wonderful", 2500)),
                                new Sequence(character.transform.GetChild(1).gameObject
                                        .GetComponent<BehaviorMecanim>()
                                        .ST_PlayHandGesture("Wonderful", 2500),
                                    character.transform.GetChild(1).gameObject.GetComponent<BehaviorMecanim>()
                                        .ST_PlayHandGesture("cheer", 2500))
                            )
                        )
                        )
                        )
                    
                        // Single Person Events - currently not working
                    
                        // new Sequence(new LeafAssert(()=>character.CompareTag("Untagged")),
                        //         new Selector(new Sequence(
                        //     new LeafAssert(()=>StoryIBT.blackboardTrigger[character] == true),
                        // character.GetComponent<BehaviorMecanim>()
                        //     .ST_TurnToFace(Val.V(() => (hero.transform.position)))
                        // ),
                        // new Sequence(character.GetComponent<BehaviorMecanim>()
                        //     .ST_PlayHandGesture(Val.V(() => affordances[random.Next(5)]), 5000))
                        // )
                    // )
                    );


        // Code converted above
           
        // Node interaction;    
        // if (character.CompareTag("Group"))
        // {
        //     var child1 = character.transform.GetChild(0).gameObject;
        //     var child2 = character.transform.GetChild(1).gameObject;
        //     Val<Vector3> child1Position = Val.V(() => (child1.transform.position));
        //     Val<Vector3> child2Position = Val.V(() => (child2.transform.position));
        //     
        //     if (StoryIBT.blackboardTrigger[child1] || StoryIBT.blackboardTrigger[child2] )
        //     {
        //         interaction = new SequenceParallel(child1.GetComponent<BehaviorMecanim>()
        //             .Node_GoTo(Val
        //                 .V(() => (hero.transform.position - new Vector3(1.0f,0,1.5f)))), 
        //             child2.GetComponent<BehaviorMecanim>()
        //                 .Node_GoTo(Val
        //                     .V(() => (hero.transform.position - new Vector3(2f,0,1.5f))))
        //             );
        //     }
        //     else
        //     {
        //         interaction = new Sequence(
        //             new SequenceParallel(
        //                 child1.GetComponent<BehaviorMecanim>().ST_TurnToFace(child2Position),
        //                 child2.GetComponent<BehaviorMecanim>().ST_TurnToFace(child1Position)),
        //             new SequenceParallel(
        //                 new Sequence(child1.GetComponent<BehaviorMecanim>()
        //                         .ST_PlayHandGesture("cheer", 2500),
        //                     child1.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wonderful", 2500)),
        //                 new Sequence(child2.GetComponent<BehaviorMecanim>()
        //                         .ST_PlayHandGesture("Wonderful", 2500),
        //                     child2.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("cheer", 2500))
        //             )
        //         );
        //     }
        //
        // }
        // else
        // {
        //     if (StoryIBT.blackboardTrigger[character])
        //     {
        //         interaction = character.GetComponent<BehaviorMecanim>()
        //             .Node_GoTo(Val.V(() => (hero.transform.position - new Vector3(1.5f,0,1.5f))));
        //     }
        //     else
        //     {
        //         var next = Val.V(() => affordances[random.Next(5)]);
        //         interaction = character.GetComponent<BehaviorMecanim>()
        //             .ST_PlayHandGesture(next, 5000);
        //     }
        // }
            // Node interaction = new LeafInvoke(()=>print("test"));
            sp.Children.Add(interaction);
        }
        return new DecoratorLoop(sp);
    }   
}

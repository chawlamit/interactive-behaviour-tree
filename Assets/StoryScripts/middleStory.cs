using UnityEngine;
using TreeSharpPlus;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class middleStory:MonoBehaviour
{
    private static string[] Askmessages = new[] {"Do you know where the devil is ?", "Am Looking for Devil, Do you know where he is ?", "Have an idea where devil is ?"};
    private static string[] Replymessages = new[] {"Go Towards the Lake", "Near the Mountains", "Brown Door Gate"};
    public static Node Get(GameObject hero, GameObject enemy)
    {
        return (new DecoratorLoop(new Sequence(
            checkCount(), interactWithMob(hero,enemy))));
    }

    protected static Node checkCount()
    {
        return new LeafAssert(() => StoryIBT.clue_Count < 1);
    }


    protected static Node interactWithMob(GameObject hero,GameObject enemy)
    {
        return new Selector(
            // outer if
            new Sequence(new LeafAssert(() => StoryIBT.blackboard["talk"]),
                                new Sequence(hero.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 800),
                                        new LeafWait(1000), StoryIBT.inquireAndRespond(hero, Askmessages[(new System.Random()).Next(3)]),
                                        new LeafWait(1000),
                                        TalkToChar(hero,enemy),
                                        new LeafWait(1000),
                                        hero.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CHESTPUMPSALUTE", 500))

                            ),
            // outer else
            new Sequence(new LeafInvoke(()=>print("middle")),
                new LeafInvoke(() => RunStatus.Success))

        );



    }

    static Node TalkToChar(GameObject hero,GameObject enemy)
    {
        Selector sp = new Selector();
        foreach (var entry in StoryIBT.blackboardTrigger)
        {
            sp.Children.Add(
                new Selector(
                    new Sequence(new LeafAssert(()=>StoryIBT.ComeBack(entry.Key) && StoryIBT.CheckTrigger(entry.Key)),
                        StoryIBT.inquireAndRespond(entry.Key, "I have already answered!"),
                        new LeafInvoke(()=>StoryIBT.blackboard["talk"] = false)
                    ),
                    
                    
                    
                    new Sequence(new LeafAssert(() => StoryIBT.CheckTrigger(entry.Key)),
                entry.Key.GetComponent<BehaviorMecanim>().Node_OrientTowards(enemy.transform.position),
                entry.Key.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("POINTING",5000),
                StoryIBT.inquireAndRespond(entry.Key, Replymessages[(new System.Random()).Next(3)]),
                entry.Key.GetComponent<BehaviorMecanim>().Node_OrientTowards(hero.transform.position),
                new LeafInvoke(()=>StoryIBT.blackboard["talk"] = false),
                new LeafInvoke(()=>StoryIBT.alreadytalked[entry.Key]=true),
                new LeafInvoke(()=>StoryIBT.clue_Count += 1)
                ))
            );
        }

        return sp;
    }
    
    
    

}

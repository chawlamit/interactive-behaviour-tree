using UnityEngine;
using TreeSharpPlus;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class middleStory:MonoBehaviour
{
    private static string[] greeting = new[] { "Hello Good Sir!! What a fine day it is.", "Hey my man!! How you doing today?", "Hola Amigo!!", "Do you mind if I ask you a question?"};
    private static string[] Askmessages = new[] {"I want Johnny, I know he's hidding somewhere here, where is he?",
        "Where can I find my arch nemesis, Johnny Lawrence?", "Help me find Johnny, and I will be forever in your debt?"};
    private static string[] Replymessages = new[] {"I think, he lives somewhere near the mountains",
        "I think, he lives somewhere by the lake",
        "Just follow this mudded path, it'll take you to him"};
    public static Node Get(GameObject hero, GameObject enemy)
    {
        return new DecoratorInvert(new DecoratorLoop(new Sequence(
            checkCount(), interactWithMob(hero,enemy))));
    }

    protected static Node checkCount()
    {
        return new LeafAssert(() => StoryIBT.clue_Count < 3);
    }


    protected static Node interactWithMob(GameObject hero,GameObject enemy)
    {
        return new Selector(
            // outer if
            new Sequence(new LeafAssert(() => StoryIBT.blackboard["talk"]),
                                new Sequence(hero.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 800),
                                        new LeafWait(1000), StoryIBT.inquireAndRespond(hero ,greeting[(new System.Random()).Next(3)]),
                                        new LeafWait(1000), StoryIBT.inquireAndRespond(hero, "I am the Hero of this area. My name is Daniel San. Can I trouble you for some info?"),
                                        new LeafWait(1000), StoryIBT.inquireAndRespond(hero, Askmessages[(new System.Random()).Next(3)]),
                                        new LeafWait(1000),
                                        TalkToChar(hero,enemy),
                                        new LeafWait(1000),
                                        hero.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CHESTPUMPSALUTE", 500))

                            ),
            // outer else
            new LeafInvoke(() => RunStatus.Success)

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
                        StoryIBT.inquireAndRespond(entry.Key, "Believe me, I am telling you the truth, that's all the information I had - second response"),
                        new LeafInvoke(()=>StoryIBT.blackboard["talk"] = false)
                    ),
                    new Sequence(new LeafAssert(() => StoryIBT.CheckTrigger(entry.Key)),
                        entry.Key.GetComponent<BehaviorMecanim>().Node_OrientTowards(enemy.transform.position),
                        entry.Key.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("POINTING",2500),
                        StoryIBT.inquireAndRespond(entry.Key, "Hmm, I've heard of such a man.... (common)"),
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

using UnityEngine;
using UnityEditor;
using TreeSharpPlus;
using UnityEngine.UI;


public class middleStory
{

    public Node middlePart(GameObject hero)
    {
        return new DecoratorLoop(new Sequence(checkCount(), interactWithMob(hero)));
    }

    protected Node checkCount()
    {
        return new LeafInvoke(() => clueCount());
    }

    protected RunStatus clueCount()
    {
        if (StoryIBT.clue_Count < 3)
        {
            return RunStatus.Success;
        }
        else
        {
            return RunStatus.Failure;
        }
    }

    protected Node interactWithMob(GameObject hero)
    {
        playerScript heroPS = hero.GetComponent<playerScript>();
        
        if ((StoryIBT.blackboard["talk"] == true) && heroPS.people.Count != 0)
        {
            var speaker2 = heroPS.people[0];
            System.Random r = new System.Random();
           
            if (r.NextDouble() <= 0.5)
            {
                if (StoryIBT.clue_Count == 0)
                {
                   return new Sequence(hero.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 300),
                    new LeafWait(1000), inquireAndRespond(hero, "Do you Know where Devil Lives?"), new LeafWait(1000),
                    speaker2.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("POINTING", 200), inquireAndRespond(speaker2, "Go Towards the Lake"),
                    new LeafWait(1000), hero.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CHESTPUMPSALUTE", 300));
                }

                if (StoryIBT.clue_Count == 1)
                {
                    return new Sequence(hero.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 300),
                    new LeafWait(1000), inquireAndRespond(hero, "I am Looking for Devil. Which Way?"), new LeafWait(1000),
                    speaker2.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("POINTING", 200), inquireAndRespond(speaker2, "Near the Mountains"),
                    new LeafWait(1000), hero.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CHESTPUMPSALUTE", 300));
                }

                if (StoryIBT.clue_Count == 2)
                {
                    return new Sequence(hero.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 300),
                    new LeafWait(1000), inquireAndRespond(hero, "I am Looking for Devil. Which Way?"), new LeafWait(1000),
                    speaker2.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("POINTING", 200), inquireAndRespond(speaker2, "Brown Door Gate"),
                    new LeafWait(1000), hero.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CHESTPUMPSALUTE", 300));
                }
            }
            else
            {
               return new Sequence(hero.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("Wave", 300),
                    new LeafWait(1000), inquireAndRespond(hero, "Do you Know where Devil Lives?"), new LeafWait(1000),
                    speaker2.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("BEINGCOCKY", 200), inquireAndRespond(speaker2, "Get Away"));
            }
        }
        return new LeafInvoke(()=>RunStatus.Success);

    }


    protected Node inquireAndRespond(GameObject player, string text)
    {       
       return new Sequence(new LeafInvoke(() => talk(player, text)),new LeafWait(2000),new LeafInvoke(()=>stopTalk(player)));             
    }

    protected RunStatus talk(GameObject player, string text)
    {
        playerScript heroPS = player.GetComponent<playerScript>();
        var heroDialogBox = heroPS.dialogBox;
        dialogBoxScript hdb = heroDialogBox.GetComponent<dialogBoxScript>();
        hdb.show(true);
        Text t= heroDialogBox.GetComponentInChildren<Text>();
        t.text = text;           
            
        return RunStatus.Success;
    }

    protected static RunStatus stopTalk(GameObject player)
    {
        playerScript ps = player.GetComponent<playerScript>();
        var dialogBox = ps.dialogBox;
        dialogBoxScript dial = dialogBox.GetComponent<dialogBoxScript>();
        dial.show(false);
        Text t = dialogBox.GetComponentInChildren<Text>();
        t.text = "";

        return RunStatus.Success;

    }

}

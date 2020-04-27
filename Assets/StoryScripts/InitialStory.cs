using UnityEngine;
using UnityEditor;
using TreeSharpPlus;
using UnityEngine.UI;

public class InitialStory
{
    public static Node Get(GameObject hero)
    {
        return new Sequence(new LeafWait(1000), selfTalk(hero, "I have come so far!!"), new LeafWait(3000), 
            hideBoxTalk(hero), selfTalk(hero, "This time I'll definitely catch him and make him pay for his mistakes")
            ,new LeafWait(3000), hideBoxTalk(hero),
            selfTalk(hero, "Let me ask around.... and figure out where exactly is he hiding")
            ,new LeafWait(3000), hideBoxTalk(hero));

    }

    protected static Node selfTalk(GameObject hero, string text)
    {
        return new LeafInvoke(() => talk(hero, text));
    }

    protected static RunStatus talk(GameObject hero, string text)
    {
        playerScript ps = hero.GetComponent<playerScript>();
        var dialogBox = ps.dialogBox;
        dialogBoxScript dial = dialogBox.GetComponent<dialogBoxScript>();
        dial.show(true);
        Debug.Log("Talk");
        Text t = dialogBox.GetComponentInChildren<Text>();
        t.text = text;
        //dial.show(false);
        return RunStatus.Success;

    }
    protected static Node hideBoxTalk(GameObject hero)
    {
        return new LeafInvoke(() => hideBox(hero));
    }



    protected static RunStatus hideBox(GameObject hero)
    {
        playerScript ps = hero.GetComponent<playerScript>();
        var dialogBox = ps.dialogBox;
        dialogBoxScript dial = dialogBox.GetComponent<dialogBoxScript>();
        dial.show(false);
        Text t = dialogBox.GetComponentInChildren<Text>();
        t.text = "";

        return RunStatus.Success;

    }
}
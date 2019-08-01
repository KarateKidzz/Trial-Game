using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Runtime.Remoting.Messaging;
using UnityEngine.UI;

[System.Serializable]
public class DialogueTrigger
{
    public List<string> Dialogue;
    public OnTriggerEnterEvent Trigger;
}

public class DungeonChaseSequence : MonoBehaviour
{
    [Header("Targets")]
    public Transform FirstTarget;
    public Transform[] Targets;
    [Header("Dialogue")]
    public List<string> StartDialogue;
    public List<string> GuardMovedDialogue;
    public DialogueTrigger[] DialogueTriggers;
    [Header("Girl")]
    public Actor Girl;
    public ActorFollowPathController FollowPathController;
    public Transform ShootPosition;
    public Transform FinalTarget;
    readonly int shootHash = Animator.StringToHash("Shoot");
    [Header("Running Cone")]
    public Transform MovingGuard;
    public float GuardSpeed = 3;
    [Header("Mole")]
    public GameObject Mole;
    public OnTriggerEnterEvent MoleDialogueTrigger;
    public CinemachineVirtualCamera CloseShot;
    public List<string> MoleDialogue;
    [Header("Controller")]
    public BlankController PlayerBlankController;
    [Header("Flash")]
    public Image Image;


    void Start()
    {
        Invoke("DelayStart", 0.5f);

        Mole.SetActive(false);
        MoleDialogueTrigger.gameObject.SetActive(false);

        for (int i = 0; i < DialogueTriggers.Length; i++)
        {
            DialogueTriggers[i].Trigger.OnTriggerEnter += DialogueCallback;
        }
    }

    void DelayStart ()
    {
        TextDisplay.DisplayBasicTextExtra(new CrawlInformation(null, StartDialogue, new CrawlAction(EndStart, 0.5f), CrawlSpeed.OverworldDialogue, false, false, 0.5f, false), false);
    }

    void EndStart ()
    {
        Girl.AddController(FollowPathController);
        FollowPathController.target = FirstTarget;
        FollowPathController.FindPath();
        TextDisplay.StaticRemoveControl();
    }

    void DialogueCallback (OnTriggerEnterEvent onTriggerEnterEvent)
    {
        for (int i = 0; i < DialogueTriggers.Length; i++)
        {
            if (DialogueTriggers[i].Trigger == onTriggerEnterEvent)
            {
                TriggerDialogue(DialogueTriggers[i].Dialogue, i);
                return;
            }
        }
    }

    void TriggerDialogue (List<string> dialogue, int i)
    {
        TextDisplay.DisplayBasicTextExtra(new CrawlInformation(null, dialogue, new CrawlAction(() => DoAction(i), 0.5f), CrawlSpeed.OverworldDialogue, false, false, 0.5f, false), false);
    }

    void DoAction (int index)
    {
        switch (index)
        {
            case 0:
                FollowPathController.target = index < Targets.Length ? Targets[index] : null;
                FollowPathController.FindPath();
                break;

            case 1:
                FollowPathController.target = index < Targets.Length ? Targets[index] : null;
                FollowPathController.FindPath();
                StartCoroutine(GuardMove());
                break;
        }
        CloseDialogue();
    }

    void CloseDialogue ()
    {
        TextDisplay.StaticRemoveControl();
    }

    IEnumerator GuardMove ()
    {
        for (float i = 0; i < 6; i += Time.deltaTime)
        {
            Vector3 position = MovingGuard.transform.position;
            position.x = position.x + GuardSpeed * -1 * Time.deltaTime;

            MovingGuard.transform.position = position;
            yield return null;
        }
        GuardMoved();
        for (float i = 0; i < 100; i += Time.deltaTime)
        {
            Vector3 position = MovingGuard.transform.position;
            position.x = position.x + GuardSpeed * -1 * Time.deltaTime;

            MovingGuard.transform.position = position;
            yield return null;
        }

        yield return null;
    }

    void GuardMoved ()
    {
        TextDisplay.DisplayBasicTextExtra(new CrawlInformation(null, GuardMovedDialogue, new CrawlAction(GuardDialogueOver, 0.5f), CrawlSpeed.OverworldDialogue, false, false, 0.5f, false), false);
    }

    void GuardDialogueOver ()
    {
        TextDisplay.StaticRemoveControl();
        MoleDialogueTrigger.gameObject.SetActive(true);
        MoleDialogueTrigger.OnTriggerEnter += MoleTrigger;

        FollowPathController.target = FinalTarget;
        FollowPathController.FindPath();
    }

    void MoleTrigger (OnTriggerEnterEvent onTriggerEnterEvent)
    {
        Mole.SetActive(true);
        CloseShot.Priority = 20;
        PlayerReference.Player.Actor.AddController(PlayerBlankController);

        Invoke("DoMoleDialogue", 0.3f);
    }

    void DoMoleDialogue ()
    {
        TextDisplay.DisplayBasicTextExtra(new CrawlInformation(null, MoleDialogue, new CrawlAction(MoleDialogueOver, 0.3f), CrawlSpeed.OverworldDialogue, false, false, 0.5f, false), false);
    }

    void MoleDialogueOver ()
    {
        CloseShot.Priority = 5;
        TextDisplay.StaticRemoveControl();

        Girl.transform.position = new Vector3(ShootPosition.position.x, ShootPosition.position.y, Girl.transform.position.z);

        Invoke("GirlText", 2);
    }

    void GirlText ()
    {
        TextDisplay.DisplayBasicTextExtra(new CrawlInformation(null, new List<string> { ".[W:0.5].[W:0.5].[W:0.5]" }, new CrawlAction(GirlShoot, 0.5f), CrawlSpeed.OverworldDialogue, false, false, 0.5f, false), false);
    }

    void GirlShoot ()
    {
        Girl.MainAnimator.SetBool(shootHash, true);

        StartCoroutine(Flash());
    }

    IEnumerator Flash ()
    {
        yield return new WaitForSeconds(2);

        Image.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(MoleFly());

        for (float i = 0; i < 3; i += Time.deltaTime)
        {
            Image.color = new Color(0, 0, 0, i / 3);
            yield return null;
        }

        SceneHelper.LoadNextScene();

        yield return null;
    }

    IEnumerator MoleFly ()
    {
        for (float i = 0; i < 10; i += Time.deltaTime)
        {
            Vector3 position = Mole.transform.position;
            position.x = position.x + 2 * -1 * Time.deltaTime;
            position.y = position.y + 1.5f * Time.deltaTime;

            Mole.transform.Rotate(new Vector3(0, 0, Time.deltaTime * 50));

            Mole.transform.position = position;
            yield return null;
        }

        yield return null;
    }
}

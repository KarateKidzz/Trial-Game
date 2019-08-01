using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public enum EncounterState
{
    /// <summary>
    /// Do nothing. The scene starts in this state. The scene progresses by checking the starting state of the current encounter
    /// </summary>
    Start,
    /// <summary>
    /// Dialgoue spoken before the action gets control. The defense round is not played after this
    /// </summary>
    PreActionDialogue,
    /// <summary>
    /// Player is locked and listens to the enemy talk
    /// </summary>
    EnemyDialogue,
    /// <summary>
    /// Player can choose an action
    /// </summary>
    ActionSelect,
    /// <summary>
    /// The player has chosen an option that does not require text to be displayed
    /// </summary>
    OptionConfirm,
    /// <summary>
    /// The player has chosen an option that should display text to the screen. Will move to <see cref="PreDefense"/> after the text has crawled
    /// </summary>
    OptionConfirmText,
    /// <summary>
    /// If the monster has additional options for the player to choose when <see cref="OptionConfirmText"/> was called
    /// </summary>
    DefenseStartTextSpecial,
    /// <summary>
    /// Lock the player for the enemy to speak before starting the wave
    /// </summary>
    PreDefense,
    /// <summary>
    /// The player attacks the monster
    /// </summary>
    Attack,
    /// <summary>
    /// When the player's heart becomes active and the enemy attacks the player
    /// </summary>
    Defense,
    /// <summary>
    /// The player has died and everything should stop
    /// </summary>
    Death,
    /// <summary>
    /// The player has spared the enemy and wins
    /// </summary>
    Spare,
    /// <summary>
    /// The player runs away
    /// </summary>
    Flee,
    /// <summary>
    /// The player kills the enemy and wins
    /// </summary>
    KillEnemy

}

/// <summary>
/// Encounter Manager. Handles input, state management and top level tasks. Uses <see cref="FightManagerUtil"/> to do the grunt work
/// </summary>
public class FightManager : MonoBehaviour
{
    public FightManagerUtil ManagerUtil;

    public float AttackCenterWidth = 50;

    [ReadOnly]
    public EncounterState GameState = EncounterState.Start;

    public GameObject TestEncounterPrefab;

    [ReadOnly]
    public GameObject InstantiatedEncounter;

    [ReadOnly]
    public EncounterBehaviourBase CurrentFightBehaviour;

    const string CheckText = "Check";

    #region Main

    void Awake()
    {
        //
        // Encounters are prefabs. We get a reference to one, load it and then get the components we need from it
        //

        // If this is the first scene, no encounters would have been set from the overworld so use a testing prefab
        if (CurrentEncounter.CurrentEcounterBehaviour == null)
        {
            InstantiatedEncounter = Instantiate(TestEncounterPrefab, transform);
            CurrentFightBehaviour = InstantiatedEncounter.GetComponent<EncounterBehaviourBase>();
        }
        // If this is not null, another scene has loaded this scene and set the encounter we are in
        else
        {
            InstantiatedEncounter = Instantiate(CurrentEncounter.CurrentEcounterBehaviour.gameObject, transform);
            CurrentFightBehaviour = InstantiatedEncounter.GetComponent<EncounterBehaviourBase>();
        }

        ManagerUtil.SetEnemySprite(CurrentFightBehaviour.Enemies[0].Sprite);

        Time.timeScale = 1;
    }

    void Start()
    {
        // Set UI to starting state
        ManagerUtil.FullReset();

        // Set HP UI for player and enemy
        ManagerUtil.SetPlayerHealth(SerializedPlayer.GetCurrentHealth(), SerializedPlayer.GetMaxHealth());
        ManagerUtil.SetEnemyHealth(CurrentFightBehaviour.Enemies[0].HealthPoints, CurrentFightBehaviour.Enemies[0].StartHealth);

        // Delay starting the encounter for UI to update and scale properly
        Invoke("StartEncounter", 0.4f);
    }

    void StartEncounter ()
    {
        // Let the encounter set the state. This means an encounter could start with fighting straight away
        SetGameState(CurrentFightBehaviour.EncounterStart());
    }

    void Update()
    {
        // If we are in the dialogue state and no selectable is active, allow selection of the default button (FIGHT button)
        // Otherwise, we have a selection and should allow activing this button with the 'ENTER' key
        if (GameState == EncounterState.ActionSelect)
        {
            if (!ManagerUtil.HasSelection())
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    ManagerUtil.SelectDefault();
                }
            }
            else
            {
                if (Input.GetButtonDown("Submit"))
                {
                    var selection = ManagerUtil.GetSelection();
                    var button = selection.GetComponent<Button>();
                    if (button)
                    {
                        button.onClick.Invoke();
                    }
                }
            }
        }

        // If we are attacking an enemy, the bar moving across the screen can be stopped by pressing 'Z'. This attacks the enemy and deals damage
        if (GameState == EncounterState.Attack)
        {
            /*
            if (Input.GetButtonDown("Submit"))
            {
                Debug.Log("are we sure?");

                
            }
*/
        }

        // A wave only updates when we (they manager) tells it to
        // Updating the wave means projectiles are moved
        if (GameState == EncounterState.Defense)
        {
            CurrentFightBehaviour.UpdateWave();
        }
    }

    /// <summary>
    /// Main function. Sets the game state and handles all logic for the game
    /// </summary>
    /// <param name="newState">New state.</param>
    /// <param name="inventoryItem">Inventory item.</param>
    /// <param name="optionNumber">Option number.</param>
    void SetGameState(EncounterState newState, int inventoryItem = -1, int optionNumber = -1)
    {
        Debug.Assert(CurrentFightBehaviour);

        switch (newState)
        {
                // We should not be in this state. This is an empty state used to start the scene in. However, if we get here, start the encounter at the beginning
            case EncounterState.Start:
                
                EncounterState startState = CurrentFightBehaviour.EncounterStart();
                Debug.Assert(startState != EncounterState.Start);
                SetGameState(startState);

                break;

            case EncounterState.PreActionDialogue:

                ManagerUtil.FullReset();
                ManagerUtil.References.MainBox.SetBoxWidth(BoxWidth.Square, 0.5f, 0.5f, false, () => ManagerUtil.SetBubbleText(CurrentFightBehaviour.Enemies[0].PreActionDialogue, TextBubbleDirection.RightLarge, new CrawlAction(() => SetGameState(EncounterState.ActionSelect), 0)));

                break;

                // Set box width to small, show but lock the player, display text in the enemy's text bubble
            case EncounterState.EnemyDialogue:

                ManagerUtil.ClearText();
                var dialogueText = CurrentFightBehaviour.EnemyDialogueStarting();
                ManagerUtil.References.MainBox.SetBoxWidth(BoxWidth.Square, 0.5f, 0.5f, false, () => ShowTextBubble(dialogueText));

                break;

                // Allow the player to take an action
            case EncounterState.ActionSelect:
                ManagerUtil.References.MainBox.SetBoxWidth(BoxWidth.Wide, 0.5f, 0.5f, false);
                CurrentFightBehaviour.EnemyDialogueStarting();

                ManagerUtil.FullReset();
                ManagerUtil.SelectDefault();
                ManagerUtil.SetBattleText(CurrentFightBehaviour.EncounterText);
                ManagerUtil.SetSpareFleeHelp(CurrentFightBehaviour.Enemies[0].CanSpare, CurrentFightBehaviour.CanFlee);
                break;

                // Get rid of text and show the player attack object
            case EncounterState.Attack:
                ManagerUtil.ClearText();
                ManagerUtil.SetSelection(null);

                ManagerUtil.References.PlayerAttackObject.SetActive(true);
                break;

                // After the player takes their action, unselect and wait a set amount of time before going into the defense round
            case EncounterState.OptionConfirm:

                ManagerUtil.SetSelection(null);
                ManagerUtil.ClearText();

                // If 1, the player has chosen 'Check' options
                if (optionNumber == 1)
                {
                    StartCoroutine(WaitForOptionText($"{CurrentFightBehaviour.Enemies[0].EnemyName} has {CurrentFightBehaviour.Enemies[0].Attack}AT and {CurrentFightBehaviour.Enemies[0].Defense}DEF"));
                    break;
                }

                string result = CurrentFightBehaviour.Enemies[0].TriggerCommandEvent(optionNumber);
                StartCoroutine(WaitForOptionText(result));

                break;

            case EncounterState.OptionConfirmText:
                
                if (inventoryItem != -1)
                {
                    ManagerUtil.SetSelection(null);
                    ManagerUtil.ClearText();

                    result = CurrentFightBehaviour.Enemies[0].HandleItem(PlayerInventory.GetItems()[inventoryItem - 1]);
                    if (!String.IsNullOrEmpty(result))
                    {
                        // Stop the coroutine if running and start it back up

                        StartCoroutine(WaitForOptionText(result));
                    }
                    else
                        StartCoroutine(WaitForOptionText());
                }

                break;

                // Set arena size and play enemy comment/dialogue
            case EncounterState.PreDefense:
                ManagerUtil.ClearText();

                ManagerUtil.References.MainBox.SetBoxWidth(CurrentFightBehaviour.CustomWidth, CurrentFightBehaviour.CustomHeight, 0.3f, 0.2f, true, OnBoxMoveFinish);
                break;

            case EncounterState.Defense:
                ManagerUtil.ClearText();

                ManagerUtil.References.MainBox.SetBoxWidth(CurrentFightBehaviour.CustomWidth, CurrentFightBehaviour.CustomHeight, 0.3f, 0.2f, true, OnBoxMoveFinish);

                break;

            

            case EncounterState.Death:

                ClearScreenExceptPlayer();
                
                StartCoroutine(WaitForEndOfDeath());

                break;

            case EncounterState.Flee:

                ClearScreenExceptPlayer();
                ManagerUtil.References.Player.GetComponent<Rigidbody2D>().gravityScale = 3;
                SceneHelper.LoadOverworldScene();

                break;

            case EncounterState.Spare:

                ManagerUtil.ClearText();
                TextCrawl.Crawl(CrawlInformation.EncounterOnFinish(ManagerUtil.References.DefaultDialogueText, new List<string> { "You Spared The Creature" }, new CrawlAction(SceneHelper.LoadOverworldScene, 0)));
                CurrentEncounter.TriggerWin();

                break;

            case EncounterState.KillEnemy:
                ManagerUtil.ClearText();
                TextCrawl.Crawl(CrawlInformation.EncounterOnFinish(ManagerUtil.References.DefaultDialogueText, new List<string> { "You Killed The Creature And Won!" }, new CrawlAction(SceneHelper.LoadOverworldScene, 0)));
                CurrentEncounter.TriggerWin();
                CurrentEncounter.DestoryCaller();

                break;
        }
        GameState = newState;
    }

    #endregion

    void OnBoxMoveFinish ()
    {
        switch (GameState)
        {
            case EncounterState.Defense:
                ManagerUtil.EnablePlayer();

                StartCoroutine(WaitForEndOfWave());
                CurrentFightBehaviour.EnemyDialogueEnding();

                CurrentFightBehaviour.StartWave(gameObject, ManagerUtil.References.MainBox.RectTransform, ManagerUtil.References.Player);

                break;
        }
    }

    void ShowTextBubble (List<string> text)
    {
        if (text == null)
        {
            SetGameState(EncounterState.Defense);
            return;
        }

        ManagerUtil.SetBubbleText(text, CurrentFightBehaviour.Enemies[0].TextBubbleDirection, new CrawlAction(() => SetGameState(EncounterState.Defense), 0));
    }

    public void OnPlayerFinishAttack ()
    {
        float damage = ManagerUtil.StopAttack(AttackCenterWidth);
        float def = CurrentFightBehaviour.Enemies[0].Defense / 2f;;
        def = def < 1f ? 1 : def;
        int damageDone = (int)(damage / def);
        CurrentFightBehaviour.Enemies[0].HealthPoints -= damageDone;
        ManagerUtil.SetEnemyHealth(CurrentFightBehaviour.Enemies[0].HealthPoints, CurrentFightBehaviour.Enemies[0].StartHealth);


        ManagerUtil.References.AttackBar.Move = false;
        float width, x;
        ManagerUtil.References.AttackBar.GetDistances(out width, out x);
        if (CurrentFightBehaviour.Enemies[0].IsDead())
        {
            SetGameState(EncounterState.KillEnemy);
        }
        else
        {
            SetGameState(EncounterState.EnemyDialogue);
        }
    }

    public void OnPlayerReachEnd ()
    {
        ManagerUtil.References.PlayerAttackObject.SetActive(false);
        SetGameState(EncounterState.EnemyDialogue);
    }

    /// <summary>
    /// If an options that displays text is shown, this displays it before going to <see cref="EncounterState.PreDefense"/>
    /// </summary>
    /// <returns>The till defense.</returns>
    /// <param name="text">Text.</param>
    IEnumerator WaitForOptionText (string text = null)
    {
        yield return ManagerUtil.SetBattleText(text ?? CurrentFightBehaviour.EncounterText);
        SetGameState(EncounterState.EnemyDialogue);
        yield return null;
    }

    IEnumerator WaitForEndOfWave ()
    {
        yield return new WaitForSeconds(CurrentFightBehaviour.WaveTime);
        if (GameState == EncounterState.Defense)    // if we haven't died while in defense round
        {
            CurrentFightBehaviour.DefenseRoundEnding();
            CurrentFightBehaviour.EndWave();
            SetGameState(EncounterState.ActionSelect);
        }
        yield return null;
    }

    IEnumerator WaitForEndOfDeath ()
    {
        yield return new WaitForSecondsRealtime(2);
        SceneHelper.LoadGameOverScene();
        yield return null;
    }

    IEnumerator FlashPlayer ()
    {
        Image playerImage = ManagerUtil.References.Player.GetComponent<Image>();

        playerImage.color = Color.white;

        yield return new WaitForSecondsRealtime(0.1f);

        playerImage.color = Color.red;

        yield return null;
    }

    public void OnPlayerHit(Collider2D collision, EncounterAttackerBase info)
    {
        float damage = (info.Damage + CurrentFightBehaviour.Enemies[0].Attack) / (SerializedPlayer.GetDefensePoints() / 2);
        SerializedPlayer.SetCurrentHealth(SerializedPlayer.GetCurrentHealth() - (int)damage);
        if (info.DestoryOnHit)
        {
            Destroy(info.gameObject);
        }

        ManagerUtil.SetPlayerHealth(SerializedPlayer.GetCurrentHealth(), SerializedPlayer.GetMaxHealth());

        if (SerializedPlayer.GetCurrentHealth() <= 0)
        {
            SetGameState(EncounterState.Death);
        }

        StartCoroutine(FlashPlayer());
    }

    void ClearScreenExceptPlayer()
    {
        ManagerUtil.ClearScreen();
        CurrentFightBehaviour.EndWave();
    }



    #region Button Inputs

    public void FightButtonDown()
    {
        SetGameState(EncounterState.Attack);
    }

    public void ActButtonDown ()
    {
        Debug.Assert(CurrentFightBehaviour);

        if (GameState != EncounterState.ActionSelect) return;

        List<string> commands = CurrentFightBehaviour.Enemies[0].Commands;

        ManagerUtil.ClearText();

        switch (commands.Count + 1)
        {
            default:
                ManagerUtil.SetOptionTexts(CheckText);
                break;

            case 2:
                ManagerUtil.SetOptionTexts(CheckText, commands[0]);
                break;

            case 3:
                ManagerUtil.SetOptionTexts(CheckText, commands[0], commands[1]);
                break;

            case 4:
                ManagerUtil.SetOptionTexts(CheckText, commands[0], commands[1], commands[2]);
                break;
        }
    }

    public void ItemButtonDown ()
    {
        Debug.Assert(CurrentFightBehaviour);

        if (GameState != EncounterState.ActionSelect) return;

        List<BaseCollectable> items = PlayerInventory.GetItems();
        ManagerUtil.ClearText();

        switch (items.Count)
        {
            case 0:
                break;

            case 1:
                ManagerUtil.SetItemText(items[0].Name);
                break;

            case 2:
                ManagerUtil.SetItemText(items[0].Name, items[1].Name);
                break;

            case 3:
                ManagerUtil.SetItemText(items[0].Name, items[1].Name, items[2].Name);
                break;

            case 4:
                ManagerUtil.SetItemText(items[0].Name, items[1].Name, items[2].Name, items[3].Name);
                break;

            case 5:
                ManagerUtil.SetItemText(items[0].Name, items[1].Name, items[2].Name, items[3].Name, items[4].Name);
                break;
        }
    }

    public void MercyButtonDown ()
    {
        ManagerUtil.ClearText();
        ManagerUtil.References.MercyOptionsParent.SetActive(true);
        ManagerUtil.SetMercyNavigation();
    }

    public void FleeButtonDown ()
    {
        if(CurrentFightBehaviour.CanFlee)
        {
            SetGameState(EncounterState.Flee);
        }
    }

    public void SpareButtonDown ()
    {
        if (CurrentFightBehaviour.Enemies[0].CanSpare)
        {
            SetGameState(EncounterState.Spare);
        }
        else
        {
            SetGameState(EncounterState.EnemyDialogue);
        }
    }

    public void OptionButtonDown (int optionNumber)
    {
        SetGameState(EncounterState.OptionConfirm, -1, optionNumber);
    }

    public void InventoryItemCllick (int invNumber)
    {
        SetGameState(EncounterState.OptionConfirmText, invNumber);
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class FightManagerUtil : MonoBehaviour
{
    public FightingUIReference References;

    public void SetEnemySprite (Sprite sprite)
    {
        References.EnemyImage.sprite = sprite;   
    }

    #region Resets

    /// <summary>
    /// Set UI to beginning
    /// </summary>
    public void FullReset()
    {
        ResetPlayerAndDisable();
        ClearText();
        Unselect();
    }

    /// <summary>
    /// Resets and disables all player contexts (attack box, heart)
    /// </summary>
    public void ResetPlayerAndDisable()
    {
        // Center player and turn off
        References.Player.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        References.Player.gameObject.SetActive(false);

        // Disable player attack window
        References.PlayerAttackObject.SetActive(false);
    }

    // Unselect any buttons
    public void Unselect()
    {
        // Unselect buttons
        References.EventSystem.SetSelectedGameObject(null);
    }

    public void SelectDefault()
    {
        References.EventSystem.SetSelectedGameObject(References.FightButton.gameObject);
    }

    #endregion

    #region Public Sets

    public void EnablePlayer ()
    {
        References.Player.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        References.Player.gameObject.SetActive(true);
    }

    public void ClearText ()
    {
        SetBattleText();
        SetOptionTexts();
        SetItemText();
        SetBubbleText(new List<string>(), TextBubbleDirection.Left, CrawlAction.Null);
        References.MercyOptionsParent.SetActive(false);
    }

    public void ClearScreen ()
    {
        ClearText();
        References.Player.transform.parent.GetComponent<LineRenderer>().enabled = false;
        References.ButtonParent.SetActive(false);
        References.TopParent.SetActive(false);
        References.TextParent.SetActive(false);
        References.CollisionParent.SetActive(false);
        References.Player.SetActive(true);
        References.PlayerController.PlayerHasControl = false;
        var rb = References.PlayerController.GetComponent<Rigidbody2D>();
        rb.angularVelocity = 0;
        rb.velocity = Vector2.zero;
    }

    public int StopAttack (float centerWidth)
    {
        // Stop the bar from moving
        References.AttackBar.Move = false;
        References.PlayerAttackObject.SetActive(false);
                                             
        // Get width of the parent Rect and get the X position of the bar
        // X = 0 would mean the bar is in the perfect center of the parent
        References.AttackBar.GetDistances(out float width, out float x);

        // If width was 1,000, the X value would never get that high
        // X could only go to -500 and +500
        float halfWidth = centerWidth / 2;

        if (x < halfWidth && x > -halfWidth)
        {
            return SerializedPlayer.GetAttackPoints();
        }

        // make x positive for easy checking for either side of the center
        if (x < 0) x *= -1; 

        //
        // The closer the bar is to the center (0,0), the more damage it does
        // If we are at 0, the fraction should be Width/Width, not 0/Width
        //

        // Get the width from 'left' to 'center' and subtract x from it
        // giving us a value that is higher when closer to the center
        float numerator = width / 2 - x;    // 30 / 100(width)  --->>>> 70/100
        // Denominator is just width from 'left' to 'center'
        float denominator = width / 2;

        // Get the fraction over 1 to multiply later
        numerator /= denominator;
        denominator /= denominator;

        // Get a fraction that will give max attack power when equal to 1/1
        numerator *= SerializedPlayer.GetAttackPoints();
        denominator *= SerializedPlayer.GetAttackPoints();

        // HP is an int
        return (int)numerator;
    }

    public void SetSpareFleeHelp (bool canSpare, bool canFlee)
    {
        References.PlayerNameText.gameObject.SetActive(canSpare);
        References.PlayerLevelText.gameObject.SetActive(canFlee);
    }

    public Coroutine SetBubbleText (List<string> text, TextBubbleDirection direction, CrawlAction returnAction)
    {
        Coroutine ret = null;

        References.Left.transform.parent.gameObject.SetActive(false);
        References.Right.transform.parent.gameObject.SetActive(false);
        References.LeftShort.transform.parent.gameObject.SetActive(false);
        References.LeftWide.transform.parent.gameObject.SetActive(false);
        References.RightLarge.transform.parent.gameObject.SetActive(false);
        //References.RightLong.transform.parent.gameObject.SetActive(false);
        References.RightShort.transform.parent.gameObject.SetActive(false);
        References.RightWide.transform.parent.gameObject.SetActive(false);
        References.Top.transform.parent.gameObject.SetActive(false);
        References.TopTiny.transform.parent.gameObject.SetActive(false);
        References.Bottom.transform.parent.gameObject.SetActive(false);

        if (text.Count == 0 || text[0] == "")
        {
            switch (direction)
            {
                case TextBubbleDirection.Left:
                    TextCrawl.CancelCrawl(References.Left);
                    break;

                case TextBubbleDirection.Right:
                    TextCrawl.CancelCrawl(References.Right);
                    break;
                case TextBubbleDirection.LeftShort:
                    TextCrawl.CancelCrawl(References.LeftShort);
                    break;
                case TextBubbleDirection.LeftWide:
                    TextCrawl.CancelCrawl(References.LeftWide);
                    break;
                case TextBubbleDirection.RightLarge:
                    TextCrawl.CancelCrawl(References.RightLarge);
                    break;
                case TextBubbleDirection.RightLong:
                    TextCrawl.CancelCrawl(References.RightLong);
                    break;
                case TextBubbleDirection.RightShort:
                    TextCrawl.CancelCrawl(References.RightShort);
                    break;
                case TextBubbleDirection.RightWide:
                    TextCrawl.CancelCrawl(References.RightWide);
                    break;
                case TextBubbleDirection.Top:
                    TextCrawl.CancelCrawl(References.Top);
                    break;
                case TextBubbleDirection.TopTiny:
                    TextCrawl.CancelCrawl(References.TopTiny);
                    break;
                case TextBubbleDirection.Bottom:
                    TextCrawl.CancelCrawl(References.Bottom);
                    break;
            }
        }
        else
        {
            switch (direction)
            {
                case TextBubbleDirection.Left:
                    References.Left.transform.parent.gameObject.SetActive(true);
                    ret = TextCrawl.Crawl(CrawlInformation.EncounterOnFinish(References.Left, text, returnAction));
                    break;

                case TextBubbleDirection.Right:
                    Debug.Log("Riught");
                    References.Right.transform.parent.gameObject.SetActive(true);
                    ret = TextCrawl.Crawl(CrawlInformation.EncounterOnFinish(References.Right, text, returnAction));
                    break;
                case TextBubbleDirection.LeftShort:
                    References.LeftShort.transform.parent.gameObject.SetActive(true);
                    ret = TextCrawl.Crawl(CrawlInformation.EncounterOnFinish(References.LeftShort, text, returnAction));
                    break;
                case TextBubbleDirection.LeftWide:
                    References.LeftWide.transform.parent.gameObject.SetActive(true);
                    ret = TextCrawl.Crawl(CrawlInformation.EncounterOnFinish(References.LeftWide, text, returnAction));
                    break;
                case TextBubbleDirection.RightLarge:
                    References.RightLarge.transform.parent.gameObject.SetActive(true);
                    ret = TextCrawl.Crawl(CrawlInformation.EncounterOnFinish(References.RightLarge, text, returnAction));
                    break;
                case TextBubbleDirection.RightLong:
                    References.RightLong.transform.parent.gameObject.SetActive(true);
                    ret = TextCrawl.Crawl(CrawlInformation.EncounterOnFinish(References.RightLong, text, returnAction));
                    break;
                case TextBubbleDirection.RightShort:
                    References.RightShort.transform.parent.gameObject.SetActive(true);
                    ret = TextCrawl.Crawl(CrawlInformation.EncounterOnFinish(References.RightShort, text, returnAction));
                    break;
                case TextBubbleDirection.RightWide:
                    References.RightWide.transform.parent.gameObject.SetActive(true);
                    ret = TextCrawl.Crawl(CrawlInformation.EncounterOnFinish(References.RightWide, text, returnAction));
                    break;
                case TextBubbleDirection.Top:
                    References.Top.transform.parent.gameObject.SetActive(true);
                    ret = TextCrawl.Crawl(CrawlInformation.EncounterOnFinish(References.Top, text, returnAction));
                    break;
                case TextBubbleDirection.TopTiny:
                    References.TopTiny.transform.parent.gameObject.SetActive(true);
                    ret = TextCrawl.Crawl(CrawlInformation.EncounterOnFinish(References.TopTiny, text, returnAction));
                    break;
                case TextBubbleDirection.Bottom:
                    References.Bottom.transform.parent.gameObject.SetActive(true);
                    ret = TextCrawl.Crawl(CrawlInformation.EncounterOnFinish(References.Bottom, text, returnAction));
                    break;
            }
        }
        return ret;
    }

    public Coroutine SetBattleText (string text = "")
    {
        if (String.IsNullOrEmpty(text))
        {
            TextCrawl.CancelCrawl(References.DefaultDialogueText);
        }
        else
        {
            return TextCrawl.Crawl(CrawlInformation.DefaultEncounter(References.DefaultDialogueText, new List<string> { text }, "event:/TextHit"));
        }
        return null;
    }

    public void SetOptionTexts(string option1 = null, string option2 = null, string option3 = null, string option4 = null)
    {
        References.OptionOneText.transform.parent.gameObject.SetActive(option1 != null);

        References.OptionTwoText.transform.parent.gameObject.SetActive(option2 != null);

        References.OptionThreeText.transform.parent.gameObject.SetActive(option3 != null);

        References.OptionFourText.transform.parent.gameObject.SetActive(option4 != null);

        References.OptionOneText.text = option1;
        References.OptionTwoText.text = option2;
        References.OptionThreeText.text = option3;
        References.OptionFourText.text = option4;

        int numberOfOptions = (option1 != null ? 1 : 0) + (option2 != null ? 1 : 0) + (option3 != null ? 1 : 0) + (option4 != null ? 1 : 0);

        UpdateMainButtonNavigationToOptions(numberOfOptions);
    }

    public void SetItemText (string item1 = null, string item2 = null, string item3 = null, string item4 = null, string item5 = null)
    {
        References.Item1Text.transform.parent.gameObject.SetActive(item1 != null);

        References.Item2Text.transform.parent.gameObject.SetActive(item2 != null);

        References.Item3Text.transform.parent.gameObject.SetActive(item3 != null);

        References.Item4Text.transform.parent.gameObject.SetActive(item4 != null);

        References.Item5Text.transform.parent.gameObject.SetActive(item5 != null);

        References.Item1Text.text = item1;
        References.Item2Text.text = item2;
        References.Item3Text.text = item3;
        References.Item4Text.text = item4;
        References.Item5Text.text = item5;

        int numOfItems = (item1 != null ? 1 : 0) + (item2 != null ? 1 : 0) + (item3 != null ? 1 : 0) + (item4 != null ? 1 : 0) + (item5 != null ? 1 : 0);

        UpdateMainNavigationToItems(numOfItems);
    }

    /// <summary>
    /// Set the player health and correctly displays 'HP:' and '/' in the text
    /// </summary>
    /// <param name="currentHealth">Current health.</param>
    /// <param name="maxHealth">Max health.</param>
    public void SetPlayerHealth (int currentHealth, int maxHealth)
    {
        References.PlayerHealthText.text = "Hp " + (currentHealth < 0 ? 0 : currentHealth) + "/" + maxHealth;
    }

    /// <summary>
    /// Set the enemy health and correctly displays 'HP:' and '/' in the text
    /// </summary>
    /// <param name="currentHealth">Current health.</param>
    /// <param name="maxHealth">Max health.</param>
    public void SetEnemyHealth(int currentHealth, int maxHealth)
    {
        References.EnemyHealthText.text = "Hp " + (currentHealth < 0 ? 0 : currentHealth) + "/" + maxHealth;
    }

    public void SetSelection (GameObject objectToSet)
    {
        References.EventSystem.SetSelectedGameObject(objectToSet);
    }

    public GameObject GetSelection ()
    {
        return References.EventSystem.currentSelectedGameObject;
    }

    public bool HasSelection ()
    {
        return References.EventSystem.currentSelectedGameObject != null;
    }

    public void SetAttackUI ()
    {
        SetBattleText();
        References.PlayerAttackObject.SetActive(true);
    }

#endregion

    #region Navigation Updates

    void UpdateMainButtonNavigationToOptions(int numberOfOptions)
    {
        Debug.Assert(numberOfOptions >= 0 && numberOfOptions < 5);


        if (numberOfOptions == 0) return;
        

        Navigation fightButtonCurrent = References.FightButton.navigation;
        Navigation actButtonCurrent = References.ActButton.navigation;
        Navigation itemButtonCurrent = References.ItemButton.navigation;
        Navigation mercyButtonCurrent = References.MercyButton.navigation;

        switch (numberOfOptions)
        {
            case 1:
                fightButtonCurrent.selectOnUp = actButtonCurrent.selectOnUp = itemButtonCurrent.selectOnUp = mercyButtonCurrent.selectOnUp = References.OptionOneText.transform.parent.GetComponent<Button>();
                fightButtonCurrent.selectOnDown = actButtonCurrent.selectOnDown = itemButtonCurrent.selectOnDown = mercyButtonCurrent.selectOnDown = References.OptionOneText.transform.parent.GetComponent<Button>();
                break;

            case 2:
                fightButtonCurrent.selectOnUp = actButtonCurrent.selectOnUp = References.OptionOneText.transform.parent.GetComponent<Button>();
                fightButtonCurrent.selectOnDown = actButtonCurrent.selectOnDown = References.OptionOneText.transform.parent.GetComponent<Button>();
                itemButtonCurrent.selectOnUp = mercyButtonCurrent.selectOnUp = References.OptionTwoText.transform.parent.GetComponent<Button>();
                itemButtonCurrent.selectOnDown = mercyButtonCurrent.selectOnDown = References.OptionTwoText.transform.parent.GetComponent<Button>();
                break;

            case 3:
                fightButtonCurrent.selectOnUp = actButtonCurrent.selectOnUp = References.OptionThreeText.transform.parent.GetComponent<Button>();
                fightButtonCurrent.selectOnDown = actButtonCurrent.selectOnDown = References.OptionOneText.transform.parent.GetComponent<Button>();
                itemButtonCurrent.selectOnUp = mercyButtonCurrent.selectOnUp = References.OptionTwoText.transform.parent.GetComponent<Button>();
                itemButtonCurrent.selectOnDown = mercyButtonCurrent.selectOnDown = References.OptionTwoText.transform.parent.GetComponent<Button>();
                break;

            case 4:
                fightButtonCurrent.selectOnUp = actButtonCurrent.selectOnUp = References.OptionThreeText.transform.parent.GetComponent<Button>();
                fightButtonCurrent.selectOnDown = actButtonCurrent.selectOnDown = References.OptionOneText.transform.parent.GetComponent<Button>();
                itemButtonCurrent.selectOnUp = mercyButtonCurrent.selectOnUp = References.OptionFourText.transform.parent.GetComponent<Button>();
                itemButtonCurrent.selectOnDown = mercyButtonCurrent.selectOnDown = References.OptionTwoText.transform.parent.GetComponent<Button>();
                break;
        }

        References.FightButton.navigation = fightButtonCurrent;
        References.ActButton.navigation = actButtonCurrent;
        References.ItemButton.navigation = itemButtonCurrent;
        References.MercyButton.navigation = mercyButtonCurrent;

        UpdateOptionTextNavigatin(numberOfOptions);
    }

    void UpdateOptionTextNavigatin(int numberOfOptions)
    {
        Debug.Assert(numberOfOptions > 0 && numberOfOptions < 5);

        // Get references to buttons
        Button option1Button = References.OptionOneText.transform.parent.GetComponent<Button>();
        Button option2Button = References.OptionTwoText.transform.parent.GetComponent<Button>();
        Button option3Button = References.OptionThreeText.transform.parent.GetComponent<Button>();
        Button option4Button = References.OptionFourText.transform.parent.GetComponent<Button>();

        // Get navigation settings
        Navigation option1Current = option1Button.navigation;
        Navigation option2Current = option2Button.navigation;
        Navigation option3Current = option3Button.navigation;
        Navigation option4Current = option4Button.navigation;

        switch (numberOfOptions)
        {
            case 1:

                option1Current.selectOnDown = References.FightButton;

                break;

            case 2:

                option1Current.selectOnDown = References.FightButton;
                option2Current.selectOnDown = References.ItemButton;

                break;

            case 3:

                option1Current.selectOnDown = option2Current.selectOnDown = option3Button;
                option3Current.selectOnDown = References.FightButton;

                break;

            case 4:

                option1Current.selectOnDown = option3Button;
                option2Current.selectOnDown = option4Button;
                option3Current.selectOnDown = References.FightButton;
                option4Current.selectOnDown = References.ItemButton;

                break;
        }

        // Set navigation back after our changes
        option1Button.navigation = option1Current;
        option2Button.navigation = option2Current;
        option3Button.navigation = option3Current;
        option4Button.navigation = option4Current;
    }

    void UpdateMainNavigationToItems (int numberOfItems)
    {
        Debug.Assert(numberOfItems >= 0 && numberOfItems < 6);

        if (numberOfItems == 0) return;

        Navigation fightButtonCurrent = References.FightButton.navigation;
        Navigation actButtonCurrent = References.ActButton.navigation;
        Navigation itemButtonCurrent = References.ItemButton.navigation;
        Navigation mercyButtonCurrent = References.MercyButton.navigation;

        switch (numberOfItems)
        {
            case 1:
                fightButtonCurrent.selectOnUp = actButtonCurrent.selectOnUp = itemButtonCurrent.selectOnUp = mercyButtonCurrent.selectOnUp = References.Item1Text.transform.parent.GetComponent<Button>();
                fightButtonCurrent.selectOnDown = actButtonCurrent.selectOnDown = itemButtonCurrent.selectOnDown = mercyButtonCurrent.selectOnDown = References.Item1Text.transform.parent.GetComponent<Button>();
                break;

            case 2:
                fightButtonCurrent.selectOnUp = actButtonCurrent.selectOnUp = itemButtonCurrent.selectOnUp = mercyButtonCurrent.selectOnUp = References.Item2Text.transform.parent.GetComponent<Button>();
                fightButtonCurrent.selectOnDown = actButtonCurrent.selectOnDown = itemButtonCurrent.selectOnDown = mercyButtonCurrent.selectOnDown = References.Item1Text.transform.parent.GetComponent<Button>();
                break;

            case 3:
                fightButtonCurrent.selectOnUp = actButtonCurrent.selectOnUp = itemButtonCurrent.selectOnUp = mercyButtonCurrent.selectOnUp = References.Item3Text.transform.parent.GetComponent<Button>();
                fightButtonCurrent.selectOnDown = actButtonCurrent.selectOnDown = itemButtonCurrent.selectOnDown = mercyButtonCurrent.selectOnDown = References.Item1Text.transform.parent.GetComponent<Button>();
                break;

            case 4:
                fightButtonCurrent.selectOnUp = actButtonCurrent.selectOnUp = itemButtonCurrent.selectOnUp = mercyButtonCurrent.selectOnUp = References.Item3Text.transform.parent.GetComponent<Button>();
                fightButtonCurrent.selectOnDown = actButtonCurrent.selectOnDown = itemButtonCurrent.selectOnDown = mercyButtonCurrent.selectOnDown = References.Item1Text.transform.parent.GetComponent<Button>();
                break;

            case 5:
                fightButtonCurrent.selectOnUp = actButtonCurrent.selectOnUp = itemButtonCurrent.selectOnUp = mercyButtonCurrent.selectOnUp = References.Item3Text.transform.parent.GetComponent<Button>();
                fightButtonCurrent.selectOnDown = actButtonCurrent.selectOnDown = itemButtonCurrent.selectOnDown = mercyButtonCurrent.selectOnDown = References.Item1Text.transform.parent.GetComponent<Button>();
                break;
        }

        References.FightButton.navigation = fightButtonCurrent;
        References.ActButton.navigation = actButtonCurrent;
        References.ItemButton.navigation = itemButtonCurrent;
        References.MercyButton.navigation = mercyButtonCurrent;

        UpdateItemNavigation(numberOfItems);
    }

    void UpdateItemNavigation (int numberOfItems)
    {
        Debug.Assert(numberOfItems > 0 && numberOfItems < 6);

        // Get references to buttons
        Button item1Button = References.Item1Text.transform.parent.GetComponent<Button>();
        Button item2Button = References.Item2Text.transform.parent.GetComponent<Button>();
        Button item3Button = References.Item3Text.transform.parent.GetComponent<Button>();
        Button item4Button = References.Item4Text.transform.parent.GetComponent<Button>();
        Button item5Button = References.Item5Text.transform.parent.GetComponent<Button>();

        // Get navigation settings
        Navigation option1Current = item1Button.navigation;
        Navigation option2Current = item2Button.navigation;
        Navigation option3Current = item3Button.navigation;
        Navigation option4Current = item4Button.navigation;
        Navigation option5Current = item5Button.navigation;

        switch (numberOfItems)
        {
            case 1:

                option1Current.selectOnDown = References.FightButton;

                break;

            case 2:

                option1Current.selectOnDown = item2Button;
                option2Current.selectOnDown = References.FightButton;

                break;

            case 3:

                option1Current.selectOnDown = item2Button;
                option2Current.selectOnDown = item3Button;
                option3Current.selectOnDown = References.FightButton;

                break;

            case 4:

                option1Current.selectOnDown = item2Button;
                option2Current.selectOnDown = item3Button;
                option3Current.selectOnDown = References.FightButton;
                option4Current.selectOnDown = item3Button;

                break;

            case 5:

                option1Current.selectOnDown = item2Button;
                option2Current.selectOnDown = item3Button;
                option3Current.selectOnDown = References.FightButton;
                option4Current.selectOnDown = item5Button;
                option5Current.selectOnDown = References.ItemButton;

                break;
        }

        // Set navigation back after our changes
        item1Button.navigation = option1Current;
        item2Button.navigation = option2Current;
        item3Button.navigation = option3Current;
        item4Button.navigation = option4Current;
        item5Button.navigation = option5Current;
    }

    public void SetMercyNavigation ()
    {
        Navigation fightButtonCurrent = References.FightButton.navigation;
        Navigation actButtonCurrent = References.ActButton.navigation;
        Navigation itemButtonCurrent = References.ItemButton.navigation;
        Navigation mercyButtonCurrent = References.MercyButton.navigation;

        fightButtonCurrent.selectOnUp = actButtonCurrent.selectOnUp = itemButtonCurrent.selectOnUp = mercyButtonCurrent.selectOnUp = References.FleeButton;
        fightButtonCurrent.selectOnDown = actButtonCurrent.selectOnDown = itemButtonCurrent.selectOnDown = mercyButtonCurrent.selectOnDown = References.SpareButton;

        References.FightButton.navigation = fightButtonCurrent;
        References.ActButton.navigation = actButtonCurrent;
        References.ItemButton.navigation = itemButtonCurrent;
        References.MercyButton.navigation = mercyButtonCurrent;
    }

    #endregion

    #region Mono

    void OnEnable()
    {
        Debug.Assert(References);

        // Remove cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Enable dialogue windows
        References.DialogueBoxDefaultParent.SetActive(true);
        References.DialogueBoxOptionsParent.SetActive(true);
        References.DialogieBoxItemsParent.SetActive(true);
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    #endregion

    IEnumerator TextScroll(string text, TextMeshProUGUI textMesh)
    {
        text = "* " + text;

        for (int c = 0; c < text.Length + 1; c++)
        {
            textMesh.text = text.Substring(0, c);
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }
}

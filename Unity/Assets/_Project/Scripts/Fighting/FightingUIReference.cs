using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class FightingUIReference : MonoBehaviour
{
    /// <summary>
    /// Class to change the dialogue box's width and height
    /// </summary>
    public BoxMover MainBox;
    public PlayerAttackBar AttackBar;
    public EventSystem EventSystem;
    public Image EnemyImage;
    public GameObject TopParent;
    public GameObject TextParent;
    public GameObject ButtonParent;

    [Header("Buttons")]
    public Button FightButton;
    public Button ActButton;
    public Button ItemButton;
    public Button MercyButton;

    [Header("Player Text")]
    public TextMeshProUGUI PlayerNameText;
    public TextMeshProUGUI PlayerLevelText;
    public TextMeshProUGUI PlayerHealthText;

    [Header("Speech Bubbles")]
    public TextMeshProUGUI BubbleLeftText;
    public TextMeshProUGUI BubbleRightText;

    [Header("Enemy Text")]
    public TextMeshProUGUI EnemyHealthText;

    [Header("Dialogue")]
    public TextMeshProUGUI DefaultDialogueText;

    [Header("Act Options")]
    public TextMeshProUGUI OptionOneText;
    public TextMeshProUGUI OptionTwoText;
    public TextMeshProUGUI OptionThreeText;
    public TextMeshProUGUI OptionFourText;

    [Header("Inventory")]
    public TextMeshProUGUI Item1Text;
    public TextMeshProUGUI Item2Text;
    public TextMeshProUGUI Item3Text;
    public TextMeshProUGUI Item4Text;
    public TextMeshProUGUI Item5Text;

    [Header("Mercy")]
    public GameObject MercyOptionsParent;
    public Button SpareButton;
    public Button FleeButton;

    [Header("GameObjects")]
    public GameObject PlayerAttackObject;           // Image for when the player attacks an enemy
    public GameObject DialogueBoxDefaultParent;     // The first text screen when no buttons have been pressed
    public GameObject DialogueBoxOptionsParent;     // Text screen when options need to be selected
    public GameObject DialogieBoxItemsParent;       // Text screen when going through inventory / items
    public GameObject CollisionParent;
    public GameObject Player;
    public HeartController PlayerController;

    [Header("Speech Bubbles")]
    public TextMeshProUGUI Bottom;
    public TextMeshProUGUI Left, LeftShort, LeftWide, Right, RightLarge, RightLong, RightShort, RightWide, Top, TopTiny;
}

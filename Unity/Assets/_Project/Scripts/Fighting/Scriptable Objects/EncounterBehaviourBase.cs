using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

/// <summary>
/// Defines an enemy encounter with references to attack patterns
/// </summary>
public class EncounterBehaviourBase : MonoBehaviour
{
    public string EncounterText = "An enemy blocks your path";
    public float WaveTime = 4.0f;
    public BoxWidth ArenaWidth = BoxWidth.Square;
    public float CustomWidth = 155;
    public float CustomHeight = 130;
    public string VoiceEvent = "event:/";   // Expecting FMOD to be used later
    public bool CanFlee = true;

    protected int NumberOfDefenseRounds;

    public EnemyBehaviourBase[] Enemies;
    public EncounterWaveBase[] Waves;

    [ReadOnly]
    public EncounterWaveBase CurrentWave;

    void Start()
    {
        Debug.Assert(Enemies != null);
        Debug.Assert(Enemies.Length != 0);

        Debug.Assert(Waves != null);
        Debug.Assert(Waves.Length != 0);
    }

    /// <summary>
    /// Called when the encounter starts. This happens only once and after loading
    /// </summary>
    /// <returns>The game state the encounter should start at</returns>
    public virtual EncounterState EncounterStart()
    {
        return EncounterState.ActionSelect;
    }

    /// <summary>
    /// Called when the game enters <see cref="EncounterState.EnemyDialogue"/> state.
    /// </summary>
    public virtual List<string> EnemyDialogueStarting ()
    {
        List<string> ret;
        if (Enemies[0].CurrentDialogue.Count == 0)
        {
            ret = new List<string> { Enemies[0].RandomDialogue[Random.Range(0, Enemies[0].RandomDialogue.Count)] };
        }
        else
        {
            ret = new List<string>(Enemies[0].CurrentDialogue);
        }
        Enemies[0].CurrentDialogue.Clear();
        return ret;
    }

    /// <summary>
    /// Called just before entering <see cref="EncounterState.Defense"/> state. Use to set next wave
    /// </summary>
    public virtual void EnemyDialogueEnding ()
    {
        CurrentWave = Waves[Random.Range(0, Waves.Length)];
    }

    /// <summary>
    /// Called after <see cref="EncounterState.Defense"/> and before <see cref="EncounterState.ActionSelect"/>. Use to set the encounter text shown in the first <see cref="EncounterState.Defense"/> widget
    /// </summary>
    public virtual void DefenseRoundEnding ()
    {
        EncounterText = Enemies[0].Comments[Random.Range(0, Enemies[0].Comments.Count)];
    }

    public void StartWave (GameObject parentCanvas, RectTransform arena, GameObject Player)
    {
        Debug.Assert(CurrentWave);

        CurrentWave.InitWave(parentCanvas, arena, Player, WaveTime, NumberOfDefenseRounds);
        NumberOfDefenseRounds++;
    }

    public void UpdateWave ()
    {
        if (!CurrentWave) return;   // wait for the wave to be set

        CurrentWave.UpdateWave();
    }

    public void EndWave ()
    {
        if (!CurrentWave) return;

        CurrentWave.EndWave();
    }
}

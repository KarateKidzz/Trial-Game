using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SerializedPlayer : ScriptableObject
{
    public ScriptableInt MaxHealth;
    public ScriptableInt CurrentHealth;
    public ScriptableInt AttackPoints;
    public ScriptableInt DefensePoints;
    public ScriptableInt ExperiencePoints;

    static SerializedPlayer instance;

    [RuntimeInitializeOnLoadMethod]
    public static void Init()
    {
        instance = Resources.LoadAll<SerializedPlayer>("")[0];
        Reset();
        Debug.Log("[Player Data] Initialised");
    }

    static void Validate ()
    {
        Debug.Assert(instance != null);
    }

    public static void Reset ()
    {
        instance.MaxHealth.RuntimeValue = instance.MaxHealth.SerializedValue;
        instance.CurrentHealth.RuntimeValue = instance.CurrentHealth.SerializedValue;
        instance.AttackPoints.RuntimeValue = instance.AttackPoints.SerializedValue;
        instance.DefensePoints.RuntimeValue = instance.DefensePoints.SerializedValue;
        instance.ExperiencePoints.RuntimeValue = instance.ExperiencePoints.SerializedValue;
    }

    public static int GetMaxHealth ()
    {
        Validate();
        return instance.MaxHealth.RuntimeValue;
    }

    public static void SetMaxHealth (int value)
    {
        Validate();
        instance.MaxHealth.RuntimeValue = value;
    }

    public static int GetCurrentHealth ()
    {
        Validate();
        return instance.CurrentHealth.RuntimeValue;
    }

    public static void SetCurrentHealth (int value)
    {
        Validate();
        instance.CurrentHealth.RuntimeValue = value;
    }

    public static int GetAttackPoints ()
    {
        Validate();
        return instance.AttackPoints.RuntimeValue;
    }

    public static void SetAttackPoints (int value)
    {
        Validate();
        instance.AttackPoints.RuntimeValue = value;
    }

    public static int GetDefensePoints ()
    {
        Validate();
        return instance.DefensePoints.RuntimeValue;
    }

    public static void SetDefensePoints(int value)
    {
        Validate();
        instance.DefensePoints.RuntimeValue = value;
    }
}

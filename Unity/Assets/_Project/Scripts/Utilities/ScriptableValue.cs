using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScriptableValue<T>
{
    /// <summary>
    /// Value set in editor. Loads value into <see cref="RuntimeValue"/> at game start
    /// </summary>
    [Tooltip("The value that is saved to disk and used to set the runtime value at the start of the game")]
    public T SerializedValue;

    /// <summary>
    /// Value used during runtime. Can be edited and reset with <see cref="SerializedValue"/>
    /// </summary>
    [NonSerialized]
    public T RuntimeValue;
}

[Serializable]
public class ScriptableInt : ScriptableValue<int> { }

[Serializable]
public class ScriptableFloat : ScriptableValue<float> { }

[Serializable]
public class ScriptableBool : ScriptableValue<bool> { }

[Serializable]
public class ScriptableString : ScriptableValue<string> { }

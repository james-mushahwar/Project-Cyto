using System.Collections;
using System.Collections.Generic;
using System;
using _Scripts._Game.General;
using _Scripts._Game.General.Managers;
using UnityEngine;
using _Scripts._Game.Audio;
using UnityEngine.SceneManagement;
using _Scripts._Game.General.SceneLoading;
using Assets._Scripts._Game.General.SceneLoading;

// premade

[Serializable]
public class StringStringDictionary : SerializableDictionary<string, string> {}

[Serializable]
public class ObjectColorDictionary : SerializableDictionary<UnityEngine.Object, Color> {}

[Serializable]
public class ColorArrayStorage : SerializableDictionary.Storage<Color[]> {}

[Serializable]
public class StringColorArrayDictionary : SerializableDictionary<string, Color[], ColorArrayStorage> {}

[Serializable]
public class MyClass
{
    public int i;
    public string str;
}

[Serializable]
public class QuaternionMyClassDictionary : SerializableDictionary<Quaternion, MyClass> {}

// Cyto classes
[Serializable]
public class EEntityFEntityStatsDictionary : SerializableDictionary<EEntity, FEntityStats> {}
[Serializable]
public class PlayerInputPromptDictionary : SerializableDictionary<EPlayerInput, RectTransform> {}
[Serializable]
public class AudioPlaybackDictionary : SerializableDictionary<EAudioType, ScriptableAudioPlayback> {}
[Serializable]
public class EntityIconDictionary : SerializableDictionary<EEntity, Sprite> { }
[Serializable]
public class AudioConcurrencyDictionary : SerializableDictionary<EAudioType, AudioConcurrency> {}

[Serializable]
public class StringArrayStorage : SerializableDictionary.Storage<SceneField[]> { }
[Serializable]
public class SceneAdditivesDictionary : SerializableDictionary<string, SceneField[], StringArrayStorage> {}
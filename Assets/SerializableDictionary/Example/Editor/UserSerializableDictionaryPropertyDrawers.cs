using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StringStringDictionary))]
[CustomPropertyDrawer(typeof(ObjectColorDictionary))]
[CustomPropertyDrawer(typeof(StringColorArrayDictionary))]
[CustomPropertyDrawer(typeof(EEntityFEntityStatsDictionary))]
[CustomPropertyDrawer(typeof(PlayerInputPromptDictionary))]
[CustomPropertyDrawer(typeof(AudioPlaybackDictionary))]
[CustomPropertyDrawer(typeof(SceneAdditivesDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

[CustomPropertyDrawer(typeof(ColorArrayStorage))]
[CustomPropertyDrawer(typeof(StringArrayStorage))]
public class AnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}



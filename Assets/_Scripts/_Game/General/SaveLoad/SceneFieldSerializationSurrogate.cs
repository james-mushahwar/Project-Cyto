using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Assets._Scripts._Game.General.SceneLoading;
using UnityEngine;

namespace _Scripts._Game.General.SaveLoad{
    
    public class SceneFieldSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            SceneField sceneField = (SceneField)obj;
            //info.AddValue("_sceneAsset", sceneField.SceneAsset);
            info.AddValue("_sceneName", sceneField.SceneName);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            SceneField sceneField = (SceneField)obj;
            //sceneField.SceneAsset = (Object)info.GetValue("_sceneAsset", typeof(Object));
            //sceneField.SceneName = (string)info.GetValue("_sceneName", typeof(string));

            obj = sceneField;
            return obj;
        }
    }
    
}

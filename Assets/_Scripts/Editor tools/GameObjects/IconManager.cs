using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.VolumeComponent;

namespace _Scripts.Editortools.GameObjects
{

    public class IconManager
    {

        public enum LabelIcon
        {
            Gray = 0,
            Blue,
            Teal,
            Green,
            Yellow,
            Orange,
            Red,
            Purple
        }

        public enum Icon
        {
            CircleGray = 0,
            CircleBlue,
            CircleTeal,
            CircleGreen,
            CircleYellow,
            CircleOrange,
            CircleRed,
            CirclePurple,
            DiamondGray,
            DiamondBlue,
            DiamondTeal,
            DiamondGreen,
            DiamondYellow,
            DiamondOrange,
            DiamondRed,
            DiamondPurple
        }

        private static GUIContent[] labelIcons;
        private static GUIContent[] largeIcons;

        public static void SetIcon(GameObject gObj, LabelIcon icon)
        {
            if (labelIcons == null)
            {
                labelIcons = GetTextures("sv_label_", string.Empty, 0, 8);
            }

            SetIcon(gObj, labelIcons[(int)icon].image as Texture2D);
        }

        public static void SetIcon(GameObject gObj, Icon icon)
        {
            if (largeIcons == null)
            {
                largeIcons = GetTextures("sv_icon_dot", "_pix16_gizmo", 0, 16);
            }

            SetIcon(gObj, largeIcons[(int)icon].image as Texture2D);
        }

        private static void SetIcon(GameObject gObj, Texture2D texture)
        {
            var ty = typeof(EditorGUIUtility);
            EditorGUIUtility.SetIconForObject(gObj, texture);
            //var mi = ty.GetMethod("SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
            //mi.MakeGenericMethod(typeof(UnityEngine.Object));
            //mi.Invoke(null, new object[] { gObj, texture });
        }

        private static GUIContent[] GetTextures(string baseName, string postFix, int startIndex, int count)
        {
            GUIContent[] guiContentArray = new GUIContent[count];

            var t = typeof(EditorGUIUtility);
            //var mi = t.GetMethod("IconContent", BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(string) }, null);
            //mi.MakeGenericMethod(typeof(UnityEngine.Object));
            for (int index = 0; index < count; ++index)
            {
                guiContentArray[index] = EditorGUIUtility.IconContent(baseName + (object)(startIndex + index) + postFix);
                //guiContentArray[index] = mi.Invoke(null, new object[] { baseName + (object)(startIndex + index) + postFix }) as GUIContent;
            }

            return guiContentArray;
        }
    }

}

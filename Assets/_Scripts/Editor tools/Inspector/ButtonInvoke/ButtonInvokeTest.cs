using _Scripts.Editortools.Inspector.ButtonInvoke;
using UnityEngine;

namespace _Scripts.Editortools.Inspector.ButtonInvoke
{
    // Test script for the [ButtonInvoke] attribute that shows various usages.
    // Add this script to a cube or another 3D primitive to see it in action.

    // Using [ExecuteAlways] here to show that buttons can be optionally displayed in the editor. It's not required for buttons to work.
    [ExecuteAlways]
    public class ButtonInvokeTest : MonoBehaviour
    {
        #region Test Buttons
        // Just the method call - shows up in play mode only.
        // You could just use "Rotate" here as a string, but using nameof() will protect from typos and renaming a method.
        // The variable here is just a dummy placeholder that the field attribute is attached to.
        // The name of the variable (rotate) is used to display the button label.
        [ButtonInvoke(nameof(Rotate))] 
        public bool rotate;

        // Method with parameter.
        // This and the next line call the same method Grow(), but with a different parameter.
        // I'm using [SerializeField] to makes this private variable show up in the inspector.
        // You can just use public variables without [SerializeField].
        [SerializeField, ButtonInvoke(nameof(Grow), 3.0f)] 
        private bool _growALot;

        // Method with parameter that only works in editor mode.
        [SerializeField, ButtonInvoke(nameof(Grow), 1.1f, ButtonInvoke.DisplayIn.EditMode)] 
        private bool _growALittle;

        // Method that works both in play and editor modes and has a custom label.
        [SerializeField, ButtonInvoke(nameof(ChangeColor), null, ButtonInvoke.DisplayIn.PlayAndEditModes, "* C * O * L * O * R * I * F * I * C")] 
        private bool _changeColor;

        // Method that works both in play and editor modes.
        [SerializeField, ButtonInvoke(nameof(ResetToDefault), null, ButtonInvoke.DisplayIn.PlayAndEditModes)] 
        private bool _resetToDefault;
        #endregion

        #region Private Variables
        private readonly Vector3 _defaultScale = Vector3.one;
        private readonly Color _defaultColor = Color.gray;
        #endregion

        #region Methods To Test
        private void Rotate()
        {
            transform.Rotate(Vector3.up, 45.0f);
        }

        private void Grow(float howMuch)
        {
            transform.localScale *= howMuch;
        }

        private void ResetToDefault()
        {
            transform.localScale = _defaultScale;
            gameObject.GetComponent<Renderer>().sharedMaterial.color = _defaultColor;
        }

        private void ChangeColor()
        {
            gameObject.GetComponent<Renderer>().sharedMaterial.color = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1.0f, 1.0f);
        }
        #endregion
    }

}

using System.IO;
using UnityEngine;

namespace CheatsCore
{
    /// <summary>
    /// Module for creating in-game cheats. Before any usage Initialize must be called.
    /// Supports following controls:
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///             Label
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             Button
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             Toggle
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             Slider (integer, float)
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             Button with input field (integer, float, string)
    ///         </description>
    ///     </item>
    /// </list>
    /// Allows to split controls by tabs and sections (folds). See usage example in <see cref="Cheats.InitDemo"/> source.
    /// </summary>
    public class Cheats : MonoBehaviour
    {
        internal static bool IsEnabled;

        internal static CheatsWindow window;

        private const int WidthPerChar = 30;

        /// <summary>
        /// Cheats was shown. See also <see cref="Show"/> and <see cref="Hide"/>.
        /// </summary>
        public static bool IsShown => window.IsShown;

        /// <summary>
        /// Was cheats initialized?
        /// </summary>
        public static bool IsInitialized => window != null;

        public static void Initialize(RectTransform container, bool enabled)
        {
            if (IsInitialized) return;

            IsEnabled = enabled;

            var windowPrefab = Resources.Load<GameObject>(Path.Combine("UI", "Cheats", "CheatsWindow"));
            window = Instantiate(windowPrefab).GetComponent<CheatsWindow>();
            window.transform.SetParent(container);
            window.Hide();
        }

        public static void DeInitialize()
        {
            if (!IsInitialized) return;

            Destroy(window);

            window = null;
        }

        /// <summary>
        /// Show cheats window.
        /// </summary>
        public static void Show()
        {
            if (!IsEnabled) return;

            window.Show();
        }

        /// <summary>
        /// Hide cheats window.
        /// </summary>
        public static void Hide() => window.Hide();

        /// <summary>
        /// Return <see cref="CheatsSection"/> object (create new or fetch cached) for adding controls to it. Sections are
        /// split by tabs for handy navigation.
        /// </summary>
        /// <param name="header">Section header</param>
        /// <param name="tab">Cheats tab section belongs to</param>
        /// <returns>Newly created or cached cheats section with specified <paramref name="header"/> and <paramref name="tab"/></returns>
        public static CheatsSection GetSection(string header, string tab = null) => window.GetSection(header, tab);

        /// <summary>
        /// Initialize demo tabs with control examples. Use as reference for real in-game cheats.
        /// </summary>
        public static void InitDemo()
        {
            const string buttonsTabName = "Demo Buttons";
            const string labelsTabName = "Demo Labels";
            const string togglesTabName = "Demo Toggles";
            const string valueChangeTabName = "Demo Value Change Shortcut";

            void DoNothing()
            {
            }

            void DoNothingTyped<TType>(TType arg) where TType : struct
            {
            }

            GetSection("Simple Buttons", buttonsTabName)
                .AddButton("Button 1", DoNothing)
                .AddButton("Button 2", DoNothing)
                .AddButton("Button 3", DoNothing)
                .AddButton("Button 4", DoNothing)
                .AddButton("Button 5", DoNothing)
                ;

            GetSection("Another buttons", buttonsTabName)
                .AddButton("Button 1", DoNothing)
                .AddButton("Button 2", DoNothing)
                .AddButton("Button 3", DoNothing)
                .AddButton("Button 4", DoNothing)
                .AddButton("Button 5", DoNothing)
                .AddButton("Button 6", DoNothing)
                ;

            GetSection("And again", buttonsTabName)
                .AddButton("Button 1", DoNothing)
                .AddButton("Button 2", DoNothing)
                .AddButton("Button 3", DoNothing)
                .AddButton("Button 4", DoNothing)
                .AddButton("Button 5", DoNothing)
                .AddButton("Button 6 not so long", DoNothing)
                .AddButton("Button 7 that is very long", DoNothing)
                .AddButton("Button 8 it's so fucking big! Oh my god", DoNothing)
                ;

            GetSection("Line by line", buttonsTabName)
                .AddButton("Button 1", DoNothing)
                .AddLineBreaker()
                .AddButton("Button 2", DoNothing)
                .AddLineBreaker()
                .AddButton("Button 3", DoNothing)
                .AddLineBreaker()
                .AddButton("Button 4", DoNothing)
                ;

            GetSection("Labels", labelsTabName)
                .AddLabel("Label 1")
                .AddLabel("Label 2")
                .AddLabel("Label 3")
                .AddLabel("Label 4")
                .AddLabel("Label 5")
                ;

            GetSection("With bounds", labelsTabName)
                .AddLabel("Label 1", true)
                .AddLabel("Label 2", true)
                .AddLabel("Label 3", true)
                .AddLabel("Label 4 not so long", true)
                .AddLabel("Label 5 very very long label", true)
                ;

            GetSection("Dynamic", labelsTabName)
                .AddDynamicLabel(() => $"Frame count {UnityEngine.Time.frameCount}")
                .AddLineBreaker()
                .AddDynamicLabel(() => $"Time passed {UnityEngine.Time.realtimeSinceStartup:0.00}")
                ;

            GetSection("Toggles", togglesTabName)
                .AddToggle("Toggle", DoNothingTyped, () => true, true)
                .AddToggle("Long toggle", DoNothingTyped, () => true, true)
                .AddToggle("Very long toggle", DoNothingTyped, () => true, true)
                .AddToggle("Very very long toggle", DoNothingTyped, () => true, true)
                ;

            GetSection("Button + Label + Toggle", "Demo Misc")
                .AddButton("Some button", DoNothing)
                .AddLabel("some label")
                .AddToggle("some toggle", DoNothingTyped)
                ;

            GetSection("Sliders", "Demo Sliders")
                .AddSlider("Volume", DoNothingTyped, 1, 100, 50)
                .AddSlider("SFX", DoNothingTyped, 1, 100, 25, true)
                ;

            GetSection("Input fields", "Demo Buttons with Input")
                .AddButtonInt("Integer", v => Debug.Log(v))
                .AddButtonFloat("Float", v => Debug.Log(v))
                .AddButtonString("String", Debug.Log)
                ;

            GetSection("Integer, long, float", valueChangeTabName)
                .AddValueChangeShortcut(
                    "Integer", 0, DoNothingTyped, 1, 10, 100, 1000)
                .AddValueChangeShortcut(
                    "Long", (long) 100, DoNothingTyped, 1, 100, 1000000, 1000000000)
                .AddValueChangeShortcut(
                    "Float", 0f, DoNothingTyped, .001f, .05f, .25f, .5f, 1.5f)
                ;

            GetSection("Many options", valueChangeTabName)
                .AddValueChangeShortcut(
                    "Integer",
                    0,
                    DoNothingTyped,
                    1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000);
        }

        internal static float CalcTextWidth(string text) => CalcTextWidth(text.Length);

        internal static float CalcTextWidth(int length) => length * WidthPerChar;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Helper behavior which manages the debug UI. </summary>
public class DebugMenuUI : MonoBehaviour
{
    #region Editor

    [Header("Global")]
    [Tooltip("Display the debug UI?")]
    public bool displayUI;

    #endregion // Editor

    #region Internal

    /// <summary> Dimensions of the main window. </summary>
    private static Vector2 WINDOW_DIMENSION = new Vector2(256.0f, 192.0f);
    /// <summary> Base padding used within the UI. </summary>
    private static float BASE_PADDING = 8.0f;

    /// <summary> Rectangle representing the screen drawing area. </summary>
    private Rect mScreenRect;
    /// <summary> Rectangle representing the main window. </summary>
    private Rect mMainWindowRect;

    /// <summary> Dummy value used for demonstration. </summary>
    private float mDummyValue = 0.0f;

    #endregion // Internal

    #region Interface

    #endregion // Interface

    /// <summary> Called when the script instance is first loaded. </summary>
    private void Awake()
    { }

    /// <summary> Called before the first frame update. </summary>
    void Start()
    {
        // Deduce the drawing screen area from the main camera.
        var mainCamera = GameSettings.Instance.mainCamera;
        mScreenRect = new Rect(
            mainCamera.rect.x * Screen.width,
            mainCamera.rect.y * Screen.height,
            mainCamera.rect.width * Screen.width,
            mainCamera.rect.height * Screen.height
        );
        // Initially place the debug window into the top right corner.
        mMainWindowRect = new Rect(
            mScreenRect.x + mScreenRect.width - WINDOW_DIMENSION.x, mScreenRect.y,
            WINDOW_DIMENSION.x, WINDOW_DIMENSION.y
        );
    }

    /// <summary> Update called once per frame. </summary>
    void Update()
    { }

    /// <summary> Called when GUI drawing should be happening. </summary>
    private void OnGUI()
    {
        if (displayUI)
        { // Only draw the window if we are currently displaying it.
            mMainWindowRect = GUI.Window(0, mMainWindowRect, MainWindowUI, "Cheat Console");
            // Limit the window position to the screen area.
            mMainWindowRect.x = Mathf.Clamp(
                mMainWindowRect.x, mScreenRect.x,
                mScreenRect.x + mScreenRect.width - WINDOW_DIMENSION.x
            );
            mMainWindowRect.y = Mathf.Clamp(
                mMainWindowRect.y, mScreenRect.y,
                mScreenRect.y + mScreenRect.height - WINDOW_DIMENSION.y
            );
        }
    }

    /// <summary> Function used for drawing the main window. </summary>
    private void MainWindowUI(int windowId)
    {
        // Start the window drawing area, starting with some padding.
        GUILayout.BeginArea(new Rect(
            BASE_PADDING, 2.0f * BASE_PADDING,
            WINDOW_DIMENSION.x - 2.0f * BASE_PADDING,
            WINDOW_DIMENSION.y - 3.0f * BASE_PADDING
        ));
        { // Main Window Area

            // GUILayout allows us to automatically place UI elements after each other.
            // BeginVertical starts a vertical group, while BeginHorizontal a horizontal one.
            GUILayout.BeginVertical();
            {
                /*
                 * Task 3b: The Cheat
                 *
                 * Getting to the fun part - cheats. Wouldn't it be nice if we
                 * could create items infinitely? Well, we wouldn't want it to 
                 * be too accessible for our player, which is the reason why we
                 * hide it within the "Cheat Console".
                 *
                 * The variable containing the current available currency is easily
                 * accessible through InventoryManager.Instance.availableCurrency.
                 * But how do we modify it in the GUI? There are several ways, which
                 * you can try.
                 *
                 * We will be using the Layout mode of IMGUI, which allows us to
                 * skip specification of absolute positions and places the UI elements
                 * quite autonomously. Importantly, this REQUIRES the use of a separate
                 * interface under GUILayout (NOT just "GUI").
                 *
                 * Start by defining a block which will place elements horizontally after
                 * each other:
                */
                GUILayout.BeginHorizontal();
                {
                    // Elements defined here will be place after each other

                    GUILayout.Label("Currency: ", GUILayout.Width(WINDOW_DIMENSION.x / 4.0f));

                    // This defined a new label with the text "Currency: " with a fixed width.
                    var currency = InventoryManager.Instance.availableCurrency;

                    // A HorizontalSlider representing the currency
                    currency = (int)GUILayout.HorizontalSlider(currency, 0.0f, 1000.0f,
                        GUILayout.ExpandWidth(true));

                    // Update the currency only if the value changed
                    if (GUI.changed)
                    {
                        InventoryManager.Instance.availableCurrency = currency;
                    }
                }
                GUILayout.EndHorizontal();
                // Do not forget to end each group in the correct order!

                /*
                 * Task 3c: The Tool
                 *
                 * In this final task, you will be creating some utility debugging
                 * functions within our Cheat Console. In this case, you will have
                 * a little more autonomy.
                 *
                 * Create appropriate controls for the following handles:
                 *  * GameManager.Instance.interactiveMode: Allows mouse interaction
                 *    with the scene. Try it out - you can left-click scene objects
                 *    and drag.
                 *  * SoundManager.Instance.masterVolume: Master volume control for
                 *    all sounds. Its value is in dB, appropriate range <-80.0f, 20.0f>.
                 *  * SoundManager.Instance.masterMuted: Allows muting of the sound.
                 *
                 * For this task, it may be useful to look at what elements are available:
                 * https://docs.unity3d.com/2021.2/Documentation/Manual/gui-Controls.html
                 * , but note that you will probably want to use GUILayout instead of "GUI".
                 *
                 * This task can be considered as completed once all three handles can
                 * be controlled from the Cheat Console.
                 */

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Interactive Mode: ", GUILayout.Width(WINDOW_DIMENSION.x / 4.0f));

                    // Toggle for GameManager.Instance.interactiveMode
                    GameManager.Instance.interactiveMode = GUILayout.Toggle(
                        GameManager.Instance.interactiveMode, "Enabled", GUILayout.ExpandWidth(true));
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Master Volume: ", GUILayout.Width(WINDOW_DIMENSION.x / 4.0f));

                    // Slider for SoundManager.Instance.masterVolume
                    SoundManager.Instance.masterVolume = GUILayout.HorizontalSlider(
                        SoundManager.Instance.masterVolume, -80.0f, 20.0f, GUILayout.ExpandWidth(true));
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Master Muted: ", GUILayout.Width(WINDOW_DIMENSION.x / 4.0f));

                    // Toggle for SoundManager.Instance.masterMuted
                    SoundManager.Instance.masterMuted = GUILayout.Toggle(
                        SoundManager.Instance.masterMuted, "Muted", GUILayout.ExpandWidth(true));
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            // End the group!
        } // End of Main Window Area
        GUILayout.EndArea();

        // Allow dragging of the window by grabbing any part of it.
        GUI.DragWindow(new Rect(
            2.0f * BASE_PADDING, 0.0f,
            WINDOW_DIMENSION.x - 4.0f * BASE_PADDING,
            WINDOW_DIMENSION.y
        ));
    }
}

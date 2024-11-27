using UnityEngine;
using UnityEngine.InputSystem;

public class DancePadInputManager : MonoBehaviour, DancePadControls.IDancePadActions
{
  public static DancePadInputManager instance;
  private DancePadControls controls;
  public bool moveUp, moveDown, moveLeft, moveRight;
  private bool leftButtonDown, rightButtonDown;

  /// <summary>
  /// Initializes the singleton instance and sets up input controls.
  /// </summary>
  private void Awake()
  {
    if (instance == null)
      instance = this;
    else
      Destroy(gameObject);

    controls = new DancePadControls();
    controls.DancePad.SetCallbacks(this);
  }

  private void OnEnable() => controls.Enable();
  private void OnDisable() => controls.Disable();
  public void OnMoveUp(InputAction.CallbackContext context) => moveUp = context.performed;
  public void OnMoveDown(InputAction.CallbackContext context) => moveDown = context.performed;

  /// <summary>
  /// Called when the left button is pressed or released.
  /// Updates the state of the left button press.
  /// </summary>
  /// <param name="context">The input action context.</param>
  public void OnMoveLeft(InputAction.CallbackContext context)
  {
    moveLeft = context.performed;
    if (context.started) leftButtonDown = true;
    if (context.canceled) leftButtonDown = false;
  }

  /// <summary>
  /// Called when the right button is pressed or released.
  /// Updates the state of the right button press.
  /// </summary>
  /// <param name="context">The input action context.</param>
  public void OnMoveRight(InputAction.CallbackContext context)
  {
    moveRight = context.performed;
    if (context.started) rightButtonDown = true;
    if (context.canceled) rightButtonDown = false;
  }

  /// <summary>
  /// Checks if the left button was pressed once, resetting its state after use.
  /// </summary>
  /// <returns>True if the left button was pressed once; otherwise, false.</returns>
  public bool IsLeftPressedOnce()
  {
    if (leftButtonDown)
    {
      leftButtonDown = false;
      return true;
    }
    return false;
  }

  /// <summary>
  /// Checks if the right button was pressed once, resetting its state after use.
  /// </summary>
  /// <returns>True if the right button was pressed once; otherwise, false.</returns>
  public bool IsRightPressedOnce()
  {
    if (rightButtonDown)
    {
      rightButtonDown = false;
      return true;
    }
    return false;
  }
}

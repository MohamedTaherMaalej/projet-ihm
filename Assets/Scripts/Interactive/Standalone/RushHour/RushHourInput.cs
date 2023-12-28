using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RushHourInput : MonoBehaviour
{
    public static RushHourInput current; // singleton
    public float cubeSpeed = 1.0f;
    public InputAction deselectAllCubes = new InputAction();
    public InputAction moveHorizontal = new InputAction();
    public InputAction moveVertical = new InputAction();
    [HideInInspector] public Vector2 stickAxes;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(obj: this);
        }
    }

    void Start()
    {
        deselectAllCubes.Enable();
        moveHorizontal.Enable();
        moveVertical.Enable();
        deselectAllCubes.performed += deselect;
        moveHorizontal.performed += horizontalStart;
        moveVertical.performed += verticalStart;
        moveHorizontal.canceled += horizontalStop;
        moveVertical.canceled += verticalStop;
    }

    void OnDestroy()
    {
        deselectAllCubes.performed -= deselect;
        moveHorizontal.performed -= horizontalStart;
        moveVertical.performed -= verticalStart;
        moveHorizontal.canceled -= horizontalStop;
        moveVertical.canceled -= verticalStop;
        current = null;
    }

    private void deselect(InputAction.CallbackContext ctx)
    {
        if (CubeSelectionManager.current.somethingIsSelected)
            AudioManager.current.getSource().PlayOneShot(CubeSelectionManager.current.deselectAllSound);
        CubeSelectionManager.current.DeselectAllCubes();
    }

    private void horizontalStart(InputAction.CallbackContext ctx)
    {
        stickAxes.x = ctx.ReadValue<float>();
    }

    private void verticalStart(InputAction.CallbackContext ctx)
    {
        stickAxes.y = ctx.ReadValue<float>();
    }

    private void horizontalStop(InputAction.CallbackContext ctx)
    {
        stickAxes.x = 0;
    }

    private void verticalStop(InputAction.CallbackContext ctx)
    {
        stickAxes.y = 0;
    }
}

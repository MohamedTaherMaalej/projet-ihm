using UnityEngine;
using UnityEngine.EventSystems;

public class CubeController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public float moveSpeed = 5f; // Adjust the speed as needed
    public string inputAxis; // "Horizontal" for Z-axis, "Vertical" for X-axis
    [HideInInspector] public bool isSelected = false;
    public bool isTarget = false;

    void Update()
    {
        if (isTarget && TriggerWall.current.complete)
        {
            transform.Translate(new Vector3(0, 0, moveSpeed / 2 * Time.deltaTime));
            return;
        }
        if (isSelected)
        {
            switch (inputAxis)
            {
                case "Horizontal":
                    MoveCube(RushHourInput.current.stickAxes.x);
                    break;
                case "Vertical":
                    MoveCube(RushHourInput.current.stickAxes.y);
                    break;
            }
        }
        else
        {
            StopCube();
        }
    }

    void MoveCube(float input)
    {
        if (Mathf.Approximately(input, 0))
        {
            StopCube();
            return;
        }
        float translation = input * moveSpeed * Time.deltaTime;

        if (inputAxis == "Horizontal")
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, translation), ForceMode.VelocityChange);
        }
        else if (inputAxis == "Vertical")
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(-translation, 0, 0), ForceMode.VelocityChange);
        }
    }

    void StopCube()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void SelectCube()
    {
        if (!isSelected)
            AudioManager.current.getSource().PlayOneShot(CubeSelectionManager.current.selectSound);
        CubeSelectionManager.current.somethingIsSelected = true;
        isSelected = true;
        if (isTarget)
            GetComponent<MeshRenderer>().material = CubeSelectionManager.current.selectedMaterialForTarget;
        else
            GetComponent<MeshRenderer>().material = CubeSelectionManager.current.selectedMaterial;
    }

    public void DeselectCube()
    {
        isSelected = false;
        if (isTarget)
            GetComponent<MeshRenderer>().material = CubeSelectionManager.current.defaultMaterialForTarget;
        else
            GetComponent<MeshRenderer>().material = CubeSelectionManager.current.defaultMaterial;
    }

    public void Clicked()
    {
        if (TriggerWall.current.complete)
            return;
        CubeSelectionManager.current.DeselectAllCubes();
        SelectCube();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}

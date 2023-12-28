using UnityEngine;

public class PadlockShackle : MonoBehaviour
{
    [Tooltip("How many degrees (clockwise) the shackle should rotate by (yaw around the pivot) when the lock opens")]
    public float turnBy;
    public float animationSpeed;
    public GameObject pivot;
    public bool open { get; private set; }
    private Padlock padlock;

    void Start()
    {
        padlock = transform.parent.GetComponent<Padlock>();
        if (padlock == null)
        {
            Debug.LogError("Error initializing PadlockShackle: must be a child of an object with Padlock component/script attached");
            Destroy(obj: this);
        }
        padlock.OnChangeOpenState += updateOpenState;
    }
    void OnDestroy()
    {
        padlock.OnChangeOpenState -= updateOpenState;
    }

    void Update()
    {
        if (open)
        {
            if (Mathf.Abs(transform.localRotation.eulerAngles.y - turnBy) >= animationSpeed * Time.deltaTime)
                transform.RotateAround(pivot.transform.position, Vector3.up, Mathf.Sign(turnBy) * animationSpeed * Time.deltaTime);
            else
                transform.localRotation = Quaternion.Euler(0, turnBy, 0);
        }
        else
        {
            if (Mathf.Abs(transform.localRotation.eulerAngles.y) >= animationSpeed * Time.deltaTime)
                transform.RotateAround(pivot.transform.position, Vector3.up, -Mathf.Sign(turnBy) * animationSpeed * Time.deltaTime);
            else
                transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void updateOpenState(bool lockIsOpen)
    {
        open = lockIsOpen;
    }
}

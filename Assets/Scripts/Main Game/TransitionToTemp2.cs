using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToTemp2 : MonoBehaviour
{
    public static TransitionToTemp2 current;

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
        StartCoroutine(transition());
    }
    IEnumerator transition()
    {
        yield return new WaitForSeconds(3);
        ProgressManager.current.changeTemp(2);
        SceneManager.LoadScene("Bedroom", LoadSceneMode.Single);
    }

    void OnDestroy()
    {
        current = null;
    }

    void OnEnable()
    {
        StartCoroutine(LateOnEnable());
    }
    private IEnumerator LateOnEnable()
    {
        yield return null; // wait one frame
        float dyaw = Camera.main.transform.rotation.eulerAngles.y - Camera.main.transform.parent.parent.rotation.eulerAngles.y;
        Camera.main.transform.parent.parent.Rotate(0, -dyaw, 0);
    }
}

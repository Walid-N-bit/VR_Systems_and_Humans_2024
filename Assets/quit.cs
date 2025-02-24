using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{
    public InputActionReference buttonPressAction;

    private void OnEnable()
    {
        buttonPressAction.action.Enable();
    }

    private void OnDisable()
    {
        buttonPressAction.action.Disable();
    }

    void Update()
    {
        if (buttonPressAction.action.triggered)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
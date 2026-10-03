using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTransition : MonoBehaviour
{
    [Header("References")]
    public GameObject mainMenuUI;       
    public Rigidbody playerCapsule;     

    [Header("Jump Settings")]
    public float delayBeforeJump = 0.5f;                  
    public Vector3 jumpForce = new Vector3(0f, 15f, 10f); 
    public float waitTimeBeforeStart = 1.5f;              
    public string nextSceneName = "SampleScene";          

    public void StartGameTransition()
    {
        StartCoroutine(PlayTransitionRoutine());
    }

    private IEnumerator PlayTransitionRoutine()
    {
        if (mainMenuUI != null)
            mainMenuUI.SetActive(false);

        yield return new WaitForSeconds(delayBeforeJump);

        if (playerCapsule != null)
        {
            MenuParallax parallaxScript = playerCapsule.GetComponent<MenuParallax>();
            if (parallaxScript != null)
            {
                parallaxScript.enabled = false;
            }
            playerCapsule.useGravity = true; 
            playerCapsule.isKinematic = false; 
            
            playerCapsule.AddForce(jumpForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(waitTimeBeforeStart);

        SceneManager.LoadScene(nextSceneName);
    }
}
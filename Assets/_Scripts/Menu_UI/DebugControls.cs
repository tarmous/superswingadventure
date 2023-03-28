using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugControls : MonoBehaviour {

    public GameObject directionReleaseForce,
                      velocityReleaseForceX,
                      velocityReleaseForceY,
                      hookForceX,
                      hookForceY,
                      denominator,
                      followSpeed,
                      mass,
                      gravityScale,
                      linearDrag,
                      angularDrag, 
                      bounciness;

    public GameObject currentDirectionReleaseForce,
                      currentVelocityReleaseForceX,
                      currentVelocityReleaseForceY,
                      currentHookForceX,
                      currentHookForceY,
                      currentdenominator,
                      currentFollowSpeed,
                      currentMass,
                      currentGravityScale,
                      currentLinearDrag,
                      currentAngularDrag,
                      currentBounciness;                  


    public const float defaultDirectionReleaseForce = 60000,
                 defaultVelocityReleaseForceX = 30000,
                 defaultVelocityReleaseForceY = 30000,
                 defaultHookForceX = 45000, 
                 defaultHookForceY = 45000,
                 defaultdenominator = 500,
                 defaultFollowSpeed = 1f,
                 defaultMass = 15,
                 defaultGravityScale = 15,
                 defaultLinearDrag = 0.1f,
                 defaultAngularDrag = 5000,
                 defaultBounciness=0.9f;


    public GameObject timeScale;
    public GameObject startGameButton, loadingSceneText;
    private AsyncOperation asyncLoad;
//    private bool loadScene = true;

    public void RestartLevel(){
        
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    
       
    }

    public void ReturnToMainMenu(){

       SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    public IEnumerator LoadAsyncScene(int id){
        asyncLoad = SceneManager.LoadSceneAsync(id, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation=false;

        while (asyncLoad.progress < 0.9f){
            yield return null;
        }

        startGameButton.SetActive(true);
        loadingSceneText.SetActive(false);
        
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone){
            yield return null;
        }
    }

    public void StartGame(){

        if ( !asyncLoad.allowSceneActivation ){    
           /*
            if (directionReleaseForce.GetComponent<Text>().text != "")
                PlayerPrefs.SetFloat("directionReleaseForce", float.Parse(directionReleaseForce.GetComponent<Text>().text));

            if (velocityReleaseForceX.GetComponent<Text>().text != "")
                PlayerPrefs.SetFloat("velocityReleaseForceX", float.Parse(velocityReleaseForceX.GetComponent<Text>().text));

            if (velocityReleaseForceY.GetComponent<Text>().text != "")
                PlayerPrefs.SetFloat("velocityReleaseForceY", float.Parse(velocityReleaseForceY.GetComponent<Text>().text));
 
            if (hookForceX.GetComponent<Text>().text != "")
                PlayerPrefs.SetFloat("hookForceX", float.Parse(hookForceX.GetComponent<Text>().text));

            if (hookForceY.GetComponent<Text>().text != "")
                PlayerPrefs.SetFloat("hookForceY", float.Parse(hookForceY.GetComponent<Text>().text));

            if (denominator.GetComponent<Text>().text != "")
                PlayerPrefs.SetFloat("denominator", float.Parse(denominator.GetComponent<Text>().text));

            if (followSpeed.GetComponent<Text>().text != "")
                PlayerPrefs.SetFloat("followSpeed", float.Parse(followSpeed.GetComponent<Text>().text));

            if (mass.GetComponent<Text>().text != "")
                PlayerPrefs.SetFloat("mass", float.Parse(mass.GetComponent<Text>().text));

            if (gravityScale.GetComponent<Text>().text != "")
                PlayerPrefs.SetFloat("gravityScale", float.Parse(gravityScale.GetComponent<Text>().text));

            if (angularDrag.GetComponent<Text>().text != "")
                PlayerPrefs.SetFloat("angularDrag", float.Parse(angularDrag.GetComponent<Text>().text));

            if (linearDrag.GetComponent<Text>().text != "")
                PlayerPrefs.SetFloat("linearDrag", float.Parse(linearDrag.GetComponent<Text>().text));  

            if (bounciness.GetComponent<Text>().text != "")
                PlayerPrefs.SetFloat("bounciness", float.Parse(bounciness.GetComponent<Text>().text));               
            */
            asyncLoad.allowSceneActivation=true;
            //StartCoroutine(LoadAsyncScene(1));
            //SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }
            
    }


    public void DefaultValues(){

        directionReleaseForce.GetComponentInParent<InputField>().text = defaultDirectionReleaseForce.ToString();

        velocityReleaseForceX.GetComponentInParent<InputField>().text = defaultVelocityReleaseForceX.ToString();

        velocityReleaseForceY.GetComponentInParent<InputField>().text = defaultVelocityReleaseForceY.ToString();

        hookForceX.GetComponentInParent<InputField>().text = defaultHookForceX.ToString();

        hookForceY.GetComponentInParent<InputField>().text = defaultHookForceY.ToString();

        denominator.GetComponentInParent<InputField>().text = defaultdenominator.ToString();

        followSpeed.GetComponentInParent<InputField>().text = defaultFollowSpeed.ToString();

        mass.GetComponentInParent<InputField>().text = defaultMass.ToString();

        gravityScale.GetComponentInParent<InputField>().text = defaultGravityScale.ToString();

        linearDrag.GetComponentInParent<InputField>().text = defaultLinearDrag.ToString();

        angularDrag.GetComponentInParent<InputField>().text = defaultAngularDrag.ToString();

        bounciness.GetComponentInParent<InputField>().text = defaultBounciness.ToString();

    }

    public void CurrentValues(){
        if (PlayerPrefs.HasKey("directionReleaseForce"))
        currentDirectionReleaseForce.GetComponent<Text>().text = "Current: " + PlayerPrefs.GetFloat("directionReleaseForce");

        if (PlayerPrefs.HasKey("velocityReleaseForceX"))
        currentVelocityReleaseForceX.GetComponent<Text>().text = "Current: " + PlayerPrefs.GetFloat("velocityReleaseForceX");

        if (PlayerPrefs.HasKey("velocityReleaseForceY"))
        currentVelocityReleaseForceY.GetComponent<Text>().text = "Current: " + PlayerPrefs.GetFloat("velocityReleaseForceY");

        if (PlayerPrefs.HasKey("hookForceX"))
        currentHookForceX.GetComponent<Text>().text = "Current: " + PlayerPrefs.GetFloat("hookForceX");

        if (PlayerPrefs.HasKey("hookForceY"))
        currentHookForceY.GetComponent<Text>().text = "Current: " + PlayerPrefs.GetFloat("hookForceY");

        if (PlayerPrefs.HasKey("denominator"))
        currentdenominator.GetComponent<Text>().text = "Current: " + PlayerPrefs.GetFloat("denominator");

        if (PlayerPrefs.HasKey("followSpeed"))
        currentFollowSpeed.GetComponent<Text>().text = "Current: " + PlayerPrefs.GetFloat("followSpeed");

        if (PlayerPrefs.HasKey("mass"))
        currentMass.GetComponent<Text>().text = "Current: " + PlayerPrefs.GetFloat("mass");

        if (PlayerPrefs.HasKey("gravityScale"))
        currentGravityScale.GetComponent<Text>().text = "Current: " + PlayerPrefs.GetFloat("gravityScale");

        if (PlayerPrefs.HasKey("linearDrag"))
        currentLinearDrag.GetComponent<Text>().text = "Current: " + PlayerPrefs.GetFloat("linearDrag");

        if (PlayerPrefs.HasKey("angularDrag"))
        currentAngularDrag.GetComponent<Text>().text = "Current: " + PlayerPrefs.GetFloat("angularDrag");

        if (PlayerPrefs.HasKey("bounciness"))
        currentBounciness.GetComponent<Text>().text = "Current: " + PlayerPrefs.GetFloat("bounciness");
    }
 
    public void Start(){
        Application.targetFrameRate = 60;
        
        //if (currentDirectionReleaseForce != null) CurrentValues();         
    }

    public void LateUpdate(){
        if (timeScale != null){
            timeScale.GetComponent<Text>().text = "Time Scale: "+ Time.timeScale;
        }

        /* if(loadScene){
            if (SceneManager.GetActiveScene().buildIndex == 0){
                loadScene = !loadScene;
                startGameButton.SetActive(false);
                loadingSceneText.SetActive(true);
                StartCoroutine(LoadAsyncScene(1));
            }
        }  */

    }
}

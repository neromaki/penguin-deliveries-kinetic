using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrajectoryManager : MonoBehaviour
{
    //*************************************INSTRUCTIONS***************************//
    //1. Attatch Script To an Empty Called TrajectoryManager. Trajectory manager MUST be in scene at all times
    //2. Create a Line Renderer, as a child of TrajectoryManager ---Assign Line Renderer to this script in Inspector
    //3. Create a Simulated Object that has the exact same Rigid Body Parameters as the Object you are launching --- Assign Simulated Object to this script in Inspector
    //4. Tag Any Collision you want interaction with as "Collidable"
    //5. When you want Trajectory line to appear/disappear, call TrajectoryManager.Instance.EnableLine(true/false) /// 
    //6. Use TrajectoryManager.Instance.SimulateLaunch(transform,force) in your launched objects script to update trajectory

    #region SingletonPattern
    private static TrajectoryManager _instance;
    public static TrajectoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("The AudioManager is Null");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }
    #endregion

    private PhysicsScene2D _physicsSim;
    [SerializeField]
    private GameObject _simulatedObject; //drag your simulated player into the inspector
    [SerializeField]
    LineRenderer line;//drag your lineRenderer into the inspector 
    Scene _simScene;
    [SerializeField]
    int _steps = 20; //how long we will be simulating for. More steps, more lenghth but also less performance
    //Vector3[] points;
    private List<Vector3> points;


    void Start()
    {
        CreateSceneParameters _param = new CreateSceneParameters(LocalPhysicsMode.Physics2D); //define the parameters of a new scene, this lets us have our own separate physics 
        _simScene =  SceneManager.CreateScene("Simulation",_param); // create a new scene and implement the parameters we just created
        _physicsSim = _simScene.GetPhysicsScene2D(); // assign the physics of the scene so we can simulate on our own time. 
        points = new List<Vector3>();
        line.positionCount = _steps; // set amount of points our drawn line will have
        CreateSimObjects(); // send over simulated objects (see method below for details)
        //points = new Vector3[_steps]; // set amount of points our simulation will record, these will later be passed into the line.
    }

        private void CreateSimObjects()  //all objects start in regulare scene, and get sent over on start. this way colliders are dynamic and we can grab refrence to simulated player in first scene.
    {
        SceneManager.MoveGameObjectToScene(_simulatedObject, _simScene); // move the simulated player to the sim scene
        GameObject[] _collidables = GameObject.FindGameObjectsWithTag("Planet");      //check for all objects tagged collidable in scene. More optimal routes but this is most user friendly
        _collidables.Concat(GameObject.FindGameObjectsWithTag("Destination")).ToArray();
        
        foreach (GameObject GO in _collidables)   //duplicate all collidables and move them to the simulation
        {
            var newGO = Instantiate(GO, GO.transform.position, GO.transform.rotation);  
            SceneManager.MoveGameObjectToScene(newGO, _simScene);
        }
    }

    public void EnableLine(bool enabled)  //call this from player to turn the projection line on/off
    {
        line.gameObject.SetActive(enabled);
    }


    Vector3 _lastForce = Vector3.zero; //used to track what the last force input was 

    public void SimulateLaunch(Transform projectile, Vector3 force)   //call this every frame while player is grabed;
    {
        _simulatedObject.transform.position = projectile.position; //set sim object to player position ;
        _simulatedObject.transform.rotation = projectile.rotation; // set sim object to player rotation;
        _simulatedObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero; // resets sim objects velocity to 0;
        points = new List<Vector3>();


        if (_lastForce != force) //if force hasnt changed, skip simulation;
        { 
            _simulatedObject.GetComponent<Rigidbody2D>().AddForce(force); //simulate the objects path
            
            for (var i = 0; i < _steps-1; i++) // steps is how many physics steps will be done in a frame 
            {
                _physicsSim.Simulate(Time.fixedDeltaTime); // move the physics, one step ahead. (anymore than 1 step creates irregularity in the trajectory)
                //points[i] = _simulatedObject.transform.position; //record the simulated objects position for that step
                points.Insert(i, _simulatedObject.transform.position);
                line.SetPosition(i, points[i]); //let the line render know where to plot a point
            }
       }
        _lastForce = force;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public float turnSpeed = 100.0f;
    public float minAngle = 51.0f;
    public float maxAngle = 311.0f;
    public float thrust = 100.0f;
    private bool _fired = false;

    public new GameObject camera;
    
    public GameObject cannonBase;
    public GameObject cannonBarrel;
    public GameObject projectile;
    public GameObject gameController;

    private AudioSource[] _audio;
    private ProjectileController _projectileController;
    private Rigidbody2D _rigidbody2D;
    private CameraController _cameraController;

    
    public float minTorque = 0.5f;
    public float maxTorque = 2.0f;

    // Start is called before the first frame update
    private void Start()
    {
        _cameraController = camera.GetComponent<CameraController>();
        _rigidbody2D = projectile.GetComponent<Rigidbody2D>();
        _projectileController = projectile.GetComponent<ProjectileController>();
        _audio = GetComponents<AudioSource>();
        //TrajectoryManager.Instance.EnableLine(true);
    }

    private void EnablePhysics()
    {
        projectile.tag = "affectedByPlanetGravity";
    }
    private void FixedUpdate()
    {
        if (!_fired)
        {
            // Rotation
            var hAxis = Input.GetAxis("Horizontal");
            var axisInput = new Vector3(0.0f, 0.0f, hAxis);
            if (Input.GetAxis("Horizontal") != 0.0f)
            {
                if (!_audio[0].isPlaying)
                {
                    _audio[0].Play();
                }
            }
            else
            {
                _audio[0].Stop();
            }
            var totalRotation = -axisInput * (turnSpeed * Time.deltaTime);
            var barrelRotation = cannonBarrel.transform.eulerAngles.z;
            

            var beyondMinAngle = barrelRotation > minAngle && barrelRotation < minAngle + 50.0f;
            var beyondMaxAngle = barrelRotation < maxAngle && barrelRotation > maxAngle - 50.0f;
            if (beyondMinAngle && hAxis < 0.0f
                || beyondMaxAngle && hAxis > 0.0f)
            {
                if (!_audio[1].isPlaying)
                {
                    _audio[1].Play();
                }
                cannonBarrel.transform.Rotate(Vector3.zero);
                if (beyondMinAngle)
                {
                    cannonBarrel.transform.eulerAngles = new Vector3(0,0,minAngle);
                } else
                {
                    cannonBarrel.transform.eulerAngles = new Vector3(0,0,maxAngle);
                }
            }
            else
            {
                cannonBarrel.transform.Rotate(totalRotation);
            }

            var simTrans = cannonBarrel.transform;
            var simForce = projectile.transform.up * thrust;
            //TrajectoryManager.Instance.SimulateLaunch(simTrans, simForce);
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (_fired) return;
            
            _fired = true;
            gameController.GetComponent<GameController>().StartTimer();
            
            _projectileController.originPos = projectile.transform.position;
            _projectileController.originRot = projectile.transform.rotation;
            projectile.transform.parent = null;
            _rigidbody2D.AddForce(projectile.transform.up * thrust);
            _rigidbody2D.AddTorque(Random.Range(minTorque, maxTorque));

            _cameraController.SetTarget(projectile);
            _cameraController.SetSize(10.0f);
            Invoke(nameof(EnablePhysics), 1.0f);
            
            _audio[0].Stop();
            _audio[2].Play();

        } else if (Input.GetKey(KeyCode.S) && _fired)
        {
            _fired = false;
            gameController.GetComponent<GameController>().ResetLevel();
         
            _cameraController.SetTarget(null);
            _cameraController.SetSize(0.0f);
            
            projectile.tag = "Untagged";
            _rigidbody2D.Sleep();
            projectile.transform.SetPositionAndRotation(_projectileController.originPos, _projectileController.originRot);
            projectile.transform.SetParent(cannonBarrel.transform);
        }
    }
}

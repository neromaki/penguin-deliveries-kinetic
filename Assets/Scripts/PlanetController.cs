using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    
	// Physics settings
	
	// The relative mass of the object
	public float mass;     		
	// The radius of the "sphere of influence" This can be set to infinity (or a very large number) for more realistic gravity.
	public int soiRadius;	
	// Used to alter (unatuarally) the coorelation between the proximity of the objects to the severity of the attraction.  Tweak to make orbits easier to achieve or more intersting.
	public int proximityModifier = 195;

	public bool rotate = false;
	public float rotateSpeed = 1.0f;
	
	// On init of obj
	void Start() {
		mass = mass * 1000; // Mass ^ 5 in order to allow the relative mass input to be more readable
	}
	
	// Creates a visual representation of the sphere of influence in the editor
	public void OnDrawGizmos() {
		// Show the Object's Sphere Of Influence
		Gizmos.DrawWireSphere (transform.position, soiRadius);
	}

	void Update()
	{
		if (rotate)
		{
			var rotation = new Vector3(0.0f, 0.0f, rotateSpeed);
			transform.Rotate(rotation);
		}
	}

	void FixedUpdate () { // Runs continuously during gameplay

		// Get all objects that will be affected by gravity (Game objects are tagged in order to be influenced by gravity)
		GameObject[] objectsAffectedByGravity;objectsAffectedByGravity = GameObject.FindGameObjectsWithTag ("affectedByPlanetGravity");
		
		foreach (GameObject gravBody in objectsAffectedByGravity) { // Iterate through objects affected by gravity
				
			Rigidbody2D gravRigidBody = gravBody.GetComponent<Rigidbody2D> (); // Get the object's Rigid Body Component
			
			float orbitalDistance = Vector3.Distance (transform.position, gravRigidBody.transform.position); // Get the object's distance from the World Body
			
			if (orbitalDistance < soiRadius) { // If the object is in the sphere of influence (close enough to be affected by the gravity of this object)

				// Get info about the object in the sphere of influence

				Vector3 objectOffset = transform.position - gravRigidBody.transform.position; // Get the object's 2d offset relative to this World Body
				objectOffset.z = 0;
				
				Vector3 objectTrajectory = gravRigidBody.velocity; // Get object's trajectory vector
				
				float angle = Vector3.Angle (objectOffset, objectTrajectory); // Calculate object's angle of attack ( Not used here, but potentially insteresting to have )
				
				float magsqr = objectOffset.sqrMagnitude; // Square Magnitude of the object's offset
				
				if ( magsqr > 0.0001f ) { // If object's force is significant

					// Apply gravitational force to the object
					Vector3 gravityVector = ( mass * objectOffset.normalized / magsqr ) * gravRigidBody.mass;
					gravRigidBody.AddForce ( gravityVector * ( orbitalDistance/proximityModifier) );
					
				}
			} 
		}
	}
}

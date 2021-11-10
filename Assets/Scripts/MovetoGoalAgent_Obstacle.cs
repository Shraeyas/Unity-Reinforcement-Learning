using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class MovetoGoalAgent_Obstacle : Agent {
    [SerializeField] private Transform targetTransform;
    [SerializeField] private MeshRenderer groundMeshRenderer;
    [SerializeField] private Material loseMaterial, winMaterial, lookattarget, lookatobstacle;
    [SerializeField] private Transform ground;
    [SerializeField] private Transform wallFront, wallBack;
    [SerializeField] private Transform wallLeft, wallRight;
    [SerializeField] private Transform wallFront_C, wallBack_C;
    [SerializeField] private Transform wallLeft_C, wallRight_C;
    [SerializeField] private Transform obstacles;
    GameObject[] gameObjects = new GameObject[1];
    void RandomizePositionsScales(float scaleX, float scaleZ) {
        ground.localScale = new Vector3(scaleX, 1, scaleZ);

        wallRight.localPosition = new Vector3(scaleX/2, 0, 0);
        wallRight.localScale = new Vector3(1, 3.9f, scaleZ);
        wallRight_C.localPosition = new Vector3(scaleX / 2, 0, 0);
        wallRight_C.localScale = new Vector3(1, 3.9f, scaleZ);

        wallLeft.localPosition = new Vector3(-scaleX/2, 0, 0);
        wallLeft.localScale = new Vector3(1, 3.9f, scaleZ);
        wallLeft_C.localPosition = new Vector3(-scaleX / 2, 0, 0);
        wallLeft_C.localScale = new Vector3(1, 3.9f, scaleZ);

        wallFront.localPosition = new Vector3(0, 0, scaleZ/2);
        wallFront.localScale = new Vector3(1, 3.9f, scaleX);
        wallFront.localRotation = Quaternion.Euler(new Vector3(0, 90f, 0));
        wallFront_C.localPosition = new Vector3(0, 0, scaleZ / 2);
        wallFront_C.localScale = new Vector3(1, 3.9f, scaleX);
        wallFront_C.localRotation = Quaternion.Euler(new Vector3(0, 90f, 0));

        wallBack.localPosition = new Vector3(0, 0, -scaleZ/2);
        wallBack.localScale = new Vector3(1, 3.9f, scaleX);
        wallBack.localRotation = Quaternion.Euler(new Vector3(0, 90f, 0));
        wallBack_C.localPosition = new Vector3(0, 0, -scaleZ / 2);
        wallBack_C.localScale = new Vector3(1, 3.9f, scaleX);
        wallBack_C.localRotation = Quaternion.Euler(new Vector3(0, 90f, 0));

        transform.localPosition = new Vector3(Random.Range(-scaleX / 2 + 1.5f, scaleX / 2 - 1.5f), 1.016f, Random.Range(-scaleZ / 2 + 1.5f, scaleZ / 2 - 1.5f));
        targetTransform.localPosition = new Vector3(Random.Range(-scaleX / 2 + 1.5f, scaleX / 2 - 1.5f), 1.016f, Random.Range(-scaleZ / 2 + 1.5f, scaleZ / 2 - 1.5f));
    }
    GameObject[] SpawnObstacles(float scaleX, float scaleZ) {
        int numObstacles = Random.Range(1, 5);
        GameObject[] gameObjects = new GameObject[numObstacles];
        for(int i = 0; i < numObstacles; i++) {
            gameObjects[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gameObjects[i].tag = "obstacle";
            gameObjects[i].transform.SetParent(obstacles);
            gameObjects[i].AddComponent<Wall>();
            gameObjects[i].AddComponent<BoxCollider>();
            gameObjects[i].GetComponent<Collider>().isTrigger = true;
            gameObjects[i].transform.localPosition = new Vector3(Random.Range(-scaleX/2 + 1, scaleX/2 - 1), 1, Random.Range(-scaleZ/2 + 1, scaleZ/2 - 1));
        }
        return gameObjects;
    }
    void DespawnObstacles(GameObject [] gameObjects) {
        for (int i = 0; i < gameObjects.Length; i++) {
            try {
                Destroy(gameObjects[i]);
            }
            catch(System.Exception e) {
                Debug.Log(e.Message);
            }
        }
    }
    Vector3[] RayCast() {
        RaycastHit raycastHit;
        Vector3[] positions = new Vector3[5];
        if (Physics.Raycast(transform.position, transform.forward, out raycastHit)) {
            positions[0] = raycastHit.transform.localPosition;
        }
        Vector3 direction = Quaternion.Euler(0, -40, 0) * transform.forward;
        if (Physics.Raycast(transform.position, direction, out raycastHit)) {
            positions[1] = raycastHit.transform.localPosition;
        }
        direction = Quaternion.Euler(0, -20, 0) * transform.forward;
        if (Physics.Raycast(transform.position, direction, out raycastHit)) {
            positions[2] = raycastHit.transform.localPosition;
        }
        direction = Quaternion.Euler(0, 20, 0) * transform.forward;
        if (Physics.Raycast(transform.position, direction, out raycastHit)) {
            positions[3] = raycastHit.transform.localPosition;
        }
        direction = Quaternion.Euler(0, 40, 0) * transform.forward;
        if (Physics.Raycast(transform.position, direction, out raycastHit)) { 
            positions[4] = raycastHit.transform.localPosition;
        }
        return positions;
    }
    /*private void OnDrawGizmosSelected () {
        Debug.DrawRay(transform.position, transform.forward);
        Vector3 direction = Quaternion.Euler(0, -40, 0) * transform.forward;
        Debug.DrawRay(transform.position, direction);
        direction = Quaternion.Euler(0, -20, 0) * transform.forward;
        Debug.DrawRay(transform.position, direction);
        direction = Quaternion.Euler(0, 20, 0) * transform.forward;
        Debug.DrawRay(transform.position, direction);
        direction = Quaternion.Euler(0, 40, 0) * transform.forward;
        Debug.DrawRay(transform.position, direction);
    }*/
    public override void OnEpisodeBegin() {
        base.OnEpisodeBegin();
        float scaleX, scaleZ; //Ground Scale
        scaleX = Random.Range(10f, 45f);
        scaleZ = Random.Range(10f, 45f);
        RandomizePositionsScales(scaleX, scaleZ);
        DespawnObstacles(gameObjects);
        gameObjects = SpawnObstacles(scaleX, scaleZ);
    }
    public override void CollectObservations(VectorSensor sensor) {
        base.CollectObservations(sensor);
        //float distance = Vector3.Distance(targetTransform.localPosition, transform.localPosition);
        //sensor.AddObservation(distance);
        sensor.AddObservation(transform.localPosition);
        //sensor.AddObservation(transform.forward);
        //Vector3 direction = targetTransform.localPosition - transform.localPosition;
        //sensor.AddObservation(Mathf.Acos(Vector3.Dot(direction, transform.forward)/direction.magnitude));
        sensor.AddObservation(targetTransform.localPosition);
        //sensor.AddObservation(direction);
        /*Vector3[] positions = RayCast();
        foreach (Vector3 position in positions) {
            sensor.AddObservation(position);
        }*/
    }

    /*private void Update() {
        //Debug.Log("UPDATE");
        
       
        /*string raycastTarget = RayCast();
        if (raycastTarget == "target") {
            groundMeshRenderer.material = lookattarget;
            //sensor.AddObservation(1);
            AddReward(0.1f);
        }
        /*else if (raycastTarget != "wall") {
            //sensor.AddObservation();
            //SetReward(-0.36f);
        }
    }*/
    public override void OnActionReceived(ActionBuffers actions) {
        base.OnActionReceived(actions);
        //Debug.Log(actions.ContinuousActions[0]);
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        float rotateY = actions.ContinuousActions[2] * 180;
        float moveSpeed = 10f;

        transform.localRotation = Quaternion.Euler(new Vector3(0f, rotateY, 0f));
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
        //transform.position += transform.forward.normalized * Time.deltaTime * moveAmount * moveSpeed;
    }
    public override void Heuristic(in ActionBuffers actionsOut) {
        base.Heuristic(actionsOut);
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
        //continuousActions[2] = Input.GetAxis("Space");
    }
    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<Goal>(out Goal goal)) {
            SetReward(1f);
            groundMeshRenderer.material = winMaterial;
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall) && other.tag != "obstacle") {
            SetReward(-1f);
            groundMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
        if (other.tag == "obstacle") {
            SetReward(-1f);
            groundMeshRenderer.material = lookatobstacle;
            EndEpisode();
        }
    }
}

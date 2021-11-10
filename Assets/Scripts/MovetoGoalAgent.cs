using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MovetoGoalAgent : Agent {
    [SerializeField] private Transform targetTransform;
    [SerializeField] private MeshRenderer groundMeshRenderer;
    [SerializeField] private Material loseMaterial, winMaterial;
    public override void OnEpisodeBegin() {
        base.OnEpisodeBegin();
        transform.localPosition = new Vector3(Random.Range(-3.76f, 4.18f), 1.016f, Random.Range (-4.2f, 4.2f));
        targetTransform.localPosition = new Vector3(Random.Range(-4.2f, 4.2f), 1.014f, Random.Range(-4.2f, 4.2f));
    }
    public override void CollectObservations(VectorSensor sensor) {
        base.CollectObservations(sensor);
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions) {
        base.OnActionReceived(actions);
        Debug.Log(actions.ContinuousActions[0]);
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        float moveSpeed = 10f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }
    public override void Heuristic(in ActionBuffers actionsOut) {
        base.Heuristic(actionsOut);
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<Goal>(out Goal goal)) {
            SetReward(1f);
            groundMeshRenderer.material = winMaterial;
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall)) {
            SetReward(-1f);
            groundMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
    }

}

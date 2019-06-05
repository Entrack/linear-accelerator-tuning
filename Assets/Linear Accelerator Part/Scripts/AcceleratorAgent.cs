using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class AcceleratorAgent : Agent {

    public List<AcceleratorLens> lensList;
    private AcceleratorAcademy myAcademy;

    public override void InitializeAgent() {
        myAcademy = GameObject.Find("Academy").GetComponent<AcceleratorAcademy>();
    }

    public override void CollectObservations() {
        for (int i = 0; i < lensList.Count; i++) {
            AddVectorObs(lensList[i].GetRotation());
        }
	}

    public override void AgentAction(float[] vectorAction, string textAction) {
        for (int i = 0; i < vectorAction.Length; i++) {
            float angle = vectorAction[i];
            lensList[i].RotateLensEuler(angle);
        }
        CollectRewards();
	}

    public override void AgentReset() {
        Debug.Log("AgentReset");
        for (int i = 0; i < lensList.Count; i++) {
            lensList[i].ResetRotation();
        }
    }

    private void CollectRewards() {
        float rewardScaler = 0.1f;
        AddReward(-0.01f * rewardScaler);

        for (int i = 0; i < lensList.Count; i++) {
            float lensReward = 0f;
            if (lensList[i].wasHit) {
                lensReward = lensList[i].GetHitAccuracy();
                lensReward *= i;
                lensReward += lensReward * (lensList[i].isDestination ? 1 : 0);
                lensList[i].wasHit = false;
            }
            AddReward(lensReward  * rewardScaler);
            AddReward(-Math.Abs(lensList[i].GetRotation()) * Math.Abs(lensList[i].GetRotation()) * 0.2f * rewardScaler);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class AcceleratorDecision : Decision {

    public override float[] Decide (List<float> state, List<Texture2D> observation, float reward, bool done, List<float> memory)
    {
        throw new NotImplementedException();
    }

    public override List<float> MakeMemory (List<float> state, List<Texture2D> observation, float reward, bool done, List<float> memory)
    {
        throw new NotImplementedException();
    }
}

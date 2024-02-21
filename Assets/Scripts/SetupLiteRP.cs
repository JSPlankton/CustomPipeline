using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SetupLiteRP : MonoBehaviour
{
    public RenderPipelineAsset currentPipelineAsset;
    private void OnEnable()
    {
        GraphicsSettings.renderPipelineAsset = currentPipelineAsset;
    }

    private void OnValidate()
    {
        GraphicsSettings.renderPipelineAsset = currentPipelineAsset;
    }
}

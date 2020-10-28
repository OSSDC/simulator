/**
 * Copyright (c) 2020 LG Electronics, Inc.
 *
 * This software contains code licensed as described in LICENSE.
 *
 */

namespace Simulator.Sensors.Postprocessing
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.HighDefinition;

    [PostProcessOrder(1)]
    public class CameraRainFX : PostProcessPass<Rain>
    {
        private const string ShaderName = "Hidden/Shader/CameraRainFX";
        private Material material;
        protected override bool IsActive => material != null;

        protected override void DoSetup()
        {
            if (Shader.Find(ShaderName) != null)
            {
                material = new Material(Shader.Find(ShaderName));
            }
            else
            {
                Debug.LogError($"Unable to find shader {ShaderName}. Post Process Volume {nameof(CameraRainFX)} is unable to load.");
            }
        }

        protected override void DoCleanup()
        {
            CoreUtils.Destroy(material);
        }

        protected override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination, Rain data)
        {
            cmd.SetGlobalFloat("_Intensity", data.intensity);
            cmd.SetGlobalTexture("_InputTexture", source);
            cmd.SetGlobalFloat("_Size", data.size);

            HDUtils.DrawFullScreen(cmd, material, destination);
        }
    }
}
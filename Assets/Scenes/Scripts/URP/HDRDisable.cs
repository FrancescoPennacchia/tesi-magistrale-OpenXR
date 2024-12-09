using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HDRDisable : MonoBehaviour
{
    void Start()
    {
        // Ottieni l'asset URP attualmente in uso
        UniversalRenderPipelineAsset urpAsset = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;

        if (urpAsset != null)
        {
            // Disabilita HDR
            urpAsset.supportsHDR = false;
            //urpAsset.supportsTerrainHoles = false;

            // Oppure, per abilitarlo:
            // urpAsset.supportsHDR = true;
        }
        else
        {
            Debug.LogWarning("Nessun URP Asset trovato!");
        }
    }
}

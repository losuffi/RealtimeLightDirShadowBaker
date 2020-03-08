using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
[System.Serializable]
public class UShadowBakerConfig
{
    // [Range(0,5)]
    // public int Precision = 2;
}
[ExecuteInEditMode]
public class UShadowCubemapBaker : MonoBehaviour
{
    public ComputeShader ComputeCore;
    public UShadowBakerConfig Config;
    private Shader GetDepthShader;

    private Camera DepthCamera;
    // [UnityEngine.SerializeField]
    // private RenderTexture DepthRT;

    [UnityEngine.SerializeField]
    private Cubemap CubemapTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BakeRender();
    }

    private void InitBaker()
    {
        GameObject Go = new GameObject();
        DepthCamera = Go.AddComponent<Camera>();
        DepthCamera.enabled = false;
        DepthCamera.cullingMask = 1<<32 - 1;
        Go.hideFlags = HideFlags.HideAndDontSave;
        Vector3 Pos = DepthCamera.transform.position;
        DepthCamera.CopyFrom(Camera.main);
        DepthCamera.transform.position = Pos;
        DepthCamera.clearFlags = CameraClearFlags.SolidColor;
        DepthCamera.backgroundColor = Color.white;
        DepthCamera.fieldOfView = 45;
        DepthCamera.farClipPlane = 50;
        GetDepthShader = Shader.Find("Hidden/RenderDepth");
        if(GetDepthShader == null)
        {
            Debug.LogError("Dont find 'RenderDepth' shader! please keep project is complete!");
        }
        // DepthRT = new RenderTexture(32 << Config.Precision, 32 << Config.Precision, 0, RenderTextureFormat.R16);
        // DepthRT.enableRandomWrite = true;
        // DepthRT.filterMode = FilterMode.Bilinear;
        // DepthRT.Create();
    }
    [ContextMenu("Run")]
    public void BakeRender()
    {
        // if(ComputeCore == null)
        // {
        //     Debug.LogError("Please set a compute shader first!");
        //     return;
        // }
        if(DepthCamera == null)
        {
            InitBaker();
        }
        if(GetDepthShader == null)
        {
            return;
        }
        // if(DepthRT == null)
        // {
        //     return;
        // }
        if(CubemapTarget == null)
        {
            Debug.LogError("Target Cubemap Is Null please set cubemap target");
            return;
        }
        DepthCamera.transform.rotation = new Quaternion(0,0,0,1);
        // DepthCamera.targetTexture = DepthRT;
        DepthCamera.SetReplacementShader(GetDepthShader, "");
        DepthCamera.RenderToCubemap(CubemapTarget);
        CubemapTarget.Apply();
        // WriteToCubemap(DepthRT, CubemapTarget, CubemapFace.PositiveZ);
        // DepthCamera.transform.rotation = new Quaternion(0, Mathf.Sin(0.25f * Mathf.PI), 0, Mathf.Cos(0.25f * Mathf.PI));
        // DepthCamera.Render();
        // WriteToCubemap(DepthRT, CubemapTarget, CubemapFace.PositiveX);
        // DepthCamera.transform.rotation = new Quaternion(0, Mathf.Sin(0.5f * Mathf.PI), 0, Mathf.Cos(0.5f * Mathf.PI));
        // DepthCamera.Render();
        // WriteToCubemap(DepthRT, CubemapTarget, CubemapFace.NegativeZ);
        // DepthCamera.transform.rotation = new Quaternion(0, Mathf.Sin(0.75f * Mathf.PI), 0, Mathf.Cos(0.75f * Mathf.PI));
        // DepthCamera.Render();
        // WriteToCubemap(DepthRT, CubemapTarget, CubemapFace.NegativeX);
        // DepthCamera.transform.rotation = new Quaternion(Mathf.Sin(0.25f * Mathf.PI), 0, 0, Mathf.Cos(0.25f * Mathf.PI));
        // DepthCamera.Render();
        // WriteToCubemap(DepthRT, CubemapTarget, CubemapFace.PositiveY);
        // DepthCamera.transform.rotation = new Quaternion(Mathf.Sin(0.75f * Mathf.PI), 0, 0, Mathf.Cos(0.75f * Mathf.PI));
        // DepthCamera.Render();
        // WriteToCubemap(DepthRT, CubemapTarget, CubemapFace.NegativeY);
    }

    private void WriteToCubemap(RenderTexture renderTexture, Cubemap Target, CubemapFace face)
    {
        RenderTexture.active = renderTexture;
        Texture2D FaceTexture2D = new Texture2D(renderTexture.width,renderTexture.height, TextureFormat.ARGB32, 0, true);
        FaceTexture2D.ReadPixels(new Rect(0,0,renderTexture.width, renderTexture.height), 0, 0);
        FaceTexture2D.Apply();
        Target.SetPixels(FaceTexture2D.GetPixels(), face);
        Target.Apply();
        RenderTexture.active = null;
    }
    [ContextMenu("Reset")]
    public void ResetData()
    {
        DepthCamera = null;
        // DepthRT = null;
        GetDepthShader = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
[System.Serializable]
public class UShadowBakerConfig
{
    [Range(0,5)]
    public int Precision = 2;
}
[ExecuteInEditMode]
public class UShadowCubemapBaker : MonoBehaviour
{
    public ComputeShader ComputeCore;
    public UShadowBakerConfig Config;
    private Shader GetDepthShader;

    private Camera DepthCamera;
    [UnityEngine.SerializeField]
    private RenderTexture DepthRT;
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
        GetDepthShader = Shader.Find("Hidden/RenderDepth");
        if(GetDepthShader == null)
        {
            Debug.LogError("Dont find 'RenderDepth' shader! please keep project is complete!");
        }
        DepthRT = new RenderTexture(32 << Config.Precision, 32 << Config.Precision, 0, RenderTextureFormat.R16);
        DepthRT.enableRandomWrite = true;
        DepthRT.filterMode = FilterMode.Bilinear;
        DepthRT.Create();
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
        if(DepthRT == null)
        {
            return;
        }
        DepthCamera.transform.rotation = new Quaternion(0,0,0,1);
        DepthCamera.targetTexture = DepthRT;
        DepthCamera.SetReplacementShader(GetDepthShader, "");
        DepthCamera.Render();
    }
    [ContextMenu("Reset")]
    public void ResetData()
    {
        DepthCamera = null;
        DepthRT = null;
        GetDepthShader = null;
    }
}

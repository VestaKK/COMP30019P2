Shader "PUNKSOULS/Cel"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,0)

        _Albedo ("Texture", 2D) = "white" {}

        _NormalMap("Normal Map", 2D) = "bump" {}
        _NormalIntensity("Normal Intensity", Range(0,1)) = 1

        [HDR]
            _AmbientLight("Ambient Color", Color) = (0,0,0,0)
        
        [HDR]
            _SpecularColor("Specular Color", Color) = (1,1,1,1)
            _Gloss("Glossiness", Range(0, 1)) = 1
        
        [HDR]
            _RimColor("Rim Color", Color) = (1,1,1,1)
            _RimWidth("Rim Witdh", Range(0, 1)) = 0.716
            _RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags{ "Queue" = "Geometry"  "RenderType" = "Opaque"}

        // BASE PASS (Directional light only (and shadows))
        Pass
        {
            Tags
            {
                "LightMode" = "ForwardBase"
            }

            Cull Back

            CGPROGRAM
            #define BASE_PASS
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "Cel.cginc"
            
            ENDCG
        }

        // ADD PASS (Other lighting types)
        Pass
        {
            Tags
            {
                "LightMode" = "ForwardAdd"
            }

            Blend One One // 1*src + 1*dst
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdadd

            // Cel Shader
            #include "Cel.cginc"

            ENDCG
        }
    UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}

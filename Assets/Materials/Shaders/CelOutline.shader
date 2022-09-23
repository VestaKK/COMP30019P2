Shader "PUNKSOULS/CelOutline"
{
    Properties
    {
        _Color("Color", Color) = (0,0,0,0)

        _Albedo("Texture", 2D) = "white" {}

        _NormalMap("Normal Map", 2D) = "bump" {}
        _NormalIntensity("Normal Intensity", Range(-1,1)) = 1

        [HDR]
            _AmbientLight("Ambient Color", Color) = (0,0,0,0)

        [HDR]
            _SpecularColor("Specular Color", Color) = (1,1,1,1)
            _Gloss("Glossiness", Range(0, 1)) = 1

        [HDR]
            _RimColor("Rim Color", Color) = (1,1,1,1)
            _RimWidth("Rim Witdh", Range(0, 1)) = 0.716
            _RimThreshold("Rim Threshold", Range(0, 1)) = 0.1


        _OutlineColor("Outline color", Color) = (1,0,0,0.5)
        _OutlineWidth("Outlines width", Range(0.0, 2.0)) = 0.15

        _Angle("Switch shader on angle", Range(0.0, 180.0)) = 89
    }
        SubShader
        {
            Tags{ "Queue" = "Transparent"  "RenderType" = "Opaque"}

            // Outline Pass
            Pass
            {
                Tags
                {
                "IgnoreProjector" = "True"
                }

            ZWrite Off
            Cull Back

            CGPROGRAM

            #include "UnityCG.cginc"   

            struct MeshData {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
            };

            uniform float4 _OutlineColor;
            uniform float _OutlineWidth;
            uniform float4 _Color;
            uniform float _Angle;

            struct v2f {
                float4 pos : SV_POSITION;
            };

            #pragma vertex vert
            #pragma fragment frag

            v2f vert(MeshData v) {

                float3 scaleDir = normalize(v.vertex.xyz - float4(0,0,0,1));

                if (degrees(acos(dot(scaleDir.xyz, v.normal.xyz))) > _Angle) {
                    v.vertex.xyz += normalize(v.normal.xyz) * _OutlineWidth;
                }
                else {
                    v.vertex.xyz += scaleDir * _OutlineWidth;
                }

                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(v2f i) : COLOR{
                return _OutlineColor;
            }
            ENDCG

        }

            // Cel BASE PASS (Directional light only (and shadows))
            Pass
            {
                Tags
                {
                    "LightMode" = "ForwardBase"
                }
                
                Cull Back
                
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fwdbase

                // Cel Shader
                #include "Cel.cginc"

                ENDCG
            }

            // Cel ADD PASS (Other lighting types)
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
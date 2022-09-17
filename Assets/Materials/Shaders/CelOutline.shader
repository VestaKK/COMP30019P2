Shader "PUNKSOULS/CelOutline"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)

        [HDR]
        _AmbientLight("Ambient Color", Color) = (1,1,1,1)
        _SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
        _Glossiness("Glossiness", Float) = 32
        _RimColor("Rim Color", Color) = (1,1,1,1)
        _RimWidth("Rim Witdh", Range(0, 1)) = 0.716
        _RimThreshold("Rim Threshold", Range(0, 1)) = 0.1

        _OutlineColor("Outline color", Color) = (1,0,0,0.5)
        _OutlineWidth("Outlines width", Range(0.0, 2.0)) = 0.15
        _Angle("Switch shader on angle", Range(0.0, 180.0)) = 89
    }
        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
            }
            Pass
            {
                Tags
                {
                "IgnoreProjector" = "True"
                }

                // Blend SrcAlpha OneMinusSrcAlpha
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
            // Cel Shader
            Pass
            {
                Tags
                {
                    "LightMode" = "ForwardBase"
                    "PassFlags" = "OnlyDirectional"
                }

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fwdbase

                #include "UnityCG.cginc"
                // Gives us main directional light in scene
                #include "Lighting.cginc"
                // Macros for shadow mapping
                #include "AutoLight.cginc"

                struct MeshData
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;

                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float3 worldNormal : NORMAL;
                    float3 viewDir : TEXCOORD1;
                    SHADOW_COORDS(2)
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;

                v2f vert(MeshData v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    o.viewDir = WorldSpaceViewDir(v.vertex);
                    TRANSFER_SHADOW(o)
                    return o;
                }

                float4 _Color;
                float4 _AmbientLight;
                float _Glossiness;
                float4 _SpecularColor;
                float4 _RimColor;
                float _RimWidth;
                float _RimThreshold;

                fixed4 frag(v2f i) : SV_Target
                {
                    float3 N = normalize(i.worldNormal);

                    // _WorldSpaceLightPos0 comes from "Lighting.cginc"
                    // Its a normalized direction vector that points to the direction
                    // opposite of the main light relative to the mesh
                    float3 V = normalize(i.viewDir);
                    float3 L = _WorldSpaceLightPos0;
                    float3 H = normalize(V + L);
                    float3 NdotH = dot(N, H);
                    float NdotL = dot(N, L);

                    float4 rimDot = 1 - dot(N, V);
                    float rimIntensity = smoothstep(_RimWidth - 0.01, _RimWidth - 0.01 + 0.01, rimDot * pow(NdotL, _RimThreshold));
                    float rimLight = _RimColor * rimIntensity;

                    // Basically a lerp between 0 and 0.01, giving a value
                    // between 0 and 1. If NdotL is larger than the lower
                    // or upper bound, smoothstep() returns 0 and 1 respectively

                    float shadow = SHADOW_ATTENUATION(i);
                    float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);
                    float specularIntensity = smoothstep(0.005, 0.01,
                                                            pow(NdotH * lightIntensity, _Glossiness * _Glossiness));

                    float4 DiffuseLight = lightIntensity * _LightColor0;

                    // TODO: replace UNITY ambient light with Ambient for each room
                    return _Color * (UNITY_LIGHTMODEL_AMBIENT + DiffuseLight + specularIntensity * _SpecularColor + rimLight);
                }
                ENDCG
            }
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
        }
}
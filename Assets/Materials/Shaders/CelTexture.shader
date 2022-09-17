Shader "PUNKSOULS/CelTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR]
        _AmbientLight("Ambient Color", Color) = (1,1,1,1)
        _SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
        _Glossiness("Glossiness", Float) = 32
        _RimColor("Rim Color", Color) = (1,1,1,1)
        _RimWidth("Rim Witdh", Range(0, 1)) = 0.716
        _RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
    }
    SubShader
    {
        // Cel Shader
        Pass
        {
            Tags
            {
                "LightMode" = "ForwardBase"
                "PassFlags" = "OnlyDirectional"
                "Queue" = "Opaque"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
            // Gives us main directional light in scene
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : NORMAL;

                // Just for shadows - this is done on Unity's side
                float3 viewDir : TEXCOORD1;
                SHADOW_COORDS(2)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (MeshData v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                TRANSFER_SHADOW(o)
                return o;
            }

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

                // Texture color is sampled
                fixed4 _OutColor = tex2D(_MainTex, i.uv);
                
                // TODO: replace UNITY ambient light with Ambient for each room
                return _OutColor * (_AmbientLight + DiffuseLight + specularIntensity * _SpecularColor + rimLight);
            }
            ENDCG
        }
    UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}

#include "UnityCG.cginc"
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
    float3 viewDir : TEXCOORD1;
    float3 wPos : TEXCOORD2;
    LIGHTING_COORDS(3,4)
};

sampler2D _MainTex;
float4 _MainTex_ST;

v2f vert(MeshData v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);             // mul(UNITY_MATRIX_MVP, v.vertex)
    o.worldNormal = UnityObjectToWorldNormal(v.normal); // Saw this in class no need to use the long version
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);               // (v.uv * _MainTex_ST.xy + _MainTex_ST.zw) 
    o.wPos = mul(unity_ObjectToWorld, v.vertex);        
    TRANSFER_VERTEX_TO_FRAGMENT(o);                     // Let Unity do shadows for us
    TRANSFER_SHADOW(o);
    return o;
}

float4 _Color;
float4 _AmbientLight;

float _Gloss;
float4 _SpecularColor;
float4 _RimColor;
float _RimWidth;
float _RimThreshold;

fixed4 frag(v2f i) : SV_Target
{
    // Based on Blinn-Phong
    float3 N = normalize(i.worldNormal);
    float3 L = normalize(UnityWorldSpaceLightDir(i.wPos)); 
    float3 V = normalize( _WorldSpaceCameraPos - i.wPos);
    float3 H = normalize(V + L);
    float fAtt = LIGHT_ATTENUATION(i);
    // Saturate clamps input between 0 - 1
    float NdotH = saturate(dot(N, H));
    float NdotL = saturate(dot(N, L));

    // smoothstep() -> Basically a lerp between 0 and 0.01, 
    // giving a value between 0 and 1. If NdotL is smaller than 
    // the lower bound or larger than the upper bound, 
    // smoothstep() returns 0 and 1 respectively
    float4 AmbientLight = fAtt * _AmbientLight;
    
    float4 shadow = SHADOW_ATTENUATION(i);

    float lightIntensity = smoothstep(0, 0.1, NdotL * shadow);
    float4 DiffuseLight = (fAtt * lightIntensity) * _LightColor0;

    float exponentialGloss = exp2(_Gloss * 11) + 2;
    float specularIntensity = smoothstep(0.005, 0.01, pow(NdotH * lightIntensity, exponentialGloss));
    float4 SpecularLight = (fAtt * specularIntensity * _SpecularColor);

    // What I'm assuming is Fresnel Stuff
    float4 rimDot = 1 - dot(N, V);
    float rimIntensity = smoothstep(_RimWidth - 0.01, _RimWidth - 0.01 + 0.01, rimDot * pow(NdotL, _RimThreshold));
    float RimColor = fAtt * _RimColor * rimIntensity;

    // Texture color is sampled
    fixed4 _TexColor = tex2D(_MainTex, i.uv);
    float4 _OutColor = _Color * _TexColor;

    // The return changes based on the material normally, but since
    // this is a cel shader maybe it's stylistically better
    return  _OutColor * (AmbientLight + DiffuseLight + SpecularLight + RimColor);
}
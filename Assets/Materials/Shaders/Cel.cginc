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
    float3 wPos : TEXCOORD1;
    LIGHTING_COORDS(2,3)
};

sampler2D _MainTex;
float4 _MainTex_ST;

v2f vert(MeshData v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);             // mul(UNITY_MATRIX_MVP, v.vertex)
    o.worldNormal = UnityObjectToWorldNormal(v.normal); // Saw this in workshop no need to use the long version
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);               // (v.uv * _MainTex_ST.xy + _MainTex_ST.zw) 
    o.wPos = mul(unity_ObjectToWorld, v.vertex);        // World Position
    TRANSFER_VERTEX_TO_FRAGMENT(o);                     // Unity gives us information on shadow and light positions (Actually Lighting LMAO)
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
    // TODOL: Normal Maps, possibly height maps

    // Surface Vectors
    float3 N = normalize(i.worldNormal);
    float3 L = normalize(UnityWorldSpaceLightDir(i.wPos)); 
    float3 V = normalize(_WorldSpaceCameraPos - i.wPos);

    // Half Vector for Blinn-Phong
    float3 H = normalize(V + L);

    // Unity macros from Lighting.cginc and Autolight.cginc
    float4 shadow = SHADOW_ATTENUATION(i);
    float fAtt = LIGHT_ATTENUATION(i);

    // This is basically it you can stop reading now
    float NdotL = saturate(dot(N, L));
    float NdotH = saturate(dot(N, H));

    // Helps reduce aliasing along light borders
    float AntiAliasingStep = fwidth(NdotL);
    
    // Albedo Color Calculation
    fixed4 _TexColor = tex2D(_MainTex, i.uv);
    float4 _OutColor = _Color * _TexColor;

    // Ambient Light
    float4 AmbientLight = _AmbientLight;

    // Diffuse Light
    float lightIntensity = smoothstep(0, AntiAliasingStep, NdotL);
    float4 DiffuseLight = lightIntensity * _LightColor0 * _OutColor;

    // Specular Light
    float exponentialGloss = exp2(_Gloss * 11) + 2;
    float specularIntensity = smoothstep(0.005, 0.01, pow(NdotH * lightIntensity, exponentialGloss));
    float4 SpecularLight = specularIntensity * _SpecularColor;
    
    // (Preference modification for specular light):
    // float DiffuseAvg = (DiffuseLight.r + DiffuseLight.g + DiffuseLight.b) / 3; 
    // float4 SpecularLight = fAtt * specularIntensity * _SpecularColor * DiffuseAvg;

    // Rim Effect
    float4 rimDot = 1 - dot(N, V);
    float rimIntensity = smoothstep(_RimWidth - 0.01, _RimWidth - 0.01 + 0.01, rimDot * pow(NdotL, _RimThreshold));
    float RimColor = rimIntensity * _RimColor;

    return (AmbientLight + DiffuseLight + SpecularLight + RimColor) * shadow * fAtt;
}
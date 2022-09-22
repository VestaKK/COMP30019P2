#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"

struct MeshData
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
    float3 normal : NORMAL;
    float4 tangent : TANGENT; // xyz = tangent direction, w = tangent sign for flipped UVs
};

struct v2f
{
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
    float3 normal : TEXCOORD1;
    float3 tangent : TEXCOORD2;
    float3 binormal : TEXCOORD3;
    float3 wPos : TEXCOORD4;
    LIGHTING_COORDS(5, 6)
    
};

sampler2D _Albedo;
float4 _Albedo_ST;

sampler2D _NormalMap;
float4 _NormalMap_ST;
float _NormalIntensity;

v2f vert(MeshData v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);                 // mul(UNITY_MATRIX_MVP, v.vertex)
    o.uv = TRANSFORM_TEX(v.uv, _Albedo);                    // (v.uv * _MainTex_ST.xy + _MainTex_ST.zw) 
    o.normal = UnityObjectToWorldNormal(v.normal);          // Saw this in workshop no need to use the long version
    o.tangent = UnityObjectToWorldDir(v.tangent.xyz);       // tangent to surface
    o.binormal = cross(o.normal, o.tangent);                // perpendicular to tangent and normal
    o.binormal *= v.tangent.w * unity_WorldTransformParams.w; // for handling flipping/mirroring textures
    o.wPos = mul(unity_ObjectToWorld, v.vertex);            // World Position
    TRANSFER_VERTEX_TO_FRAGMENT(o);                         // Unity gives us information on shadow and light positions (Actually Lighting LMAO)
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

    // Albedo Color Calculation
    fixed4 _TexColor = tex2D(_Albedo, i.uv);
    float4 _OutColor = _Color * _TexColor;

    // Take out Normal from Normal Map (Normals are packed weirdly because of compression)
    fixed3 NormalMapNormal = UnpackNormal(tex2D(_NormalMap, i.uv));

    // Allows us to adjust how much the normal map normal affects the look of the material
    NormalMapNormal = normalize(lerp(float3(0, 0, 1), NormalMapNormal, _NormalIntensity));
    // essentially multiply 
    float3x3 tangentToWorld = {
        i.tangent.x, i.binormal.x, i.normal.x,
        i.tangent.y, i.binormal.y, i.normal.y,
        i.tangent.z, i.binormal.z, i.normal.z
    };

    // N = convert normal from tangent space to world space
    // L = surface to Light direction
    // V = surface to camera position direction
    // H = Half Vector for Blinn-Phong
    float3 N = mul(tangentToWorld, NormalMapNormal);
    float3 L = normalize(UnityWorldSpaceLightDir(i.wPos)); 
    float3 V = normalize(_WorldSpaceCameraPos - i.wPos);
    float3 H = normalize(V + L);

    // Unity macros from Lighting.cginc and Autolight.cginc
    float4 shadow = SHADOW_ATTENUATION(i);
    float fAtt = LIGHT_ATTENUATION(i);

    // This is basically it you can stop reading now
    float NdotL = saturate(dot(N, L));
    float NdotH = saturate(dot(N, H));

    // Helps reduce aliasing along light borders
    float AntiAliasingStep = fwidth(NdotL);

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
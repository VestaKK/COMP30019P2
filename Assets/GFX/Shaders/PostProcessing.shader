Shader "PUNKSOULS/PostProcessing"
{
    Properties
    {
        [HideInInspector] _MainTex("Texture", 2D) = "white" {}
        _SampleX("X-axis Resolution", float) = 1920
        _SampleY("Y-axis Resolution", float) = 1080
        _Amount("Chromatic Abberation Intensity", Range(0,0.02)) = 0

    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 depth : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_DEPTH(o.depth);
                return o;
            }

            sampler2D _CameraDepthTexture;
            sampler2D _MainTex;
            float _SampleX;
            float _SampleY;
            float _Amount;

            float2 pixelate(float2 uv, const float pixelSampleX, const float pixelSampleY, const float depth) {
                float pixelX = 1 / (pixelSampleX );
                float pixelY = 1 / (pixelSampleY);
                return half2((int)(uv.x / pixelX) * pixelX, (int)(uv.y / pixelY) * pixelY);
            }
            
            float remap(float origFrom, float origTo, float targetFrom, float targetTo, float value) {
                float rel = (value - origFrom) / (origTo - origFrom);
                return lerp(targetFrom, targetTo, rel);
            }


            fixed4 frag(v2f i) : SV_Target
            {
                float depth = tex2D(_CameraDepthTexture, i.uv).r;

                depth = clamp(0, 0.3, Linear01Depth(depth));

                _Amount = _Amount * depth;
                float2 UV = pixelate(i.uv, _SampleX, _SampleY, depth);
                float colR = tex2D(_MainTex, float2(UV.x - _Amount, UV.y - _Amount)).r;
                float colG = tex2D(_MainTex, UV).g;
                float colB = tex2D(_MainTex, float2(UV.x + _Amount, UV.y + _Amount)).b;
                fixed4 col = fixed4(colR, colG, colB, 1);
                
                return col;
            }
            ENDCG
        }
    }
}

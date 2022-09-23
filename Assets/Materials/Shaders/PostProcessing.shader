Shader "PUNKSOULS/PostProcessing"
{
    Properties
    {
        [HideInInspector] _MainTex("Texture", 2D) = "white" {}
        _SampleX ("X-axis Resolution", float) = 1920
        _SampleY ("Y-axis Resolution", float) = 1080
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
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
                return o;
            }

            sampler2D _MainTex;
            float _SampleX;
            float _SampleY;

            float2 pixelate(float2 uv, const float pixelSampleX, const float pixelSampleY) {
                float pixelX = 1 / pixelSampleX;
                float pixelY = 1 / pixelSampleY;
                return half2((int)(uv.x / pixelX) * pixelX, (int)(uv.y / pixelY) * pixelY);
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, pixelate(i.uv, _SampleX, _SampleY));
                // just invert the colors
            return col;
            }
            ENDCG
        }
    }
}

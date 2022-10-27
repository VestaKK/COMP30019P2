Shader "PUNKSOULS/PostProcessing"
{
    Properties
    {
        [HideInInspector] _MainTex("Texture", 2D) = "white" {}
        _SampleX("X-axis Resolution", float) = 1920
        _SampleY("Y-axis Resolution", float) = 1080
        _Amount("Chromatic Abberation Intensity", Range(0,0.02)) = 0
        _Rate("Sin Wave Rate", float) = 1
        _Amplitude("Sin Wave Amplitude", float) = 0.3
        _Tightness("Lens Tightness", float) = 10
        _Intensity("Lens Intensity", float) = 10

    }
    SubShader
    {
        Pass
        {
            Tags { "RenderType" = "Transperant" }
            LOD 100

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

            sampler2D _CameraDepthTexture;
            sampler2D _MainTex;
            float _SampleX;
            float _SampleY;
            float _Amount;
            float _Rate;
            float _Amplitude;
            float _Tightness;
            float _Intensity;

            float2 pixelate(float2 uv, const float pixelSampleX, const float pixelSampleY) {
                float pixelX = 1 / pixelSampleX;
                float pixelY = 1 / pixelSampleY;
                return half2((int)(uv.x / pixelX) * pixelX, (int)(uv.y / pixelY) * pixelY);
            }

            fixed4 frag(v2f i) : SV_Target
            {

                // Lens Distortion
                float2 uvCentered = i.uv * 2 - 1;
                float2 distortionMagnitude = sqrt(uvCentered.x * uvCentered.x * uvCentered.y * uvCentered.y) * (_Amount / 0.02);
                float2 smoothDistortionMagnitude = pow(distortionMagnitude, _Tightness);
                float2 uvDistorted = i.uv + uvCentered * smoothDistortionMagnitude * _Intensity;

                // Pixelation
                float2 UV = pixelate(uvDistorted, _SampleX, _SampleY);
                
                if (uvDistorted.x < 0 || uvDistorted.x > 1)
                    return float4(0, 0, 0, 0);

                if (uvDistorted.y < 0 || uvDistorted.y > 1)
                    return float4(0, 0, 0, 0);

                _Amount = _Amount - _Amount * _Amplitude * sin(_Time * _Rate);

                // Chromatic Abberation
                float _AmountX = _Amount * pow(distortionMagnitude.x, 1.2);
                float _AmountY = _Amount * pow(distortionMagnitude.y, 1.2);
                
                float2 uvRed = float2(UV.x - _AmountX, UV.y - _AmountY);
                float2 uvGreen = float2(UV);
                float2 uvBlue = float2(UV.x + _AmountX, UV.y + _AmountY);

                float colR = tex2D(_MainTex, uvRed).r;
                float colG = tex2D(_MainTex, uvGreen).g;
                float colB = tex2D(_MainTex, uvBlue).b;
                fixed4 col = fixed4(colR, colG, colB, 1);
                
                return col;
            }
            ENDCG
        }
    }
}

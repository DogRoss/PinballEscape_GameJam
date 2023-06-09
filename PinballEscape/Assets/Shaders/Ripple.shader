Shader "Custom/Ripple"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RippleOrigin("Ripple Origin", Vector) = (0,0,0,0)
        _RippleDensity("Ripple Density", Float) = 0
        _Frequency("Frequency", Float) = 0
        _Amplitude("Amplitude", Float) = 0
        _EffectRadius("Effect Radius", FLoat) = 0
        _EdgeBlend("Edge Blend", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                //float3 localPos : TEXCOORD1;
                float3 vData : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _RippleOrigin;
            float _RippleDensity;
            float _Frequency;
            float _Amplitude;
            float _EffectRadius;
            float _EdgeBlend;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                //o.localPos = v.vertex.xyz;
                float3 sub = _RippleOrigin - v.vertex.xyz;
                float _Length = length(sub);
                float _DensityMul = mul(_RippleDensity, 6.28);
                float _DenLenMul = mul(_Length, _DensityMul);

                float _FreqMul = mul(_Frequency, -6.28);
                float _FreqTimeMul = mul(_FreqMul, _Time.y);

                //combines _DenLenMul and _FreqTimeMul
                float _DLM_FTM = _DenLenMul + _FreqTimeMul;
                //sin of _DLM_FTM
                float _DF_Sin = sin(_DLM_FTM);
                float _Amp = mul(_Amplitude, _DF_Sin);

                // - FadeOut
                float _RadiusBlendMul = mul(_EffectRadius, _EdgeBlend);
                float _FadeSmooth = smoothstep(_EffectRadius, _RadiusBlendMul, _Length);

                float _FadeAmpMul = mul(_FadeSmooth, _Amp);
                float3 _RippleNormal = mul(v.normal.xyz, _FadeAmpMul);
                //o.vertex = v.vertex + float4(_RippleNormal.xyz, 1);
                o.vertex += float4(_RippleNormal.xyz, 1);


                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}

Shader "Custom/VertexRipple"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _UpperFeather("Upper Feather", Float) = 1
        _LowerFeather("Lower Feather", Float) = 1

        _RippleUVPositionX("Ripple UV Position X", Float) = 0
        _RippleUVPositionY("Ripple UV Position Y", Float) = 0

        _RippleTimer("Ripple Timer", Float) = 0
        _RippleFactor("Ripple Factor", Float) = 0
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 rippleUV: TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _UpperFeather;
            float _LowerFeather;

            float _RippleUVPositionX;
            float _RippleUVPositionY;

            float _RippleTimer;
            float _RippleFactor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.rippleUV = float2(_RippleUVPositionX, _RippleUVPositionY);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float2 addVU = (o.rippleUV) * 2 - 1;
                float2 newVU = (o.uv) * 2 - 1;
                newVU -= addVU;

                float timer = frac(_Time.y);
                float len = length(newVU);
                float upperRing = smoothstep(len + _UpperFeather, len - _LowerFeather, timer);
                float inverseRing = 1 - upperRing;
                    
                o.vertex.y += upperRing * inverseRing;      

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                /*float2 addVU = (i.rippleUV) * 2 - 1;
                float2 newVU = (i.uv) * 2 - 1;
                newVU -= addVU;

                float timer = frac(_Time.y);
                float len = length(newVU);
                float upperRing = smoothstep(len + _UpperFeather, len - _LowerFeather, timer);
                float inverseRing = 1 - upperRing;
                float finalRing = upperRing * inverseRing;

                float2 finalUV = i.uv + newVU * finalRing;

                return fixed4(finalUV.xy, 0, 1);*/
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}

Shader "Custom/Distortion"
{
    Properties
    {
        _DistortionTex("Distortion Texture", 2D) = "white" {}
        _Filter("Filter", 2D) = "white" {}
        _DistortionAmount("Distortion", Range(0.0, 1.0)) = 0.0
        _DistortionSpeed("DistortionSpeed", Float) = 1.0
        _TintColor("Tint", Color) = (1,1,1,1)
    }
        SubShader
        {
            //Tags { "RenderType"="Transparent" }
            Tags { "Queue" = "Transparent" }
            LOD 100

            GrabPass { "_ScreenTexture" }

            Pass
            {
                Blend SrcAlpha OneMinusSrcAlpha

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
                    float4 grabPos : TEXCOORD1;
                    float2 saveUV : TEXCOORD2;
                };

                fixed4 _TintColor;

                sampler2D _DistortionTex;
                float4 _DistortionTex_ST;
                float _DistortionAmount;
                float _DistortionSpeed;

                sampler2D _Filter;
                float4 _Filter_ST;

                sampler2D _ScreenTexture;
                float4 _ScreenTexture_ST;

                float filt;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _ScreenTexture);

                    float noise = tex2Dlod(_DistortionTex, float4(v.uv, 0, 0)).rgb;
                    filt = tex2Dlod(_Filter, float4(v.uv, 0, 0)).rgb;

                    o.saveUV = v.uv;

                    o.uv.x += sin(noise * _Time.y * _DistortionSpeed) * filt * _DistortionAmount;
                    o.uv.y += cos(noise * _Time.y * _DistortionSpeed) * filt * _DistortionAmount;
                    o.grabPos = ComputeGrabScreenPos(o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float4 disp = tex2D(_DistortionTex, i.uv);
                    disp = ((disp * 2) - 1) * _DistortionAmount;
                    filt = tex2Dlod(_Filter, float4(i.saveUV.xy, 0, 0)).rgb;
                    disp *= filt;

                    // sample the texture
                    //fixed4 col = tex2Dproj(_ScreenTexture, i.grabPos + disp);
                    fixed4 col = tex2Dproj(_ScreenTexture, i.grabPos);
                    col.w = 1;
                    return col * _TintColor;
                }
                ENDCG
            }
        }
}

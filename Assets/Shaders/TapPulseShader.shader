Shader "Custom/TapPulseShader"
{
    Properties
    {
        _MainTex ("Sprite", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _PulseCenter ("Pulse Center (UV)", Vector) = (0.5, 0.5, 0, 0)
        _PulseWidth ("Pulse Width", Float) = 0.0
        _PulseSoftness ("Pulse Softness", Float) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float4 _PulseCenter;
            float _PulseWidth;
            float _PulseSoftness;

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

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float dist = abs(uv.x - _PulseCenter.x);
                float alpha = 1.0 - smoothstep(_PulseWidth - _PulseSoftness, _PulseWidth, dist);

                fixed4 texColor = tex2D(_MainTex, uv) * _Color;
                texColor.a *= alpha;

                return texColor;
            }
            ENDCG
        }
    }
}
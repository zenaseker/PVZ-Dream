Shader "UI/UI_DissolveMask"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        [Header(Dissolve Settings)]
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _DissolveThreshold("Dissolve Threshold", Range(0, 1)) = 0.5
        _EdgeWidth("Edge Width", Range(0, 0.2)) = 0.1
        _EdgeColor("Edge Color", Color) = (1,1,1,1)
        _EdgeGlow("Edge Glow", Range(0, 10)) = 2
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float _DissolveThreshold;
            float _EdgeWidth;
            fixed4 _EdgeColor;
            float _EdgeGlow;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // 主纹理采样
                half4 color = tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd;
                color *= IN.color;
                
                // 噪声纹理采样
                float noise = tex2D(_NoiseTex, IN.texcoord).r;
                
                // 溶解计算
                float dissolve = step(_DissolveThreshold, noise);
                float edge = smoothstep(_DissolveThreshold, _DissolveThreshold + _EdgeWidth, noise);
                
                // 边缘发光效果
                float edgeGlow = (1.0 - edge) * _EdgeGlow;
                
                // 组合最终颜色
                half4 finalColor = color;
                finalColor.a *= dissolve;
                
                // 添加边缘效果
                finalColor.rgb = lerp(finalColor.rgb, _EdgeColor.rgb, edge);
                finalColor.rgb += _EdgeColor.rgb * edgeGlow;
                
                // UI矩形裁剪
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

                return finalColor;
            }
            ENDCG
        }
    }
}
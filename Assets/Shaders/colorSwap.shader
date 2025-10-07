Shader "Custom/ExactColorSwap_Multi"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _ColorCount("Color Count", Int) = 1
        [HideInInspector]_Padding("Padding", Float) = 0 // For alignment

        // Support up to 5 color swaps (expand as needed)
        _OriginalColor0("Original Color 0", Color) = (1,1,1,1)
        _TargetColor0("Target Color 0", Color) = (1,1,1,1)
        _OriginalColor1("Original Color 1", Color) = (1,1,1,1)
        _TargetColor1("Target Color 1", Color) = (1,1,1,1)
        _OriginalColor2("Original Color 2", Color) = (1,1,1,1)
        _TargetColor2("Target Color 2", Color) = (1,1,1,1)
        _OriginalColor3("Original Color 3", Color) = (1,1,1,1)
        _TargetColor3("Target Color 3", Color) = (1,1,1,1)
        _OriginalColor4("Original Color 4", Color) = (1,1,1,1)
        _TargetColor4("Target Color 4", Color) = (1,1,1,1)

        _Tolerance("Tolerance", Range(0, 0.1)) = 0.001  
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Cull Off
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            int _ColorCount;
            float4 _OriginalColor0;
            float4 _TargetColor0;
            float4 _OriginalColor1;
            float4 _TargetColor1;
            float4 _OriginalColor2;
            float4 _TargetColor2;
            float4 _OriginalColor3;
            float4 _TargetColor3;
            float4 _OriginalColor4;
            float4 _TargetColor4;
            float _Tolerance;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);

                if (col.a == 0)
                {
                    return half4(0, 0, 0, 0);
                }

                // Array emulation for up to 5 color swaps
                float4 originalColors[5] = {
                    _OriginalColor0, _OriginalColor1, _OriginalColor2, _OriginalColor3, _OriginalColor4
                };
                float4 targetColors[5] = {
                    _TargetColor0, _TargetColor1, _TargetColor2, _TargetColor3, _TargetColor4
                };

                for (int idx = 0; idx < _ColorCount; idx++)
                {
                    if (length(col - originalColors[idx]) < _Tolerance)
                    {
                        return half4(targetColors[idx].rgb, col.a);
                    }
                }

                return col;
            }

            ENDCG
        }
    }
}
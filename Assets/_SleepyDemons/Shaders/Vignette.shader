Shader "Custom/Vignette"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Vignette Color", Color) = (0, 0, 1, 1)
        _Intensity ("Intensity", Range(0, 1)) = 0.5
        _Sharpness ("Sharpness", Range(0.01, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color;
            float _Intensity;
            float _Sharpness;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;
                float2 center = float2(0.5, 0.5);
                float dist = distance(uv, center);
                float calculatedIntensity = 1.0f - _Intensity;
                float calculatedSharpness = 1.0f - _Sharpness;
                float vignette = smoothstep(calculatedIntensity, calculatedIntensity + calculatedSharpness, dist);

                fixed4 col = tex2D(_MainTex, uv);
                col.rgb = lerp(col.rgb, _Color.rgb, vignette);
                return col;
            }
            ENDCG
        }
    }
}

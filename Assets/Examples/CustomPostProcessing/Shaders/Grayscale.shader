Shader "Hidden/PostProcess/Grayscale"
{
    Properties
    {
        [HideInInspector] _MainTex("Base Map", 2D) = "white" {}
        [HideInInspector] _Blend("Blend", Range(0, 1)) = .5
    }

    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

    TEXTURE2D(_MainTex);
    SAMPLER(sampler_MainTex);
    float _Blend;

    struct Attributes
    {
        float4 positionOS : POSITION;
        float2 uv : TEXCOORD0;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };
 
    struct Varyings
    {
        float2 uv : TEXCOORD0;       
        float4 positionCS : SV_POSITION;
 
        UNITY_VERTEX_INPUT_INSTANCE_ID
        UNITY_VERTEX_OUTPUT_STEREO
    };

    Varyings Vert(Attributes IN)
    {
        Varyings OUT;
        OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
        OUT.uv = IN.uv;
        return OUT;
    }

    half4 Frag(Varyings IN) : SV_Target
    {
        half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
        float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
        color.rgb = lerp(color.rgb, luminance.xxx, _Blend.xxx);
        return color;
    }
    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            Name "Grayscale"
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            ENDHLSL
        }
    }
    Fallback Off
}

Shader "Hidden/PostProcess/Awake"
{
    Properties
    {
        [HideInInspector] _MainTex("Base Map", 2D) = "white" {}
        [HideInInspector] _Progress("Progress", Range(0, 1)) = .5
        [HideInInspector] _ArchHeight ("Arch Height", Range (0, .5)) = .2
        [HideInInspector] _BlurSize ("Blur Size", Float) = 1
    }

    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

    TEXTURE2D(_MainTex);
    SAMPLER(sampler_MainTex);
    float4 _MainTex_TexelSize;
    float _Progress;
    float _ArchHeight;
    float _BlurSize;

    struct Attributes
    {
        float4 positionOS : POSITION;
        float2 uv : TEXCOORD0;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct Varyings
    {
        float2 uv[5] : TEXCOORD0;
        float4 positionCS : SV_POSITION;

        UNITY_VERTEX_INPUT_INSTANCE_ID
        UNITY_VERTEX_OUTPUT_STEREO
    };

    Varyings Vert(Attributes IN)
    {
        Varyings OUT;
        OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
        OUT.uv[0] = IN.uv;
        return OUT;
    }

    half4 Frag(Varyings IN) : SV_Target
    {
        half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv[0]);

        // 上眼皮与下眼皮边界
        float upBorder = .5 + _Progress * (.5 + _ArchHeight);
        float downBorder = .5 - _Progress * (.5 + _ArchHeight);
        upBorder -= _ArchHeight * pow(IN.uv[0].x - .5, 2);
        downBorder += _ArchHeight * pow(IN.uv[0].x - .5, 2);

        // 可视区域
        float visibleV = (1 - step(upBorder, IN.uv[0].y)) * step(downBorder, IN.uv[0].y);
        color *= visibleV;
        color *= _Progress;

        return color;
    }

    Varyings VertBlurVertical(Attributes IN)
    {
        Varyings OUT;
        OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);

        // XXX_TexelSize (1 / width, 1 / height, width, height)
        OUT.uv[0] = IN.uv;
        OUT.uv[1] = IN.uv + float2(0, _MainTex_TexelSize.y * 1.0) * _BlurSize;
        OUT.uv[2] = IN.uv - float2(0, _MainTex_TexelSize.y * 1.0) * _BlurSize;
        OUT.uv[3] = IN.uv + float2(0, _MainTex_TexelSize.y * 2.0) * _BlurSize;
        OUT.uv[4] = IN.uv - float2(0, _MainTex_TexelSize.y * 2.0) * _BlurSize;

        return OUT;
    }

    Varyings VertBlurHorizontal(Attributes IN)
    {
        Varyings OUT;
        OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);

        OUT.uv[0] = IN.uv;
        OUT.uv[1] = IN.uv + float2(_MainTex_TexelSize.x * 1.0, 0) * _BlurSize;
        OUT.uv[2] = IN.uv - float2(_MainTex_TexelSize.x * 1.0, 0) * _BlurSize;
        OUT.uv[3] = IN.uv + float2(_MainTex_TexelSize.x * 2.0, 0) * _BlurSize;
        OUT.uv[4] = IN.uv - float2(_MainTex_TexelSize.x * 2.0, 0) * _BlurSize;

        return OUT;
    }

    half4 FragBlur(Varyings IN) : SV_TARGET
    {
        float weight[3] = {0.4026, 0.2442, 0.0545};
        half3 sum = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv[0]).rgb * weight[0];

        for (int it = 1; it < 3; it++)
        {
            sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv[it * 2 - 1]).rgb * weight[it];
            sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv[it * 2]).rgb * weight[it];
        }

        return half4(sum, 1);
    }
    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            Name "AWAKE"
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            ENDHLSL
        }
        
        Pass
        {
            Name "GAUSSIAN_BLUR_VERTICAL"
            HLSLPROGRAM
            #pragma vertex VertBlurVertical
            #pragma fragment FragBlur
            ENDHLSL
        }
        
        Pass
        {
            Name "GAUSSIAN_BLUR_HORIZONTAL"
            HLSLPROGRAM
            #pragma vertex VertBlurHorizontal
            #pragma fragment FragBlur
            ENDHLSL
        }
        
    }
    Fallback Off
}
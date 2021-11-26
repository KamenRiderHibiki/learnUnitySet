Shader "Graph/Point Surface"
{
    Properties
    {
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
    }
    //子着色器
    SubShader
    {
        CGPROGRAM
        // pragma一词来自希腊语，指的是一项行动或需要完成的事情
        // 基于物理的标准光线模型, 在所有光线类型中启用阴影
        #pragma surface surf Standard fullforwardshadows

        // 将着色器模型设为 3.0 target, 获得更好的光线效果。
        // 该指令为着色器的 target 级别和质量设置了最小值
        #pragma target 3.0

        struct Input
        {
            float3 worldPos;
        };

        float _Smoothness;

        void surf (Input IN, inout SurfaceOutputStandard outSurface)
        {
            outSurface.Albedo.rgb = saturate(IN.worldPos.xyz * 0.5 + 0.5);
            outSurface.Smoothness = _Smoothness;
        }
        ENDCG
    }
    //向标准的漫反射着色器添加一个后备
    FallBack "Diffuse"
}

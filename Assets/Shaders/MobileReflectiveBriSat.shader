Shader "Mobile/Reflective Brightness Saturation"
{
	Properties{
	_MainTex("Base Texture", 2D) = "white" {}
	_BaseBri("Base Brightness", Range(-1, 1)) = 0
	_BaseSat("Base Saturation", Range(0.01, 3)) = 1.0
	_Cube("ReflectionCube", CUBE) = "black" {}
	_Reflection("Reflection Power", Range(0.01, 1)) = 0.5
	_Glossiness ("Smoothness", Range(0,1)) = 0.8
        _SpecularColor("Specular", Color) = (0.4,0.4,0.4)
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		CGPROGRAM
#pragma surface surf StandardSpecular fullforwardshadows

		struct Input {
		float2 uv_MainTex;
		float3 worldRefl;
		float3 viewDir;
	};
	sampler2D _MainTex;
	samplerCUBE _Cube;
	fixed _BaseBri;
	fixed _BaseSat;
	fixed _Reflection;
	half _Glossiness;
        fixed3 _SpecularColor;

    	

	void surf(Input IN, inout SurfaceOutputStandardSpecular o) {
		fixed4 mytex = tex2D(_MainTex, IN.uv_MainTex) + _BaseBri;
		float3 intensity = dot(mytex.rgb, float3(0.299, 0.587, 0.114));
		o.Albedo = lerp(intensity, mytex.rgb, _BaseSat);
		// Specular from specular color
            	o.Specular = _SpecularColor;
            	// Smoothness come from slider variable
            	o.Smoothness = _Glossiness;
		o.Emission = texCUBE(_Cube, IN.worldRefl).rgb * _Reflection;
	}
	ENDCG
	}
		Fallback "Diffuse"
}
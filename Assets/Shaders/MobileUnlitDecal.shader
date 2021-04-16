Shader "Mobile/UnlitDecal"
{
Properties{
	_Color("Main Color", Color) = (1, 1, 1, 1)
	_MainTex("Base (RGB)", 2D) = "white" {}
	_DecalTex("Decal (RGBA)", 2D) = "black" {}
}

SubShader{

	Tags{
	"Queue" = "Transparent"
	"IgnoreProjector" = "True"
	"RenderType" = "Transparent"
	}

Pass{

		BindChannels{
		Bind "Vertex", vertex
		Bind "normal", normal
		Bind "texcoord", texcoord0 // main uses 1st uv
		Bind "texcoord1", texcoord1  // decal uses 2nd uv
	}

	SetTexture[_MainTex]{
		constantColor[_Color]
		Combine texture * constant
	}

	SetTexture[_DecalTex]{
		combine texture lerp(texture) previous
	}
}
}
}
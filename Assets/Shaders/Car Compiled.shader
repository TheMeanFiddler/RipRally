Shader "Custom/Car Compiled" {
	Properties {
		_Cube("Reflection Cubemap", Cube) = "black" {}
		_ReflectColor("Reflection Color", Color) = (1,1,1,0.5)
		_FresnelColor("Fresnel Color", Color) = (1,0.4338235,0.4338235,1)
		_Color("Main Color", Color) = (1,0,0,1)
		_TransmissiveColor("_TransmissiveColor", Range(0,1) ) = 1
		_Shininess("Shininess", Range(0.01,10) ) = 0.069
		_HdrPower("HDR Power" , Range(2,4)) = 4
		_HdrContrast("HDR Contrast" , Range(0,10)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
			
	Pass {
		Name "FORWARD"
		Tags { "LightMode" = "ForwardBase" }
Program "vp" {
// Vertex combos: 9
//   opengl - ALU: 33 to 94
//   d3d9 - ALU: 34 to 96
//   d3d11 - ALU: 31 to 79, TEX: 0 to 0, FLOW: 1 to 1
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Vector 13 [_WorldSpaceCameraPos]
Vector 14 [_WorldSpaceLightPos0]
Vector 15 [unity_SHAr]
Vector 16 [unity_SHAg]
Vector 17 [unity_SHAb]
Vector 18 [unity_SHBr]
Vector 19 [unity_SHBg]
Vector 20 [unity_SHBb]
Vector 21 [unity_SHC]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 22 [unity_Scale]
"3.0-!!ARBvp1.0
# 57 ALU
PARAM c[23] = { { 1 },
		state.matrix.mvp,
		program.local[5..22] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MUL R1.xyz, vertex.normal, c[22].w;
DP3 R2.w, R1, c[6];
DP3 R0.x, R1, c[5];
DP3 R0.z, R1, c[7];
MOV R0.y, R2.w;
MUL R1, R0.xyzz, R0.yzzx;
MOV R0.w, c[0].x;
DP4 R2.z, R0, c[17];
DP4 R2.y, R0, c[16];
DP4 R2.x, R0, c[15];
MUL R0.y, R2.w, R2.w;
DP4 R3.z, R1, c[20];
DP4 R3.y, R1, c[19];
DP4 R3.x, R1, c[18];
ADD R2.xyz, R2, R3;
MAD R0.x, R0, R0, -R0.y;
MUL R3.xyz, R0.x, c[21];
MOV R1.xyz, vertex.attrib[14];
MUL R0.xyz, vertex.normal.zxyw, R1.yzxw;
MAD R0.xyz, vertex.normal.yzxw, R1.zxyw, -R0;
ADD result.texcoord[5].xyz, R2, R3;
MUL R3.xyz, R0, vertex.attrib[14].w;
MOV R0, c[14];
MOV R1.xyz, c[13];
MOV R1.w, c[0].x;
DP4 R2.z, R1, c[11];
DP4 R2.x, R1, c[9];
DP4 R2.y, R1, c[10];
MAD R2.xyz, R2, c[22].w, -vertex.position;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
DP3 R0.y, R3, c[5];
DP3 R0.w, -R2, c[5];
DP3 R0.x, vertex.attrib[14], c[5];
DP3 R0.z, vertex.normal, c[5];
MUL result.texcoord[1], R0, c[22].w;
DP3 R0.y, R3, c[6];
DP3 R0.w, -R2, c[6];
DP3 R0.x, vertex.attrib[14], c[6];
DP3 R0.z, vertex.normal, c[6];
MUL result.texcoord[2], R0, c[22].w;
DP3 R0.y, R3, c[7];
DP3 R0.w, -R2, c[7];
DP3 R0.x, vertex.attrib[14], c[7];
DP3 R0.z, vertex.normal, c[7];
DP3 result.texcoord[0].y, R2, R3;
DP3 result.texcoord[4].y, R3, R1;
MUL result.texcoord[3], R0, c[22].w;
DP3 result.texcoord[0].z, vertex.normal, R2;
DP3 result.texcoord[0].x, R2, vertex.attrib[14];
DP3 result.texcoord[4].z, vertex.normal, R1;
DP3 result.texcoord[4].x, vertex.attrib[14], R1;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 57 instructions, 4 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Matrix 0 [glstate_matrix_mvp]
Vector 12 [_WorldSpaceCameraPos]
Vector 13 [_WorldSpaceLightPos0]
Vector 14 [unity_SHAr]
Vector 15 [unity_SHAg]
Vector 16 [unity_SHAb]
Vector 17 [unity_SHBr]
Vector 18 [unity_SHBg]
Vector 19 [unity_SHBb]
Vector 20 [unity_SHC]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 21 [unity_Scale]
"vs_3_0
; 60 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
def c22, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
mul r1.xyz, v2, c21.w
dp3 r2.w, r1, c5
dp3 r0.x, r1, c4
dp3 r0.z, r1, c6
mov r0.y, r2.w
mul r1, r0.xyzz, r0.yzzx
mov r0.w, c22.x
dp4 r2.z, r0, c16
dp4 r2.y, r0, c15
dp4 r2.x, r0, c14
mul r0.y, r2.w, r2.w
dp4 r3.z, r1, c19
dp4 r3.y, r1, c18
dp4 r3.x, r1, c17
add r1.xyz, r2, r3
mad r0.x, r0, r0, -r0.y
mul r2.xyz, r0.x, c20
add o6.xyz, r1, r2
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r0.xyz, v2.yzxw, r0.zxyw, -r1
mul r3.xyz, r0, v1.w
mov r0, c10
dp4 r4.z, c13, r0
mov r0, c9
dp4 r4.y, c13, r0
mov r1.w, c22.x
mov r1.xyz, c12
dp4 r2.z, r1, c10
dp4 r2.x, r1, c8
dp4 r2.y, r1, c9
mad r2.xyz, r2, c21.w, -v0
mov r1, c8
dp4 r4.x, c13, r1
dp3 r0.y, r3, c4
dp3 r0.w, -r2, c4
dp3 r0.x, v1, c4
dp3 r0.z, v2, c4
mul o2, r0, c21.w
dp3 r0.y, r3, c5
dp3 r0.w, -r2, c5
dp3 r0.x, v1, c5
dp3 r0.z, v2, c5
mul o3, r0, c21.w
dp3 r0.y, r3, c6
dp3 r0.w, -r2, c6
dp3 r0.x, v1, c6
dp3 r0.z, v2, c6
dp3 o1.y, r2, r3
dp3 o5.y, r3, r4
mul o4, r0, c21.w
dp3 o1.z, v2, r2
dp3 o1.x, r2, v1
dp3 o5.z, v2, r4
dp3 o5.x, v1, r4
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "color" Color
ConstBuffer "UnityPerCamera" 128 // 76 used size, 8 vars
Vector 64 [_WorldSpaceCameraPos] 3
ConstBuffer "UnityLighting" 720 // 720 used size, 17 vars
Vector 0 [_WorldSpaceLightPos0] 4
Vector 608 [unity_SHAr] 4
Vector 624 [unity_SHAg] 4
Vector 640 [unity_SHAb] 4
Vector 656 [unity_SHBr] 4
Vector 672 [unity_SHBg] 4
Vector 688 [unity_SHBb] 4
Vector 704 [unity_SHC] 4
ConstBuffer "UnityPerDraw" 336 // 336 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
Matrix 192 [_Object2World] 4
Matrix 256 [_World2Object] 4
Vector 320 [unity_Scale] 4
BindCB "UnityPerCamera" 0
BindCB "UnityLighting" 1
BindCB "UnityPerDraw" 2
// 65 instructions, 4 temp regs, 0 temp arrays:
// ALU 52 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedajapbimdcjhcbkaifmbnndfnkekghdpdabaaaaaahmakaaaaadaaaaaa
cmaaaaaapeaaaaaameabaaaaejfdeheomaaaaaaaagaaaaaaaiaaaaaajiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaakbaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaakjaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
ahahaaaalaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaadaaaaaaapaaaaaalaaaaaaa
abaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaaljaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaafaaaaaaapaaaaaafaepfdejfeejepeoaafeebeoehefeofeaaeoepfc
enebemaafeeffiedepepfceeaaedepemepfcaaklepfdeheomiaaaaaaahaaaaaa
aiaaaaaalaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaalmaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaahaiaaaalmaaaaaaabaaaaaaaaaaaaaa
adaaaaaaacaaaaaaapaaaaaalmaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaa
apaaaaaalmaaaaaaadaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaalmaaaaaa
aeaaaaaaaaaaaaaaadaaaaaaafaaaaaaahaiaaaalmaaaaaaafaaaaaaaaaaaaaa
adaaaaaaagaaaaaaahaiaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklklfdeieefclaaiaaaaeaaaabaacmacaaaafjaaaaaeegiocaaaaaaaaaaa
afaaaaaafjaaaaaeegiocaaaabaaaaaacnaaaaaafjaaaaaeegiocaaaacaaaaaa
bfaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaadpcbabaaaabaaaaaafpaaaaad
hcbabaaaacaaaaaaghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaadhccabaaa
abaaaaaagfaaaaadpccabaaaacaaaaaagfaaaaadpccabaaaadaaaaaagfaaaaad
pccabaaaaeaaaaaagfaaaaadhccabaaaafaaaaaagfaaaaadhccabaaaagaaaaaa
giaaaaacaeaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaa
acaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaacaaaaaaaaaaaaaa
agbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaa
acaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaa
aaaaaaaaegiocaaaacaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaa
diaaaaajhcaabaaaaaaaaaaafgifcaaaaaaaaaaaaeaaaaaaegiccaaaacaaaaaa
bbaaaaaadcaaaaalhcaabaaaaaaaaaaaegiccaaaacaaaaaabaaaaaaaagiacaaa
aaaaaaaaaeaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaaaaaaaaaegiccaaa
acaaaaaabcaaaaaakgikcaaaaaaaaaaaaeaaaaaaegacbaaaaaaaaaaaaaaaaaai
hcaabaaaaaaaaaaaegacbaaaaaaaaaaaegiccaaaacaaaaaabdaaaaaadcaaaaal
hcaabaaaaaaaaaaaegacbaaaaaaaaaaapgipcaaaacaaaaaabeaaaaaaegbcbaia
ebaaaaaaaaaaaaaabaaaaaahbccabaaaabaaaaaaegbcbaaaabaaaaaaegacbaaa
aaaaaaaabaaaaaaheccabaaaabaaaaaaegbcbaaaacaaaaaaegacbaaaaaaaaaaa
diaaaaahhcaabaaaabaaaaaajgbebaaaabaaaaaacgbjbaaaacaaaaaadcaaaaak
hcaabaaaabaaaaaajgbebaaaacaaaaaacgbjbaaaabaaaaaaegacbaiaebaaaaaa
abaaaaaadiaaaaahhcaabaaaabaaaaaaegacbaaaabaaaaaapgbpbaaaabaaaaaa
baaaaaahcccabaaaabaaaaaaegacbaaaabaaaaaaegacbaaaaaaaaaaadiaaaaaj
hcaabaaaacaaaaaafgafbaiaebaaaaaaaaaaaaaaegiccaaaacaaaaaaanaaaaaa
dcaaaaallcaabaaaaaaaaaaaegiicaaaacaaaaaaamaaaaaaagaabaiaebaaaaaa
aaaaaaaaegaibaaaacaaaaaadcaaaaallcaabaaaaaaaaaaaegiicaaaacaaaaaa
aoaaaaaakgakbaiaebaaaaaaaaaaaaaaegambaaaaaaaaaaadgaaaaaficaabaaa
acaaaaaaakaabaaaaaaaaaaadgaaaaagbcaabaaaadaaaaaaakiacaaaacaaaaaa
amaaaaaadgaaaaagccaabaaaadaaaaaaakiacaaaacaaaaaaanaaaaaadgaaaaag
ecaabaaaadaaaaaaakiacaaaacaaaaaaaoaaaaaabaaaaaahccaabaaaacaaaaaa
egacbaaaabaaaaaaegacbaaaadaaaaaabaaaaaahbcaabaaaacaaaaaaegbcbaaa
abaaaaaaegacbaaaadaaaaaabaaaaaahecaabaaaacaaaaaaegbcbaaaacaaaaaa
egacbaaaadaaaaaadiaaaaaipccabaaaacaaaaaaegaobaaaacaaaaaapgipcaaa
acaaaaaabeaaaaaadgaaaaaficaabaaaacaaaaaabkaabaaaaaaaaaaadgaaaaag
bcaabaaaadaaaaaabkiacaaaacaaaaaaamaaaaaadgaaaaagccaabaaaadaaaaaa
bkiacaaaacaaaaaaanaaaaaadgaaaaagecaabaaaadaaaaaabkiacaaaacaaaaaa
aoaaaaaabaaaaaahccaabaaaacaaaaaaegacbaaaabaaaaaaegacbaaaadaaaaaa
baaaaaahbcaabaaaacaaaaaaegbcbaaaabaaaaaaegacbaaaadaaaaaabaaaaaah
ecaabaaaacaaaaaaegbcbaaaacaaaaaaegacbaaaadaaaaaadiaaaaaipccabaaa
adaaaaaaegaobaaaacaaaaaapgipcaaaacaaaaaabeaaaaaadgaaaaagbcaabaaa
acaaaaaackiacaaaacaaaaaaamaaaaaadgaaaaagccaabaaaacaaaaaackiacaaa
acaaaaaaanaaaaaadgaaaaagecaabaaaacaaaaaackiacaaaacaaaaaaaoaaaaaa
baaaaaahccaabaaaaaaaaaaaegacbaaaabaaaaaaegacbaaaacaaaaaabaaaaaah
bcaabaaaaaaaaaaaegbcbaaaabaaaaaaegacbaaaacaaaaaabaaaaaahecaabaaa
aaaaaaaaegbcbaaaacaaaaaaegacbaaaacaaaaaadiaaaaaipccabaaaaeaaaaaa
egaobaaaaaaaaaaapgipcaaaacaaaaaabeaaaaaadiaaaaajhcaabaaaaaaaaaaa
fgifcaaaabaaaaaaaaaaaaaaegiccaaaacaaaaaabbaaaaaadcaaaaalhcaabaaa
aaaaaaaaegiccaaaacaaaaaabaaaaaaaagiacaaaabaaaaaaaaaaaaaaegacbaaa
aaaaaaaadcaaaaalhcaabaaaaaaaaaaaegiccaaaacaaaaaabcaaaaaakgikcaaa
abaaaaaaaaaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaaaaaaaaaegiccaaa
acaaaaaabdaaaaaapgipcaaaabaaaaaaaaaaaaaaegacbaaaaaaaaaaabaaaaaah
cccabaaaafaaaaaaegacbaaaabaaaaaaegacbaaaaaaaaaaabaaaaaahbccabaaa
afaaaaaaegbcbaaaabaaaaaaegacbaaaaaaaaaaabaaaaaaheccabaaaafaaaaaa
egbcbaaaacaaaaaaegacbaaaaaaaaaaadiaaaaaihcaabaaaaaaaaaaaegbcbaaa
acaaaaaapgipcaaaacaaaaaabeaaaaaadiaaaaaihcaabaaaabaaaaaafgafbaaa
aaaaaaaaegiccaaaacaaaaaaanaaaaaadcaaaaaklcaabaaaaaaaaaaaegiicaaa
acaaaaaaamaaaaaaagaabaaaaaaaaaaaegaibaaaabaaaaaadcaaaaakhcaabaaa
aaaaaaaaegiccaaaacaaaaaaaoaaaaaakgakbaaaaaaaaaaaegadbaaaaaaaaaaa
dgaaaaaficaabaaaaaaaaaaaabeaaaaaaaaaiadpbbaaaaaibcaabaaaabaaaaaa
egiocaaaabaaaaaacgaaaaaaegaobaaaaaaaaaaabbaaaaaiccaabaaaabaaaaaa
egiocaaaabaaaaaachaaaaaaegaobaaaaaaaaaaabbaaaaaiecaabaaaabaaaaaa
egiocaaaabaaaaaaciaaaaaaegaobaaaaaaaaaaadiaaaaahpcaabaaaacaaaaaa
jgacbaaaaaaaaaaaegakbaaaaaaaaaaabbaaaaaibcaabaaaadaaaaaaegiocaaa
abaaaaaacjaaaaaaegaobaaaacaaaaaabbaaaaaiccaabaaaadaaaaaaegiocaaa
abaaaaaackaaaaaaegaobaaaacaaaaaabbaaaaaiecaabaaaadaaaaaaegiocaaa
abaaaaaaclaaaaaaegaobaaaacaaaaaaaaaaaaahhcaabaaaabaaaaaaegacbaaa
abaaaaaaegacbaaaadaaaaaadiaaaaahccaabaaaaaaaaaaabkaabaaaaaaaaaaa
bkaabaaaaaaaaaaadcaaaaakbcaabaaaaaaaaaaaakaabaaaaaaaaaaaakaabaaa
aaaaaaaabkaabaiaebaaaaaaaaaaaaaadcaaaaakhccabaaaagaaaaaaegiccaaa
abaaaaaacmaaaaaaagaabaaaaaaaaaaaegacbaaaabaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES


#ifdef VERTEX

varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_SHC;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAr;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  lowp vec4 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (_glesVertex.xyz - ((_World2Object * tmpvar_9).xyz * unity_Scale.w)));
  highp vec3 tmpvar_12;
  highp vec3 tmpvar_13;
  tmpvar_12 = tmpvar_1.xyz;
  tmpvar_13 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_13.x;
  tmpvar_14[0].z = tmpvar_2.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_13.y;
  tmpvar_14[1].z = tmpvar_2.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_13.z;
  tmpvar_14[2].z = tmpvar_2.z;
  vec4 v_15;
  v_15.x = _Object2World[0].x;
  v_15.y = _Object2World[1].x;
  v_15.z = _Object2World[2].x;
  v_15.w = _Object2World[3].x;
  highp vec4 tmpvar_16;
  tmpvar_16.xyz = (tmpvar_14 * v_15.xyz);
  tmpvar_16.w = tmpvar_11.x;
  highp vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * unity_Scale.w);
  tmpvar_4 = tmpvar_17;
  vec4 v_18;
  v_18.x = _Object2World[0].y;
  v_18.y = _Object2World[1].y;
  v_18.z = _Object2World[2].y;
  v_18.w = _Object2World[3].y;
  highp vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_14 * v_18.xyz);
  tmpvar_19.w = tmpvar_11.y;
  highp vec4 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * unity_Scale.w);
  tmpvar_5 = tmpvar_20;
  vec4 v_21;
  v_21.x = _Object2World[0].z;
  v_21.y = _Object2World[1].z;
  v_21.z = _Object2World[2].z;
  v_21.w = _Object2World[3].z;
  highp vec4 tmpvar_22;
  tmpvar_22.xyz = (tmpvar_14 * v_21.xyz);
  tmpvar_22.w = tmpvar_11.z;
  highp vec4 tmpvar_23;
  tmpvar_23 = (tmpvar_22 * unity_Scale.w);
  tmpvar_6 = tmpvar_23;
  mat3 tmpvar_24;
  tmpvar_24[0] = _Object2World[0].xyz;
  tmpvar_24[1] = _Object2World[1].xyz;
  tmpvar_24[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_14 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_7 = tmpvar_25;
  highp vec4 tmpvar_26;
  tmpvar_26.w = 1.0;
  tmpvar_26.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_27;
  tmpvar_27.w = 1.0;
  tmpvar_27.xyz = (tmpvar_24 * (tmpvar_2 * unity_Scale.w));
  mediump vec3 tmpvar_28;
  mediump vec4 normal_29;
  normal_29 = tmpvar_27;
  highp float vC_30;
  mediump vec3 x3_31;
  mediump vec3 x2_32;
  mediump vec3 x1_33;
  highp float tmpvar_34;
  tmpvar_34 = dot (unity_SHAr, normal_29);
  x1_33.x = tmpvar_34;
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHAg, normal_29);
  x1_33.y = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHAb, normal_29);
  x1_33.z = tmpvar_36;
  mediump vec4 tmpvar_37;
  tmpvar_37 = (normal_29.xyzz * normal_29.yzzx);
  highp float tmpvar_38;
  tmpvar_38 = dot (unity_SHBr, tmpvar_37);
  x2_32.x = tmpvar_38;
  highp float tmpvar_39;
  tmpvar_39 = dot (unity_SHBg, tmpvar_37);
  x2_32.y = tmpvar_39;
  highp float tmpvar_40;
  tmpvar_40 = dot (unity_SHBb, tmpvar_37);
  x2_32.z = tmpvar_40;
  mediump float tmpvar_41;
  tmpvar_41 = ((normal_29.x * normal_29.x) - (normal_29.y * normal_29.y));
  vC_30 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42 = (unity_SHC.xyz * vC_30);
  x3_31 = tmpvar_42;
  tmpvar_28 = ((x1_33 + x2_32) + x3_31);
  shlight_3 = tmpvar_28;
  tmpvar_8 = shlight_3;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_14 * (((_World2Object * tmpvar_26).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
}



#endif
#ifdef FRAGMENT

varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform lowp vec4 _LightColor0;
void main ()
{
  lowp vec4 c_1;
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  tmpvar_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD1.xyz;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD2.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD3.xyz;
  tmpvar_6 = tmpvar_10;
  tmpvar_2 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_11;
  lowp float tmpvar_12;
  mediump vec3 HDR_13;
  highp vec4 c_14;
  mediump vec3 tmpvar_15;
  tmpvar_15.x = tmpvar_4.z;
  tmpvar_15.y = tmpvar_5.z;
  tmpvar_15.z = tmpvar_6.z;
  highp vec3 tmpvar_16;
  tmpvar_16 = (tmpvar_3 - (2.0 * (dot (tmpvar_15, tmpvar_3) * tmpvar_15)));
  lowp vec4 tmpvar_17;
  tmpvar_17 = textureCube (_Cube, tmpvar_16);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * _ReflectColor);
  c_14 = tmpvar_18;
  highp vec3 tmpvar_19;
  tmpvar_19 = vec3((pow ((((c_14.x + c_14.y) + c_14.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_13 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = (1.0 - dot (normalize(tmpvar_2), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_21;
  tmpvar_21 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_20, _TransmissiveColor)));
  mediump float tmpvar_22;
  tmpvar_22 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_20)).x;
  tmpvar_12 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = ((c_14.xyz + HDR_13) + tmpvar_21.xyz);
  tmpvar_11 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = normalize(xlv_TEXCOORD0);
  mediump vec3 lightDir_25;
  lightDir_25 = xlv_TEXCOORD4;
  mediump vec3 viewDir_26;
  viewDir_26 = tmpvar_24;
  mediump vec4 c_27;
  c_27.xyz = ((((((lightDir_25.z * 0.5) + 0.5) * tmpvar_11) * _LightColor0.xyz) * 2.0) + (_LightColor0.xyz * max (0.0, (pow ((viewDir_26 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_12))));
  c_27.w = 1.0;
  c_1 = c_27;
  c_1.xyz = (c_1.xyz + (tmpvar_11 * xlv_TEXCOORD5));
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES


#ifdef VERTEX

varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_SHC;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAr;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  lowp vec4 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (_glesVertex.xyz - ((_World2Object * tmpvar_9).xyz * unity_Scale.w)));
  highp vec3 tmpvar_12;
  highp vec3 tmpvar_13;
  tmpvar_12 = tmpvar_1.xyz;
  tmpvar_13 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_13.x;
  tmpvar_14[0].z = tmpvar_2.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_13.y;
  tmpvar_14[1].z = tmpvar_2.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_13.z;
  tmpvar_14[2].z = tmpvar_2.z;
  vec4 v_15;
  v_15.x = _Object2World[0].x;
  v_15.y = _Object2World[1].x;
  v_15.z = _Object2World[2].x;
  v_15.w = _Object2World[3].x;
  highp vec4 tmpvar_16;
  tmpvar_16.xyz = (tmpvar_14 * v_15.xyz);
  tmpvar_16.w = tmpvar_11.x;
  highp vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * unity_Scale.w);
  tmpvar_4 = tmpvar_17;
  vec4 v_18;
  v_18.x = _Object2World[0].y;
  v_18.y = _Object2World[1].y;
  v_18.z = _Object2World[2].y;
  v_18.w = _Object2World[3].y;
  highp vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_14 * v_18.xyz);
  tmpvar_19.w = tmpvar_11.y;
  highp vec4 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * unity_Scale.w);
  tmpvar_5 = tmpvar_20;
  vec4 v_21;
  v_21.x = _Object2World[0].z;
  v_21.y = _Object2World[1].z;
  v_21.z = _Object2World[2].z;
  v_21.w = _Object2World[3].z;
  highp vec4 tmpvar_22;
  tmpvar_22.xyz = (tmpvar_14 * v_21.xyz);
  tmpvar_22.w = tmpvar_11.z;
  highp vec4 tmpvar_23;
  tmpvar_23 = (tmpvar_22 * unity_Scale.w);
  tmpvar_6 = tmpvar_23;
  mat3 tmpvar_24;
  tmpvar_24[0] = _Object2World[0].xyz;
  tmpvar_24[1] = _Object2World[1].xyz;
  tmpvar_24[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_14 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_7 = tmpvar_25;
  highp vec4 tmpvar_26;
  tmpvar_26.w = 1.0;
  tmpvar_26.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_27;
  tmpvar_27.w = 1.0;
  tmpvar_27.xyz = (tmpvar_24 * (tmpvar_2 * unity_Scale.w));
  mediump vec3 tmpvar_28;
  mediump vec4 normal_29;
  normal_29 = tmpvar_27;
  highp float vC_30;
  mediump vec3 x3_31;
  mediump vec3 x2_32;
  mediump vec3 x1_33;
  highp float tmpvar_34;
  tmpvar_34 = dot (unity_SHAr, normal_29);
  x1_33.x = tmpvar_34;
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHAg, normal_29);
  x1_33.y = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHAb, normal_29);
  x1_33.z = tmpvar_36;
  mediump vec4 tmpvar_37;
  tmpvar_37 = (normal_29.xyzz * normal_29.yzzx);
  highp float tmpvar_38;
  tmpvar_38 = dot (unity_SHBr, tmpvar_37);
  x2_32.x = tmpvar_38;
  highp float tmpvar_39;
  tmpvar_39 = dot (unity_SHBg, tmpvar_37);
  x2_32.y = tmpvar_39;
  highp float tmpvar_40;
  tmpvar_40 = dot (unity_SHBb, tmpvar_37);
  x2_32.z = tmpvar_40;
  mediump float tmpvar_41;
  tmpvar_41 = ((normal_29.x * normal_29.x) - (normal_29.y * normal_29.y));
  vC_30 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42 = (unity_SHC.xyz * vC_30);
  x3_31 = tmpvar_42;
  tmpvar_28 = ((x1_33 + x2_32) + x3_31);
  shlight_3 = tmpvar_28;
  tmpvar_8 = shlight_3;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_14 * (((_World2Object * tmpvar_26).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
}



#endif
#ifdef FRAGMENT

varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform lowp vec4 _LightColor0;
void main ()
{
  lowp vec4 c_1;
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  tmpvar_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD1.xyz;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD2.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD3.xyz;
  tmpvar_6 = tmpvar_10;
  tmpvar_2 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_11;
  lowp float tmpvar_12;
  mediump vec3 HDR_13;
  highp vec4 c_14;
  mediump vec3 tmpvar_15;
  tmpvar_15.x = tmpvar_4.z;
  tmpvar_15.y = tmpvar_5.z;
  tmpvar_15.z = tmpvar_6.z;
  highp vec3 tmpvar_16;
  tmpvar_16 = (tmpvar_3 - (2.0 * (dot (tmpvar_15, tmpvar_3) * tmpvar_15)));
  lowp vec4 tmpvar_17;
  tmpvar_17 = textureCube (_Cube, tmpvar_16);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * _ReflectColor);
  c_14 = tmpvar_18;
  highp vec3 tmpvar_19;
  tmpvar_19 = vec3((pow ((((c_14.x + c_14.y) + c_14.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_13 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = (1.0 - dot (normalize(tmpvar_2), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_21;
  tmpvar_21 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_20, _TransmissiveColor)));
  mediump float tmpvar_22;
  tmpvar_22 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_20)).x;
  tmpvar_12 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = ((c_14.xyz + HDR_13) + tmpvar_21.xyz);
  tmpvar_11 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = normalize(xlv_TEXCOORD0);
  mediump vec3 lightDir_25;
  lightDir_25 = xlv_TEXCOORD4;
  mediump vec3 viewDir_26;
  viewDir_26 = tmpvar_24;
  mediump vec4 c_27;
  c_27.xyz = ((((((lightDir_25.z * 0.5) + 0.5) * tmpvar_11) * _LightColor0.xyz) * 2.0) + (_LightColor0.xyz * max (0.0, (pow ((viewDir_26 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_12))));
  c_27.w = 1.0;
  c_1 = c_27;
  c_1.xyz = (c_1.xyz + (tmpvar_11 * xlv_TEXCOORD5));
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Color _glesColor
in vec4 _glesColor;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;
#define gl_MultiTexCoord1 _glesMultiTexCoord1
in vec4 _glesMultiTexCoord1;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
mat2 xll_transpose_mf2x2(mat2 m) {
  return mat2( m[0][0], m[1][0], m[0][1], m[1][1]);
}
mat3 xll_transpose_mf3x3(mat3 m) {
  return mat3( m[0][0], m[1][0], m[2][0],
               m[0][1], m[1][1], m[2][1],
               m[0][2], m[1][2], m[2][2]);
}
mat4 xll_transpose_mf4x4(mat4 m) {
  return mat4( m[0][0], m[1][0], m[2][0], m[3][0],
               m[0][1], m[1][1], m[2][1], m[3][1],
               m[0][2], m[1][2], m[2][2], m[3][2],
               m[0][3], m[1][3], m[2][3], m[3][3]);
}
vec2 xll_matrixindex_mf2x2_i (mat2 m, int i) { vec2 v; v.x=m[0][i]; v.y=m[1][i]; return v; }
vec3 xll_matrixindex_mf3x3_i (mat3 m, int i) { vec3 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; return v; }
vec4 xll_matrixindex_mf4x4_i (mat4 m, int i) { vec4 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; v.w=m[3][i]; return v; }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 399
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 432
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    lowp vec3 lightDir;
    lowp vec3 vlight;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 393
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 397
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 408
#line 443
#line 464
#line 82
highp vec3 ObjSpaceLightDir( in highp vec4 v ) {
    highp vec3 objSpaceLightPos = (_World2Object * _WorldSpaceLightPos0).xyz;
    return objSpaceLightPos.xyz;
}
#line 91
highp vec3 ObjSpaceViewDir( in highp vec4 v ) {
    highp vec3 objSpaceCameraPos = ((_World2Object * vec4( _WorldSpaceCameraPos.xyz, 1.0)).xyz * unity_Scale.w);
    return (objSpaceCameraPos - v.xyz);
}
#line 137
mediump vec3 ShadeSH9( in mediump vec4 normal ) {
    mediump vec3 x1;
    mediump vec3 x2;
    mediump vec3 x3;
    x1.x = dot( unity_SHAr, normal);
    #line 141
    x1.y = dot( unity_SHAg, normal);
    x1.z = dot( unity_SHAb, normal);
    mediump vec4 vB = (normal.xyzz * normal.yzzx);
    x2.x = dot( unity_SHBr, vB);
    #line 145
    x2.y = dot( unity_SHBg, vB);
    x2.z = dot( unity_SHBb, vB);
    highp float vC = ((normal.x * normal.x) - (normal.y * normal.y));
    x3 = (unity_SHC.xyz * vC);
    #line 149
    return ((x1 + x2) + x3);
}
#line 443
v2f_surf vert_surf( in appdata_full v ) {
    v2f_surf o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 447
    highp vec3 viewDir = (-ObjSpaceViewDir( v.vertex));
    highp vec3 worldRefl = (mat3( _Object2World) * viewDir);
    highp vec3 binormal = (cross( v.normal, v.tangent.xyz) * v.tangent.w);
    highp mat3 rotation = xll_transpose_mf3x3(mat3( v.tangent.xyz, binormal, v.normal));
    #line 451
    o.TtoW0 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 0).xyz), worldRefl.x) * unity_Scale.w);
    o.TtoW1 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 1).xyz), worldRefl.y) * unity_Scale.w);
    o.TtoW2 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 2).xyz), worldRefl.z) * unity_Scale.w);
    highp vec3 worldN = (mat3( _Object2World) * (v.normal * unity_Scale.w));
    #line 455
    highp vec3 lightDir = (rotation * ObjSpaceLightDir( v.vertex));
    o.lightDir = lightDir;
    highp vec3 viewDirForLight = (rotation * ObjSpaceViewDir( v.vertex));
    o.viewDir = viewDirForLight;
    #line 459
    highp vec3 shlight = ShadeSH9( vec4( worldN, 1.0));
    o.vlight = shlight;
    return o;
}

out highp vec3 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_TEXCOORD2;
out lowp vec4 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
out lowp vec3 xlv_TEXCOORD5;
void main() {
    v2f_surf xl_retval;
    appdata_full xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.tangent = vec4(TANGENT);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xlt_v.texcoord1 = vec4(gl_MultiTexCoord1);
    xlt_v.color = vec4(gl_Color);
    xl_retval = vert_surf( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD0 = vec3(xl_retval.viewDir);
    xlv_TEXCOORD1 = vec4(xl_retval.TtoW0);
    xlv_TEXCOORD2 = vec4(xl_retval.TtoW1);
    xlv_TEXCOORD3 = vec4(xl_retval.TtoW2);
    xlv_TEXCOORD4 = vec3(xl_retval.lightDir);
    xlv_TEXCOORD5 = vec3(xl_retval.vlight);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 399
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 432
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    lowp vec3 lightDir;
    lowp vec3 vlight;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 393
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 397
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 408
#line 443
#line 464
#line 422
mediump vec4 LightingHalfLambertSpecular( in SurfaceOutput s, in mediump vec3 lightDir, in mediump vec3 viewDir, in mediump float atten ) {
    #line 424
    mediump float NdotL = ((dot( s.Normal, lightDir) * 0.5) + 0.5);
    mediump vec3 halfVector = (viewDir + s.Normal);
    mediump float spec = max( 0.0, (pow( dot( halfVector, s.Normal), (10.0 - s.Specular)) * s.Gloss));
    mediump vec4 c;
    #line 428
    c.xyz = (((((NdotL * s.Albedo) * _LightColor0.xyz) * atten) * 2.0) + ((_LightColor0.xyz * spec) * atten));
    c.w = s.Alpha;
    return c;
}
#line 408
void surf( in Input IN, inout SurfaceOutput o ) {
    o.Normal = vec3( 0.0, 0.0, 1.0);
    highp vec3 worldRefl = reflect( IN.worldRefl, vec3( dot( IN.TtoW0, o.Normal), dot( IN.TtoW1, o.Normal), dot( IN.TtoW2, o.Normal)));
    #line 412
    highp vec4 c = (texture( _Cube, worldRefl) * _ReflectColor);
    mediump vec3 HDR = vec3( (pow( (((c.x + c.y) + c.z) / 3.0), _HdrPower) * _HdrContrast));
    mediump float fresnel = (1.0 - dot( normalize(IN.viewDir), normalize(vec3( 0.0, 0.0, 1.0))));
    mediump vec4 fresnelLerp = mix( _Color, _FresnelColor, vec4( pow( fresnel, _TransmissiveColor)));
    #line 416
    mediump vec4 GlossLerp = mix( vec4( 0.0, 0.0, 0.0, 0.0), vec4( 1.0, 1.0, 1.0, 1.0), vec4( fresnel));
    o.Specular = _Shininess;
    o.Gloss = GlossLerp.x;
    o.Albedo = ((c.xyz + HDR) + fresnelLerp.xyz);
    #line 420
    o.Alpha = 1.0;
}
#line 464
lowp vec4 frag_surf( in v2f_surf IN ) {
    Input surfIN;
    surfIN.worldRefl = vec3( IN.TtoW0.w, IN.TtoW1.w, IN.TtoW2.w);
    #line 468
    surfIN.TtoW0 = IN.TtoW0.xyz;
    surfIN.TtoW1 = IN.TtoW1.xyz;
    surfIN.TtoW2 = IN.TtoW2.xyz;
    surfIN.viewDir = IN.viewDir;
    #line 472
    SurfaceOutput o;
    o.Albedo = vec3( 0.0);
    o.Emission = vec3( 0.0);
    o.Specular = 0.0;
    #line 476
    o.Alpha = 0.0;
    o.Gloss = 0.0;
    surf( surfIN, o);
    lowp float atten = 1.0;
    #line 480
    lowp vec4 c = vec4( 0.0);
    c = LightingHalfLambertSpecular( o, IN.lightDir, normalize(IN.viewDir), atten);
    c.xyz += (o.Albedo * IN.vlight);
    return c;
}
in highp vec3 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_TEXCOORD2;
in lowp vec4 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in lowp vec3 xlv_TEXCOORD5;
void main() {
    lowp vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.viewDir = vec3(xlv_TEXCOORD0);
    xlt_IN.TtoW0 = vec4(xlv_TEXCOORD1);
    xlt_IN.TtoW1 = vec4(xlv_TEXCOORD2);
    xlt_IN.TtoW2 = vec4(xlv_TEXCOORD3);
    xlt_IN.lightDir = vec3(xlv_TEXCOORD4);
    xlt_IN.vlight = vec3(xlv_TEXCOORD5);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Bind "texcoord1" TexCoord1
Vector 13 [_WorldSpaceCameraPos]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 15 [unity_Scale]
Vector 16 [unity_LightmapST]
"3.0-!!ARBvp1.0
# 33 ALU
PARAM c[17] = { { 1 },
		state.matrix.mvp,
		program.local[5..16] };
TEMP R0;
TEMP R1;
TEMP R2;
MOV R0.xyz, vertex.attrib[14];
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R0.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MUL R2.xyz, R0, vertex.attrib[14].w;
MOV R0.xyz, c[13];
MOV R0.w, c[0].x;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
MAD R1.xyz, R1, c[15].w, -vertex.position;
DP3 R0.y, R2, c[5];
DP3 R0.w, -R1, c[5];
DP3 R0.x, vertex.attrib[14], c[5];
DP3 R0.z, vertex.normal, c[5];
MUL result.texcoord[1], R0, c[15].w;
DP3 R0.y, R2, c[6];
DP3 R0.w, -R1, c[6];
DP3 R0.x, vertex.attrib[14], c[6];
DP3 R0.z, vertex.normal, c[6];
MUL result.texcoord[2], R0, c[15].w;
DP3 R0.y, R2, c[7];
DP3 R0.w, -R1, c[7];
DP3 R0.x, vertex.attrib[14], c[7];
DP3 R0.z, vertex.normal, c[7];
DP3 result.texcoord[0].y, R1, R2;
MUL result.texcoord[3], R0, c[15].w;
DP3 result.texcoord[0].z, vertex.normal, R1;
DP3 result.texcoord[0].x, R1, vertex.attrib[14];
MAD result.texcoord[4].xy, vertex.texcoord[1], c[16], c[16].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 33 instructions, 3 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "texcoord1" TexCoord1
Matrix 0 [glstate_matrix_mvp]
Vector 12 [_WorldSpaceCameraPos]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 13 [unity_Scale]
Vector 14 [unity_LightmapST]
"vs_3_0
; 34 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
def c15, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord1 v3
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r0.xyz, v2.yzxw, r0.zxyw, -r1
mul r2.xyz, r0, v1.w
mov r0.xyz, c12
mov r0.w, c15.x
dp4 r1.z, r0, c10
dp4 r1.x, r0, c8
dp4 r1.y, r0, c9
mad r1.xyz, r1, c13.w, -v0
dp3 r0.y, r2, c4
dp3 r0.w, -r1, c4
dp3 r0.x, v1, c4
dp3 r0.z, v2, c4
mul o2, r0, c13.w
dp3 r0.y, r2, c5
dp3 r0.w, -r1, c5
dp3 r0.x, v1, c5
dp3 r0.z, v2, c5
mul o3, r0, c13.w
dp3 r0.y, r2, c6
dp3 r0.w, -r1, c6
dp3 r0.x, v1, c6
dp3 r0.z, v2, c6
dp3 o1.y, r1, r2
mul o4, r0, c13.w
dp3 o1.z, v2, r1
dp3 o1.x, r1, v1
mad o5.xy, v3, c14, c14.zwzw
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "texcoord1" TexCoord1
Bind "color" Color
ConstBuffer "$Globals" 128 // 128 used size, 11 vars
Vector 112 [unity_LightmapST] 4
ConstBuffer "UnityPerCamera" 128 // 76 used size, 8 vars
Vector 64 [_WorldSpaceCameraPos] 3
ConstBuffer "UnityPerDraw" 336 // 336 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
Matrix 192 [_Object2World] 4
Matrix 256 [_World2Object] 4
Vector 320 [unity_Scale] 4
BindCB "$Globals" 0
BindCB "UnityPerCamera" 1
BindCB "UnityPerDraw" 2
// 43 instructions, 4 temp regs, 0 temp arrays:
// ALU 31 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecednneicomeflmlekddjjeelbgfgjcfgnpfabaaaaaaimahaaaaadaaaaaa
cmaaaaaapeaaaaaakmabaaaaejfdeheomaaaaaaaagaaaaaaaiaaaaaajiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaakbaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaakjaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
ahahaaaalaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaadaaaaaaapaaaaaalaaaaaaa
abaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapadaaaaljaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaafaaaaaaapaaaaaafaepfdejfeejepeoaafeebeoehefeofeaaeoepfc
enebemaafeeffiedepepfceeaaedepemepfcaaklepfdeheolaaaaaaaagaaaaaa
aiaaaaaajiaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaakeaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaahaiaaaakeaaaaaaabaaaaaaaaaaaaaa
adaaaaaaacaaaaaaapaaaaaakeaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaa
apaaaaaakeaaaaaaadaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaakeaaaaaa
aeaaaaaaaaaaaaaaadaaaaaaafaaaaaaadamaaaafdfgfpfaepfdejfeejepeoaa
feeffiedepepfceeaaklklklfdeieefcniafaaaaeaaaabaahgabaaaafjaaaaae
egiocaaaaaaaaaaaaiaaaaaafjaaaaaeegiocaaaabaaaaaaafaaaaaafjaaaaae
egiocaaaacaaaaaabfaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaadpcbabaaa
abaaaaaafpaaaaadhcbabaaaacaaaaaafpaaaaaddcbabaaaaeaaaaaaghaaaaae
pccabaaaaaaaaaaaabaaaaaagfaaaaadhccabaaaabaaaaaagfaaaaadpccabaaa
acaaaaaagfaaaaadpccabaaaadaaaaaagfaaaaadpccabaaaaeaaaaaagfaaaaad
dccabaaaafaaaaaagiaaaaacaeaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaa
aaaaaaaaegiocaaaacaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaa
acaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaacaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpccabaaaaaaaaaaaegiocaaaacaaaaaaadaaaaaapgbpbaaaaaaaaaaa
egaobaaaaaaaaaaadiaaaaajhcaabaaaaaaaaaaafgifcaaaabaaaaaaaeaaaaaa
egiccaaaacaaaaaabbaaaaaadcaaaaalhcaabaaaaaaaaaaaegiccaaaacaaaaaa
baaaaaaaagiacaaaabaaaaaaaeaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaa
aaaaaaaaegiccaaaacaaaaaabcaaaaaakgikcaaaabaaaaaaaeaaaaaaegacbaaa
aaaaaaaaaaaaaaaihcaabaaaaaaaaaaaegacbaaaaaaaaaaaegiccaaaacaaaaaa
bdaaaaaadcaaaaalhcaabaaaaaaaaaaaegacbaaaaaaaaaaapgipcaaaacaaaaaa
beaaaaaaegbcbaiaebaaaaaaaaaaaaaabaaaaaahbccabaaaabaaaaaaegbcbaaa
abaaaaaaegacbaaaaaaaaaaabaaaaaaheccabaaaabaaaaaaegbcbaaaacaaaaaa
egacbaaaaaaaaaaadiaaaaahhcaabaaaabaaaaaajgbebaaaabaaaaaacgbjbaaa
acaaaaaadcaaaaakhcaabaaaabaaaaaajgbebaaaacaaaaaacgbjbaaaabaaaaaa
egacbaiaebaaaaaaabaaaaaadiaaaaahhcaabaaaabaaaaaaegacbaaaabaaaaaa
pgbpbaaaabaaaaaabaaaaaahcccabaaaabaaaaaaegacbaaaabaaaaaaegacbaaa
aaaaaaaadiaaaaajhcaabaaaacaaaaaafgafbaiaebaaaaaaaaaaaaaaegiccaaa
acaaaaaaanaaaaaadcaaaaallcaabaaaaaaaaaaaegiicaaaacaaaaaaamaaaaaa
agaabaiaebaaaaaaaaaaaaaaegaibaaaacaaaaaadcaaaaallcaabaaaaaaaaaaa
egiicaaaacaaaaaaaoaaaaaakgakbaiaebaaaaaaaaaaaaaaegambaaaaaaaaaaa
dgaaaaaficaabaaaacaaaaaaakaabaaaaaaaaaaadgaaaaagbcaabaaaadaaaaaa
akiacaaaacaaaaaaamaaaaaadgaaaaagccaabaaaadaaaaaaakiacaaaacaaaaaa
anaaaaaadgaaaaagecaabaaaadaaaaaaakiacaaaacaaaaaaaoaaaaaabaaaaaah
ccaabaaaacaaaaaaegacbaaaabaaaaaaegacbaaaadaaaaaabaaaaaahbcaabaaa
acaaaaaaegbcbaaaabaaaaaaegacbaaaadaaaaaabaaaaaahecaabaaaacaaaaaa
egbcbaaaacaaaaaaegacbaaaadaaaaaadiaaaaaipccabaaaacaaaaaaegaobaaa
acaaaaaapgipcaaaacaaaaaabeaaaaaadgaaaaaficaabaaaacaaaaaabkaabaaa
aaaaaaaadgaaaaagbcaabaaaadaaaaaabkiacaaaacaaaaaaamaaaaaadgaaaaag
ccaabaaaadaaaaaabkiacaaaacaaaaaaanaaaaaadgaaaaagecaabaaaadaaaaaa
bkiacaaaacaaaaaaaoaaaaaabaaaaaahccaabaaaacaaaaaaegacbaaaabaaaaaa
egacbaaaadaaaaaabaaaaaahbcaabaaaacaaaaaaegbcbaaaabaaaaaaegacbaaa
adaaaaaabaaaaaahecaabaaaacaaaaaaegbcbaaaacaaaaaaegacbaaaadaaaaaa
diaaaaaipccabaaaadaaaaaaegaobaaaacaaaaaapgipcaaaacaaaaaabeaaaaaa
dgaaaaagbcaabaaaacaaaaaackiacaaaacaaaaaaamaaaaaadgaaaaagccaabaaa
acaaaaaackiacaaaacaaaaaaanaaaaaadgaaaaagecaabaaaacaaaaaackiacaaa
acaaaaaaaoaaaaaabaaaaaahccaabaaaaaaaaaaaegacbaaaabaaaaaaegacbaaa
acaaaaaabaaaaaahbcaabaaaaaaaaaaaegbcbaaaabaaaaaaegacbaaaacaaaaaa
baaaaaahecaabaaaaaaaaaaaegbcbaaaacaaaaaaegacbaaaacaaaaaadiaaaaai
pccabaaaaeaaaaaaegaobaaaaaaaaaaapgipcaaaacaaaaaabeaaaaaadcaaaaal
dccabaaaafaaaaaaegbabaaaaeaaaaaaegiacaaaaaaaaaaaahaaaaaaogikcaaa
aaaaaaaaahaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES


#ifdef VERTEX

varying highp vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec4 _glesMultiTexCoord1;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (_glesVertex.xyz - ((_World2Object * tmpvar_6).xyz * unity_Scale.w)));
  highp vec3 tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_9 = tmpvar_1.xyz;
  tmpvar_10 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_11;
  tmpvar_11[0].x = tmpvar_9.x;
  tmpvar_11[0].y = tmpvar_10.x;
  tmpvar_11[0].z = tmpvar_2.x;
  tmpvar_11[1].x = tmpvar_9.y;
  tmpvar_11[1].y = tmpvar_10.y;
  tmpvar_11[1].z = tmpvar_2.y;
  tmpvar_11[2].x = tmpvar_9.z;
  tmpvar_11[2].y = tmpvar_10.z;
  tmpvar_11[2].z = tmpvar_2.z;
  vec4 v_12;
  v_12.x = _Object2World[0].x;
  v_12.y = _Object2World[1].x;
  v_12.z = _Object2World[2].x;
  v_12.w = _Object2World[3].x;
  highp vec4 tmpvar_13;
  tmpvar_13.xyz = (tmpvar_11 * v_12.xyz);
  tmpvar_13.w = tmpvar_8.x;
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_13 * unity_Scale.w);
  tmpvar_3 = tmpvar_14;
  vec4 v_15;
  v_15.x = _Object2World[0].y;
  v_15.y = _Object2World[1].y;
  v_15.z = _Object2World[2].y;
  v_15.w = _Object2World[3].y;
  highp vec4 tmpvar_16;
  tmpvar_16.xyz = (tmpvar_11 * v_15.xyz);
  tmpvar_16.w = tmpvar_8.y;
  highp vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * unity_Scale.w);
  tmpvar_4 = tmpvar_17;
  vec4 v_18;
  v_18.x = _Object2World[0].z;
  v_18.y = _Object2World[1].z;
  v_18.z = _Object2World[2].z;
  v_18.w = _Object2World[3].z;
  highp vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_11 * v_18.xyz);
  tmpvar_19.w = tmpvar_8.z;
  highp vec4 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * unity_Scale.w);
  tmpvar_5 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_11 * (((_World2Object * tmpvar_21).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform sampler2D unity_Lightmap;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
void main ()
{
  lowp vec4 c_1;
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  tmpvar_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD1.xyz;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD2.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD3.xyz;
  tmpvar_6 = tmpvar_10;
  tmpvar_2 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_11;
  mediump vec3 HDR_12;
  highp vec4 c_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = tmpvar_4.z;
  tmpvar_14.y = tmpvar_5.z;
  tmpvar_14.z = tmpvar_6.z;
  highp vec3 tmpvar_15;
  tmpvar_15 = (tmpvar_3 - (2.0 * (dot (tmpvar_14, tmpvar_3) * tmpvar_14)));
  lowp vec4 tmpvar_16;
  tmpvar_16 = textureCube (_Cube, tmpvar_15);
  mediump vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * _ReflectColor);
  c_13 = tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_18 = vec3((pow ((((c_13.x + c_13.y) + c_13.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_12 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (_Color, _FresnelColor, vec4(pow ((1.0 - dot (normalize(tmpvar_2), normalize(vec3(0.0, 0.0, 1.0)))), _TransmissiveColor)));
  highp vec3 tmpvar_20;
  tmpvar_20 = ((c_13.xyz + HDR_12) + tmpvar_19.xyz);
  tmpvar_11 = tmpvar_20;
  c_1.xyz = (tmpvar_11 * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD4).xyz));
  c_1.w = 1.0;
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES


#ifdef VERTEX

varying highp vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec4 _glesMultiTexCoord1;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (_glesVertex.xyz - ((_World2Object * tmpvar_6).xyz * unity_Scale.w)));
  highp vec3 tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_9 = tmpvar_1.xyz;
  tmpvar_10 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_11;
  tmpvar_11[0].x = tmpvar_9.x;
  tmpvar_11[0].y = tmpvar_10.x;
  tmpvar_11[0].z = tmpvar_2.x;
  tmpvar_11[1].x = tmpvar_9.y;
  tmpvar_11[1].y = tmpvar_10.y;
  tmpvar_11[1].z = tmpvar_2.y;
  tmpvar_11[2].x = tmpvar_9.z;
  tmpvar_11[2].y = tmpvar_10.z;
  tmpvar_11[2].z = tmpvar_2.z;
  vec4 v_12;
  v_12.x = _Object2World[0].x;
  v_12.y = _Object2World[1].x;
  v_12.z = _Object2World[2].x;
  v_12.w = _Object2World[3].x;
  highp vec4 tmpvar_13;
  tmpvar_13.xyz = (tmpvar_11 * v_12.xyz);
  tmpvar_13.w = tmpvar_8.x;
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_13 * unity_Scale.w);
  tmpvar_3 = tmpvar_14;
  vec4 v_15;
  v_15.x = _Object2World[0].y;
  v_15.y = _Object2World[1].y;
  v_15.z = _Object2World[2].y;
  v_15.w = _Object2World[3].y;
  highp vec4 tmpvar_16;
  tmpvar_16.xyz = (tmpvar_11 * v_15.xyz);
  tmpvar_16.w = tmpvar_8.y;
  highp vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * unity_Scale.w);
  tmpvar_4 = tmpvar_17;
  vec4 v_18;
  v_18.x = _Object2World[0].z;
  v_18.y = _Object2World[1].z;
  v_18.z = _Object2World[2].z;
  v_18.w = _Object2World[3].z;
  highp vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_11 * v_18.xyz);
  tmpvar_19.w = tmpvar_8.z;
  highp vec4 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * unity_Scale.w);
  tmpvar_5 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_11 * (((_World2Object * tmpvar_21).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform sampler2D unity_Lightmap;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
void main ()
{
  lowp vec4 c_1;
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  tmpvar_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD1.xyz;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD2.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD3.xyz;
  tmpvar_6 = tmpvar_10;
  tmpvar_2 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_11;
  mediump vec3 HDR_12;
  highp vec4 c_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = tmpvar_4.z;
  tmpvar_14.y = tmpvar_5.z;
  tmpvar_14.z = tmpvar_6.z;
  highp vec3 tmpvar_15;
  tmpvar_15 = (tmpvar_3 - (2.0 * (dot (tmpvar_14, tmpvar_3) * tmpvar_14)));
  lowp vec4 tmpvar_16;
  tmpvar_16 = textureCube (_Cube, tmpvar_15);
  mediump vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * _ReflectColor);
  c_13 = tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_18 = vec3((pow ((((c_13.x + c_13.y) + c_13.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_12 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (_Color, _FresnelColor, vec4(pow ((1.0 - dot (normalize(tmpvar_2), normalize(vec3(0.0, 0.0, 1.0)))), _TransmissiveColor)));
  highp vec3 tmpvar_20;
  tmpvar_20 = ((c_13.xyz + HDR_12) + tmpvar_19.xyz);
  tmpvar_11 = tmpvar_20;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (unity_Lightmap, xlv_TEXCOORD4);
  c_1.xyz = (tmpvar_11 * ((8.0 * tmpvar_21.w) * tmpvar_21.xyz));
  c_1.w = 1.0;
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Color _glesColor
in vec4 _glesColor;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;
#define gl_MultiTexCoord1 _glesMultiTexCoord1
in vec4 _glesMultiTexCoord1;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
mat2 xll_transpose_mf2x2(mat2 m) {
  return mat2( m[0][0], m[1][0], m[0][1], m[1][1]);
}
mat3 xll_transpose_mf3x3(mat3 m) {
  return mat3( m[0][0], m[1][0], m[2][0],
               m[0][1], m[1][1], m[2][1],
               m[0][2], m[1][2], m[2][2]);
}
mat4 xll_transpose_mf4x4(mat4 m) {
  return mat4( m[0][0], m[1][0], m[2][0], m[3][0],
               m[0][1], m[1][1], m[2][1], m[3][1],
               m[0][2], m[1][2], m[2][2], m[3][2],
               m[0][3], m[1][3], m[2][3], m[3][3]);
}
vec2 xll_matrixindex_mf2x2_i (mat2 m, int i) { vec2 v; v.x=m[0][i]; v.y=m[1][i]; return v; }
vec3 xll_matrixindex_mf3x3_i (mat3 m, int i) { vec3 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; return v; }
vec4 xll_matrixindex_mf4x4_i (mat4 m, int i) { vec4 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; v.w=m[3][i]; return v; }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 399
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 432
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    highp vec2 lmap;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 393
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 397
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 408
#line 442
uniform highp vec4 unity_LightmapST;
#line 462
uniform sampler2D unity_Lightmap;
#line 82
highp vec3 ObjSpaceLightDir( in highp vec4 v ) {
    highp vec3 objSpaceLightPos = (_World2Object * _WorldSpaceLightPos0).xyz;
    return objSpaceLightPos.xyz;
}
#line 91
highp vec3 ObjSpaceViewDir( in highp vec4 v ) {
    highp vec3 objSpaceCameraPos = ((_World2Object * vec4( _WorldSpaceCameraPos.xyz, 1.0)).xyz * unity_Scale.w);
    return (objSpaceCameraPos - v.xyz);
}
#line 443
v2f_surf vert_surf( in appdata_full v ) {
    v2f_surf o;
    #line 446
    o.pos = (glstate_matrix_mvp * v.vertex);
    highp vec3 viewDir = (-ObjSpaceViewDir( v.vertex));
    highp vec3 worldRefl = (mat3( _Object2World) * viewDir);
    highp vec3 binormal = (cross( v.normal, v.tangent.xyz) * v.tangent.w);
    #line 450
    highp mat3 rotation = xll_transpose_mf3x3(mat3( v.tangent.xyz, binormal, v.normal));
    o.TtoW0 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 0).xyz), worldRefl.x) * unity_Scale.w);
    o.TtoW1 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 1).xyz), worldRefl.y) * unity_Scale.w);
    o.TtoW2 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 2).xyz), worldRefl.z) * unity_Scale.w);
    #line 454
    o.lmap.xy = ((v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
    highp vec3 worldN = (mat3( _Object2World) * (v.normal * unity_Scale.w));
    highp vec3 lightDir = (rotation * ObjSpaceLightDir( v.vertex));
    highp vec3 viewDirForLight = (rotation * ObjSpaceViewDir( v.vertex));
    #line 458
    o.viewDir = viewDirForLight;
    return o;
}

out highp vec3 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_TEXCOORD2;
out lowp vec4 xlv_TEXCOORD3;
out highp vec2 xlv_TEXCOORD4;
void main() {
    v2f_surf xl_retval;
    appdata_full xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.tangent = vec4(TANGENT);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xlt_v.texcoord1 = vec4(gl_MultiTexCoord1);
    xlt_v.color = vec4(gl_Color);
    xl_retval = vert_surf( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD0 = vec3(xl_retval.viewDir);
    xlv_TEXCOORD1 = vec4(xl_retval.TtoW0);
    xlv_TEXCOORD2 = vec4(xl_retval.TtoW1);
    xlv_TEXCOORD3 = vec4(xl_retval.TtoW2);
    xlv_TEXCOORD4 = vec2(xl_retval.lmap);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 399
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 432
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    highp vec2 lmap;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 393
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 397
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 408
#line 442
uniform highp vec4 unity_LightmapST;
#line 462
uniform sampler2D unity_Lightmap;
#line 177
lowp vec3 DecodeLightmap( in lowp vec4 color ) {
    #line 179
    return (2.0 * color.xyz);
}
#line 408
void surf( in Input IN, inout SurfaceOutput o ) {
    o.Normal = vec3( 0.0, 0.0, 1.0);
    highp vec3 worldRefl = reflect( IN.worldRefl, vec3( dot( IN.TtoW0, o.Normal), dot( IN.TtoW1, o.Normal), dot( IN.TtoW2, o.Normal)));
    #line 412
    highp vec4 c = (texture( _Cube, worldRefl) * _ReflectColor);
    mediump vec3 HDR = vec3( (pow( (((c.x + c.y) + c.z) / 3.0), _HdrPower) * _HdrContrast));
    mediump float fresnel = (1.0 - dot( normalize(IN.viewDir), normalize(vec3( 0.0, 0.0, 1.0))));
    mediump vec4 fresnelLerp = mix( _Color, _FresnelColor, vec4( pow( fresnel, _TransmissiveColor)));
    #line 416
    mediump vec4 GlossLerp = mix( vec4( 0.0, 0.0, 0.0, 0.0), vec4( 1.0, 1.0, 1.0, 1.0), vec4( fresnel));
    o.Specular = _Shininess;
    o.Gloss = GlossLerp.x;
    o.Albedo = ((c.xyz + HDR) + fresnelLerp.xyz);
    #line 420
    o.Alpha = 1.0;
}
#line 463
lowp vec4 frag_surf( in v2f_surf IN ) {
    Input surfIN;
    #line 466
    surfIN.worldRefl = vec3( IN.TtoW0.w, IN.TtoW1.w, IN.TtoW2.w);
    surfIN.TtoW0 = IN.TtoW0.xyz;
    surfIN.TtoW1 = IN.TtoW1.xyz;
    surfIN.TtoW2 = IN.TtoW2.xyz;
    #line 470
    surfIN.viewDir = IN.viewDir;
    SurfaceOutput o;
    o.Albedo = vec3( 0.0);
    o.Emission = vec3( 0.0);
    #line 474
    o.Specular = 0.0;
    o.Alpha = 0.0;
    o.Gloss = 0.0;
    surf( surfIN, o);
    #line 478
    lowp float atten = 1.0;
    lowp vec4 c = vec4( 0.0);
    lowp vec4 lmtex = texture( unity_Lightmap, IN.lmap.xy);
    lowp vec3 lm = DecodeLightmap( lmtex);
    #line 482
    c.xyz += (o.Albedo * lm);
    c.w = o.Alpha;
    return c;
}
in highp vec3 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_TEXCOORD2;
in lowp vec4 xlv_TEXCOORD3;
in highp vec2 xlv_TEXCOORD4;
void main() {
    lowp vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.viewDir = vec3(xlv_TEXCOORD0);
    xlt_IN.TtoW0 = vec4(xlv_TEXCOORD1);
    xlt_IN.TtoW1 = vec4(xlv_TEXCOORD2);
    xlt_IN.TtoW2 = vec4(xlv_TEXCOORD3);
    xlt_IN.lmap = vec2(xlv_TEXCOORD4);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Vector 13 [_WorldSpaceCameraPos]
Vector 14 [_ProjectionParams]
Vector 15 [_WorldSpaceLightPos0]
Vector 16 [unity_SHAr]
Vector 17 [unity_SHAg]
Vector 18 [unity_SHAb]
Vector 19 [unity_SHBr]
Vector 20 [unity_SHBg]
Vector 21 [unity_SHBb]
Vector 22 [unity_SHC]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 23 [unity_Scale]
"3.0-!!ARBvp1.0
# 63 ALU
PARAM c[24] = { { 1, 0.5 },
		state.matrix.mvp,
		program.local[5..23] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MUL R1.xyz, vertex.normal, c[23].w;
DP3 R2.w, R1, c[6];
DP3 R0.x, R1, c[5];
DP3 R0.z, R1, c[7];
MOV R0.y, R2.w;
MUL R1, R0.xyzz, R0.yzzx;
MOV R0.w, c[0].x;
DP4 R2.z, R0, c[18];
DP4 R2.y, R0, c[17];
DP4 R2.x, R0, c[16];
MUL R0.y, R2.w, R2.w;
DP4 R3.z, R1, c[21];
DP4 R3.y, R1, c[20];
DP4 R3.x, R1, c[19];
ADD R2.xyz, R2, R3;
MAD R0.x, R0, R0, -R0.y;
MUL R3.xyz, R0.x, c[22];
MOV R1.xyz, vertex.attrib[14];
MUL R0.xyz, vertex.normal.zxyw, R1.yzxw;
MAD R0.xyz, vertex.normal.yzxw, R1.zxyw, -R0;
ADD result.texcoord[5].xyz, R2, R3;
MUL R3.xyz, R0, vertex.attrib[14].w;
MOV R0, c[15];
MOV R1.xyz, c[13];
MOV R1.w, c[0].x;
DP4 R2.z, R1, c[11];
DP4 R2.x, R1, c[9];
DP4 R2.y, R1, c[10];
MAD R2.xyz, R2, c[23].w, -vertex.position;
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
DP4 R1.z, R0, c[11];
DP3 R0.w, -R2, c[5];
DP3 result.texcoord[4].y, R3, R1;
DP3 R0.y, R3, c[5];
DP3 R0.x, vertex.attrib[14], c[5];
DP3 R0.z, vertex.normal, c[5];
MUL result.texcoord[1], R0, c[23].w;
DP3 R0.w, -R2, c[6];
DP3 R0.y, R3, c[6];
DP3 R0.x, vertex.attrib[14], c[6];
DP3 R0.z, vertex.normal, c[6];
MUL result.texcoord[2], R0, c[23].w;
DP3 R0.w, -R2, c[7];
DP3 R0.y, R3, c[7];
DP3 R0.x, vertex.attrib[14], c[7];
DP3 R0.z, vertex.normal, c[7];
MUL result.texcoord[3], R0, c[23].w;
DP4 R0.w, vertex.position, c[4];
DP4 R0.z, vertex.position, c[3];
DP3 result.texcoord[4].z, vertex.normal, R1;
DP4 R0.x, vertex.position, c[1];
DP4 R0.y, vertex.position, c[2];
DP3 result.texcoord[4].x, vertex.attrib[14], R1;
DP3 result.texcoord[0].y, R2, R3;
DP3 result.texcoord[0].z, vertex.normal, R2;
DP3 result.texcoord[0].x, R2, vertex.attrib[14];
MUL R2.xyz, R0.xyww, c[0].y;
MOV R1.x, R2;
MUL R1.y, R2, c[14].x;
ADD result.texcoord[6].xy, R1, R2.z;
MOV result.position, R0;
MOV result.texcoord[6].zw, R0;
END
# 63 instructions, 4 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Matrix 0 [glstate_matrix_mvp]
Vector 12 [_WorldSpaceCameraPos]
Vector 13 [_ProjectionParams]
Vector 14 [_ScreenParams]
Vector 15 [_WorldSpaceLightPos0]
Vector 16 [unity_SHAr]
Vector 17 [unity_SHAg]
Vector 18 [unity_SHAb]
Vector 19 [unity_SHBr]
Vector 20 [unity_SHBg]
Vector 21 [unity_SHBb]
Vector 22 [unity_SHC]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 23 [unity_Scale]
"vs_3_0
; 65 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
dcl_texcoord6 o7
def c24, 1.00000000, 0.50000000, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
mul r1.xyz, v2, c23.w
dp3 r2.w, r1, c5
dp3 r0.x, r1, c4
dp3 r0.z, r1, c6
mov r0.y, r2.w
mul r1, r0.xyzz, r0.yzzx
mov r0.w, c24.x
dp4 r2.z, r0, c18
dp4 r2.y, r0, c17
dp4 r2.x, r0, c16
mul r0.y, r2.w, r2.w
dp4 r3.z, r1, c21
dp4 r3.y, r1, c20
dp4 r3.x, r1, c19
add r1.xyz, r2, r3
mad r0.x, r0, r0, -r0.y
mul r2.xyz, r0.x, c22
add o6.xyz, r1, r2
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r0.xyz, v2.yzxw, r0.zxyw, -r1
mul r3.xyz, r0, v1.w
mov r0, c10
dp4 r4.z, c15, r0
mov r0, c9
dp4 r4.y, c15, r0
mov r1.w, c24.x
mov r1.xyz, c12
dp4 r2.z, r1, c10
dp4 r2.x, r1, c8
dp4 r2.y, r1, c9
mad r2.xyz, r2, c23.w, -v0
mov r1, c8
dp4 r4.x, c15, r1
dp3 r0.y, r3, c4
dp3 r0.w, -r2, c4
dp3 r0.x, v1, c4
dp3 r0.z, v2, c4
mul o2, r0, c23.w
dp3 r0.y, r3, c5
dp3 r0.w, -r2, c5
dp3 r0.x, v1, c5
dp3 r0.z, v2, c5
mul o3, r0, c23.w
dp3 r0.y, r3, c6
dp3 r0.w, -r2, c6
dp3 r0.x, v1, c6
dp3 r0.z, v2, c6
mul o4, r0, c23.w
dp4 r0.w, v0, c3
dp4 r0.z, v0, c2
dp4 r0.x, v0, c0
dp4 r0.y, v0, c1
mul r1.xyz, r0.xyww, c24.y
mul r1.y, r1, c13.x
dp3 o1.y, r2, r3
dp3 o5.y, r3, r4
dp3 o1.z, v2, r2
dp3 o1.x, r2, v1
dp3 o5.z, v2, r4
dp3 o5.x, v1, r4
mad o7.xy, r1.z, c14.zwzw, r1
mov o0, r0
mov o7.zw, r0
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "color" Color
ConstBuffer "UnityPerCamera" 128 // 96 used size, 8 vars
Vector 64 [_WorldSpaceCameraPos] 3
Vector 80 [_ProjectionParams] 4
ConstBuffer "UnityLighting" 720 // 720 used size, 17 vars
Vector 0 [_WorldSpaceLightPos0] 4
Vector 608 [unity_SHAr] 4
Vector 624 [unity_SHAg] 4
Vector 640 [unity_SHAb] 4
Vector 656 [unity_SHBr] 4
Vector 672 [unity_SHBg] 4
Vector 688 [unity_SHBb] 4
Vector 704 [unity_SHC] 4
ConstBuffer "UnityPerDraw" 336 // 336 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
Matrix 192 [_Object2World] 4
Matrix 256 [_World2Object] 4
Vector 320 [unity_Scale] 4
BindCB "UnityPerCamera" 0
BindCB "UnityLighting" 1
BindCB "UnityPerDraw" 2
// 70 instructions, 5 temp regs, 0 temp arrays:
// ALU 55 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedfblcgbcgbchdohaldeddgmleojjeafjfabaaaaaacmalaaaaadaaaaaa
cmaaaaaapeaaaaaanmabaaaaejfdeheomaaaaaaaagaaaaaaaiaaaaaajiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaakbaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaakjaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
ahahaaaalaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaadaaaaaaapaaaaaalaaaaaaa
abaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaaljaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaafaaaaaaapaaaaaafaepfdejfeejepeoaafeebeoehefeofeaaeoepfc
enebemaafeeffiedepepfceeaaedepemepfcaaklepfdeheooaaaaaaaaiaaaaaa
aiaaaaaamiaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaneaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaahaiaaaaneaaaaaaabaaaaaaaaaaaaaa
adaaaaaaacaaaaaaapaaaaaaneaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaa
apaaaaaaneaaaaaaadaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaaneaaaaaa
aeaaaaaaaaaaaaaaadaaaaaaafaaaaaaahaiaaaaneaaaaaaafaaaaaaaaaaaaaa
adaaaaaaagaaaaaaahaiaaaaneaaaaaaagaaaaaaaaaaaaaaadaaaaaaahaaaaaa
apaaaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklfdeieefc
eiajaaaaeaaaabaafcacaaaafjaaaaaeegiocaaaaaaaaaaaagaaaaaafjaaaaae
egiocaaaabaaaaaacnaaaaaafjaaaaaeegiocaaaacaaaaaabfaaaaaafpaaaaad
pcbabaaaaaaaaaaafpaaaaadpcbabaaaabaaaaaafpaaaaadhcbabaaaacaaaaaa
ghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaadhccabaaaabaaaaaagfaaaaad
pccabaaaacaaaaaagfaaaaadpccabaaaadaaaaaagfaaaaadpccabaaaaeaaaaaa
gfaaaaadhccabaaaafaaaaaagfaaaaadhccabaaaagaaaaaagfaaaaadpccabaaa
ahaaaaaagiaaaaacafaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaa
egiocaaaacaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaacaaaaaa
aaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaa
egiocaaaacaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaak
pcaabaaaaaaaaaaaegiocaaaacaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaa
aaaaaaaadgaaaaafpccabaaaaaaaaaaaegaobaaaaaaaaaaadiaaaaajhcaabaaa
abaaaaaafgifcaaaaaaaaaaaaeaaaaaaegiccaaaacaaaaaabbaaaaaadcaaaaal
hcaabaaaabaaaaaaegiccaaaacaaaaaabaaaaaaaagiacaaaaaaaaaaaaeaaaaaa
egacbaaaabaaaaaadcaaaaalhcaabaaaabaaaaaaegiccaaaacaaaaaabcaaaaaa
kgikcaaaaaaaaaaaaeaaaaaaegacbaaaabaaaaaaaaaaaaaihcaabaaaabaaaaaa
egacbaaaabaaaaaaegiccaaaacaaaaaabdaaaaaadcaaaaalhcaabaaaabaaaaaa
egacbaaaabaaaaaapgipcaaaacaaaaaabeaaaaaaegbcbaiaebaaaaaaaaaaaaaa
baaaaaahbccabaaaabaaaaaaegbcbaaaabaaaaaaegacbaaaabaaaaaabaaaaaah
eccabaaaabaaaaaaegbcbaaaacaaaaaaegacbaaaabaaaaaadiaaaaahhcaabaaa
acaaaaaajgbebaaaabaaaaaacgbjbaaaacaaaaaadcaaaaakhcaabaaaacaaaaaa
jgbebaaaacaaaaaacgbjbaaaabaaaaaaegacbaiaebaaaaaaacaaaaaadiaaaaah
hcaabaaaacaaaaaaegacbaaaacaaaaaapgbpbaaaabaaaaaabaaaaaahcccabaaa
abaaaaaaegacbaaaacaaaaaaegacbaaaabaaaaaadiaaaaajhcaabaaaadaaaaaa
fgafbaiaebaaaaaaabaaaaaaegiccaaaacaaaaaaanaaaaaadcaaaaallcaabaaa
abaaaaaaegiicaaaacaaaaaaamaaaaaaagaabaiaebaaaaaaabaaaaaaegaibaaa
adaaaaaadcaaaaallcaabaaaabaaaaaaegiicaaaacaaaaaaaoaaaaaakgakbaia
ebaaaaaaabaaaaaaegambaaaabaaaaaadgaaaaaficaabaaaadaaaaaaakaabaaa
abaaaaaadgaaaaagbcaabaaaaeaaaaaaakiacaaaacaaaaaaamaaaaaadgaaaaag
ccaabaaaaeaaaaaaakiacaaaacaaaaaaanaaaaaadgaaaaagecaabaaaaeaaaaaa
akiacaaaacaaaaaaaoaaaaaabaaaaaahccaabaaaadaaaaaaegacbaaaacaaaaaa
egacbaaaaeaaaaaabaaaaaahbcaabaaaadaaaaaaegbcbaaaabaaaaaaegacbaaa
aeaaaaaabaaaaaahecaabaaaadaaaaaaegbcbaaaacaaaaaaegacbaaaaeaaaaaa
diaaaaaipccabaaaacaaaaaaegaobaaaadaaaaaapgipcaaaacaaaaaabeaaaaaa
dgaaaaaficaabaaaadaaaaaabkaabaaaabaaaaaadgaaaaagbcaabaaaaeaaaaaa
bkiacaaaacaaaaaaamaaaaaadgaaaaagccaabaaaaeaaaaaabkiacaaaacaaaaaa
anaaaaaadgaaaaagecaabaaaaeaaaaaabkiacaaaacaaaaaaaoaaaaaabaaaaaah
ccaabaaaadaaaaaaegacbaaaacaaaaaaegacbaaaaeaaaaaabaaaaaahbcaabaaa
adaaaaaaegbcbaaaabaaaaaaegacbaaaaeaaaaaabaaaaaahecaabaaaadaaaaaa
egbcbaaaacaaaaaaegacbaaaaeaaaaaadiaaaaaipccabaaaadaaaaaaegaobaaa
adaaaaaapgipcaaaacaaaaaabeaaaaaadgaaaaagbcaabaaaadaaaaaackiacaaa
acaaaaaaamaaaaaadgaaaaagccaabaaaadaaaaaackiacaaaacaaaaaaanaaaaaa
dgaaaaagecaabaaaadaaaaaackiacaaaacaaaaaaaoaaaaaabaaaaaahccaabaaa
abaaaaaaegacbaaaacaaaaaaegacbaaaadaaaaaabaaaaaahbcaabaaaabaaaaaa
egbcbaaaabaaaaaaegacbaaaadaaaaaabaaaaaahecaabaaaabaaaaaaegbcbaaa
acaaaaaaegacbaaaadaaaaaadiaaaaaipccabaaaaeaaaaaaegaobaaaabaaaaaa
pgipcaaaacaaaaaabeaaaaaadiaaaaajhcaabaaaabaaaaaafgifcaaaabaaaaaa
aaaaaaaaegiccaaaacaaaaaabbaaaaaadcaaaaalhcaabaaaabaaaaaaegiccaaa
acaaaaaabaaaaaaaagiacaaaabaaaaaaaaaaaaaaegacbaaaabaaaaaadcaaaaal
hcaabaaaabaaaaaaegiccaaaacaaaaaabcaaaaaakgikcaaaabaaaaaaaaaaaaaa
egacbaaaabaaaaaadcaaaaalhcaabaaaabaaaaaaegiccaaaacaaaaaabdaaaaaa
pgipcaaaabaaaaaaaaaaaaaaegacbaaaabaaaaaabaaaaaahcccabaaaafaaaaaa
egacbaaaacaaaaaaegacbaaaabaaaaaabaaaaaahbccabaaaafaaaaaaegbcbaaa
abaaaaaaegacbaaaabaaaaaabaaaaaaheccabaaaafaaaaaaegbcbaaaacaaaaaa
egacbaaaabaaaaaadiaaaaaihcaabaaaabaaaaaaegbcbaaaacaaaaaapgipcaaa
acaaaaaabeaaaaaadiaaaaaihcaabaaaacaaaaaafgafbaaaabaaaaaaegiccaaa
acaaaaaaanaaaaaadcaaaaaklcaabaaaabaaaaaaegiicaaaacaaaaaaamaaaaaa
agaabaaaabaaaaaaegaibaaaacaaaaaadcaaaaakhcaabaaaabaaaaaaegiccaaa
acaaaaaaaoaaaaaakgakbaaaabaaaaaaegadbaaaabaaaaaadgaaaaaficaabaaa
abaaaaaaabeaaaaaaaaaiadpbbaaaaaibcaabaaaacaaaaaaegiocaaaabaaaaaa
cgaaaaaaegaobaaaabaaaaaabbaaaaaiccaabaaaacaaaaaaegiocaaaabaaaaaa
chaaaaaaegaobaaaabaaaaaabbaaaaaiecaabaaaacaaaaaaegiocaaaabaaaaaa
ciaaaaaaegaobaaaabaaaaaadiaaaaahpcaabaaaadaaaaaajgacbaaaabaaaaaa
egakbaaaabaaaaaabbaaaaaibcaabaaaaeaaaaaaegiocaaaabaaaaaacjaaaaaa
egaobaaaadaaaaaabbaaaaaiccaabaaaaeaaaaaaegiocaaaabaaaaaackaaaaaa
egaobaaaadaaaaaabbaaaaaiecaabaaaaeaaaaaaegiocaaaabaaaaaaclaaaaaa
egaobaaaadaaaaaaaaaaaaahhcaabaaaacaaaaaaegacbaaaacaaaaaaegacbaaa
aeaaaaaadiaaaaahccaabaaaabaaaaaabkaabaaaabaaaaaabkaabaaaabaaaaaa
dcaaaaakbcaabaaaabaaaaaaakaabaaaabaaaaaaakaabaaaabaaaaaabkaabaia
ebaaaaaaabaaaaaadcaaaaakhccabaaaagaaaaaaegiccaaaabaaaaaacmaaaaaa
agaabaaaabaaaaaaegacbaaaacaaaaaadiaaaaaiccaabaaaaaaaaaaabkaabaaa
aaaaaaaaakiacaaaaaaaaaaaafaaaaaadiaaaaakncaabaaaabaaaaaaagahbaaa
aaaaaaaaaceaaaaaaaaaaadpaaaaaaaaaaaaaadpaaaaaadpdgaaaaafmccabaaa
ahaaaaaakgaobaaaaaaaaaaaaaaaaaahdccabaaaahaaaaaakgakbaaaabaaaaaa
mgaabaaaabaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD6;
varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 unity_SHC;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAr;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  lowp vec4 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (_glesVertex.xyz - ((_World2Object * tmpvar_9).xyz * unity_Scale.w)));
  highp vec3 tmpvar_12;
  highp vec3 tmpvar_13;
  tmpvar_12 = tmpvar_1.xyz;
  tmpvar_13 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_13.x;
  tmpvar_14[0].z = tmpvar_2.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_13.y;
  tmpvar_14[1].z = tmpvar_2.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_13.z;
  tmpvar_14[2].z = tmpvar_2.z;
  vec4 v_15;
  v_15.x = _Object2World[0].x;
  v_15.y = _Object2World[1].x;
  v_15.z = _Object2World[2].x;
  v_15.w = _Object2World[3].x;
  highp vec4 tmpvar_16;
  tmpvar_16.xyz = (tmpvar_14 * v_15.xyz);
  tmpvar_16.w = tmpvar_11.x;
  highp vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * unity_Scale.w);
  tmpvar_4 = tmpvar_17;
  vec4 v_18;
  v_18.x = _Object2World[0].y;
  v_18.y = _Object2World[1].y;
  v_18.z = _Object2World[2].y;
  v_18.w = _Object2World[3].y;
  highp vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_14 * v_18.xyz);
  tmpvar_19.w = tmpvar_11.y;
  highp vec4 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * unity_Scale.w);
  tmpvar_5 = tmpvar_20;
  vec4 v_21;
  v_21.x = _Object2World[0].z;
  v_21.y = _Object2World[1].z;
  v_21.z = _Object2World[2].z;
  v_21.w = _Object2World[3].z;
  highp vec4 tmpvar_22;
  tmpvar_22.xyz = (tmpvar_14 * v_21.xyz);
  tmpvar_22.w = tmpvar_11.z;
  highp vec4 tmpvar_23;
  tmpvar_23 = (tmpvar_22 * unity_Scale.w);
  tmpvar_6 = tmpvar_23;
  mat3 tmpvar_24;
  tmpvar_24[0] = _Object2World[0].xyz;
  tmpvar_24[1] = _Object2World[1].xyz;
  tmpvar_24[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_14 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_7 = tmpvar_25;
  highp vec4 tmpvar_26;
  tmpvar_26.w = 1.0;
  tmpvar_26.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_27;
  tmpvar_27.w = 1.0;
  tmpvar_27.xyz = (tmpvar_24 * (tmpvar_2 * unity_Scale.w));
  mediump vec3 tmpvar_28;
  mediump vec4 normal_29;
  normal_29 = tmpvar_27;
  highp float vC_30;
  mediump vec3 x3_31;
  mediump vec3 x2_32;
  mediump vec3 x1_33;
  highp float tmpvar_34;
  tmpvar_34 = dot (unity_SHAr, normal_29);
  x1_33.x = tmpvar_34;
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHAg, normal_29);
  x1_33.y = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHAb, normal_29);
  x1_33.z = tmpvar_36;
  mediump vec4 tmpvar_37;
  tmpvar_37 = (normal_29.xyzz * normal_29.yzzx);
  highp float tmpvar_38;
  tmpvar_38 = dot (unity_SHBr, tmpvar_37);
  x2_32.x = tmpvar_38;
  highp float tmpvar_39;
  tmpvar_39 = dot (unity_SHBg, tmpvar_37);
  x2_32.y = tmpvar_39;
  highp float tmpvar_40;
  tmpvar_40 = dot (unity_SHBb, tmpvar_37);
  x2_32.z = tmpvar_40;
  mediump float tmpvar_41;
  tmpvar_41 = ((normal_29.x * normal_29.x) - (normal_29.y * normal_29.y));
  vC_30 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42 = (unity_SHC.xyz * vC_30);
  x3_31 = tmpvar_42;
  tmpvar_28 = ((x1_33 + x2_32) + x3_31);
  shlight_3 = tmpvar_28;
  tmpvar_8 = shlight_3;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_14 * (((_World2Object * tmpvar_26).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
  xlv_TEXCOORD6 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD6;
varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform sampler2D _ShadowMapTexture;
uniform lowp vec4 _LightColor0;
uniform highp vec4 _LightShadowData;
void main ()
{
  lowp vec4 c_1;
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  tmpvar_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD1.xyz;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD2.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD3.xyz;
  tmpvar_6 = tmpvar_10;
  tmpvar_2 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_11;
  lowp float tmpvar_12;
  mediump vec3 HDR_13;
  highp vec4 c_14;
  mediump vec3 tmpvar_15;
  tmpvar_15.x = tmpvar_4.z;
  tmpvar_15.y = tmpvar_5.z;
  tmpvar_15.z = tmpvar_6.z;
  highp vec3 tmpvar_16;
  tmpvar_16 = (tmpvar_3 - (2.0 * (dot (tmpvar_15, tmpvar_3) * tmpvar_15)));
  lowp vec4 tmpvar_17;
  tmpvar_17 = textureCube (_Cube, tmpvar_16);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * _ReflectColor);
  c_14 = tmpvar_18;
  highp vec3 tmpvar_19;
  tmpvar_19 = vec3((pow ((((c_14.x + c_14.y) + c_14.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_13 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = (1.0 - dot (normalize(tmpvar_2), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_21;
  tmpvar_21 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_20, _TransmissiveColor)));
  mediump float tmpvar_22;
  tmpvar_22 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_20)).x;
  tmpvar_12 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = ((c_14.xyz + HDR_13) + tmpvar_21.xyz);
  tmpvar_11 = tmpvar_23;
  lowp float tmpvar_24;
  mediump float lightShadowDataX_25;
  highp float dist_26;
  lowp float tmpvar_27;
  tmpvar_27 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD6).x;
  dist_26 = tmpvar_27;
  highp float tmpvar_28;
  tmpvar_28 = _LightShadowData.x;
  lightShadowDataX_25 = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = max (float((dist_26 > (xlv_TEXCOORD6.z / xlv_TEXCOORD6.w))), lightShadowDataX_25);
  tmpvar_24 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = normalize(xlv_TEXCOORD0);
  mediump vec3 lightDir_31;
  lightDir_31 = xlv_TEXCOORD4;
  mediump vec3 viewDir_32;
  viewDir_32 = tmpvar_30;
  mediump float atten_33;
  atten_33 = tmpvar_24;
  mediump vec4 c_34;
  c_34.xyz = (((((((lightDir_31.z * 0.5) + 0.5) * tmpvar_11) * _LightColor0.xyz) * atten_33) * 2.0) + ((_LightColor0.xyz * max (0.0, (pow ((viewDir_32 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_12))) * atten_33));
  c_34.w = 1.0;
  c_1 = c_34;
  c_1.xyz = (c_1.xyz + (tmpvar_11 * xlv_TEXCOORD5));
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD6;
varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_SHC;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAr;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 _ProjectionParams;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  lowp vec4 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 tmpvar_10;
  tmpvar_10.w = 1.0;
  tmpvar_10.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = (tmpvar_11 * (_glesVertex.xyz - ((_World2Object * tmpvar_10).xyz * unity_Scale.w)));
  highp vec3 tmpvar_13;
  highp vec3 tmpvar_14;
  tmpvar_13 = tmpvar_1.xyz;
  tmpvar_14 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_15;
  tmpvar_15[0].x = tmpvar_13.x;
  tmpvar_15[0].y = tmpvar_14.x;
  tmpvar_15[0].z = tmpvar_2.x;
  tmpvar_15[1].x = tmpvar_13.y;
  tmpvar_15[1].y = tmpvar_14.y;
  tmpvar_15[1].z = tmpvar_2.y;
  tmpvar_15[2].x = tmpvar_13.z;
  tmpvar_15[2].y = tmpvar_14.z;
  tmpvar_15[2].z = tmpvar_2.z;
  vec4 v_16;
  v_16.x = _Object2World[0].x;
  v_16.y = _Object2World[1].x;
  v_16.z = _Object2World[2].x;
  v_16.w = _Object2World[3].x;
  highp vec4 tmpvar_17;
  tmpvar_17.xyz = (tmpvar_15 * v_16.xyz);
  tmpvar_17.w = tmpvar_12.x;
  highp vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * unity_Scale.w);
  tmpvar_4 = tmpvar_18;
  vec4 v_19;
  v_19.x = _Object2World[0].y;
  v_19.y = _Object2World[1].y;
  v_19.z = _Object2World[2].y;
  v_19.w = _Object2World[3].y;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = (tmpvar_15 * v_19.xyz);
  tmpvar_20.w = tmpvar_12.y;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * unity_Scale.w);
  tmpvar_5 = tmpvar_21;
  vec4 v_22;
  v_22.x = _Object2World[0].z;
  v_22.y = _Object2World[1].z;
  v_22.z = _Object2World[2].z;
  v_22.w = _Object2World[3].z;
  highp vec4 tmpvar_23;
  tmpvar_23.xyz = (tmpvar_15 * v_22.xyz);
  tmpvar_23.w = tmpvar_12.z;
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * unity_Scale.w);
  tmpvar_6 = tmpvar_24;
  mat3 tmpvar_25;
  tmpvar_25[0] = _Object2World[0].xyz;
  tmpvar_25[1] = _Object2World[1].xyz;
  tmpvar_25[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_26;
  tmpvar_26 = (tmpvar_15 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_7 = tmpvar_26;
  highp vec4 tmpvar_27;
  tmpvar_27.w = 1.0;
  tmpvar_27.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_28;
  tmpvar_28.w = 1.0;
  tmpvar_28.xyz = (tmpvar_25 * (tmpvar_2 * unity_Scale.w));
  mediump vec3 tmpvar_29;
  mediump vec4 normal_30;
  normal_30 = tmpvar_28;
  highp float vC_31;
  mediump vec3 x3_32;
  mediump vec3 x2_33;
  mediump vec3 x1_34;
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHAr, normal_30);
  x1_34.x = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHAg, normal_30);
  x1_34.y = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = dot (unity_SHAb, normal_30);
  x1_34.z = tmpvar_37;
  mediump vec4 tmpvar_38;
  tmpvar_38 = (normal_30.xyzz * normal_30.yzzx);
  highp float tmpvar_39;
  tmpvar_39 = dot (unity_SHBr, tmpvar_38);
  x2_33.x = tmpvar_39;
  highp float tmpvar_40;
  tmpvar_40 = dot (unity_SHBg, tmpvar_38);
  x2_33.y = tmpvar_40;
  highp float tmpvar_41;
  tmpvar_41 = dot (unity_SHBb, tmpvar_38);
  x2_33.z = tmpvar_41;
  mediump float tmpvar_42;
  tmpvar_42 = ((normal_30.x * normal_30.x) - (normal_30.y * normal_30.y));
  vC_31 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43 = (unity_SHC.xyz * vC_31);
  x3_32 = tmpvar_43;
  tmpvar_29 = ((x1_34 + x2_33) + x3_32);
  shlight_3 = tmpvar_29;
  tmpvar_8 = shlight_3;
  highp vec4 o_44;
  highp vec4 tmpvar_45;
  tmpvar_45 = (tmpvar_9 * 0.5);
  highp vec2 tmpvar_46;
  tmpvar_46.x = tmpvar_45.x;
  tmpvar_46.y = (tmpvar_45.y * _ProjectionParams.x);
  o_44.xy = (tmpvar_46 + tmpvar_45.w);
  o_44.zw = tmpvar_9.zw;
  gl_Position = tmpvar_9;
  xlv_TEXCOORD0 = (tmpvar_15 * (((_World2Object * tmpvar_27).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
  xlv_TEXCOORD6 = o_44;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD6;
varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform sampler2D _ShadowMapTexture;
uniform lowp vec4 _LightColor0;
void main ()
{
  lowp vec4 c_1;
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  tmpvar_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD1.xyz;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD2.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD3.xyz;
  tmpvar_6 = tmpvar_10;
  tmpvar_2 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_11;
  lowp float tmpvar_12;
  mediump vec3 HDR_13;
  highp vec4 c_14;
  mediump vec3 tmpvar_15;
  tmpvar_15.x = tmpvar_4.z;
  tmpvar_15.y = tmpvar_5.z;
  tmpvar_15.z = tmpvar_6.z;
  highp vec3 tmpvar_16;
  tmpvar_16 = (tmpvar_3 - (2.0 * (dot (tmpvar_15, tmpvar_3) * tmpvar_15)));
  lowp vec4 tmpvar_17;
  tmpvar_17 = textureCube (_Cube, tmpvar_16);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * _ReflectColor);
  c_14 = tmpvar_18;
  highp vec3 tmpvar_19;
  tmpvar_19 = vec3((pow ((((c_14.x + c_14.y) + c_14.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_13 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = (1.0 - dot (normalize(tmpvar_2), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_21;
  tmpvar_21 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_20, _TransmissiveColor)));
  mediump float tmpvar_22;
  tmpvar_22 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_20)).x;
  tmpvar_12 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = ((c_14.xyz + HDR_13) + tmpvar_21.xyz);
  tmpvar_11 = tmpvar_23;
  lowp float tmpvar_24;
  tmpvar_24 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD6).x;
  highp vec3 tmpvar_25;
  tmpvar_25 = normalize(xlv_TEXCOORD0);
  mediump vec3 lightDir_26;
  lightDir_26 = xlv_TEXCOORD4;
  mediump vec3 viewDir_27;
  viewDir_27 = tmpvar_25;
  mediump float atten_28;
  atten_28 = tmpvar_24;
  mediump vec4 c_29;
  c_29.xyz = (((((((lightDir_26.z * 0.5) + 0.5) * tmpvar_11) * _LightColor0.xyz) * atten_28) * 2.0) + ((_LightColor0.xyz * max (0.0, (pow ((viewDir_27 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_12))) * atten_28));
  c_29.w = 1.0;
  c_1 = c_29;
  c_1.xyz = (c_1.xyz + (tmpvar_11 * xlv_TEXCOORD5));
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Color _glesColor
in vec4 _glesColor;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;
#define gl_MultiTexCoord1 _glesMultiTexCoord1
in vec4 _glesMultiTexCoord1;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
mat2 xll_transpose_mf2x2(mat2 m) {
  return mat2( m[0][0], m[1][0], m[0][1], m[1][1]);
}
mat3 xll_transpose_mf3x3(mat3 m) {
  return mat3( m[0][0], m[1][0], m[2][0],
               m[0][1], m[1][1], m[2][1],
               m[0][2], m[1][2], m[2][2]);
}
mat4 xll_transpose_mf4x4(mat4 m) {
  return mat4( m[0][0], m[1][0], m[2][0], m[3][0],
               m[0][1], m[1][1], m[2][1], m[3][1],
               m[0][2], m[1][2], m[2][2], m[3][2],
               m[0][3], m[1][3], m[2][3], m[3][3]);
}
vec2 xll_matrixindex_mf2x2_i (mat2 m, int i) { vec2 v; v.x=m[0][i]; v.y=m[1][i]; return v; }
vec3 xll_matrixindex_mf3x3_i (mat3 m, int i) { vec3 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; return v; }
vec4 xll_matrixindex_mf4x4_i (mat4 m, int i) { vec4 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; v.w=m[3][i]; return v; }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 407
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 440
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    lowp vec3 lightDir;
    lowp vec3 vlight;
    highp vec4 _ShadowCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform highp vec4 _ShadowOffsets[4];
uniform sampler2D _ShadowMapTexture;
#line 393
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 401
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 405
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 416
#line 452
#line 82
highp vec3 ObjSpaceLightDir( in highp vec4 v ) {
    highp vec3 objSpaceLightPos = (_World2Object * _WorldSpaceLightPos0).xyz;
    return objSpaceLightPos.xyz;
}
#line 91
highp vec3 ObjSpaceViewDir( in highp vec4 v ) {
    highp vec3 objSpaceCameraPos = ((_World2Object * vec4( _WorldSpaceCameraPos.xyz, 1.0)).xyz * unity_Scale.w);
    return (objSpaceCameraPos - v.xyz);
}
#line 137
mediump vec3 ShadeSH9( in mediump vec4 normal ) {
    mediump vec3 x1;
    mediump vec3 x2;
    mediump vec3 x3;
    x1.x = dot( unity_SHAr, normal);
    #line 141
    x1.y = dot( unity_SHAg, normal);
    x1.z = dot( unity_SHAb, normal);
    mediump vec4 vB = (normal.xyzz * normal.yzzx);
    x2.x = dot( unity_SHBr, vB);
    #line 145
    x2.y = dot( unity_SHBg, vB);
    x2.z = dot( unity_SHBb, vB);
    highp float vC = ((normal.x * normal.x) - (normal.y * normal.y));
    x3 = (unity_SHC.xyz * vC);
    #line 149
    return ((x1 + x2) + x3);
}
#line 452
v2f_surf vert_surf( in appdata_full v ) {
    v2f_surf o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 456
    highp vec3 viewDir = (-ObjSpaceViewDir( v.vertex));
    highp vec3 worldRefl = (mat3( _Object2World) * viewDir);
    highp vec3 binormal = (cross( v.normal, v.tangent.xyz) * v.tangent.w);
    highp mat3 rotation = xll_transpose_mf3x3(mat3( v.tangent.xyz, binormal, v.normal));
    #line 460
    o.TtoW0 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 0).xyz), worldRefl.x) * unity_Scale.w);
    o.TtoW1 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 1).xyz), worldRefl.y) * unity_Scale.w);
    o.TtoW2 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 2).xyz), worldRefl.z) * unity_Scale.w);
    highp vec3 worldN = (mat3( _Object2World) * (v.normal * unity_Scale.w));
    #line 464
    highp vec3 lightDir = (rotation * ObjSpaceLightDir( v.vertex));
    o.lightDir = lightDir;
    highp vec3 viewDirForLight = (rotation * ObjSpaceViewDir( v.vertex));
    o.viewDir = viewDirForLight;
    #line 468
    highp vec3 shlight = ShadeSH9( vec4( worldN, 1.0));
    o.vlight = shlight;
    o._ShadowCoord = (unity_World2Shadow[0] * (_Object2World * v.vertex));
    #line 472
    return o;
}

out highp vec3 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_TEXCOORD2;
out lowp vec4 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
out lowp vec3 xlv_TEXCOORD5;
out highp vec4 xlv_TEXCOORD6;
void main() {
    v2f_surf xl_retval;
    appdata_full xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.tangent = vec4(TANGENT);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xlt_v.texcoord1 = vec4(gl_MultiTexCoord1);
    xlt_v.color = vec4(gl_Color);
    xl_retval = vert_surf( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD0 = vec3(xl_retval.viewDir);
    xlv_TEXCOORD1 = vec4(xl_retval.TtoW0);
    xlv_TEXCOORD2 = vec4(xl_retval.TtoW1);
    xlv_TEXCOORD3 = vec4(xl_retval.TtoW2);
    xlv_TEXCOORD4 = vec3(xl_retval.lightDir);
    xlv_TEXCOORD5 = vec3(xl_retval.vlight);
    xlv_TEXCOORD6 = vec4(xl_retval._ShadowCoord);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 407
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 440
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    lowp vec3 lightDir;
    lowp vec3 vlight;
    highp vec4 _ShadowCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform highp vec4 _ShadowOffsets[4];
uniform sampler2D _ShadowMapTexture;
#line 393
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 401
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 405
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 416
#line 452
#line 430
mediump vec4 LightingHalfLambertSpecular( in SurfaceOutput s, in mediump vec3 lightDir, in mediump vec3 viewDir, in mediump float atten ) {
    #line 432
    mediump float NdotL = ((dot( s.Normal, lightDir) * 0.5) + 0.5);
    mediump vec3 halfVector = (viewDir + s.Normal);
    mediump float spec = max( 0.0, (pow( dot( halfVector, s.Normal), (10.0 - s.Specular)) * s.Gloss));
    mediump vec4 c;
    #line 436
    c.xyz = (((((NdotL * s.Albedo) * _LightColor0.xyz) * atten) * 2.0) + ((_LightColor0.xyz * spec) * atten));
    c.w = s.Alpha;
    return c;
}
#line 416
void surf( in Input IN, inout SurfaceOutput o ) {
    o.Normal = vec3( 0.0, 0.0, 1.0);
    highp vec3 worldRefl = reflect( IN.worldRefl, vec3( dot( IN.TtoW0, o.Normal), dot( IN.TtoW1, o.Normal), dot( IN.TtoW2, o.Normal)));
    #line 420
    highp vec4 c = (texture( _Cube, worldRefl) * _ReflectColor);
    mediump vec3 HDR = vec3( (pow( (((c.x + c.y) + c.z) / 3.0), _HdrPower) * _HdrContrast));
    mediump float fresnel = (1.0 - dot( normalize(IN.viewDir), normalize(vec3( 0.0, 0.0, 1.0))));
    mediump vec4 fresnelLerp = mix( _Color, _FresnelColor, vec4( pow( fresnel, _TransmissiveColor)));
    #line 424
    mediump vec4 GlossLerp = mix( vec4( 0.0, 0.0, 0.0, 0.0), vec4( 1.0, 1.0, 1.0, 1.0), vec4( fresnel));
    o.Specular = _Shininess;
    o.Gloss = GlossLerp.x;
    o.Albedo = ((c.xyz + HDR) + fresnelLerp.xyz);
    #line 428
    o.Alpha = 1.0;
}
#line 393
lowp float unitySampleShadow( in highp vec4 shadowCoord ) {
    highp float dist = textureProj( _ShadowMapTexture, shadowCoord).x;
    mediump float lightShadowDataX = _LightShadowData.x;
    #line 397
    return max( float((dist > (shadowCoord.z / shadowCoord.w))), lightShadowDataX);
}
#line 474
lowp vec4 frag_surf( in v2f_surf IN ) {
    #line 476
    Input surfIN;
    surfIN.worldRefl = vec3( IN.TtoW0.w, IN.TtoW1.w, IN.TtoW2.w);
    surfIN.TtoW0 = IN.TtoW0.xyz;
    surfIN.TtoW1 = IN.TtoW1.xyz;
    #line 480
    surfIN.TtoW2 = IN.TtoW2.xyz;
    surfIN.viewDir = IN.viewDir;
    SurfaceOutput o;
    o.Albedo = vec3( 0.0);
    #line 484
    o.Emission = vec3( 0.0);
    o.Specular = 0.0;
    o.Alpha = 0.0;
    o.Gloss = 0.0;
    #line 488
    surf( surfIN, o);
    lowp float atten = unitySampleShadow( IN._ShadowCoord);
    lowp vec4 c = vec4( 0.0);
    c = LightingHalfLambertSpecular( o, IN.lightDir, normalize(IN.viewDir), atten);
    #line 492
    c.xyz += (o.Albedo * IN.vlight);
    return c;
}
in highp vec3 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_TEXCOORD2;
in lowp vec4 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in lowp vec3 xlv_TEXCOORD5;
in highp vec4 xlv_TEXCOORD6;
void main() {
    lowp vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.viewDir = vec3(xlv_TEXCOORD0);
    xlt_IN.TtoW0 = vec4(xlv_TEXCOORD1);
    xlt_IN.TtoW1 = vec4(xlv_TEXCOORD2);
    xlt_IN.TtoW2 = vec4(xlv_TEXCOORD3);
    xlt_IN.lightDir = vec3(xlv_TEXCOORD4);
    xlt_IN.vlight = vec3(xlv_TEXCOORD5);
    xlt_IN._ShadowCoord = vec4(xlv_TEXCOORD6);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Bind "texcoord1" TexCoord1
Vector 13 [_WorldSpaceCameraPos]
Vector 14 [_ProjectionParams]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 16 [unity_Scale]
Vector 17 [unity_LightmapST]
"3.0-!!ARBvp1.0
# 39 ALU
PARAM c[18] = { { 1, 0.5 },
		state.matrix.mvp,
		program.local[5..17] };
TEMP R0;
TEMP R1;
TEMP R2;
MOV R0.xyz, vertex.attrib[14];
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R0.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MUL R2.xyz, R0, vertex.attrib[14].w;
MOV R0.xyz, c[13];
MOV R0.w, c[0].x;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
MAD R1.xyz, R1, c[16].w, -vertex.position;
DP3 R0.w, -R1, c[5];
DP3 result.texcoord[0].y, R1, R2;
DP3 R0.y, R2, c[5];
DP3 R0.x, vertex.attrib[14], c[5];
DP3 R0.z, vertex.normal, c[5];
MUL result.texcoord[1], R0, c[16].w;
DP3 R0.w, -R1, c[6];
DP3 R0.y, R2, c[6];
DP3 R0.x, vertex.attrib[14], c[6];
DP3 R0.z, vertex.normal, c[6];
MUL result.texcoord[2], R0, c[16].w;
DP3 R0.w, -R1, c[7];
DP3 R0.y, R2, c[7];
DP3 R0.x, vertex.attrib[14], c[7];
DP3 R0.z, vertex.normal, c[7];
MUL result.texcoord[3], R0, c[16].w;
DP4 R0.w, vertex.position, c[4];
DP4 R0.z, vertex.position, c[3];
DP3 result.texcoord[0].z, vertex.normal, R1;
DP4 R0.x, vertex.position, c[1];
DP4 R0.y, vertex.position, c[2];
MUL R2.xyz, R0.xyww, c[0].y;
DP3 result.texcoord[0].x, R1, vertex.attrib[14];
MOV R1.x, R2;
MUL R1.y, R2, c[14].x;
ADD result.texcoord[5].xy, R1, R2.z;
MOV result.position, R0;
MOV result.texcoord[5].zw, R0;
MAD result.texcoord[4].xy, vertex.texcoord[1], c[17], c[17].zwzw;
END
# 39 instructions, 3 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "texcoord1" TexCoord1
Matrix 0 [glstate_matrix_mvp]
Vector 12 [_WorldSpaceCameraPos]
Vector 13 [_ProjectionParams]
Vector 14 [_ScreenParams]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 15 [unity_Scale]
Vector 16 [unity_LightmapST]
"vs_3_0
; 40 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
def c17, 1.00000000, 0.50000000, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
dcl_texcoord1 v3
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r0.xyz, v2.yzxw, r0.zxyw, -r1
mul r2.xyz, r0, v1.w
mov r0.xyz, c12
mov r0.w, c17.x
dp4 r1.z, r0, c10
dp4 r1.x, r0, c8
dp4 r1.y, r0, c9
mad r1.xyz, r1, c15.w, -v0
dp3 r0.w, -r1, c4
dp3 o1.y, r1, r2
dp3 r0.y, r2, c4
dp3 r0.x, v1, c4
dp3 r0.z, v2, c4
mul o2, r0, c15.w
dp3 r0.w, -r1, c5
dp3 r0.y, r2, c5
dp3 r0.x, v1, c5
dp3 r0.z, v2, c5
mul o3, r0, c15.w
dp3 r0.w, -r1, c6
dp3 r0.y, r2, c6
dp3 r0.x, v1, c6
dp3 r0.z, v2, c6
mul o4, r0, c15.w
dp4 r0.w, v0, c3
dp4 r0.z, v0, c2
dp3 o1.z, v2, r1
dp4 r0.x, v0, c0
dp4 r0.y, v0, c1
mul r2.xyz, r0.xyww, c17.y
dp3 o1.x, r1, v1
mov r1.x, r2
mul r1.y, r2, c13.x
mad o6.xy, r2.z, c14.zwzw, r1
mov o0, r0
mov o6.zw, r0
mad o5.xy, v3, c16, c16.zwzw
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "texcoord1" TexCoord1
Bind "color" Color
ConstBuffer "$Globals" 192 // 192 used size, 12 vars
Vector 176 [unity_LightmapST] 4
ConstBuffer "UnityPerCamera" 128 // 96 used size, 8 vars
Vector 64 [_WorldSpaceCameraPos] 3
Vector 80 [_ProjectionParams] 4
ConstBuffer "UnityPerDraw" 336 // 336 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
Matrix 192 [_Object2World] 4
Matrix 256 [_World2Object] 4
Vector 320 [unity_Scale] 4
BindCB "$Globals" 0
BindCB "UnityPerCamera" 1
BindCB "UnityPerDraw" 2
// 48 instructions, 5 temp regs, 0 temp arrays:
// ALU 34 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedmlpgcmkfckcghnmcpbdfnbjmkpgmeccnabaaaaaadmaiaaaaadaaaaaa
cmaaaaaapeaaaaaameabaaaaejfdeheomaaaaaaaagaaaaaaaiaaaaaajiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaakbaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaakjaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
ahahaaaalaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaadaaaaaaapaaaaaalaaaaaaa
abaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapadaaaaljaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaafaaaaaaapaaaaaafaepfdejfeejepeoaafeebeoehefeofeaaeoepfc
enebemaafeeffiedepepfceeaaedepemepfcaaklepfdeheomiaaaaaaahaaaaaa
aiaaaaaalaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaalmaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaahaiaaaalmaaaaaaabaaaaaaaaaaaaaa
adaaaaaaacaaaaaaapaaaaaalmaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaa
apaaaaaalmaaaaaaadaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaalmaaaaaa
aeaaaaaaaaaaaaaaadaaaaaaafaaaaaaadamaaaalmaaaaaaafaaaaaaaaaaaaaa
adaaaaaaagaaaaaaapaaaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklklfdeieefchaagaaaaeaaaabaajmabaaaafjaaaaaeegiocaaaaaaaaaaa
amaaaaaafjaaaaaeegiocaaaabaaaaaaagaaaaaafjaaaaaeegiocaaaacaaaaaa
bfaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaadpcbabaaaabaaaaaafpaaaaad
hcbabaaaacaaaaaafpaaaaaddcbabaaaaeaaaaaaghaaaaaepccabaaaaaaaaaaa
abaaaaaagfaaaaadhccabaaaabaaaaaagfaaaaadpccabaaaacaaaaaagfaaaaad
pccabaaaadaaaaaagfaaaaadpccabaaaaeaaaaaagfaaaaaddccabaaaafaaaaaa
gfaaaaadpccabaaaagaaaaaagiaaaaacafaaaaaadiaaaaaipcaabaaaaaaaaaaa
fgbfbaaaaaaaaaaaegiocaaaacaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaa
egiocaaaacaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaak
pcaabaaaaaaaaaaaegiocaaaacaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaa
aaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaacaaaaaaadaaaaaapgbpbaaa
aaaaaaaaegaobaaaaaaaaaaadgaaaaafpccabaaaaaaaaaaaegaobaaaaaaaaaaa
diaaaaajhcaabaaaabaaaaaafgifcaaaabaaaaaaaeaaaaaaegiccaaaacaaaaaa
bbaaaaaadcaaaaalhcaabaaaabaaaaaaegiccaaaacaaaaaabaaaaaaaagiacaaa
abaaaaaaaeaaaaaaegacbaaaabaaaaaadcaaaaalhcaabaaaabaaaaaaegiccaaa
acaaaaaabcaaaaaakgikcaaaabaaaaaaaeaaaaaaegacbaaaabaaaaaaaaaaaaai
hcaabaaaabaaaaaaegacbaaaabaaaaaaegiccaaaacaaaaaabdaaaaaadcaaaaal
hcaabaaaabaaaaaaegacbaaaabaaaaaapgipcaaaacaaaaaabeaaaaaaegbcbaia
ebaaaaaaaaaaaaaabaaaaaahbccabaaaabaaaaaaegbcbaaaabaaaaaaegacbaaa
abaaaaaabaaaaaaheccabaaaabaaaaaaegbcbaaaacaaaaaaegacbaaaabaaaaaa
diaaaaahhcaabaaaacaaaaaajgbebaaaabaaaaaacgbjbaaaacaaaaaadcaaaaak
hcaabaaaacaaaaaajgbebaaaacaaaaaacgbjbaaaabaaaaaaegacbaiaebaaaaaa
acaaaaaadiaaaaahhcaabaaaacaaaaaaegacbaaaacaaaaaapgbpbaaaabaaaaaa
baaaaaahcccabaaaabaaaaaaegacbaaaacaaaaaaegacbaaaabaaaaaadiaaaaaj
hcaabaaaadaaaaaafgafbaiaebaaaaaaabaaaaaaegiccaaaacaaaaaaanaaaaaa
dcaaaaallcaabaaaabaaaaaaegiicaaaacaaaaaaamaaaaaaagaabaiaebaaaaaa
abaaaaaaegaibaaaadaaaaaadcaaaaallcaabaaaabaaaaaaegiicaaaacaaaaaa
aoaaaaaakgakbaiaebaaaaaaabaaaaaaegambaaaabaaaaaadgaaaaaficaabaaa
adaaaaaaakaabaaaabaaaaaadgaaaaagbcaabaaaaeaaaaaaakiacaaaacaaaaaa
amaaaaaadgaaaaagccaabaaaaeaaaaaaakiacaaaacaaaaaaanaaaaaadgaaaaag
ecaabaaaaeaaaaaaakiacaaaacaaaaaaaoaaaaaabaaaaaahccaabaaaadaaaaaa
egacbaaaacaaaaaaegacbaaaaeaaaaaabaaaaaahbcaabaaaadaaaaaaegbcbaaa
abaaaaaaegacbaaaaeaaaaaabaaaaaahecaabaaaadaaaaaaegbcbaaaacaaaaaa
egacbaaaaeaaaaaadiaaaaaipccabaaaacaaaaaaegaobaaaadaaaaaapgipcaaa
acaaaaaabeaaaaaadgaaaaaficaabaaaadaaaaaabkaabaaaabaaaaaadgaaaaag
bcaabaaaaeaaaaaabkiacaaaacaaaaaaamaaaaaadgaaaaagccaabaaaaeaaaaaa
bkiacaaaacaaaaaaanaaaaaadgaaaaagecaabaaaaeaaaaaabkiacaaaacaaaaaa
aoaaaaaabaaaaaahccaabaaaadaaaaaaegacbaaaacaaaaaaegacbaaaaeaaaaaa
baaaaaahbcaabaaaadaaaaaaegbcbaaaabaaaaaaegacbaaaaeaaaaaabaaaaaah
ecaabaaaadaaaaaaegbcbaaaacaaaaaaegacbaaaaeaaaaaadiaaaaaipccabaaa
adaaaaaaegaobaaaadaaaaaapgipcaaaacaaaaaabeaaaaaadgaaaaagbcaabaaa
adaaaaaackiacaaaacaaaaaaamaaaaaadgaaaaagccaabaaaadaaaaaackiacaaa
acaaaaaaanaaaaaadgaaaaagecaabaaaadaaaaaackiacaaaacaaaaaaaoaaaaaa
baaaaaahccaabaaaabaaaaaaegacbaaaacaaaaaaegacbaaaadaaaaaabaaaaaah
bcaabaaaabaaaaaaegbcbaaaabaaaaaaegacbaaaadaaaaaabaaaaaahecaabaaa
abaaaaaaegbcbaaaacaaaaaaegacbaaaadaaaaaadiaaaaaipccabaaaaeaaaaaa
egaobaaaabaaaaaapgipcaaaacaaaaaabeaaaaaadcaaaaaldccabaaaafaaaaaa
egbabaaaaeaaaaaaegiacaaaaaaaaaaaalaaaaaaogikcaaaaaaaaaaaalaaaaaa
diaaaaaiccaabaaaaaaaaaaabkaabaaaaaaaaaaaakiacaaaabaaaaaaafaaaaaa
diaaaaakncaabaaaabaaaaaaagahbaaaaaaaaaaaaceaaaaaaaaaaadpaaaaaaaa
aaaaaadpaaaaaadpdgaaaaafmccabaaaagaaaaaakgaobaaaaaaaaaaaaaaaaaah
dccabaaaagaaaaaakgakbaaaabaaaaaamgaabaaaabaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD5;
varying highp vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec4 _glesMultiTexCoord1;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (_glesVertex.xyz - ((_World2Object * tmpvar_6).xyz * unity_Scale.w)));
  highp vec3 tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_9 = tmpvar_1.xyz;
  tmpvar_10 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_11;
  tmpvar_11[0].x = tmpvar_9.x;
  tmpvar_11[0].y = tmpvar_10.x;
  tmpvar_11[0].z = tmpvar_2.x;
  tmpvar_11[1].x = tmpvar_9.y;
  tmpvar_11[1].y = tmpvar_10.y;
  tmpvar_11[1].z = tmpvar_2.y;
  tmpvar_11[2].x = tmpvar_9.z;
  tmpvar_11[2].y = tmpvar_10.z;
  tmpvar_11[2].z = tmpvar_2.z;
  vec4 v_12;
  v_12.x = _Object2World[0].x;
  v_12.y = _Object2World[1].x;
  v_12.z = _Object2World[2].x;
  v_12.w = _Object2World[3].x;
  highp vec4 tmpvar_13;
  tmpvar_13.xyz = (tmpvar_11 * v_12.xyz);
  tmpvar_13.w = tmpvar_8.x;
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_13 * unity_Scale.w);
  tmpvar_3 = tmpvar_14;
  vec4 v_15;
  v_15.x = _Object2World[0].y;
  v_15.y = _Object2World[1].y;
  v_15.z = _Object2World[2].y;
  v_15.w = _Object2World[3].y;
  highp vec4 tmpvar_16;
  tmpvar_16.xyz = (tmpvar_11 * v_15.xyz);
  tmpvar_16.w = tmpvar_8.y;
  highp vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * unity_Scale.w);
  tmpvar_4 = tmpvar_17;
  vec4 v_18;
  v_18.x = _Object2World[0].z;
  v_18.y = _Object2World[1].z;
  v_18.z = _Object2World[2].z;
  v_18.w = _Object2World[3].z;
  highp vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_11 * v_18.xyz);
  tmpvar_19.w = tmpvar_8.z;
  highp vec4 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * unity_Scale.w);
  tmpvar_5 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_11 * (((_World2Object * tmpvar_21).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD5 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD5;
varying highp vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform sampler2D unity_Lightmap;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform sampler2D _ShadowMapTexture;
uniform highp vec4 _LightShadowData;
void main ()
{
  lowp vec4 c_1;
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  tmpvar_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD1.xyz;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD2.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD3.xyz;
  tmpvar_6 = tmpvar_10;
  tmpvar_2 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_11;
  mediump vec3 HDR_12;
  highp vec4 c_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = tmpvar_4.z;
  tmpvar_14.y = tmpvar_5.z;
  tmpvar_14.z = tmpvar_6.z;
  highp vec3 tmpvar_15;
  tmpvar_15 = (tmpvar_3 - (2.0 * (dot (tmpvar_14, tmpvar_3) * tmpvar_14)));
  lowp vec4 tmpvar_16;
  tmpvar_16 = textureCube (_Cube, tmpvar_15);
  mediump vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * _ReflectColor);
  c_13 = tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_18 = vec3((pow ((((c_13.x + c_13.y) + c_13.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_12 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (_Color, _FresnelColor, vec4(pow ((1.0 - dot (normalize(tmpvar_2), normalize(vec3(0.0, 0.0, 1.0)))), _TransmissiveColor)));
  highp vec3 tmpvar_20;
  tmpvar_20 = ((c_13.xyz + HDR_12) + tmpvar_19.xyz);
  tmpvar_11 = tmpvar_20;
  lowp float tmpvar_21;
  mediump float lightShadowDataX_22;
  highp float dist_23;
  lowp float tmpvar_24;
  tmpvar_24 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD5).x;
  dist_23 = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = _LightShadowData.x;
  lightShadowDataX_22 = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = max (float((dist_23 > (xlv_TEXCOORD5.z / xlv_TEXCOORD5.w))), lightShadowDataX_22);
  tmpvar_21 = tmpvar_26;
  c_1.xyz = (tmpvar_11 * min ((2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD4).xyz), vec3((tmpvar_21 * 2.0))));
  c_1.w = 1.0;
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD5;
varying highp vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _ProjectionParams;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec4 _glesMultiTexCoord1;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (_glesVertex.xyz - ((_World2Object * tmpvar_7).xyz * unity_Scale.w)));
  highp vec3 tmpvar_10;
  highp vec3 tmpvar_11;
  tmpvar_10 = tmpvar_1.xyz;
  tmpvar_11 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_11.x;
  tmpvar_12[0].z = tmpvar_2.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_11.y;
  tmpvar_12[1].z = tmpvar_2.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_11.z;
  tmpvar_12[2].z = tmpvar_2.z;
  vec4 v_13;
  v_13.x = _Object2World[0].x;
  v_13.y = _Object2World[1].x;
  v_13.z = _Object2World[2].x;
  v_13.w = _Object2World[3].x;
  highp vec4 tmpvar_14;
  tmpvar_14.xyz = (tmpvar_12 * v_13.xyz);
  tmpvar_14.w = tmpvar_9.x;
  highp vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_14 * unity_Scale.w);
  tmpvar_3 = tmpvar_15;
  vec4 v_16;
  v_16.x = _Object2World[0].y;
  v_16.y = _Object2World[1].y;
  v_16.z = _Object2World[2].y;
  v_16.w = _Object2World[3].y;
  highp vec4 tmpvar_17;
  tmpvar_17.xyz = (tmpvar_12 * v_16.xyz);
  tmpvar_17.w = tmpvar_9.y;
  highp vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * unity_Scale.w);
  tmpvar_4 = tmpvar_18;
  vec4 v_19;
  v_19.x = _Object2World[0].z;
  v_19.y = _Object2World[1].z;
  v_19.z = _Object2World[2].z;
  v_19.w = _Object2World[3].z;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = (tmpvar_12 * v_19.xyz);
  tmpvar_20.w = tmpvar_9.z;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * unity_Scale.w);
  tmpvar_5 = tmpvar_21;
  highp vec4 tmpvar_22;
  tmpvar_22.w = 1.0;
  tmpvar_22.xyz = _WorldSpaceCameraPos;
  highp vec4 o_23;
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_6 * 0.5);
  highp vec2 tmpvar_25;
  tmpvar_25.x = tmpvar_24.x;
  tmpvar_25.y = (tmpvar_24.y * _ProjectionParams.x);
  o_23.xy = (tmpvar_25 + tmpvar_24.w);
  o_23.zw = tmpvar_6.zw;
  gl_Position = tmpvar_6;
  xlv_TEXCOORD0 = (tmpvar_12 * (((_World2Object * tmpvar_22).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD5 = o_23;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD5;
varying highp vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform sampler2D unity_Lightmap;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform sampler2D _ShadowMapTexture;
void main ()
{
  lowp vec4 c_1;
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  tmpvar_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD1.xyz;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD2.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD3.xyz;
  tmpvar_6 = tmpvar_10;
  tmpvar_2 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_11;
  mediump vec3 HDR_12;
  highp vec4 c_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = tmpvar_4.z;
  tmpvar_14.y = tmpvar_5.z;
  tmpvar_14.z = tmpvar_6.z;
  highp vec3 tmpvar_15;
  tmpvar_15 = (tmpvar_3 - (2.0 * (dot (tmpvar_14, tmpvar_3) * tmpvar_14)));
  lowp vec4 tmpvar_16;
  tmpvar_16 = textureCube (_Cube, tmpvar_15);
  mediump vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * _ReflectColor);
  c_13 = tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_18 = vec3((pow ((((c_13.x + c_13.y) + c_13.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_12 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (_Color, _FresnelColor, vec4(pow ((1.0 - dot (normalize(tmpvar_2), normalize(vec3(0.0, 0.0, 1.0)))), _TransmissiveColor)));
  highp vec3 tmpvar_20;
  tmpvar_20 = ((c_13.xyz + HDR_12) + tmpvar_19.xyz);
  tmpvar_11 = tmpvar_20;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD5);
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture2D (unity_Lightmap, xlv_TEXCOORD4);
  lowp vec3 tmpvar_23;
  tmpvar_23 = ((8.0 * tmpvar_22.w) * tmpvar_22.xyz);
  c_1.xyz = (tmpvar_11 * max (min (tmpvar_23, ((tmpvar_21.x * 2.0) * tmpvar_22.xyz)), (tmpvar_23 * tmpvar_21.x)));
  c_1.w = 1.0;
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Color _glesColor
in vec4 _glesColor;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;
#define gl_MultiTexCoord1 _glesMultiTexCoord1
in vec4 _glesMultiTexCoord1;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
mat2 xll_transpose_mf2x2(mat2 m) {
  return mat2( m[0][0], m[1][0], m[0][1], m[1][1]);
}
mat3 xll_transpose_mf3x3(mat3 m) {
  return mat3( m[0][0], m[1][0], m[2][0],
               m[0][1], m[1][1], m[2][1],
               m[0][2], m[1][2], m[2][2]);
}
mat4 xll_transpose_mf4x4(mat4 m) {
  return mat4( m[0][0], m[1][0], m[2][0], m[3][0],
               m[0][1], m[1][1], m[2][1], m[3][1],
               m[0][2], m[1][2], m[2][2], m[3][2],
               m[0][3], m[1][3], m[2][3], m[3][3]);
}
vec2 xll_matrixindex_mf2x2_i (mat2 m, int i) { vec2 v; v.x=m[0][i]; v.y=m[1][i]; return v; }
vec3 xll_matrixindex_mf3x3_i (mat3 m, int i) { vec3 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; return v; }
vec4 xll_matrixindex_mf4x4_i (mat4 m, int i) { vec4 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; v.w=m[3][i]; return v; }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 407
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 440
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    highp vec2 lmap;
    highp vec4 _ShadowCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform highp vec4 _ShadowOffsets[4];
uniform sampler2D _ShadowMapTexture;
#line 393
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 401
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 405
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 416
#line 451
uniform highp vec4 unity_LightmapST;
#line 472
uniform sampler2D unity_Lightmap;
#line 82
highp vec3 ObjSpaceLightDir( in highp vec4 v ) {
    highp vec3 objSpaceLightPos = (_World2Object * _WorldSpaceLightPos0).xyz;
    return objSpaceLightPos.xyz;
}
#line 91
highp vec3 ObjSpaceViewDir( in highp vec4 v ) {
    highp vec3 objSpaceCameraPos = ((_World2Object * vec4( _WorldSpaceCameraPos.xyz, 1.0)).xyz * unity_Scale.w);
    return (objSpaceCameraPos - v.xyz);
}
#line 452
v2f_surf vert_surf( in appdata_full v ) {
    v2f_surf o;
    #line 455
    o.pos = (glstate_matrix_mvp * v.vertex);
    highp vec3 viewDir = (-ObjSpaceViewDir( v.vertex));
    highp vec3 worldRefl = (mat3( _Object2World) * viewDir);
    highp vec3 binormal = (cross( v.normal, v.tangent.xyz) * v.tangent.w);
    #line 459
    highp mat3 rotation = xll_transpose_mf3x3(mat3( v.tangent.xyz, binormal, v.normal));
    o.TtoW0 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 0).xyz), worldRefl.x) * unity_Scale.w);
    o.TtoW1 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 1).xyz), worldRefl.y) * unity_Scale.w);
    o.TtoW2 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 2).xyz), worldRefl.z) * unity_Scale.w);
    #line 463
    o.lmap.xy = ((v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
    highp vec3 worldN = (mat3( _Object2World) * (v.normal * unity_Scale.w));
    highp vec3 lightDir = (rotation * ObjSpaceLightDir( v.vertex));
    highp vec3 viewDirForLight = (rotation * ObjSpaceViewDir( v.vertex));
    #line 467
    o.viewDir = viewDirForLight;
    o._ShadowCoord = (unity_World2Shadow[0] * (_Object2World * v.vertex));
    return o;
}

out highp vec3 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_TEXCOORD2;
out lowp vec4 xlv_TEXCOORD3;
out highp vec2 xlv_TEXCOORD4;
out highp vec4 xlv_TEXCOORD5;
void main() {
    v2f_surf xl_retval;
    appdata_full xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.tangent = vec4(TANGENT);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xlt_v.texcoord1 = vec4(gl_MultiTexCoord1);
    xlt_v.color = vec4(gl_Color);
    xl_retval = vert_surf( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD0 = vec3(xl_retval.viewDir);
    xlv_TEXCOORD1 = vec4(xl_retval.TtoW0);
    xlv_TEXCOORD2 = vec4(xl_retval.TtoW1);
    xlv_TEXCOORD3 = vec4(xl_retval.TtoW2);
    xlv_TEXCOORD4 = vec2(xl_retval.lmap);
    xlv_TEXCOORD5 = vec4(xl_retval._ShadowCoord);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 407
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 440
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    highp vec2 lmap;
    highp vec4 _ShadowCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform highp vec4 _ShadowOffsets[4];
uniform sampler2D _ShadowMapTexture;
#line 393
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 401
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 405
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 416
#line 451
uniform highp vec4 unity_LightmapST;
#line 472
uniform sampler2D unity_Lightmap;
#line 177
lowp vec3 DecodeLightmap( in lowp vec4 color ) {
    #line 179
    return (2.0 * color.xyz);
}
#line 416
void surf( in Input IN, inout SurfaceOutput o ) {
    o.Normal = vec3( 0.0, 0.0, 1.0);
    highp vec3 worldRefl = reflect( IN.worldRefl, vec3( dot( IN.TtoW0, o.Normal), dot( IN.TtoW1, o.Normal), dot( IN.TtoW2, o.Normal)));
    #line 420
    highp vec4 c = (texture( _Cube, worldRefl) * _ReflectColor);
    mediump vec3 HDR = vec3( (pow( (((c.x + c.y) + c.z) / 3.0), _HdrPower) * _HdrContrast));
    mediump float fresnel = (1.0 - dot( normalize(IN.viewDir), normalize(vec3( 0.0, 0.0, 1.0))));
    mediump vec4 fresnelLerp = mix( _Color, _FresnelColor, vec4( pow( fresnel, _TransmissiveColor)));
    #line 424
    mediump vec4 GlossLerp = mix( vec4( 0.0, 0.0, 0.0, 0.0), vec4( 1.0, 1.0, 1.0, 1.0), vec4( fresnel));
    o.Specular = _Shininess;
    o.Gloss = GlossLerp.x;
    o.Albedo = ((c.xyz + HDR) + fresnelLerp.xyz);
    #line 428
    o.Alpha = 1.0;
}
#line 393
lowp float unitySampleShadow( in highp vec4 shadowCoord ) {
    highp float dist = textureProj( _ShadowMapTexture, shadowCoord).x;
    mediump float lightShadowDataX = _LightShadowData.x;
    #line 397
    return max( float((dist > (shadowCoord.z / shadowCoord.w))), lightShadowDataX);
}
#line 473
lowp vec4 frag_surf( in v2f_surf IN ) {
    Input surfIN;
    #line 476
    surfIN.worldRefl = vec3( IN.TtoW0.w, IN.TtoW1.w, IN.TtoW2.w);
    surfIN.TtoW0 = IN.TtoW0.xyz;
    surfIN.TtoW1 = IN.TtoW1.xyz;
    surfIN.TtoW2 = IN.TtoW2.xyz;
    #line 480
    surfIN.viewDir = IN.viewDir;
    SurfaceOutput o;
    o.Albedo = vec3( 0.0);
    o.Emission = vec3( 0.0);
    #line 484
    o.Specular = 0.0;
    o.Alpha = 0.0;
    o.Gloss = 0.0;
    surf( surfIN, o);
    #line 488
    lowp float atten = unitySampleShadow( IN._ShadowCoord);
    lowp vec4 c = vec4( 0.0);
    lowp vec4 lmtex = texture( unity_Lightmap, IN.lmap.xy);
    lowp vec3 lm = DecodeLightmap( lmtex);
    #line 492
    c.xyz += (o.Albedo * min( lm, vec3( (atten * 2.0))));
    c.w = o.Alpha;
    return c;
}
in highp vec3 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_TEXCOORD2;
in lowp vec4 xlv_TEXCOORD3;
in highp vec2 xlv_TEXCOORD4;
in highp vec4 xlv_TEXCOORD5;
void main() {
    lowp vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.viewDir = vec3(xlv_TEXCOORD0);
    xlt_IN.TtoW0 = vec4(xlv_TEXCOORD1);
    xlt_IN.TtoW1 = vec4(xlv_TEXCOORD2);
    xlt_IN.TtoW2 = vec4(xlv_TEXCOORD3);
    xlt_IN.lmap = vec2(xlv_TEXCOORD4);
    xlt_IN._ShadowCoord = vec4(xlv_TEXCOORD5);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Vector 13 [_WorldSpaceCameraPos]
Vector 14 [_WorldSpaceLightPos0]
Vector 15 [unity_4LightPosX0]
Vector 16 [unity_4LightPosY0]
Vector 17 [unity_4LightPosZ0]
Vector 18 [unity_4LightAtten0]
Vector 19 [unity_LightColor0]
Vector 20 [unity_LightColor1]
Vector 21 [unity_LightColor2]
Vector 22 [unity_LightColor3]
Vector 23 [unity_SHAr]
Vector 24 [unity_SHAg]
Vector 25 [unity_SHAb]
Vector 26 [unity_SHBr]
Vector 27 [unity_SHBg]
Vector 28 [unity_SHBb]
Vector 29 [unity_SHC]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 30 [unity_Scale]
"3.0-!!ARBvp1.0
# 88 ALU
PARAM c[31] = { { 1, 0 },
		state.matrix.mvp,
		program.local[5..30] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
MUL R3.xyz, vertex.normal, c[30].w;
DP4 R0.x, vertex.position, c[6];
ADD R1, -R0.x, c[16];
DP3 R3.w, R3, c[6];
DP3 R4.x, R3, c[5];
DP3 R3.x, R3, c[7];
MUL R2, R3.w, R1;
DP4 R0.x, vertex.position, c[5];
ADD R0, -R0.x, c[15];
MUL R1, R1, R1;
MOV R4.z, R3.x;
MAD R2, R4.x, R0, R2;
MOV R4.w, c[0].x;
DP4 R4.y, vertex.position, c[7];
MAD R1, R0, R0, R1;
ADD R0, -R4.y, c[17];
MAD R1, R0, R0, R1;
MAD R0, R3.x, R0, R2;
MUL R2, R1, c[18];
MOV R4.y, R3.w;
RSQ R1.x, R1.x;
RSQ R1.y, R1.y;
RSQ R1.w, R1.w;
RSQ R1.z, R1.z;
MUL R0, R0, R1;
ADD R1, R2, c[0].x;
RCP R1.x, R1.x;
RCP R1.y, R1.y;
RCP R1.w, R1.w;
RCP R1.z, R1.z;
MAX R0, R0, c[0].y;
MUL R0, R0, R1;
MUL R1.xyz, R0.y, c[20];
MAD R1.xyz, R0.x, c[19], R1;
MAD R0.xyz, R0.z, c[21], R1;
MAD R1.xyz, R0.w, c[22], R0;
MUL R0, R4.xyzz, R4.yzzx;
DP4 R3.z, R0, c[28];
DP4 R3.y, R0, c[27];
DP4 R3.x, R0, c[26];
MUL R1.w, R3, R3;
MAD R0.x, R4, R4, -R1.w;
MOV R1.w, c[0].x;
DP4 R2.z, R4, c[25];
DP4 R2.y, R4, c[24];
DP4 R2.x, R4, c[23];
ADD R2.xyz, R2, R3;
MUL R3.xyz, R0.x, c[29];
ADD R3.xyz, R2, R3;
MOV R0.xyz, vertex.attrib[14];
MUL R2.xyz, vertex.normal.zxyw, R0.yzxw;
ADD result.texcoord[5].xyz, R3, R1;
MAD R0.xyz, vertex.normal.yzxw, R0.zxyw, -R2;
MOV R1.xyz, c[13];
MUL R3.xyz, R0, vertex.attrib[14].w;
MOV R0, c[14];
DP4 R2.z, R1, c[11];
DP4 R2.x, R1, c[9];
DP4 R2.y, R1, c[10];
MAD R2.xyz, R2, c[30].w, -vertex.position;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
DP3 R0.y, R3, c[5];
DP3 R0.w, -R2, c[5];
DP3 R0.x, vertex.attrib[14], c[5];
DP3 R0.z, vertex.normal, c[5];
MUL result.texcoord[1], R0, c[30].w;
DP3 R0.y, R3, c[6];
DP3 R0.w, -R2, c[6];
DP3 R0.x, vertex.attrib[14], c[6];
DP3 R0.z, vertex.normal, c[6];
MUL result.texcoord[2], R0, c[30].w;
DP3 R0.y, R3, c[7];
DP3 R0.w, -R2, c[7];
DP3 R0.x, vertex.attrib[14], c[7];
DP3 R0.z, vertex.normal, c[7];
DP3 result.texcoord[0].y, R2, R3;
DP3 result.texcoord[4].y, R3, R1;
MUL result.texcoord[3], R0, c[30].w;
DP3 result.texcoord[0].z, vertex.normal, R2;
DP3 result.texcoord[0].x, R2, vertex.attrib[14];
DP3 result.texcoord[4].z, vertex.normal, R1;
DP3 result.texcoord[4].x, vertex.attrib[14], R1;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 88 instructions, 5 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Matrix 0 [glstate_matrix_mvp]
Vector 12 [_WorldSpaceCameraPos]
Vector 13 [_WorldSpaceLightPos0]
Vector 14 [unity_4LightPosX0]
Vector 15 [unity_4LightPosY0]
Vector 16 [unity_4LightPosZ0]
Vector 17 [unity_4LightAtten0]
Vector 18 [unity_LightColor0]
Vector 19 [unity_LightColor1]
Vector 20 [unity_LightColor2]
Vector 21 [unity_LightColor3]
Vector 22 [unity_SHAr]
Vector 23 [unity_SHAg]
Vector 24 [unity_SHAb]
Vector 25 [unity_SHBr]
Vector 26 [unity_SHBg]
Vector 27 [unity_SHBb]
Vector 28 [unity_SHC]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 29 [unity_Scale]
"vs_3_0
; 91 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
def c30, 1.00000000, 0.00000000, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
mul r3.xyz, v2, c29.w
dp4 r0.x, v0, c5
add r1, -r0.x, c15
dp3 r3.w, r3, c5
dp3 r4.x, r3, c4
dp3 r3.x, r3, c6
mul r2, r3.w, r1
dp4 r0.x, v0, c4
add r0, -r0.x, c14
mul r1, r1, r1
mov r4.z, r3.x
mad r2, r4.x, r0, r2
mov r4.w, c30.x
dp4 r4.y, v0, c6
mad r1, r0, r0, r1
add r0, -r4.y, c16
mad r1, r0, r0, r1
mad r0, r3.x, r0, r2
mul r2, r1, c17
mov r4.y, r3.w
rsq r1.x, r1.x
rsq r1.y, r1.y
rsq r1.w, r1.w
rsq r1.z, r1.z
mul r0, r0, r1
add r1, r2, c30.x
dp4 r2.z, r4, c24
dp4 r2.y, r4, c23
dp4 r2.x, r4, c22
rcp r1.x, r1.x
rcp r1.y, r1.y
rcp r1.w, r1.w
rcp r1.z, r1.z
max r0, r0, c30.y
mul r0, r0, r1
mul r1.xyz, r0.y, c19
mad r1.xyz, r0.x, c18, r1
mad r0.xyz, r0.z, c20, r1
mad r1.xyz, r0.w, c21, r0
mul r0, r4.xyzz, r4.yzzx
mul r1.w, r3, r3
dp4 r3.z, r0, c27
dp4 r3.y, r0, c26
dp4 r3.x, r0, c25
mad r1.w, r4.x, r4.x, -r1
mul r0.xyz, r1.w, c28
add r2.xyz, r2, r3
add r2.xyz, r2, r0
add o6.xyz, r2, r1
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r0.xyz, v2.yzxw, r0.zxyw, -r1
mul r3.xyz, r0, v1.w
mov r0, c10
dp4 r4.z, c13, r0
mov r0, c9
dp4 r4.y, c13, r0
mov r1.w, c30.x
mov r1.xyz, c12
dp4 r2.z, r1, c10
dp4 r2.x, r1, c8
dp4 r2.y, r1, c9
mad r2.xyz, r2, c29.w, -v0
mov r1, c8
dp4 r4.x, c13, r1
dp3 r0.y, r3, c4
dp3 r0.w, -r2, c4
dp3 r0.x, v1, c4
dp3 r0.z, v2, c4
mul o2, r0, c29.w
dp3 r0.y, r3, c5
dp3 r0.w, -r2, c5
dp3 r0.x, v1, c5
dp3 r0.z, v2, c5
mul o3, r0, c29.w
dp3 r0.y, r3, c6
dp3 r0.w, -r2, c6
dp3 r0.x, v1, c6
dp3 r0.z, v2, c6
dp3 o1.y, r2, r3
dp3 o5.y, r3, r4
mul o4, r0, c29.w
dp3 o1.z, v2, r2
dp3 o1.x, r2, v1
dp3 o5.z, v2, r4
dp3 o5.x, v1, r4
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "color" Color
ConstBuffer "UnityPerCamera" 128 // 76 used size, 8 vars
Vector 64 [_WorldSpaceCameraPos] 3
ConstBuffer "UnityLighting" 720 // 720 used size, 17 vars
Vector 0 [_WorldSpaceLightPos0] 4
Vector 32 [unity_4LightPosX0] 4
Vector 48 [unity_4LightPosY0] 4
Vector 64 [unity_4LightPosZ0] 4
Vector 80 [unity_4LightAtten0] 4
Vector 96 [unity_LightColor0] 4
Vector 112 [unity_LightColor1] 4
Vector 128 [unity_LightColor2] 4
Vector 144 [unity_LightColor3] 4
Vector 160 [unity_LightColor4] 4
Vector 176 [unity_LightColor5] 4
Vector 192 [unity_LightColor6] 4
Vector 208 [unity_LightColor7] 4
Vector 608 [unity_SHAr] 4
Vector 624 [unity_SHAg] 4
Vector 640 [unity_SHAb] 4
Vector 656 [unity_SHBr] 4
Vector 672 [unity_SHBg] 4
Vector 688 [unity_SHBb] 4
Vector 704 [unity_SHC] 4
ConstBuffer "UnityPerDraw" 336 // 336 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
Matrix 192 [_Object2World] 4
Matrix 256 [_World2Object] 4
Vector 320 [unity_Scale] 4
BindCB "UnityPerCamera" 0
BindCB "UnityLighting" 1
BindCB "UnityPerDraw" 2
// 89 instructions, 6 temp regs, 0 temp arrays:
// ALU 76 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedmjefgcodfnhfaplojkpnfnkmmkkcjngfabaaaaaammanaaaaadaaaaaa
cmaaaaaapeaaaaaameabaaaaejfdeheomaaaaaaaagaaaaaaaiaaaaaajiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaakbaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaakjaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
ahahaaaalaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaadaaaaaaapaaaaaalaaaaaaa
abaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaaljaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaafaaaaaaapaaaaaafaepfdejfeejepeoaafeebeoehefeofeaaeoepfc
enebemaafeeffiedepepfceeaaedepemepfcaaklepfdeheomiaaaaaaahaaaaaa
aiaaaaaalaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaalmaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaahaiaaaalmaaaaaaabaaaaaaaaaaaaaa
adaaaaaaacaaaaaaapaaaaaalmaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaa
apaaaaaalmaaaaaaadaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaalmaaaaaa
aeaaaaaaaaaaaaaaadaaaaaaafaaaaaaahaiaaaalmaaaaaaafaaaaaaaaaaaaaa
adaaaaaaagaaaaaaahaiaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklklfdeieefcaaamaaaaeaaaabaaaaadaaaafjaaaaaeegiocaaaaaaaaaaa
afaaaaaafjaaaaaeegiocaaaabaaaaaacnaaaaaafjaaaaaeegiocaaaacaaaaaa
bfaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaadpcbabaaaabaaaaaafpaaaaad
hcbabaaaacaaaaaaghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaadhccabaaa
abaaaaaagfaaaaadpccabaaaacaaaaaagfaaaaadpccabaaaadaaaaaagfaaaaad
pccabaaaaeaaaaaagfaaaaadhccabaaaafaaaaaagfaaaaadhccabaaaagaaaaaa
giaaaaacagaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaa
acaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaacaaaaaaaaaaaaaa
agbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaa
acaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaa
aaaaaaaaegiocaaaacaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaa
diaaaaajhcaabaaaaaaaaaaafgifcaaaaaaaaaaaaeaaaaaaegiccaaaacaaaaaa
bbaaaaaadcaaaaalhcaabaaaaaaaaaaaegiccaaaacaaaaaabaaaaaaaagiacaaa
aaaaaaaaaeaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaaaaaaaaaegiccaaa
acaaaaaabcaaaaaakgikcaaaaaaaaaaaaeaaaaaaegacbaaaaaaaaaaaaaaaaaai
hcaabaaaaaaaaaaaegacbaaaaaaaaaaaegiccaaaacaaaaaabdaaaaaadcaaaaal
hcaabaaaaaaaaaaaegacbaaaaaaaaaaapgipcaaaacaaaaaabeaaaaaaegbcbaia
ebaaaaaaaaaaaaaabaaaaaahbccabaaaabaaaaaaegbcbaaaabaaaaaaegacbaaa
aaaaaaaabaaaaaaheccabaaaabaaaaaaegbcbaaaacaaaaaaegacbaaaaaaaaaaa
diaaaaahhcaabaaaabaaaaaajgbebaaaabaaaaaacgbjbaaaacaaaaaadcaaaaak
hcaabaaaabaaaaaajgbebaaaacaaaaaacgbjbaaaabaaaaaaegacbaiaebaaaaaa
abaaaaaadiaaaaahhcaabaaaabaaaaaaegacbaaaabaaaaaapgbpbaaaabaaaaaa
baaaaaahcccabaaaabaaaaaaegacbaaaabaaaaaaegacbaaaaaaaaaaadiaaaaaj
hcaabaaaacaaaaaafgafbaiaebaaaaaaaaaaaaaaegiccaaaacaaaaaaanaaaaaa
dcaaaaallcaabaaaaaaaaaaaegiicaaaacaaaaaaamaaaaaaagaabaiaebaaaaaa
aaaaaaaaegaibaaaacaaaaaadcaaaaallcaabaaaaaaaaaaaegiicaaaacaaaaaa
aoaaaaaakgakbaiaebaaaaaaaaaaaaaaegambaaaaaaaaaaadgaaaaaficaabaaa
acaaaaaaakaabaaaaaaaaaaadgaaaaagbcaabaaaadaaaaaaakiacaaaacaaaaaa
amaaaaaadgaaaaagccaabaaaadaaaaaaakiacaaaacaaaaaaanaaaaaadgaaaaag
ecaabaaaadaaaaaaakiacaaaacaaaaaaaoaaaaaabaaaaaahccaabaaaacaaaaaa
egacbaaaabaaaaaaegacbaaaadaaaaaabaaaaaahbcaabaaaacaaaaaaegbcbaaa
abaaaaaaegacbaaaadaaaaaabaaaaaahecaabaaaacaaaaaaegbcbaaaacaaaaaa
egacbaaaadaaaaaadiaaaaaipccabaaaacaaaaaaegaobaaaacaaaaaapgipcaaa
acaaaaaabeaaaaaadgaaaaaficaabaaaacaaaaaabkaabaaaaaaaaaaadgaaaaag
bcaabaaaadaaaaaabkiacaaaacaaaaaaamaaaaaadgaaaaagccaabaaaadaaaaaa
bkiacaaaacaaaaaaanaaaaaadgaaaaagecaabaaaadaaaaaabkiacaaaacaaaaaa
aoaaaaaabaaaaaahccaabaaaacaaaaaaegacbaaaabaaaaaaegacbaaaadaaaaaa
baaaaaahbcaabaaaacaaaaaaegbcbaaaabaaaaaaegacbaaaadaaaaaabaaaaaah
ecaabaaaacaaaaaaegbcbaaaacaaaaaaegacbaaaadaaaaaadiaaaaaipccabaaa
adaaaaaaegaobaaaacaaaaaapgipcaaaacaaaaaabeaaaaaadgaaaaagbcaabaaa
acaaaaaackiacaaaacaaaaaaamaaaaaadgaaaaagccaabaaaacaaaaaackiacaaa
acaaaaaaanaaaaaadgaaaaagecaabaaaacaaaaaackiacaaaacaaaaaaaoaaaaaa
baaaaaahccaabaaaaaaaaaaaegacbaaaabaaaaaaegacbaaaacaaaaaabaaaaaah
bcaabaaaaaaaaaaaegbcbaaaabaaaaaaegacbaaaacaaaaaabaaaaaahecaabaaa
aaaaaaaaegbcbaaaacaaaaaaegacbaaaacaaaaaadiaaaaaipccabaaaaeaaaaaa
egaobaaaaaaaaaaapgipcaaaacaaaaaabeaaaaaadiaaaaajhcaabaaaaaaaaaaa
fgifcaaaabaaaaaaaaaaaaaaegiccaaaacaaaaaabbaaaaaadcaaaaalhcaabaaa
aaaaaaaaegiccaaaacaaaaaabaaaaaaaagiacaaaabaaaaaaaaaaaaaaegacbaaa
aaaaaaaadcaaaaalhcaabaaaaaaaaaaaegiccaaaacaaaaaabcaaaaaakgikcaaa
abaaaaaaaaaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaaaaaaaaaegiccaaa
acaaaaaabdaaaaaapgipcaaaabaaaaaaaaaaaaaaegacbaaaaaaaaaaabaaaaaah
cccabaaaafaaaaaaegacbaaaabaaaaaaegacbaaaaaaaaaaabaaaaaahbccabaaa
afaaaaaaegbcbaaaabaaaaaaegacbaaaaaaaaaaabaaaaaaheccabaaaafaaaaaa
egbcbaaaacaaaaaaegacbaaaaaaaaaaadgaaaaaficaabaaaaaaaaaaaabeaaaaa
aaaaiadpdiaaaaaihcaabaaaabaaaaaaegbcbaaaacaaaaaapgipcaaaacaaaaaa
beaaaaaadiaaaaaihcaabaaaacaaaaaafgafbaaaabaaaaaaegiccaaaacaaaaaa
anaaaaaadcaaaaaklcaabaaaabaaaaaaegiicaaaacaaaaaaamaaaaaaagaabaaa
abaaaaaaegaibaaaacaaaaaadcaaaaakhcaabaaaaaaaaaaaegiccaaaacaaaaaa
aoaaaaaakgakbaaaabaaaaaaegadbaaaabaaaaaabbaaaaaibcaabaaaabaaaaaa
egiocaaaabaaaaaacgaaaaaaegaobaaaaaaaaaaabbaaaaaiccaabaaaabaaaaaa
egiocaaaabaaaaaachaaaaaaegaobaaaaaaaaaaabbaaaaaiecaabaaaabaaaaaa
egiocaaaabaaaaaaciaaaaaaegaobaaaaaaaaaaadiaaaaahpcaabaaaacaaaaaa
jgacbaaaaaaaaaaaegakbaaaaaaaaaaabbaaaaaibcaabaaaadaaaaaaegiocaaa
abaaaaaacjaaaaaaegaobaaaacaaaaaabbaaaaaiccaabaaaadaaaaaaegiocaaa
abaaaaaackaaaaaaegaobaaaacaaaaaabbaaaaaiecaabaaaadaaaaaaegiocaaa
abaaaaaaclaaaaaaegaobaaaacaaaaaaaaaaaaahhcaabaaaabaaaaaaegacbaaa
abaaaaaaegacbaaaadaaaaaadiaaaaahicaabaaaaaaaaaaabkaabaaaaaaaaaaa
bkaabaaaaaaaaaaadcaaaaakicaabaaaaaaaaaaaakaabaaaaaaaaaaaakaabaaa
aaaaaaaadkaabaiaebaaaaaaaaaaaaaadcaaaaakhcaabaaaabaaaaaaegiccaaa
abaaaaaacmaaaaaapgapbaaaaaaaaaaaegacbaaaabaaaaaadiaaaaaihcaabaaa
acaaaaaafgbfbaaaaaaaaaaaegiccaaaacaaaaaaanaaaaaadcaaaaakhcaabaaa
acaaaaaaegiccaaaacaaaaaaamaaaaaaagbabaaaaaaaaaaaegacbaaaacaaaaaa
dcaaaaakhcaabaaaacaaaaaaegiccaaaacaaaaaaaoaaaaaakgbkbaaaaaaaaaaa
egacbaaaacaaaaaadcaaaaakhcaabaaaacaaaaaaegiccaaaacaaaaaaapaaaaaa
pgbpbaaaaaaaaaaaegacbaaaacaaaaaaaaaaaaajpcaabaaaadaaaaaafgafbaia
ebaaaaaaacaaaaaaegiocaaaabaaaaaaadaaaaaadiaaaaahpcaabaaaaeaaaaaa
fgafbaaaaaaaaaaaegaobaaaadaaaaaadiaaaaahpcaabaaaadaaaaaaegaobaaa
adaaaaaaegaobaaaadaaaaaaaaaaaaajpcaabaaaafaaaaaaagaabaiaebaaaaaa
acaaaaaaegiocaaaabaaaaaaacaaaaaaaaaaaaajpcaabaaaacaaaaaakgakbaia
ebaaaaaaacaaaaaaegiocaaaabaaaaaaaeaaaaaadcaaaaajpcaabaaaaeaaaaaa
egaobaaaafaaaaaaagaabaaaaaaaaaaaegaobaaaaeaaaaaadcaaaaajpcaabaaa
aaaaaaaaegaobaaaacaaaaaakgakbaaaaaaaaaaaegaobaaaaeaaaaaadcaaaaaj
pcaabaaaadaaaaaaegaobaaaafaaaaaaegaobaaaafaaaaaaegaobaaaadaaaaaa
dcaaaaajpcaabaaaacaaaaaaegaobaaaacaaaaaaegaobaaaacaaaaaaegaobaaa
adaaaaaaeeaaaaafpcaabaaaadaaaaaaegaobaaaacaaaaaadcaaaaanpcaabaaa
acaaaaaaegaobaaaacaaaaaaegiocaaaabaaaaaaafaaaaaaaceaaaaaaaaaiadp
aaaaiadpaaaaiadpaaaaiadpaoaaaaakpcaabaaaacaaaaaaaceaaaaaaaaaiadp
aaaaiadpaaaaiadpaaaaiadpegaobaaaacaaaaaadiaaaaahpcaabaaaaaaaaaaa
egaobaaaaaaaaaaaegaobaaaadaaaaaadeaaaaakpcaabaaaaaaaaaaaegaobaaa
aaaaaaaaaceaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaadiaaaaahpcaabaaa
aaaaaaaaegaobaaaacaaaaaaegaobaaaaaaaaaaadiaaaaaihcaabaaaacaaaaaa
fgafbaaaaaaaaaaaegiccaaaabaaaaaaahaaaaaadcaaaaakhcaabaaaacaaaaaa
egiccaaaabaaaaaaagaaaaaaagaabaaaaaaaaaaaegacbaaaacaaaaaadcaaaaak
hcaabaaaaaaaaaaaegiccaaaabaaaaaaaiaaaaaakgakbaaaaaaaaaaaegacbaaa
acaaaaaadcaaaaakhcaabaaaaaaaaaaaegiccaaaabaaaaaaajaaaaaapgapbaaa
aaaaaaaaegacbaaaaaaaaaaaaaaaaaahhccabaaaagaaaaaaegacbaaaaaaaaaaa
egacbaaaabaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_SHC;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosX0;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  lowp vec4 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (_glesVertex.xyz - ((_World2Object * tmpvar_9).xyz * unity_Scale.w)));
  highp vec3 tmpvar_12;
  highp vec3 tmpvar_13;
  tmpvar_12 = tmpvar_1.xyz;
  tmpvar_13 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_13.x;
  tmpvar_14[0].z = tmpvar_2.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_13.y;
  tmpvar_14[1].z = tmpvar_2.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_13.z;
  tmpvar_14[2].z = tmpvar_2.z;
  vec4 v_15;
  v_15.x = _Object2World[0].x;
  v_15.y = _Object2World[1].x;
  v_15.z = _Object2World[2].x;
  v_15.w = _Object2World[3].x;
  highp vec4 tmpvar_16;
  tmpvar_16.xyz = (tmpvar_14 * v_15.xyz);
  tmpvar_16.w = tmpvar_11.x;
  highp vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * unity_Scale.w);
  tmpvar_4 = tmpvar_17;
  vec4 v_18;
  v_18.x = _Object2World[0].y;
  v_18.y = _Object2World[1].y;
  v_18.z = _Object2World[2].y;
  v_18.w = _Object2World[3].y;
  highp vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_14 * v_18.xyz);
  tmpvar_19.w = tmpvar_11.y;
  highp vec4 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * unity_Scale.w);
  tmpvar_5 = tmpvar_20;
  vec4 v_21;
  v_21.x = _Object2World[0].z;
  v_21.y = _Object2World[1].z;
  v_21.z = _Object2World[2].z;
  v_21.w = _Object2World[3].z;
  highp vec4 tmpvar_22;
  tmpvar_22.xyz = (tmpvar_14 * v_21.xyz);
  tmpvar_22.w = tmpvar_11.z;
  highp vec4 tmpvar_23;
  tmpvar_23 = (tmpvar_22 * unity_Scale.w);
  tmpvar_6 = tmpvar_23;
  mat3 tmpvar_24;
  tmpvar_24[0] = _Object2World[0].xyz;
  tmpvar_24[1] = _Object2World[1].xyz;
  tmpvar_24[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_24 * (tmpvar_2 * unity_Scale.w));
  highp vec3 tmpvar_26;
  tmpvar_26 = (tmpvar_14 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_7 = tmpvar_26;
  highp vec4 tmpvar_27;
  tmpvar_27.w = 1.0;
  tmpvar_27.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_28;
  tmpvar_28.w = 1.0;
  tmpvar_28.xyz = tmpvar_25;
  mediump vec3 tmpvar_29;
  mediump vec4 normal_30;
  normal_30 = tmpvar_28;
  highp float vC_31;
  mediump vec3 x3_32;
  mediump vec3 x2_33;
  mediump vec3 x1_34;
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHAr, normal_30);
  x1_34.x = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHAg, normal_30);
  x1_34.y = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = dot (unity_SHAb, normal_30);
  x1_34.z = tmpvar_37;
  mediump vec4 tmpvar_38;
  tmpvar_38 = (normal_30.xyzz * normal_30.yzzx);
  highp float tmpvar_39;
  tmpvar_39 = dot (unity_SHBr, tmpvar_38);
  x2_33.x = tmpvar_39;
  highp float tmpvar_40;
  tmpvar_40 = dot (unity_SHBg, tmpvar_38);
  x2_33.y = tmpvar_40;
  highp float tmpvar_41;
  tmpvar_41 = dot (unity_SHBb, tmpvar_38);
  x2_33.z = tmpvar_41;
  mediump float tmpvar_42;
  tmpvar_42 = ((normal_30.x * normal_30.x) - (normal_30.y * normal_30.y));
  vC_31 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43 = (unity_SHC.xyz * vC_31);
  x3_32 = tmpvar_43;
  tmpvar_29 = ((x1_34 + x2_33) + x3_32);
  shlight_3 = tmpvar_29;
  tmpvar_8 = shlight_3;
  highp vec3 tmpvar_44;
  tmpvar_44 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_45;
  tmpvar_45 = (unity_4LightPosX0 - tmpvar_44.x);
  highp vec4 tmpvar_46;
  tmpvar_46 = (unity_4LightPosY0 - tmpvar_44.y);
  highp vec4 tmpvar_47;
  tmpvar_47 = (unity_4LightPosZ0 - tmpvar_44.z);
  highp vec4 tmpvar_48;
  tmpvar_48 = (((tmpvar_45 * tmpvar_45) + (tmpvar_46 * tmpvar_46)) + (tmpvar_47 * tmpvar_47));
  highp vec4 tmpvar_49;
  tmpvar_49 = (max (vec4(0.0, 0.0, 0.0, 0.0), ((((tmpvar_45 * tmpvar_25.x) + (tmpvar_46 * tmpvar_25.y)) + (tmpvar_47 * tmpvar_25.z)) * inversesqrt(tmpvar_48))) * (1.0/((1.0 + (tmpvar_48 * unity_4LightAtten0)))));
  highp vec3 tmpvar_50;
  tmpvar_50 = (tmpvar_8 + ((((unity_LightColor[0].xyz * tmpvar_49.x) + (unity_LightColor[1].xyz * tmpvar_49.y)) + (unity_LightColor[2].xyz * tmpvar_49.z)) + (unity_LightColor[3].xyz * tmpvar_49.w)));
  tmpvar_8 = tmpvar_50;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_14 * (((_World2Object * tmpvar_27).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
}



#endif
#ifdef FRAGMENT

varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform lowp vec4 _LightColor0;
void main ()
{
  lowp vec4 c_1;
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  tmpvar_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD1.xyz;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD2.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD3.xyz;
  tmpvar_6 = tmpvar_10;
  tmpvar_2 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_11;
  lowp float tmpvar_12;
  mediump vec3 HDR_13;
  highp vec4 c_14;
  mediump vec3 tmpvar_15;
  tmpvar_15.x = tmpvar_4.z;
  tmpvar_15.y = tmpvar_5.z;
  tmpvar_15.z = tmpvar_6.z;
  highp vec3 tmpvar_16;
  tmpvar_16 = (tmpvar_3 - (2.0 * (dot (tmpvar_15, tmpvar_3) * tmpvar_15)));
  lowp vec4 tmpvar_17;
  tmpvar_17 = textureCube (_Cube, tmpvar_16);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * _ReflectColor);
  c_14 = tmpvar_18;
  highp vec3 tmpvar_19;
  tmpvar_19 = vec3((pow ((((c_14.x + c_14.y) + c_14.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_13 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = (1.0 - dot (normalize(tmpvar_2), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_21;
  tmpvar_21 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_20, _TransmissiveColor)));
  mediump float tmpvar_22;
  tmpvar_22 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_20)).x;
  tmpvar_12 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = ((c_14.xyz + HDR_13) + tmpvar_21.xyz);
  tmpvar_11 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = normalize(xlv_TEXCOORD0);
  mediump vec3 lightDir_25;
  lightDir_25 = xlv_TEXCOORD4;
  mediump vec3 viewDir_26;
  viewDir_26 = tmpvar_24;
  mediump vec4 c_27;
  c_27.xyz = ((((((lightDir_25.z * 0.5) + 0.5) * tmpvar_11) * _LightColor0.xyz) * 2.0) + (_LightColor0.xyz * max (0.0, (pow ((viewDir_26 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_12))));
  c_27.w = 1.0;
  c_1 = c_27;
  c_1.xyz = (c_1.xyz + (tmpvar_11 * xlv_TEXCOORD5));
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_SHC;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosX0;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  lowp vec4 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (_glesVertex.xyz - ((_World2Object * tmpvar_9).xyz * unity_Scale.w)));
  highp vec3 tmpvar_12;
  highp vec3 tmpvar_13;
  tmpvar_12 = tmpvar_1.xyz;
  tmpvar_13 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_13.x;
  tmpvar_14[0].z = tmpvar_2.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_13.y;
  tmpvar_14[1].z = tmpvar_2.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_13.z;
  tmpvar_14[2].z = tmpvar_2.z;
  vec4 v_15;
  v_15.x = _Object2World[0].x;
  v_15.y = _Object2World[1].x;
  v_15.z = _Object2World[2].x;
  v_15.w = _Object2World[3].x;
  highp vec4 tmpvar_16;
  tmpvar_16.xyz = (tmpvar_14 * v_15.xyz);
  tmpvar_16.w = tmpvar_11.x;
  highp vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * unity_Scale.w);
  tmpvar_4 = tmpvar_17;
  vec4 v_18;
  v_18.x = _Object2World[0].y;
  v_18.y = _Object2World[1].y;
  v_18.z = _Object2World[2].y;
  v_18.w = _Object2World[3].y;
  highp vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_14 * v_18.xyz);
  tmpvar_19.w = tmpvar_11.y;
  highp vec4 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * unity_Scale.w);
  tmpvar_5 = tmpvar_20;
  vec4 v_21;
  v_21.x = _Object2World[0].z;
  v_21.y = _Object2World[1].z;
  v_21.z = _Object2World[2].z;
  v_21.w = _Object2World[3].z;
  highp vec4 tmpvar_22;
  tmpvar_22.xyz = (tmpvar_14 * v_21.xyz);
  tmpvar_22.w = tmpvar_11.z;
  highp vec4 tmpvar_23;
  tmpvar_23 = (tmpvar_22 * unity_Scale.w);
  tmpvar_6 = tmpvar_23;
  mat3 tmpvar_24;
  tmpvar_24[0] = _Object2World[0].xyz;
  tmpvar_24[1] = _Object2World[1].xyz;
  tmpvar_24[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_24 * (tmpvar_2 * unity_Scale.w));
  highp vec3 tmpvar_26;
  tmpvar_26 = (tmpvar_14 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_7 = tmpvar_26;
  highp vec4 tmpvar_27;
  tmpvar_27.w = 1.0;
  tmpvar_27.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_28;
  tmpvar_28.w = 1.0;
  tmpvar_28.xyz = tmpvar_25;
  mediump vec3 tmpvar_29;
  mediump vec4 normal_30;
  normal_30 = tmpvar_28;
  highp float vC_31;
  mediump vec3 x3_32;
  mediump vec3 x2_33;
  mediump vec3 x1_34;
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHAr, normal_30);
  x1_34.x = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHAg, normal_30);
  x1_34.y = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = dot (unity_SHAb, normal_30);
  x1_34.z = tmpvar_37;
  mediump vec4 tmpvar_38;
  tmpvar_38 = (normal_30.xyzz * normal_30.yzzx);
  highp float tmpvar_39;
  tmpvar_39 = dot (unity_SHBr, tmpvar_38);
  x2_33.x = tmpvar_39;
  highp float tmpvar_40;
  tmpvar_40 = dot (unity_SHBg, tmpvar_38);
  x2_33.y = tmpvar_40;
  highp float tmpvar_41;
  tmpvar_41 = dot (unity_SHBb, tmpvar_38);
  x2_33.z = tmpvar_41;
  mediump float tmpvar_42;
  tmpvar_42 = ((normal_30.x * normal_30.x) - (normal_30.y * normal_30.y));
  vC_31 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43 = (unity_SHC.xyz * vC_31);
  x3_32 = tmpvar_43;
  tmpvar_29 = ((x1_34 + x2_33) + x3_32);
  shlight_3 = tmpvar_29;
  tmpvar_8 = shlight_3;
  highp vec3 tmpvar_44;
  tmpvar_44 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_45;
  tmpvar_45 = (unity_4LightPosX0 - tmpvar_44.x);
  highp vec4 tmpvar_46;
  tmpvar_46 = (unity_4LightPosY0 - tmpvar_44.y);
  highp vec4 tmpvar_47;
  tmpvar_47 = (unity_4LightPosZ0 - tmpvar_44.z);
  highp vec4 tmpvar_48;
  tmpvar_48 = (((tmpvar_45 * tmpvar_45) + (tmpvar_46 * tmpvar_46)) + (tmpvar_47 * tmpvar_47));
  highp vec4 tmpvar_49;
  tmpvar_49 = (max (vec4(0.0, 0.0, 0.0, 0.0), ((((tmpvar_45 * tmpvar_25.x) + (tmpvar_46 * tmpvar_25.y)) + (tmpvar_47 * tmpvar_25.z)) * inversesqrt(tmpvar_48))) * (1.0/((1.0 + (tmpvar_48 * unity_4LightAtten0)))));
  highp vec3 tmpvar_50;
  tmpvar_50 = (tmpvar_8 + ((((unity_LightColor[0].xyz * tmpvar_49.x) + (unity_LightColor[1].xyz * tmpvar_49.y)) + (unity_LightColor[2].xyz * tmpvar_49.z)) + (unity_LightColor[3].xyz * tmpvar_49.w)));
  tmpvar_8 = tmpvar_50;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_14 * (((_World2Object * tmpvar_27).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
}



#endif
#ifdef FRAGMENT

varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform lowp vec4 _LightColor0;
void main ()
{
  lowp vec4 c_1;
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  tmpvar_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD1.xyz;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD2.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD3.xyz;
  tmpvar_6 = tmpvar_10;
  tmpvar_2 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_11;
  lowp float tmpvar_12;
  mediump vec3 HDR_13;
  highp vec4 c_14;
  mediump vec3 tmpvar_15;
  tmpvar_15.x = tmpvar_4.z;
  tmpvar_15.y = tmpvar_5.z;
  tmpvar_15.z = tmpvar_6.z;
  highp vec3 tmpvar_16;
  tmpvar_16 = (tmpvar_3 - (2.0 * (dot (tmpvar_15, tmpvar_3) * tmpvar_15)));
  lowp vec4 tmpvar_17;
  tmpvar_17 = textureCube (_Cube, tmpvar_16);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * _ReflectColor);
  c_14 = tmpvar_18;
  highp vec3 tmpvar_19;
  tmpvar_19 = vec3((pow ((((c_14.x + c_14.y) + c_14.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_13 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = (1.0 - dot (normalize(tmpvar_2), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_21;
  tmpvar_21 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_20, _TransmissiveColor)));
  mediump float tmpvar_22;
  tmpvar_22 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_20)).x;
  tmpvar_12 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = ((c_14.xyz + HDR_13) + tmpvar_21.xyz);
  tmpvar_11 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = normalize(xlv_TEXCOORD0);
  mediump vec3 lightDir_25;
  lightDir_25 = xlv_TEXCOORD4;
  mediump vec3 viewDir_26;
  viewDir_26 = tmpvar_24;
  mediump vec4 c_27;
  c_27.xyz = ((((((lightDir_25.z * 0.5) + 0.5) * tmpvar_11) * _LightColor0.xyz) * 2.0) + (_LightColor0.xyz * max (0.0, (pow ((viewDir_26 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_12))));
  c_27.w = 1.0;
  c_1 = c_27;
  c_1.xyz = (c_1.xyz + (tmpvar_11 * xlv_TEXCOORD5));
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" "VERTEXLIGHT_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Color _glesColor
in vec4 _glesColor;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;
#define gl_MultiTexCoord1 _glesMultiTexCoord1
in vec4 _glesMultiTexCoord1;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
mat2 xll_transpose_mf2x2(mat2 m) {
  return mat2( m[0][0], m[1][0], m[0][1], m[1][1]);
}
mat3 xll_transpose_mf3x3(mat3 m) {
  return mat3( m[0][0], m[1][0], m[2][0],
               m[0][1], m[1][1], m[2][1],
               m[0][2], m[1][2], m[2][2]);
}
mat4 xll_transpose_mf4x4(mat4 m) {
  return mat4( m[0][0], m[1][0], m[2][0], m[3][0],
               m[0][1], m[1][1], m[2][1], m[3][1],
               m[0][2], m[1][2], m[2][2], m[3][2],
               m[0][3], m[1][3], m[2][3], m[3][3]);
}
vec2 xll_matrixindex_mf2x2_i (mat2 m, int i) { vec2 v; v.x=m[0][i]; v.y=m[1][i]; return v; }
vec3 xll_matrixindex_mf3x3_i (mat3 m, int i) { vec3 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; return v; }
vec4 xll_matrixindex_mf4x4_i (mat4 m, int i) { vec4 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; v.w=m[3][i]; return v; }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 399
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 432
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    lowp vec3 lightDir;
    lowp vec3 vlight;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 393
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 397
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 408
#line 443
#line 82
highp vec3 ObjSpaceLightDir( in highp vec4 v ) {
    highp vec3 objSpaceLightPos = (_World2Object * _WorldSpaceLightPos0).xyz;
    return objSpaceLightPos.xyz;
}
#line 91
highp vec3 ObjSpaceViewDir( in highp vec4 v ) {
    highp vec3 objSpaceCameraPos = ((_World2Object * vec4( _WorldSpaceCameraPos.xyz, 1.0)).xyz * unity_Scale.w);
    return (objSpaceCameraPos - v.xyz);
}
#line 96
highp vec3 Shade4PointLights( in highp vec4 lightPosX, in highp vec4 lightPosY, in highp vec4 lightPosZ, in highp vec3 lightColor0, in highp vec3 lightColor1, in highp vec3 lightColor2, in highp vec3 lightColor3, in highp vec4 lightAttenSq, in highp vec3 pos, in highp vec3 normal ) {
    highp vec4 toLightX = (lightPosX - pos.x);
    highp vec4 toLightY = (lightPosY - pos.y);
    #line 100
    highp vec4 toLightZ = (lightPosZ - pos.z);
    highp vec4 lengthSq = vec4( 0.0);
    lengthSq += (toLightX * toLightX);
    lengthSq += (toLightY * toLightY);
    #line 104
    lengthSq += (toLightZ * toLightZ);
    highp vec4 ndotl = vec4( 0.0);
    ndotl += (toLightX * normal.x);
    ndotl += (toLightY * normal.y);
    #line 108
    ndotl += (toLightZ * normal.z);
    highp vec4 corr = inversesqrt(lengthSq);
    ndotl = max( vec4( 0.0, 0.0, 0.0, 0.0), (ndotl * corr));
    highp vec4 atten = (1.0 / (1.0 + (lengthSq * lightAttenSq)));
    #line 112
    highp vec4 diff = (ndotl * atten);
    highp vec3 col = vec3( 0.0);
    col += (lightColor0 * diff.x);
    col += (lightColor1 * diff.y);
    #line 116
    col += (lightColor2 * diff.z);
    col += (lightColor3 * diff.w);
    return col;
}
#line 137
mediump vec3 ShadeSH9( in mediump vec4 normal ) {
    mediump vec3 x1;
    mediump vec3 x2;
    mediump vec3 x3;
    x1.x = dot( unity_SHAr, normal);
    #line 141
    x1.y = dot( unity_SHAg, normal);
    x1.z = dot( unity_SHAb, normal);
    mediump vec4 vB = (normal.xyzz * normal.yzzx);
    x2.x = dot( unity_SHBr, vB);
    #line 145
    x2.y = dot( unity_SHBg, vB);
    x2.z = dot( unity_SHBb, vB);
    highp float vC = ((normal.x * normal.x) - (normal.y * normal.y));
    x3 = (unity_SHC.xyz * vC);
    #line 149
    return ((x1 + x2) + x3);
}
#line 443
v2f_surf vert_surf( in appdata_full v ) {
    v2f_surf o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 447
    highp vec3 viewDir = (-ObjSpaceViewDir( v.vertex));
    highp vec3 worldRefl = (mat3( _Object2World) * viewDir);
    highp vec3 binormal = (cross( v.normal, v.tangent.xyz) * v.tangent.w);
    highp mat3 rotation = xll_transpose_mf3x3(mat3( v.tangent.xyz, binormal, v.normal));
    #line 451
    o.TtoW0 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 0).xyz), worldRefl.x) * unity_Scale.w);
    o.TtoW1 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 1).xyz), worldRefl.y) * unity_Scale.w);
    o.TtoW2 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 2).xyz), worldRefl.z) * unity_Scale.w);
    highp vec3 worldN = (mat3( _Object2World) * (v.normal * unity_Scale.w));
    #line 455
    highp vec3 lightDir = (rotation * ObjSpaceLightDir( v.vertex));
    o.lightDir = lightDir;
    highp vec3 viewDirForLight = (rotation * ObjSpaceViewDir( v.vertex));
    o.viewDir = viewDirForLight;
    #line 459
    highp vec3 shlight = ShadeSH9( vec4( worldN, 1.0));
    o.vlight = shlight;
    highp vec3 worldPos = (_Object2World * v.vertex).xyz;
    o.vlight += Shade4PointLights( unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0, unity_LightColor[0].xyz, unity_LightColor[1].xyz, unity_LightColor[2].xyz, unity_LightColor[3].xyz, unity_4LightAtten0, worldPos, worldN);
    #line 464
    return o;
}

out highp vec3 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_TEXCOORD2;
out lowp vec4 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
out lowp vec3 xlv_TEXCOORD5;
void main() {
    v2f_surf xl_retval;
    appdata_full xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.tangent = vec4(TANGENT);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xlt_v.texcoord1 = vec4(gl_MultiTexCoord1);
    xlt_v.color = vec4(gl_Color);
    xl_retval = vert_surf( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD0 = vec3(xl_retval.viewDir);
    xlv_TEXCOORD1 = vec4(xl_retval.TtoW0);
    xlv_TEXCOORD2 = vec4(xl_retval.TtoW1);
    xlv_TEXCOORD3 = vec4(xl_retval.TtoW2);
    xlv_TEXCOORD4 = vec3(xl_retval.lightDir);
    xlv_TEXCOORD5 = vec3(xl_retval.vlight);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 399
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 432
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    lowp vec3 lightDir;
    lowp vec3 vlight;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 393
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 397
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 408
#line 443
#line 422
mediump vec4 LightingHalfLambertSpecular( in SurfaceOutput s, in mediump vec3 lightDir, in mediump vec3 viewDir, in mediump float atten ) {
    #line 424
    mediump float NdotL = ((dot( s.Normal, lightDir) * 0.5) + 0.5);
    mediump vec3 halfVector = (viewDir + s.Normal);
    mediump float spec = max( 0.0, (pow( dot( halfVector, s.Normal), (10.0 - s.Specular)) * s.Gloss));
    mediump vec4 c;
    #line 428
    c.xyz = (((((NdotL * s.Albedo) * _LightColor0.xyz) * atten) * 2.0) + ((_LightColor0.xyz * spec) * atten));
    c.w = s.Alpha;
    return c;
}
#line 408
void surf( in Input IN, inout SurfaceOutput o ) {
    o.Normal = vec3( 0.0, 0.0, 1.0);
    highp vec3 worldRefl = reflect( IN.worldRefl, vec3( dot( IN.TtoW0, o.Normal), dot( IN.TtoW1, o.Normal), dot( IN.TtoW2, o.Normal)));
    #line 412
    highp vec4 c = (texture( _Cube, worldRefl) * _ReflectColor);
    mediump vec3 HDR = vec3( (pow( (((c.x + c.y) + c.z) / 3.0), _HdrPower) * _HdrContrast));
    mediump float fresnel = (1.0 - dot( normalize(IN.viewDir), normalize(vec3( 0.0, 0.0, 1.0))));
    mediump vec4 fresnelLerp = mix( _Color, _FresnelColor, vec4( pow( fresnel, _TransmissiveColor)));
    #line 416
    mediump vec4 GlossLerp = mix( vec4( 0.0, 0.0, 0.0, 0.0), vec4( 1.0, 1.0, 1.0, 1.0), vec4( fresnel));
    o.Specular = _Shininess;
    o.Gloss = GlossLerp.x;
    o.Albedo = ((c.xyz + HDR) + fresnelLerp.xyz);
    #line 420
    o.Alpha = 1.0;
}
#line 466
lowp vec4 frag_surf( in v2f_surf IN ) {
    #line 468
    Input surfIN;
    surfIN.worldRefl = vec3( IN.TtoW0.w, IN.TtoW1.w, IN.TtoW2.w);
    surfIN.TtoW0 = IN.TtoW0.xyz;
    surfIN.TtoW1 = IN.TtoW1.xyz;
    #line 472
    surfIN.TtoW2 = IN.TtoW2.xyz;
    surfIN.viewDir = IN.viewDir;
    SurfaceOutput o;
    o.Albedo = vec3( 0.0);
    #line 476
    o.Emission = vec3( 0.0);
    o.Specular = 0.0;
    o.Alpha = 0.0;
    o.Gloss = 0.0;
    #line 480
    surf( surfIN, o);
    lowp float atten = 1.0;
    lowp vec4 c = vec4( 0.0);
    c = LightingHalfLambertSpecular( o, IN.lightDir, normalize(IN.viewDir), atten);
    #line 484
    c.xyz += (o.Albedo * IN.vlight);
    return c;
}
in highp vec3 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_TEXCOORD2;
in lowp vec4 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in lowp vec3 xlv_TEXCOORD5;
void main() {
    lowp vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.viewDir = vec3(xlv_TEXCOORD0);
    xlt_IN.TtoW0 = vec4(xlv_TEXCOORD1);
    xlt_IN.TtoW1 = vec4(xlv_TEXCOORD2);
    xlt_IN.TtoW2 = vec4(xlv_TEXCOORD3);
    xlt_IN.lightDir = vec3(xlv_TEXCOORD4);
    xlt_IN.vlight = vec3(xlv_TEXCOORD5);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Vector 13 [_WorldSpaceCameraPos]
Vector 14 [_ProjectionParams]
Vector 15 [_WorldSpaceLightPos0]
Vector 16 [unity_4LightPosX0]
Vector 17 [unity_4LightPosY0]
Vector 18 [unity_4LightPosZ0]
Vector 19 [unity_4LightAtten0]
Vector 20 [unity_LightColor0]
Vector 21 [unity_LightColor1]
Vector 22 [unity_LightColor2]
Vector 23 [unity_LightColor3]
Vector 24 [unity_SHAr]
Vector 25 [unity_SHAg]
Vector 26 [unity_SHAb]
Vector 27 [unity_SHBr]
Vector 28 [unity_SHBg]
Vector 29 [unity_SHBb]
Vector 30 [unity_SHC]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 31 [unity_Scale]
"3.0-!!ARBvp1.0
# 94 ALU
PARAM c[32] = { { 1, 0, 0.5 },
		state.matrix.mvp,
		program.local[5..31] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
TEMP R4;
MUL R3.xyz, vertex.normal, c[31].w;
DP4 R0.x, vertex.position, c[6];
ADD R1, -R0.x, c[17];
DP3 R3.w, R3, c[6];
DP3 R4.x, R3, c[5];
DP3 R3.x, R3, c[7];
MUL R2, R3.w, R1;
DP4 R0.x, vertex.position, c[5];
ADD R0, -R0.x, c[16];
MUL R1, R1, R1;
MOV R4.z, R3.x;
MAD R2, R4.x, R0, R2;
MOV R4.w, c[0].x;
DP4 R4.y, vertex.position, c[7];
MAD R1, R0, R0, R1;
ADD R0, -R4.y, c[18];
MAD R1, R0, R0, R1;
MAD R0, R3.x, R0, R2;
MUL R2, R1, c[19];
MOV R4.y, R3.w;
RSQ R1.x, R1.x;
RSQ R1.y, R1.y;
RSQ R1.w, R1.w;
RSQ R1.z, R1.z;
MUL R0, R0, R1;
ADD R1, R2, c[0].x;
RCP R1.x, R1.x;
RCP R1.y, R1.y;
RCP R1.w, R1.w;
RCP R1.z, R1.z;
MAX R0, R0, c[0].y;
MUL R0, R0, R1;
MUL R1.xyz, R0.y, c[21];
MAD R1.xyz, R0.x, c[20], R1;
MAD R0.xyz, R0.z, c[22], R1;
MAD R1.xyz, R0.w, c[23], R0;
MUL R0, R4.xyzz, R4.yzzx;
DP4 R3.z, R0, c[29];
DP4 R3.y, R0, c[28];
DP4 R3.x, R0, c[27];
MUL R1.w, R3, R3;
MAD R0.x, R4, R4, -R1.w;
MOV R1.w, c[0].x;
DP4 R2.z, R4, c[26];
DP4 R2.y, R4, c[25];
DP4 R2.x, R4, c[24];
ADD R2.xyz, R2, R3;
MUL R3.xyz, R0.x, c[30];
ADD R3.xyz, R2, R3;
MOV R0.xyz, vertex.attrib[14];
MUL R2.xyz, vertex.normal.zxyw, R0.yzxw;
ADD result.texcoord[5].xyz, R3, R1;
MAD R0.xyz, vertex.normal.yzxw, R0.zxyw, -R2;
MOV R1.xyz, c[13];
MUL R3.xyz, R0, vertex.attrib[14].w;
MOV R0, c[15];
DP4 R2.z, R1, c[11];
DP4 R2.x, R1, c[9];
DP4 R2.y, R1, c[10];
MAD R2.xyz, R2, c[31].w, -vertex.position;
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
DP4 R1.z, R0, c[11];
DP3 R0.w, -R2, c[5];
DP3 result.texcoord[4].y, R3, R1;
DP3 R0.y, R3, c[5];
DP3 R0.x, vertex.attrib[14], c[5];
DP3 R0.z, vertex.normal, c[5];
MUL result.texcoord[1], R0, c[31].w;
DP3 R0.w, -R2, c[6];
DP3 R0.y, R3, c[6];
DP3 R0.x, vertex.attrib[14], c[6];
DP3 R0.z, vertex.normal, c[6];
MUL result.texcoord[2], R0, c[31].w;
DP3 R0.w, -R2, c[7];
DP3 R0.y, R3, c[7];
DP3 R0.x, vertex.attrib[14], c[7];
DP3 R0.z, vertex.normal, c[7];
MUL result.texcoord[3], R0, c[31].w;
DP4 R0.w, vertex.position, c[4];
DP4 R0.z, vertex.position, c[3];
DP3 result.texcoord[4].z, vertex.normal, R1;
DP4 R0.x, vertex.position, c[1];
DP4 R0.y, vertex.position, c[2];
DP3 result.texcoord[4].x, vertex.attrib[14], R1;
DP3 result.texcoord[0].y, R2, R3;
DP3 result.texcoord[0].z, vertex.normal, R2;
DP3 result.texcoord[0].x, R2, vertex.attrib[14];
MUL R2.xyz, R0.xyww, c[0].z;
MOV R1.x, R2;
MUL R1.y, R2, c[14].x;
ADD result.texcoord[6].xy, R1, R2.z;
MOV result.position, R0;
MOV result.texcoord[6].zw, R0;
END
# 94 instructions, 5 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Matrix 0 [glstate_matrix_mvp]
Vector 12 [_WorldSpaceCameraPos]
Vector 13 [_ProjectionParams]
Vector 14 [_ScreenParams]
Vector 15 [_WorldSpaceLightPos0]
Vector 16 [unity_4LightPosX0]
Vector 17 [unity_4LightPosY0]
Vector 18 [unity_4LightPosZ0]
Vector 19 [unity_4LightAtten0]
Vector 20 [unity_LightColor0]
Vector 21 [unity_LightColor1]
Vector 22 [unity_LightColor2]
Vector 23 [unity_LightColor3]
Vector 24 [unity_SHAr]
Vector 25 [unity_SHAg]
Vector 26 [unity_SHAb]
Vector 27 [unity_SHBr]
Vector 28 [unity_SHBg]
Vector 29 [unity_SHBb]
Vector 30 [unity_SHC]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 31 [unity_Scale]
"vs_3_0
; 96 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
dcl_texcoord6 o7
def c32, 1.00000000, 0.00000000, 0.50000000, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
mul r3.xyz, v2, c31.w
dp4 r0.x, v0, c5
add r1, -r0.x, c17
dp3 r3.w, r3, c5
dp3 r4.x, r3, c4
dp3 r3.x, r3, c6
mul r2, r3.w, r1
dp4 r0.x, v0, c4
add r0, -r0.x, c16
mul r1, r1, r1
mov r4.z, r3.x
mad r2, r4.x, r0, r2
mov r4.w, c32.x
dp4 r4.y, v0, c6
mad r1, r0, r0, r1
add r0, -r4.y, c18
mad r1, r0, r0, r1
mad r0, r3.x, r0, r2
mul r2, r1, c19
mov r4.y, r3.w
rsq r1.x, r1.x
rsq r1.y, r1.y
rsq r1.w, r1.w
rsq r1.z, r1.z
mul r0, r0, r1
add r1, r2, c32.x
dp4 r2.z, r4, c26
dp4 r2.y, r4, c25
dp4 r2.x, r4, c24
rcp r1.x, r1.x
rcp r1.y, r1.y
rcp r1.w, r1.w
rcp r1.z, r1.z
max r0, r0, c32.y
mul r0, r0, r1
mul r1.xyz, r0.y, c21
mad r1.xyz, r0.x, c20, r1
mad r0.xyz, r0.z, c22, r1
mad r1.xyz, r0.w, c23, r0
mul r0, r4.xyzz, r4.yzzx
mul r1.w, r3, r3
dp4 r3.z, r0, c29
dp4 r3.y, r0, c28
dp4 r3.x, r0, c27
mad r1.w, r4.x, r4.x, -r1
mul r0.xyz, r1.w, c30
add r2.xyz, r2, r3
add r2.xyz, r2, r0
add o6.xyz, r2, r1
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r0.xyz, v2.yzxw, r0.zxyw, -r1
mul r3.xyz, r0, v1.w
mov r0, c10
dp4 r4.z, c15, r0
mov r0, c9
dp4 r4.y, c15, r0
mov r1.w, c32.x
mov r1.xyz, c12
dp4 r2.z, r1, c10
dp4 r2.x, r1, c8
dp4 r2.y, r1, c9
mad r2.xyz, r2, c31.w, -v0
mov r1, c8
dp4 r4.x, c15, r1
dp3 r0.y, r3, c4
dp3 r0.w, -r2, c4
dp3 r0.x, v1, c4
dp3 r0.z, v2, c4
mul o2, r0, c31.w
dp3 r0.y, r3, c5
dp3 r0.w, -r2, c5
dp3 r0.x, v1, c5
dp3 r0.z, v2, c5
mul o3, r0, c31.w
dp3 r0.y, r3, c6
dp3 r0.w, -r2, c6
dp3 r0.x, v1, c6
dp3 r0.z, v2, c6
mul o4, r0, c31.w
dp4 r0.w, v0, c3
dp4 r0.z, v0, c2
dp4 r0.x, v0, c0
dp4 r0.y, v0, c1
mul r1.xyz, r0.xyww, c32.z
mul r1.y, r1, c13.x
dp3 o1.y, r2, r3
dp3 o5.y, r3, r4
dp3 o1.z, v2, r2
dp3 o1.x, r2, v1
dp3 o5.z, v2, r4
dp3 o5.x, v1, r4
mad o7.xy, r1.z, c14.zwzw, r1
mov o0, r0
mov o7.zw, r0
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "color" Color
ConstBuffer "UnityPerCamera" 128 // 96 used size, 8 vars
Vector 64 [_WorldSpaceCameraPos] 3
Vector 80 [_ProjectionParams] 4
ConstBuffer "UnityLighting" 720 // 720 used size, 17 vars
Vector 0 [_WorldSpaceLightPos0] 4
Vector 32 [unity_4LightPosX0] 4
Vector 48 [unity_4LightPosY0] 4
Vector 64 [unity_4LightPosZ0] 4
Vector 80 [unity_4LightAtten0] 4
Vector 96 [unity_LightColor0] 4
Vector 112 [unity_LightColor1] 4
Vector 128 [unity_LightColor2] 4
Vector 144 [unity_LightColor3] 4
Vector 160 [unity_LightColor4] 4
Vector 176 [unity_LightColor5] 4
Vector 192 [unity_LightColor6] 4
Vector 208 [unity_LightColor7] 4
Vector 608 [unity_SHAr] 4
Vector 624 [unity_SHAg] 4
Vector 640 [unity_SHAb] 4
Vector 656 [unity_SHBr] 4
Vector 672 [unity_SHBg] 4
Vector 688 [unity_SHBb] 4
Vector 704 [unity_SHC] 4
ConstBuffer "UnityPerDraw" 336 // 336 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
Matrix 192 [_Object2World] 4
Matrix 256 [_World2Object] 4
Vector 320 [unity_Scale] 4
BindCB "UnityPerCamera" 0
BindCB "UnityLighting" 1
BindCB "UnityPerDraw" 2
// 94 instructions, 7 temp regs, 0 temp arrays:
// ALU 79 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedkkmcnmdhjfjamdmhnpldljaajjannebfabaaaaaahmaoaaaaadaaaaaa
cmaaaaaapeaaaaaanmabaaaaejfdeheomaaaaaaaagaaaaaaaiaaaaaajiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaakbaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaakjaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
ahahaaaalaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaadaaaaaaapaaaaaalaaaaaaa
abaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaaljaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaafaaaaaaapaaaaaafaepfdejfeejepeoaafeebeoehefeofeaaeoepfc
enebemaafeeffiedepepfceeaaedepemepfcaaklepfdeheooaaaaaaaaiaaaaaa
aiaaaaaamiaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaneaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaahaiaaaaneaaaaaaabaaaaaaaaaaaaaa
adaaaaaaacaaaaaaapaaaaaaneaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaa
apaaaaaaneaaaaaaadaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaaneaaaaaa
aeaaaaaaaaaaaaaaadaaaaaaafaaaaaaahaiaaaaneaaaaaaafaaaaaaaaaaaaaa
adaaaaaaagaaaaaaahaiaaaaneaaaaaaagaaaaaaaaaaaaaaadaaaaaaahaaaaaa
apaaaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklfdeieefc
jiamaaaaeaaaabaacgadaaaafjaaaaaeegiocaaaaaaaaaaaagaaaaaafjaaaaae
egiocaaaabaaaaaacnaaaaaafjaaaaaeegiocaaaacaaaaaabfaaaaaafpaaaaad
pcbabaaaaaaaaaaafpaaaaadpcbabaaaabaaaaaafpaaaaadhcbabaaaacaaaaaa
ghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaadhccabaaaabaaaaaagfaaaaad
pccabaaaacaaaaaagfaaaaadpccabaaaadaaaaaagfaaaaadpccabaaaaeaaaaaa
gfaaaaadhccabaaaafaaaaaagfaaaaadhccabaaaagaaaaaagfaaaaadpccabaaa
ahaaaaaagiaaaaacahaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaa
egiocaaaacaaaaaaabaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaacaaaaaa
aaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaa
egiocaaaacaaaaaaacaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaak
pcaabaaaaaaaaaaaegiocaaaacaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaa
aaaaaaaadgaaaaafpccabaaaaaaaaaaaegaobaaaaaaaaaaadiaaaaajhcaabaaa
abaaaaaafgifcaaaaaaaaaaaaeaaaaaaegiccaaaacaaaaaabbaaaaaadcaaaaal
hcaabaaaabaaaaaaegiccaaaacaaaaaabaaaaaaaagiacaaaaaaaaaaaaeaaaaaa
egacbaaaabaaaaaadcaaaaalhcaabaaaabaaaaaaegiccaaaacaaaaaabcaaaaaa
kgikcaaaaaaaaaaaaeaaaaaaegacbaaaabaaaaaaaaaaaaaihcaabaaaabaaaaaa
egacbaaaabaaaaaaegiccaaaacaaaaaabdaaaaaadcaaaaalhcaabaaaabaaaaaa
egacbaaaabaaaaaapgipcaaaacaaaaaabeaaaaaaegbcbaiaebaaaaaaaaaaaaaa
baaaaaahbccabaaaabaaaaaaegbcbaaaabaaaaaaegacbaaaabaaaaaabaaaaaah
eccabaaaabaaaaaaegbcbaaaacaaaaaaegacbaaaabaaaaaadiaaaaahhcaabaaa
acaaaaaajgbebaaaabaaaaaacgbjbaaaacaaaaaadcaaaaakhcaabaaaacaaaaaa
jgbebaaaacaaaaaacgbjbaaaabaaaaaaegacbaiaebaaaaaaacaaaaaadiaaaaah
hcaabaaaacaaaaaaegacbaaaacaaaaaapgbpbaaaabaaaaaabaaaaaahcccabaaa
abaaaaaaegacbaaaacaaaaaaegacbaaaabaaaaaadiaaaaajhcaabaaaadaaaaaa
fgafbaiaebaaaaaaabaaaaaaegiccaaaacaaaaaaanaaaaaadcaaaaallcaabaaa
abaaaaaaegiicaaaacaaaaaaamaaaaaaagaabaiaebaaaaaaabaaaaaaegaibaaa
adaaaaaadcaaaaallcaabaaaabaaaaaaegiicaaaacaaaaaaaoaaaaaakgakbaia
ebaaaaaaabaaaaaaegambaaaabaaaaaadgaaaaaficaabaaaadaaaaaaakaabaaa
abaaaaaadgaaaaagbcaabaaaaeaaaaaaakiacaaaacaaaaaaamaaaaaadgaaaaag
ccaabaaaaeaaaaaaakiacaaaacaaaaaaanaaaaaadgaaaaagecaabaaaaeaaaaaa
akiacaaaacaaaaaaaoaaaaaabaaaaaahccaabaaaadaaaaaaegacbaaaacaaaaaa
egacbaaaaeaaaaaabaaaaaahbcaabaaaadaaaaaaegbcbaaaabaaaaaaegacbaaa
aeaaaaaabaaaaaahecaabaaaadaaaaaaegbcbaaaacaaaaaaegacbaaaaeaaaaaa
diaaaaaipccabaaaacaaaaaaegaobaaaadaaaaaapgipcaaaacaaaaaabeaaaaaa
dgaaaaaficaabaaaadaaaaaabkaabaaaabaaaaaadgaaaaagbcaabaaaaeaaaaaa
bkiacaaaacaaaaaaamaaaaaadgaaaaagccaabaaaaeaaaaaabkiacaaaacaaaaaa
anaaaaaadgaaaaagecaabaaaaeaaaaaabkiacaaaacaaaaaaaoaaaaaabaaaaaah
ccaabaaaadaaaaaaegacbaaaacaaaaaaegacbaaaaeaaaaaabaaaaaahbcaabaaa
adaaaaaaegbcbaaaabaaaaaaegacbaaaaeaaaaaabaaaaaahecaabaaaadaaaaaa
egbcbaaaacaaaaaaegacbaaaaeaaaaaadiaaaaaipccabaaaadaaaaaaegaobaaa
adaaaaaapgipcaaaacaaaaaabeaaaaaadgaaaaagbcaabaaaadaaaaaackiacaaa
acaaaaaaamaaaaaadgaaaaagccaabaaaadaaaaaackiacaaaacaaaaaaanaaaaaa
dgaaaaagecaabaaaadaaaaaackiacaaaacaaaaaaaoaaaaaabaaaaaahccaabaaa
abaaaaaaegacbaaaacaaaaaaegacbaaaadaaaaaabaaaaaahbcaabaaaabaaaaaa
egbcbaaaabaaaaaaegacbaaaadaaaaaabaaaaaahecaabaaaabaaaaaaegbcbaaa
acaaaaaaegacbaaaadaaaaaadiaaaaaipccabaaaaeaaaaaaegaobaaaabaaaaaa
pgipcaaaacaaaaaabeaaaaaadiaaaaajhcaabaaaabaaaaaafgifcaaaabaaaaaa
aaaaaaaaegiccaaaacaaaaaabbaaaaaadcaaaaalhcaabaaaabaaaaaaegiccaaa
acaaaaaabaaaaaaaagiacaaaabaaaaaaaaaaaaaaegacbaaaabaaaaaadcaaaaal
hcaabaaaabaaaaaaegiccaaaacaaaaaabcaaaaaakgikcaaaabaaaaaaaaaaaaaa
egacbaaaabaaaaaadcaaaaalhcaabaaaabaaaaaaegiccaaaacaaaaaabdaaaaaa
pgipcaaaabaaaaaaaaaaaaaaegacbaaaabaaaaaabaaaaaahcccabaaaafaaaaaa
egacbaaaacaaaaaaegacbaaaabaaaaaabaaaaaahbccabaaaafaaaaaaegbcbaaa
abaaaaaaegacbaaaabaaaaaabaaaaaaheccabaaaafaaaaaaegbcbaaaacaaaaaa
egacbaaaabaaaaaadgaaaaaficaabaaaabaaaaaaabeaaaaaaaaaiadpdiaaaaai
hcaabaaaacaaaaaaegbcbaaaacaaaaaapgipcaaaacaaaaaabeaaaaaadiaaaaai
hcaabaaaadaaaaaafgafbaaaacaaaaaaegiccaaaacaaaaaaanaaaaaadcaaaaak
lcaabaaaacaaaaaaegiicaaaacaaaaaaamaaaaaaagaabaaaacaaaaaaegaibaaa
adaaaaaadcaaaaakhcaabaaaabaaaaaaegiccaaaacaaaaaaaoaaaaaakgakbaaa
acaaaaaaegadbaaaacaaaaaabbaaaaaibcaabaaaacaaaaaaegiocaaaabaaaaaa
cgaaaaaaegaobaaaabaaaaaabbaaaaaiccaabaaaacaaaaaaegiocaaaabaaaaaa
chaaaaaaegaobaaaabaaaaaabbaaaaaiecaabaaaacaaaaaaegiocaaaabaaaaaa
ciaaaaaaegaobaaaabaaaaaadiaaaaahpcaabaaaadaaaaaajgacbaaaabaaaaaa
egakbaaaabaaaaaabbaaaaaibcaabaaaaeaaaaaaegiocaaaabaaaaaacjaaaaaa
egaobaaaadaaaaaabbaaaaaiccaabaaaaeaaaaaaegiocaaaabaaaaaackaaaaaa
egaobaaaadaaaaaabbaaaaaiecaabaaaaeaaaaaaegiocaaaabaaaaaaclaaaaaa
egaobaaaadaaaaaaaaaaaaahhcaabaaaacaaaaaaegacbaaaacaaaaaaegacbaaa
aeaaaaaadiaaaaahicaabaaaabaaaaaabkaabaaaabaaaaaabkaabaaaabaaaaaa
dcaaaaakicaabaaaabaaaaaaakaabaaaabaaaaaaakaabaaaabaaaaaadkaabaia
ebaaaaaaabaaaaaadcaaaaakhcaabaaaacaaaaaaegiccaaaabaaaaaacmaaaaaa
pgapbaaaabaaaaaaegacbaaaacaaaaaadiaaaaaihcaabaaaadaaaaaafgbfbaaa
aaaaaaaaegiccaaaacaaaaaaanaaaaaadcaaaaakhcaabaaaadaaaaaaegiccaaa
acaaaaaaamaaaaaaagbabaaaaaaaaaaaegacbaaaadaaaaaadcaaaaakhcaabaaa
adaaaaaaegiccaaaacaaaaaaaoaaaaaakgbkbaaaaaaaaaaaegacbaaaadaaaaaa
dcaaaaakhcaabaaaadaaaaaaegiccaaaacaaaaaaapaaaaaapgbpbaaaaaaaaaaa
egacbaaaadaaaaaaaaaaaaajpcaabaaaaeaaaaaafgafbaiaebaaaaaaadaaaaaa
egiocaaaabaaaaaaadaaaaaadiaaaaahpcaabaaaafaaaaaafgafbaaaabaaaaaa
egaobaaaaeaaaaaadiaaaaahpcaabaaaaeaaaaaaegaobaaaaeaaaaaaegaobaaa
aeaaaaaaaaaaaaajpcaabaaaagaaaaaaagaabaiaebaaaaaaadaaaaaaegiocaaa
abaaaaaaacaaaaaaaaaaaaajpcaabaaaadaaaaaakgakbaiaebaaaaaaadaaaaaa
egiocaaaabaaaaaaaeaaaaaadcaaaaajpcaabaaaafaaaaaaegaobaaaagaaaaaa
agaabaaaabaaaaaaegaobaaaafaaaaaadcaaaaajpcaabaaaabaaaaaaegaobaaa
adaaaaaakgakbaaaabaaaaaaegaobaaaafaaaaaadcaaaaajpcaabaaaaeaaaaaa
egaobaaaagaaaaaaegaobaaaagaaaaaaegaobaaaaeaaaaaadcaaaaajpcaabaaa
adaaaaaaegaobaaaadaaaaaaegaobaaaadaaaaaaegaobaaaaeaaaaaaeeaaaaaf
pcaabaaaaeaaaaaaegaobaaaadaaaaaadcaaaaanpcaabaaaadaaaaaaegaobaaa
adaaaaaaegiocaaaabaaaaaaafaaaaaaaceaaaaaaaaaiadpaaaaiadpaaaaiadp
aaaaiadpaoaaaaakpcaabaaaadaaaaaaaceaaaaaaaaaiadpaaaaiadpaaaaiadp
aaaaiadpegaobaaaadaaaaaadiaaaaahpcaabaaaabaaaaaaegaobaaaabaaaaaa
egaobaaaaeaaaaaadeaaaaakpcaabaaaabaaaaaaegaobaaaabaaaaaaaceaaaaa
aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaadiaaaaahpcaabaaaabaaaaaaegaobaaa
adaaaaaaegaobaaaabaaaaaadiaaaaaihcaabaaaadaaaaaafgafbaaaabaaaaaa
egiccaaaabaaaaaaahaaaaaadcaaaaakhcaabaaaadaaaaaaegiccaaaabaaaaaa
agaaaaaaagaabaaaabaaaaaaegacbaaaadaaaaaadcaaaaakhcaabaaaabaaaaaa
egiccaaaabaaaaaaaiaaaaaakgakbaaaabaaaaaaegacbaaaadaaaaaadcaaaaak
hcaabaaaabaaaaaaegiccaaaabaaaaaaajaaaaaapgapbaaaabaaaaaaegacbaaa
abaaaaaaaaaaaaahhccabaaaagaaaaaaegacbaaaabaaaaaaegacbaaaacaaaaaa
diaaaaaiccaabaaaaaaaaaaabkaabaaaaaaaaaaaakiacaaaaaaaaaaaafaaaaaa
diaaaaakncaabaaaabaaaaaaagahbaaaaaaaaaaaaceaaaaaaaaaaadpaaaaaaaa
aaaaaadpaaaaaadpdgaaaaafmccabaaaahaaaaaakgaobaaaaaaaaaaaaaaaaaah
dccabaaaahaaaaaakgakbaaaabaaaaaamgaabaaaabaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD6;
varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 unity_SHC;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosX0;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  lowp vec4 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (_glesVertex.xyz - ((_World2Object * tmpvar_9).xyz * unity_Scale.w)));
  highp vec3 tmpvar_12;
  highp vec3 tmpvar_13;
  tmpvar_12 = tmpvar_1.xyz;
  tmpvar_13 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_13.x;
  tmpvar_14[0].z = tmpvar_2.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_13.y;
  tmpvar_14[1].z = tmpvar_2.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_13.z;
  tmpvar_14[2].z = tmpvar_2.z;
  vec4 v_15;
  v_15.x = _Object2World[0].x;
  v_15.y = _Object2World[1].x;
  v_15.z = _Object2World[2].x;
  v_15.w = _Object2World[3].x;
  highp vec4 tmpvar_16;
  tmpvar_16.xyz = (tmpvar_14 * v_15.xyz);
  tmpvar_16.w = tmpvar_11.x;
  highp vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * unity_Scale.w);
  tmpvar_4 = tmpvar_17;
  vec4 v_18;
  v_18.x = _Object2World[0].y;
  v_18.y = _Object2World[1].y;
  v_18.z = _Object2World[2].y;
  v_18.w = _Object2World[3].y;
  highp vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_14 * v_18.xyz);
  tmpvar_19.w = tmpvar_11.y;
  highp vec4 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * unity_Scale.w);
  tmpvar_5 = tmpvar_20;
  vec4 v_21;
  v_21.x = _Object2World[0].z;
  v_21.y = _Object2World[1].z;
  v_21.z = _Object2World[2].z;
  v_21.w = _Object2World[3].z;
  highp vec4 tmpvar_22;
  tmpvar_22.xyz = (tmpvar_14 * v_21.xyz);
  tmpvar_22.w = tmpvar_11.z;
  highp vec4 tmpvar_23;
  tmpvar_23 = (tmpvar_22 * unity_Scale.w);
  tmpvar_6 = tmpvar_23;
  mat3 tmpvar_24;
  tmpvar_24[0] = _Object2World[0].xyz;
  tmpvar_24[1] = _Object2World[1].xyz;
  tmpvar_24[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_24 * (tmpvar_2 * unity_Scale.w));
  highp vec3 tmpvar_26;
  tmpvar_26 = (tmpvar_14 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_7 = tmpvar_26;
  highp vec4 tmpvar_27;
  tmpvar_27.w = 1.0;
  tmpvar_27.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_28;
  tmpvar_28.w = 1.0;
  tmpvar_28.xyz = tmpvar_25;
  mediump vec3 tmpvar_29;
  mediump vec4 normal_30;
  normal_30 = tmpvar_28;
  highp float vC_31;
  mediump vec3 x3_32;
  mediump vec3 x2_33;
  mediump vec3 x1_34;
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHAr, normal_30);
  x1_34.x = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHAg, normal_30);
  x1_34.y = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = dot (unity_SHAb, normal_30);
  x1_34.z = tmpvar_37;
  mediump vec4 tmpvar_38;
  tmpvar_38 = (normal_30.xyzz * normal_30.yzzx);
  highp float tmpvar_39;
  tmpvar_39 = dot (unity_SHBr, tmpvar_38);
  x2_33.x = tmpvar_39;
  highp float tmpvar_40;
  tmpvar_40 = dot (unity_SHBg, tmpvar_38);
  x2_33.y = tmpvar_40;
  highp float tmpvar_41;
  tmpvar_41 = dot (unity_SHBb, tmpvar_38);
  x2_33.z = tmpvar_41;
  mediump float tmpvar_42;
  tmpvar_42 = ((normal_30.x * normal_30.x) - (normal_30.y * normal_30.y));
  vC_31 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43 = (unity_SHC.xyz * vC_31);
  x3_32 = tmpvar_43;
  tmpvar_29 = ((x1_34 + x2_33) + x3_32);
  shlight_3 = tmpvar_29;
  tmpvar_8 = shlight_3;
  highp vec3 tmpvar_44;
  tmpvar_44 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_45;
  tmpvar_45 = (unity_4LightPosX0 - tmpvar_44.x);
  highp vec4 tmpvar_46;
  tmpvar_46 = (unity_4LightPosY0 - tmpvar_44.y);
  highp vec4 tmpvar_47;
  tmpvar_47 = (unity_4LightPosZ0 - tmpvar_44.z);
  highp vec4 tmpvar_48;
  tmpvar_48 = (((tmpvar_45 * tmpvar_45) + (tmpvar_46 * tmpvar_46)) + (tmpvar_47 * tmpvar_47));
  highp vec4 tmpvar_49;
  tmpvar_49 = (max (vec4(0.0, 0.0, 0.0, 0.0), ((((tmpvar_45 * tmpvar_25.x) + (tmpvar_46 * tmpvar_25.y)) + (tmpvar_47 * tmpvar_25.z)) * inversesqrt(tmpvar_48))) * (1.0/((1.0 + (tmpvar_48 * unity_4LightAtten0)))));
  highp vec3 tmpvar_50;
  tmpvar_50 = (tmpvar_8 + ((((unity_LightColor[0].xyz * tmpvar_49.x) + (unity_LightColor[1].xyz * tmpvar_49.y)) + (unity_LightColor[2].xyz * tmpvar_49.z)) + (unity_LightColor[3].xyz * tmpvar_49.w)));
  tmpvar_8 = tmpvar_50;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_14 * (((_World2Object * tmpvar_27).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
  xlv_TEXCOORD6 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD6;
varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform sampler2D _ShadowMapTexture;
uniform lowp vec4 _LightColor0;
uniform highp vec4 _LightShadowData;
void main ()
{
  lowp vec4 c_1;
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  tmpvar_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD1.xyz;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD2.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD3.xyz;
  tmpvar_6 = tmpvar_10;
  tmpvar_2 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_11;
  lowp float tmpvar_12;
  mediump vec3 HDR_13;
  highp vec4 c_14;
  mediump vec3 tmpvar_15;
  tmpvar_15.x = tmpvar_4.z;
  tmpvar_15.y = tmpvar_5.z;
  tmpvar_15.z = tmpvar_6.z;
  highp vec3 tmpvar_16;
  tmpvar_16 = (tmpvar_3 - (2.0 * (dot (tmpvar_15, tmpvar_3) * tmpvar_15)));
  lowp vec4 tmpvar_17;
  tmpvar_17 = textureCube (_Cube, tmpvar_16);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * _ReflectColor);
  c_14 = tmpvar_18;
  highp vec3 tmpvar_19;
  tmpvar_19 = vec3((pow ((((c_14.x + c_14.y) + c_14.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_13 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = (1.0 - dot (normalize(tmpvar_2), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_21;
  tmpvar_21 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_20, _TransmissiveColor)));
  mediump float tmpvar_22;
  tmpvar_22 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_20)).x;
  tmpvar_12 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = ((c_14.xyz + HDR_13) + tmpvar_21.xyz);
  tmpvar_11 = tmpvar_23;
  lowp float tmpvar_24;
  mediump float lightShadowDataX_25;
  highp float dist_26;
  lowp float tmpvar_27;
  tmpvar_27 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD6).x;
  dist_26 = tmpvar_27;
  highp float tmpvar_28;
  tmpvar_28 = _LightShadowData.x;
  lightShadowDataX_25 = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = max (float((dist_26 > (xlv_TEXCOORD6.z / xlv_TEXCOORD6.w))), lightShadowDataX_25);
  tmpvar_24 = tmpvar_29;
  highp vec3 tmpvar_30;
  tmpvar_30 = normalize(xlv_TEXCOORD0);
  mediump vec3 lightDir_31;
  lightDir_31 = xlv_TEXCOORD4;
  mediump vec3 viewDir_32;
  viewDir_32 = tmpvar_30;
  mediump float atten_33;
  atten_33 = tmpvar_24;
  mediump vec4 c_34;
  c_34.xyz = (((((((lightDir_31.z * 0.5) + 0.5) * tmpvar_11) * _LightColor0.xyz) * atten_33) * 2.0) + ((_LightColor0.xyz * max (0.0, (pow ((viewDir_32 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_12))) * atten_33));
  c_34.w = 1.0;
  c_1 = c_34;
  c_1.xyz = (c_1.xyz + (tmpvar_11 * xlv_TEXCOORD5));
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD6;
varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_SHC;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosX0;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 _ProjectionParams;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  lowp vec4 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 tmpvar_10;
  tmpvar_10.w = 1.0;
  tmpvar_10.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_11;
  tmpvar_11[0] = _Object2World[0].xyz;
  tmpvar_11[1] = _Object2World[1].xyz;
  tmpvar_11[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_12;
  tmpvar_12 = (tmpvar_11 * (_glesVertex.xyz - ((_World2Object * tmpvar_10).xyz * unity_Scale.w)));
  highp vec3 tmpvar_13;
  highp vec3 tmpvar_14;
  tmpvar_13 = tmpvar_1.xyz;
  tmpvar_14 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_15;
  tmpvar_15[0].x = tmpvar_13.x;
  tmpvar_15[0].y = tmpvar_14.x;
  tmpvar_15[0].z = tmpvar_2.x;
  tmpvar_15[1].x = tmpvar_13.y;
  tmpvar_15[1].y = tmpvar_14.y;
  tmpvar_15[1].z = tmpvar_2.y;
  tmpvar_15[2].x = tmpvar_13.z;
  tmpvar_15[2].y = tmpvar_14.z;
  tmpvar_15[2].z = tmpvar_2.z;
  vec4 v_16;
  v_16.x = _Object2World[0].x;
  v_16.y = _Object2World[1].x;
  v_16.z = _Object2World[2].x;
  v_16.w = _Object2World[3].x;
  highp vec4 tmpvar_17;
  tmpvar_17.xyz = (tmpvar_15 * v_16.xyz);
  tmpvar_17.w = tmpvar_12.x;
  highp vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * unity_Scale.w);
  tmpvar_4 = tmpvar_18;
  vec4 v_19;
  v_19.x = _Object2World[0].y;
  v_19.y = _Object2World[1].y;
  v_19.z = _Object2World[2].y;
  v_19.w = _Object2World[3].y;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = (tmpvar_15 * v_19.xyz);
  tmpvar_20.w = tmpvar_12.y;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * unity_Scale.w);
  tmpvar_5 = tmpvar_21;
  vec4 v_22;
  v_22.x = _Object2World[0].z;
  v_22.y = _Object2World[1].z;
  v_22.z = _Object2World[2].z;
  v_22.w = _Object2World[3].z;
  highp vec4 tmpvar_23;
  tmpvar_23.xyz = (tmpvar_15 * v_22.xyz);
  tmpvar_23.w = tmpvar_12.z;
  highp vec4 tmpvar_24;
  tmpvar_24 = (tmpvar_23 * unity_Scale.w);
  tmpvar_6 = tmpvar_24;
  mat3 tmpvar_25;
  tmpvar_25[0] = _Object2World[0].xyz;
  tmpvar_25[1] = _Object2World[1].xyz;
  tmpvar_25[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_26;
  tmpvar_26 = (tmpvar_25 * (tmpvar_2 * unity_Scale.w));
  highp vec3 tmpvar_27;
  tmpvar_27 = (tmpvar_15 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_7 = tmpvar_27;
  highp vec4 tmpvar_28;
  tmpvar_28.w = 1.0;
  tmpvar_28.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_29;
  tmpvar_29.w = 1.0;
  tmpvar_29.xyz = tmpvar_26;
  mediump vec3 tmpvar_30;
  mediump vec4 normal_31;
  normal_31 = tmpvar_29;
  highp float vC_32;
  mediump vec3 x3_33;
  mediump vec3 x2_34;
  mediump vec3 x1_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHAr, normal_31);
  x1_35.x = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = dot (unity_SHAg, normal_31);
  x1_35.y = tmpvar_37;
  highp float tmpvar_38;
  tmpvar_38 = dot (unity_SHAb, normal_31);
  x1_35.z = tmpvar_38;
  mediump vec4 tmpvar_39;
  tmpvar_39 = (normal_31.xyzz * normal_31.yzzx);
  highp float tmpvar_40;
  tmpvar_40 = dot (unity_SHBr, tmpvar_39);
  x2_34.x = tmpvar_40;
  highp float tmpvar_41;
  tmpvar_41 = dot (unity_SHBg, tmpvar_39);
  x2_34.y = tmpvar_41;
  highp float tmpvar_42;
  tmpvar_42 = dot (unity_SHBb, tmpvar_39);
  x2_34.z = tmpvar_42;
  mediump float tmpvar_43;
  tmpvar_43 = ((normal_31.x * normal_31.x) - (normal_31.y * normal_31.y));
  vC_32 = tmpvar_43;
  highp vec3 tmpvar_44;
  tmpvar_44 = (unity_SHC.xyz * vC_32);
  x3_33 = tmpvar_44;
  tmpvar_30 = ((x1_35 + x2_34) + x3_33);
  shlight_3 = tmpvar_30;
  tmpvar_8 = shlight_3;
  highp vec3 tmpvar_45;
  tmpvar_45 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_46;
  tmpvar_46 = (unity_4LightPosX0 - tmpvar_45.x);
  highp vec4 tmpvar_47;
  tmpvar_47 = (unity_4LightPosY0 - tmpvar_45.y);
  highp vec4 tmpvar_48;
  tmpvar_48 = (unity_4LightPosZ0 - tmpvar_45.z);
  highp vec4 tmpvar_49;
  tmpvar_49 = (((tmpvar_46 * tmpvar_46) + (tmpvar_47 * tmpvar_47)) + (tmpvar_48 * tmpvar_48));
  highp vec4 tmpvar_50;
  tmpvar_50 = (max (vec4(0.0, 0.0, 0.0, 0.0), ((((tmpvar_46 * tmpvar_26.x) + (tmpvar_47 * tmpvar_26.y)) + (tmpvar_48 * tmpvar_26.z)) * inversesqrt(tmpvar_49))) * (1.0/((1.0 + (tmpvar_49 * unity_4LightAtten0)))));
  highp vec3 tmpvar_51;
  tmpvar_51 = (tmpvar_8 + ((((unity_LightColor[0].xyz * tmpvar_50.x) + (unity_LightColor[1].xyz * tmpvar_50.y)) + (unity_LightColor[2].xyz * tmpvar_50.z)) + (unity_LightColor[3].xyz * tmpvar_50.w)));
  tmpvar_8 = tmpvar_51;
  highp vec4 o_52;
  highp vec4 tmpvar_53;
  tmpvar_53 = (tmpvar_9 * 0.5);
  highp vec2 tmpvar_54;
  tmpvar_54.x = tmpvar_53.x;
  tmpvar_54.y = (tmpvar_53.y * _ProjectionParams.x);
  o_52.xy = (tmpvar_54 + tmpvar_53.w);
  o_52.zw = tmpvar_9.zw;
  gl_Position = tmpvar_9;
  xlv_TEXCOORD0 = (tmpvar_15 * (((_World2Object * tmpvar_28).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
  xlv_TEXCOORD6 = o_52;
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD6;
varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform sampler2D _ShadowMapTexture;
uniform lowp vec4 _LightColor0;
void main ()
{
  lowp vec4 c_1;
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  tmpvar_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD1.xyz;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD2.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD3.xyz;
  tmpvar_6 = tmpvar_10;
  tmpvar_2 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_11;
  lowp float tmpvar_12;
  mediump vec3 HDR_13;
  highp vec4 c_14;
  mediump vec3 tmpvar_15;
  tmpvar_15.x = tmpvar_4.z;
  tmpvar_15.y = tmpvar_5.z;
  tmpvar_15.z = tmpvar_6.z;
  highp vec3 tmpvar_16;
  tmpvar_16 = (tmpvar_3 - (2.0 * (dot (tmpvar_15, tmpvar_3) * tmpvar_15)));
  lowp vec4 tmpvar_17;
  tmpvar_17 = textureCube (_Cube, tmpvar_16);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * _ReflectColor);
  c_14 = tmpvar_18;
  highp vec3 tmpvar_19;
  tmpvar_19 = vec3((pow ((((c_14.x + c_14.y) + c_14.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_13 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = (1.0 - dot (normalize(tmpvar_2), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_21;
  tmpvar_21 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_20, _TransmissiveColor)));
  mediump float tmpvar_22;
  tmpvar_22 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_20)).x;
  tmpvar_12 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = ((c_14.xyz + HDR_13) + tmpvar_21.xyz);
  tmpvar_11 = tmpvar_23;
  lowp float tmpvar_24;
  tmpvar_24 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD6).x;
  highp vec3 tmpvar_25;
  tmpvar_25 = normalize(xlv_TEXCOORD0);
  mediump vec3 lightDir_26;
  lightDir_26 = xlv_TEXCOORD4;
  mediump vec3 viewDir_27;
  viewDir_27 = tmpvar_25;
  mediump float atten_28;
  atten_28 = tmpvar_24;
  mediump vec4 c_29;
  c_29.xyz = (((((((lightDir_26.z * 0.5) + 0.5) * tmpvar_11) * _LightColor0.xyz) * atten_28) * 2.0) + ((_LightColor0.xyz * max (0.0, (pow ((viewDir_27 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_12))) * atten_28));
  c_29.w = 1.0;
  c_1 = c_29;
  c_1.xyz = (c_1.xyz + (tmpvar_11 * xlv_TEXCOORD5));
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Color _glesColor
in vec4 _glesColor;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;
#define gl_MultiTexCoord1 _glesMultiTexCoord1
in vec4 _glesMultiTexCoord1;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
mat2 xll_transpose_mf2x2(mat2 m) {
  return mat2( m[0][0], m[1][0], m[0][1], m[1][1]);
}
mat3 xll_transpose_mf3x3(mat3 m) {
  return mat3( m[0][0], m[1][0], m[2][0],
               m[0][1], m[1][1], m[2][1],
               m[0][2], m[1][2], m[2][2]);
}
mat4 xll_transpose_mf4x4(mat4 m) {
  return mat4( m[0][0], m[1][0], m[2][0], m[3][0],
               m[0][1], m[1][1], m[2][1], m[3][1],
               m[0][2], m[1][2], m[2][2], m[3][2],
               m[0][3], m[1][3], m[2][3], m[3][3]);
}
vec2 xll_matrixindex_mf2x2_i (mat2 m, int i) { vec2 v; v.x=m[0][i]; v.y=m[1][i]; return v; }
vec3 xll_matrixindex_mf3x3_i (mat3 m, int i) { vec3 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; return v; }
vec4 xll_matrixindex_mf4x4_i (mat4 m, int i) { vec4 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; v.w=m[3][i]; return v; }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 407
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 440
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    lowp vec3 lightDir;
    lowp vec3 vlight;
    highp vec4 _ShadowCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform highp vec4 _ShadowOffsets[4];
uniform sampler2D _ShadowMapTexture;
#line 393
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 401
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 405
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 416
#line 452
#line 476
#line 82
highp vec3 ObjSpaceLightDir( in highp vec4 v ) {
    highp vec3 objSpaceLightPos = (_World2Object * _WorldSpaceLightPos0).xyz;
    return objSpaceLightPos.xyz;
}
#line 91
highp vec3 ObjSpaceViewDir( in highp vec4 v ) {
    highp vec3 objSpaceCameraPos = ((_World2Object * vec4( _WorldSpaceCameraPos.xyz, 1.0)).xyz * unity_Scale.w);
    return (objSpaceCameraPos - v.xyz);
}
#line 96
highp vec3 Shade4PointLights( in highp vec4 lightPosX, in highp vec4 lightPosY, in highp vec4 lightPosZ, in highp vec3 lightColor0, in highp vec3 lightColor1, in highp vec3 lightColor2, in highp vec3 lightColor3, in highp vec4 lightAttenSq, in highp vec3 pos, in highp vec3 normal ) {
    highp vec4 toLightX = (lightPosX - pos.x);
    highp vec4 toLightY = (lightPosY - pos.y);
    #line 100
    highp vec4 toLightZ = (lightPosZ - pos.z);
    highp vec4 lengthSq = vec4( 0.0);
    lengthSq += (toLightX * toLightX);
    lengthSq += (toLightY * toLightY);
    #line 104
    lengthSq += (toLightZ * toLightZ);
    highp vec4 ndotl = vec4( 0.0);
    ndotl += (toLightX * normal.x);
    ndotl += (toLightY * normal.y);
    #line 108
    ndotl += (toLightZ * normal.z);
    highp vec4 corr = inversesqrt(lengthSq);
    ndotl = max( vec4( 0.0, 0.0, 0.0, 0.0), (ndotl * corr));
    highp vec4 atten = (1.0 / (1.0 + (lengthSq * lightAttenSq)));
    #line 112
    highp vec4 diff = (ndotl * atten);
    highp vec3 col = vec3( 0.0);
    col += (lightColor0 * diff.x);
    col += (lightColor1 * diff.y);
    #line 116
    col += (lightColor2 * diff.z);
    col += (lightColor3 * diff.w);
    return col;
}
#line 137
mediump vec3 ShadeSH9( in mediump vec4 normal ) {
    mediump vec3 x1;
    mediump vec3 x2;
    mediump vec3 x3;
    x1.x = dot( unity_SHAr, normal);
    #line 141
    x1.y = dot( unity_SHAg, normal);
    x1.z = dot( unity_SHAb, normal);
    mediump vec4 vB = (normal.xyzz * normal.yzzx);
    x2.x = dot( unity_SHBr, vB);
    #line 145
    x2.y = dot( unity_SHBg, vB);
    x2.z = dot( unity_SHBb, vB);
    highp float vC = ((normal.x * normal.x) - (normal.y * normal.y));
    x3 = (unity_SHC.xyz * vC);
    #line 149
    return ((x1 + x2) + x3);
}
#line 452
v2f_surf vert_surf( in appdata_full v ) {
    v2f_surf o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 456
    highp vec3 viewDir = (-ObjSpaceViewDir( v.vertex));
    highp vec3 worldRefl = (mat3( _Object2World) * viewDir);
    highp vec3 binormal = (cross( v.normal, v.tangent.xyz) * v.tangent.w);
    highp mat3 rotation = xll_transpose_mf3x3(mat3( v.tangent.xyz, binormal, v.normal));
    #line 460
    o.TtoW0 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 0).xyz), worldRefl.x) * unity_Scale.w);
    o.TtoW1 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 1).xyz), worldRefl.y) * unity_Scale.w);
    o.TtoW2 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 2).xyz), worldRefl.z) * unity_Scale.w);
    highp vec3 worldN = (mat3( _Object2World) * (v.normal * unity_Scale.w));
    #line 464
    highp vec3 lightDir = (rotation * ObjSpaceLightDir( v.vertex));
    o.lightDir = lightDir;
    highp vec3 viewDirForLight = (rotation * ObjSpaceViewDir( v.vertex));
    o.viewDir = viewDirForLight;
    #line 468
    highp vec3 shlight = ShadeSH9( vec4( worldN, 1.0));
    o.vlight = shlight;
    highp vec3 worldPos = (_Object2World * v.vertex).xyz;
    o.vlight += Shade4PointLights( unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0, unity_LightColor[0].xyz, unity_LightColor[1].xyz, unity_LightColor[2].xyz, unity_LightColor[3].xyz, unity_4LightAtten0, worldPos, worldN);
    #line 472
    o._ShadowCoord = (unity_World2Shadow[0] * (_Object2World * v.vertex));
    return o;
}

out highp vec3 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_TEXCOORD2;
out lowp vec4 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
out lowp vec3 xlv_TEXCOORD5;
out highp vec4 xlv_TEXCOORD6;
void main() {
    v2f_surf xl_retval;
    appdata_full xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.tangent = vec4(TANGENT);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xlt_v.texcoord1 = vec4(gl_MultiTexCoord1);
    xlt_v.color = vec4(gl_Color);
    xl_retval = vert_surf( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD0 = vec3(xl_retval.viewDir);
    xlv_TEXCOORD1 = vec4(xl_retval.TtoW0);
    xlv_TEXCOORD2 = vec4(xl_retval.TtoW1);
    xlv_TEXCOORD3 = vec4(xl_retval.TtoW2);
    xlv_TEXCOORD4 = vec3(xl_retval.lightDir);
    xlv_TEXCOORD5 = vec3(xl_retval.vlight);
    xlv_TEXCOORD6 = vec4(xl_retval._ShadowCoord);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 407
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 440
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    lowp vec3 lightDir;
    lowp vec3 vlight;
    highp vec4 _ShadowCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform highp vec4 _ShadowOffsets[4];
uniform sampler2D _ShadowMapTexture;
#line 393
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 401
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 405
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 416
#line 452
#line 476
#line 430
mediump vec4 LightingHalfLambertSpecular( in SurfaceOutput s, in mediump vec3 lightDir, in mediump vec3 viewDir, in mediump float atten ) {
    #line 432
    mediump float NdotL = ((dot( s.Normal, lightDir) * 0.5) + 0.5);
    mediump vec3 halfVector = (viewDir + s.Normal);
    mediump float spec = max( 0.0, (pow( dot( halfVector, s.Normal), (10.0 - s.Specular)) * s.Gloss));
    mediump vec4 c;
    #line 436
    c.xyz = (((((NdotL * s.Albedo) * _LightColor0.xyz) * atten) * 2.0) + ((_LightColor0.xyz * spec) * atten));
    c.w = s.Alpha;
    return c;
}
#line 416
void surf( in Input IN, inout SurfaceOutput o ) {
    o.Normal = vec3( 0.0, 0.0, 1.0);
    highp vec3 worldRefl = reflect( IN.worldRefl, vec3( dot( IN.TtoW0, o.Normal), dot( IN.TtoW1, o.Normal), dot( IN.TtoW2, o.Normal)));
    #line 420
    highp vec4 c = (texture( _Cube, worldRefl) * _ReflectColor);
    mediump vec3 HDR = vec3( (pow( (((c.x + c.y) + c.z) / 3.0), _HdrPower) * _HdrContrast));
    mediump float fresnel = (1.0 - dot( normalize(IN.viewDir), normalize(vec3( 0.0, 0.0, 1.0))));
    mediump vec4 fresnelLerp = mix( _Color, _FresnelColor, vec4( pow( fresnel, _TransmissiveColor)));
    #line 424
    mediump vec4 GlossLerp = mix( vec4( 0.0, 0.0, 0.0, 0.0), vec4( 1.0, 1.0, 1.0, 1.0), vec4( fresnel));
    o.Specular = _Shininess;
    o.Gloss = GlossLerp.x;
    o.Albedo = ((c.xyz + HDR) + fresnelLerp.xyz);
    #line 428
    o.Alpha = 1.0;
}
#line 393
lowp float unitySampleShadow( in highp vec4 shadowCoord ) {
    highp float dist = textureProj( _ShadowMapTexture, shadowCoord).x;
    mediump float lightShadowDataX = _LightShadowData.x;
    #line 397
    return max( float((dist > (shadowCoord.z / shadowCoord.w))), lightShadowDataX);
}
#line 476
lowp vec4 frag_surf( in v2f_surf IN ) {
    Input surfIN;
    surfIN.worldRefl = vec3( IN.TtoW0.w, IN.TtoW1.w, IN.TtoW2.w);
    #line 480
    surfIN.TtoW0 = IN.TtoW0.xyz;
    surfIN.TtoW1 = IN.TtoW1.xyz;
    surfIN.TtoW2 = IN.TtoW2.xyz;
    surfIN.viewDir = IN.viewDir;
    #line 484
    SurfaceOutput o;
    o.Albedo = vec3( 0.0);
    o.Emission = vec3( 0.0);
    o.Specular = 0.0;
    #line 488
    o.Alpha = 0.0;
    o.Gloss = 0.0;
    surf( surfIN, o);
    lowp float atten = unitySampleShadow( IN._ShadowCoord);
    #line 492
    lowp vec4 c = vec4( 0.0);
    c = LightingHalfLambertSpecular( o, IN.lightDir, normalize(IN.viewDir), atten);
    c.xyz += (o.Albedo * IN.vlight);
    return c;
}
in highp vec3 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_TEXCOORD2;
in lowp vec4 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in lowp vec3 xlv_TEXCOORD5;
in highp vec4 xlv_TEXCOORD6;
void main() {
    lowp vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.viewDir = vec3(xlv_TEXCOORD0);
    xlt_IN.TtoW0 = vec4(xlv_TEXCOORD1);
    xlt_IN.TtoW1 = vec4(xlv_TEXCOORD2);
    xlt_IN.TtoW2 = vec4(xlv_TEXCOORD3);
    xlt_IN.lightDir = vec3(xlv_TEXCOORD4);
    xlt_IN.vlight = vec3(xlv_TEXCOORD5);
    xlt_IN._ShadowCoord = vec4(xlv_TEXCOORD6);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
varying highp vec4 xlv_TEXCOORD6;
varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 unity_SHC;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAr;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  lowp vec4 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (_glesVertex.xyz - ((_World2Object * tmpvar_9).xyz * unity_Scale.w)));
  highp vec3 tmpvar_12;
  highp vec3 tmpvar_13;
  tmpvar_12 = tmpvar_1.xyz;
  tmpvar_13 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_13.x;
  tmpvar_14[0].z = tmpvar_2.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_13.y;
  tmpvar_14[1].z = tmpvar_2.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_13.z;
  tmpvar_14[2].z = tmpvar_2.z;
  vec4 v_15;
  v_15.x = _Object2World[0].x;
  v_15.y = _Object2World[1].x;
  v_15.z = _Object2World[2].x;
  v_15.w = _Object2World[3].x;
  highp vec4 tmpvar_16;
  tmpvar_16.xyz = (tmpvar_14 * v_15.xyz);
  tmpvar_16.w = tmpvar_11.x;
  highp vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * unity_Scale.w);
  tmpvar_4 = tmpvar_17;
  vec4 v_18;
  v_18.x = _Object2World[0].y;
  v_18.y = _Object2World[1].y;
  v_18.z = _Object2World[2].y;
  v_18.w = _Object2World[3].y;
  highp vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_14 * v_18.xyz);
  tmpvar_19.w = tmpvar_11.y;
  highp vec4 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * unity_Scale.w);
  tmpvar_5 = tmpvar_20;
  vec4 v_21;
  v_21.x = _Object2World[0].z;
  v_21.y = _Object2World[1].z;
  v_21.z = _Object2World[2].z;
  v_21.w = _Object2World[3].z;
  highp vec4 tmpvar_22;
  tmpvar_22.xyz = (tmpvar_14 * v_21.xyz);
  tmpvar_22.w = tmpvar_11.z;
  highp vec4 tmpvar_23;
  tmpvar_23 = (tmpvar_22 * unity_Scale.w);
  tmpvar_6 = tmpvar_23;
  mat3 tmpvar_24;
  tmpvar_24[0] = _Object2World[0].xyz;
  tmpvar_24[1] = _Object2World[1].xyz;
  tmpvar_24[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_14 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_7 = tmpvar_25;
  highp vec4 tmpvar_26;
  tmpvar_26.w = 1.0;
  tmpvar_26.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_27;
  tmpvar_27.w = 1.0;
  tmpvar_27.xyz = (tmpvar_24 * (tmpvar_2 * unity_Scale.w));
  mediump vec3 tmpvar_28;
  mediump vec4 normal_29;
  normal_29 = tmpvar_27;
  highp float vC_30;
  mediump vec3 x3_31;
  mediump vec3 x2_32;
  mediump vec3 x1_33;
  highp float tmpvar_34;
  tmpvar_34 = dot (unity_SHAr, normal_29);
  x1_33.x = tmpvar_34;
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHAg, normal_29);
  x1_33.y = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHAb, normal_29);
  x1_33.z = tmpvar_36;
  mediump vec4 tmpvar_37;
  tmpvar_37 = (normal_29.xyzz * normal_29.yzzx);
  highp float tmpvar_38;
  tmpvar_38 = dot (unity_SHBr, tmpvar_37);
  x2_32.x = tmpvar_38;
  highp float tmpvar_39;
  tmpvar_39 = dot (unity_SHBg, tmpvar_37);
  x2_32.y = tmpvar_39;
  highp float tmpvar_40;
  tmpvar_40 = dot (unity_SHBb, tmpvar_37);
  x2_32.z = tmpvar_40;
  mediump float tmpvar_41;
  tmpvar_41 = ((normal_29.x * normal_29.x) - (normal_29.y * normal_29.y));
  vC_30 = tmpvar_41;
  highp vec3 tmpvar_42;
  tmpvar_42 = (unity_SHC.xyz * vC_30);
  x3_31 = tmpvar_42;
  tmpvar_28 = ((x1_33 + x2_32) + x3_31);
  shlight_3 = tmpvar_28;
  tmpvar_8 = shlight_3;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_14 * (((_World2Object * tmpvar_26).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
  xlv_TEXCOORD6 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
varying highp vec4 xlv_TEXCOORD6;
varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform lowp vec4 _LightColor0;
uniform highp vec4 _LightShadowData;
void main ()
{
  lowp vec4 c_1;
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  tmpvar_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD1.xyz;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD2.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD3.xyz;
  tmpvar_6 = tmpvar_10;
  tmpvar_2 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_11;
  lowp float tmpvar_12;
  mediump vec3 HDR_13;
  highp vec4 c_14;
  mediump vec3 tmpvar_15;
  tmpvar_15.x = tmpvar_4.z;
  tmpvar_15.y = tmpvar_5.z;
  tmpvar_15.z = tmpvar_6.z;
  highp vec3 tmpvar_16;
  tmpvar_16 = (tmpvar_3 - (2.0 * (dot (tmpvar_15, tmpvar_3) * tmpvar_15)));
  lowp vec4 tmpvar_17;
  tmpvar_17 = textureCube (_Cube, tmpvar_16);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * _ReflectColor);
  c_14 = tmpvar_18;
  highp vec3 tmpvar_19;
  tmpvar_19 = vec3((pow ((((c_14.x + c_14.y) + c_14.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_13 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = (1.0 - dot (normalize(tmpvar_2), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_21;
  tmpvar_21 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_20, _TransmissiveColor)));
  mediump float tmpvar_22;
  tmpvar_22 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_20)).x;
  tmpvar_12 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = ((c_14.xyz + HDR_13) + tmpvar_21.xyz);
  tmpvar_11 = tmpvar_23;
  lowp float shadow_24;
  lowp float tmpvar_25;
  tmpvar_25 = shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD6.xyz);
  highp float tmpvar_26;
  tmpvar_26 = (_LightShadowData.x + (tmpvar_25 * (1.0 - _LightShadowData.x)));
  shadow_24 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = normalize(xlv_TEXCOORD0);
  mediump vec3 lightDir_28;
  lightDir_28 = xlv_TEXCOORD4;
  mediump vec3 viewDir_29;
  viewDir_29 = tmpvar_27;
  mediump float atten_30;
  atten_30 = shadow_24;
  mediump vec4 c_31;
  c_31.xyz = (((((((lightDir_28.z * 0.5) + 0.5) * tmpvar_11) * _LightColor0.xyz) * atten_30) * 2.0) + ((_LightColor0.xyz * max (0.0, (pow ((viewDir_29 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_12))) * atten_30));
  c_31.w = 1.0;
  c_1 = c_31;
  c_1.xyz = (c_1.xyz + (tmpvar_11 * xlv_TEXCOORD5));
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Color _glesColor
in vec4 _glesColor;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;
#define gl_MultiTexCoord1 _glesMultiTexCoord1
in vec4 _glesMultiTexCoord1;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
mat2 xll_transpose_mf2x2(mat2 m) {
  return mat2( m[0][0], m[1][0], m[0][1], m[1][1]);
}
mat3 xll_transpose_mf3x3(mat3 m) {
  return mat3( m[0][0], m[1][0], m[2][0],
               m[0][1], m[1][1], m[2][1],
               m[0][2], m[1][2], m[2][2]);
}
mat4 xll_transpose_mf4x4(mat4 m) {
  return mat4( m[0][0], m[1][0], m[2][0], m[3][0],
               m[0][1], m[1][1], m[2][1], m[3][1],
               m[0][2], m[1][2], m[2][2], m[3][2],
               m[0][3], m[1][3], m[2][3], m[3][3]);
}
vec2 xll_matrixindex_mf2x2_i (mat2 m, int i) { vec2 v; v.x=m[0][i]; v.y=m[1][i]; return v; }
vec3 xll_matrixindex_mf3x3_i (mat3 m, int i) { vec3 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; return v; }
vec4 xll_matrixindex_mf4x4_i (mat4 m, int i) { vec4 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; v.w=m[3][i]; return v; }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 407
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 440
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    lowp vec3 lightDir;
    lowp vec3 vlight;
    highp vec4 _ShadowCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform highp vec4 _ShadowOffsets[4];
uniform lowp sampler2DShadow _ShadowMapTexture;
#line 393
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 401
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 405
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 416
#line 452
#line 82
highp vec3 ObjSpaceLightDir( in highp vec4 v ) {
    highp vec3 objSpaceLightPos = (_World2Object * _WorldSpaceLightPos0).xyz;
    return objSpaceLightPos.xyz;
}
#line 91
highp vec3 ObjSpaceViewDir( in highp vec4 v ) {
    highp vec3 objSpaceCameraPos = ((_World2Object * vec4( _WorldSpaceCameraPos.xyz, 1.0)).xyz * unity_Scale.w);
    return (objSpaceCameraPos - v.xyz);
}
#line 137
mediump vec3 ShadeSH9( in mediump vec4 normal ) {
    mediump vec3 x1;
    mediump vec3 x2;
    mediump vec3 x3;
    x1.x = dot( unity_SHAr, normal);
    #line 141
    x1.y = dot( unity_SHAg, normal);
    x1.z = dot( unity_SHAb, normal);
    mediump vec4 vB = (normal.xyzz * normal.yzzx);
    x2.x = dot( unity_SHBr, vB);
    #line 145
    x2.y = dot( unity_SHBg, vB);
    x2.z = dot( unity_SHBb, vB);
    highp float vC = ((normal.x * normal.x) - (normal.y * normal.y));
    x3 = (unity_SHC.xyz * vC);
    #line 149
    return ((x1 + x2) + x3);
}
#line 452
v2f_surf vert_surf( in appdata_full v ) {
    v2f_surf o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 456
    highp vec3 viewDir = (-ObjSpaceViewDir( v.vertex));
    highp vec3 worldRefl = (mat3( _Object2World) * viewDir);
    highp vec3 binormal = (cross( v.normal, v.tangent.xyz) * v.tangent.w);
    highp mat3 rotation = xll_transpose_mf3x3(mat3( v.tangent.xyz, binormal, v.normal));
    #line 460
    o.TtoW0 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 0).xyz), worldRefl.x) * unity_Scale.w);
    o.TtoW1 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 1).xyz), worldRefl.y) * unity_Scale.w);
    o.TtoW2 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 2).xyz), worldRefl.z) * unity_Scale.w);
    highp vec3 worldN = (mat3( _Object2World) * (v.normal * unity_Scale.w));
    #line 464
    highp vec3 lightDir = (rotation * ObjSpaceLightDir( v.vertex));
    o.lightDir = lightDir;
    highp vec3 viewDirForLight = (rotation * ObjSpaceViewDir( v.vertex));
    o.viewDir = viewDirForLight;
    #line 468
    highp vec3 shlight = ShadeSH9( vec4( worldN, 1.0));
    o.vlight = shlight;
    o._ShadowCoord = (unity_World2Shadow[0] * (_Object2World * v.vertex));
    #line 472
    return o;
}

out highp vec3 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_TEXCOORD2;
out lowp vec4 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
out lowp vec3 xlv_TEXCOORD5;
out highp vec4 xlv_TEXCOORD6;
void main() {
    v2f_surf xl_retval;
    appdata_full xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.tangent = vec4(TANGENT);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xlt_v.texcoord1 = vec4(gl_MultiTexCoord1);
    xlt_v.color = vec4(gl_Color);
    xl_retval = vert_surf( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD0 = vec3(xl_retval.viewDir);
    xlv_TEXCOORD1 = vec4(xl_retval.TtoW0);
    xlv_TEXCOORD2 = vec4(xl_retval.TtoW1);
    xlv_TEXCOORD3 = vec4(xl_retval.TtoW2);
    xlv_TEXCOORD4 = vec3(xl_retval.lightDir);
    xlv_TEXCOORD5 = vec3(xl_retval.vlight);
    xlv_TEXCOORD6 = vec4(xl_retval._ShadowCoord);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];
float xll_shadow2D(mediump sampler2DShadow s, vec3 coord) { return texture (s, coord); }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 407
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 440
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    lowp vec3 lightDir;
    lowp vec3 vlight;
    highp vec4 _ShadowCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform highp vec4 _ShadowOffsets[4];
uniform lowp sampler2DShadow _ShadowMapTexture;
#line 393
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 401
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 405
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 416
#line 452
#line 430
mediump vec4 LightingHalfLambertSpecular( in SurfaceOutput s, in mediump vec3 lightDir, in mediump vec3 viewDir, in mediump float atten ) {
    #line 432
    mediump float NdotL = ((dot( s.Normal, lightDir) * 0.5) + 0.5);
    mediump vec3 halfVector = (viewDir + s.Normal);
    mediump float spec = max( 0.0, (pow( dot( halfVector, s.Normal), (10.0 - s.Specular)) * s.Gloss));
    mediump vec4 c;
    #line 436
    c.xyz = (((((NdotL * s.Albedo) * _LightColor0.xyz) * atten) * 2.0) + ((_LightColor0.xyz * spec) * atten));
    c.w = s.Alpha;
    return c;
}
#line 416
void surf( in Input IN, inout SurfaceOutput o ) {
    o.Normal = vec3( 0.0, 0.0, 1.0);
    highp vec3 worldRefl = reflect( IN.worldRefl, vec3( dot( IN.TtoW0, o.Normal), dot( IN.TtoW1, o.Normal), dot( IN.TtoW2, o.Normal)));
    #line 420
    highp vec4 c = (texture( _Cube, worldRefl) * _ReflectColor);
    mediump vec3 HDR = vec3( (pow( (((c.x + c.y) + c.z) / 3.0), _HdrPower) * _HdrContrast));
    mediump float fresnel = (1.0 - dot( normalize(IN.viewDir), normalize(vec3( 0.0, 0.0, 1.0))));
    mediump vec4 fresnelLerp = mix( _Color, _FresnelColor, vec4( pow( fresnel, _TransmissiveColor)));
    #line 424
    mediump vec4 GlossLerp = mix( vec4( 0.0, 0.0, 0.0, 0.0), vec4( 1.0, 1.0, 1.0, 1.0), vec4( fresnel));
    o.Specular = _Shininess;
    o.Gloss = GlossLerp.x;
    o.Albedo = ((c.xyz + HDR) + fresnelLerp.xyz);
    #line 428
    o.Alpha = 1.0;
}
#line 393
lowp float unitySampleShadow( in highp vec4 shadowCoord ) {
    lowp float shadow = xll_shadow2D( _ShadowMapTexture, shadowCoord.xyz.xyz);
    shadow = (_LightShadowData.x + (shadow * (1.0 - _LightShadowData.x)));
    #line 397
    return shadow;
}
#line 474
lowp vec4 frag_surf( in v2f_surf IN ) {
    #line 476
    Input surfIN;
    surfIN.worldRefl = vec3( IN.TtoW0.w, IN.TtoW1.w, IN.TtoW2.w);
    surfIN.TtoW0 = IN.TtoW0.xyz;
    surfIN.TtoW1 = IN.TtoW1.xyz;
    #line 480
    surfIN.TtoW2 = IN.TtoW2.xyz;
    surfIN.viewDir = IN.viewDir;
    SurfaceOutput o;
    o.Albedo = vec3( 0.0);
    #line 484
    o.Emission = vec3( 0.0);
    o.Specular = 0.0;
    o.Alpha = 0.0;
    o.Gloss = 0.0;
    #line 488
    surf( surfIN, o);
    lowp float atten = unitySampleShadow( IN._ShadowCoord);
    lowp vec4 c = vec4( 0.0);
    c = LightingHalfLambertSpecular( o, IN.lightDir, normalize(IN.viewDir), atten);
    #line 492
    c.xyz += (o.Albedo * IN.vlight);
    return c;
}
in highp vec3 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_TEXCOORD2;
in lowp vec4 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in lowp vec3 xlv_TEXCOORD5;
in highp vec4 xlv_TEXCOORD6;
void main() {
    lowp vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.viewDir = vec3(xlv_TEXCOORD0);
    xlt_IN.TtoW0 = vec4(xlv_TEXCOORD1);
    xlt_IN.TtoW1 = vec4(xlv_TEXCOORD2);
    xlt_IN.TtoW2 = vec4(xlv_TEXCOORD3);
    xlt_IN.lightDir = vec3(xlv_TEXCOORD4);
    xlt_IN.vlight = vec3(xlv_TEXCOORD5);
    xlt_IN._ShadowCoord = vec4(xlv_TEXCOORD6);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
varying highp vec4 xlv_TEXCOORD5;
varying highp vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec4 _glesMultiTexCoord1;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (_glesVertex.xyz - ((_World2Object * tmpvar_6).xyz * unity_Scale.w)));
  highp vec3 tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_9 = tmpvar_1.xyz;
  tmpvar_10 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_11;
  tmpvar_11[0].x = tmpvar_9.x;
  tmpvar_11[0].y = tmpvar_10.x;
  tmpvar_11[0].z = tmpvar_2.x;
  tmpvar_11[1].x = tmpvar_9.y;
  tmpvar_11[1].y = tmpvar_10.y;
  tmpvar_11[1].z = tmpvar_2.y;
  tmpvar_11[2].x = tmpvar_9.z;
  tmpvar_11[2].y = tmpvar_10.z;
  tmpvar_11[2].z = tmpvar_2.z;
  vec4 v_12;
  v_12.x = _Object2World[0].x;
  v_12.y = _Object2World[1].x;
  v_12.z = _Object2World[2].x;
  v_12.w = _Object2World[3].x;
  highp vec4 tmpvar_13;
  tmpvar_13.xyz = (tmpvar_11 * v_12.xyz);
  tmpvar_13.w = tmpvar_8.x;
  highp vec4 tmpvar_14;
  tmpvar_14 = (tmpvar_13 * unity_Scale.w);
  tmpvar_3 = tmpvar_14;
  vec4 v_15;
  v_15.x = _Object2World[0].y;
  v_15.y = _Object2World[1].y;
  v_15.z = _Object2World[2].y;
  v_15.w = _Object2World[3].y;
  highp vec4 tmpvar_16;
  tmpvar_16.xyz = (tmpvar_11 * v_15.xyz);
  tmpvar_16.w = tmpvar_8.y;
  highp vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * unity_Scale.w);
  tmpvar_4 = tmpvar_17;
  vec4 v_18;
  v_18.x = _Object2World[0].z;
  v_18.y = _Object2World[1].z;
  v_18.z = _Object2World[2].z;
  v_18.w = _Object2World[3].z;
  highp vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_11 * v_18.xyz);
  tmpvar_19.w = tmpvar_8.z;
  highp vec4 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * unity_Scale.w);
  tmpvar_5 = tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = _WorldSpaceCameraPos;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_11 * (((_World2Object * tmpvar_21).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD5 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
varying highp vec4 xlv_TEXCOORD5;
varying highp vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform sampler2D unity_Lightmap;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform highp vec4 _LightShadowData;
void main ()
{
  lowp vec4 c_1;
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  tmpvar_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD1.xyz;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD2.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD3.xyz;
  tmpvar_6 = tmpvar_10;
  tmpvar_2 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_11;
  mediump vec3 HDR_12;
  highp vec4 c_13;
  mediump vec3 tmpvar_14;
  tmpvar_14.x = tmpvar_4.z;
  tmpvar_14.y = tmpvar_5.z;
  tmpvar_14.z = tmpvar_6.z;
  highp vec3 tmpvar_15;
  tmpvar_15 = (tmpvar_3 - (2.0 * (dot (tmpvar_14, tmpvar_3) * tmpvar_14)));
  lowp vec4 tmpvar_16;
  tmpvar_16 = textureCube (_Cube, tmpvar_15);
  mediump vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * _ReflectColor);
  c_13 = tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_18 = vec3((pow ((((c_13.x + c_13.y) + c_13.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_12 = tmpvar_18;
  mediump vec4 tmpvar_19;
  tmpvar_19 = mix (_Color, _FresnelColor, vec4(pow ((1.0 - dot (normalize(tmpvar_2), normalize(vec3(0.0, 0.0, 1.0)))), _TransmissiveColor)));
  highp vec3 tmpvar_20;
  tmpvar_20 = ((c_13.xyz + HDR_12) + tmpvar_19.xyz);
  tmpvar_11 = tmpvar_20;
  lowp float shadow_21;
  lowp float tmpvar_22;
  tmpvar_22 = shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD5.xyz);
  highp float tmpvar_23;
  tmpvar_23 = (_LightShadowData.x + (tmpvar_22 * (1.0 - _LightShadowData.x)));
  shadow_21 = tmpvar_23;
  c_1.xyz = (tmpvar_11 * min ((2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD4).xyz), vec3((shadow_21 * 2.0))));
  c_1.w = 1.0;
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Color _glesColor
in vec4 _glesColor;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;
#define gl_MultiTexCoord1 _glesMultiTexCoord1
in vec4 _glesMultiTexCoord1;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
mat2 xll_transpose_mf2x2(mat2 m) {
  return mat2( m[0][0], m[1][0], m[0][1], m[1][1]);
}
mat3 xll_transpose_mf3x3(mat3 m) {
  return mat3( m[0][0], m[1][0], m[2][0],
               m[0][1], m[1][1], m[2][1],
               m[0][2], m[1][2], m[2][2]);
}
mat4 xll_transpose_mf4x4(mat4 m) {
  return mat4( m[0][0], m[1][0], m[2][0], m[3][0],
               m[0][1], m[1][1], m[2][1], m[3][1],
               m[0][2], m[1][2], m[2][2], m[3][2],
               m[0][3], m[1][3], m[2][3], m[3][3]);
}
vec2 xll_matrixindex_mf2x2_i (mat2 m, int i) { vec2 v; v.x=m[0][i]; v.y=m[1][i]; return v; }
vec3 xll_matrixindex_mf3x3_i (mat3 m, int i) { vec3 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; return v; }
vec4 xll_matrixindex_mf4x4_i (mat4 m, int i) { vec4 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; v.w=m[3][i]; return v; }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 407
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 440
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    highp vec2 lmap;
    highp vec4 _ShadowCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform highp vec4 _ShadowOffsets[4];
uniform lowp sampler2DShadow _ShadowMapTexture;
#line 393
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 401
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 405
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 416
#line 451
uniform highp vec4 unity_LightmapST;
#line 472
uniform sampler2D unity_Lightmap;
#line 82
highp vec3 ObjSpaceLightDir( in highp vec4 v ) {
    highp vec3 objSpaceLightPos = (_World2Object * _WorldSpaceLightPos0).xyz;
    return objSpaceLightPos.xyz;
}
#line 91
highp vec3 ObjSpaceViewDir( in highp vec4 v ) {
    highp vec3 objSpaceCameraPos = ((_World2Object * vec4( _WorldSpaceCameraPos.xyz, 1.0)).xyz * unity_Scale.w);
    return (objSpaceCameraPos - v.xyz);
}
#line 452
v2f_surf vert_surf( in appdata_full v ) {
    v2f_surf o;
    #line 455
    o.pos = (glstate_matrix_mvp * v.vertex);
    highp vec3 viewDir = (-ObjSpaceViewDir( v.vertex));
    highp vec3 worldRefl = (mat3( _Object2World) * viewDir);
    highp vec3 binormal = (cross( v.normal, v.tangent.xyz) * v.tangent.w);
    #line 459
    highp mat3 rotation = xll_transpose_mf3x3(mat3( v.tangent.xyz, binormal, v.normal));
    o.TtoW0 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 0).xyz), worldRefl.x) * unity_Scale.w);
    o.TtoW1 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 1).xyz), worldRefl.y) * unity_Scale.w);
    o.TtoW2 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 2).xyz), worldRefl.z) * unity_Scale.w);
    #line 463
    o.lmap.xy = ((v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
    highp vec3 worldN = (mat3( _Object2World) * (v.normal * unity_Scale.w));
    highp vec3 lightDir = (rotation * ObjSpaceLightDir( v.vertex));
    highp vec3 viewDirForLight = (rotation * ObjSpaceViewDir( v.vertex));
    #line 467
    o.viewDir = viewDirForLight;
    o._ShadowCoord = (unity_World2Shadow[0] * (_Object2World * v.vertex));
    return o;
}

out highp vec3 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_TEXCOORD2;
out lowp vec4 xlv_TEXCOORD3;
out highp vec2 xlv_TEXCOORD4;
out highp vec4 xlv_TEXCOORD5;
void main() {
    v2f_surf xl_retval;
    appdata_full xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.tangent = vec4(TANGENT);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xlt_v.texcoord1 = vec4(gl_MultiTexCoord1);
    xlt_v.color = vec4(gl_Color);
    xl_retval = vert_surf( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD0 = vec3(xl_retval.viewDir);
    xlv_TEXCOORD1 = vec4(xl_retval.TtoW0);
    xlv_TEXCOORD2 = vec4(xl_retval.TtoW1);
    xlv_TEXCOORD3 = vec4(xl_retval.TtoW2);
    xlv_TEXCOORD4 = vec2(xl_retval.lmap);
    xlv_TEXCOORD5 = vec4(xl_retval._ShadowCoord);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];
float xll_shadow2D(mediump sampler2DShadow s, vec3 coord) { return texture (s, coord); }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 407
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 440
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    highp vec2 lmap;
    highp vec4 _ShadowCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform highp vec4 _ShadowOffsets[4];
uniform lowp sampler2DShadow _ShadowMapTexture;
#line 393
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 401
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 405
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 416
#line 451
uniform highp vec4 unity_LightmapST;
#line 472
uniform sampler2D unity_Lightmap;
#line 177
lowp vec3 DecodeLightmap( in lowp vec4 color ) {
    #line 179
    return (2.0 * color.xyz);
}
#line 416
void surf( in Input IN, inout SurfaceOutput o ) {
    o.Normal = vec3( 0.0, 0.0, 1.0);
    highp vec3 worldRefl = reflect( IN.worldRefl, vec3( dot( IN.TtoW0, o.Normal), dot( IN.TtoW1, o.Normal), dot( IN.TtoW2, o.Normal)));
    #line 420
    highp vec4 c = (texture( _Cube, worldRefl) * _ReflectColor);
    mediump vec3 HDR = vec3( (pow( (((c.x + c.y) + c.z) / 3.0), _HdrPower) * _HdrContrast));
    mediump float fresnel = (1.0 - dot( normalize(IN.viewDir), normalize(vec3( 0.0, 0.0, 1.0))));
    mediump vec4 fresnelLerp = mix( _Color, _FresnelColor, vec4( pow( fresnel, _TransmissiveColor)));
    #line 424
    mediump vec4 GlossLerp = mix( vec4( 0.0, 0.0, 0.0, 0.0), vec4( 1.0, 1.0, 1.0, 1.0), vec4( fresnel));
    o.Specular = _Shininess;
    o.Gloss = GlossLerp.x;
    o.Albedo = ((c.xyz + HDR) + fresnelLerp.xyz);
    #line 428
    o.Alpha = 1.0;
}
#line 393
lowp float unitySampleShadow( in highp vec4 shadowCoord ) {
    lowp float shadow = xll_shadow2D( _ShadowMapTexture, shadowCoord.xyz.xyz);
    shadow = (_LightShadowData.x + (shadow * (1.0 - _LightShadowData.x)));
    #line 397
    return shadow;
}
#line 473
lowp vec4 frag_surf( in v2f_surf IN ) {
    Input surfIN;
    #line 476
    surfIN.worldRefl = vec3( IN.TtoW0.w, IN.TtoW1.w, IN.TtoW2.w);
    surfIN.TtoW0 = IN.TtoW0.xyz;
    surfIN.TtoW1 = IN.TtoW1.xyz;
    surfIN.TtoW2 = IN.TtoW2.xyz;
    #line 480
    surfIN.viewDir = IN.viewDir;
    SurfaceOutput o;
    o.Albedo = vec3( 0.0);
    o.Emission = vec3( 0.0);
    #line 484
    o.Specular = 0.0;
    o.Alpha = 0.0;
    o.Gloss = 0.0;
    surf( surfIN, o);
    #line 488
    lowp float atten = unitySampleShadow( IN._ShadowCoord);
    lowp vec4 c = vec4( 0.0);
    lowp vec4 lmtex = texture( unity_Lightmap, IN.lmap.xy);
    lowp vec3 lm = DecodeLightmap( lmtex);
    #line 492
    c.xyz += (o.Albedo * min( lm, vec3( (atten * 2.0))));
    c.w = o.Alpha;
    return c;
}
in highp vec3 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_TEXCOORD2;
in lowp vec4 xlv_TEXCOORD3;
in highp vec2 xlv_TEXCOORD4;
in highp vec4 xlv_TEXCOORD5;
void main() {
    lowp vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.viewDir = vec3(xlv_TEXCOORD0);
    xlt_IN.TtoW0 = vec4(xlv_TEXCOORD1);
    xlt_IN.TtoW1 = vec4(xlv_TEXCOORD2);
    xlt_IN.TtoW2 = vec4(xlv_TEXCOORD3);
    xlt_IN.lmap = vec2(xlv_TEXCOORD4);
    xlt_IN._ShadowCoord = vec4(xlv_TEXCOORD5);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
varying highp vec4 xlv_TEXCOORD6;
varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 unity_SHC;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosX0;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  highp vec3 shlight_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  lowp vec4 tmpvar_6;
  lowp vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (_glesVertex.xyz - ((_World2Object * tmpvar_9).xyz * unity_Scale.w)));
  highp vec3 tmpvar_12;
  highp vec3 tmpvar_13;
  tmpvar_12 = tmpvar_1.xyz;
  tmpvar_13 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_14;
  tmpvar_14[0].x = tmpvar_12.x;
  tmpvar_14[0].y = tmpvar_13.x;
  tmpvar_14[0].z = tmpvar_2.x;
  tmpvar_14[1].x = tmpvar_12.y;
  tmpvar_14[1].y = tmpvar_13.y;
  tmpvar_14[1].z = tmpvar_2.y;
  tmpvar_14[2].x = tmpvar_12.z;
  tmpvar_14[2].y = tmpvar_13.z;
  tmpvar_14[2].z = tmpvar_2.z;
  vec4 v_15;
  v_15.x = _Object2World[0].x;
  v_15.y = _Object2World[1].x;
  v_15.z = _Object2World[2].x;
  v_15.w = _Object2World[3].x;
  highp vec4 tmpvar_16;
  tmpvar_16.xyz = (tmpvar_14 * v_15.xyz);
  tmpvar_16.w = tmpvar_11.x;
  highp vec4 tmpvar_17;
  tmpvar_17 = (tmpvar_16 * unity_Scale.w);
  tmpvar_4 = tmpvar_17;
  vec4 v_18;
  v_18.x = _Object2World[0].y;
  v_18.y = _Object2World[1].y;
  v_18.z = _Object2World[2].y;
  v_18.w = _Object2World[3].y;
  highp vec4 tmpvar_19;
  tmpvar_19.xyz = (tmpvar_14 * v_18.xyz);
  tmpvar_19.w = tmpvar_11.y;
  highp vec4 tmpvar_20;
  tmpvar_20 = (tmpvar_19 * unity_Scale.w);
  tmpvar_5 = tmpvar_20;
  vec4 v_21;
  v_21.x = _Object2World[0].z;
  v_21.y = _Object2World[1].z;
  v_21.z = _Object2World[2].z;
  v_21.w = _Object2World[3].z;
  highp vec4 tmpvar_22;
  tmpvar_22.xyz = (tmpvar_14 * v_21.xyz);
  tmpvar_22.w = tmpvar_11.z;
  highp vec4 tmpvar_23;
  tmpvar_23 = (tmpvar_22 * unity_Scale.w);
  tmpvar_6 = tmpvar_23;
  mat3 tmpvar_24;
  tmpvar_24[0] = _Object2World[0].xyz;
  tmpvar_24[1] = _Object2World[1].xyz;
  tmpvar_24[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_24 * (tmpvar_2 * unity_Scale.w));
  highp vec3 tmpvar_26;
  tmpvar_26 = (tmpvar_14 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_7 = tmpvar_26;
  highp vec4 tmpvar_27;
  tmpvar_27.w = 1.0;
  tmpvar_27.xyz = _WorldSpaceCameraPos;
  highp vec4 tmpvar_28;
  tmpvar_28.w = 1.0;
  tmpvar_28.xyz = tmpvar_25;
  mediump vec3 tmpvar_29;
  mediump vec4 normal_30;
  normal_30 = tmpvar_28;
  highp float vC_31;
  mediump vec3 x3_32;
  mediump vec3 x2_33;
  mediump vec3 x1_34;
  highp float tmpvar_35;
  tmpvar_35 = dot (unity_SHAr, normal_30);
  x1_34.x = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = dot (unity_SHAg, normal_30);
  x1_34.y = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = dot (unity_SHAb, normal_30);
  x1_34.z = tmpvar_37;
  mediump vec4 tmpvar_38;
  tmpvar_38 = (normal_30.xyzz * normal_30.yzzx);
  highp float tmpvar_39;
  tmpvar_39 = dot (unity_SHBr, tmpvar_38);
  x2_33.x = tmpvar_39;
  highp float tmpvar_40;
  tmpvar_40 = dot (unity_SHBg, tmpvar_38);
  x2_33.y = tmpvar_40;
  highp float tmpvar_41;
  tmpvar_41 = dot (unity_SHBb, tmpvar_38);
  x2_33.z = tmpvar_41;
  mediump float tmpvar_42;
  tmpvar_42 = ((normal_30.x * normal_30.x) - (normal_30.y * normal_30.y));
  vC_31 = tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43 = (unity_SHC.xyz * vC_31);
  x3_32 = tmpvar_43;
  tmpvar_29 = ((x1_34 + x2_33) + x3_32);
  shlight_3 = tmpvar_29;
  tmpvar_8 = shlight_3;
  highp vec3 tmpvar_44;
  tmpvar_44 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_45;
  tmpvar_45 = (unity_4LightPosX0 - tmpvar_44.x);
  highp vec4 tmpvar_46;
  tmpvar_46 = (unity_4LightPosY0 - tmpvar_44.y);
  highp vec4 tmpvar_47;
  tmpvar_47 = (unity_4LightPosZ0 - tmpvar_44.z);
  highp vec4 tmpvar_48;
  tmpvar_48 = (((tmpvar_45 * tmpvar_45) + (tmpvar_46 * tmpvar_46)) + (tmpvar_47 * tmpvar_47));
  highp vec4 tmpvar_49;
  tmpvar_49 = (max (vec4(0.0, 0.0, 0.0, 0.0), ((((tmpvar_45 * tmpvar_25.x) + (tmpvar_46 * tmpvar_25.y)) + (tmpvar_47 * tmpvar_25.z)) * inversesqrt(tmpvar_48))) * (1.0/((1.0 + (tmpvar_48 * unity_4LightAtten0)))));
  highp vec3 tmpvar_50;
  tmpvar_50 = (tmpvar_8 + ((((unity_LightColor[0].xyz * tmpvar_49.x) + (unity_LightColor[1].xyz * tmpvar_49.y)) + (unity_LightColor[2].xyz * tmpvar_49.z)) + (unity_LightColor[3].xyz * tmpvar_49.w)));
  tmpvar_8 = tmpvar_50;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_14 * (((_World2Object * tmpvar_27).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_4;
  xlv_TEXCOORD2 = tmpvar_5;
  xlv_TEXCOORD3 = tmpvar_6;
  xlv_TEXCOORD4 = tmpvar_7;
  xlv_TEXCOORD5 = tmpvar_8;
  xlv_TEXCOORD6 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
varying highp vec4 xlv_TEXCOORD6;
varying lowp vec3 xlv_TEXCOORD5;
varying lowp vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform lowp vec4 _LightColor0;
uniform highp vec4 _LightShadowData;
void main ()
{
  lowp vec4 c_1;
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  mediump vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  lowp vec3 tmpvar_7;
  tmpvar_7.x = xlv_TEXCOORD1.w;
  tmpvar_7.y = xlv_TEXCOORD2.w;
  tmpvar_7.z = xlv_TEXCOORD3.w;
  tmpvar_3 = tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8 = xlv_TEXCOORD1.xyz;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD2.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD3.xyz;
  tmpvar_6 = tmpvar_10;
  tmpvar_2 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_11;
  lowp float tmpvar_12;
  mediump vec3 HDR_13;
  highp vec4 c_14;
  mediump vec3 tmpvar_15;
  tmpvar_15.x = tmpvar_4.z;
  tmpvar_15.y = tmpvar_5.z;
  tmpvar_15.z = tmpvar_6.z;
  highp vec3 tmpvar_16;
  tmpvar_16 = (tmpvar_3 - (2.0 * (dot (tmpvar_15, tmpvar_3) * tmpvar_15)));
  lowp vec4 tmpvar_17;
  tmpvar_17 = textureCube (_Cube, tmpvar_16);
  mediump vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * _ReflectColor);
  c_14 = tmpvar_18;
  highp vec3 tmpvar_19;
  tmpvar_19 = vec3((pow ((((c_14.x + c_14.y) + c_14.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_13 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = (1.0 - dot (normalize(tmpvar_2), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_21;
  tmpvar_21 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_20, _TransmissiveColor)));
  mediump float tmpvar_22;
  tmpvar_22 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_20)).x;
  tmpvar_12 = tmpvar_22;
  highp vec3 tmpvar_23;
  tmpvar_23 = ((c_14.xyz + HDR_13) + tmpvar_21.xyz);
  tmpvar_11 = tmpvar_23;
  lowp float shadow_24;
  lowp float tmpvar_25;
  tmpvar_25 = shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD6.xyz);
  highp float tmpvar_26;
  tmpvar_26 = (_LightShadowData.x + (tmpvar_25 * (1.0 - _LightShadowData.x)));
  shadow_24 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = normalize(xlv_TEXCOORD0);
  mediump vec3 lightDir_28;
  lightDir_28 = xlv_TEXCOORD4;
  mediump vec3 viewDir_29;
  viewDir_29 = tmpvar_27;
  mediump float atten_30;
  atten_30 = shadow_24;
  mediump vec4 c_31;
  c_31.xyz = (((((((lightDir_28.z * 0.5) + 0.5) * tmpvar_11) * _LightColor0.xyz) * atten_30) * 2.0) + ((_LightColor0.xyz * max (0.0, (pow ((viewDir_29 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_12))) * atten_30));
  c_31.w = 1.0;
  c_1 = c_31;
  c_1.xyz = (c_1.xyz + (tmpvar_11 * xlv_TEXCOORD5));
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "VERTEXLIGHT_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Color _glesColor
in vec4 _glesColor;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;
#define gl_MultiTexCoord1 _glesMultiTexCoord1
in vec4 _glesMultiTexCoord1;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
mat2 xll_transpose_mf2x2(mat2 m) {
  return mat2( m[0][0], m[1][0], m[0][1], m[1][1]);
}
mat3 xll_transpose_mf3x3(mat3 m) {
  return mat3( m[0][0], m[1][0], m[2][0],
               m[0][1], m[1][1], m[2][1],
               m[0][2], m[1][2], m[2][2]);
}
mat4 xll_transpose_mf4x4(mat4 m) {
  return mat4( m[0][0], m[1][0], m[2][0], m[3][0],
               m[0][1], m[1][1], m[2][1], m[3][1],
               m[0][2], m[1][2], m[2][2], m[3][2],
               m[0][3], m[1][3], m[2][3], m[3][3]);
}
vec2 xll_matrixindex_mf2x2_i (mat2 m, int i) { vec2 v; v.x=m[0][i]; v.y=m[1][i]; return v; }
vec3 xll_matrixindex_mf3x3_i (mat3 m, int i) { vec3 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; return v; }
vec4 xll_matrixindex_mf4x4_i (mat4 m, int i) { vec4 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; v.w=m[3][i]; return v; }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 407
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 440
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    lowp vec3 lightDir;
    lowp vec3 vlight;
    highp vec4 _ShadowCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform highp vec4 _ShadowOffsets[4];
uniform lowp sampler2DShadow _ShadowMapTexture;
#line 393
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 401
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 405
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 416
#line 452
#line 476
#line 82
highp vec3 ObjSpaceLightDir( in highp vec4 v ) {
    highp vec3 objSpaceLightPos = (_World2Object * _WorldSpaceLightPos0).xyz;
    return objSpaceLightPos.xyz;
}
#line 91
highp vec3 ObjSpaceViewDir( in highp vec4 v ) {
    highp vec3 objSpaceCameraPos = ((_World2Object * vec4( _WorldSpaceCameraPos.xyz, 1.0)).xyz * unity_Scale.w);
    return (objSpaceCameraPos - v.xyz);
}
#line 96
highp vec3 Shade4PointLights( in highp vec4 lightPosX, in highp vec4 lightPosY, in highp vec4 lightPosZ, in highp vec3 lightColor0, in highp vec3 lightColor1, in highp vec3 lightColor2, in highp vec3 lightColor3, in highp vec4 lightAttenSq, in highp vec3 pos, in highp vec3 normal ) {
    highp vec4 toLightX = (lightPosX - pos.x);
    highp vec4 toLightY = (lightPosY - pos.y);
    #line 100
    highp vec4 toLightZ = (lightPosZ - pos.z);
    highp vec4 lengthSq = vec4( 0.0);
    lengthSq += (toLightX * toLightX);
    lengthSq += (toLightY * toLightY);
    #line 104
    lengthSq += (toLightZ * toLightZ);
    highp vec4 ndotl = vec4( 0.0);
    ndotl += (toLightX * normal.x);
    ndotl += (toLightY * normal.y);
    #line 108
    ndotl += (toLightZ * normal.z);
    highp vec4 corr = inversesqrt(lengthSq);
    ndotl = max( vec4( 0.0, 0.0, 0.0, 0.0), (ndotl * corr));
    highp vec4 atten = (1.0 / (1.0 + (lengthSq * lightAttenSq)));
    #line 112
    highp vec4 diff = (ndotl * atten);
    highp vec3 col = vec3( 0.0);
    col += (lightColor0 * diff.x);
    col += (lightColor1 * diff.y);
    #line 116
    col += (lightColor2 * diff.z);
    col += (lightColor3 * diff.w);
    return col;
}
#line 137
mediump vec3 ShadeSH9( in mediump vec4 normal ) {
    mediump vec3 x1;
    mediump vec3 x2;
    mediump vec3 x3;
    x1.x = dot( unity_SHAr, normal);
    #line 141
    x1.y = dot( unity_SHAg, normal);
    x1.z = dot( unity_SHAb, normal);
    mediump vec4 vB = (normal.xyzz * normal.yzzx);
    x2.x = dot( unity_SHBr, vB);
    #line 145
    x2.y = dot( unity_SHBg, vB);
    x2.z = dot( unity_SHBb, vB);
    highp float vC = ((normal.x * normal.x) - (normal.y * normal.y));
    x3 = (unity_SHC.xyz * vC);
    #line 149
    return ((x1 + x2) + x3);
}
#line 452
v2f_surf vert_surf( in appdata_full v ) {
    v2f_surf o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 456
    highp vec3 viewDir = (-ObjSpaceViewDir( v.vertex));
    highp vec3 worldRefl = (mat3( _Object2World) * viewDir);
    highp vec3 binormal = (cross( v.normal, v.tangent.xyz) * v.tangent.w);
    highp mat3 rotation = xll_transpose_mf3x3(mat3( v.tangent.xyz, binormal, v.normal));
    #line 460
    o.TtoW0 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 0).xyz), worldRefl.x) * unity_Scale.w);
    o.TtoW1 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 1).xyz), worldRefl.y) * unity_Scale.w);
    o.TtoW2 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 2).xyz), worldRefl.z) * unity_Scale.w);
    highp vec3 worldN = (mat3( _Object2World) * (v.normal * unity_Scale.w));
    #line 464
    highp vec3 lightDir = (rotation * ObjSpaceLightDir( v.vertex));
    o.lightDir = lightDir;
    highp vec3 viewDirForLight = (rotation * ObjSpaceViewDir( v.vertex));
    o.viewDir = viewDirForLight;
    #line 468
    highp vec3 shlight = ShadeSH9( vec4( worldN, 1.0));
    o.vlight = shlight;
    highp vec3 worldPos = (_Object2World * v.vertex).xyz;
    o.vlight += Shade4PointLights( unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0, unity_LightColor[0].xyz, unity_LightColor[1].xyz, unity_LightColor[2].xyz, unity_LightColor[3].xyz, unity_4LightAtten0, worldPos, worldN);
    #line 472
    o._ShadowCoord = (unity_World2Shadow[0] * (_Object2World * v.vertex));
    return o;
}

out highp vec3 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_TEXCOORD2;
out lowp vec4 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
out lowp vec3 xlv_TEXCOORD5;
out highp vec4 xlv_TEXCOORD6;
void main() {
    v2f_surf xl_retval;
    appdata_full xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.tangent = vec4(TANGENT);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xlt_v.texcoord1 = vec4(gl_MultiTexCoord1);
    xlt_v.color = vec4(gl_Color);
    xl_retval = vert_surf( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD0 = vec3(xl_retval.viewDir);
    xlv_TEXCOORD1 = vec4(xl_retval.TtoW0);
    xlv_TEXCOORD2 = vec4(xl_retval.TtoW1);
    xlv_TEXCOORD3 = vec4(xl_retval.TtoW2);
    xlv_TEXCOORD4 = vec3(xl_retval.lightDir);
    xlv_TEXCOORD5 = vec3(xl_retval.vlight);
    xlv_TEXCOORD6 = vec4(xl_retval._ShadowCoord);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];
float xll_shadow2D(mediump sampler2DShadow s, vec3 coord) { return texture (s, coord); }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 407
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 440
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    lowp vec3 lightDir;
    lowp vec3 vlight;
    highp vec4 _ShadowCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform highp vec4 _ShadowOffsets[4];
uniform lowp sampler2DShadow _ShadowMapTexture;
#line 393
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 401
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 405
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 416
#line 452
#line 476
#line 430
mediump vec4 LightingHalfLambertSpecular( in SurfaceOutput s, in mediump vec3 lightDir, in mediump vec3 viewDir, in mediump float atten ) {
    #line 432
    mediump float NdotL = ((dot( s.Normal, lightDir) * 0.5) + 0.5);
    mediump vec3 halfVector = (viewDir + s.Normal);
    mediump float spec = max( 0.0, (pow( dot( halfVector, s.Normal), (10.0 - s.Specular)) * s.Gloss));
    mediump vec4 c;
    #line 436
    c.xyz = (((((NdotL * s.Albedo) * _LightColor0.xyz) * atten) * 2.0) + ((_LightColor0.xyz * spec) * atten));
    c.w = s.Alpha;
    return c;
}
#line 416
void surf( in Input IN, inout SurfaceOutput o ) {
    o.Normal = vec3( 0.0, 0.0, 1.0);
    highp vec3 worldRefl = reflect( IN.worldRefl, vec3( dot( IN.TtoW0, o.Normal), dot( IN.TtoW1, o.Normal), dot( IN.TtoW2, o.Normal)));
    #line 420
    highp vec4 c = (texture( _Cube, worldRefl) * _ReflectColor);
    mediump vec3 HDR = vec3( (pow( (((c.x + c.y) + c.z) / 3.0), _HdrPower) * _HdrContrast));
    mediump float fresnel = (1.0 - dot( normalize(IN.viewDir), normalize(vec3( 0.0, 0.0, 1.0))));
    mediump vec4 fresnelLerp = mix( _Color, _FresnelColor, vec4( pow( fresnel, _TransmissiveColor)));
    #line 424
    mediump vec4 GlossLerp = mix( vec4( 0.0, 0.0, 0.0, 0.0), vec4( 1.0, 1.0, 1.0, 1.0), vec4( fresnel));
    o.Specular = _Shininess;
    o.Gloss = GlossLerp.x;
    o.Albedo = ((c.xyz + HDR) + fresnelLerp.xyz);
    #line 428
    o.Alpha = 1.0;
}
#line 393
lowp float unitySampleShadow( in highp vec4 shadowCoord ) {
    lowp float shadow = xll_shadow2D( _ShadowMapTexture, shadowCoord.xyz.xyz);
    shadow = (_LightShadowData.x + (shadow * (1.0 - _LightShadowData.x)));
    #line 397
    return shadow;
}
#line 476
lowp vec4 frag_surf( in v2f_surf IN ) {
    Input surfIN;
    surfIN.worldRefl = vec3( IN.TtoW0.w, IN.TtoW1.w, IN.TtoW2.w);
    #line 480
    surfIN.TtoW0 = IN.TtoW0.xyz;
    surfIN.TtoW1 = IN.TtoW1.xyz;
    surfIN.TtoW2 = IN.TtoW2.xyz;
    surfIN.viewDir = IN.viewDir;
    #line 484
    SurfaceOutput o;
    o.Albedo = vec3( 0.0);
    o.Emission = vec3( 0.0);
    o.Specular = 0.0;
    #line 488
    o.Alpha = 0.0;
    o.Gloss = 0.0;
    surf( surfIN, o);
    lowp float atten = unitySampleShadow( IN._ShadowCoord);
    #line 492
    lowp vec4 c = vec4( 0.0);
    c = LightingHalfLambertSpecular( o, IN.lightDir, normalize(IN.viewDir), atten);
    c.xyz += (o.Albedo * IN.vlight);
    return c;
}
in highp vec3 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_TEXCOORD2;
in lowp vec4 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in lowp vec3 xlv_TEXCOORD5;
in highp vec4 xlv_TEXCOORD6;
void main() {
    lowp vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.viewDir = vec3(xlv_TEXCOORD0);
    xlt_IN.TtoW0 = vec4(xlv_TEXCOORD1);
    xlt_IN.TtoW1 = vec4(xlv_TEXCOORD2);
    xlt_IN.TtoW2 = vec4(xlv_TEXCOORD3);
    xlt_IN.lightDir = vec3(xlv_TEXCOORD4);
    xlt_IN.vlight = vec3(xlv_TEXCOORD5);
    xlt_IN._ShadowCoord = vec4(xlv_TEXCOORD6);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

}
Program "fp" {
// Fragment combos: 4
//   opengl - ALU: 29 to 40, TEX: 1 to 3
//   d3d9 - ALU: 31 to 46, TEX: 1 to 3
//   d3d11 - ALU: 23 to 36, TEX: 1 to 3, FLOW: 1 to 1
SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Vector 0 [_LightColor0]
Vector 1 [_ReflectColor]
Vector 2 [_FresnelColor]
Vector 3 [_Color]
Float 4 [_TransmissiveColor]
Float 5 [_Shininess]
Float 6 [_HdrPower]
Float 7 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
"3.0-!!ARBfp1.0
# 37 ALU, 1 TEX
PARAM c[10] = { program.local[0..7],
		{ 2, 0.33333334, 1, 0.5 },
		{ 10, 0 } };
TEMP R0;
TEMP R1;
TEMP R2;
MOV R2.x, c[9];
MOV R1.x, fragment.texcoord[1].z;
MOV R1.y, fragment.texcoord[2].z;
MOV R1.z, fragment.texcoord[3];
MOV R0.x, fragment.texcoord[1].w;
MOV R0.z, fragment.texcoord[3].w;
MOV R0.y, fragment.texcoord[2].w;
DP3 R0.w, R1, R0;
MUL R1.xyz, R1, R0.w;
MAD R0.xyz, -R1, c[8].x, R0;
TEX R0.xyz, R0, texture[0], CUBE;
MUL R0.xyz, R0, c[1];
ADD R0.w, R0.x, R0.y;
ADD R0.w, R0, R0.z;
MUL R0.w, R0, c[8].y;
POW R0.w, R0.w, c[6].x;
MAD R0.xyz, R0.w, c[7].x, R0;
DP3 R0.w, fragment.texcoord[0], fragment.texcoord[0];
RSQ R1.w, R0.w;
MAD R0.w, -R1, fragment.texcoord[0].z, c[8].z;
MOV R1.xyz, c[3];
POW R2.y, R0.w, c[4].x;
ADD R1.xyz, -R1, c[2];
MAD R1.xyz, R2.y, R1, c[3];
ADD R0.xyz, R0, R1;
ADD R2.x, R2, -c[5];
MAD R1.w, R1, fragment.texcoord[0].z, c[8].z;
POW R1.w, R1.w, R2.x;
MUL R1.x, R0.w, R1.w;
MAX R1.w, R1.x, c[9].y;
MAD R0.w, fragment.texcoord[4].z, c[8], c[8];
MUL R1.xyz, R0, R0.w;
MUL R2.xyz, R1.w, c[0];
MUL R1.xyz, R1, c[0];
MAD R1.xyz, R1, c[8].x, R2;
MAD result.color.xyz, R0, fragment.texcoord[5], R1;
MOV result.color.w, c[8].z;
END
# 37 instructions, 3 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Vector 0 [_LightColor0]
Vector 1 [_ReflectColor]
Vector 2 [_FresnelColor]
Vector 3 [_Color]
Float 4 [_TransmissiveColor]
Float 5 [_Shininess]
Float 6 [_HdrPower]
Float 7 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
"ps_3_0
; 43 ALU, 1 TEX
dcl_cube s0
def c8, 2.00000000, 0.33333334, 1.00000000, 0.50000000
def c9, 10.00000000, 0.00000000, 0, 0
dcl_texcoord0 v0.xyz
dcl_texcoord1 v1.xyzw
dcl_texcoord2 v2.xyzw
dcl_texcoord3 v3.xyzw
dcl_texcoord4 v4.xyz
dcl_texcoord5 v5.xyz
mov_pp r1.x, v1.z
mov_pp r1.y, v2.z
mov_pp r1.z, v3
mov r0.x, v1.w
mov r0.z, v3.w
mov r0.y, v2.w
dp3 r0.w, r1, r0
mul r1.xyz, r1, r0.w
mad r0.xyz, -r1, c8.x, r0
texld r0.xyz, r0, s0
mul r1.xyz, r0, c1
add r0.x, r1, r1.y
add r0.x, r0, r1.z
mul r1.w, r0.x, c8.y
pow r0, r1.w, c6.x
mad r1.xyz, r0.x, c7.x, r1
dp3_pp r0.x, v0, v0
rsq_pp r1.w, r0.x
mad_pp r2.x, r1.w, v0.z, c8.z
mov_pp r0.y, c5.x
add_pp r2.y, c9.x, -r0
pow_pp r0, r2.x, r2.y
mad_pp r1.w, -r1, v0.z, c8.z
pow_pp r2, r1.w, c4.x
mov_pp r0.yzw, c2.xxyz
add_pp r0.yzw, -c3.xxyz, r0
mad_pp r2.xyz, r2.x, r0.yzww, c3
mov_pp r0.w, r0.x
add r0.xyz, r1, r2
mul_pp r1.x, r1.w, r0.w
max_pp r1.w, r1.x, c9.y
mad_pp r0.w, v4.z, c8, c8
mul_pp r1.xyz, r0, r0.w
mul_pp r2.xyz, r1.w, c0
mul_pp r1.xyz, r1, c0
mad_pp r1.xyz, r1, c8.x, r2
mad_pp oC0.xyz, r0, v5, r1
mov_pp oC0.w, c8.z
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
ConstBuffer "$Globals" 112 // 112 used size, 10 vars
Vector 16 [_LightColor0] 4
Vector 48 [_ReflectColor] 4
Vector 64 [_FresnelColor] 4
Vector 80 [_Color] 4
Float 96 [_TransmissiveColor]
Float 100 [_Shininess]
Float 104 [_HdrPower]
Float 108 [_HdrContrast]
BindCB "$Globals" 0
SetTexture 0 [_Cube] CUBE 0
// 42 instructions, 3 temp regs, 0 temp arrays:
// ALU 33 float, 0 int, 0 uint
// TEX 1 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0
eefiecedgphgopccoigkocfahhjlaefjkblpfjjnabaaaaaageagaaaaadaaaaaa
cmaaaaaapmaaaaaadaabaaaaejfdeheomiaaaaaaahaaaaaaaiaaaaaalaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaalmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahahaaaalmaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apamaaaalmaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaaapamaaaalmaaaaaa
adaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapamaaaalmaaaaaaaeaaaaaaaaaaaaaa
adaaaaaaafaaaaaaahaeaaaalmaaaaaaafaaaaaaaaaaaaaaadaaaaaaagaaaaaa
ahahaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklklfdeieefccmafaaaaeaaaaaaaelabaaaa
fjaaaaaeegiocaaaaaaaaaaaahaaaaaafkaaaaadaagabaaaaaaaaaaafidaaaae
aahabaaaaaaaaaaaffffaaaagcbaaaadhcbabaaaabaaaaaagcbaaaadmcbabaaa
acaaaaaagcbaaaadmcbabaaaadaaaaaagcbaaaadmcbabaaaaeaaaaaagcbaaaad
ecbabaaaafaaaaaagcbaaaadhcbabaaaagaaaaaagfaaaaadpccabaaaaaaaaaaa
giaaaaacadaaaaaadgaaaaafbcaabaaaaaaaaaaadkbabaaaacaaaaaadgaaaaaf
ccaabaaaaaaaaaaadkbabaaaadaaaaaadgaaaaafecaabaaaaaaaaaaadkbabaaa
aeaaaaaadgaaaaafbcaabaaaabaaaaaackbabaaaacaaaaaadgaaaaafccaabaaa
abaaaaaackbabaaaadaaaaaadgaaaaafecaabaaaabaaaaaackbabaaaaeaaaaaa
baaaaaahicaabaaaaaaaaaaaegacbaaaaaaaaaaaegacbaaaabaaaaaaaaaaaaah
icaabaaaaaaaaaaadkaabaaaaaaaaaaadkaabaaaaaaaaaaadcaaaaakhcaabaaa
aaaaaaaaegacbaaaabaaaaaapgapbaiaebaaaaaaaaaaaaaaegacbaaaaaaaaaaa
efaaaaajpcaabaaaaaaaaaaaegacbaaaaaaaaaaaeghobaaaaaaaaaaaaagabaaa
aaaaaaaadiaaaaailcaabaaaaaaaaaaaegaibaaaaaaaaaaaegiicaaaaaaaaaaa
adaaaaaaaaaaaaahbcaabaaaabaaaaaabkaabaaaaaaaaaaaakaabaaaaaaaaaaa
dcaaaaakecaabaaaaaaaaaaackaabaaaaaaaaaaackiacaaaaaaaaaaaadaaaaaa
akaabaaaabaaaaaadiaaaaahecaabaaaaaaaaaaackaabaaaaaaaaaaaabeaaaaa
klkkkkdocpaaaaafecaabaaaaaaaaaaackaabaaaaaaaaaaadiaaaaaiecaabaaa
aaaaaaaackaabaaaaaaaaaaackiacaaaaaaaaaaaagaaaaaabjaaaaafecaabaaa
aaaaaaaackaabaaaaaaaaaaadcaaaaakhcaabaaaaaaaaaaakgakbaaaaaaaaaaa
pgipcaaaaaaaaaaaagaaaaaaegadbaaaaaaaaaaabaaaaaahicaabaaaaaaaaaaa
egbcbaaaabaaaaaaegbcbaaaabaaaaaaeeaaaaaficaabaaaaaaaaaaadkaabaaa
aaaaaaaadcaaaaakbcaabaaaabaaaaaackbabaiaebaaaaaaabaaaaaadkaabaaa
aaaaaaaaabeaaaaaaaaaiadpdcaaaaajicaabaaaaaaaaaaackbabaaaabaaaaaa
dkaabaaaaaaaaaaaabeaaaaaaaaaiadpcpaaaaaficaabaaaaaaaaaaadkaabaaa
aaaaaaaacpaaaaafccaabaaaabaaaaaaakaabaaaabaaaaaadiaaaaaiccaabaaa
abaaaaaabkaabaaaabaaaaaaakiacaaaaaaaaaaaagaaaaaabjaaaaafccaabaaa
abaaaaaabkaabaaaabaaaaaaaaaaaaakhcaabaaaacaaaaaaegiccaaaaaaaaaaa
aeaaaaaaegiccaiaebaaaaaaaaaaaaaaafaaaaaadcaaaaakocaabaaaabaaaaaa
fgafbaaaabaaaaaaagajbaaaacaaaaaaagijcaaaaaaaaaaaafaaaaaaaaaaaaah
hcaabaaaaaaaaaaaegacbaaaaaaaaaaajgahbaaaabaaaaaadcaaaaajccaabaaa
abaaaaaackbabaaaafaaaaaaabeaaaaaaaaaaadpabeaaaaaaaaaaadpdiaaaaah
ocaabaaaabaaaaaaagajbaaaaaaaaaaafgafbaaaabaaaaaadiaaaaaiocaabaaa
abaaaaaafgaobaaaabaaaaaaagijcaaaaaaaaaaaabaaaaaaaaaaaaahocaabaaa
abaaaaaafgaobaaaabaaaaaafgaobaaaabaaaaaaaaaaaaajbcaabaaaacaaaaaa
bkiacaiaebaaaaaaaaaaaaaaagaaaaaaabeaaaaaaaaacaebdiaaaaahicaabaaa
aaaaaaaadkaabaaaaaaaaaaaakaabaaaacaaaaaabjaaaaaficaabaaaaaaaaaaa
dkaabaaaaaaaaaaadiaaaaahicaabaaaaaaaaaaaakaabaaaabaaaaaadkaabaaa
aaaaaaaadeaaaaahicaabaaaaaaaaaaadkaabaaaaaaaaaaaabeaaaaaaaaaaaaa
dcaaaaakhcaabaaaabaaaaaaegiccaaaaaaaaaaaabaaaaaapgapbaaaaaaaaaaa
jgahbaaaabaaaaaadcaaaaajhccabaaaaaaaaaaaegacbaaaaaaaaaaaegbcbaaa
agaaaaaaegacbaaaabaaaaaadgaaaaaficcabaaaaaaaaaaaabeaaaaaaaaaiadp
doaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES3"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Vector 0 [_ReflectColor]
Vector 1 [_FresnelColor]
Vector 2 [_Color]
Float 3 [_TransmissiveColor]
Float 4 [_HdrPower]
Float 5 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
SetTexture 1 [unity_Lightmap] 2D
"3.0-!!ARBfp1.0
# 29 ALU, 2 TEX
PARAM c[7] = { program.local[0..5],
		{ 0.33333334, 1, 2, 8 } };
TEMP R0;
TEMP R1;
TEMP R2;
MOV R1.x, fragment.texcoord[1].z;
MOV R1.y, fragment.texcoord[2].z;
MOV R1.z, fragment.texcoord[3];
MOV R0.x, fragment.texcoord[1].w;
MOV R0.z, fragment.texcoord[3].w;
MOV R0.y, fragment.texcoord[2].w;
DP3 R0.w, R1, R0;
MUL R1.xyz, R1, R0.w;
MAD R0.xyz, -R1, c[6].z, R0;
TEX R0.xyz, R0, texture[0], CUBE;
MUL R0.xyz, R0, c[0];
ADD R0.w, R0.x, R0.y;
ADD R0.w, R0, R0.z;
MUL R0.w, R0, c[6].x;
POW R0.w, R0.w, c[4].x;
MAD R1.xyz, R0.w, c[5].x, R0;
DP3 R1.w, fragment.texcoord[0], fragment.texcoord[0];
RSQ R0.w, R1.w;
MOV R0.xyz, c[2];
MAD R0.w, -R0, fragment.texcoord[0].z, c[6].y;
ADD R0.xyz, -R0, c[1];
POW R0.w, R0.w, c[3].x;
MAD R2.xyz, R0.w, R0, c[2];
TEX R0, fragment.texcoord[4], texture[1], 2D;
ADD R1.xyz, R1, R2;
MUL R0.xyz, R0.w, R0;
MUL R0.xyz, R0, R1;
MUL result.color.xyz, R0, c[6].w;
MOV result.color.w, c[6].y;
END
# 29 instructions, 3 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
Vector 0 [_ReflectColor]
Vector 1 [_FresnelColor]
Vector 2 [_Color]
Float 3 [_TransmissiveColor]
Float 4 [_HdrPower]
Float 5 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
SetTexture 1 [unity_Lightmap] 2D
"ps_3_0
; 31 ALU, 2 TEX
dcl_cube s0
dcl_2d s1
def c6, 2.00000000, 0.33333334, 1.00000000, 8.00000000
dcl_texcoord0 v0.xyz
dcl_texcoord1 v1.xyzw
dcl_texcoord2 v2.xyzw
dcl_texcoord3 v3.xyzw
dcl_texcoord4 v4.xy
mov_pp r2.xyz, c1
mov_pp r1.x, v1.z
mov_pp r1.y, v2.z
mov_pp r1.z, v3
mov r0.x, v1.w
mov r0.z, v3.w
mov r0.y, v2.w
dp3 r0.w, r1, r0
mul r1.xyz, r1, r0.w
mad r0.xyz, -r1, c6.x, r0
texld r0.xyz, r0, s0
mul r1.xyz, r0, c0
add r0.x, r1, r1.y
add r0.x, r0, r1.z
mul r1.w, r0.x, c6.y
pow r0, r1.w, c4.x
dp3_pp r0.y, v0, v0
rsq_pp r0.y, r0.y
mad r1.xyz, r0.x, c5.x, r1
mad_pp r1.w, -r0.y, v0.z, c6.z
pow_pp r0, r1.w, c3.x
add_pp r2.xyz, -c2, r2
mad_pp r2.xyz, r0.x, r2, c2
texld r0, v4, s1
add r1.xyz, r1, r2
mul_pp r0.xyz, r0.w, r0
mul_pp r0.xyz, r0, r1
mul_pp oC0.xyz, r0, c6.w
mov_pp oC0.w, c6.z
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
ConstBuffer "$Globals" 128 // 112 used size, 11 vars
Vector 48 [_ReflectColor] 4
Vector 64 [_FresnelColor] 4
Vector 80 [_Color] 4
Float 96 [_TransmissiveColor]
Float 104 [_HdrPower]
Float 108 [_HdrContrast]
BindCB "$Globals" 0
SetTexture 0 [_Cube] CUBE 0
SetTexture 1 [unity_Lightmap] 2D 1
// 33 instructions, 2 temp regs, 0 temp arrays:
// ALU 23 float, 0 int, 0 uint
// TEX 2 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0
eefiecedbhnomddkoigaednlmmlkofjmlghhkaieabaaaaaaeiafaaaaadaaaaaa
cmaaaaaaoeaaaaaabiabaaaaejfdeheolaaaaaaaagaaaaaaaiaaaaaajiaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaakeaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahahaaaakeaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apamaaaakeaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaaapamaaaakeaaaaaa
adaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapamaaaakeaaaaaaaeaaaaaaaaaaaaaa
adaaaaaaafaaaaaaadadaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklklepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcciaeaaaa
eaaaaaaaakabaaaafjaaaaaeegiocaaaaaaaaaaaahaaaaaafkaaaaadaagabaaa
aaaaaaaafkaaaaadaagabaaaabaaaaaafidaaaaeaahabaaaaaaaaaaaffffaaaa
fibiaaaeaahabaaaabaaaaaaffffaaaagcbaaaadhcbabaaaabaaaaaagcbaaaad
mcbabaaaacaaaaaagcbaaaadmcbabaaaadaaaaaagcbaaaadmcbabaaaaeaaaaaa
gcbaaaaddcbabaaaafaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacacaaaaaa
dgaaaaafbcaabaaaaaaaaaaadkbabaaaacaaaaaadgaaaaafccaabaaaaaaaaaaa
dkbabaaaadaaaaaadgaaaaafecaabaaaaaaaaaaadkbabaaaaeaaaaaadgaaaaaf
bcaabaaaabaaaaaackbabaaaacaaaaaadgaaaaafccaabaaaabaaaaaackbabaaa
adaaaaaadgaaaaafecaabaaaabaaaaaackbabaaaaeaaaaaabaaaaaahicaabaaa
aaaaaaaaegacbaaaaaaaaaaaegacbaaaabaaaaaaaaaaaaahicaabaaaaaaaaaaa
dkaabaaaaaaaaaaadkaabaaaaaaaaaaadcaaaaakhcaabaaaaaaaaaaaegacbaaa
abaaaaaapgapbaiaebaaaaaaaaaaaaaaegacbaaaaaaaaaaaefaaaaajpcaabaaa
aaaaaaaaegacbaaaaaaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadiaaaaai
lcaabaaaaaaaaaaaegaibaaaaaaaaaaaegiicaaaaaaaaaaaadaaaaaaaaaaaaah
bcaabaaaabaaaaaabkaabaaaaaaaaaaaakaabaaaaaaaaaaadcaaaaakecaabaaa
aaaaaaaackaabaaaaaaaaaaackiacaaaaaaaaaaaadaaaaaaakaabaaaabaaaaaa
diaaaaahecaabaaaaaaaaaaackaabaaaaaaaaaaaabeaaaaaklkkkkdocpaaaaaf
ecaabaaaaaaaaaaackaabaaaaaaaaaaadiaaaaaiecaabaaaaaaaaaaackaabaaa
aaaaaaaackiacaaaaaaaaaaaagaaaaaabjaaaaafecaabaaaaaaaaaaackaabaaa
aaaaaaaadcaaaaakhcaabaaaaaaaaaaakgakbaaaaaaaaaaapgipcaaaaaaaaaaa
agaaaaaaegadbaaaaaaaaaaabaaaaaahicaabaaaaaaaaaaaegbcbaaaabaaaaaa
egbcbaaaabaaaaaaeeaaaaaficaabaaaaaaaaaaadkaabaaaaaaaaaaadcaaaaak
icaabaaaaaaaaaaackbabaiaebaaaaaaabaaaaaadkaabaaaaaaaaaaaabeaaaaa
aaaaiadpcpaaaaaficaabaaaaaaaaaaadkaabaaaaaaaaaaadiaaaaaiicaabaaa
aaaaaaaadkaabaaaaaaaaaaaakiacaaaaaaaaaaaagaaaaaabjaaaaaficaabaaa
aaaaaaaadkaabaaaaaaaaaaaaaaaaaakhcaabaaaabaaaaaaegiccaaaaaaaaaaa
aeaaaaaaegiccaiaebaaaaaaaaaaaaaaafaaaaaadcaaaaakhcaabaaaabaaaaaa
pgapbaaaaaaaaaaaegacbaaaabaaaaaaegiccaaaaaaaaaaaafaaaaaaaaaaaaah
hcaabaaaaaaaaaaaegacbaaaaaaaaaaaegacbaaaabaaaaaaefaaaaajpcaabaaa
abaaaaaaegbabaaaafaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaadiaaaaah
icaabaaaaaaaaaaadkaabaaaabaaaaaaabeaaaaaaaaaaaebdiaaaaahhcaabaaa
abaaaaaaegacbaaaabaaaaaapgapbaaaaaaaaaaadiaaaaahhccabaaaaaaaaaaa
egacbaaaaaaaaaaaegacbaaaabaaaaaadgaaaaaficcabaaaaaaaaaaaabeaaaaa
aaaaiadpdoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_OFF" }
"!!GLES3"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Vector 0 [_LightColor0]
Vector 1 [_ReflectColor]
Vector 2 [_FresnelColor]
Vector 3 [_Color]
Float 4 [_TransmissiveColor]
Float 5 [_Shininess]
Float 6 [_HdrPower]
Float 7 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
SetTexture 1 [_ShadowMapTexture] 2D
"3.0-!!ARBfp1.0
# 40 ALU, 2 TEX
PARAM c[10] = { program.local[0..7],
		{ 2, 0.33333334, 1, 0.5 },
		{ 10, 0 } };
TEMP R0;
TEMP R1;
TEMP R2;
MOV R1.x, fragment.texcoord[1].z;
MOV R1.y, fragment.texcoord[2].z;
MOV R1.z, fragment.texcoord[3];
MOV R0.x, fragment.texcoord[1].w;
MOV R0.z, fragment.texcoord[3].w;
MOV R0.y, fragment.texcoord[2].w;
DP3 R0.w, R1, R0;
MUL R1.xyz, R1, R0.w;
MAD R0.xyz, -R1, c[8].x, R0;
DP3 R1.x, fragment.texcoord[0], fragment.texcoord[0];
RSQ R1.w, R1.x;
TEX R0.xyz, R0, texture[0], CUBE;
MUL R0.xyz, R0, c[1];
ADD R0.w, R0.x, R0.y;
ADD R0.w, R0, R0.z;
MUL R0.w, R0, c[8].y;
POW R0.w, R0.w, c[6].x;
MOV R1.xyz, c[3];
MAD R0.xyz, R0.w, c[7].x, R0;
MAD R2.x, -R1.w, fragment.texcoord[0].z, c[8].z;
POW R0.w, R2.x, c[4].x;
ADD R1.xyz, -R1, c[2];
MAD R1.xyz, R0.w, R1, c[3];
ADD R0.yzw, R0.xxyz, R1.xxyz;
MOV R2.y, c[9].x;
MAD R0.x, R1.w, fragment.texcoord[0].z, c[8].z;
ADD R1.x, R2.y, -c[5];
POW R1.x, R0.x, R1.x;
MUL R1.w, R2.x, R1.x;
MAD R0.x, fragment.texcoord[4].z, c[8].w, c[8].w;
MUL R1.xyz, R0.yzww, R0.x;
MAX R0.x, R1.w, c[9].y;
MUL R2.xyz, R1, c[0];
MUL R1.xyz, R0.x, c[0];
TXP R0.x, fragment.texcoord[6], texture[1], 2D;
MUL R1.xyz, R0.x, R1;
MUL R2.xyz, R2, R0.x;
MAD R1.xyz, R2, c[8].x, R1;
MAD result.color.xyz, R0.yzww, fragment.texcoord[5], R1;
MOV result.color.w, c[8].z;
END
# 40 instructions, 3 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Vector 0 [_LightColor0]
Vector 1 [_ReflectColor]
Vector 2 [_FresnelColor]
Vector 3 [_Color]
Float 4 [_TransmissiveColor]
Float 5 [_Shininess]
Float 6 [_HdrPower]
Float 7 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
SetTexture 1 [_ShadowMapTexture] 2D
"ps_3_0
; 46 ALU, 2 TEX
dcl_cube s0
dcl_2d s1
def c8, 2.00000000, 0.33333334, 1.00000000, 0.50000000
def c9, 10.00000000, 0.00000000, 0, 0
dcl_texcoord0 v0.xyz
dcl_texcoord1 v1.xyzw
dcl_texcoord2 v2.xyzw
dcl_texcoord3 v3.xyzw
dcl_texcoord4 v4.xyz
dcl_texcoord5 v5.xyz
dcl_texcoord6 v6
mov_pp r3.xyz, c2
mov_pp r1.x, v1.z
mov_pp r1.y, v2.z
mov_pp r1.z, v3
mov r0.x, v1.w
mov r0.z, v3.w
mov r0.y, v2.w
dp3 r0.w, r1, r0
mul r1.xyz, r1, r0.w
mad r0.xyz, -r1, c8.x, r0
texld r0.xyz, r0, s0
mul r1.xyz, r0, c1
add r0.x, r1, r1.y
add r0.x, r0, r1.z
mul r1.w, r0.x, c8.y
pow r0, r1.w, c6.x
dp3_pp r0.y, v0, v0
mad r1.xyz, r0.x, c7.x, r1
rsq_pp r0.x, r0.y
mov_pp r0.y, c5.x
mad_pp r1.w, -r0.x, v0.z, c8.z
add_pp r2.y, c9.x, -r0
mad_pp r2.x, r0, v0.z, c8.z
pow_pp r0, r2.x, r2.y
pow_pp r2, r1.w, c4.x
mov_pp r0.y, r2.x
add_pp r3.xyz, -c3, r3
mad_pp r2.xyz, r0.y, r3, c3
mov_pp r0.y, r0.x
mul_pp r0.w, r1, r0.y
add r1.xyz, r1, r2
mad_pp r0.x, v4.z, c8.w, c8.w
mul_pp r0.xyz, r1, r0.x
max_pp r0.w, r0, c9.y
texldp r3.x, v6, s1
mul_pp r0.xyz, r0, c0
mul_pp r2.xyz, r0.w, c0
mul_pp r2.xyz, r3.x, r2
mul_pp r0.xyz, r0, r3.x
mad_pp r0.xyz, r0, c8.x, r2
mad_pp oC0.xyz, r1, v5, r0
mov_pp oC0.w, c8.z
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
ConstBuffer "$Globals" 176 // 176 used size, 11 vars
Vector 16 [_LightColor0] 4
Vector 112 [_ReflectColor] 4
Vector 128 [_FresnelColor] 4
Vector 144 [_Color] 4
Float 160 [_TransmissiveColor]
Float 164 [_Shininess]
Float 168 [_HdrPower]
Float 172 [_HdrContrast]
BindCB "$Globals" 0
SetTexture 0 [_Cube] CUBE 1
SetTexture 1 [_ShadowMapTexture] 2D 0
// 46 instructions, 3 temp regs, 0 temp arrays:
// ALU 36 float, 0 int, 0 uint
// TEX 2 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0
eefiecedpdfbefdbcgagploekcodbcajomdcbcbaabaaaaaabmahaaaaadaaaaaa
cmaaaaaabeabaaaaeiabaaaaejfdeheooaaaaaaaaiaaaaaaaiaaaaaamiaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaaneaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahahaaaaneaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apamaaaaneaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaaapamaaaaneaaaaaa
adaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapamaaaaneaaaaaaaeaaaaaaaaaaaaaa
adaaaaaaafaaaaaaahaeaaaaneaaaaaaafaaaaaaaaaaaaaaadaaaaaaagaaaaaa
ahahaaaaneaaaaaaagaaaaaaaaaaaaaaadaaaaaaahaaaaaaapalaaaafdfgfpfa
epfdejfeejepeoaafeeffiedepepfceeaaklklklepfdeheocmaaaaaaabaaaaaa
aiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapaaaaaafdfgfpfe
gbhcghgfheaaklklfdeieefcmmafaaaaeaaaaaaahdabaaaafjaaaaaeegiocaaa
aaaaaaaaalaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaadaagabaaaabaaaaaa
fidaaaaeaahabaaaaaaaaaaaffffaaaafibiaaaeaahabaaaabaaaaaaffffaaaa
gcbaaaadhcbabaaaabaaaaaagcbaaaadmcbabaaaacaaaaaagcbaaaadmcbabaaa
adaaaaaagcbaaaadmcbabaaaaeaaaaaagcbaaaadecbabaaaafaaaaaagcbaaaad
hcbabaaaagaaaaaagcbaaaadlcbabaaaahaaaaaagfaaaaadpccabaaaaaaaaaaa
giaaaaacadaaaaaadgaaaaafbcaabaaaaaaaaaaadkbabaaaacaaaaaadgaaaaaf
ccaabaaaaaaaaaaadkbabaaaadaaaaaadgaaaaafecaabaaaaaaaaaaadkbabaaa
aeaaaaaadgaaaaafbcaabaaaabaaaaaackbabaaaacaaaaaadgaaaaafccaabaaa
abaaaaaackbabaaaadaaaaaadgaaaaafecaabaaaabaaaaaackbabaaaaeaaaaaa
baaaaaahicaabaaaaaaaaaaaegacbaaaaaaaaaaaegacbaaaabaaaaaaaaaaaaah
icaabaaaaaaaaaaadkaabaaaaaaaaaaadkaabaaaaaaaaaaadcaaaaakhcaabaaa
aaaaaaaaegacbaaaabaaaaaapgapbaiaebaaaaaaaaaaaaaaegacbaaaaaaaaaaa
efaaaaajpcaabaaaaaaaaaaaegacbaaaaaaaaaaaeghobaaaaaaaaaaaaagabaaa
abaaaaaadiaaaaailcaabaaaaaaaaaaaegaibaaaaaaaaaaaegiicaaaaaaaaaaa
ahaaaaaaaaaaaaahbcaabaaaabaaaaaabkaabaaaaaaaaaaaakaabaaaaaaaaaaa
dcaaaaakecaabaaaaaaaaaaackaabaaaaaaaaaaackiacaaaaaaaaaaaahaaaaaa
akaabaaaabaaaaaadiaaaaahecaabaaaaaaaaaaackaabaaaaaaaaaaaabeaaaaa
klkkkkdocpaaaaafecaabaaaaaaaaaaackaabaaaaaaaaaaadiaaaaaiecaabaaa
aaaaaaaackaabaaaaaaaaaaackiacaaaaaaaaaaaakaaaaaabjaaaaafecaabaaa
aaaaaaaackaabaaaaaaaaaaadcaaaaakhcaabaaaaaaaaaaakgakbaaaaaaaaaaa
pgipcaaaaaaaaaaaakaaaaaaegadbaaaaaaaaaaabaaaaaahicaabaaaaaaaaaaa
egbcbaaaabaaaaaaegbcbaaaabaaaaaaeeaaaaaficaabaaaaaaaaaaadkaabaaa
aaaaaaaadcaaaaakbcaabaaaabaaaaaackbabaiaebaaaaaaabaaaaaadkaabaaa
aaaaaaaaabeaaaaaaaaaiadpdcaaaaajicaabaaaaaaaaaaackbabaaaabaaaaaa
dkaabaaaaaaaaaaaabeaaaaaaaaaiadpcpaaaaaficaabaaaaaaaaaaadkaabaaa
aaaaaaaacpaaaaafccaabaaaabaaaaaaakaabaaaabaaaaaadiaaaaaiccaabaaa
abaaaaaabkaabaaaabaaaaaaakiacaaaaaaaaaaaakaaaaaabjaaaaafccaabaaa
abaaaaaabkaabaaaabaaaaaaaaaaaaakhcaabaaaacaaaaaaegiccaaaaaaaaaaa
aiaaaaaaegiccaiaebaaaaaaaaaaaaaaajaaaaaadcaaaaakocaabaaaabaaaaaa
fgafbaaaabaaaaaaagajbaaaacaaaaaaagijcaaaaaaaaaaaajaaaaaaaaaaaaah
hcaabaaaaaaaaaaaegacbaaaaaaaaaaajgahbaaaabaaaaaadcaaaaajccaabaaa
abaaaaaackbabaaaafaaaaaaabeaaaaaaaaaaadpabeaaaaaaaaaaadpdiaaaaah
ocaabaaaabaaaaaaagajbaaaaaaaaaaafgafbaaaabaaaaaadiaaaaaiocaabaaa
abaaaaaafgaobaaaabaaaaaaagijcaaaaaaaaaaaabaaaaaaaoaaaaahdcaabaaa
acaaaaaaegbabaaaahaaaaaapgbpbaaaahaaaaaaefaaaaajpcaabaaaacaaaaaa
egaabaaaacaaaaaaeghobaaaabaaaaaaaagabaaaaaaaaaaadiaaaaahocaabaaa
abaaaaaafgaobaaaabaaaaaaagaabaaaacaaaaaaaaaaaaahocaabaaaabaaaaaa
fgaobaaaabaaaaaafgaobaaaabaaaaaaaaaaaaajccaabaaaacaaaaaabkiacaia
ebaaaaaaaaaaaaaaakaaaaaaabeaaaaaaaaacaebdiaaaaahicaabaaaaaaaaaaa
dkaabaaaaaaaaaaabkaabaaaacaaaaaabjaaaaaficaabaaaaaaaaaaadkaabaaa
aaaaaaaadiaaaaahicaabaaaaaaaaaaaakaabaaaabaaaaaadkaabaaaaaaaaaaa
deaaaaahicaabaaaaaaaaaaadkaabaaaaaaaaaaaabeaaaaaaaaaaaaadiaaaaai
ocaabaaaacaaaaaapgapbaaaaaaaaaaaagijcaaaaaaaaaaaabaaaaaadcaaaaaj
hcaabaaaabaaaaaajgahbaaaacaaaaaaagaabaaaacaaaaaajgahbaaaabaaaaaa
dcaaaaajhccabaaaaaaaaaaaegacbaaaaaaaaaaaegbcbaaaagaaaaaaegacbaaa
abaaaaaadgaaaaaficcabaaaaaaaaaaaabeaaaaaaaaaiadpdoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES3"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Vector 0 [_ReflectColor]
Vector 1 [_FresnelColor]
Vector 2 [_Color]
Float 3 [_TransmissiveColor]
Float 4 [_HdrPower]
Float 5 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
SetTexture 1 [_ShadowMapTexture] 2D
SetTexture 2 [unity_Lightmap] 2D
"3.0-!!ARBfp1.0
# 35 ALU, 3 TEX
PARAM c[7] = { program.local[0..5],
		{ 0.33333334, 1, 2, 8 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
DP3 R1.w, fragment.texcoord[0], fragment.texcoord[0];
RSQ R1.w, R1.w;
MAD R1.w, -R1, fragment.texcoord[0].z, c[6].y;
MOV R1.x, fragment.texcoord[1].z;
MOV R1.y, fragment.texcoord[2].z;
MOV R1.z, fragment.texcoord[3];
MOV R0.x, fragment.texcoord[1].w;
MOV R0.z, fragment.texcoord[3].w;
MOV R0.y, fragment.texcoord[2].w;
DP3 R0.w, R1, R0;
MUL R1.xyz, R1, R0.w;
MAD R0.xyz, -R1, c[6].z, R0;
TEX R2, fragment.texcoord[4], texture[2], 2D;
MUL R1.xyz, R2.w, R2;
TXP R3.x, fragment.texcoord[5], texture[1], 2D;
MUL R2.xyz, R2, R3.x;
MUL R1.xyz, R1, c[6].w;
TEX R0.xyz, R0, texture[0], CUBE;
MUL R0.xyz, R0, c[0];
ADD R0.w, R0.x, R0.y;
ADD R0.w, R0, R0.z;
MUL R0.w, R0, c[6].x;
POW R0.w, R0.w, c[4].x;
MUL R2.xyz, R2, c[6].z;
MUL R3.xyz, R1, R3.x;
MIN R1.xyz, R1, R2;
MOV R2.xyz, c[2];
MAX R1.xyz, R1, R3;
ADD R2.xyz, -R2, c[1];
POW R1.w, R1.w, c[3].x;
MAD R2.xyz, R1.w, R2, c[2];
MAD R0.xyz, R0.w, c[5].x, R0;
ADD R0.xyz, R0, R2;
MUL result.color.xyz, R0, R1;
MOV result.color.w, c[6].y;
END
# 35 instructions, 4 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
Vector 0 [_ReflectColor]
Vector 1 [_FresnelColor]
Vector 2 [_Color]
Float 3 [_TransmissiveColor]
Float 4 [_HdrPower]
Float 5 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
SetTexture 1 [_ShadowMapTexture] 2D
SetTexture 2 [unity_Lightmap] 2D
"ps_3_0
; 37 ALU, 3 TEX
dcl_cube s0
dcl_2d s1
dcl_2d s2
def c6, 2.00000000, 0.33333334, 1.00000000, 8.00000000
dcl_texcoord0 v0.xyz
dcl_texcoord1 v1.xyzw
dcl_texcoord2 v2.xyzw
dcl_texcoord3 v3.xyzw
dcl_texcoord4 v4.xy
dcl_texcoord5 v5
texld r2, v4, s2
mov_pp r1.x, v1.z
mov_pp r1.y, v2.z
mov_pp r1.z, v3
mov r0.x, v1.w
mov r0.z, v3.w
mov r0.y, v2.w
dp3 r0.w, r1, r0
mul r1.xyz, r1, r0.w
mad r0.xyz, -r1, c6.x, r0
texld r0.xyz, r0, s0
mul r1.xyz, r0, c0
add r0.x, r1, r1.y
add r0.w, r0.x, r1.z
mul_pp r0.xyz, r2.w, r2
texldp r3.x, v5, s1
mul_pp r2.xyz, r2, r3.x
mul_pp r0.xyz, r0, c6.w
mul_pp r3.xyz, r0, r3.x
mul_pp r2.xyz, r2, c6.x
min_pp r0.xyz, r0, r2
max_pp r2.xyz, r0, r3
mul r1.w, r0, c6.y
mov_pp r3.xyz, c1
pow r0, r1.w, c4.x
dp3_pp r2.w, v0, v0
rsq_pp r0.y, r2.w
mov r1.w, r0.x
mad_pp r2.w, -r0.y, v0.z, c6.z
pow_pp r0, r2.w, c3.x
add_pp r3.xyz, -c2, r3
mad_pp r0.xyz, r0.x, r3, c2
mad r1.xyz, r1.w, c5.x, r1
add r0.xyz, r1, r0
mul_pp oC0.xyz, r0, r2
mov_pp oC0.w, c6.z
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
ConstBuffer "$Globals" 192 // 176 used size, 12 vars
Vector 112 [_ReflectColor] 4
Vector 128 [_FresnelColor] 4
Vector 144 [_Color] 4
Float 160 [_TransmissiveColor]
Float 168 [_HdrPower]
Float 172 [_HdrContrast]
BindCB "$Globals" 0
SetTexture 0 [_Cube] CUBE 1
SetTexture 1 [_ShadowMapTexture] 2D 0
SetTexture 2 [unity_Lightmap] 2D 2
// 40 instructions, 3 temp regs, 0 temp arrays:
// ALU 29 float, 0 int, 0 uint
// TEX 3 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0
eefiecedanbehodjepjacbihgcifahnpffdijndcabaaaaaafeagaaaaadaaaaaa
cmaaaaaapmaaaaaadaabaaaaejfdeheomiaaaaaaahaaaaaaaiaaaaaalaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaalmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahahaaaalmaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apamaaaalmaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaaapamaaaalmaaaaaa
adaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapamaaaalmaaaaaaaeaaaaaaaaaaaaaa
adaaaaaaafaaaaaaadadaaaalmaaaaaaafaaaaaaaaaaaaaaadaaaaaaagaaaaaa
apalaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcbmafaaaaeaaaaaaaehabaaaa
fjaaaaaeegiocaaaaaaaaaaaalaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaad
aagabaaaabaaaaaafkaaaaadaagabaaaacaaaaaafidaaaaeaahabaaaaaaaaaaa
ffffaaaafibiaaaeaahabaaaabaaaaaaffffaaaafibiaaaeaahabaaaacaaaaaa
ffffaaaagcbaaaadhcbabaaaabaaaaaagcbaaaadmcbabaaaacaaaaaagcbaaaad
mcbabaaaadaaaaaagcbaaaadmcbabaaaaeaaaaaagcbaaaaddcbabaaaafaaaaaa
gcbaaaadlcbabaaaagaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacadaaaaaa
dgaaaaafbcaabaaaaaaaaaaadkbabaaaacaaaaaadgaaaaafccaabaaaaaaaaaaa
dkbabaaaadaaaaaadgaaaaafecaabaaaaaaaaaaadkbabaaaaeaaaaaadgaaaaaf
bcaabaaaabaaaaaackbabaaaacaaaaaadgaaaaafccaabaaaabaaaaaackbabaaa
adaaaaaadgaaaaafecaabaaaabaaaaaackbabaaaaeaaaaaabaaaaaahicaabaaa
aaaaaaaaegacbaaaaaaaaaaaegacbaaaabaaaaaaaaaaaaahicaabaaaaaaaaaaa
dkaabaaaaaaaaaaadkaabaaaaaaaaaaadcaaaaakhcaabaaaaaaaaaaaegacbaaa
abaaaaaapgapbaiaebaaaaaaaaaaaaaaegacbaaaaaaaaaaaefaaaaajpcaabaaa
aaaaaaaaegacbaaaaaaaaaaaeghobaaaaaaaaaaaaagabaaaabaaaaaadiaaaaai
lcaabaaaaaaaaaaaegaibaaaaaaaaaaaegiicaaaaaaaaaaaahaaaaaaaaaaaaah
bcaabaaaabaaaaaabkaabaaaaaaaaaaaakaabaaaaaaaaaaadcaaaaakecaabaaa
aaaaaaaackaabaaaaaaaaaaackiacaaaaaaaaaaaahaaaaaaakaabaaaabaaaaaa
diaaaaahecaabaaaaaaaaaaackaabaaaaaaaaaaaabeaaaaaklkkkkdocpaaaaaf
ecaabaaaaaaaaaaackaabaaaaaaaaaaadiaaaaaiecaabaaaaaaaaaaackaabaaa
aaaaaaaackiacaaaaaaaaaaaakaaaaaabjaaaaafecaabaaaaaaaaaaackaabaaa
aaaaaaaadcaaaaakhcaabaaaaaaaaaaakgakbaaaaaaaaaaapgipcaaaaaaaaaaa
akaaaaaaegadbaaaaaaaaaaabaaaaaahicaabaaaaaaaaaaaegbcbaaaabaaaaaa
egbcbaaaabaaaaaaeeaaaaaficaabaaaaaaaaaaadkaabaaaaaaaaaaadcaaaaak
icaabaaaaaaaaaaackbabaiaebaaaaaaabaaaaaadkaabaaaaaaaaaaaabeaaaaa
aaaaiadpcpaaaaaficaabaaaaaaaaaaadkaabaaaaaaaaaaadiaaaaaiicaabaaa
aaaaaaaadkaabaaaaaaaaaaaakiacaaaaaaaaaaaakaaaaaabjaaaaaficaabaaa
aaaaaaaadkaabaaaaaaaaaaaaaaaaaakhcaabaaaabaaaaaaegiccaaaaaaaaaaa
aiaaaaaaegiccaiaebaaaaaaaaaaaaaaajaaaaaadcaaaaakhcaabaaaabaaaaaa
pgapbaaaaaaaaaaaegacbaaaabaaaaaaegiccaaaaaaaaaaaajaaaaaaaaaaaaah
hcaabaaaaaaaaaaaegacbaaaaaaaaaaaegacbaaaabaaaaaaaoaaaaahdcaabaaa
abaaaaaaegbabaaaagaaaaaapgbpbaaaagaaaaaaefaaaaajpcaabaaaabaaaaaa
egaabaaaabaaaaaaeghobaaaabaaaaaaaagabaaaaaaaaaaaaaaaaaahicaabaaa
aaaaaaaaakaabaaaabaaaaaaakaabaaaabaaaaaaefaaaaajpcaabaaaacaaaaaa
egbabaaaafaaaaaaeghobaaaacaaaaaaaagabaaaacaaaaaadiaaaaahocaabaaa
abaaaaaapgapbaaaaaaaaaaaagajbaaaacaaaaaadiaaaaahicaabaaaaaaaaaaa
dkaabaaaacaaaaaaabeaaaaaaaaaaaebdiaaaaahhcaabaaaacaaaaaaegacbaaa
acaaaaaapgapbaaaaaaaaaaaddaaaaahocaabaaaabaaaaaafgaobaaaabaaaaaa
agajbaaaacaaaaaadiaaaaahhcaabaaaacaaaaaaagaabaaaabaaaaaaegacbaaa
acaaaaaadeaaaaahhcaabaaaabaaaaaajgahbaaaabaaaaaaegacbaaaacaaaaaa
diaaaaahhccabaaaaaaaaaaaegacbaaaaaaaaaaaegacbaaaabaaaaaadgaaaaaf
iccabaaaaaaaaaaaabeaaaaaaaaaiadpdoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "SHADOWS_SCREEN" }
"!!GLES3"
}

}
	}
	Pass {
		Name "FORWARD"
		Tags { "LightMode" = "ForwardAdd" }
		ZWrite Off Blend One One Fog { Color (0,0,0,0) }
Program "vp" {
// Vertex combos: 5
//   opengl - ALU: 39 to 48
//   d3d9 - ALU: 42 to 51
//   d3d11 - ALU: 37 to 46, TEX: 0 to 0, FLOW: 1 to 1
SubProgram "opengl " {
Keywords { "POINT" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Vector 17 [_WorldSpaceCameraPos]
Vector 18 [_WorldSpaceLightPos0]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 19 [unity_Scale]
Matrix 13 [_LightMatrix0]
"3.0-!!ARBvp1.0
# 47 ALU
PARAM c[20] = { { 1 },
		state.matrix.mvp,
		program.local[5..19] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R0.xyz, vertex.attrib[14];
MUL R2.xyz, vertex.normal.zxyw, R0.yzxw;
MOV R1, c[18];
MAD R0.xyz, vertex.normal.yzxw, R0.zxyw, -R2;
DP4 R2.z, R1, c[11];
DP4 R2.x, R1, c[9];
DP4 R2.y, R1, c[10];
MAD R3.xyz, R2, c[19].w, -vertex.position;
MUL R2.xyz, R0, vertex.attrib[14].w;
MOV R0.xyz, c[17];
MOV R0.w, c[0].x;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
MAD R1.xyz, R1, c[19].w, -vertex.position;
DP3 R0.y, R2, c[5];
DP3 R0.w, -R1, c[5];
DP3 R0.x, vertex.attrib[14], c[5];
DP3 R0.z, vertex.normal, c[5];
MUL result.texcoord[1], R0, c[19].w;
DP3 R0.y, R2, c[6];
DP3 R0.w, -R1, c[6];
DP3 R0.x, vertex.attrib[14], c[6];
DP3 R0.z, vertex.normal, c[6];
MUL result.texcoord[2], R0, c[19].w;
DP3 R0.y, R2, c[7];
DP3 R0.w, -R1, c[7];
DP3 R0.x, vertex.attrib[14], c[7];
DP3 R0.z, vertex.normal, c[7];
MUL result.texcoord[3], R0, c[19].w;
DP4 R0.w, vertex.position, c[8];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP3 result.texcoord[0].y, R1, R2;
DP3 result.texcoord[4].y, R2, R3;
DP3 result.texcoord[0].z, vertex.normal, R1;
DP3 result.texcoord[0].x, R1, vertex.attrib[14];
DP3 result.texcoord[4].z, vertex.normal, R3;
DP3 result.texcoord[4].x, vertex.attrib[14], R3;
DP4 result.texcoord[5].z, R0, c[15];
DP4 result.texcoord[5].y, R0, c[14];
DP4 result.texcoord[5].x, R0, c[13];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 47 instructions, 4 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "POINT" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Matrix 0 [glstate_matrix_mvp]
Vector 16 [_WorldSpaceCameraPos]
Vector 17 [_WorldSpaceLightPos0]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 18 [unity_Scale]
Matrix 12 [_LightMatrix0]
"vs_3_0
; 50 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
def c19, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
mov r0.xyz, v1
mul r2.xyz, v2.zxyw, r0.yzxw
mov r1, c10
dp4 r3.z, c17, r1
mov r1, c9
mov r0.xyz, v1
mad r0.xyz, v2.yzxw, r0.zxyw, -r2
mov r2, c8
dp4 r3.x, c17, r2
mul r2.xyz, r0, v1.w
dp4 r3.y, c17, r1
mad r3.xyz, r3, c18.w, -v0
mov r0.xyz, c16
mov r0.w, c19.x
dp4 r1.z, r0, c10
dp4 r1.x, r0, c8
dp4 r1.y, r0, c9
mad r1.xyz, r1, c18.w, -v0
dp3 r0.y, r2, c4
dp3 r0.w, -r1, c4
dp3 r0.x, v1, c4
dp3 r0.z, v2, c4
mul o2, r0, c18.w
dp3 r0.y, r2, c5
dp3 r0.w, -r1, c5
dp3 r0.x, v1, c5
dp3 r0.z, v2, c5
mul o3, r0, c18.w
dp3 r0.y, r2, c6
dp3 r0.w, -r1, c6
dp3 r0.x, v1, c6
dp3 r0.z, v2, c6
mul o4, r0, c18.w
dp4 r0.w, v0, c7
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
dp3 o1.y, r1, r2
dp3 o5.y, r2, r3
dp3 o1.z, v2, r1
dp3 o1.x, r1, v1
dp3 o5.z, v2, r3
dp3 o5.x, v1, r3
dp4 o6.z, r0, c14
dp4 o6.y, r0, c13
dp4 o6.x, r0, c12
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "d3d11 " {
Keywords { "POINT" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "color" Color
ConstBuffer "$Globals" 176 // 112 used size, 11 vars
Matrix 48 [_LightMatrix0] 4
ConstBuffer "UnityPerCamera" 128 // 76 used size, 8 vars
Vector 64 [_WorldSpaceCameraPos] 3
ConstBuffer "UnityLighting" 720 // 16 used size, 17 vars
Vector 0 [_WorldSpaceLightPos0] 4
ConstBuffer "UnityPerDraw" 336 // 336 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
Matrix 192 [_Object2World] 4
Matrix 256 [_World2Object] 4
Vector 320 [unity_Scale] 4
BindCB "$Globals" 0
BindCB "UnityPerCamera" 1
BindCB "UnityLighting" 2
BindCB "UnityPerDraw" 3
// 58 instructions, 4 temp regs, 0 temp arrays:
// ALU 46 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedonannpmdciocmbbbknokjjjhihhgbcfkabaaaaaaoaajaaaaadaaaaaa
cmaaaaaapeaaaaaameabaaaaejfdeheomaaaaaaaagaaaaaaaiaaaaaajiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaakbaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaakjaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
ahahaaaalaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaadaaaaaaapaaaaaalaaaaaaa
abaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaaljaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaafaaaaaaapaaaaaafaepfdejfeejepeoaafeebeoehefeofeaaeoepfc
enebemaafeeffiedepepfceeaaedepemepfcaaklepfdeheomiaaaaaaahaaaaaa
aiaaaaaalaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaalmaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaahaiaaaalmaaaaaaabaaaaaaaaaaaaaa
adaaaaaaacaaaaaaapaaaaaalmaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaa
apaaaaaalmaaaaaaadaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaalmaaaaaa
aeaaaaaaaaaaaaaaadaaaaaaafaaaaaaahaiaaaalmaaaaaaafaaaaaaaaaaaaaa
adaaaaaaagaaaaaaahaiaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklklfdeieefcbeaiaaaaeaaaabaaafacaaaafjaaaaaeegiocaaaaaaaaaaa
ahaaaaaafjaaaaaeegiocaaaabaaaaaaafaaaaaafjaaaaaeegiocaaaacaaaaaa
abaaaaaafjaaaaaeegiocaaaadaaaaaabfaaaaaafpaaaaadpcbabaaaaaaaaaaa
fpaaaaadpcbabaaaabaaaaaafpaaaaadhcbabaaaacaaaaaaghaaaaaepccabaaa
aaaaaaaaabaaaaaagfaaaaadhccabaaaabaaaaaagfaaaaadpccabaaaacaaaaaa
gfaaaaadpccabaaaadaaaaaagfaaaaadpccabaaaaeaaaaaagfaaaaadhccabaaa
afaaaaaagfaaaaadhccabaaaagaaaaaagiaaaaacaeaaaaaadiaaaaaipcaabaaa
aaaaaaaafgbfbaaaaaaaaaaaegiocaaaadaaaaaaabaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaadaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpcaabaaaaaaaaaaaegiocaaaadaaaaaaacaaaaaakgbkbaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaaegiocaaaadaaaaaaadaaaaaa
pgbpbaaaaaaaaaaaegaobaaaaaaaaaaadiaaaaajhcaabaaaaaaaaaaafgifcaaa
abaaaaaaaeaaaaaaegiccaaaadaaaaaabbaaaaaadcaaaaalhcaabaaaaaaaaaaa
egiccaaaadaaaaaabaaaaaaaagiacaaaabaaaaaaaeaaaaaaegacbaaaaaaaaaaa
dcaaaaalhcaabaaaaaaaaaaaegiccaaaadaaaaaabcaaaaaakgikcaaaabaaaaaa
aeaaaaaaegacbaaaaaaaaaaaaaaaaaaihcaabaaaaaaaaaaaegacbaaaaaaaaaaa
egiccaaaadaaaaaabdaaaaaadcaaaaalhcaabaaaaaaaaaaaegacbaaaaaaaaaaa
pgipcaaaadaaaaaabeaaaaaaegbcbaiaebaaaaaaaaaaaaaabaaaaaahbccabaaa
abaaaaaaegbcbaaaabaaaaaaegacbaaaaaaaaaaabaaaaaaheccabaaaabaaaaaa
egbcbaaaacaaaaaaegacbaaaaaaaaaaadiaaaaahhcaabaaaabaaaaaajgbebaaa
abaaaaaacgbjbaaaacaaaaaadcaaaaakhcaabaaaabaaaaaajgbebaaaacaaaaaa
cgbjbaaaabaaaaaaegacbaiaebaaaaaaabaaaaaadiaaaaahhcaabaaaabaaaaaa
egacbaaaabaaaaaapgbpbaaaabaaaaaabaaaaaahcccabaaaabaaaaaaegacbaaa
abaaaaaaegacbaaaaaaaaaaadiaaaaajhcaabaaaacaaaaaafgafbaiaebaaaaaa
aaaaaaaaegiccaaaadaaaaaaanaaaaaadcaaaaallcaabaaaaaaaaaaaegiicaaa
adaaaaaaamaaaaaaagaabaiaebaaaaaaaaaaaaaaegaibaaaacaaaaaadcaaaaal
lcaabaaaaaaaaaaaegiicaaaadaaaaaaaoaaaaaakgakbaiaebaaaaaaaaaaaaaa
egambaaaaaaaaaaadgaaaaaficaabaaaacaaaaaaakaabaaaaaaaaaaadgaaaaag
bcaabaaaadaaaaaaakiacaaaadaaaaaaamaaaaaadgaaaaagccaabaaaadaaaaaa
akiacaaaadaaaaaaanaaaaaadgaaaaagecaabaaaadaaaaaaakiacaaaadaaaaaa
aoaaaaaabaaaaaahccaabaaaacaaaaaaegacbaaaabaaaaaaegacbaaaadaaaaaa
baaaaaahbcaabaaaacaaaaaaegbcbaaaabaaaaaaegacbaaaadaaaaaabaaaaaah
ecaabaaaacaaaaaaegbcbaaaacaaaaaaegacbaaaadaaaaaadiaaaaaipccabaaa
acaaaaaaegaobaaaacaaaaaapgipcaaaadaaaaaabeaaaaaadgaaaaaficaabaaa
acaaaaaabkaabaaaaaaaaaaadgaaaaagbcaabaaaadaaaaaabkiacaaaadaaaaaa
amaaaaaadgaaaaagccaabaaaadaaaaaabkiacaaaadaaaaaaanaaaaaadgaaaaag
ecaabaaaadaaaaaabkiacaaaadaaaaaaaoaaaaaabaaaaaahccaabaaaacaaaaaa
egacbaaaabaaaaaaegacbaaaadaaaaaabaaaaaahbcaabaaaacaaaaaaegbcbaaa
abaaaaaaegacbaaaadaaaaaabaaaaaahecaabaaaacaaaaaaegbcbaaaacaaaaaa
egacbaaaadaaaaaadiaaaaaipccabaaaadaaaaaaegaobaaaacaaaaaapgipcaaa
adaaaaaabeaaaaaadgaaaaagbcaabaaaacaaaaaackiacaaaadaaaaaaamaaaaaa
dgaaaaagccaabaaaacaaaaaackiacaaaadaaaaaaanaaaaaadgaaaaagecaabaaa
acaaaaaackiacaaaadaaaaaaaoaaaaaabaaaaaahccaabaaaaaaaaaaaegacbaaa
abaaaaaaegacbaaaacaaaaaabaaaaaahbcaabaaaaaaaaaaaegbcbaaaabaaaaaa
egacbaaaacaaaaaabaaaaaahecaabaaaaaaaaaaaegbcbaaaacaaaaaaegacbaaa
acaaaaaadiaaaaaipccabaaaaeaaaaaaegaobaaaaaaaaaaapgipcaaaadaaaaaa
beaaaaaadiaaaaajhcaabaaaaaaaaaaafgifcaaaacaaaaaaaaaaaaaaegiccaaa
adaaaaaabbaaaaaadcaaaaalhcaabaaaaaaaaaaaegiccaaaadaaaaaabaaaaaaa
agiacaaaacaaaaaaaaaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaaaaaaaaa
egiccaaaadaaaaaabcaaaaaakgikcaaaacaaaaaaaaaaaaaaegacbaaaaaaaaaaa
dcaaaaalhcaabaaaaaaaaaaaegiccaaaadaaaaaabdaaaaaapgipcaaaacaaaaaa
aaaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaaaaaaaaaegacbaaaaaaaaaaa
pgipcaaaadaaaaaabeaaaaaaegbcbaiaebaaaaaaaaaaaaaabaaaaaahcccabaaa
afaaaaaaegacbaaaabaaaaaaegacbaaaaaaaaaaabaaaaaahbccabaaaafaaaaaa
egbcbaaaabaaaaaaegacbaaaaaaaaaaabaaaaaaheccabaaaafaaaaaaegbcbaaa
acaaaaaaegacbaaaaaaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaa
egiocaaaadaaaaaaanaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaadaaaaaa
amaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaa
egiocaaaadaaaaaaaoaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaak
pcaabaaaaaaaaaaaegiocaaaadaaaaaaapaaaaaapgbpbaaaaaaaaaaaegaobaaa
aaaaaaaadiaaaaaihcaabaaaabaaaaaafgafbaaaaaaaaaaaegiccaaaaaaaaaaa
aeaaaaaadcaaaaakhcaabaaaabaaaaaaegiccaaaaaaaaaaaadaaaaaaagaabaaa
aaaaaaaaegacbaaaabaaaaaadcaaaaakhcaabaaaaaaaaaaaegiccaaaaaaaaaaa
afaaaaaakgakbaaaaaaaaaaaegacbaaaabaaaaaadcaaaaakhccabaaaagaaaaaa
egiccaaaaaaaaaaaagaaaaaapgapbaaaaaaaaaaaegacbaaaaaaaaaaadoaaaaab
"
}

SubProgram "gles " {
Keywords { "POINT" }
"!!GLES


#ifdef VERTEX

varying highp vec3 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp mat4 _LightMatrix0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (_glesVertex.xyz - ((_World2Object * tmpvar_7).xyz * unity_Scale.w)));
  highp vec3 tmpvar_10;
  highp vec3 tmpvar_11;
  tmpvar_10 = tmpvar_1.xyz;
  tmpvar_11 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_11.x;
  tmpvar_12[0].z = tmpvar_2.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_11.y;
  tmpvar_12[1].z = tmpvar_2.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_11.z;
  tmpvar_12[2].z = tmpvar_2.z;
  vec4 v_13;
  v_13.x = _Object2World[0].x;
  v_13.y = _Object2World[1].x;
  v_13.z = _Object2World[2].x;
  v_13.w = _Object2World[3].x;
  highp vec4 tmpvar_14;
  tmpvar_14.xyz = (tmpvar_12 * v_13.xyz);
  tmpvar_14.w = tmpvar_9.x;
  highp vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_14 * unity_Scale.w);
  tmpvar_3 = tmpvar_15;
  vec4 v_16;
  v_16.x = _Object2World[0].y;
  v_16.y = _Object2World[1].y;
  v_16.z = _Object2World[2].y;
  v_16.w = _Object2World[3].y;
  highp vec4 tmpvar_17;
  tmpvar_17.xyz = (tmpvar_12 * v_16.xyz);
  tmpvar_17.w = tmpvar_9.y;
  highp vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * unity_Scale.w);
  tmpvar_4 = tmpvar_18;
  vec4 v_19;
  v_19.x = _Object2World[0].z;
  v_19.y = _Object2World[1].z;
  v_19.z = _Object2World[2].z;
  v_19.w = _Object2World[3].z;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = (tmpvar_12 * v_19.xyz);
  tmpvar_20.w = tmpvar_9.z;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * unity_Scale.w);
  tmpvar_5 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_12 * (((_World2Object * _WorldSpaceLightPos0).xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_6 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = _WorldSpaceCameraPos;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_12 * (((_World2Object * tmpvar_23).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * _glesVertex)).xyz;
}



#endif
#ifdef FRAGMENT

varying highp vec3 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform sampler2D _LightTexture0;
uniform lowp vec4 _LightColor0;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  mediump vec3 tmpvar_3;
  highp vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  mediump vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8.x = xlv_TEXCOORD1.w;
  tmpvar_8.y = xlv_TEXCOORD2.w;
  tmpvar_8.z = xlv_TEXCOORD3.w;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD1.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD2.xyz;
  tmpvar_6 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = xlv_TEXCOORD3.xyz;
  tmpvar_7 = tmpvar_11;
  tmpvar_3 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_12;
  lowp float tmpvar_13;
  mediump vec3 HDR_14;
  highp vec4 c_15;
  mediump vec3 tmpvar_16;
  tmpvar_16.x = tmpvar_5.z;
  tmpvar_16.y = tmpvar_6.z;
  tmpvar_16.z = tmpvar_7.z;
  highp vec3 tmpvar_17;
  tmpvar_17 = (tmpvar_4 - (2.0 * (dot (tmpvar_16, tmpvar_4) * tmpvar_16)));
  lowp vec4 tmpvar_18;
  tmpvar_18 = textureCube (_Cube, tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_18 * _ReflectColor);
  c_15 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = vec3((pow ((((c_15.x + c_15.y) + c_15.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_14 = tmpvar_20;
  mediump float tmpvar_21;
  tmpvar_21 = (1.0 - dot (normalize(tmpvar_3), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_22;
  tmpvar_22 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_21, _TransmissiveColor)));
  mediump float tmpvar_23;
  tmpvar_23 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_21)).x;
  tmpvar_13 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = ((c_15.xyz + HDR_14) + tmpvar_22.xyz);
  tmpvar_12 = tmpvar_24;
  mediump vec3 tmpvar_25;
  tmpvar_25 = normalize(xlv_TEXCOORD4);
  lightDir_2 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = normalize(xlv_TEXCOORD0);
  highp float tmpvar_27;
  tmpvar_27 = dot (xlv_TEXCOORD5, xlv_TEXCOORD5);
  lowp vec4 tmpvar_28;
  tmpvar_28 = texture2D (_LightTexture0, vec2(tmpvar_27));
  mediump vec3 lightDir_29;
  lightDir_29 = lightDir_2;
  mediump vec3 viewDir_30;
  viewDir_30 = tmpvar_26;
  mediump float atten_31;
  atten_31 = tmpvar_28.w;
  mediump vec4 c_32;
  c_32.xyz = (((((((lightDir_29.z * 0.5) + 0.5) * tmpvar_12) * _LightColor0.xyz) * atten_31) * 2.0) + ((_LightColor0.xyz * max (0.0, (pow ((viewDir_30 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_13))) * atten_31));
  c_32.w = 1.0;
  c_1.xyz = c_32.xyz;
  c_1.w = 0.0;
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "POINT" }
"!!GLES


#ifdef VERTEX

varying highp vec3 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp mat4 _LightMatrix0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (_glesVertex.xyz - ((_World2Object * tmpvar_7).xyz * unity_Scale.w)));
  highp vec3 tmpvar_10;
  highp vec3 tmpvar_11;
  tmpvar_10 = tmpvar_1.xyz;
  tmpvar_11 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_11.x;
  tmpvar_12[0].z = tmpvar_2.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_11.y;
  tmpvar_12[1].z = tmpvar_2.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_11.z;
  tmpvar_12[2].z = tmpvar_2.z;
  vec4 v_13;
  v_13.x = _Object2World[0].x;
  v_13.y = _Object2World[1].x;
  v_13.z = _Object2World[2].x;
  v_13.w = _Object2World[3].x;
  highp vec4 tmpvar_14;
  tmpvar_14.xyz = (tmpvar_12 * v_13.xyz);
  tmpvar_14.w = tmpvar_9.x;
  highp vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_14 * unity_Scale.w);
  tmpvar_3 = tmpvar_15;
  vec4 v_16;
  v_16.x = _Object2World[0].y;
  v_16.y = _Object2World[1].y;
  v_16.z = _Object2World[2].y;
  v_16.w = _Object2World[3].y;
  highp vec4 tmpvar_17;
  tmpvar_17.xyz = (tmpvar_12 * v_16.xyz);
  tmpvar_17.w = tmpvar_9.y;
  highp vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * unity_Scale.w);
  tmpvar_4 = tmpvar_18;
  vec4 v_19;
  v_19.x = _Object2World[0].z;
  v_19.y = _Object2World[1].z;
  v_19.z = _Object2World[2].z;
  v_19.w = _Object2World[3].z;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = (tmpvar_12 * v_19.xyz);
  tmpvar_20.w = tmpvar_9.z;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * unity_Scale.w);
  tmpvar_5 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_12 * (((_World2Object * _WorldSpaceLightPos0).xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_6 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = _WorldSpaceCameraPos;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_12 * (((_World2Object * tmpvar_23).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * _glesVertex)).xyz;
}



#endif
#ifdef FRAGMENT

varying highp vec3 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform sampler2D _LightTexture0;
uniform lowp vec4 _LightColor0;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  mediump vec3 tmpvar_3;
  highp vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  mediump vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8.x = xlv_TEXCOORD1.w;
  tmpvar_8.y = xlv_TEXCOORD2.w;
  tmpvar_8.z = xlv_TEXCOORD3.w;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD1.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD2.xyz;
  tmpvar_6 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = xlv_TEXCOORD3.xyz;
  tmpvar_7 = tmpvar_11;
  tmpvar_3 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_12;
  lowp float tmpvar_13;
  mediump vec3 HDR_14;
  highp vec4 c_15;
  mediump vec3 tmpvar_16;
  tmpvar_16.x = tmpvar_5.z;
  tmpvar_16.y = tmpvar_6.z;
  tmpvar_16.z = tmpvar_7.z;
  highp vec3 tmpvar_17;
  tmpvar_17 = (tmpvar_4 - (2.0 * (dot (tmpvar_16, tmpvar_4) * tmpvar_16)));
  lowp vec4 tmpvar_18;
  tmpvar_18 = textureCube (_Cube, tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_18 * _ReflectColor);
  c_15 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = vec3((pow ((((c_15.x + c_15.y) + c_15.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_14 = tmpvar_20;
  mediump float tmpvar_21;
  tmpvar_21 = (1.0 - dot (normalize(tmpvar_3), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_22;
  tmpvar_22 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_21, _TransmissiveColor)));
  mediump float tmpvar_23;
  tmpvar_23 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_21)).x;
  tmpvar_13 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = ((c_15.xyz + HDR_14) + tmpvar_22.xyz);
  tmpvar_12 = tmpvar_24;
  mediump vec3 tmpvar_25;
  tmpvar_25 = normalize(xlv_TEXCOORD4);
  lightDir_2 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = normalize(xlv_TEXCOORD0);
  highp float tmpvar_27;
  tmpvar_27 = dot (xlv_TEXCOORD5, xlv_TEXCOORD5);
  lowp vec4 tmpvar_28;
  tmpvar_28 = texture2D (_LightTexture0, vec2(tmpvar_27));
  mediump vec3 lightDir_29;
  lightDir_29 = lightDir_2;
  mediump vec3 viewDir_30;
  viewDir_30 = tmpvar_26;
  mediump float atten_31;
  atten_31 = tmpvar_28.w;
  mediump vec4 c_32;
  c_32.xyz = (((((((lightDir_29.z * 0.5) + 0.5) * tmpvar_12) * _LightColor0.xyz) * atten_31) * 2.0) + ((_LightColor0.xyz * max (0.0, (pow ((viewDir_30 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_13))) * atten_31));
  c_32.w = 1.0;
  c_1.xyz = c_32.xyz;
  c_1.w = 0.0;
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "POINT" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Color _glesColor
in vec4 _glesColor;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;
#define gl_MultiTexCoord1 _glesMultiTexCoord1
in vec4 _glesMultiTexCoord1;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
mat2 xll_transpose_mf2x2(mat2 m) {
  return mat2( m[0][0], m[1][0], m[0][1], m[1][1]);
}
mat3 xll_transpose_mf3x3(mat3 m) {
  return mat3( m[0][0], m[1][0], m[2][0],
               m[0][1], m[1][1], m[2][1],
               m[0][2], m[1][2], m[2][2]);
}
mat4 xll_transpose_mf4x4(mat4 m) {
  return mat4( m[0][0], m[1][0], m[2][0], m[3][0],
               m[0][1], m[1][1], m[2][1], m[3][1],
               m[0][2], m[1][2], m[2][2], m[3][2],
               m[0][3], m[1][3], m[2][3], m[3][3]);
}
vec2 xll_matrixindex_mf2x2_i (mat2 m, int i) { vec2 v; v.x=m[0][i]; v.y=m[1][i]; return v; }
vec3 xll_matrixindex_mf3x3_i (mat3 m, int i) { vec3 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; return v; }
vec4 xll_matrixindex_mf4x4_i (mat4 m, int i) { vec4 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; v.w=m[3][i]; return v; }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 401
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 434
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    mediump vec3 lightDir;
    highp vec3 _LightCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform highp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform sampler2D _LightTexture0;
uniform highp mat4 _LightMatrix0;
#line 393
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
#line 397
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 410
#line 445
#line 82
highp vec3 ObjSpaceLightDir( in highp vec4 v ) {
    highp vec3 objSpaceLightPos = (_World2Object * _WorldSpaceLightPos0).xyz;
    return ((objSpaceLightPos.xyz * unity_Scale.w) - v.xyz);
}
#line 91
highp vec3 ObjSpaceViewDir( in highp vec4 v ) {
    highp vec3 objSpaceCameraPos = ((_World2Object * vec4( _WorldSpaceCameraPos.xyz, 1.0)).xyz * unity_Scale.w);
    return (objSpaceCameraPos - v.xyz);
}
#line 445
v2f_surf vert_surf( in appdata_full v ) {
    v2f_surf o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 449
    highp vec3 viewDir = (-ObjSpaceViewDir( v.vertex));
    highp vec3 worldRefl = (mat3( _Object2World) * viewDir);
    highp vec3 binormal = (cross( v.normal, v.tangent.xyz) * v.tangent.w);
    highp mat3 rotation = xll_transpose_mf3x3(mat3( v.tangent.xyz, binormal, v.normal));
    #line 453
    o.TtoW0 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 0).xyz), worldRefl.x) * unity_Scale.w);
    o.TtoW1 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 1).xyz), worldRefl.y) * unity_Scale.w);
    o.TtoW2 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 2).xyz), worldRefl.z) * unity_Scale.w);
    highp vec3 lightDir = (rotation * ObjSpaceLightDir( v.vertex));
    #line 457
    o.lightDir = lightDir;
    highp vec3 viewDirForLight = (rotation * ObjSpaceViewDir( v.vertex));
    o.viewDir = viewDirForLight;
    o._LightCoord = (_LightMatrix0 * (_Object2World * v.vertex)).xyz;
    #line 462
    return o;
}

out highp vec3 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_TEXCOORD2;
out lowp vec4 xlv_TEXCOORD3;
out mediump vec3 xlv_TEXCOORD4;
out highp vec3 xlv_TEXCOORD5;
void main() {
    v2f_surf xl_retval;
    appdata_full xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.tangent = vec4(TANGENT);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xlt_v.texcoord1 = vec4(gl_MultiTexCoord1);
    xlt_v.color = vec4(gl_Color);
    xl_retval = vert_surf( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD0 = vec3(xl_retval.viewDir);
    xlv_TEXCOORD1 = vec4(xl_retval.TtoW0);
    xlv_TEXCOORD2 = vec4(xl_retval.TtoW1);
    xlv_TEXCOORD3 = vec4(xl_retval.TtoW2);
    xlv_TEXCOORD4 = vec3(xl_retval.lightDir);
    xlv_TEXCOORD5 = vec3(xl_retval._LightCoord);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 401
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 434
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    mediump vec3 lightDir;
    highp vec3 _LightCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform highp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform sampler2D _LightTexture0;
uniform highp mat4 _LightMatrix0;
#line 393
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
#line 397
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 410
#line 445
#line 424
mediump vec4 LightingHalfLambertSpecular( in SurfaceOutput s, in mediump vec3 lightDir, in mediump vec3 viewDir, in mediump float atten ) {
    #line 426
    mediump float NdotL = ((dot( s.Normal, lightDir) * 0.5) + 0.5);
    mediump vec3 halfVector = (viewDir + s.Normal);
    mediump float spec = max( 0.0, (pow( dot( halfVector, s.Normal), (10.0 - s.Specular)) * s.Gloss));
    mediump vec4 c;
    #line 430
    c.xyz = (((((NdotL * s.Albedo) * _LightColor0.xyz) * atten) * 2.0) + ((_LightColor0.xyz * spec) * atten));
    c.w = s.Alpha;
    return c;
}
#line 410
void surf( in Input IN, inout SurfaceOutput o ) {
    o.Normal = vec3( 0.0, 0.0, 1.0);
    highp vec3 worldRefl = reflect( IN.worldRefl, vec3( dot( IN.TtoW0, o.Normal), dot( IN.TtoW1, o.Normal), dot( IN.TtoW2, o.Normal)));
    #line 414
    highp vec4 c = (texture( _Cube, worldRefl) * _ReflectColor);
    mediump vec3 HDR = vec3( (pow( (((c.x + c.y) + c.z) / 3.0), _HdrPower) * _HdrContrast));
    mediump float fresnel = (1.0 - dot( normalize(IN.viewDir), normalize(vec3( 0.0, 0.0, 1.0))));
    mediump vec4 fresnelLerp = mix( _Color, _FresnelColor, vec4( pow( fresnel, _TransmissiveColor)));
    #line 418
    mediump vec4 GlossLerp = mix( vec4( 0.0, 0.0, 0.0, 0.0), vec4( 1.0, 1.0, 1.0, 1.0), vec4( fresnel));
    o.Specular = _Shininess;
    o.Gloss = GlossLerp.x;
    o.Albedo = ((c.xyz + HDR) + fresnelLerp.xyz);
    #line 422
    o.Alpha = 1.0;
}
#line 464
lowp vec4 frag_surf( in v2f_surf IN ) {
    #line 466
    Input surfIN;
    surfIN.worldRefl = vec3( IN.TtoW0.w, IN.TtoW1.w, IN.TtoW2.w);
    surfIN.TtoW0 = IN.TtoW0.xyz;
    surfIN.TtoW1 = IN.TtoW1.xyz;
    #line 470
    surfIN.TtoW2 = IN.TtoW2.xyz;
    surfIN.viewDir = IN.viewDir;
    SurfaceOutput o;
    o.Albedo = vec3( 0.0);
    #line 474
    o.Emission = vec3( 0.0);
    o.Specular = 0.0;
    o.Alpha = 0.0;
    o.Gloss = 0.0;
    #line 478
    surf( surfIN, o);
    lowp vec3 lightDir = normalize(IN.lightDir);
    lowp vec4 c = LightingHalfLambertSpecular( o, lightDir, normalize(IN.viewDir), (texture( _LightTexture0, vec2( dot( IN._LightCoord, IN._LightCoord))).w * 1.0));
    c.w = 0.0;
    #line 482
    return c;
}
in highp vec3 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_TEXCOORD2;
in lowp vec4 xlv_TEXCOORD3;
in mediump vec3 xlv_TEXCOORD4;
in highp vec3 xlv_TEXCOORD5;
void main() {
    lowp vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.viewDir = vec3(xlv_TEXCOORD0);
    xlt_IN.TtoW0 = vec4(xlv_TEXCOORD1);
    xlt_IN.TtoW1 = vec4(xlv_TEXCOORD2);
    xlt_IN.TtoW2 = vec4(xlv_TEXCOORD3);
    xlt_IN.lightDir = vec3(xlv_TEXCOORD4);
    xlt_IN._LightCoord = vec3(xlv_TEXCOORD5);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Vector 13 [_WorldSpaceCameraPos]
Vector 14 [_WorldSpaceLightPos0]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 15 [unity_Scale]
"3.0-!!ARBvp1.0
# 39 ALU
PARAM c[16] = { { 1 },
		state.matrix.mvp,
		program.local[5..15] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R0.xyz, vertex.attrib[14];
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R0.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MUL R3.xyz, R0, vertex.attrib[14].w;
MOV R0, c[14];
MOV R1.xyz, c[13];
MOV R1.w, c[0].x;
DP4 R2.z, R1, c[11];
DP4 R2.x, R1, c[9];
DP4 R2.y, R1, c[10];
MAD R2.xyz, R2, c[15].w, -vertex.position;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
DP3 R0.y, R3, c[5];
DP3 R0.w, -R2, c[5];
DP3 R0.x, vertex.attrib[14], c[5];
DP3 R0.z, vertex.normal, c[5];
MUL result.texcoord[1], R0, c[15].w;
DP3 R0.y, R3, c[6];
DP3 R0.w, -R2, c[6];
DP3 R0.x, vertex.attrib[14], c[6];
DP3 R0.z, vertex.normal, c[6];
MUL result.texcoord[2], R0, c[15].w;
DP3 R0.y, R3, c[7];
DP3 R0.w, -R2, c[7];
DP3 R0.x, vertex.attrib[14], c[7];
DP3 R0.z, vertex.normal, c[7];
DP3 result.texcoord[0].y, R2, R3;
DP3 result.texcoord[4].y, R3, R1;
MUL result.texcoord[3], R0, c[15].w;
DP3 result.texcoord[0].z, vertex.normal, R2;
DP3 result.texcoord[0].x, R2, vertex.attrib[14];
DP3 result.texcoord[4].z, vertex.normal, R1;
DP3 result.texcoord[4].x, vertex.attrib[14], R1;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 39 instructions, 4 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Matrix 0 [glstate_matrix_mvp]
Vector 12 [_WorldSpaceCameraPos]
Vector 13 [_WorldSpaceLightPos0]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 14 [unity_Scale]
"vs_3_0
; 42 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
def c15, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r0.xyz, v2.yzxw, r0.zxyw, -r1
mul r3.xyz, r0, v1.w
mov r0, c10
dp4 r4.z, c13, r0
mov r0, c9
dp4 r4.y, c13, r0
mov r1.w, c15.x
mov r1.xyz, c12
dp4 r2.z, r1, c10
dp4 r2.x, r1, c8
dp4 r2.y, r1, c9
mad r2.xyz, r2, c14.w, -v0
mov r1, c8
dp4 r4.x, c13, r1
dp3 r0.y, r3, c4
dp3 r0.w, -r2, c4
dp3 r0.x, v1, c4
dp3 r0.z, v2, c4
mul o2, r0, c14.w
dp3 r0.y, r3, c5
dp3 r0.w, -r2, c5
dp3 r0.x, v1, c5
dp3 r0.z, v2, c5
mul o3, r0, c14.w
dp3 r0.y, r3, c6
dp3 r0.w, -r2, c6
dp3 r0.x, v1, c6
dp3 r0.z, v2, c6
dp3 o1.y, r2, r3
dp3 o5.y, r3, r4
mul o4, r0, c14.w
dp3 o1.z, v2, r2
dp3 o1.x, r2, v1
dp3 o5.z, v2, r4
dp3 o5.x, v1, r4
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "color" Color
ConstBuffer "UnityPerCamera" 128 // 76 used size, 8 vars
Vector 64 [_WorldSpaceCameraPos] 3
ConstBuffer "UnityLighting" 720 // 16 used size, 17 vars
Vector 0 [_WorldSpaceLightPos0] 4
ConstBuffer "UnityPerDraw" 336 // 336 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
Matrix 192 [_Object2World] 4
Matrix 256 [_World2Object] 4
Vector 320 [unity_Scale] 4
BindCB "UnityPerCamera" 0
BindCB "UnityLighting" 1
BindCB "UnityPerDraw" 2
// 49 instructions, 4 temp regs, 0 temp arrays:
// ALU 37 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedplbonjiohgapmlfeehoepnaiammekbdcabaaaaaafaaiaaaaadaaaaaa
cmaaaaaapeaaaaaakmabaaaaejfdeheomaaaaaaaagaaaaaaaiaaaaaajiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaakbaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaakjaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
ahahaaaalaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaadaaaaaaapaaaaaalaaaaaaa
abaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaaljaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaafaaaaaaapaaaaaafaepfdejfeejepeoaafeebeoehefeofeaaeoepfc
enebemaafeeffiedepepfceeaaedepemepfcaaklepfdeheolaaaaaaaagaaaaaa
aiaaaaaajiaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaakeaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaahaiaaaakeaaaaaaabaaaaaaaaaaaaaa
adaaaaaaacaaaaaaapaaaaaakeaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaa
apaaaaaakeaaaaaaadaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaakeaaaaaa
aeaaaaaaaaaaaaaaadaaaaaaafaaaaaaahaiaaaafdfgfpfaepfdejfeejepeoaa
feeffiedepepfceeaaklklklfdeieefcjmagaaaaeaaaabaakhabaaaafjaaaaae
egiocaaaaaaaaaaaafaaaaaafjaaaaaeegiocaaaabaaaaaaabaaaaaafjaaaaae
egiocaaaacaaaaaabfaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaadpcbabaaa
abaaaaaafpaaaaadhcbabaaaacaaaaaaghaaaaaepccabaaaaaaaaaaaabaaaaaa
gfaaaaadhccabaaaabaaaaaagfaaaaadpccabaaaacaaaaaagfaaaaadpccabaaa
adaaaaaagfaaaaadpccabaaaaeaaaaaagfaaaaadhccabaaaafaaaaaagiaaaaac
aeaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaacaaaaaa
abaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaacaaaaaaaaaaaaaaagbabaaa
aaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaacaaaaaa
acaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaa
egiocaaaacaaaaaaadaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadiaaaaaj
hcaabaaaaaaaaaaafgifcaaaaaaaaaaaaeaaaaaaegiccaaaacaaaaaabbaaaaaa
dcaaaaalhcaabaaaaaaaaaaaegiccaaaacaaaaaabaaaaaaaagiacaaaaaaaaaaa
aeaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaaaaaaaaaegiccaaaacaaaaaa
bcaaaaaakgikcaaaaaaaaaaaaeaaaaaaegacbaaaaaaaaaaaaaaaaaaihcaabaaa
aaaaaaaaegacbaaaaaaaaaaaegiccaaaacaaaaaabdaaaaaadcaaaaalhcaabaaa
aaaaaaaaegacbaaaaaaaaaaapgipcaaaacaaaaaabeaaaaaaegbcbaiaebaaaaaa
aaaaaaaabaaaaaahbccabaaaabaaaaaaegbcbaaaabaaaaaaegacbaaaaaaaaaaa
baaaaaaheccabaaaabaaaaaaegbcbaaaacaaaaaaegacbaaaaaaaaaaadiaaaaah
hcaabaaaabaaaaaajgbebaaaabaaaaaacgbjbaaaacaaaaaadcaaaaakhcaabaaa
abaaaaaajgbebaaaacaaaaaacgbjbaaaabaaaaaaegacbaiaebaaaaaaabaaaaaa
diaaaaahhcaabaaaabaaaaaaegacbaaaabaaaaaapgbpbaaaabaaaaaabaaaaaah
cccabaaaabaaaaaaegacbaaaabaaaaaaegacbaaaaaaaaaaadiaaaaajhcaabaaa
acaaaaaafgafbaiaebaaaaaaaaaaaaaaegiccaaaacaaaaaaanaaaaaadcaaaaal
lcaabaaaaaaaaaaaegiicaaaacaaaaaaamaaaaaaagaabaiaebaaaaaaaaaaaaaa
egaibaaaacaaaaaadcaaaaallcaabaaaaaaaaaaaegiicaaaacaaaaaaaoaaaaaa
kgakbaiaebaaaaaaaaaaaaaaegambaaaaaaaaaaadgaaaaaficaabaaaacaaaaaa
akaabaaaaaaaaaaadgaaaaagbcaabaaaadaaaaaaakiacaaaacaaaaaaamaaaaaa
dgaaaaagccaabaaaadaaaaaaakiacaaaacaaaaaaanaaaaaadgaaaaagecaabaaa
adaaaaaaakiacaaaacaaaaaaaoaaaaaabaaaaaahccaabaaaacaaaaaaegacbaaa
abaaaaaaegacbaaaadaaaaaabaaaaaahbcaabaaaacaaaaaaegbcbaaaabaaaaaa
egacbaaaadaaaaaabaaaaaahecaabaaaacaaaaaaegbcbaaaacaaaaaaegacbaaa
adaaaaaadiaaaaaipccabaaaacaaaaaaegaobaaaacaaaaaapgipcaaaacaaaaaa
beaaaaaadgaaaaaficaabaaaacaaaaaabkaabaaaaaaaaaaadgaaaaagbcaabaaa
adaaaaaabkiacaaaacaaaaaaamaaaaaadgaaaaagccaabaaaadaaaaaabkiacaaa
acaaaaaaanaaaaaadgaaaaagecaabaaaadaaaaaabkiacaaaacaaaaaaaoaaaaaa
baaaaaahccaabaaaacaaaaaaegacbaaaabaaaaaaegacbaaaadaaaaaabaaaaaah
bcaabaaaacaaaaaaegbcbaaaabaaaaaaegacbaaaadaaaaaabaaaaaahecaabaaa
acaaaaaaegbcbaaaacaaaaaaegacbaaaadaaaaaadiaaaaaipccabaaaadaaaaaa
egaobaaaacaaaaaapgipcaaaacaaaaaabeaaaaaadgaaaaagbcaabaaaacaaaaaa
ckiacaaaacaaaaaaamaaaaaadgaaaaagccaabaaaacaaaaaackiacaaaacaaaaaa
anaaaaaadgaaaaagecaabaaaacaaaaaackiacaaaacaaaaaaaoaaaaaabaaaaaah
ccaabaaaaaaaaaaaegacbaaaabaaaaaaegacbaaaacaaaaaabaaaaaahbcaabaaa
aaaaaaaaegbcbaaaabaaaaaaegacbaaaacaaaaaabaaaaaahecaabaaaaaaaaaaa
egbcbaaaacaaaaaaegacbaaaacaaaaaadiaaaaaipccabaaaaeaaaaaaegaobaaa
aaaaaaaapgipcaaaacaaaaaabeaaaaaadiaaaaajhcaabaaaaaaaaaaafgifcaaa
abaaaaaaaaaaaaaaegiccaaaacaaaaaabbaaaaaadcaaaaalhcaabaaaaaaaaaaa
egiccaaaacaaaaaabaaaaaaaagiacaaaabaaaaaaaaaaaaaaegacbaaaaaaaaaaa
dcaaaaalhcaabaaaaaaaaaaaegiccaaaacaaaaaabcaaaaaakgikcaaaabaaaaaa
aaaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaaaaaaaaaegiccaaaacaaaaaa
bdaaaaaapgipcaaaabaaaaaaaaaaaaaaegacbaaaaaaaaaaabaaaaaahcccabaaa
afaaaaaaegacbaaaabaaaaaaegacbaaaaaaaaaaabaaaaaahbccabaaaafaaaaaa
egbcbaaaabaaaaaaegacbaaaaaaaaaaabaaaaaaheccabaaaafaaaaaaegbcbaaa
acaaaaaaegacbaaaaaaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" }
"!!GLES


#ifdef VERTEX

varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (_glesVertex.xyz - ((_World2Object * tmpvar_7).xyz * unity_Scale.w)));
  highp vec3 tmpvar_10;
  highp vec3 tmpvar_11;
  tmpvar_10 = tmpvar_1.xyz;
  tmpvar_11 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_11.x;
  tmpvar_12[0].z = tmpvar_2.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_11.y;
  tmpvar_12[1].z = tmpvar_2.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_11.z;
  tmpvar_12[2].z = tmpvar_2.z;
  vec4 v_13;
  v_13.x = _Object2World[0].x;
  v_13.y = _Object2World[1].x;
  v_13.z = _Object2World[2].x;
  v_13.w = _Object2World[3].x;
  highp vec4 tmpvar_14;
  tmpvar_14.xyz = (tmpvar_12 * v_13.xyz);
  tmpvar_14.w = tmpvar_9.x;
  highp vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_14 * unity_Scale.w);
  tmpvar_3 = tmpvar_15;
  vec4 v_16;
  v_16.x = _Object2World[0].y;
  v_16.y = _Object2World[1].y;
  v_16.z = _Object2World[2].y;
  v_16.w = _Object2World[3].y;
  highp vec4 tmpvar_17;
  tmpvar_17.xyz = (tmpvar_12 * v_16.xyz);
  tmpvar_17.w = tmpvar_9.y;
  highp vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * unity_Scale.w);
  tmpvar_4 = tmpvar_18;
  vec4 v_19;
  v_19.x = _Object2World[0].z;
  v_19.y = _Object2World[1].z;
  v_19.z = _Object2World[2].z;
  v_19.w = _Object2World[3].z;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = (tmpvar_12 * v_19.xyz);
  tmpvar_20.w = tmpvar_9.z;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * unity_Scale.w);
  tmpvar_5 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_12 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_6 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = _WorldSpaceCameraPos;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_12 * (((_World2Object * tmpvar_23).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
}



#endif
#ifdef FRAGMENT

varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform lowp vec4 _LightColor0;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  mediump vec3 tmpvar_3;
  highp vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  mediump vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8.x = xlv_TEXCOORD1.w;
  tmpvar_8.y = xlv_TEXCOORD2.w;
  tmpvar_8.z = xlv_TEXCOORD3.w;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD1.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD2.xyz;
  tmpvar_6 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = xlv_TEXCOORD3.xyz;
  tmpvar_7 = tmpvar_11;
  tmpvar_3 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_12;
  lowp float tmpvar_13;
  mediump vec3 HDR_14;
  highp vec4 c_15;
  mediump vec3 tmpvar_16;
  tmpvar_16.x = tmpvar_5.z;
  tmpvar_16.y = tmpvar_6.z;
  tmpvar_16.z = tmpvar_7.z;
  highp vec3 tmpvar_17;
  tmpvar_17 = (tmpvar_4 - (2.0 * (dot (tmpvar_16, tmpvar_4) * tmpvar_16)));
  lowp vec4 tmpvar_18;
  tmpvar_18 = textureCube (_Cube, tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_18 * _ReflectColor);
  c_15 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = vec3((pow ((((c_15.x + c_15.y) + c_15.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_14 = tmpvar_20;
  mediump float tmpvar_21;
  tmpvar_21 = (1.0 - dot (normalize(tmpvar_3), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_22;
  tmpvar_22 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_21, _TransmissiveColor)));
  mediump float tmpvar_23;
  tmpvar_23 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_21)).x;
  tmpvar_13 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = ((c_15.xyz + HDR_14) + tmpvar_22.xyz);
  tmpvar_12 = tmpvar_24;
  lightDir_2 = xlv_TEXCOORD4;
  highp vec3 tmpvar_25;
  tmpvar_25 = normalize(xlv_TEXCOORD0);
  mediump vec3 lightDir_26;
  lightDir_26 = lightDir_2;
  mediump vec3 viewDir_27;
  viewDir_27 = tmpvar_25;
  mediump vec4 c_28;
  c_28.xyz = ((((((lightDir_26.z * 0.5) + 0.5) * tmpvar_12) * _LightColor0.xyz) * 2.0) + (_LightColor0.xyz * max (0.0, (pow ((viewDir_27 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_13))));
  c_28.w = 1.0;
  c_1.xyz = c_28.xyz;
  c_1.w = 0.0;
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" }
"!!GLES


#ifdef VERTEX

varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (_glesVertex.xyz - ((_World2Object * tmpvar_7).xyz * unity_Scale.w)));
  highp vec3 tmpvar_10;
  highp vec3 tmpvar_11;
  tmpvar_10 = tmpvar_1.xyz;
  tmpvar_11 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_11.x;
  tmpvar_12[0].z = tmpvar_2.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_11.y;
  tmpvar_12[1].z = tmpvar_2.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_11.z;
  tmpvar_12[2].z = tmpvar_2.z;
  vec4 v_13;
  v_13.x = _Object2World[0].x;
  v_13.y = _Object2World[1].x;
  v_13.z = _Object2World[2].x;
  v_13.w = _Object2World[3].x;
  highp vec4 tmpvar_14;
  tmpvar_14.xyz = (tmpvar_12 * v_13.xyz);
  tmpvar_14.w = tmpvar_9.x;
  highp vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_14 * unity_Scale.w);
  tmpvar_3 = tmpvar_15;
  vec4 v_16;
  v_16.x = _Object2World[0].y;
  v_16.y = _Object2World[1].y;
  v_16.z = _Object2World[2].y;
  v_16.w = _Object2World[3].y;
  highp vec4 tmpvar_17;
  tmpvar_17.xyz = (tmpvar_12 * v_16.xyz);
  tmpvar_17.w = tmpvar_9.y;
  highp vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * unity_Scale.w);
  tmpvar_4 = tmpvar_18;
  vec4 v_19;
  v_19.x = _Object2World[0].z;
  v_19.y = _Object2World[1].z;
  v_19.z = _Object2World[2].z;
  v_19.w = _Object2World[3].z;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = (tmpvar_12 * v_19.xyz);
  tmpvar_20.w = tmpvar_9.z;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * unity_Scale.w);
  tmpvar_5 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_12 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_6 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = _WorldSpaceCameraPos;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_12 * (((_World2Object * tmpvar_23).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
}



#endif
#ifdef FRAGMENT

varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform lowp vec4 _LightColor0;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  mediump vec3 tmpvar_3;
  highp vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  mediump vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8.x = xlv_TEXCOORD1.w;
  tmpvar_8.y = xlv_TEXCOORD2.w;
  tmpvar_8.z = xlv_TEXCOORD3.w;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD1.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD2.xyz;
  tmpvar_6 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = xlv_TEXCOORD3.xyz;
  tmpvar_7 = tmpvar_11;
  tmpvar_3 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_12;
  lowp float tmpvar_13;
  mediump vec3 HDR_14;
  highp vec4 c_15;
  mediump vec3 tmpvar_16;
  tmpvar_16.x = tmpvar_5.z;
  tmpvar_16.y = tmpvar_6.z;
  tmpvar_16.z = tmpvar_7.z;
  highp vec3 tmpvar_17;
  tmpvar_17 = (tmpvar_4 - (2.0 * (dot (tmpvar_16, tmpvar_4) * tmpvar_16)));
  lowp vec4 tmpvar_18;
  tmpvar_18 = textureCube (_Cube, tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_18 * _ReflectColor);
  c_15 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = vec3((pow ((((c_15.x + c_15.y) + c_15.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_14 = tmpvar_20;
  mediump float tmpvar_21;
  tmpvar_21 = (1.0 - dot (normalize(tmpvar_3), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_22;
  tmpvar_22 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_21, _TransmissiveColor)));
  mediump float tmpvar_23;
  tmpvar_23 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_21)).x;
  tmpvar_13 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = ((c_15.xyz + HDR_14) + tmpvar_22.xyz);
  tmpvar_12 = tmpvar_24;
  lightDir_2 = xlv_TEXCOORD4;
  highp vec3 tmpvar_25;
  tmpvar_25 = normalize(xlv_TEXCOORD0);
  mediump vec3 lightDir_26;
  lightDir_26 = lightDir_2;
  mediump vec3 viewDir_27;
  viewDir_27 = tmpvar_25;
  mediump vec4 c_28;
  c_28.xyz = ((((((lightDir_26.z * 0.5) + 0.5) * tmpvar_12) * _LightColor0.xyz) * 2.0) + (_LightColor0.xyz * max (0.0, (pow ((viewDir_27 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_13))));
  c_28.w = 1.0;
  c_1.xyz = c_28.xyz;
  c_1.w = 0.0;
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Color _glesColor
in vec4 _glesColor;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;
#define gl_MultiTexCoord1 _glesMultiTexCoord1
in vec4 _glesMultiTexCoord1;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
mat2 xll_transpose_mf2x2(mat2 m) {
  return mat2( m[0][0], m[1][0], m[0][1], m[1][1]);
}
mat3 xll_transpose_mf3x3(mat3 m) {
  return mat3( m[0][0], m[1][0], m[2][0],
               m[0][1], m[1][1], m[2][1],
               m[0][2], m[1][2], m[2][2]);
}
mat4 xll_transpose_mf4x4(mat4 m) {
  return mat4( m[0][0], m[1][0], m[2][0], m[3][0],
               m[0][1], m[1][1], m[2][1], m[3][1],
               m[0][2], m[1][2], m[2][2], m[3][2],
               m[0][3], m[1][3], m[2][3], m[3][3]);
}
vec2 xll_matrixindex_mf2x2_i (mat2 m, int i) { vec2 v; v.x=m[0][i]; v.y=m[1][i]; return v; }
vec3 xll_matrixindex_mf3x3_i (mat3 m, int i) { vec3 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; return v; }
vec4 xll_matrixindex_mf4x4_i (mat4 m, int i) { vec4 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; v.w=m[3][i]; return v; }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 399
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 432
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    mediump vec3 lightDir;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 393
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 397
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 408
#line 442
#line 82
highp vec3 ObjSpaceLightDir( in highp vec4 v ) {
    highp vec3 objSpaceLightPos = (_World2Object * _WorldSpaceLightPos0).xyz;
    return objSpaceLightPos.xyz;
}
#line 91
highp vec3 ObjSpaceViewDir( in highp vec4 v ) {
    highp vec3 objSpaceCameraPos = ((_World2Object * vec4( _WorldSpaceCameraPos.xyz, 1.0)).xyz * unity_Scale.w);
    return (objSpaceCameraPos - v.xyz);
}
#line 442
v2f_surf vert_surf( in appdata_full v ) {
    v2f_surf o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 446
    highp vec3 viewDir = (-ObjSpaceViewDir( v.vertex));
    highp vec3 worldRefl = (mat3( _Object2World) * viewDir);
    highp vec3 binormal = (cross( v.normal, v.tangent.xyz) * v.tangent.w);
    highp mat3 rotation = xll_transpose_mf3x3(mat3( v.tangent.xyz, binormal, v.normal));
    #line 450
    o.TtoW0 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 0).xyz), worldRefl.x) * unity_Scale.w);
    o.TtoW1 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 1).xyz), worldRefl.y) * unity_Scale.w);
    o.TtoW2 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 2).xyz), worldRefl.z) * unity_Scale.w);
    highp vec3 lightDir = (rotation * ObjSpaceLightDir( v.vertex));
    #line 454
    o.lightDir = lightDir;
    highp vec3 viewDirForLight = (rotation * ObjSpaceViewDir( v.vertex));
    o.viewDir = viewDirForLight;
    #line 458
    return o;
}

out highp vec3 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_TEXCOORD2;
out lowp vec4 xlv_TEXCOORD3;
out mediump vec3 xlv_TEXCOORD4;
void main() {
    v2f_surf xl_retval;
    appdata_full xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.tangent = vec4(TANGENT);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xlt_v.texcoord1 = vec4(gl_MultiTexCoord1);
    xlt_v.color = vec4(gl_Color);
    xl_retval = vert_surf( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD0 = vec3(xl_retval.viewDir);
    xlv_TEXCOORD1 = vec4(xl_retval.TtoW0);
    xlv_TEXCOORD2 = vec4(xl_retval.TtoW1);
    xlv_TEXCOORD3 = vec4(xl_retval.TtoW2);
    xlv_TEXCOORD4 = vec3(xl_retval.lightDir);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 399
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 432
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    mediump vec3 lightDir;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
#line 393
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
#line 397
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 408
#line 442
#line 422
mediump vec4 LightingHalfLambertSpecular( in SurfaceOutput s, in mediump vec3 lightDir, in mediump vec3 viewDir, in mediump float atten ) {
    #line 424
    mediump float NdotL = ((dot( s.Normal, lightDir) * 0.5) + 0.5);
    mediump vec3 halfVector = (viewDir + s.Normal);
    mediump float spec = max( 0.0, (pow( dot( halfVector, s.Normal), (10.0 - s.Specular)) * s.Gloss));
    mediump vec4 c;
    #line 428
    c.xyz = (((((NdotL * s.Albedo) * _LightColor0.xyz) * atten) * 2.0) + ((_LightColor0.xyz * spec) * atten));
    c.w = s.Alpha;
    return c;
}
#line 408
void surf( in Input IN, inout SurfaceOutput o ) {
    o.Normal = vec3( 0.0, 0.0, 1.0);
    highp vec3 worldRefl = reflect( IN.worldRefl, vec3( dot( IN.TtoW0, o.Normal), dot( IN.TtoW1, o.Normal), dot( IN.TtoW2, o.Normal)));
    #line 412
    highp vec4 c = (texture( _Cube, worldRefl) * _ReflectColor);
    mediump vec3 HDR = vec3( (pow( (((c.x + c.y) + c.z) / 3.0), _HdrPower) * _HdrContrast));
    mediump float fresnel = (1.0 - dot( normalize(IN.viewDir), normalize(vec3( 0.0, 0.0, 1.0))));
    mediump vec4 fresnelLerp = mix( _Color, _FresnelColor, vec4( pow( fresnel, _TransmissiveColor)));
    #line 416
    mediump vec4 GlossLerp = mix( vec4( 0.0, 0.0, 0.0, 0.0), vec4( 1.0, 1.0, 1.0, 1.0), vec4( fresnel));
    o.Specular = _Shininess;
    o.Gloss = GlossLerp.x;
    o.Albedo = ((c.xyz + HDR) + fresnelLerp.xyz);
    #line 420
    o.Alpha = 1.0;
}
#line 460
lowp vec4 frag_surf( in v2f_surf IN ) {
    #line 462
    Input surfIN;
    surfIN.worldRefl = vec3( IN.TtoW0.w, IN.TtoW1.w, IN.TtoW2.w);
    surfIN.TtoW0 = IN.TtoW0.xyz;
    surfIN.TtoW1 = IN.TtoW1.xyz;
    #line 466
    surfIN.TtoW2 = IN.TtoW2.xyz;
    surfIN.viewDir = IN.viewDir;
    SurfaceOutput o;
    o.Albedo = vec3( 0.0);
    #line 470
    o.Emission = vec3( 0.0);
    o.Specular = 0.0;
    o.Alpha = 0.0;
    o.Gloss = 0.0;
    #line 474
    surf( surfIN, o);
    lowp vec3 lightDir = IN.lightDir;
    lowp vec4 c = LightingHalfLambertSpecular( o, lightDir, normalize(IN.viewDir), 1.0);
    c.w = 0.0;
    #line 478
    return c;
}
in highp vec3 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_TEXCOORD2;
in lowp vec4 xlv_TEXCOORD3;
in mediump vec3 xlv_TEXCOORD4;
void main() {
    lowp vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.viewDir = vec3(xlv_TEXCOORD0);
    xlt_IN.TtoW0 = vec4(xlv_TEXCOORD1);
    xlt_IN.TtoW1 = vec4(xlv_TEXCOORD2);
    xlt_IN.TtoW2 = vec4(xlv_TEXCOORD3);
    xlt_IN.lightDir = vec3(xlv_TEXCOORD4);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "opengl " {
Keywords { "SPOT" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Vector 17 [_WorldSpaceCameraPos]
Vector 18 [_WorldSpaceLightPos0]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 19 [unity_Scale]
Matrix 13 [_LightMatrix0]
"3.0-!!ARBvp1.0
# 48 ALU
PARAM c[20] = { { 1 },
		state.matrix.mvp,
		program.local[5..19] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R0.xyz, vertex.attrib[14];
MUL R2.xyz, vertex.normal.zxyw, R0.yzxw;
MOV R1, c[18];
MAD R0.xyz, vertex.normal.yzxw, R0.zxyw, -R2;
DP4 R2.z, R1, c[11];
DP4 R2.x, R1, c[9];
DP4 R2.y, R1, c[10];
MAD R3.xyz, R2, c[19].w, -vertex.position;
MUL R2.xyz, R0, vertex.attrib[14].w;
MOV R0.xyz, c[17];
MOV R0.w, c[0].x;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
MAD R1.xyz, R1, c[19].w, -vertex.position;
DP3 R0.y, R2, c[5];
DP3 R0.w, -R1, c[5];
DP3 R0.x, vertex.attrib[14], c[5];
DP3 R0.z, vertex.normal, c[5];
MUL result.texcoord[1], R0, c[19].w;
DP3 R0.y, R2, c[6];
DP3 R0.w, -R1, c[6];
DP3 R0.x, vertex.attrib[14], c[6];
DP3 R0.z, vertex.normal, c[6];
MUL result.texcoord[2], R0, c[19].w;
DP3 R0.y, R2, c[7];
DP3 R0.w, -R1, c[7];
DP3 R0.x, vertex.attrib[14], c[7];
DP3 R0.z, vertex.normal, c[7];
MUL result.texcoord[3], R0, c[19].w;
DP4 R0.w, vertex.position, c[8];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP3 result.texcoord[0].y, R1, R2;
DP3 result.texcoord[4].y, R2, R3;
DP3 result.texcoord[0].z, vertex.normal, R1;
DP3 result.texcoord[0].x, R1, vertex.attrib[14];
DP3 result.texcoord[4].z, vertex.normal, R3;
DP3 result.texcoord[4].x, vertex.attrib[14], R3;
DP4 result.texcoord[5].w, R0, c[16];
DP4 result.texcoord[5].z, R0, c[15];
DP4 result.texcoord[5].y, R0, c[14];
DP4 result.texcoord[5].x, R0, c[13];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 48 instructions, 4 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "SPOT" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Matrix 0 [glstate_matrix_mvp]
Vector 16 [_WorldSpaceCameraPos]
Vector 17 [_WorldSpaceLightPos0]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 18 [unity_Scale]
Matrix 12 [_LightMatrix0]
"vs_3_0
; 51 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
def c19, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
mov r0.xyz, v1
mul r2.xyz, v2.zxyw, r0.yzxw
mov r1, c10
dp4 r3.z, c17, r1
mov r1, c9
mov r0.xyz, v1
mad r0.xyz, v2.yzxw, r0.zxyw, -r2
mov r2, c8
dp4 r3.x, c17, r2
mul r2.xyz, r0, v1.w
dp4 r3.y, c17, r1
mad r3.xyz, r3, c18.w, -v0
mov r0.xyz, c16
mov r0.w, c19.x
dp4 r1.z, r0, c10
dp4 r1.x, r0, c8
dp4 r1.y, r0, c9
mad r1.xyz, r1, c18.w, -v0
dp3 r0.y, r2, c4
dp3 r0.w, -r1, c4
dp3 r0.x, v1, c4
dp3 r0.z, v2, c4
mul o2, r0, c18.w
dp3 r0.y, r2, c5
dp3 r0.w, -r1, c5
dp3 r0.x, v1, c5
dp3 r0.z, v2, c5
mul o3, r0, c18.w
dp3 r0.y, r2, c6
dp3 r0.w, -r1, c6
dp3 r0.x, v1, c6
dp3 r0.z, v2, c6
mul o4, r0, c18.w
dp4 r0.w, v0, c7
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
dp3 o1.y, r1, r2
dp3 o5.y, r2, r3
dp3 o1.z, v2, r1
dp3 o1.x, r1, v1
dp3 o5.z, v2, r3
dp3 o5.x, v1, r3
dp4 o6.w, r0, c15
dp4 o6.z, r0, c14
dp4 o6.y, r0, c13
dp4 o6.x, r0, c12
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "d3d11 " {
Keywords { "SPOT" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "color" Color
ConstBuffer "$Globals" 176 // 112 used size, 11 vars
Matrix 48 [_LightMatrix0] 4
ConstBuffer "UnityPerCamera" 128 // 76 used size, 8 vars
Vector 64 [_WorldSpaceCameraPos] 3
ConstBuffer "UnityLighting" 720 // 16 used size, 17 vars
Vector 0 [_WorldSpaceLightPos0] 4
ConstBuffer "UnityPerDraw" 336 // 336 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
Matrix 192 [_Object2World] 4
Matrix 256 [_World2Object] 4
Vector 320 [unity_Scale] 4
BindCB "$Globals" 0
BindCB "UnityPerCamera" 1
BindCB "UnityLighting" 2
BindCB "UnityPerDraw" 3
// 58 instructions, 4 temp regs, 0 temp arrays:
// ALU 46 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefieceddpdkoopnblbhofnoinpeeecfjpkengjpabaaaaaaoaajaaaaadaaaaaa
cmaaaaaapeaaaaaameabaaaaejfdeheomaaaaaaaagaaaaaaaiaaaaaajiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaakbaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaakjaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
ahahaaaalaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaadaaaaaaapaaaaaalaaaaaaa
abaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaaljaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaafaaaaaaapaaaaaafaepfdejfeejepeoaafeebeoehefeofeaaeoepfc
enebemaafeeffiedepepfceeaaedepemepfcaaklepfdeheomiaaaaaaahaaaaaa
aiaaaaaalaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaalmaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaahaiaaaalmaaaaaaabaaaaaaaaaaaaaa
adaaaaaaacaaaaaaapaaaaaalmaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaa
apaaaaaalmaaaaaaadaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaalmaaaaaa
aeaaaaaaaaaaaaaaadaaaaaaafaaaaaaahaiaaaalmaaaaaaafaaaaaaaaaaaaaa
adaaaaaaagaaaaaaapaaaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklklfdeieefcbeaiaaaaeaaaabaaafacaaaafjaaaaaeegiocaaaaaaaaaaa
ahaaaaaafjaaaaaeegiocaaaabaaaaaaafaaaaaafjaaaaaeegiocaaaacaaaaaa
abaaaaaafjaaaaaeegiocaaaadaaaaaabfaaaaaafpaaaaadpcbabaaaaaaaaaaa
fpaaaaadpcbabaaaabaaaaaafpaaaaadhcbabaaaacaaaaaaghaaaaaepccabaaa
aaaaaaaaabaaaaaagfaaaaadhccabaaaabaaaaaagfaaaaadpccabaaaacaaaaaa
gfaaaaadpccabaaaadaaaaaagfaaaaadpccabaaaaeaaaaaagfaaaaadhccabaaa
afaaaaaagfaaaaadpccabaaaagaaaaaagiaaaaacaeaaaaaadiaaaaaipcaabaaa
aaaaaaaafgbfbaaaaaaaaaaaegiocaaaadaaaaaaabaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaadaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpcaabaaaaaaaaaaaegiocaaaadaaaaaaacaaaaaakgbkbaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaaegiocaaaadaaaaaaadaaaaaa
pgbpbaaaaaaaaaaaegaobaaaaaaaaaaadiaaaaajhcaabaaaaaaaaaaafgifcaaa
abaaaaaaaeaaaaaaegiccaaaadaaaaaabbaaaaaadcaaaaalhcaabaaaaaaaaaaa
egiccaaaadaaaaaabaaaaaaaagiacaaaabaaaaaaaeaaaaaaegacbaaaaaaaaaaa
dcaaaaalhcaabaaaaaaaaaaaegiccaaaadaaaaaabcaaaaaakgikcaaaabaaaaaa
aeaaaaaaegacbaaaaaaaaaaaaaaaaaaihcaabaaaaaaaaaaaegacbaaaaaaaaaaa
egiccaaaadaaaaaabdaaaaaadcaaaaalhcaabaaaaaaaaaaaegacbaaaaaaaaaaa
pgipcaaaadaaaaaabeaaaaaaegbcbaiaebaaaaaaaaaaaaaabaaaaaahbccabaaa
abaaaaaaegbcbaaaabaaaaaaegacbaaaaaaaaaaabaaaaaaheccabaaaabaaaaaa
egbcbaaaacaaaaaaegacbaaaaaaaaaaadiaaaaahhcaabaaaabaaaaaajgbebaaa
abaaaaaacgbjbaaaacaaaaaadcaaaaakhcaabaaaabaaaaaajgbebaaaacaaaaaa
cgbjbaaaabaaaaaaegacbaiaebaaaaaaabaaaaaadiaaaaahhcaabaaaabaaaaaa
egacbaaaabaaaaaapgbpbaaaabaaaaaabaaaaaahcccabaaaabaaaaaaegacbaaa
abaaaaaaegacbaaaaaaaaaaadiaaaaajhcaabaaaacaaaaaafgafbaiaebaaaaaa
aaaaaaaaegiccaaaadaaaaaaanaaaaaadcaaaaallcaabaaaaaaaaaaaegiicaaa
adaaaaaaamaaaaaaagaabaiaebaaaaaaaaaaaaaaegaibaaaacaaaaaadcaaaaal
lcaabaaaaaaaaaaaegiicaaaadaaaaaaaoaaaaaakgakbaiaebaaaaaaaaaaaaaa
egambaaaaaaaaaaadgaaaaaficaabaaaacaaaaaaakaabaaaaaaaaaaadgaaaaag
bcaabaaaadaaaaaaakiacaaaadaaaaaaamaaaaaadgaaaaagccaabaaaadaaaaaa
akiacaaaadaaaaaaanaaaaaadgaaaaagecaabaaaadaaaaaaakiacaaaadaaaaaa
aoaaaaaabaaaaaahccaabaaaacaaaaaaegacbaaaabaaaaaaegacbaaaadaaaaaa
baaaaaahbcaabaaaacaaaaaaegbcbaaaabaaaaaaegacbaaaadaaaaaabaaaaaah
ecaabaaaacaaaaaaegbcbaaaacaaaaaaegacbaaaadaaaaaadiaaaaaipccabaaa
acaaaaaaegaobaaaacaaaaaapgipcaaaadaaaaaabeaaaaaadgaaaaaficaabaaa
acaaaaaabkaabaaaaaaaaaaadgaaaaagbcaabaaaadaaaaaabkiacaaaadaaaaaa
amaaaaaadgaaaaagccaabaaaadaaaaaabkiacaaaadaaaaaaanaaaaaadgaaaaag
ecaabaaaadaaaaaabkiacaaaadaaaaaaaoaaaaaabaaaaaahccaabaaaacaaaaaa
egacbaaaabaaaaaaegacbaaaadaaaaaabaaaaaahbcaabaaaacaaaaaaegbcbaaa
abaaaaaaegacbaaaadaaaaaabaaaaaahecaabaaaacaaaaaaegbcbaaaacaaaaaa
egacbaaaadaaaaaadiaaaaaipccabaaaadaaaaaaegaobaaaacaaaaaapgipcaaa
adaaaaaabeaaaaaadgaaaaagbcaabaaaacaaaaaackiacaaaadaaaaaaamaaaaaa
dgaaaaagccaabaaaacaaaaaackiacaaaadaaaaaaanaaaaaadgaaaaagecaabaaa
acaaaaaackiacaaaadaaaaaaaoaaaaaabaaaaaahccaabaaaaaaaaaaaegacbaaa
abaaaaaaegacbaaaacaaaaaabaaaaaahbcaabaaaaaaaaaaaegbcbaaaabaaaaaa
egacbaaaacaaaaaabaaaaaahecaabaaaaaaaaaaaegbcbaaaacaaaaaaegacbaaa
acaaaaaadiaaaaaipccabaaaaeaaaaaaegaobaaaaaaaaaaapgipcaaaadaaaaaa
beaaaaaadiaaaaajhcaabaaaaaaaaaaafgifcaaaacaaaaaaaaaaaaaaegiccaaa
adaaaaaabbaaaaaadcaaaaalhcaabaaaaaaaaaaaegiccaaaadaaaaaabaaaaaaa
agiacaaaacaaaaaaaaaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaaaaaaaaa
egiccaaaadaaaaaabcaaaaaakgikcaaaacaaaaaaaaaaaaaaegacbaaaaaaaaaaa
dcaaaaalhcaabaaaaaaaaaaaegiccaaaadaaaaaabdaaaaaapgipcaaaacaaaaaa
aaaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaaaaaaaaaegacbaaaaaaaaaaa
pgipcaaaadaaaaaabeaaaaaaegbcbaiaebaaaaaaaaaaaaaabaaaaaahcccabaaa
afaaaaaaegacbaaaabaaaaaaegacbaaaaaaaaaaabaaaaaahbccabaaaafaaaaaa
egbcbaaaabaaaaaaegacbaaaaaaaaaaabaaaaaaheccabaaaafaaaaaaegbcbaaa
acaaaaaaegacbaaaaaaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaa
egiocaaaadaaaaaaanaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaadaaaaaa
amaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaa
egiocaaaadaaaaaaaoaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaak
pcaabaaaaaaaaaaaegiocaaaadaaaaaaapaaaaaapgbpbaaaaaaaaaaaegaobaaa
aaaaaaaadiaaaaaipcaabaaaabaaaaaafgafbaaaaaaaaaaaegiocaaaaaaaaaaa
aeaaaaaadcaaaaakpcaabaaaabaaaaaaegiocaaaaaaaaaaaadaaaaaaagaabaaa
aaaaaaaaegaobaaaabaaaaaadcaaaaakpcaabaaaabaaaaaaegiocaaaaaaaaaaa
afaaaaaakgakbaaaaaaaaaaaegaobaaaabaaaaaadcaaaaakpccabaaaagaaaaaa
egiocaaaaaaaaaaaagaaaaaapgapbaaaaaaaaaaaegaobaaaabaaaaaadoaaaaab
"
}

SubProgram "gles " {
Keywords { "SPOT" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp mat4 _LightMatrix0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (_glesVertex.xyz - ((_World2Object * tmpvar_7).xyz * unity_Scale.w)));
  highp vec3 tmpvar_10;
  highp vec3 tmpvar_11;
  tmpvar_10 = tmpvar_1.xyz;
  tmpvar_11 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_11.x;
  tmpvar_12[0].z = tmpvar_2.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_11.y;
  tmpvar_12[1].z = tmpvar_2.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_11.z;
  tmpvar_12[2].z = tmpvar_2.z;
  vec4 v_13;
  v_13.x = _Object2World[0].x;
  v_13.y = _Object2World[1].x;
  v_13.z = _Object2World[2].x;
  v_13.w = _Object2World[3].x;
  highp vec4 tmpvar_14;
  tmpvar_14.xyz = (tmpvar_12 * v_13.xyz);
  tmpvar_14.w = tmpvar_9.x;
  highp vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_14 * unity_Scale.w);
  tmpvar_3 = tmpvar_15;
  vec4 v_16;
  v_16.x = _Object2World[0].y;
  v_16.y = _Object2World[1].y;
  v_16.z = _Object2World[2].y;
  v_16.w = _Object2World[3].y;
  highp vec4 tmpvar_17;
  tmpvar_17.xyz = (tmpvar_12 * v_16.xyz);
  tmpvar_17.w = tmpvar_9.y;
  highp vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * unity_Scale.w);
  tmpvar_4 = tmpvar_18;
  vec4 v_19;
  v_19.x = _Object2World[0].z;
  v_19.y = _Object2World[1].z;
  v_19.z = _Object2World[2].z;
  v_19.w = _Object2World[3].z;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = (tmpvar_12 * v_19.xyz);
  tmpvar_20.w = tmpvar_9.z;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * unity_Scale.w);
  tmpvar_5 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_12 * (((_World2Object * _WorldSpaceLightPos0).xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_6 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = _WorldSpaceCameraPos;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_12 * (((_World2Object * tmpvar_23).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform lowp vec4 _LightColor0;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  mediump vec3 tmpvar_3;
  highp vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  mediump vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8.x = xlv_TEXCOORD1.w;
  tmpvar_8.y = xlv_TEXCOORD2.w;
  tmpvar_8.z = xlv_TEXCOORD3.w;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD1.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD2.xyz;
  tmpvar_6 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = xlv_TEXCOORD3.xyz;
  tmpvar_7 = tmpvar_11;
  tmpvar_3 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_12;
  lowp float tmpvar_13;
  mediump vec3 HDR_14;
  highp vec4 c_15;
  mediump vec3 tmpvar_16;
  tmpvar_16.x = tmpvar_5.z;
  tmpvar_16.y = tmpvar_6.z;
  tmpvar_16.z = tmpvar_7.z;
  highp vec3 tmpvar_17;
  tmpvar_17 = (tmpvar_4 - (2.0 * (dot (tmpvar_16, tmpvar_4) * tmpvar_16)));
  lowp vec4 tmpvar_18;
  tmpvar_18 = textureCube (_Cube, tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_18 * _ReflectColor);
  c_15 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = vec3((pow ((((c_15.x + c_15.y) + c_15.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_14 = tmpvar_20;
  mediump float tmpvar_21;
  tmpvar_21 = (1.0 - dot (normalize(tmpvar_3), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_22;
  tmpvar_22 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_21, _TransmissiveColor)));
  mediump float tmpvar_23;
  tmpvar_23 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_21)).x;
  tmpvar_13 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = ((c_15.xyz + HDR_14) + tmpvar_22.xyz);
  tmpvar_12 = tmpvar_24;
  mediump vec3 tmpvar_25;
  tmpvar_25 = normalize(xlv_TEXCOORD4);
  lightDir_2 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = normalize(xlv_TEXCOORD0);
  lowp vec4 tmpvar_27;
  highp vec2 P_28;
  P_28 = ((xlv_TEXCOORD5.xy / xlv_TEXCOORD5.w) + 0.5);
  tmpvar_27 = texture2D (_LightTexture0, P_28);
  highp float tmpvar_29;
  tmpvar_29 = dot (xlv_TEXCOORD5.xyz, xlv_TEXCOORD5.xyz);
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_LightTextureB0, vec2(tmpvar_29));
  mediump vec3 lightDir_31;
  lightDir_31 = lightDir_2;
  mediump vec3 viewDir_32;
  viewDir_32 = tmpvar_26;
  mediump float atten_33;
  atten_33 = ((float((xlv_TEXCOORD5.z > 0.0)) * tmpvar_27.w) * tmpvar_30.w);
  mediump vec4 c_34;
  c_34.xyz = (((((((lightDir_31.z * 0.5) + 0.5) * tmpvar_12) * _LightColor0.xyz) * atten_33) * 2.0) + ((_LightColor0.xyz * max (0.0, (pow ((viewDir_32 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_13))) * atten_33));
  c_34.w = 1.0;
  c_1.xyz = c_34.xyz;
  c_1.w = 0.0;
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "SPOT" }
"!!GLES


#ifdef VERTEX

varying highp vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp mat4 _LightMatrix0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (_glesVertex.xyz - ((_World2Object * tmpvar_7).xyz * unity_Scale.w)));
  highp vec3 tmpvar_10;
  highp vec3 tmpvar_11;
  tmpvar_10 = tmpvar_1.xyz;
  tmpvar_11 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_11.x;
  tmpvar_12[0].z = tmpvar_2.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_11.y;
  tmpvar_12[1].z = tmpvar_2.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_11.z;
  tmpvar_12[2].z = tmpvar_2.z;
  vec4 v_13;
  v_13.x = _Object2World[0].x;
  v_13.y = _Object2World[1].x;
  v_13.z = _Object2World[2].x;
  v_13.w = _Object2World[3].x;
  highp vec4 tmpvar_14;
  tmpvar_14.xyz = (tmpvar_12 * v_13.xyz);
  tmpvar_14.w = tmpvar_9.x;
  highp vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_14 * unity_Scale.w);
  tmpvar_3 = tmpvar_15;
  vec4 v_16;
  v_16.x = _Object2World[0].y;
  v_16.y = _Object2World[1].y;
  v_16.z = _Object2World[2].y;
  v_16.w = _Object2World[3].y;
  highp vec4 tmpvar_17;
  tmpvar_17.xyz = (tmpvar_12 * v_16.xyz);
  tmpvar_17.w = tmpvar_9.y;
  highp vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * unity_Scale.w);
  tmpvar_4 = tmpvar_18;
  vec4 v_19;
  v_19.x = _Object2World[0].z;
  v_19.y = _Object2World[1].z;
  v_19.z = _Object2World[2].z;
  v_19.w = _Object2World[3].z;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = (tmpvar_12 * v_19.xyz);
  tmpvar_20.w = tmpvar_9.z;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * unity_Scale.w);
  tmpvar_5 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_12 * (((_World2Object * _WorldSpaceLightPos0).xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_6 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = _WorldSpaceCameraPos;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_12 * (((_World2Object * tmpvar_23).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform lowp vec4 _LightColor0;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  mediump vec3 tmpvar_3;
  highp vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  mediump vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8.x = xlv_TEXCOORD1.w;
  tmpvar_8.y = xlv_TEXCOORD2.w;
  tmpvar_8.z = xlv_TEXCOORD3.w;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD1.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD2.xyz;
  tmpvar_6 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = xlv_TEXCOORD3.xyz;
  tmpvar_7 = tmpvar_11;
  tmpvar_3 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_12;
  lowp float tmpvar_13;
  mediump vec3 HDR_14;
  highp vec4 c_15;
  mediump vec3 tmpvar_16;
  tmpvar_16.x = tmpvar_5.z;
  tmpvar_16.y = tmpvar_6.z;
  tmpvar_16.z = tmpvar_7.z;
  highp vec3 tmpvar_17;
  tmpvar_17 = (tmpvar_4 - (2.0 * (dot (tmpvar_16, tmpvar_4) * tmpvar_16)));
  lowp vec4 tmpvar_18;
  tmpvar_18 = textureCube (_Cube, tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_18 * _ReflectColor);
  c_15 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = vec3((pow ((((c_15.x + c_15.y) + c_15.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_14 = tmpvar_20;
  mediump float tmpvar_21;
  tmpvar_21 = (1.0 - dot (normalize(tmpvar_3), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_22;
  tmpvar_22 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_21, _TransmissiveColor)));
  mediump float tmpvar_23;
  tmpvar_23 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_21)).x;
  tmpvar_13 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = ((c_15.xyz + HDR_14) + tmpvar_22.xyz);
  tmpvar_12 = tmpvar_24;
  mediump vec3 tmpvar_25;
  tmpvar_25 = normalize(xlv_TEXCOORD4);
  lightDir_2 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = normalize(xlv_TEXCOORD0);
  lowp vec4 tmpvar_27;
  highp vec2 P_28;
  P_28 = ((xlv_TEXCOORD5.xy / xlv_TEXCOORD5.w) + 0.5);
  tmpvar_27 = texture2D (_LightTexture0, P_28);
  highp float tmpvar_29;
  tmpvar_29 = dot (xlv_TEXCOORD5.xyz, xlv_TEXCOORD5.xyz);
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_LightTextureB0, vec2(tmpvar_29));
  mediump vec3 lightDir_31;
  lightDir_31 = lightDir_2;
  mediump vec3 viewDir_32;
  viewDir_32 = tmpvar_26;
  mediump float atten_33;
  atten_33 = ((float((xlv_TEXCOORD5.z > 0.0)) * tmpvar_27.w) * tmpvar_30.w);
  mediump vec4 c_34;
  c_34.xyz = (((((((lightDir_31.z * 0.5) + 0.5) * tmpvar_12) * _LightColor0.xyz) * atten_33) * 2.0) + ((_LightColor0.xyz * max (0.0, (pow ((viewDir_32 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_13))) * atten_33));
  c_34.w = 1.0;
  c_1.xyz = c_34.xyz;
  c_1.w = 0.0;
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "SPOT" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Color _glesColor
in vec4 _glesColor;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;
#define gl_MultiTexCoord1 _glesMultiTexCoord1
in vec4 _glesMultiTexCoord1;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
mat2 xll_transpose_mf2x2(mat2 m) {
  return mat2( m[0][0], m[1][0], m[0][1], m[1][1]);
}
mat3 xll_transpose_mf3x3(mat3 m) {
  return mat3( m[0][0], m[1][0], m[2][0],
               m[0][1], m[1][1], m[2][1],
               m[0][2], m[1][2], m[2][2]);
}
mat4 xll_transpose_mf4x4(mat4 m) {
  return mat4( m[0][0], m[1][0], m[2][0], m[3][0],
               m[0][1], m[1][1], m[2][1], m[3][1],
               m[0][2], m[1][2], m[2][2], m[3][2],
               m[0][3], m[1][3], m[2][3], m[3][3]);
}
vec2 xll_matrixindex_mf2x2_i (mat2 m, int i) { vec2 v; v.x=m[0][i]; v.y=m[1][i]; return v; }
vec3 xll_matrixindex_mf3x3_i (mat3 m, int i) { vec3 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; return v; }
vec4 xll_matrixindex_mf4x4_i (mat4 m, int i) { vec4 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; v.w=m[3][i]; return v; }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 410
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 443
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    mediump vec3 lightDir;
    highp vec4 _LightCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform highp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform sampler2D _LightTexture0;
uniform highp mat4 _LightMatrix0;
#line 393
uniform sampler2D _LightTextureB0;
#line 398
#line 402
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
#line 406
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 419
#line 454
#line 82
highp vec3 ObjSpaceLightDir( in highp vec4 v ) {
    highp vec3 objSpaceLightPos = (_World2Object * _WorldSpaceLightPos0).xyz;
    return ((objSpaceLightPos.xyz * unity_Scale.w) - v.xyz);
}
#line 91
highp vec3 ObjSpaceViewDir( in highp vec4 v ) {
    highp vec3 objSpaceCameraPos = ((_World2Object * vec4( _WorldSpaceCameraPos.xyz, 1.0)).xyz * unity_Scale.w);
    return (objSpaceCameraPos - v.xyz);
}
#line 454
v2f_surf vert_surf( in appdata_full v ) {
    v2f_surf o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 458
    highp vec3 viewDir = (-ObjSpaceViewDir( v.vertex));
    highp vec3 worldRefl = (mat3( _Object2World) * viewDir);
    highp vec3 binormal = (cross( v.normal, v.tangent.xyz) * v.tangent.w);
    highp mat3 rotation = xll_transpose_mf3x3(mat3( v.tangent.xyz, binormal, v.normal));
    #line 462
    o.TtoW0 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 0).xyz), worldRefl.x) * unity_Scale.w);
    o.TtoW1 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 1).xyz), worldRefl.y) * unity_Scale.w);
    o.TtoW2 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 2).xyz), worldRefl.z) * unity_Scale.w);
    highp vec3 lightDir = (rotation * ObjSpaceLightDir( v.vertex));
    #line 466
    o.lightDir = lightDir;
    highp vec3 viewDirForLight = (rotation * ObjSpaceViewDir( v.vertex));
    o.viewDir = viewDirForLight;
    o._LightCoord = (_LightMatrix0 * (_Object2World * v.vertex));
    #line 471
    return o;
}

out highp vec3 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_TEXCOORD2;
out lowp vec4 xlv_TEXCOORD3;
out mediump vec3 xlv_TEXCOORD4;
out highp vec4 xlv_TEXCOORD5;
void main() {
    v2f_surf xl_retval;
    appdata_full xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.tangent = vec4(TANGENT);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xlt_v.texcoord1 = vec4(gl_MultiTexCoord1);
    xlt_v.color = vec4(gl_Color);
    xl_retval = vert_surf( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD0 = vec3(xl_retval.viewDir);
    xlv_TEXCOORD1 = vec4(xl_retval.TtoW0);
    xlv_TEXCOORD2 = vec4(xl_retval.TtoW1);
    xlv_TEXCOORD3 = vec4(xl_retval.TtoW2);
    xlv_TEXCOORD4 = vec3(xl_retval.lightDir);
    xlv_TEXCOORD5 = vec4(xl_retval._LightCoord);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 410
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 443
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    mediump vec3 lightDir;
    highp vec4 _LightCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform highp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform sampler2D _LightTexture0;
uniform highp mat4 _LightMatrix0;
#line 393
uniform sampler2D _LightTextureB0;
#line 398
#line 402
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
#line 406
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 419
#line 454
#line 433
mediump vec4 LightingHalfLambertSpecular( in SurfaceOutput s, in mediump vec3 lightDir, in mediump vec3 viewDir, in mediump float atten ) {
    #line 435
    mediump float NdotL = ((dot( s.Normal, lightDir) * 0.5) + 0.5);
    mediump vec3 halfVector = (viewDir + s.Normal);
    mediump float spec = max( 0.0, (pow( dot( halfVector, s.Normal), (10.0 - s.Specular)) * s.Gloss));
    mediump vec4 c;
    #line 439
    c.xyz = (((((NdotL * s.Albedo) * _LightColor0.xyz) * atten) * 2.0) + ((_LightColor0.xyz * spec) * atten));
    c.w = s.Alpha;
    return c;
}
#line 398
lowp float UnitySpotAttenuate( in highp vec3 LightCoord ) {
    return texture( _LightTextureB0, vec2( dot( LightCoord, LightCoord))).w;
}
#line 394
lowp float UnitySpotCookie( in highp vec4 LightCoord ) {
    return texture( _LightTexture0, ((LightCoord.xy / LightCoord.w) + 0.5)).w;
}
#line 419
void surf( in Input IN, inout SurfaceOutput o ) {
    o.Normal = vec3( 0.0, 0.0, 1.0);
    highp vec3 worldRefl = reflect( IN.worldRefl, vec3( dot( IN.TtoW0, o.Normal), dot( IN.TtoW1, o.Normal), dot( IN.TtoW2, o.Normal)));
    #line 423
    highp vec4 c = (texture( _Cube, worldRefl) * _ReflectColor);
    mediump vec3 HDR = vec3( (pow( (((c.x + c.y) + c.z) / 3.0), _HdrPower) * _HdrContrast));
    mediump float fresnel = (1.0 - dot( normalize(IN.viewDir), normalize(vec3( 0.0, 0.0, 1.0))));
    mediump vec4 fresnelLerp = mix( _Color, _FresnelColor, vec4( pow( fresnel, _TransmissiveColor)));
    #line 427
    mediump vec4 GlossLerp = mix( vec4( 0.0, 0.0, 0.0, 0.0), vec4( 1.0, 1.0, 1.0, 1.0), vec4( fresnel));
    o.Specular = _Shininess;
    o.Gloss = GlossLerp.x;
    o.Albedo = ((c.xyz + HDR) + fresnelLerp.xyz);
    #line 431
    o.Alpha = 1.0;
}
#line 473
lowp vec4 frag_surf( in v2f_surf IN ) {
    #line 475
    Input surfIN;
    surfIN.worldRefl = vec3( IN.TtoW0.w, IN.TtoW1.w, IN.TtoW2.w);
    surfIN.TtoW0 = IN.TtoW0.xyz;
    surfIN.TtoW1 = IN.TtoW1.xyz;
    #line 479
    surfIN.TtoW2 = IN.TtoW2.xyz;
    surfIN.viewDir = IN.viewDir;
    SurfaceOutput o;
    o.Albedo = vec3( 0.0);
    #line 483
    o.Emission = vec3( 0.0);
    o.Specular = 0.0;
    o.Alpha = 0.0;
    o.Gloss = 0.0;
    #line 487
    surf( surfIN, o);
    lowp vec3 lightDir = normalize(IN.lightDir);
    lowp vec4 c = LightingHalfLambertSpecular( o, lightDir, normalize(IN.viewDir), (((float((IN._LightCoord.z > 0.0)) * UnitySpotCookie( IN._LightCoord)) * UnitySpotAttenuate( IN._LightCoord.xyz)) * 1.0));
    c.w = 0.0;
    #line 491
    return c;
}
in highp vec3 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_TEXCOORD2;
in lowp vec4 xlv_TEXCOORD3;
in mediump vec3 xlv_TEXCOORD4;
in highp vec4 xlv_TEXCOORD5;
void main() {
    lowp vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.viewDir = vec3(xlv_TEXCOORD0);
    xlt_IN.TtoW0 = vec4(xlv_TEXCOORD1);
    xlt_IN.TtoW1 = vec4(xlv_TEXCOORD2);
    xlt_IN.TtoW2 = vec4(xlv_TEXCOORD3);
    xlt_IN.lightDir = vec3(xlv_TEXCOORD4);
    xlt_IN._LightCoord = vec4(xlv_TEXCOORD5);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "opengl " {
Keywords { "POINT_COOKIE" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Vector 17 [_WorldSpaceCameraPos]
Vector 18 [_WorldSpaceLightPos0]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 19 [unity_Scale]
Matrix 13 [_LightMatrix0]
"3.0-!!ARBvp1.0
# 47 ALU
PARAM c[20] = { { 1 },
		state.matrix.mvp,
		program.local[5..19] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R0.xyz, vertex.attrib[14];
MUL R2.xyz, vertex.normal.zxyw, R0.yzxw;
MOV R1, c[18];
MAD R0.xyz, vertex.normal.yzxw, R0.zxyw, -R2;
DP4 R2.z, R1, c[11];
DP4 R2.x, R1, c[9];
DP4 R2.y, R1, c[10];
MAD R3.xyz, R2, c[19].w, -vertex.position;
MUL R2.xyz, R0, vertex.attrib[14].w;
MOV R0.xyz, c[17];
MOV R0.w, c[0].x;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
MAD R1.xyz, R1, c[19].w, -vertex.position;
DP3 R0.y, R2, c[5];
DP3 R0.w, -R1, c[5];
DP3 R0.x, vertex.attrib[14], c[5];
DP3 R0.z, vertex.normal, c[5];
MUL result.texcoord[1], R0, c[19].w;
DP3 R0.y, R2, c[6];
DP3 R0.w, -R1, c[6];
DP3 R0.x, vertex.attrib[14], c[6];
DP3 R0.z, vertex.normal, c[6];
MUL result.texcoord[2], R0, c[19].w;
DP3 R0.y, R2, c[7];
DP3 R0.w, -R1, c[7];
DP3 R0.x, vertex.attrib[14], c[7];
DP3 R0.z, vertex.normal, c[7];
MUL result.texcoord[3], R0, c[19].w;
DP4 R0.w, vertex.position, c[8];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP3 result.texcoord[0].y, R1, R2;
DP3 result.texcoord[4].y, R2, R3;
DP3 result.texcoord[0].z, vertex.normal, R1;
DP3 result.texcoord[0].x, R1, vertex.attrib[14];
DP3 result.texcoord[4].z, vertex.normal, R3;
DP3 result.texcoord[4].x, vertex.attrib[14], R3;
DP4 result.texcoord[5].z, R0, c[15];
DP4 result.texcoord[5].y, R0, c[14];
DP4 result.texcoord[5].x, R0, c[13];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 47 instructions, 4 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "POINT_COOKIE" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Matrix 0 [glstate_matrix_mvp]
Vector 16 [_WorldSpaceCameraPos]
Vector 17 [_WorldSpaceLightPos0]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 18 [unity_Scale]
Matrix 12 [_LightMatrix0]
"vs_3_0
; 50 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
def c19, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
mov r0.xyz, v1
mul r2.xyz, v2.zxyw, r0.yzxw
mov r1, c10
dp4 r3.z, c17, r1
mov r1, c9
mov r0.xyz, v1
mad r0.xyz, v2.yzxw, r0.zxyw, -r2
mov r2, c8
dp4 r3.x, c17, r2
mul r2.xyz, r0, v1.w
dp4 r3.y, c17, r1
mad r3.xyz, r3, c18.w, -v0
mov r0.xyz, c16
mov r0.w, c19.x
dp4 r1.z, r0, c10
dp4 r1.x, r0, c8
dp4 r1.y, r0, c9
mad r1.xyz, r1, c18.w, -v0
dp3 r0.y, r2, c4
dp3 r0.w, -r1, c4
dp3 r0.x, v1, c4
dp3 r0.z, v2, c4
mul o2, r0, c18.w
dp3 r0.y, r2, c5
dp3 r0.w, -r1, c5
dp3 r0.x, v1, c5
dp3 r0.z, v2, c5
mul o3, r0, c18.w
dp3 r0.y, r2, c6
dp3 r0.w, -r1, c6
dp3 r0.x, v1, c6
dp3 r0.z, v2, c6
mul o4, r0, c18.w
dp4 r0.w, v0, c7
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
dp3 o1.y, r1, r2
dp3 o5.y, r2, r3
dp3 o1.z, v2, r1
dp3 o1.x, r1, v1
dp3 o5.z, v2, r3
dp3 o5.x, v1, r3
dp4 o6.z, r0, c14
dp4 o6.y, r0, c13
dp4 o6.x, r0, c12
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "d3d11 " {
Keywords { "POINT_COOKIE" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "color" Color
ConstBuffer "$Globals" 176 // 112 used size, 11 vars
Matrix 48 [_LightMatrix0] 4
ConstBuffer "UnityPerCamera" 128 // 76 used size, 8 vars
Vector 64 [_WorldSpaceCameraPos] 3
ConstBuffer "UnityLighting" 720 // 16 used size, 17 vars
Vector 0 [_WorldSpaceLightPos0] 4
ConstBuffer "UnityPerDraw" 336 // 336 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
Matrix 192 [_Object2World] 4
Matrix 256 [_World2Object] 4
Vector 320 [unity_Scale] 4
BindCB "$Globals" 0
BindCB "UnityPerCamera" 1
BindCB "UnityLighting" 2
BindCB "UnityPerDraw" 3
// 58 instructions, 4 temp regs, 0 temp arrays:
// ALU 46 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedonannpmdciocmbbbknokjjjhihhgbcfkabaaaaaaoaajaaaaadaaaaaa
cmaaaaaapeaaaaaameabaaaaejfdeheomaaaaaaaagaaaaaaaiaaaaaajiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaakbaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaakjaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
ahahaaaalaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaadaaaaaaapaaaaaalaaaaaaa
abaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaaljaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaafaaaaaaapaaaaaafaepfdejfeejepeoaafeebeoehefeofeaaeoepfc
enebemaafeeffiedepepfceeaaedepemepfcaaklepfdeheomiaaaaaaahaaaaaa
aiaaaaaalaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaalmaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaahaiaaaalmaaaaaaabaaaaaaaaaaaaaa
adaaaaaaacaaaaaaapaaaaaalmaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaa
apaaaaaalmaaaaaaadaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaalmaaaaaa
aeaaaaaaaaaaaaaaadaaaaaaafaaaaaaahaiaaaalmaaaaaaafaaaaaaaaaaaaaa
adaaaaaaagaaaaaaahaiaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklklfdeieefcbeaiaaaaeaaaabaaafacaaaafjaaaaaeegiocaaaaaaaaaaa
ahaaaaaafjaaaaaeegiocaaaabaaaaaaafaaaaaafjaaaaaeegiocaaaacaaaaaa
abaaaaaafjaaaaaeegiocaaaadaaaaaabfaaaaaafpaaaaadpcbabaaaaaaaaaaa
fpaaaaadpcbabaaaabaaaaaafpaaaaadhcbabaaaacaaaaaaghaaaaaepccabaaa
aaaaaaaaabaaaaaagfaaaaadhccabaaaabaaaaaagfaaaaadpccabaaaacaaaaaa
gfaaaaadpccabaaaadaaaaaagfaaaaadpccabaaaaeaaaaaagfaaaaadhccabaaa
afaaaaaagfaaaaadhccabaaaagaaaaaagiaaaaacaeaaaaaadiaaaaaipcaabaaa
aaaaaaaafgbfbaaaaaaaaaaaegiocaaaadaaaaaaabaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaadaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpcaabaaaaaaaaaaaegiocaaaadaaaaaaacaaaaaakgbkbaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaaegiocaaaadaaaaaaadaaaaaa
pgbpbaaaaaaaaaaaegaobaaaaaaaaaaadiaaaaajhcaabaaaaaaaaaaafgifcaaa
abaaaaaaaeaaaaaaegiccaaaadaaaaaabbaaaaaadcaaaaalhcaabaaaaaaaaaaa
egiccaaaadaaaaaabaaaaaaaagiacaaaabaaaaaaaeaaaaaaegacbaaaaaaaaaaa
dcaaaaalhcaabaaaaaaaaaaaegiccaaaadaaaaaabcaaaaaakgikcaaaabaaaaaa
aeaaaaaaegacbaaaaaaaaaaaaaaaaaaihcaabaaaaaaaaaaaegacbaaaaaaaaaaa
egiccaaaadaaaaaabdaaaaaadcaaaaalhcaabaaaaaaaaaaaegacbaaaaaaaaaaa
pgipcaaaadaaaaaabeaaaaaaegbcbaiaebaaaaaaaaaaaaaabaaaaaahbccabaaa
abaaaaaaegbcbaaaabaaaaaaegacbaaaaaaaaaaabaaaaaaheccabaaaabaaaaaa
egbcbaaaacaaaaaaegacbaaaaaaaaaaadiaaaaahhcaabaaaabaaaaaajgbebaaa
abaaaaaacgbjbaaaacaaaaaadcaaaaakhcaabaaaabaaaaaajgbebaaaacaaaaaa
cgbjbaaaabaaaaaaegacbaiaebaaaaaaabaaaaaadiaaaaahhcaabaaaabaaaaaa
egacbaaaabaaaaaapgbpbaaaabaaaaaabaaaaaahcccabaaaabaaaaaaegacbaaa
abaaaaaaegacbaaaaaaaaaaadiaaaaajhcaabaaaacaaaaaafgafbaiaebaaaaaa
aaaaaaaaegiccaaaadaaaaaaanaaaaaadcaaaaallcaabaaaaaaaaaaaegiicaaa
adaaaaaaamaaaaaaagaabaiaebaaaaaaaaaaaaaaegaibaaaacaaaaaadcaaaaal
lcaabaaaaaaaaaaaegiicaaaadaaaaaaaoaaaaaakgakbaiaebaaaaaaaaaaaaaa
egambaaaaaaaaaaadgaaaaaficaabaaaacaaaaaaakaabaaaaaaaaaaadgaaaaag
bcaabaaaadaaaaaaakiacaaaadaaaaaaamaaaaaadgaaaaagccaabaaaadaaaaaa
akiacaaaadaaaaaaanaaaaaadgaaaaagecaabaaaadaaaaaaakiacaaaadaaaaaa
aoaaaaaabaaaaaahccaabaaaacaaaaaaegacbaaaabaaaaaaegacbaaaadaaaaaa
baaaaaahbcaabaaaacaaaaaaegbcbaaaabaaaaaaegacbaaaadaaaaaabaaaaaah
ecaabaaaacaaaaaaegbcbaaaacaaaaaaegacbaaaadaaaaaadiaaaaaipccabaaa
acaaaaaaegaobaaaacaaaaaapgipcaaaadaaaaaabeaaaaaadgaaaaaficaabaaa
acaaaaaabkaabaaaaaaaaaaadgaaaaagbcaabaaaadaaaaaabkiacaaaadaaaaaa
amaaaaaadgaaaaagccaabaaaadaaaaaabkiacaaaadaaaaaaanaaaaaadgaaaaag
ecaabaaaadaaaaaabkiacaaaadaaaaaaaoaaaaaabaaaaaahccaabaaaacaaaaaa
egacbaaaabaaaaaaegacbaaaadaaaaaabaaaaaahbcaabaaaacaaaaaaegbcbaaa
abaaaaaaegacbaaaadaaaaaabaaaaaahecaabaaaacaaaaaaegbcbaaaacaaaaaa
egacbaaaadaaaaaadiaaaaaipccabaaaadaaaaaaegaobaaaacaaaaaapgipcaaa
adaaaaaabeaaaaaadgaaaaagbcaabaaaacaaaaaackiacaaaadaaaaaaamaaaaaa
dgaaaaagccaabaaaacaaaaaackiacaaaadaaaaaaanaaaaaadgaaaaagecaabaaa
acaaaaaackiacaaaadaaaaaaaoaaaaaabaaaaaahccaabaaaaaaaaaaaegacbaaa
abaaaaaaegacbaaaacaaaaaabaaaaaahbcaabaaaaaaaaaaaegbcbaaaabaaaaaa
egacbaaaacaaaaaabaaaaaahecaabaaaaaaaaaaaegbcbaaaacaaaaaaegacbaaa
acaaaaaadiaaaaaipccabaaaaeaaaaaaegaobaaaaaaaaaaapgipcaaaadaaaaaa
beaaaaaadiaaaaajhcaabaaaaaaaaaaafgifcaaaacaaaaaaaaaaaaaaegiccaaa
adaaaaaabbaaaaaadcaaaaalhcaabaaaaaaaaaaaegiccaaaadaaaaaabaaaaaaa
agiacaaaacaaaaaaaaaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaaaaaaaaa
egiccaaaadaaaaaabcaaaaaakgikcaaaacaaaaaaaaaaaaaaegacbaaaaaaaaaaa
dcaaaaalhcaabaaaaaaaaaaaegiccaaaadaaaaaabdaaaaaapgipcaaaacaaaaaa
aaaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaaaaaaaaaegacbaaaaaaaaaaa
pgipcaaaadaaaaaabeaaaaaaegbcbaiaebaaaaaaaaaaaaaabaaaaaahcccabaaa
afaaaaaaegacbaaaabaaaaaaegacbaaaaaaaaaaabaaaaaahbccabaaaafaaaaaa
egbcbaaaabaaaaaaegacbaaaaaaaaaaabaaaaaaheccabaaaafaaaaaaegbcbaaa
acaaaaaaegacbaaaaaaaaaaadiaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaa
egiocaaaadaaaaaaanaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaadaaaaaa
amaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaa
egiocaaaadaaaaaaaoaaaaaakgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaak
pcaabaaaaaaaaaaaegiocaaaadaaaaaaapaaaaaapgbpbaaaaaaaaaaaegaobaaa
aaaaaaaadiaaaaaihcaabaaaabaaaaaafgafbaaaaaaaaaaaegiccaaaaaaaaaaa
aeaaaaaadcaaaaakhcaabaaaabaaaaaaegiccaaaaaaaaaaaadaaaaaaagaabaaa
aaaaaaaaegacbaaaabaaaaaadcaaaaakhcaabaaaaaaaaaaaegiccaaaaaaaaaaa
afaaaaaakgakbaaaaaaaaaaaegacbaaaabaaaaaadcaaaaakhccabaaaagaaaaaa
egiccaaaaaaaaaaaagaaaaaapgapbaaaaaaaaaaaegacbaaaaaaaaaaadoaaaaab
"
}

SubProgram "gles " {
Keywords { "POINT_COOKIE" }
"!!GLES


#ifdef VERTEX

varying highp vec3 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp mat4 _LightMatrix0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (_glesVertex.xyz - ((_World2Object * tmpvar_7).xyz * unity_Scale.w)));
  highp vec3 tmpvar_10;
  highp vec3 tmpvar_11;
  tmpvar_10 = tmpvar_1.xyz;
  tmpvar_11 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_11.x;
  tmpvar_12[0].z = tmpvar_2.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_11.y;
  tmpvar_12[1].z = tmpvar_2.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_11.z;
  tmpvar_12[2].z = tmpvar_2.z;
  vec4 v_13;
  v_13.x = _Object2World[0].x;
  v_13.y = _Object2World[1].x;
  v_13.z = _Object2World[2].x;
  v_13.w = _Object2World[3].x;
  highp vec4 tmpvar_14;
  tmpvar_14.xyz = (tmpvar_12 * v_13.xyz);
  tmpvar_14.w = tmpvar_9.x;
  highp vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_14 * unity_Scale.w);
  tmpvar_3 = tmpvar_15;
  vec4 v_16;
  v_16.x = _Object2World[0].y;
  v_16.y = _Object2World[1].y;
  v_16.z = _Object2World[2].y;
  v_16.w = _Object2World[3].y;
  highp vec4 tmpvar_17;
  tmpvar_17.xyz = (tmpvar_12 * v_16.xyz);
  tmpvar_17.w = tmpvar_9.y;
  highp vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * unity_Scale.w);
  tmpvar_4 = tmpvar_18;
  vec4 v_19;
  v_19.x = _Object2World[0].z;
  v_19.y = _Object2World[1].z;
  v_19.z = _Object2World[2].z;
  v_19.w = _Object2World[3].z;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = (tmpvar_12 * v_19.xyz);
  tmpvar_20.w = tmpvar_9.z;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * unity_Scale.w);
  tmpvar_5 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_12 * (((_World2Object * _WorldSpaceLightPos0).xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_6 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = _WorldSpaceCameraPos;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_12 * (((_World2Object * tmpvar_23).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * _glesVertex)).xyz;
}



#endif
#ifdef FRAGMENT

varying highp vec3 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform sampler2D _LightTextureB0;
uniform samplerCube _LightTexture0;
uniform lowp vec4 _LightColor0;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  mediump vec3 tmpvar_3;
  highp vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  mediump vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8.x = xlv_TEXCOORD1.w;
  tmpvar_8.y = xlv_TEXCOORD2.w;
  tmpvar_8.z = xlv_TEXCOORD3.w;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD1.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD2.xyz;
  tmpvar_6 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = xlv_TEXCOORD3.xyz;
  tmpvar_7 = tmpvar_11;
  tmpvar_3 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_12;
  lowp float tmpvar_13;
  mediump vec3 HDR_14;
  highp vec4 c_15;
  mediump vec3 tmpvar_16;
  tmpvar_16.x = tmpvar_5.z;
  tmpvar_16.y = tmpvar_6.z;
  tmpvar_16.z = tmpvar_7.z;
  highp vec3 tmpvar_17;
  tmpvar_17 = (tmpvar_4 - (2.0 * (dot (tmpvar_16, tmpvar_4) * tmpvar_16)));
  lowp vec4 tmpvar_18;
  tmpvar_18 = textureCube (_Cube, tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_18 * _ReflectColor);
  c_15 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = vec3((pow ((((c_15.x + c_15.y) + c_15.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_14 = tmpvar_20;
  mediump float tmpvar_21;
  tmpvar_21 = (1.0 - dot (normalize(tmpvar_3), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_22;
  tmpvar_22 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_21, _TransmissiveColor)));
  mediump float tmpvar_23;
  tmpvar_23 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_21)).x;
  tmpvar_13 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = ((c_15.xyz + HDR_14) + tmpvar_22.xyz);
  tmpvar_12 = tmpvar_24;
  mediump vec3 tmpvar_25;
  tmpvar_25 = normalize(xlv_TEXCOORD4);
  lightDir_2 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = normalize(xlv_TEXCOORD0);
  highp float tmpvar_27;
  tmpvar_27 = dot (xlv_TEXCOORD5, xlv_TEXCOORD5);
  lowp vec4 tmpvar_28;
  tmpvar_28 = texture2D (_LightTextureB0, vec2(tmpvar_27));
  lowp vec4 tmpvar_29;
  tmpvar_29 = textureCube (_LightTexture0, xlv_TEXCOORD5);
  mediump vec3 lightDir_30;
  lightDir_30 = lightDir_2;
  mediump vec3 viewDir_31;
  viewDir_31 = tmpvar_26;
  mediump float atten_32;
  atten_32 = (tmpvar_28.w * tmpvar_29.w);
  mediump vec4 c_33;
  c_33.xyz = (((((((lightDir_30.z * 0.5) + 0.5) * tmpvar_12) * _LightColor0.xyz) * atten_32) * 2.0) + ((_LightColor0.xyz * max (0.0, (pow ((viewDir_31 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_13))) * atten_32));
  c_33.w = 1.0;
  c_1.xyz = c_33.xyz;
  c_1.w = 0.0;
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "POINT_COOKIE" }
"!!GLES


#ifdef VERTEX

varying highp vec3 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp mat4 _LightMatrix0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (_glesVertex.xyz - ((_World2Object * tmpvar_7).xyz * unity_Scale.w)));
  highp vec3 tmpvar_10;
  highp vec3 tmpvar_11;
  tmpvar_10 = tmpvar_1.xyz;
  tmpvar_11 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_11.x;
  tmpvar_12[0].z = tmpvar_2.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_11.y;
  tmpvar_12[1].z = tmpvar_2.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_11.z;
  tmpvar_12[2].z = tmpvar_2.z;
  vec4 v_13;
  v_13.x = _Object2World[0].x;
  v_13.y = _Object2World[1].x;
  v_13.z = _Object2World[2].x;
  v_13.w = _Object2World[3].x;
  highp vec4 tmpvar_14;
  tmpvar_14.xyz = (tmpvar_12 * v_13.xyz);
  tmpvar_14.w = tmpvar_9.x;
  highp vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_14 * unity_Scale.w);
  tmpvar_3 = tmpvar_15;
  vec4 v_16;
  v_16.x = _Object2World[0].y;
  v_16.y = _Object2World[1].y;
  v_16.z = _Object2World[2].y;
  v_16.w = _Object2World[3].y;
  highp vec4 tmpvar_17;
  tmpvar_17.xyz = (tmpvar_12 * v_16.xyz);
  tmpvar_17.w = tmpvar_9.y;
  highp vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * unity_Scale.w);
  tmpvar_4 = tmpvar_18;
  vec4 v_19;
  v_19.x = _Object2World[0].z;
  v_19.y = _Object2World[1].z;
  v_19.z = _Object2World[2].z;
  v_19.w = _Object2World[3].z;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = (tmpvar_12 * v_19.xyz);
  tmpvar_20.w = tmpvar_9.z;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * unity_Scale.w);
  tmpvar_5 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_12 * (((_World2Object * _WorldSpaceLightPos0).xyz * unity_Scale.w) - _glesVertex.xyz));
  tmpvar_6 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = _WorldSpaceCameraPos;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_12 * (((_World2Object * tmpvar_23).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * _glesVertex)).xyz;
}



#endif
#ifdef FRAGMENT

varying highp vec3 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform sampler2D _LightTextureB0;
uniform samplerCube _LightTexture0;
uniform lowp vec4 _LightColor0;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  mediump vec3 tmpvar_3;
  highp vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  mediump vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8.x = xlv_TEXCOORD1.w;
  tmpvar_8.y = xlv_TEXCOORD2.w;
  tmpvar_8.z = xlv_TEXCOORD3.w;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD1.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD2.xyz;
  tmpvar_6 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = xlv_TEXCOORD3.xyz;
  tmpvar_7 = tmpvar_11;
  tmpvar_3 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_12;
  lowp float tmpvar_13;
  mediump vec3 HDR_14;
  highp vec4 c_15;
  mediump vec3 tmpvar_16;
  tmpvar_16.x = tmpvar_5.z;
  tmpvar_16.y = tmpvar_6.z;
  tmpvar_16.z = tmpvar_7.z;
  highp vec3 tmpvar_17;
  tmpvar_17 = (tmpvar_4 - (2.0 * (dot (tmpvar_16, tmpvar_4) * tmpvar_16)));
  lowp vec4 tmpvar_18;
  tmpvar_18 = textureCube (_Cube, tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_18 * _ReflectColor);
  c_15 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = vec3((pow ((((c_15.x + c_15.y) + c_15.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_14 = tmpvar_20;
  mediump float tmpvar_21;
  tmpvar_21 = (1.0 - dot (normalize(tmpvar_3), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_22;
  tmpvar_22 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_21, _TransmissiveColor)));
  mediump float tmpvar_23;
  tmpvar_23 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_21)).x;
  tmpvar_13 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = ((c_15.xyz + HDR_14) + tmpvar_22.xyz);
  tmpvar_12 = tmpvar_24;
  mediump vec3 tmpvar_25;
  tmpvar_25 = normalize(xlv_TEXCOORD4);
  lightDir_2 = tmpvar_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = normalize(xlv_TEXCOORD0);
  highp float tmpvar_27;
  tmpvar_27 = dot (xlv_TEXCOORD5, xlv_TEXCOORD5);
  lowp vec4 tmpvar_28;
  tmpvar_28 = texture2D (_LightTextureB0, vec2(tmpvar_27));
  lowp vec4 tmpvar_29;
  tmpvar_29 = textureCube (_LightTexture0, xlv_TEXCOORD5);
  mediump vec3 lightDir_30;
  lightDir_30 = lightDir_2;
  mediump vec3 viewDir_31;
  viewDir_31 = tmpvar_26;
  mediump float atten_32;
  atten_32 = (tmpvar_28.w * tmpvar_29.w);
  mediump vec4 c_33;
  c_33.xyz = (((((((lightDir_30.z * 0.5) + 0.5) * tmpvar_12) * _LightColor0.xyz) * atten_32) * 2.0) + ((_LightColor0.xyz * max (0.0, (pow ((viewDir_31 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_13))) * atten_32));
  c_33.w = 1.0;
  c_1.xyz = c_33.xyz;
  c_1.w = 0.0;
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "POINT_COOKIE" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Color _glesColor
in vec4 _glesColor;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;
#define gl_MultiTexCoord1 _glesMultiTexCoord1
in vec4 _glesMultiTexCoord1;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
mat2 xll_transpose_mf2x2(mat2 m) {
  return mat2( m[0][0], m[1][0], m[0][1], m[1][1]);
}
mat3 xll_transpose_mf3x3(mat3 m) {
  return mat3( m[0][0], m[1][0], m[2][0],
               m[0][1], m[1][1], m[2][1],
               m[0][2], m[1][2], m[2][2]);
}
mat4 xll_transpose_mf4x4(mat4 m) {
  return mat4( m[0][0], m[1][0], m[2][0], m[3][0],
               m[0][1], m[1][1], m[2][1], m[3][1],
               m[0][2], m[1][2], m[2][2], m[3][2],
               m[0][3], m[1][3], m[2][3], m[3][3]);
}
vec2 xll_matrixindex_mf2x2_i (mat2 m, int i) { vec2 v; v.x=m[0][i]; v.y=m[1][i]; return v; }
vec3 xll_matrixindex_mf3x3_i (mat3 m, int i) { vec3 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; return v; }
vec4 xll_matrixindex_mf4x4_i (mat4 m, int i) { vec4 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; v.w=m[3][i]; return v; }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 402
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 435
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    mediump vec3 lightDir;
    highp vec3 _LightCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform highp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform samplerCube _LightTexture0;
uniform highp mat4 _LightMatrix0;
#line 393
uniform sampler2D _LightTextureB0;
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
uniform mediump vec4 _FresnelColor;
#line 397
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
uniform mediump float _HdrPower;
#line 401
uniform mediump float _HdrContrast;
#line 411
#line 446
#line 82
highp vec3 ObjSpaceLightDir( in highp vec4 v ) {
    highp vec3 objSpaceLightPos = (_World2Object * _WorldSpaceLightPos0).xyz;
    return ((objSpaceLightPos.xyz * unity_Scale.w) - v.xyz);
}
#line 91
highp vec3 ObjSpaceViewDir( in highp vec4 v ) {
    highp vec3 objSpaceCameraPos = ((_World2Object * vec4( _WorldSpaceCameraPos.xyz, 1.0)).xyz * unity_Scale.w);
    return (objSpaceCameraPos - v.xyz);
}
#line 446
v2f_surf vert_surf( in appdata_full v ) {
    v2f_surf o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 450
    highp vec3 viewDir = (-ObjSpaceViewDir( v.vertex));
    highp vec3 worldRefl = (mat3( _Object2World) * viewDir);
    highp vec3 binormal = (cross( v.normal, v.tangent.xyz) * v.tangent.w);
    highp mat3 rotation = xll_transpose_mf3x3(mat3( v.tangent.xyz, binormal, v.normal));
    #line 454
    o.TtoW0 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 0).xyz), worldRefl.x) * unity_Scale.w);
    o.TtoW1 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 1).xyz), worldRefl.y) * unity_Scale.w);
    o.TtoW2 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 2).xyz), worldRefl.z) * unity_Scale.w);
    highp vec3 lightDir = (rotation * ObjSpaceLightDir( v.vertex));
    #line 458
    o.lightDir = lightDir;
    highp vec3 viewDirForLight = (rotation * ObjSpaceViewDir( v.vertex));
    o.viewDir = viewDirForLight;
    o._LightCoord = (_LightMatrix0 * (_Object2World * v.vertex)).xyz;
    #line 463
    return o;
}

out highp vec3 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_TEXCOORD2;
out lowp vec4 xlv_TEXCOORD3;
out mediump vec3 xlv_TEXCOORD4;
out highp vec3 xlv_TEXCOORD5;
void main() {
    v2f_surf xl_retval;
    appdata_full xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.tangent = vec4(TANGENT);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xlt_v.texcoord1 = vec4(gl_MultiTexCoord1);
    xlt_v.color = vec4(gl_Color);
    xl_retval = vert_surf( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD0 = vec3(xl_retval.viewDir);
    xlv_TEXCOORD1 = vec4(xl_retval.TtoW0);
    xlv_TEXCOORD2 = vec4(xl_retval.TtoW1);
    xlv_TEXCOORD3 = vec4(xl_retval.TtoW2);
    xlv_TEXCOORD4 = vec3(xl_retval.lightDir);
    xlv_TEXCOORD5 = vec3(xl_retval._LightCoord);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 402
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 435
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    mediump vec3 lightDir;
    highp vec3 _LightCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform highp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform samplerCube _LightTexture0;
uniform highp mat4 _LightMatrix0;
#line 393
uniform sampler2D _LightTextureB0;
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
uniform mediump vec4 _FresnelColor;
#line 397
uniform mediump vec4 _Color;
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
uniform mediump float _HdrPower;
#line 401
uniform mediump float _HdrContrast;
#line 411
#line 446
#line 425
mediump vec4 LightingHalfLambertSpecular( in SurfaceOutput s, in mediump vec3 lightDir, in mediump vec3 viewDir, in mediump float atten ) {
    #line 427
    mediump float NdotL = ((dot( s.Normal, lightDir) * 0.5) + 0.5);
    mediump vec3 halfVector = (viewDir + s.Normal);
    mediump float spec = max( 0.0, (pow( dot( halfVector, s.Normal), (10.0 - s.Specular)) * s.Gloss));
    mediump vec4 c;
    #line 431
    c.xyz = (((((NdotL * s.Albedo) * _LightColor0.xyz) * atten) * 2.0) + ((_LightColor0.xyz * spec) * atten));
    c.w = s.Alpha;
    return c;
}
#line 411
void surf( in Input IN, inout SurfaceOutput o ) {
    o.Normal = vec3( 0.0, 0.0, 1.0);
    highp vec3 worldRefl = reflect( IN.worldRefl, vec3( dot( IN.TtoW0, o.Normal), dot( IN.TtoW1, o.Normal), dot( IN.TtoW2, o.Normal)));
    #line 415
    highp vec4 c = (texture( _Cube, worldRefl) * _ReflectColor);
    mediump vec3 HDR = vec3( (pow( (((c.x + c.y) + c.z) / 3.0), _HdrPower) * _HdrContrast));
    mediump float fresnel = (1.0 - dot( normalize(IN.viewDir), normalize(vec3( 0.0, 0.0, 1.0))));
    mediump vec4 fresnelLerp = mix( _Color, _FresnelColor, vec4( pow( fresnel, _TransmissiveColor)));
    #line 419
    mediump vec4 GlossLerp = mix( vec4( 0.0, 0.0, 0.0, 0.0), vec4( 1.0, 1.0, 1.0, 1.0), vec4( fresnel));
    o.Specular = _Shininess;
    o.Gloss = GlossLerp.x;
    o.Albedo = ((c.xyz + HDR) + fresnelLerp.xyz);
    #line 423
    o.Alpha = 1.0;
}
#line 465
lowp vec4 frag_surf( in v2f_surf IN ) {
    #line 467
    Input surfIN;
    surfIN.worldRefl = vec3( IN.TtoW0.w, IN.TtoW1.w, IN.TtoW2.w);
    surfIN.TtoW0 = IN.TtoW0.xyz;
    surfIN.TtoW1 = IN.TtoW1.xyz;
    #line 471
    surfIN.TtoW2 = IN.TtoW2.xyz;
    surfIN.viewDir = IN.viewDir;
    SurfaceOutput o;
    o.Albedo = vec3( 0.0);
    #line 475
    o.Emission = vec3( 0.0);
    o.Specular = 0.0;
    o.Alpha = 0.0;
    o.Gloss = 0.0;
    #line 479
    surf( surfIN, o);
    lowp vec3 lightDir = normalize(IN.lightDir);
    lowp vec4 c = LightingHalfLambertSpecular( o, lightDir, normalize(IN.viewDir), ((texture( _LightTextureB0, vec2( dot( IN._LightCoord, IN._LightCoord))).w * texture( _LightTexture0, IN._LightCoord).w) * 1.0));
    c.w = 0.0;
    #line 483
    return c;
}
in highp vec3 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_TEXCOORD2;
in lowp vec4 xlv_TEXCOORD3;
in mediump vec3 xlv_TEXCOORD4;
in highp vec3 xlv_TEXCOORD5;
void main() {
    lowp vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.viewDir = vec3(xlv_TEXCOORD0);
    xlt_IN.TtoW0 = vec4(xlv_TEXCOORD1);
    xlt_IN.TtoW1 = vec4(xlv_TEXCOORD2);
    xlt_IN.TtoW2 = vec4(xlv_TEXCOORD3);
    xlt_IN.lightDir = vec3(xlv_TEXCOORD4);
    xlt_IN._LightCoord = vec3(xlv_TEXCOORD5);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL_COOKIE" }
Bind "vertex" Vertex
Bind "tangent" ATTR14
Bind "normal" Normal
Vector 17 [_WorldSpaceCameraPos]
Vector 18 [_WorldSpaceLightPos0]
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 19 [unity_Scale]
Matrix 13 [_LightMatrix0]
"3.0-!!ARBvp1.0
# 45 ALU
PARAM c[20] = { { 1 },
		state.matrix.mvp,
		program.local[5..19] };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R0.xyz, vertex.attrib[14];
MUL R1.xyz, vertex.normal.zxyw, R0.yzxw;
MAD R0.xyz, vertex.normal.yzxw, R0.zxyw, -R1;
MUL R3.xyz, R0, vertex.attrib[14].w;
MOV R0, c[18];
MOV R1.xyz, c[17];
MOV R1.w, c[0].x;
DP4 R2.z, R1, c[11];
DP4 R2.x, R1, c[9];
DP4 R2.y, R1, c[10];
MAD R2.xyz, R2, c[19].w, -vertex.position;
DP4 R1.z, R0, c[11];
DP4 R1.x, R0, c[9];
DP4 R1.y, R0, c[10];
DP3 R0.y, R3, c[5];
DP3 R0.w, -R2, c[5];
DP3 R0.x, vertex.attrib[14], c[5];
DP3 R0.z, vertex.normal, c[5];
MUL result.texcoord[1], R0, c[19].w;
DP3 R0.y, R3, c[6];
DP3 R0.w, -R2, c[6];
DP3 R0.x, vertex.attrib[14], c[6];
DP3 R0.z, vertex.normal, c[6];
MUL result.texcoord[2], R0, c[19].w;
DP3 R0.y, R3, c[7];
DP3 R0.w, -R2, c[7];
DP3 R0.x, vertex.attrib[14], c[7];
DP3 R0.z, vertex.normal, c[7];
MUL result.texcoord[3], R0, c[19].w;
DP4 R0.w, vertex.position, c[8];
DP4 R0.z, vertex.position, c[7];
DP4 R0.x, vertex.position, c[5];
DP4 R0.y, vertex.position, c[6];
DP3 result.texcoord[0].y, R2, R3;
DP3 result.texcoord[4].y, R3, R1;
DP3 result.texcoord[0].z, vertex.normal, R2;
DP3 result.texcoord[0].x, R2, vertex.attrib[14];
DP3 result.texcoord[4].z, vertex.normal, R1;
DP3 result.texcoord[4].x, vertex.attrib[14], R1;
DP4 result.texcoord[5].y, R0, c[14];
DP4 result.texcoord[5].x, R0, c[13];
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
END
# 45 instructions, 4 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL_COOKIE" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Matrix 0 [glstate_matrix_mvp]
Vector 16 [_WorldSpaceCameraPos]
Vector 17 [_WorldSpaceLightPos0]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 18 [unity_Scale]
Matrix 12 [_LightMatrix0]
"vs_3_0
; 48 ALU
dcl_position o0
dcl_texcoord0 o1
dcl_texcoord1 o2
dcl_texcoord2 o3
dcl_texcoord3 o4
dcl_texcoord4 o5
dcl_texcoord5 o6
def c19, 1.00000000, 0, 0, 0
dcl_position0 v0
dcl_tangent0 v1
dcl_normal0 v2
mov r0.xyz, v1
mul r1.xyz, v2.zxyw, r0.yzxw
mov r0.xyz, v1
mad r0.xyz, v2.yzxw, r0.zxyw, -r1
mul r3.xyz, r0, v1.w
mov r0, c10
dp4 r4.z, c17, r0
mov r0, c9
dp4 r4.y, c17, r0
mov r1.w, c19.x
mov r1.xyz, c16
dp4 r2.z, r1, c10
dp4 r2.x, r1, c8
dp4 r2.y, r1, c9
mad r2.xyz, r2, c18.w, -v0
mov r1, c8
dp4 r4.x, c17, r1
dp3 r0.y, r3, c4
dp3 r0.w, -r2, c4
dp3 r0.x, v1, c4
dp3 r0.z, v2, c4
mul o2, r0, c18.w
dp3 r0.y, r3, c5
dp3 r0.w, -r2, c5
dp3 r0.x, v1, c5
dp3 r0.z, v2, c5
mul o3, r0, c18.w
dp3 r0.y, r3, c6
dp3 r0.w, -r2, c6
dp3 r0.x, v1, c6
dp3 r0.z, v2, c6
mul o4, r0, c18.w
dp4 r0.w, v0, c7
dp4 r0.z, v0, c6
dp4 r0.x, v0, c4
dp4 r0.y, v0, c5
dp3 o1.y, r2, r3
dp3 o5.y, r3, r4
dp3 o1.z, v2, r2
dp3 o1.x, r2, v1
dp3 o5.z, v2, r4
dp3 o5.x, v1, r4
dp4 o6.y, r0, c13
dp4 o6.x, r0, c12
dp4 o0.w, v0, c3
dp4 o0.z, v0, c2
dp4 o0.y, v0, c1
dp4 o0.x, v0, c0
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL_COOKIE" }
Bind "vertex" Vertex
Bind "tangent" TexCoord2
Bind "normal" Normal
Bind "color" Color
ConstBuffer "$Globals" 176 // 112 used size, 11 vars
Matrix 48 [_LightMatrix0] 4
ConstBuffer "UnityPerCamera" 128 // 76 used size, 8 vars
Vector 64 [_WorldSpaceCameraPos] 3
ConstBuffer "UnityLighting" 720 // 16 used size, 17 vars
Vector 0 [_WorldSpaceLightPos0] 4
ConstBuffer "UnityPerDraw" 336 // 336 used size, 6 vars
Matrix 0 [glstate_matrix_mvp] 4
Matrix 192 [_Object2World] 4
Matrix 256 [_World2Object] 4
Vector 320 [unity_Scale] 4
BindCB "$Globals" 0
BindCB "UnityPerCamera" 1
BindCB "UnityLighting" 2
BindCB "UnityPerDraw" 3
// 57 instructions, 4 temp regs, 0 temp arrays:
// ALU 45 float, 0 int, 0 uint
// TEX 0 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"vs_4_0
eefiecedahkbfhngjbhciceiffbaaaefnghabkjnabaaaaaaleajaaaaadaaaaaa
cmaaaaaapeaaaaaameabaaaaejfdeheomaaaaaaaagaaaaaaaiaaaaaajiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaakbaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapapaaaakjaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
ahahaaaalaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaadaaaaaaapaaaaaalaaaaaaa
abaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaaljaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaafaaaaaaapaaaaaafaepfdejfeejepeoaafeebeoehefeofeaaeoepfc
enebemaafeeffiedepepfceeaaedepemepfcaaklepfdeheomiaaaaaaahaaaaaa
aiaaaaaalaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaalmaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaahaiaaaalmaaaaaaabaaaaaaaaaaaaaa
adaaaaaaacaaaaaaapaaaaaalmaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaa
apaaaaaalmaaaaaaadaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaalmaaaaaa
aeaaaaaaaaaaaaaaadaaaaaaafaaaaaaahaiaaaalmaaaaaaafaaaaaaaaaaaaaa
adaaaaaaagaaaaaaadamaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklklfdeieefcoiahaaaaeaaaabaapkabaaaafjaaaaaeegiocaaaaaaaaaaa
ahaaaaaafjaaaaaeegiocaaaabaaaaaaafaaaaaafjaaaaaeegiocaaaacaaaaaa
abaaaaaafjaaaaaeegiocaaaadaaaaaabfaaaaaafpaaaaadpcbabaaaaaaaaaaa
fpaaaaadpcbabaaaabaaaaaafpaaaaadhcbabaaaacaaaaaaghaaaaaepccabaaa
aaaaaaaaabaaaaaagfaaaaadhccabaaaabaaaaaagfaaaaadpccabaaaacaaaaaa
gfaaaaadpccabaaaadaaaaaagfaaaaadpccabaaaaeaaaaaagfaaaaadhccabaaa
afaaaaaagfaaaaaddccabaaaagaaaaaagiaaaaacaeaaaaaadiaaaaaipcaabaaa
aaaaaaaafgbfbaaaaaaaaaaaegiocaaaadaaaaaaabaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaadaaaaaaaaaaaaaaagbabaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaakpcaabaaaaaaaaaaaegiocaaaadaaaaaaacaaaaaakgbkbaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaakpccabaaaaaaaaaaaegiocaaaadaaaaaaadaaaaaa
pgbpbaaaaaaaaaaaegaobaaaaaaaaaaadiaaaaajhcaabaaaaaaaaaaafgifcaaa
abaaaaaaaeaaaaaaegiccaaaadaaaaaabbaaaaaadcaaaaalhcaabaaaaaaaaaaa
egiccaaaadaaaaaabaaaaaaaagiacaaaabaaaaaaaeaaaaaaegacbaaaaaaaaaaa
dcaaaaalhcaabaaaaaaaaaaaegiccaaaadaaaaaabcaaaaaakgikcaaaabaaaaaa
aeaaaaaaegacbaaaaaaaaaaaaaaaaaaihcaabaaaaaaaaaaaegacbaaaaaaaaaaa
egiccaaaadaaaaaabdaaaaaadcaaaaalhcaabaaaaaaaaaaaegacbaaaaaaaaaaa
pgipcaaaadaaaaaabeaaaaaaegbcbaiaebaaaaaaaaaaaaaabaaaaaahbccabaaa
abaaaaaaegbcbaaaabaaaaaaegacbaaaaaaaaaaabaaaaaaheccabaaaabaaaaaa
egbcbaaaacaaaaaaegacbaaaaaaaaaaadiaaaaahhcaabaaaabaaaaaajgbebaaa
abaaaaaacgbjbaaaacaaaaaadcaaaaakhcaabaaaabaaaaaajgbebaaaacaaaaaa
cgbjbaaaabaaaaaaegacbaiaebaaaaaaabaaaaaadiaaaaahhcaabaaaabaaaaaa
egacbaaaabaaaaaapgbpbaaaabaaaaaabaaaaaahcccabaaaabaaaaaaegacbaaa
abaaaaaaegacbaaaaaaaaaaadiaaaaajhcaabaaaacaaaaaafgafbaiaebaaaaaa
aaaaaaaaegiccaaaadaaaaaaanaaaaaadcaaaaallcaabaaaaaaaaaaaegiicaaa
adaaaaaaamaaaaaaagaabaiaebaaaaaaaaaaaaaaegaibaaaacaaaaaadcaaaaal
lcaabaaaaaaaaaaaegiicaaaadaaaaaaaoaaaaaakgakbaiaebaaaaaaaaaaaaaa
egambaaaaaaaaaaadgaaaaaficaabaaaacaaaaaaakaabaaaaaaaaaaadgaaaaag
bcaabaaaadaaaaaaakiacaaaadaaaaaaamaaaaaadgaaaaagccaabaaaadaaaaaa
akiacaaaadaaaaaaanaaaaaadgaaaaagecaabaaaadaaaaaaakiacaaaadaaaaaa
aoaaaaaabaaaaaahccaabaaaacaaaaaaegacbaaaabaaaaaaegacbaaaadaaaaaa
baaaaaahbcaabaaaacaaaaaaegbcbaaaabaaaaaaegacbaaaadaaaaaabaaaaaah
ecaabaaaacaaaaaaegbcbaaaacaaaaaaegacbaaaadaaaaaadiaaaaaipccabaaa
acaaaaaaegaobaaaacaaaaaapgipcaaaadaaaaaabeaaaaaadgaaaaaficaabaaa
acaaaaaabkaabaaaaaaaaaaadgaaaaagbcaabaaaadaaaaaabkiacaaaadaaaaaa
amaaaaaadgaaaaagccaabaaaadaaaaaabkiacaaaadaaaaaaanaaaaaadgaaaaag
ecaabaaaadaaaaaabkiacaaaadaaaaaaaoaaaaaabaaaaaahccaabaaaacaaaaaa
egacbaaaabaaaaaaegacbaaaadaaaaaabaaaaaahbcaabaaaacaaaaaaegbcbaaa
abaaaaaaegacbaaaadaaaaaabaaaaaahecaabaaaacaaaaaaegbcbaaaacaaaaaa
egacbaaaadaaaaaadiaaaaaipccabaaaadaaaaaaegaobaaaacaaaaaapgipcaaa
adaaaaaabeaaaaaadgaaaaagbcaabaaaacaaaaaackiacaaaadaaaaaaamaaaaaa
dgaaaaagccaabaaaacaaaaaackiacaaaadaaaaaaanaaaaaadgaaaaagecaabaaa
acaaaaaackiacaaaadaaaaaaaoaaaaaabaaaaaahccaabaaaaaaaaaaaegacbaaa
abaaaaaaegacbaaaacaaaaaabaaaaaahbcaabaaaaaaaaaaaegbcbaaaabaaaaaa
egacbaaaacaaaaaabaaaaaahecaabaaaaaaaaaaaegbcbaaaacaaaaaaegacbaaa
acaaaaaadiaaaaaipccabaaaaeaaaaaaegaobaaaaaaaaaaapgipcaaaadaaaaaa
beaaaaaadiaaaaajhcaabaaaaaaaaaaafgifcaaaacaaaaaaaaaaaaaaegiccaaa
adaaaaaabbaaaaaadcaaaaalhcaabaaaaaaaaaaaegiccaaaadaaaaaabaaaaaaa
agiacaaaacaaaaaaaaaaaaaaegacbaaaaaaaaaaadcaaaaalhcaabaaaaaaaaaaa
egiccaaaadaaaaaabcaaaaaakgikcaaaacaaaaaaaaaaaaaaegacbaaaaaaaaaaa
dcaaaaalhcaabaaaaaaaaaaaegiccaaaadaaaaaabdaaaaaapgipcaaaacaaaaaa
aaaaaaaaegacbaaaaaaaaaaabaaaaaahcccabaaaafaaaaaaegacbaaaabaaaaaa
egacbaaaaaaaaaaabaaaaaahbccabaaaafaaaaaaegbcbaaaabaaaaaaegacbaaa
aaaaaaaabaaaaaaheccabaaaafaaaaaaegbcbaaaacaaaaaaegacbaaaaaaaaaaa
diaaaaaipcaabaaaaaaaaaaafgbfbaaaaaaaaaaaegiocaaaadaaaaaaanaaaaaa
dcaaaaakpcaabaaaaaaaaaaaegiocaaaadaaaaaaamaaaaaaagbabaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaaadaaaaaaaoaaaaaa
kgbkbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaakpcaabaaaaaaaaaaaegiocaaa
adaaaaaaapaaaaaapgbpbaaaaaaaaaaaegaobaaaaaaaaaaadiaaaaaidcaabaaa
abaaaaaafgafbaaaaaaaaaaaegiacaaaaaaaaaaaaeaaaaaadcaaaaakdcaabaaa
aaaaaaaaegiacaaaaaaaaaaaadaaaaaaagaabaaaaaaaaaaaegaabaaaabaaaaaa
dcaaaaakdcaabaaaaaaaaaaaegiacaaaaaaaaaaaafaaaaaakgakbaaaaaaaaaaa
egaabaaaaaaaaaaadcaaaaakdccabaaaagaaaaaaegiacaaaaaaaaaaaagaaaaaa
pgapbaaaaaaaaaaaegaabaaaaaaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES


#ifdef VERTEX

varying highp vec2 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp mat4 _LightMatrix0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (_glesVertex.xyz - ((_World2Object * tmpvar_7).xyz * unity_Scale.w)));
  highp vec3 tmpvar_10;
  highp vec3 tmpvar_11;
  tmpvar_10 = tmpvar_1.xyz;
  tmpvar_11 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_11.x;
  tmpvar_12[0].z = tmpvar_2.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_11.y;
  tmpvar_12[1].z = tmpvar_2.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_11.z;
  tmpvar_12[2].z = tmpvar_2.z;
  vec4 v_13;
  v_13.x = _Object2World[0].x;
  v_13.y = _Object2World[1].x;
  v_13.z = _Object2World[2].x;
  v_13.w = _Object2World[3].x;
  highp vec4 tmpvar_14;
  tmpvar_14.xyz = (tmpvar_12 * v_13.xyz);
  tmpvar_14.w = tmpvar_9.x;
  highp vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_14 * unity_Scale.w);
  tmpvar_3 = tmpvar_15;
  vec4 v_16;
  v_16.x = _Object2World[0].y;
  v_16.y = _Object2World[1].y;
  v_16.z = _Object2World[2].y;
  v_16.w = _Object2World[3].y;
  highp vec4 tmpvar_17;
  tmpvar_17.xyz = (tmpvar_12 * v_16.xyz);
  tmpvar_17.w = tmpvar_9.y;
  highp vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * unity_Scale.w);
  tmpvar_4 = tmpvar_18;
  vec4 v_19;
  v_19.x = _Object2World[0].z;
  v_19.y = _Object2World[1].z;
  v_19.z = _Object2World[2].z;
  v_19.w = _Object2World[3].z;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = (tmpvar_12 * v_19.xyz);
  tmpvar_20.w = tmpvar_9.z;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * unity_Scale.w);
  tmpvar_5 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_12 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_6 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = _WorldSpaceCameraPos;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_12 * (((_World2Object * tmpvar_23).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * _glesVertex)).xy;
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform sampler2D _LightTexture0;
uniform lowp vec4 _LightColor0;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  mediump vec3 tmpvar_3;
  highp vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  mediump vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8.x = xlv_TEXCOORD1.w;
  tmpvar_8.y = xlv_TEXCOORD2.w;
  tmpvar_8.z = xlv_TEXCOORD3.w;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD1.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD2.xyz;
  tmpvar_6 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = xlv_TEXCOORD3.xyz;
  tmpvar_7 = tmpvar_11;
  tmpvar_3 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_12;
  lowp float tmpvar_13;
  mediump vec3 HDR_14;
  highp vec4 c_15;
  mediump vec3 tmpvar_16;
  tmpvar_16.x = tmpvar_5.z;
  tmpvar_16.y = tmpvar_6.z;
  tmpvar_16.z = tmpvar_7.z;
  highp vec3 tmpvar_17;
  tmpvar_17 = (tmpvar_4 - (2.0 * (dot (tmpvar_16, tmpvar_4) * tmpvar_16)));
  lowp vec4 tmpvar_18;
  tmpvar_18 = textureCube (_Cube, tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_18 * _ReflectColor);
  c_15 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = vec3((pow ((((c_15.x + c_15.y) + c_15.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_14 = tmpvar_20;
  mediump float tmpvar_21;
  tmpvar_21 = (1.0 - dot (normalize(tmpvar_3), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_22;
  tmpvar_22 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_21, _TransmissiveColor)));
  mediump float tmpvar_23;
  tmpvar_23 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_21)).x;
  tmpvar_13 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = ((c_15.xyz + HDR_14) + tmpvar_22.xyz);
  tmpvar_12 = tmpvar_24;
  lightDir_2 = xlv_TEXCOORD4;
  highp vec3 tmpvar_25;
  tmpvar_25 = normalize(xlv_TEXCOORD0);
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_LightTexture0, xlv_TEXCOORD5);
  mediump vec3 lightDir_27;
  lightDir_27 = lightDir_2;
  mediump vec3 viewDir_28;
  viewDir_28 = tmpvar_25;
  mediump float atten_29;
  atten_29 = tmpvar_26.w;
  mediump vec4 c_30;
  c_30.xyz = (((((((lightDir_27.z * 0.5) + 0.5) * tmpvar_12) * _LightColor0.xyz) * atten_29) * 2.0) + ((_LightColor0.xyz * max (0.0, (pow ((viewDir_28 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_13))) * atten_29));
  c_30.w = 1.0;
  c_1.xyz = c_30.xyz;
  c_1.w = 0.0;
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES


#ifdef VERTEX

varying highp vec2 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform highp mat4 _LightMatrix0;
uniform highp vec4 unity_Scale;
uniform highp mat4 _World2Object;
uniform highp mat4 _Object2World;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec3 _WorldSpaceCameraPos;
attribute vec4 _glesTANGENT;
attribute vec3 _glesNormal;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize(_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  vec3 tmpvar_2;
  tmpvar_2 = normalize(_glesNormal);
  lowp vec4 tmpvar_3;
  lowp vec4 tmpvar_4;
  lowp vec4 tmpvar_5;
  mediump vec3 tmpvar_6;
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = _WorldSpaceCameraPos;
  mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (_glesVertex.xyz - ((_World2Object * tmpvar_7).xyz * unity_Scale.w)));
  highp vec3 tmpvar_10;
  highp vec3 tmpvar_11;
  tmpvar_10 = tmpvar_1.xyz;
  tmpvar_11 = (((tmpvar_2.yzx * tmpvar_1.zxy) - (tmpvar_2.zxy * tmpvar_1.yzx)) * _glesTANGENT.w);
  highp mat3 tmpvar_12;
  tmpvar_12[0].x = tmpvar_10.x;
  tmpvar_12[0].y = tmpvar_11.x;
  tmpvar_12[0].z = tmpvar_2.x;
  tmpvar_12[1].x = tmpvar_10.y;
  tmpvar_12[1].y = tmpvar_11.y;
  tmpvar_12[1].z = tmpvar_2.y;
  tmpvar_12[2].x = tmpvar_10.z;
  tmpvar_12[2].y = tmpvar_11.z;
  tmpvar_12[2].z = tmpvar_2.z;
  vec4 v_13;
  v_13.x = _Object2World[0].x;
  v_13.y = _Object2World[1].x;
  v_13.z = _Object2World[2].x;
  v_13.w = _Object2World[3].x;
  highp vec4 tmpvar_14;
  tmpvar_14.xyz = (tmpvar_12 * v_13.xyz);
  tmpvar_14.w = tmpvar_9.x;
  highp vec4 tmpvar_15;
  tmpvar_15 = (tmpvar_14 * unity_Scale.w);
  tmpvar_3 = tmpvar_15;
  vec4 v_16;
  v_16.x = _Object2World[0].y;
  v_16.y = _Object2World[1].y;
  v_16.z = _Object2World[2].y;
  v_16.w = _Object2World[3].y;
  highp vec4 tmpvar_17;
  tmpvar_17.xyz = (tmpvar_12 * v_16.xyz);
  tmpvar_17.w = tmpvar_9.y;
  highp vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * unity_Scale.w);
  tmpvar_4 = tmpvar_18;
  vec4 v_19;
  v_19.x = _Object2World[0].z;
  v_19.y = _Object2World[1].z;
  v_19.z = _Object2World[2].z;
  v_19.w = _Object2World[3].z;
  highp vec4 tmpvar_20;
  tmpvar_20.xyz = (tmpvar_12 * v_19.xyz);
  tmpvar_20.w = tmpvar_9.z;
  highp vec4 tmpvar_21;
  tmpvar_21 = (tmpvar_20 * unity_Scale.w);
  tmpvar_5 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = (tmpvar_12 * (_World2Object * _WorldSpaceLightPos0).xyz);
  tmpvar_6 = tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = _WorldSpaceCameraPos;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = (tmpvar_12 * (((_World2Object * tmpvar_23).xyz * unity_Scale.w) - _glesVertex.xyz));
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = tmpvar_6;
  xlv_TEXCOORD5 = (_LightMatrix0 * (_Object2World * _glesVertex)).xy;
}



#endif
#ifdef FRAGMENT

varying highp vec2 xlv_TEXCOORD5;
varying mediump vec3 xlv_TEXCOORD4;
varying lowp vec4 xlv_TEXCOORD3;
varying lowp vec4 xlv_TEXCOORD2;
varying lowp vec4 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD0;
uniform mediump float _HdrContrast;
uniform mediump float _HdrPower;
uniform mediump float _Shininess;
uniform mediump float _TransmissiveColor;
uniform mediump vec4 _Color;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _ReflectColor;
uniform samplerCube _Cube;
uniform sampler2D _LightTexture0;
uniform lowp vec4 _LightColor0;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  mediump vec3 tmpvar_3;
  highp vec3 tmpvar_4;
  mediump vec3 tmpvar_5;
  mediump vec3 tmpvar_6;
  mediump vec3 tmpvar_7;
  lowp vec3 tmpvar_8;
  tmpvar_8.x = xlv_TEXCOORD1.w;
  tmpvar_8.y = xlv_TEXCOORD2.w;
  tmpvar_8.z = xlv_TEXCOORD3.w;
  tmpvar_4 = tmpvar_8;
  lowp vec3 tmpvar_9;
  tmpvar_9 = xlv_TEXCOORD1.xyz;
  tmpvar_5 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = xlv_TEXCOORD2.xyz;
  tmpvar_6 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = xlv_TEXCOORD3.xyz;
  tmpvar_7 = tmpvar_11;
  tmpvar_3 = xlv_TEXCOORD0;
  lowp vec3 tmpvar_12;
  lowp float tmpvar_13;
  mediump vec3 HDR_14;
  highp vec4 c_15;
  mediump vec3 tmpvar_16;
  tmpvar_16.x = tmpvar_5.z;
  tmpvar_16.y = tmpvar_6.z;
  tmpvar_16.z = tmpvar_7.z;
  highp vec3 tmpvar_17;
  tmpvar_17 = (tmpvar_4 - (2.0 * (dot (tmpvar_16, tmpvar_4) * tmpvar_16)));
  lowp vec4 tmpvar_18;
  tmpvar_18 = textureCube (_Cube, tmpvar_17);
  mediump vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_18 * _ReflectColor);
  c_15 = tmpvar_19;
  highp vec3 tmpvar_20;
  tmpvar_20 = vec3((pow ((((c_15.x + c_15.y) + c_15.z) / 3.0), _HdrPower) * _HdrContrast));
  HDR_14 = tmpvar_20;
  mediump float tmpvar_21;
  tmpvar_21 = (1.0 - dot (normalize(tmpvar_3), normalize(vec3(0.0, 0.0, 1.0))));
  mediump vec4 tmpvar_22;
  tmpvar_22 = mix (_Color, _FresnelColor, vec4(pow (tmpvar_21, _TransmissiveColor)));
  mediump float tmpvar_23;
  tmpvar_23 = mix (vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(tmpvar_21)).x;
  tmpvar_13 = tmpvar_23;
  highp vec3 tmpvar_24;
  tmpvar_24 = ((c_15.xyz + HDR_14) + tmpvar_22.xyz);
  tmpvar_12 = tmpvar_24;
  lightDir_2 = xlv_TEXCOORD4;
  highp vec3 tmpvar_25;
  tmpvar_25 = normalize(xlv_TEXCOORD0);
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_LightTexture0, xlv_TEXCOORD5);
  mediump vec3 lightDir_27;
  lightDir_27 = lightDir_2;
  mediump vec3 viewDir_28;
  viewDir_28 = tmpvar_25;
  mediump float atten_29;
  atten_29 = tmpvar_26.w;
  mediump vec4 c_30;
  c_30.xyz = (((((((lightDir_27.z * 0.5) + 0.5) * tmpvar_12) * _LightColor0.xyz) * atten_29) * 2.0) + ((_LightColor0.xyz * max (0.0, (pow ((viewDir_28 + vec3(0.0, 0.0, 1.0)).z, (10.0 - _Shininess)) * tmpvar_13))) * atten_29));
  c_30.w = 1.0;
  c_1.xyz = c_30.xyz;
  c_1.w = 0.0;
  gl_FragData[0] = c_1;
}



#endif"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES3#version 300 es


#ifdef VERTEX

#define gl_Vertex _glesVertex
in vec4 _glesVertex;
#define gl_Color _glesColor
in vec4 _glesColor;
#define gl_Normal (normalize(_glesNormal))
in vec3 _glesNormal;
#define gl_MultiTexCoord0 _glesMultiTexCoord0
in vec4 _glesMultiTexCoord0;
#define gl_MultiTexCoord1 _glesMultiTexCoord1
in vec4 _glesMultiTexCoord1;
#define TANGENT vec4(normalize(_glesTANGENT.xyz), _glesTANGENT.w)
in vec4 _glesTANGENT;
mat2 xll_transpose_mf2x2(mat2 m) {
  return mat2( m[0][0], m[1][0], m[0][1], m[1][1]);
}
mat3 xll_transpose_mf3x3(mat3 m) {
  return mat3( m[0][0], m[1][0], m[2][0],
               m[0][1], m[1][1], m[2][1],
               m[0][2], m[1][2], m[2][2]);
}
mat4 xll_transpose_mf4x4(mat4 m) {
  return mat4( m[0][0], m[1][0], m[2][0], m[3][0],
               m[0][1], m[1][1], m[2][1], m[3][1],
               m[0][2], m[1][2], m[2][2], m[3][2],
               m[0][3], m[1][3], m[2][3], m[3][3]);
}
vec2 xll_matrixindex_mf2x2_i (mat2 m, int i) { vec2 v; v.x=m[0][i]; v.y=m[1][i]; return v; }
vec3 xll_matrixindex_mf3x3_i (mat3 m, int i) { vec3 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; return v; }
vec4 xll_matrixindex_mf4x4_i (mat4 m, int i) { vec4 v; v.x=m[0][i]; v.y=m[1][i]; v.z=m[2][i]; v.w=m[3][i]; return v; }
#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 401
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 434
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    mediump vec3 lightDir;
    highp vec2 _LightCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform sampler2D _LightTexture0;
uniform highp mat4 _LightMatrix0;
#line 393
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
#line 397
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 410
#line 445
#line 82
highp vec3 ObjSpaceLightDir( in highp vec4 v ) {
    highp vec3 objSpaceLightPos = (_World2Object * _WorldSpaceLightPos0).xyz;
    return objSpaceLightPos.xyz;
}
#line 91
highp vec3 ObjSpaceViewDir( in highp vec4 v ) {
    highp vec3 objSpaceCameraPos = ((_World2Object * vec4( _WorldSpaceCameraPos.xyz, 1.0)).xyz * unity_Scale.w);
    return (objSpaceCameraPos - v.xyz);
}
#line 445
v2f_surf vert_surf( in appdata_full v ) {
    v2f_surf o;
    o.pos = (glstate_matrix_mvp * v.vertex);
    #line 449
    highp vec3 viewDir = (-ObjSpaceViewDir( v.vertex));
    highp vec3 worldRefl = (mat3( _Object2World) * viewDir);
    highp vec3 binormal = (cross( v.normal, v.tangent.xyz) * v.tangent.w);
    highp mat3 rotation = xll_transpose_mf3x3(mat3( v.tangent.xyz, binormal, v.normal));
    #line 453
    o.TtoW0 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 0).xyz), worldRefl.x) * unity_Scale.w);
    o.TtoW1 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 1).xyz), worldRefl.y) * unity_Scale.w);
    o.TtoW2 = (vec4( (rotation * xll_matrixindex_mf4x4_i (_Object2World, 2).xyz), worldRefl.z) * unity_Scale.w);
    highp vec3 lightDir = (rotation * ObjSpaceLightDir( v.vertex));
    #line 457
    o.lightDir = lightDir;
    highp vec3 viewDirForLight = (rotation * ObjSpaceViewDir( v.vertex));
    o.viewDir = viewDirForLight;
    o._LightCoord = (_LightMatrix0 * (_Object2World * v.vertex)).xy;
    #line 462
    return o;
}

out highp vec3 xlv_TEXCOORD0;
out lowp vec4 xlv_TEXCOORD1;
out lowp vec4 xlv_TEXCOORD2;
out lowp vec4 xlv_TEXCOORD3;
out mediump vec3 xlv_TEXCOORD4;
out highp vec2 xlv_TEXCOORD5;
void main() {
    v2f_surf xl_retval;
    appdata_full xlt_v;
    xlt_v.vertex = vec4(gl_Vertex);
    xlt_v.tangent = vec4(TANGENT);
    xlt_v.normal = vec3(gl_Normal);
    xlt_v.texcoord = vec4(gl_MultiTexCoord0);
    xlt_v.texcoord1 = vec4(gl_MultiTexCoord1);
    xlt_v.color = vec4(gl_Color);
    xl_retval = vert_surf( xlt_v);
    gl_Position = vec4(xl_retval.pos);
    xlv_TEXCOORD0 = vec3(xl_retval.viewDir);
    xlv_TEXCOORD1 = vec4(xl_retval.TtoW0);
    xlv_TEXCOORD2 = vec4(xl_retval.TtoW1);
    xlv_TEXCOORD3 = vec4(xl_retval.TtoW2);
    xlv_TEXCOORD4 = vec3(xl_retval.lightDir);
    xlv_TEXCOORD5 = vec2(xl_retval._LightCoord);
}


#endif
#ifdef FRAGMENT

#define gl_FragData _glesFragData
layout(location = 0) out mediump vec4 _glesFragData[4];

#line 151
struct v2f_vertex_lit {
    highp vec2 uv;
    lowp vec4 diff;
    lowp vec4 spec;
};
#line 187
struct v2f_img {
    highp vec4 pos;
    mediump vec2 uv;
};
#line 181
struct appdata_img {
    highp vec4 vertex;
    mediump vec2 texcoord;
};
#line 315
struct SurfaceOutput {
    lowp vec3 Albedo;
    lowp vec3 Normal;
    lowp vec3 Emission;
    mediump float Specular;
    lowp float Gloss;
    lowp float Alpha;
};
#line 401
struct Input {
    mediump vec3 viewDir;
    highp vec3 worldRefl;
    mediump vec3 TtoW0;
    mediump vec3 TtoW1;
    mediump vec3 TtoW2;
};
#line 434
struct v2f_surf {
    highp vec4 pos;
    highp vec3 viewDir;
    lowp vec4 TtoW0;
    lowp vec4 TtoW1;
    lowp vec4 TtoW2;
    mediump vec3 lightDir;
    highp vec2 _LightCoord;
};
#line 67
struct appdata_full {
    highp vec4 vertex;
    highp vec4 tangent;
    highp vec3 normal;
    highp vec4 texcoord;
    highp vec4 texcoord1;
    lowp vec4 color;
};
uniform highp vec4 _Time;
uniform highp vec4 _SinTime;
#line 3
uniform highp vec4 _CosTime;
uniform highp vec4 unity_DeltaTime;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
#line 7
uniform highp vec4 _ScreenParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_CameraWorldClipPlanes[6];
uniform lowp vec4 _WorldSpaceLightPos0;
#line 11
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
#line 15
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp vec4 unity_LightAtten[8];
#line 19
uniform highp vec4 unity_SpotDirection[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
#line 23
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
#line 27
uniform highp vec3 unity_LightColor0;
uniform highp vec3 unity_LightColor1;
uniform highp vec3 unity_LightColor2;
uniform highp vec3 unity_LightColor3;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp vec4 unity_LightShadowBias;
#line 31
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform highp vec4 _LightShadowData;
#line 35
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
#line 39
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp mat4 glstate_matrix_transpose_modelview0;
#line 43
uniform highp mat4 glstate_matrix_texture0;
uniform highp mat4 glstate_matrix_texture1;
uniform highp mat4 glstate_matrix_texture2;
uniform highp mat4 glstate_matrix_texture3;
#line 47
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp mat4 unity_MatrixV;
uniform highp mat4 unity_MatrixVP;
#line 51
uniform lowp vec4 unity_ColorSpaceGrey;
#line 77
#line 82
#line 87
#line 91
#line 96
#line 120
#line 137
#line 158
#line 166
#line 193
#line 206
#line 215
#line 220
#line 229
#line 234
#line 243
#line 260
#line 265
#line 291
#line 299
#line 307
#line 311
#line 325
uniform lowp vec4 _LightColor0;
uniform lowp vec4 _SpecColor;
#line 338
#line 346
#line 360
uniform sampler2D _LightTexture0;
uniform highp mat4 _LightMatrix0;
#line 393
uniform samplerCube _Cube;
uniform mediump vec4 _ReflectColor;
uniform mediump vec4 _FresnelColor;
uniform mediump vec4 _Color;
#line 397
uniform mediump float _TransmissiveColor;
uniform mediump float _Shininess;
uniform mediump float _HdrPower;
uniform mediump float _HdrContrast;
#line 410
#line 445
#line 424
mediump vec4 LightingHalfLambertSpecular( in SurfaceOutput s, in mediump vec3 lightDir, in mediump vec3 viewDir, in mediump float atten ) {
    #line 426
    mediump float NdotL = ((dot( s.Normal, lightDir) * 0.5) + 0.5);
    mediump vec3 halfVector = (viewDir + s.Normal);
    mediump float spec = max( 0.0, (pow( dot( halfVector, s.Normal), (10.0 - s.Specular)) * s.Gloss));
    mediump vec4 c;
    #line 430
    c.xyz = (((((NdotL * s.Albedo) * _LightColor0.xyz) * atten) * 2.0) + ((_LightColor0.xyz * spec) * atten));
    c.w = s.Alpha;
    return c;
}
#line 410
void surf( in Input IN, inout SurfaceOutput o ) {
    o.Normal = vec3( 0.0, 0.0, 1.0);
    highp vec3 worldRefl = reflect( IN.worldRefl, vec3( dot( IN.TtoW0, o.Normal), dot( IN.TtoW1, o.Normal), dot( IN.TtoW2, o.Normal)));
    #line 414
    highp vec4 c = (texture( _Cube, worldRefl) * _ReflectColor);
    mediump vec3 HDR = vec3( (pow( (((c.x + c.y) + c.z) / 3.0), _HdrPower) * _HdrContrast));
    mediump float fresnel = (1.0 - dot( normalize(IN.viewDir), normalize(vec3( 0.0, 0.0, 1.0))));
    mediump vec4 fresnelLerp = mix( _Color, _FresnelColor, vec4( pow( fresnel, _TransmissiveColor)));
    #line 418
    mediump vec4 GlossLerp = mix( vec4( 0.0, 0.0, 0.0, 0.0), vec4( 1.0, 1.0, 1.0, 1.0), vec4( fresnel));
    o.Specular = _Shininess;
    o.Gloss = GlossLerp.x;
    o.Albedo = ((c.xyz + HDR) + fresnelLerp.xyz);
    #line 422
    o.Alpha = 1.0;
}
#line 464
lowp vec4 frag_surf( in v2f_surf IN ) {
    #line 466
    Input surfIN;
    surfIN.worldRefl = vec3( IN.TtoW0.w, IN.TtoW1.w, IN.TtoW2.w);
    surfIN.TtoW0 = IN.TtoW0.xyz;
    surfIN.TtoW1 = IN.TtoW1.xyz;
    #line 470
    surfIN.TtoW2 = IN.TtoW2.xyz;
    surfIN.viewDir = IN.viewDir;
    SurfaceOutput o;
    o.Albedo = vec3( 0.0);
    #line 474
    o.Emission = vec3( 0.0);
    o.Specular = 0.0;
    o.Alpha = 0.0;
    o.Gloss = 0.0;
    #line 478
    surf( surfIN, o);
    lowp vec3 lightDir = IN.lightDir;
    lowp vec4 c = LightingHalfLambertSpecular( o, lightDir, normalize(IN.viewDir), (texture( _LightTexture0, IN._LightCoord).w * 1.0));
    c.w = 0.0;
    #line 482
    return c;
}
in highp vec3 xlv_TEXCOORD0;
in lowp vec4 xlv_TEXCOORD1;
in lowp vec4 xlv_TEXCOORD2;
in lowp vec4 xlv_TEXCOORD3;
in mediump vec3 xlv_TEXCOORD4;
in highp vec2 xlv_TEXCOORD5;
void main() {
    lowp vec4 xl_retval;
    v2f_surf xlt_IN;
    xlt_IN.pos = vec4(0.0);
    xlt_IN.viewDir = vec3(xlv_TEXCOORD0);
    xlt_IN.TtoW0 = vec4(xlv_TEXCOORD1);
    xlt_IN.TtoW1 = vec4(xlv_TEXCOORD2);
    xlt_IN.TtoW2 = vec4(xlv_TEXCOORD3);
    xlt_IN.lightDir = vec3(xlv_TEXCOORD4);
    xlt_IN._LightCoord = vec2(xlv_TEXCOORD5);
    xl_retval = frag_surf( xlt_IN);
    gl_FragData[0] = vec4(xl_retval);
}


#endif"
}

}
Program "fp" {
// Fragment combos: 5
//   opengl - ALU: 36 to 49, TEX: 1 to 3
//   d3d9 - ALU: 43 to 54, TEX: 1 to 3
//   d3d11 - ALU: 32 to 44, TEX: 1 to 3, FLOW: 1 to 1
SubProgram "opengl " {
Keywords { "POINT" }
Vector 0 [_LightColor0]
Vector 1 [_ReflectColor]
Vector 2 [_FresnelColor]
Vector 3 [_Color]
Float 4 [_TransmissiveColor]
Float 5 [_Shininess]
Float 6 [_HdrPower]
Float 7 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
SetTexture 1 [_LightTexture0] 2D
"3.0-!!ARBfp1.0
# 43 ALU, 2 TEX
PARAM c[10] = { program.local[0..7],
		{ 0, 0.5, 2, 0.33333334 },
		{ 1, 10 } };
TEMP R0;
TEMP R1;
TEMP R2;
MOV R1.x, fragment.texcoord[1].z;
MOV R1.y, fragment.texcoord[2].z;
MOV R1.z, fragment.texcoord[3];
MOV R0.x, fragment.texcoord[1].w;
MOV R0.z, fragment.texcoord[3].w;
MOV R0.y, fragment.texcoord[2].w;
DP3 R0.w, R1, R0;
MUL R1.xyz, R1, R0.w;
MAD R0.xyz, -R1, c[8].z, R0;
TEX R0.xyz, R0, texture[0], CUBE;
MUL R0.xyz, R0, c[1];
ADD R0.w, R0.x, R0.y;
ADD R0.w, R0, R0.z;
MUL R0.w, R0, c[8];
POW R0.w, R0.w, c[6].x;
DP3 R1.x, fragment.texcoord[0], fragment.texcoord[0];
MAD R0.xyz, R0.w, c[7].x, R0;
RSQ R0.w, R1.x;
MAD R1.w, -R0, fragment.texcoord[0].z, c[9].x;
MOV R1.xyz, c[3];
POW R2.x, R1.w, c[4].x;
ADD R1.xyz, -R1, c[2];
MAD R1.xyz, R2.x, R1, c[3];
ADD R0.xyz, R0, R1;
DP3 R2.x, fragment.texcoord[4], fragment.texcoord[4];
RSQ R1.y, R2.x;
MOV R1.x, c[9].y;
MUL R1.y, R1, fragment.texcoord[4].z;
MAD R0.w, R0, fragment.texcoord[0].z, c[9].x;
ADD R1.x, R1, -c[5];
POW R1.x, R0.w, R1.x;
MAD R0.w, R1.y, c[8].y, c[8].y;
MUL R0.xyz, R0.w, R0;
MUL R0.w, R1, R1.x;
MAX R1.x, R0.w, c[8];
DP3 R0.w, fragment.texcoord[5], fragment.texcoord[5];
TEX R0.w, R0.w, texture[1], 2D;
MUL R0.xyz, R0, c[0];
MUL R1.xyz, R1.x, c[0];
MUL R1.xyz, R0.w, R1;
MUL R0.xyz, R0, R0.w;
MAD result.color.xyz, R0, c[8].z, R1;
MOV result.color.w, c[8].x;
END
# 43 instructions, 3 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "POINT" }
Vector 0 [_LightColor0]
Vector 1 [_ReflectColor]
Vector 2 [_FresnelColor]
Vector 3 [_Color]
Float 4 [_TransmissiveColor]
Float 5 [_Shininess]
Float 6 [_HdrPower]
Float 7 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
SetTexture 1 [_LightTexture0] 2D
"ps_3_0
; 49 ALU, 2 TEX
dcl_cube s0
dcl_2d s1
def c8, 0.50000000, 2.00000000, 0.33333334, 1.00000000
def c9, 10.00000000, 0.00000000, 0, 0
dcl_texcoord0 v0.xyz
dcl_texcoord1 v1.xyzw
dcl_texcoord2 v2.xyzw
dcl_texcoord3 v3.xyzw
dcl_texcoord4 v4.xyz
dcl_texcoord5 v5.xyz
mov_pp r1.x, v1.z
mov_pp r1.y, v2.z
mov_pp r1.z, v3
mov r0.x, v1.w
mov r0.z, v3.w
mov r0.y, v2.w
dp3 r0.w, r1, r0
mul r1.xyz, r1, r0.w
mad r0.xyz, -r1, c8.y, r0
texld r0.xyz, r0, s0
mul r1.xyz, r0, c1
add r0.x, r1, r1.y
add r0.x, r0, r1.z
mul r1.w, r0.x, c8.z
pow r0, r1.w, c6.x
dp3_pp r2.x, v0, v0
rsq_pp r1.w, r2.x
mad_pp r2.w, -r1, v0.z, c8
mov r3.x, r0
pow_pp r0, r2.w, c4.x
mov_pp r2.xyz, c2
add_pp r2.xyz, -c3, r2
mad_pp r0.xyz, r0.x, r2, c3
mad r1.xyz, r3.x, c7.x, r1
add r1.xyz, r1, r0
mov_pp r0.x, c5
add_pp r2.y, c9.x, -r0.x
mad_pp r1.w, r1, v0.z, c8
pow_pp r0, r1.w, r2.y
dp3_pp r2.x, v4, v4
rsq_pp r0.y, r2.x
mov_pp r0.w, r0.x
mul_pp r0.y, r0, v4.z
mad_pp r0.x, r0.y, c8, c8
mul_pp r0.xyz, r0.x, r1
mul_pp r2.xyz, r0, c0
mul_pp r0.w, r2, r0
max_pp r0.y, r0.w, c9
dp3 r0.x, v5, v5
mul_pp r1.xyz, r0.y, c0
texld r0.x, r0.x, s1
mul_pp r1.xyz, r0.x, r1
mul_pp r0.xyz, r2, r0.x
mad_pp oC0.xyz, r0, c8.y, r1
mov_pp oC0.w, c9.y
"
}

SubProgram "d3d11 " {
Keywords { "POINT" }
ConstBuffer "$Globals" 176 // 176 used size, 11 vars
Vector 16 [_LightColor0] 4
Vector 112 [_ReflectColor] 4
Vector 128 [_FresnelColor] 4
Vector 144 [_Color] 4
Float 160 [_TransmissiveColor]
Float 164 [_Shininess]
Float 168 [_HdrPower]
Float 172 [_HdrContrast]
BindCB "$Globals" 0
SetTexture 0 [_Cube] CUBE 1
SetTexture 1 [_LightTexture0] 2D 0
// 48 instructions, 3 temp regs, 0 temp arrays:
// ALU 38 float, 0 int, 0 uint
// TEX 2 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0
eefiecedioiemahpgdeeopdnmmpcpmmclagidjgaabaaaaaacaahaaaaadaaaaaa
cmaaaaaapmaaaaaadaabaaaaejfdeheomiaaaaaaahaaaaaaaiaaaaaalaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaalmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahahaaaalmaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apamaaaalmaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaaapamaaaalmaaaaaa
adaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapamaaaalmaaaaaaaeaaaaaaaaaaaaaa
adaaaaaaafaaaaaaahahaaaalmaaaaaaafaaaaaaaaaaaaaaadaaaaaaagaaaaaa
ahahaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcoiafaaaaeaaaaaaahkabaaaa
fjaaaaaeegiocaaaaaaaaaaaalaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaad
aagabaaaabaaaaaafidaaaaeaahabaaaaaaaaaaaffffaaaafibiaaaeaahabaaa
abaaaaaaffffaaaagcbaaaadhcbabaaaabaaaaaagcbaaaadmcbabaaaacaaaaaa
gcbaaaadmcbabaaaadaaaaaagcbaaaadmcbabaaaaeaaaaaagcbaaaadhcbabaaa
afaaaaaagcbaaaadhcbabaaaagaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaac
adaaaaaadgaaaaafbcaabaaaaaaaaaaadkbabaaaacaaaaaadgaaaaafccaabaaa
aaaaaaaadkbabaaaadaaaaaadgaaaaafecaabaaaaaaaaaaadkbabaaaaeaaaaaa
dgaaaaafbcaabaaaabaaaaaackbabaaaacaaaaaadgaaaaafccaabaaaabaaaaaa
ckbabaaaadaaaaaadgaaaaafecaabaaaabaaaaaackbabaaaaeaaaaaabaaaaaah
icaabaaaaaaaaaaaegacbaaaaaaaaaaaegacbaaaabaaaaaaaaaaaaahicaabaaa
aaaaaaaadkaabaaaaaaaaaaadkaabaaaaaaaaaaadcaaaaakhcaabaaaaaaaaaaa
egacbaaaabaaaaaapgapbaiaebaaaaaaaaaaaaaaegacbaaaaaaaaaaaefaaaaaj
pcaabaaaaaaaaaaaegacbaaaaaaaaaaaeghobaaaaaaaaaaaaagabaaaabaaaaaa
diaaaaailcaabaaaaaaaaaaaegaibaaaaaaaaaaaegiicaaaaaaaaaaaahaaaaaa
aaaaaaahbcaabaaaabaaaaaabkaabaaaaaaaaaaaakaabaaaaaaaaaaadcaaaaak
ecaabaaaaaaaaaaackaabaaaaaaaaaaackiacaaaaaaaaaaaahaaaaaaakaabaaa
abaaaaaadiaaaaahecaabaaaaaaaaaaackaabaaaaaaaaaaaabeaaaaaklkkkkdo
cpaaaaafecaabaaaaaaaaaaackaabaaaaaaaaaaadiaaaaaiecaabaaaaaaaaaaa
ckaabaaaaaaaaaaackiacaaaaaaaaaaaakaaaaaabjaaaaafecaabaaaaaaaaaaa
ckaabaaaaaaaaaaadcaaaaakhcaabaaaaaaaaaaakgakbaaaaaaaaaaapgipcaaa
aaaaaaaaakaaaaaaegadbaaaaaaaaaaabaaaaaahicaabaaaaaaaaaaaegbcbaaa
abaaaaaaegbcbaaaabaaaaaaeeaaaaaficaabaaaaaaaaaaadkaabaaaaaaaaaaa
dcaaaaakbcaabaaaabaaaaaackbabaiaebaaaaaaabaaaaaadkaabaaaaaaaaaaa
abeaaaaaaaaaiadpdcaaaaajicaabaaaaaaaaaaackbabaaaabaaaaaadkaabaaa
aaaaaaaaabeaaaaaaaaaiadpcpaaaaaficaabaaaaaaaaaaadkaabaaaaaaaaaaa
cpaaaaafccaabaaaabaaaaaaakaabaaaabaaaaaadiaaaaaiccaabaaaabaaaaaa
bkaabaaaabaaaaaaakiacaaaaaaaaaaaakaaaaaabjaaaaafccaabaaaabaaaaaa
bkaabaaaabaaaaaaaaaaaaakhcaabaaaacaaaaaaegiccaaaaaaaaaaaaiaaaaaa
egiccaiaebaaaaaaaaaaaaaaajaaaaaadcaaaaakocaabaaaabaaaaaafgafbaaa
abaaaaaaagajbaaaacaaaaaaagijcaaaaaaaaaaaajaaaaaaaaaaaaahhcaabaaa
aaaaaaaaegacbaaaaaaaaaaajgahbaaaabaaaaaabaaaaaahccaabaaaabaaaaaa
egbcbaaaafaaaaaaegbcbaaaafaaaaaaeeaaaaafccaabaaaabaaaaaabkaabaaa
abaaaaaadiaaaaahccaabaaaabaaaaaabkaabaaaabaaaaaackbabaaaafaaaaaa
dcaaaaajccaabaaaabaaaaaabkaabaaaabaaaaaaabeaaaaaaaaaaadpabeaaaaa
aaaaaadpdiaaaaahhcaabaaaaaaaaaaaegacbaaaaaaaaaaafgafbaaaabaaaaaa
diaaaaaihcaabaaaaaaaaaaaegacbaaaaaaaaaaaegiccaaaaaaaaaaaabaaaaaa
baaaaaahccaabaaaabaaaaaaegbcbaaaagaaaaaaegbcbaaaagaaaaaaefaaaaaj
pcaabaaaacaaaaaafgafbaaaabaaaaaaeghobaaaabaaaaaaaagabaaaaaaaaaaa
diaaaaahhcaabaaaaaaaaaaaegacbaaaaaaaaaaaagaabaaaacaaaaaaaaaaaaah
hcaabaaaaaaaaaaaegacbaaaaaaaaaaaegacbaaaaaaaaaaaaaaaaaajccaabaaa
abaaaaaabkiacaiaebaaaaaaaaaaaaaaakaaaaaaabeaaaaaaaaacaebdiaaaaah
icaabaaaaaaaaaaadkaabaaaaaaaaaaabkaabaaaabaaaaaabjaaaaaficaabaaa
aaaaaaaadkaabaaaaaaaaaaadiaaaaahicaabaaaaaaaaaaaakaabaaaabaaaaaa
dkaabaaaaaaaaaaadeaaaaahicaabaaaaaaaaaaadkaabaaaaaaaaaaaabeaaaaa
aaaaaaaadiaaaaaihcaabaaaabaaaaaapgapbaaaaaaaaaaaegiccaaaaaaaaaaa
abaaaaaadcaaaaajhccabaaaaaaaaaaaegacbaaaabaaaaaaagaabaaaacaaaaaa
egacbaaaaaaaaaaadgaaaaaficcabaaaaaaaaaaaabeaaaaaaaaaaaaadoaaaaab
"
}

SubProgram "gles " {
Keywords { "POINT" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "POINT" }
"!!GLES"
}

SubProgram "gles3 " {
Keywords { "POINT" }
"!!GLES3"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL" }
Vector 0 [_LightColor0]
Vector 1 [_ReflectColor]
Vector 2 [_FresnelColor]
Vector 3 [_Color]
Float 4 [_TransmissiveColor]
Float 5 [_Shininess]
Float 6 [_HdrPower]
Float 7 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
"3.0-!!ARBfp1.0
# 36 ALU, 1 TEX
PARAM c[10] = { program.local[0..7],
		{ 0, 0.5, 2, 0.33333334 },
		{ 1, 10 } };
TEMP R0;
TEMP R1;
TEMP R2;
MOV R1.w, c[9].y;
MOV R1.x, fragment.texcoord[1].z;
MOV R1.y, fragment.texcoord[2].z;
MOV R1.z, fragment.texcoord[3];
MOV R0.x, fragment.texcoord[1].w;
MOV R0.z, fragment.texcoord[3].w;
MOV R0.y, fragment.texcoord[2].w;
DP3 R0.w, R1, R0;
MUL R1.xyz, R1, R0.w;
MAD R0.xyz, -R1, c[8].z, R0;
TEX R0.xyz, R0, texture[0], CUBE;
MUL R0.xyz, R0, c[1];
ADD R0.w, R0.x, R0.y;
ADD R0.w, R0, R0.z;
MUL R0.w, R0, c[8];
POW R0.w, R0.w, c[6].x;
MAD R0.xyz, R0.w, c[7].x, R0;
DP3 R0.w, fragment.texcoord[0], fragment.texcoord[0];
RSQ R2.x, R0.w;
ADD R2.y, R1.w, -c[5].x;
MOV R1.xyz, c[3];
MAD R0.w, -R2.x, fragment.texcoord[0].z, c[9].x;
MAD R1.w, R2.x, fragment.texcoord[0].z, c[9].x;
POW R2.x, R1.w, R2.y;
POW R1.w, R0.w, c[4].x;
ADD R1.xyz, -R1, c[2];
MAD R1.xyz, R1.w, R1, c[3];
ADD R1.xyz, R0, R1;
MAD R0.x, fragment.texcoord[4].z, c[8].y, c[8].y;
MUL R1.xyz, R0.x, R1;
MUL R0.w, R0, R2.x;
MAX R0.y, R0.w, c[8].x;
MUL R0.xyz, R0.y, c[0];
MUL R1.xyz, R1, c[0];
MAD result.color.xyz, R1, c[8].z, R0;
MOV result.color.w, c[8].x;
END
# 36 instructions, 3 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL" }
Vector 0 [_LightColor0]
Vector 1 [_ReflectColor]
Vector 2 [_FresnelColor]
Vector 3 [_Color]
Float 4 [_TransmissiveColor]
Float 5 [_Shininess]
Float 6 [_HdrPower]
Float 7 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
"ps_3_0
; 43 ALU, 1 TEX
dcl_cube s0
def c8, 0.50000000, 2.00000000, 0.33333334, 1.00000000
def c9, 10.00000000, 0.00000000, 0, 0
dcl_texcoord0 v0.xyz
dcl_texcoord1 v1.xyzw
dcl_texcoord2 v2.xyzw
dcl_texcoord3 v3.xyzw
dcl_texcoord4 v4.xyz
mov_pp r1.x, v1.z
mov_pp r1.y, v2.z
mov_pp r1.z, v3
mov r0.x, v1.w
mov r0.z, v3.w
mov r0.y, v2.w
dp3 r0.w, r1, r0
mul r1.xyz, r1, r0.w
mad r0.xyz, -r1, c8.y, r0
texld r0.xyz, r0, s0
mul r1.xyz, r0, c1
add r0.x, r1, r1.y
add r0.x, r0, r1.z
mul r1.w, r0.x, c8.z
pow r0, r1.w, c6.x
mov r2.x, r0
dp3_pp r0.x, v0, v0
rsq_pp r1.w, r0.x
mad_pp r2.y, r1.w, v0.z, c8.w
mov_pp r0.y, c5.x
add_pp r2.z, c9.x, -r0.y
pow_pp r0, r2.y, r2.z
mad r1.xyz, r2.x, c7.x, r1
mov_pp r2.xyz, c2
mad_pp r1.w, -r1, v0.z, c8
mov_pp r2.w, r0.x
pow_pp r0, r1.w, c4.x
add_pp r2.xyz, -c3, r2
mad_pp r0.xyz, r0.x, r2, c3
add r1.xyz, r1, r0
mad_pp r0.x, v4.z, c8, c8
mul_pp r1.xyz, r0.x, r1
mul_pp r0.w, r1, r2
max_pp r0.y, r0.w, c9
mul_pp r0.xyz, r0.y, c0
mul_pp r1.xyz, r1, c0
mad_pp oC0.xyz, r1, c8.y, r0
mov_pp oC0.w, c9.y
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL" }
ConstBuffer "$Globals" 112 // 112 used size, 10 vars
Vector 16 [_LightColor0] 4
Vector 48 [_ReflectColor] 4
Vector 64 [_FresnelColor] 4
Vector 80 [_Color] 4
Float 96 [_TransmissiveColor]
Float 100 [_Shininess]
Float 104 [_HdrPower]
Float 108 [_HdrContrast]
BindCB "$Globals" 0
SetTexture 0 [_Cube] CUBE 0
// 41 instructions, 3 temp regs, 0 temp arrays:
// ALU 32 float, 0 int, 0 uint
// TEX 1 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0
eefieceddmhlemkconlpejkdkofomogejpnddjeaabaaaaaabmagaaaaadaaaaaa
cmaaaaaaoeaaaaaabiabaaaaejfdeheolaaaaaaaagaaaaaaaiaaaaaajiaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaakeaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahahaaaakeaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apamaaaakeaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaaapamaaaakeaaaaaa
adaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapamaaaakeaaaaaaaeaaaaaaaaaaaaaa
adaaaaaaafaaaaaaahaeaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklklepfdeheocmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaaaaaaaaaapaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcpmaeaaaa
eaaaaaaadpabaaaafjaaaaaeegiocaaaaaaaaaaaahaaaaaafkaaaaadaagabaaa
aaaaaaaafidaaaaeaahabaaaaaaaaaaaffffaaaagcbaaaadhcbabaaaabaaaaaa
gcbaaaadmcbabaaaacaaaaaagcbaaaadmcbabaaaadaaaaaagcbaaaadmcbabaaa
aeaaaaaagcbaaaadecbabaaaafaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaac
adaaaaaadgaaaaafbcaabaaaaaaaaaaadkbabaaaacaaaaaadgaaaaafccaabaaa
aaaaaaaadkbabaaaadaaaaaadgaaaaafecaabaaaaaaaaaaadkbabaaaaeaaaaaa
dgaaaaafbcaabaaaabaaaaaackbabaaaacaaaaaadgaaaaafccaabaaaabaaaaaa
ckbabaaaadaaaaaadgaaaaafecaabaaaabaaaaaackbabaaaaeaaaaaabaaaaaah
icaabaaaaaaaaaaaegacbaaaaaaaaaaaegacbaaaabaaaaaaaaaaaaahicaabaaa
aaaaaaaadkaabaaaaaaaaaaadkaabaaaaaaaaaaadcaaaaakhcaabaaaaaaaaaaa
egacbaaaabaaaaaapgapbaiaebaaaaaaaaaaaaaaegacbaaaaaaaaaaaefaaaaaj
pcaabaaaaaaaaaaaegacbaaaaaaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaa
diaaaaailcaabaaaaaaaaaaaegaibaaaaaaaaaaaegiicaaaaaaaaaaaadaaaaaa
aaaaaaahbcaabaaaabaaaaaabkaabaaaaaaaaaaaakaabaaaaaaaaaaadcaaaaak
ecaabaaaaaaaaaaackaabaaaaaaaaaaackiacaaaaaaaaaaaadaaaaaaakaabaaa
abaaaaaadiaaaaahecaabaaaaaaaaaaackaabaaaaaaaaaaaabeaaaaaklkkkkdo
cpaaaaafecaabaaaaaaaaaaackaabaaaaaaaaaaadiaaaaaiecaabaaaaaaaaaaa
ckaabaaaaaaaaaaackiacaaaaaaaaaaaagaaaaaabjaaaaafecaabaaaaaaaaaaa
ckaabaaaaaaaaaaadcaaaaakhcaabaaaaaaaaaaakgakbaaaaaaaaaaapgipcaaa
aaaaaaaaagaaaaaaegadbaaaaaaaaaaabaaaaaahicaabaaaaaaaaaaaegbcbaaa
abaaaaaaegbcbaaaabaaaaaaeeaaaaaficaabaaaaaaaaaaadkaabaaaaaaaaaaa
dcaaaaakbcaabaaaabaaaaaackbabaiaebaaaaaaabaaaaaadkaabaaaaaaaaaaa
abeaaaaaaaaaiadpdcaaaaajicaabaaaaaaaaaaackbabaaaabaaaaaadkaabaaa
aaaaaaaaabeaaaaaaaaaiadpcpaaaaaficaabaaaaaaaaaaadkaabaaaaaaaaaaa
cpaaaaafccaabaaaabaaaaaaakaabaaaabaaaaaadiaaaaaiccaabaaaabaaaaaa
bkaabaaaabaaaaaaakiacaaaaaaaaaaaagaaaaaabjaaaaafccaabaaaabaaaaaa
bkaabaaaabaaaaaaaaaaaaakhcaabaaaacaaaaaaegiccaaaaaaaaaaaaeaaaaaa
egiccaiaebaaaaaaaaaaaaaaafaaaaaadcaaaaakocaabaaaabaaaaaafgafbaaa
abaaaaaaagajbaaaacaaaaaaagijcaaaaaaaaaaaafaaaaaaaaaaaaahhcaabaaa
aaaaaaaaegacbaaaaaaaaaaajgahbaaaabaaaaaadcaaaaajccaabaaaabaaaaaa
ckbabaaaafaaaaaaabeaaaaaaaaaaadpabeaaaaaaaaaaadpdiaaaaahhcaabaaa
aaaaaaaaegacbaaaaaaaaaaafgafbaaaabaaaaaadiaaaaaihcaabaaaaaaaaaaa
egacbaaaaaaaaaaaegiccaaaaaaaaaaaabaaaaaaaaaaaaahhcaabaaaaaaaaaaa
egacbaaaaaaaaaaaegacbaaaaaaaaaaaaaaaaaajccaabaaaabaaaaaabkiacaia
ebaaaaaaaaaaaaaaagaaaaaaabeaaaaaaaaacaebdiaaaaahicaabaaaaaaaaaaa
dkaabaaaaaaaaaaabkaabaaaabaaaaaabjaaaaaficaabaaaaaaaaaaadkaabaaa
aaaaaaaadiaaaaahicaabaaaaaaaaaaaakaabaaaabaaaaaadkaabaaaaaaaaaaa
deaaaaahicaabaaaaaaaaaaadkaabaaaaaaaaaaaabeaaaaaaaaaaaaadcaaaaak
hccabaaaaaaaaaaaegiccaaaaaaaaaaaabaaaaaapgapbaaaaaaaaaaaegacbaaa
aaaaaaaadgaaaaaficcabaaaaaaaaaaaabeaaaaaaaaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL" }
"!!GLES"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL" }
"!!GLES3"
}

SubProgram "opengl " {
Keywords { "SPOT" }
Vector 0 [_LightColor0]
Vector 1 [_ReflectColor]
Vector 2 [_FresnelColor]
Vector 3 [_Color]
Float 4 [_TransmissiveColor]
Float 5 [_Shininess]
Float 6 [_HdrPower]
Float 7 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
SetTexture 1 [_LightTexture0] 2D
SetTexture 2 [_LightTextureB0] 2D
"3.0-!!ARBfp1.0
# 49 ALU, 3 TEX
PARAM c[10] = { program.local[0..7],
		{ 0, 0.5, 2, 0.33333334 },
		{ 1, 10 } };
TEMP R0;
TEMP R1;
TEMP R2;
MOV R1.x, fragment.texcoord[1].z;
MOV R1.y, fragment.texcoord[2].z;
MOV R1.z, fragment.texcoord[3];
MOV R0.x, fragment.texcoord[1].w;
MOV R0.z, fragment.texcoord[3].w;
MOV R0.y, fragment.texcoord[2].w;
DP3 R0.w, R1, R0;
MUL R1.xyz, R1, R0.w;
MAD R0.xyz, -R1, c[8].z, R0;
TEX R0.xyz, R0, texture[0], CUBE;
MUL R0.xyz, R0, c[1];
ADD R0.w, R0.x, R0.y;
ADD R0.w, R0, R0.z;
MUL R0.w, R0, c[8];
POW R0.w, R0.w, c[6].x;
DP3 R1.x, fragment.texcoord[0], fragment.texcoord[0];
MAD R0.xyz, R0.w, c[7].x, R0;
RSQ R0.w, R1.x;
MAD R1.w, -R0, fragment.texcoord[0].z, c[9].x;
MOV R1.xyz, c[3];
POW R2.x, R1.w, c[4].x;
ADD R1.xyz, -R1, c[2];
MAD R1.xyz, R2.x, R1, c[3];
ADD R0.xyz, R0, R1;
DP3 R2.x, fragment.texcoord[4], fragment.texcoord[4];
RSQ R1.y, R2.x;
MOV R1.x, c[9].y;
MUL R1.y, R1, fragment.texcoord[4].z;
MAD R0.w, R0, fragment.texcoord[0].z, c[9].x;
ADD R1.x, R1, -c[5];
POW R1.x, R0.w, R1.x;
MAD R0.w, R1.y, c[8].y, c[8].y;
MUL R0.xyz, R0.w, R0;
MUL R0.w, R1, R1.x;
MAX R1.x, R0.w, c[8];
RCP R0.w, fragment.texcoord[5].w;
MAD R2.xy, fragment.texcoord[5], R0.w, c[8].y;
TEX R0.w, R2, texture[1], 2D;
DP3 R1.w, fragment.texcoord[5], fragment.texcoord[5];
SLT R2.x, c[8], fragment.texcoord[5].z;
MUL R0.xyz, R0, c[0];
MUL R1.xyz, R1.x, c[0];
TEX R1.w, R1.w, texture[2], 2D;
MUL R0.w, R2.x, R0;
MUL R0.w, R0, R1;
MUL R1.xyz, R0.w, R1;
MUL R0.xyz, R0, R0.w;
MAD result.color.xyz, R0, c[8].z, R1;
MOV result.color.w, c[8].x;
END
# 49 instructions, 3 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "SPOT" }
Vector 0 [_LightColor0]
Vector 1 [_ReflectColor]
Vector 2 [_FresnelColor]
Vector 3 [_Color]
Float 4 [_TransmissiveColor]
Float 5 [_Shininess]
Float 6 [_HdrPower]
Float 7 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
SetTexture 1 [_LightTexture0] 2D
SetTexture 2 [_LightTextureB0] 2D
"ps_3_0
; 54 ALU, 3 TEX
dcl_cube s0
dcl_2d s1
dcl_2d s2
def c8, 0.50000000, 2.00000000, 0.33333334, 1.00000000
def c9, 0.00000000, 1.00000000, 10.00000000, 0
dcl_texcoord0 v0.xyz
dcl_texcoord1 v1.xyzw
dcl_texcoord2 v2.xyzw
dcl_texcoord3 v3.xyzw
dcl_texcoord4 v4.xyz
dcl_texcoord5 v5
mov_pp r1.x, v1.z
mov_pp r1.y, v2.z
mov_pp r1.z, v3
mov r0.x, v1.w
mov r0.z, v3.w
mov r0.y, v2.w
dp3 r0.w, r1, r0
mul r1.xyz, r1, r0.w
mad r0.xyz, -r1, c8.y, r0
texld r0.xyz, r0, s0
mul r1.xyz, r0, c1
add r0.x, r1, r1.y
add r0.x, r0, r1.z
mul r1.w, r0.x, c8.z
pow r0, r1.w, c6.x
dp3_pp r2.x, v0, v0
rsq_pp r1.w, r2.x
mad_pp r2.w, -r1, v0.z, c8
mov r3.x, r0
pow_pp r0, r2.w, c4.x
mov_pp r2.xyz, c2
add_pp r2.xyz, -c3, r2
mad_pp r0.xyz, r0.x, r2, c3
mad r1.xyz, r3.x, c7.x, r1
add r1.xyz, r1, r0
mov_pp r0.x, c5
add_pp r2.y, c9.z, -r0.x
mad_pp r1.w, r1, v0.z, c8
pow_pp r0, r1.w, r2.y
dp3_pp r2.x, v4, v4
rsq_pp r0.y, r2.x
mov_pp r0.w, r0.x
mul_pp r0.y, r0, v4.z
mad_pp r0.x, r0.y, c8, c8
mul_pp r0.xyz, r0.x, r1
mul_pp r1.xyz, r0, c0
mul_pp r0.w, r2, r0
max_pp r0.y, r0.w, c9.x
rcp r0.x, v5.w
mad r3.xy, v5, r0.x, c8.x
mul_pp r2.xyz, r0.y, c0
dp3 r0.x, v5, v5
texld r0.w, r3, s1
cmp r0.y, -v5.z, c9.x, c9
mul_pp r0.y, r0, r0.w
texld r0.x, r0.x, s2
mul_pp r0.w, r0.y, r0.x
mul_pp r0.xyz, r0.w, r2
mul_pp r1.xyz, r1, r0.w
mad_pp oC0.xyz, r1, c8.y, r0
mov_pp oC0.w, c9.x
"
}

SubProgram "d3d11 " {
Keywords { "SPOT" }
ConstBuffer "$Globals" 176 // 176 used size, 11 vars
Vector 16 [_LightColor0] 4
Vector 112 [_ReflectColor] 4
Vector 128 [_FresnelColor] 4
Vector 144 [_Color] 4
Float 160 [_TransmissiveColor]
Float 164 [_Shininess]
Float 168 [_HdrPower]
Float 172 [_HdrContrast]
BindCB "$Globals" 0
SetTexture 0 [_Cube] CUBE 2
SetTexture 1 [_LightTexture0] 2D 0
SetTexture 2 [_LightTextureB0] 2D 1
// 55 instructions, 3 temp regs, 0 temp arrays:
// ALU 43 float, 0 int, 1 uint
// TEX 3 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0
eefiecedkdmibiedokgbflplalhpgionbinlindhabaaaaaabeaiaaaaadaaaaaa
cmaaaaaapmaaaaaadaabaaaaejfdeheomiaaaaaaahaaaaaaaiaaaaaalaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaalmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahahaaaalmaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apamaaaalmaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaaapamaaaalmaaaaaa
adaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapamaaaalmaaaaaaaeaaaaaaaaaaaaaa
adaaaaaaafaaaaaaahahaaaalmaaaaaaafaaaaaaaaaaaaaaadaaaaaaagaaaaaa
apapaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcnmagaaaaeaaaaaaalhabaaaa
fjaaaaaeegiocaaaaaaaaaaaalaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaad
aagabaaaabaaaaaafkaaaaadaagabaaaacaaaaaafidaaaaeaahabaaaaaaaaaaa
ffffaaaafibiaaaeaahabaaaabaaaaaaffffaaaafibiaaaeaahabaaaacaaaaaa
ffffaaaagcbaaaadhcbabaaaabaaaaaagcbaaaadmcbabaaaacaaaaaagcbaaaad
mcbabaaaadaaaaaagcbaaaadmcbabaaaaeaaaaaagcbaaaadhcbabaaaafaaaaaa
gcbaaaadpcbabaaaagaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacadaaaaaa
dgaaaaafbcaabaaaaaaaaaaadkbabaaaacaaaaaadgaaaaafccaabaaaaaaaaaaa
dkbabaaaadaaaaaadgaaaaafecaabaaaaaaaaaaadkbabaaaaeaaaaaadgaaaaaf
bcaabaaaabaaaaaackbabaaaacaaaaaadgaaaaafccaabaaaabaaaaaackbabaaa
adaaaaaadgaaaaafecaabaaaabaaaaaackbabaaaaeaaaaaabaaaaaahicaabaaa
aaaaaaaaegacbaaaaaaaaaaaegacbaaaabaaaaaaaaaaaaahicaabaaaaaaaaaaa
dkaabaaaaaaaaaaadkaabaaaaaaaaaaadcaaaaakhcaabaaaaaaaaaaaegacbaaa
abaaaaaapgapbaiaebaaaaaaaaaaaaaaegacbaaaaaaaaaaaefaaaaajpcaabaaa
aaaaaaaaegacbaaaaaaaaaaaeghobaaaaaaaaaaaaagabaaaacaaaaaadiaaaaai
lcaabaaaaaaaaaaaegaibaaaaaaaaaaaegiicaaaaaaaaaaaahaaaaaaaaaaaaah
bcaabaaaabaaaaaabkaabaaaaaaaaaaaakaabaaaaaaaaaaadcaaaaakecaabaaa
aaaaaaaackaabaaaaaaaaaaackiacaaaaaaaaaaaahaaaaaaakaabaaaabaaaaaa
diaaaaahecaabaaaaaaaaaaackaabaaaaaaaaaaaabeaaaaaklkkkkdocpaaaaaf
ecaabaaaaaaaaaaackaabaaaaaaaaaaadiaaaaaiecaabaaaaaaaaaaackaabaaa
aaaaaaaackiacaaaaaaaaaaaakaaaaaabjaaaaafecaabaaaaaaaaaaackaabaaa
aaaaaaaadcaaaaakhcaabaaaaaaaaaaakgakbaaaaaaaaaaapgipcaaaaaaaaaaa
akaaaaaaegadbaaaaaaaaaaabaaaaaahicaabaaaaaaaaaaaegbcbaaaabaaaaaa
egbcbaaaabaaaaaaeeaaaaaficaabaaaaaaaaaaadkaabaaaaaaaaaaadcaaaaak
bcaabaaaabaaaaaackbabaiaebaaaaaaabaaaaaadkaabaaaaaaaaaaaabeaaaaa
aaaaiadpdcaaaaajicaabaaaaaaaaaaackbabaaaabaaaaaadkaabaaaaaaaaaaa
abeaaaaaaaaaiadpcpaaaaaficaabaaaaaaaaaaadkaabaaaaaaaaaaacpaaaaaf
ccaabaaaabaaaaaaakaabaaaabaaaaaadiaaaaaiccaabaaaabaaaaaabkaabaaa
abaaaaaaakiacaaaaaaaaaaaakaaaaaabjaaaaafccaabaaaabaaaaaabkaabaaa
abaaaaaaaaaaaaakhcaabaaaacaaaaaaegiccaaaaaaaaaaaaiaaaaaaegiccaia
ebaaaaaaaaaaaaaaajaaaaaadcaaaaakocaabaaaabaaaaaafgafbaaaabaaaaaa
agajbaaaacaaaaaaagijcaaaaaaaaaaaajaaaaaaaaaaaaahhcaabaaaaaaaaaaa
egacbaaaaaaaaaaajgahbaaaabaaaaaabaaaaaahccaabaaaabaaaaaaegbcbaaa
afaaaaaaegbcbaaaafaaaaaaeeaaaaafccaabaaaabaaaaaabkaabaaaabaaaaaa
diaaaaahccaabaaaabaaaaaabkaabaaaabaaaaaackbabaaaafaaaaaadcaaaaaj
ccaabaaaabaaaaaabkaabaaaabaaaaaaabeaaaaaaaaaaadpabeaaaaaaaaaaadp
diaaaaahhcaabaaaaaaaaaaaegacbaaaaaaaaaaafgafbaaaabaaaaaadiaaaaai
hcaabaaaaaaaaaaaegacbaaaaaaaaaaaegiccaaaaaaaaaaaabaaaaaaaoaaaaah
gcaabaaaabaaaaaaagbbbaaaagaaaaaapgbpbaaaagaaaaaaaaaaaaakgcaabaaa
abaaaaaafgagbaaaabaaaaaaaceaaaaaaaaaaaaaaaaaaadpaaaaaadpaaaaaaaa
efaaaaajpcaabaaaacaaaaaajgafbaaaabaaaaaaeghobaaaabaaaaaaaagabaaa
aaaaaaaadbaaaaahccaabaaaabaaaaaaabeaaaaaaaaaaaaackbabaaaagaaaaaa
abaaaaahccaabaaaabaaaaaabkaabaaaabaaaaaaabeaaaaaaaaaiadpdiaaaaah
ccaabaaaabaaaaaadkaabaaaacaaaaaabkaabaaaabaaaaaabaaaaaahecaabaaa
abaaaaaaegbcbaaaagaaaaaaegbcbaaaagaaaaaaefaaaaajpcaabaaaacaaaaaa
kgakbaaaabaaaaaaeghobaaaacaaaaaaaagabaaaabaaaaaadiaaaaahccaabaaa
abaaaaaabkaabaaaabaaaaaaakaabaaaacaaaaaadiaaaaahhcaabaaaaaaaaaaa
egacbaaaaaaaaaaafgafbaaaabaaaaaaaaaaaaahhcaabaaaaaaaaaaaegacbaaa
aaaaaaaaegacbaaaaaaaaaaaaaaaaaajecaabaaaabaaaaaabkiacaiaebaaaaaa
aaaaaaaaakaaaaaaabeaaaaaaaaacaebdiaaaaahicaabaaaaaaaaaaadkaabaaa
aaaaaaaackaabaaaabaaaaaabjaaaaaficaabaaaaaaaaaaadkaabaaaaaaaaaaa
diaaaaahicaabaaaaaaaaaaaakaabaaaabaaaaaadkaabaaaaaaaaaaadeaaaaah
icaabaaaaaaaaaaadkaabaaaaaaaaaaaabeaaaaaaaaaaaaadiaaaaaincaabaaa
abaaaaaapgapbaaaaaaaaaaaagijcaaaaaaaaaaaabaaaaaadcaaaaajhccabaaa
aaaaaaaaigadbaaaabaaaaaafgafbaaaabaaaaaaegacbaaaaaaaaaaadgaaaaaf
iccabaaaaaaaaaaaabeaaaaaaaaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "SPOT" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "SPOT" }
"!!GLES"
}

SubProgram "gles3 " {
Keywords { "SPOT" }
"!!GLES3"
}

SubProgram "opengl " {
Keywords { "POINT_COOKIE" }
Vector 0 [_LightColor0]
Vector 1 [_ReflectColor]
Vector 2 [_FresnelColor]
Vector 3 [_Color]
Float 4 [_TransmissiveColor]
Float 5 [_Shininess]
Float 6 [_HdrPower]
Float 7 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
SetTexture 1 [_LightTextureB0] 2D
SetTexture 2 [_LightTexture0] CUBE
"3.0-!!ARBfp1.0
# 45 ALU, 3 TEX
PARAM c[10] = { program.local[0..7],
		{ 0, 0.5, 2, 0.33333334 },
		{ 1, 10 } };
TEMP R0;
TEMP R1;
TEMP R2;
MOV R1.x, fragment.texcoord[1].z;
MOV R1.y, fragment.texcoord[2].z;
MOV R1.z, fragment.texcoord[3];
MOV R0.x, fragment.texcoord[1].w;
MOV R0.z, fragment.texcoord[3].w;
MOV R0.y, fragment.texcoord[2].w;
DP3 R0.w, R1, R0;
MUL R1.xyz, R1, R0.w;
MAD R0.xyz, -R1, c[8].z, R0;
TEX R0.xyz, R0, texture[0], CUBE;
MUL R1.xyz, R0, c[1];
ADD R0.x, R1, R1.y;
ADD R0.x, R0, R1.z;
MUL R0.x, R0, c[8].w;
DP3 R0.y, fragment.texcoord[0], fragment.texcoord[0];
RSQ R0.w, R0.y;
MAD R1.w, -R0, fragment.texcoord[0].z, c[9].x;
POW R2.x, R0.x, c[6].x;
MOV R0.xyz, c[3];
MAD R1.xyz, R2.x, c[7].x, R1;
POW R2.y, R1.w, c[4].x;
ADD R0.xyz, -R0, c[2];
MAD R0.xyz, R2.y, R0, c[3];
ADD R0.xyz, R1, R0;
DP3 R1.y, fragment.texcoord[4], fragment.texcoord[4];
MOV R1.x, c[9].y;
RSQ R1.y, R1.y;
MAD R0.w, R0, fragment.texcoord[0].z, c[9].x;
ADD R1.x, R1, -c[5];
POW R1.x, R0.w, R1.x;
MUL R1.x, R1.w, R1;
MUL R0.w, R1.y, fragment.texcoord[4].z;
MAD R0.w, R0, c[8].y, c[8].y;
MUL R0.xyz, R0.w, R0;
MAX R0.w, R1.x, c[8].x;
MUL R1.xyz, R0.w, c[0];
DP3 R1.w, fragment.texcoord[5], fragment.texcoord[5];
MUL R0.xyz, R0, c[0];
TEX R0.w, fragment.texcoord[5], texture[2], CUBE;
TEX R1.w, R1.w, texture[1], 2D;
MUL R0.w, R1, R0;
MUL R1.xyz, R0.w, R1;
MUL R0.xyz, R0, R0.w;
MAD result.color.xyz, R0, c[8].z, R1;
MOV result.color.w, c[8].x;
END
# 45 instructions, 3 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "POINT_COOKIE" }
Vector 0 [_LightColor0]
Vector 1 [_ReflectColor]
Vector 2 [_FresnelColor]
Vector 3 [_Color]
Float 4 [_TransmissiveColor]
Float 5 [_Shininess]
Float 6 [_HdrPower]
Float 7 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
SetTexture 1 [_LightTextureB0] 2D
SetTexture 2 [_LightTexture0] CUBE
"ps_3_0
; 50 ALU, 3 TEX
dcl_cube s0
dcl_2d s1
dcl_cube s2
def c8, 0.50000000, 2.00000000, 0.33333334, 1.00000000
def c9, 10.00000000, 0.00000000, 0, 0
dcl_texcoord0 v0.xyz
dcl_texcoord1 v1.xyzw
dcl_texcoord2 v2.xyzw
dcl_texcoord3 v3.xyzw
dcl_texcoord4 v4.xyz
dcl_texcoord5 v5.xyz
mov_pp r2.xyz, c2
add_pp r2.xyz, -c3, r2
mov_pp r1.x, v1.z
mov_pp r1.y, v2.z
mov_pp r1.z, v3
mov r0.x, v1.w
mov r0.z, v3.w
mov r0.y, v2.w
dp3 r0.w, r1, r0
mul r1.xyz, r1, r0.w
mad r0.xyz, -r1, c8.y, r0
texld r0.xyz, r0, s0
mul r0.xyz, r0, c1
add r0.w, r0.x, r0.y
add r0.w, r0, r0.z
mul r0.w, r0, c8.z
pow r1, r0.w, c6.x
dp3_pp r0.w, v0, v0
rsq_pp r2.w, r0.w
mov r0.w, r1.x
mad_pp r1.w, -r2, v0.z, c8
mad r1.xyz, r0.w, c7.x, r0
pow_pp r0, r1.w, c4.x
mad_pp r0.xyz, r0.x, r2, c3
mov_pp r0.w, c5.x
add r1.xyz, r1, r0
add_pp r2.y, c9.x, -r0.w
mad_pp r2.x, r2.w, v0.z, c8.w
pow_pp r0, r2.x, r2.y
dp3_pp r0.y, v4, v4
mov_pp r0.z, r0.x
rsq_pp r0.y, r0.y
mul_pp r0.x, r0.y, v4.z
mul_pp r0.w, r1, r0.z
max_pp r0.w, r0, c9.y
mul_pp r2.xyz, r0.w, c0
mad_pp r0.x, r0, c8, c8
mul_pp r0.xyz, r0.x, r1
mul_pp r1.xyz, r0, c0
dp3 r0.x, v5, v5
texld r0.x, r0.x, s1
texld r0.w, v5, s2
mul r0.w, r0.x, r0
mul_pp r0.xyz, r0.w, r2
mul_pp r1.xyz, r1, r0.w
mad_pp oC0.xyz, r1, c8.y, r0
mov_pp oC0.w, c9.y
"
}

SubProgram "d3d11 " {
Keywords { "POINT_COOKIE" }
ConstBuffer "$Globals" 176 // 176 used size, 11 vars
Vector 16 [_LightColor0] 4
Vector 112 [_ReflectColor] 4
Vector 128 [_FresnelColor] 4
Vector 144 [_Color] 4
Float 160 [_TransmissiveColor]
Float 164 [_Shininess]
Float 168 [_HdrPower]
Float 172 [_HdrContrast]
BindCB "$Globals" 0
SetTexture 0 [_Cube] CUBE 2
SetTexture 1 [_LightTextureB0] 2D 1
SetTexture 2 [_LightTexture0] CUBE 0
// 50 instructions, 4 temp regs, 0 temp arrays:
// ALU 39 float, 0 int, 0 uint
// TEX 3 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0
eefiecedgmphmhcliakghhcofcmhdgfokinojfemabaaaaaahmahaaaaadaaaaaa
cmaaaaaapmaaaaaadaabaaaaejfdeheomiaaaaaaahaaaaaaaiaaaaaalaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaalmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahahaaaalmaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apamaaaalmaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaaapamaaaalmaaaaaa
adaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapamaaaalmaaaaaaaeaaaaaaaaaaaaaa
adaaaaaaafaaaaaaahahaaaalmaaaaaaafaaaaaaaaaaaaaaadaaaaaaagaaaaaa
ahahaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklklfdeieefceeagaaaaeaaaaaaajbabaaaa
fjaaaaaeegiocaaaaaaaaaaaalaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaad
aagabaaaabaaaaaafkaaaaadaagabaaaacaaaaaafidaaaaeaahabaaaaaaaaaaa
ffffaaaafibiaaaeaahabaaaabaaaaaaffffaaaafidaaaaeaahabaaaacaaaaaa
ffffaaaagcbaaaadhcbabaaaabaaaaaagcbaaaadmcbabaaaacaaaaaagcbaaaad
mcbabaaaadaaaaaagcbaaaadmcbabaaaaeaaaaaagcbaaaadhcbabaaaafaaaaaa
gcbaaaadhcbabaaaagaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaacaeaaaaaa
dgaaaaafbcaabaaaaaaaaaaadkbabaaaacaaaaaadgaaaaafccaabaaaaaaaaaaa
dkbabaaaadaaaaaadgaaaaafecaabaaaaaaaaaaadkbabaaaaeaaaaaadgaaaaaf
bcaabaaaabaaaaaackbabaaaacaaaaaadgaaaaafccaabaaaabaaaaaackbabaaa
adaaaaaadgaaaaafecaabaaaabaaaaaackbabaaaaeaaaaaabaaaaaahicaabaaa
aaaaaaaaegacbaaaaaaaaaaaegacbaaaabaaaaaaaaaaaaahicaabaaaaaaaaaaa
dkaabaaaaaaaaaaadkaabaaaaaaaaaaadcaaaaakhcaabaaaaaaaaaaaegacbaaa
abaaaaaapgapbaiaebaaaaaaaaaaaaaaegacbaaaaaaaaaaaefaaaaajpcaabaaa
aaaaaaaaegacbaaaaaaaaaaaeghobaaaaaaaaaaaaagabaaaacaaaaaadiaaaaai
lcaabaaaaaaaaaaaegaibaaaaaaaaaaaegiicaaaaaaaaaaaahaaaaaaaaaaaaah
bcaabaaaabaaaaaabkaabaaaaaaaaaaaakaabaaaaaaaaaaadcaaaaakecaabaaa
aaaaaaaackaabaaaaaaaaaaackiacaaaaaaaaaaaahaaaaaaakaabaaaabaaaaaa
diaaaaahecaabaaaaaaaaaaackaabaaaaaaaaaaaabeaaaaaklkkkkdocpaaaaaf
ecaabaaaaaaaaaaackaabaaaaaaaaaaadiaaaaaiecaabaaaaaaaaaaackaabaaa
aaaaaaaackiacaaaaaaaaaaaakaaaaaabjaaaaafecaabaaaaaaaaaaackaabaaa
aaaaaaaadcaaaaakhcaabaaaaaaaaaaakgakbaaaaaaaaaaapgipcaaaaaaaaaaa
akaaaaaaegadbaaaaaaaaaaabaaaaaahicaabaaaaaaaaaaaegbcbaaaabaaaaaa
egbcbaaaabaaaaaaeeaaaaaficaabaaaaaaaaaaadkaabaaaaaaaaaaadcaaaaak
bcaabaaaabaaaaaackbabaiaebaaaaaaabaaaaaadkaabaaaaaaaaaaaabeaaaaa
aaaaiadpdcaaaaajicaabaaaaaaaaaaackbabaaaabaaaaaadkaabaaaaaaaaaaa
abeaaaaaaaaaiadpcpaaaaaficaabaaaaaaaaaaadkaabaaaaaaaaaaacpaaaaaf
ccaabaaaabaaaaaaakaabaaaabaaaaaadiaaaaaiccaabaaaabaaaaaabkaabaaa
abaaaaaaakiacaaaaaaaaaaaakaaaaaabjaaaaafccaabaaaabaaaaaabkaabaaa
abaaaaaaaaaaaaakhcaabaaaacaaaaaaegiccaaaaaaaaaaaaiaaaaaaegiccaia
ebaaaaaaaaaaaaaaajaaaaaadcaaaaakocaabaaaabaaaaaafgafbaaaabaaaaaa
agajbaaaacaaaaaaagijcaaaaaaaaaaaajaaaaaaaaaaaaahhcaabaaaaaaaaaaa
egacbaaaaaaaaaaajgahbaaaabaaaaaabaaaaaahccaabaaaabaaaaaaegbcbaaa
afaaaaaaegbcbaaaafaaaaaaeeaaaaafccaabaaaabaaaaaabkaabaaaabaaaaaa
diaaaaahccaabaaaabaaaaaabkaabaaaabaaaaaackbabaaaafaaaaaadcaaaaaj
ccaabaaaabaaaaaabkaabaaaabaaaaaaabeaaaaaaaaaaadpabeaaaaaaaaaaadp
diaaaaahhcaabaaaaaaaaaaaegacbaaaaaaaaaaafgafbaaaabaaaaaadiaaaaai
hcaabaaaaaaaaaaaegacbaaaaaaaaaaaegiccaaaaaaaaaaaabaaaaaabaaaaaah
ccaabaaaabaaaaaaegbcbaaaagaaaaaaegbcbaaaagaaaaaaefaaaaajpcaabaaa
acaaaaaafgafbaaaabaaaaaaeghobaaaabaaaaaaaagabaaaabaaaaaaefaaaaaj
pcaabaaaadaaaaaaegbcbaaaagaaaaaaeghobaaaacaaaaaaaagabaaaaaaaaaaa
diaaaaahccaabaaaabaaaaaaakaabaaaacaaaaaadkaabaaaadaaaaaadiaaaaah
hcaabaaaaaaaaaaaegacbaaaaaaaaaaafgafbaaaabaaaaaaaaaaaaahhcaabaaa
aaaaaaaaegacbaaaaaaaaaaaegacbaaaaaaaaaaaaaaaaaajecaabaaaabaaaaaa
bkiacaiaebaaaaaaaaaaaaaaakaaaaaaabeaaaaaaaaacaebdiaaaaahicaabaaa
aaaaaaaadkaabaaaaaaaaaaackaabaaaabaaaaaabjaaaaaficaabaaaaaaaaaaa
dkaabaaaaaaaaaaadiaaaaahicaabaaaaaaaaaaaakaabaaaabaaaaaadkaabaaa
aaaaaaaadeaaaaahicaabaaaaaaaaaaadkaabaaaaaaaaaaaabeaaaaaaaaaaaaa
diaaaaaincaabaaaabaaaaaapgapbaaaaaaaaaaaagijcaaaaaaaaaaaabaaaaaa
dcaaaaajhccabaaaaaaaaaaaigadbaaaabaaaaaafgafbaaaabaaaaaaegacbaaa
aaaaaaaadgaaaaaficcabaaaaaaaaaaaabeaaaaaaaaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "POINT_COOKIE" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "POINT_COOKIE" }
"!!GLES"
}

SubProgram "gles3 " {
Keywords { "POINT_COOKIE" }
"!!GLES3"
}

SubProgram "opengl " {
Keywords { "DIRECTIONAL_COOKIE" }
Vector 0 [_LightColor0]
Vector 1 [_ReflectColor]
Vector 2 [_FresnelColor]
Vector 3 [_Color]
Float 4 [_TransmissiveColor]
Float 5 [_Shininess]
Float 6 [_HdrPower]
Float 7 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
SetTexture 1 [_LightTexture0] 2D
"3.0-!!ARBfp1.0
# 39 ALU, 2 TEX
PARAM c[10] = { program.local[0..7],
		{ 0, 0.5, 2, 0.33333334 },
		{ 1, 10 } };
TEMP R0;
TEMP R1;
TEMP R2;
MOV R2.x, c[9].y;
MOV R1.x, fragment.texcoord[1].z;
MOV R1.y, fragment.texcoord[2].z;
MOV R1.z, fragment.texcoord[3];
MOV R0.x, fragment.texcoord[1].w;
MOV R0.z, fragment.texcoord[3].w;
MOV R0.y, fragment.texcoord[2].w;
DP3 R0.w, R1, R0;
MUL R1.xyz, R1, R0.w;
MAD R0.xyz, -R1, c[8].z, R0;
TEX R0.xyz, R0, texture[0], CUBE;
MUL R0.xyz, R0, c[1];
ADD R0.w, R0.x, R0.y;
ADD R0.w, R0, R0.z;
MUL R0.w, R0, c[8];
POW R0.w, R0.w, c[6].x;
MAD R0.xyz, R0.w, c[7].x, R0;
DP3 R0.w, fragment.texcoord[0], fragment.texcoord[0];
RSQ R0.w, R0.w;
MAD R1.w, -R0, fragment.texcoord[0].z, c[9].x;
MOV R1.xyz, c[3];
ADD R1.xyz, -R1, c[2];
POW R2.y, R1.w, c[4].x;
MAD R1.xyz, R2.y, R1, c[3];
ADD R0.xyz, R0, R1;
ADD R2.x, R2, -c[5];
MAD R0.w, R0, fragment.texcoord[0].z, c[9].x;
POW R0.w, R0.w, R2.x;
MUL R1.x, R1.w, R0.w;
MAD R0.w, fragment.texcoord[4].z, c[8].y, c[8].y;
MUL R0.xyz, R0.w, R0;
MAX R0.w, R1.x, c[8].x;
MUL R1.xyz, R0.w, c[0];
TEX R0.w, fragment.texcoord[5], texture[1], 2D;
MUL R0.xyz, R0, c[0];
MUL R1.xyz, R0.w, R1;
MUL R0.xyz, R0, R0.w;
MAD result.color.xyz, R0, c[8].z, R1;
MOV result.color.w, c[8].x;
END
# 39 instructions, 3 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "DIRECTIONAL_COOKIE" }
Vector 0 [_LightColor0]
Vector 1 [_ReflectColor]
Vector 2 [_FresnelColor]
Vector 3 [_Color]
Float 4 [_TransmissiveColor]
Float 5 [_Shininess]
Float 6 [_HdrPower]
Float 7 [_HdrContrast]
SetTexture 0 [_Cube] CUBE
SetTexture 1 [_LightTexture0] 2D
"ps_3_0
; 44 ALU, 2 TEX
dcl_cube s0
dcl_2d s1
def c8, 0.50000000, 2.00000000, 0.33333334, 1.00000000
def c9, 10.00000000, 0.00000000, 0, 0
dcl_texcoord0 v0.xyz
dcl_texcoord1 v1.xyzw
dcl_texcoord2 v2.xyzw
dcl_texcoord3 v3.xyzw
dcl_texcoord4 v4.xyz
dcl_texcoord5 v5.xy
mov_pp r1.x, v1.z
mov_pp r1.y, v2.z
mov_pp r1.z, v3
mov r0.x, v1.w
mov r0.z, v3.w
mov r0.y, v2.w
dp3 r0.w, r1, r0
mul r1.xyz, r1, r0.w
mad r0.xyz, -r1, c8.y, r0
texld r0.xyz, r0, s0
mul r1.xyz, r0, c1
add r0.x, r1, r1.y
add r0.x, r0, r1.z
mul r1.w, r0.x, c8.z
pow r0, r1.w, c6.x
mad r1.xyz, r0.x, c7.x, r1
dp3_pp r0.x, v0, v0
rsq_pp r1.w, r0.x
mad_pp r2.x, r1.w, v0.z, c8.w
mov_pp r0.y, c5.x
add_pp r2.y, c9.x, -r0
pow_pp r0, r2.x, r2.y
mad_pp r1.w, -r1, v0.z, c8
pow_pp r2, r1.w, c4.x
mov_pp r0.yzw, c2.xxyz
add_pp r0.yzw, -c3.xxyz, r0
mad_pp r2.xyz, r2.x, r0.yzww, c3
mov_pp r0.w, r0.x
add r0.xyz, r1, r2
mul_pp r1.x, r1.w, r0.w
mad_pp r0.w, v4.z, c8.x, c8.x
mul_pp r0.xyz, r0.w, r0
max_pp r0.w, r1.x, c9.y
mul_pp r1.xyz, r0.w, c0
texld r0.w, v5, s1
mul_pp r0.xyz, r0, c0
mul_pp r1.xyz, r0.w, r1
mul_pp r0.xyz, r0, r0.w
mad_pp oC0.xyz, r0, c8.y, r1
mov_pp oC0.w, c9.y
"
}

SubProgram "d3d11 " {
Keywords { "DIRECTIONAL_COOKIE" }
ConstBuffer "$Globals" 176 // 176 used size, 11 vars
Vector 16 [_LightColor0] 4
Vector 112 [_ReflectColor] 4
Vector 128 [_FresnelColor] 4
Vector 144 [_Color] 4
Float 160 [_TransmissiveColor]
Float 164 [_Shininess]
Float 168 [_HdrPower]
Float 172 [_HdrContrast]
BindCB "$Globals" 0
SetTexture 0 [_Cube] CUBE 1
SetTexture 1 [_LightTexture0] 2D 0
// 44 instructions, 3 temp regs, 0 temp arrays:
// ALU 34 float, 0 int, 0 uint
// TEX 2 (0 load, 0 comp, 0 bias, 0 grad)
// FLOW 1 static, 0 dynamic
"ps_4_0
eefiecedfmocllmffinemgohdmhdnebiijhjngneabaaaaaaliagaaaaadaaaaaa
cmaaaaaapmaaaaaadaabaaaaejfdeheomiaaaaaaahaaaaaaaiaaaaaalaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaalmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaahahaaaalmaaaaaaabaaaaaaaaaaaaaaadaaaaaaacaaaaaa
apamaaaalmaaaaaaacaaaaaaaaaaaaaaadaaaaaaadaaaaaaapamaaaalmaaaaaa
adaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapamaaaalmaaaaaaaeaaaaaaaaaaaaaa
adaaaaaaafaaaaaaahaeaaaalmaaaaaaafaaaaaaaaaaaaaaadaaaaaaagaaaaaa
adadaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklklfdeieefciaafaaaaeaaaaaaagaabaaaa
fjaaaaaeegiocaaaaaaaaaaaalaaaaaafkaaaaadaagabaaaaaaaaaaafkaaaaad
aagabaaaabaaaaaafidaaaaeaahabaaaaaaaaaaaffffaaaafibiaaaeaahabaaa
abaaaaaaffffaaaagcbaaaadhcbabaaaabaaaaaagcbaaaadmcbabaaaacaaaaaa
gcbaaaadmcbabaaaadaaaaaagcbaaaadmcbabaaaaeaaaaaagcbaaaadecbabaaa
afaaaaaagcbaaaaddcbabaaaagaaaaaagfaaaaadpccabaaaaaaaaaaagiaaaaac
adaaaaaadgaaaaafbcaabaaaaaaaaaaadkbabaaaacaaaaaadgaaaaafccaabaaa
aaaaaaaadkbabaaaadaaaaaadgaaaaafecaabaaaaaaaaaaadkbabaaaaeaaaaaa
dgaaaaafbcaabaaaabaaaaaackbabaaaacaaaaaadgaaaaafccaabaaaabaaaaaa
ckbabaaaadaaaaaadgaaaaafecaabaaaabaaaaaackbabaaaaeaaaaaabaaaaaah
icaabaaaaaaaaaaaegacbaaaaaaaaaaaegacbaaaabaaaaaaaaaaaaahicaabaaa
aaaaaaaadkaabaaaaaaaaaaadkaabaaaaaaaaaaadcaaaaakhcaabaaaaaaaaaaa
egacbaaaabaaaaaapgapbaiaebaaaaaaaaaaaaaaegacbaaaaaaaaaaaefaaaaaj
pcaabaaaaaaaaaaaegacbaaaaaaaaaaaeghobaaaaaaaaaaaaagabaaaabaaaaaa
diaaaaailcaabaaaaaaaaaaaegaibaaaaaaaaaaaegiicaaaaaaaaaaaahaaaaaa
aaaaaaahbcaabaaaabaaaaaabkaabaaaaaaaaaaaakaabaaaaaaaaaaadcaaaaak
ecaabaaaaaaaaaaackaabaaaaaaaaaaackiacaaaaaaaaaaaahaaaaaaakaabaaa
abaaaaaadiaaaaahecaabaaaaaaaaaaackaabaaaaaaaaaaaabeaaaaaklkkkkdo
cpaaaaafecaabaaaaaaaaaaackaabaaaaaaaaaaadiaaaaaiecaabaaaaaaaaaaa
ckaabaaaaaaaaaaackiacaaaaaaaaaaaakaaaaaabjaaaaafecaabaaaaaaaaaaa
ckaabaaaaaaaaaaadcaaaaakhcaabaaaaaaaaaaakgakbaaaaaaaaaaapgipcaaa
aaaaaaaaakaaaaaaegadbaaaaaaaaaaabaaaaaahicaabaaaaaaaaaaaegbcbaaa
abaaaaaaegbcbaaaabaaaaaaeeaaaaaficaabaaaaaaaaaaadkaabaaaaaaaaaaa
dcaaaaakbcaabaaaabaaaaaackbabaiaebaaaaaaabaaaaaadkaabaaaaaaaaaaa
abeaaaaaaaaaiadpdcaaaaajicaabaaaaaaaaaaackbabaaaabaaaaaadkaabaaa
aaaaaaaaabeaaaaaaaaaiadpcpaaaaaficaabaaaaaaaaaaadkaabaaaaaaaaaaa
cpaaaaafccaabaaaabaaaaaaakaabaaaabaaaaaadiaaaaaiccaabaaaabaaaaaa
bkaabaaaabaaaaaaakiacaaaaaaaaaaaakaaaaaabjaaaaafccaabaaaabaaaaaa
bkaabaaaabaaaaaaaaaaaaakhcaabaaaacaaaaaaegiccaaaaaaaaaaaaiaaaaaa
egiccaiaebaaaaaaaaaaaaaaajaaaaaadcaaaaakocaabaaaabaaaaaafgafbaaa
abaaaaaaagajbaaaacaaaaaaagijcaaaaaaaaaaaajaaaaaaaaaaaaahhcaabaaa
aaaaaaaaegacbaaaaaaaaaaajgahbaaaabaaaaaadcaaaaajccaabaaaabaaaaaa
ckbabaaaafaaaaaaabeaaaaaaaaaaadpabeaaaaaaaaaaadpdiaaaaahhcaabaaa
aaaaaaaaegacbaaaaaaaaaaafgafbaaaabaaaaaadiaaaaaihcaabaaaaaaaaaaa
egacbaaaaaaaaaaaegiccaaaaaaaaaaaabaaaaaaefaaaaajpcaabaaaacaaaaaa
egbabaaaagaaaaaaeghobaaaabaaaaaaaagabaaaaaaaaaaadiaaaaahhcaabaaa
aaaaaaaaegacbaaaaaaaaaaapgapbaaaacaaaaaaaaaaaaahhcaabaaaaaaaaaaa
egacbaaaaaaaaaaaegacbaaaaaaaaaaaaaaaaaajccaabaaaabaaaaaabkiacaia
ebaaaaaaaaaaaaaaakaaaaaaabeaaaaaaaaacaebdiaaaaahicaabaaaaaaaaaaa
dkaabaaaaaaaaaaabkaabaaaabaaaaaabjaaaaaficaabaaaaaaaaaaadkaabaaa
aaaaaaaadiaaaaahicaabaaaaaaaaaaaakaabaaaabaaaaaadkaabaaaaaaaaaaa
deaaaaahicaabaaaaaaaaaaadkaabaaaaaaaaaaaabeaaaaaaaaaaaaadiaaaaai
hcaabaaaabaaaaaapgapbaaaaaaaaaaaegiccaaaaaaaaaaaabaaaaaadcaaaaaj
hccabaaaaaaaaaaaegacbaaaabaaaaaapgapbaaaacaaaaaaegacbaaaaaaaaaaa
dgaaaaaficcabaaaaaaaaaaaabeaaaaaaaaaaaaadoaaaaab"
}

SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES"
}

SubProgram "gles3 " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES3"
}

}
	}

#LINE 59

	} 
	FallBack "Diffuse"
}

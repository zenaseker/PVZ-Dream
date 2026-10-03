// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "鱼干/屛效/径向模糊"
{
	Properties
	{
		[Header(RadialBlur)]_radialblurmask("径向模糊遮罩", 2D) = "white" {}
		_radialblurlentgh("径向模糊步长", Float) = 0
		_radialblurpower("径向模糊强度(W)", Float) = 0

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		GrabPass{ }

		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
			#else
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
			#endif


			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
			uniform float _radialblurlentgh;
			uniform sampler2D _radialblurmask;
			uniform float4 _radialblurmask_ST;
			uniform float _radialblurpower;
			inline float4 ASE_ComputeGrabScreenPos( float4 pos )
			{
				#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
				#else
				float scale = 1.0;
				#endif
				float4 o = pos;
				o.y = pos.w * 0.5f;
				o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
				return o;
			}
			

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord1 = screenPos;
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_texcoord3 = v.ase_texcoord1;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float4 screenPos = i.ase_texcoord1;
				float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( screenPos );
				float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
				float2 uv_radialblurmask = i.ase_texcoord2.xy * _radialblurmask_ST.xy + _radialblurmask_ST.zw;
				float4 texCoord142 = i.ase_texcoord3;
				texCoord142.xy = i.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 temp_output_98_0 = ( ( (ase_grabScreenPosNorm).xy - float2( 0.5,0.5 ) ) * _radialblurlentgh * tex2D( _radialblurmask, uv_radialblurmask ).r * ( _radialblurpower * texCoord142.w ) );
				float4 temp_output_100_0 = ( ase_grabScreenPosNorm - float4( temp_output_98_0, 0.0 , 0.0 ) );
				float4 screenColor90 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,temp_output_100_0.xy);
				float4 temp_output_103_0 = ( temp_output_100_0 - float4( temp_output_98_0, 0.0 , 0.0 ) );
				float4 screenColor102 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,temp_output_103_0.xy);
				float4 temp_output_136_0 = ( temp_output_103_0 - float4( temp_output_98_0, 0.0 , 0.0 ) );
				float4 screenColor137 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,temp_output_136_0.xy);
				float4 temp_output_138_0 = ( temp_output_136_0 - float4( temp_output_98_0, 0.0 , 0.0 ) );
				float4 screenColor139 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,temp_output_138_0.xy);
				float4 screenColor141 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( temp_output_138_0 - float4( temp_output_98_0, 0.0 , 0.0 ) ).xy);
				
				
				finalColor = ( ( screenColor90 + screenColor102 + screenColor137 + screenColor139 + screenColor141 ) / 5.0 );
				return finalColor;
			}
			ENDCG
		}
	}
	
	
}
/*ASEBEGIN
Version=18912
6.4;253.6;1164;496.6;2900.82;-1725.046;2.174381;True;True
Node;AmplifyShaderEditor.GrabScreenPosition;89;-1970.302,1874.28;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;95;-1727.186,1927.545;Inherit;False;Constant;_Vector0;Vector 0;9;0;Create;True;0;0;0;False;0;False;0.5,0.5;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;142;-1813.958,2342.818;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;99;-1765.244,2242.738;Inherit;False;Property;_radialblurpower;径向模糊强度(W);2;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;93;-1751.856,1817.679;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;96;-1537.619,1821.046;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;144;-1553.5,2256.927;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-1546.866,1925.13;Inherit;False;Property;_radialblurlentgh;径向模糊步长;1;0;Create;False;0;0;0;False;0;False;0;0.28;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;101;-1705.729,2058.371;Inherit;True;Property;_radialblurmask;径向模糊遮罩;0;1;[Header];Create;False;1;RadialBlur;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;-1342.585,1970.921;Inherit;False;4;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;100;-1188.318,1890.905;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;103;-931.4209,1942.032;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;136;-908.689,2105.613;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;138;-895.0829,2287.045;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;140;-919.9529,2484.167;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ScreenColorNode;90;-692.767,1672.368;Inherit;False;Global;_GrabScreen4;Grab Screen 4;2;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;139;-695.6099,2280.873;Inherit;False;Global;_GrabScreen7;Grab Screen 7;2;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;102;-688.5109,1908.979;Inherit;False;Global;_GrabScreen5;Grab Screen 5;2;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;137;-704.3229,2092.67;Inherit;False;Global;_GrabScreen6;Grab Screen 6;2;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;141;-688.6399,2470.818;Inherit;False;Global;_GrabScreen8;Grab Screen 8;2;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;104;-441.3839,1881.315;Inherit;False;5;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;106;-446.8639,1799.397;Inherit;False;Constant;_Float0;Float 0;12;0;Create;True;0;0;0;False;0;False;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;105;-250.3259,1895.062;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;36;-68.10268,1904.918;Float;False;True;-1;2;ASEMaterialInspector;100;1;鱼干/屛效/径向模糊;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Transparent=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;93;0;89;0
WireConnection;96;0;93;0
WireConnection;96;1;95;0
WireConnection;144;0;99;0
WireConnection;144;1;142;4
WireConnection;98;0;96;0
WireConnection;98;1;97;0
WireConnection;98;2;101;1
WireConnection;98;3;144;0
WireConnection;100;0;89;0
WireConnection;100;1;98;0
WireConnection;103;0;100;0
WireConnection;103;1;98;0
WireConnection;136;0;103;0
WireConnection;136;1;98;0
WireConnection;138;0;136;0
WireConnection;138;1;98;0
WireConnection;140;0;138;0
WireConnection;140;1;98;0
WireConnection;90;0;100;0
WireConnection;139;0;138;0
WireConnection;102;0;103;0
WireConnection;137;0;136;0
WireConnection;141;0;140;0
WireConnection;104;0;90;0
WireConnection;104;1;102;0
WireConnection;104;2;137;0
WireConnection;104;3;139;0
WireConnection;104;4;141;0
WireConnection;105;0;104;0
WireConnection;105;1;106;0
WireConnection;36;0;105;0
ASEEND*/
//CHKSM=4CBE1B43B9723C9CFAB0260F6070523611050900
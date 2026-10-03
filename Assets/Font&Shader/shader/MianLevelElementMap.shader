// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MianLevelElementMap"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		_Mask("Mask", 2D) = "white" {}
		_MaskPower("MaskPower", Range( 0 , 1)) = 0
		_MaskWidth("MaskWidth", Range( 0 , 0.3)) = 0
		_EdgeNoise("EdgeNoise", 2D) = "white" {}
		_EdgeTex("EdgeTex", 2D) = "white" {}
		[HDR]_EdgeCol("EdgeCol", Color) = (1,1,1,1)
		_EdgePos("EdgePos", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref 1
			CompFront Equal
			FailFront Keep
			ZFailFront Keep
			CompBack Equal
			PassBack Keep
			FailBack Keep
			ZFailBack Keep
		}


		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		
		Pass
		{
			Name "Default"
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			#define ASE_NEEDS_FRAG_COLOR

			
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
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord2 : TEXCOORD2;
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform sampler2D _Mask;
			uniform sampler2D _EdgeNoise;
			uniform float4 _EdgeNoise_ST;
			uniform float2 _EdgePos;
			uniform float _MaskPower;
			uniform float _MaskWidth;
			uniform sampler2D _EdgeTex;
			uniform float4 _EdgeTex_ST;
			uniform float4 _EdgeCol;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				float4 ase_clipPos = UnityObjectToClipPos(IN.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				OUT.ase_texcoord2 = screenPos;
				
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float4 screenPos = IN.ase_texcoord2;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 uv_EdgeNoise = IN.texcoord.xy * _EdgeNoise_ST.xy + _EdgeNoise_ST.zw;
				float2 _StartPos = float2(0.5,0.5);
				float4 tex2DNode8 = tex2D( _Mask, ( ( tex2D( _EdgeNoise, uv_EdgeNoise ).r * ( IN.texcoord.xy - _StartPos ) ) + _StartPos + _EdgePos ) );
				float temp_output_12_0 = step( tex2DNode8.r , _MaskPower );
				float temp_output_15_0 = ( step( tex2DNode8.r , ( _MaskPower + _MaskWidth ) ) - temp_output_12_0 );
				float2 uv_EdgeTex = IN.texcoord.xy * _EdgeTex_ST.xy + _EdgeTex_ST.zw;
				float4 lerpResult49 = lerp( ( ( tex2D( _MainTex, ase_screenPosNorm.xy ) * IN.color ) * ( 1.0 - temp_output_12_0 ) ) , ( temp_output_15_0 * tex2D( _EdgeTex, uv_EdgeTex ) * _EdgeCol ) , temp_output_15_0);
				
				half4 color = lerpResult49;
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18912
213;318;1326;638;679.8752;351.2828;2.141968;True;True
Node;AmplifyShaderEditor.Vector2Node;46;-1560.682,357.4344;Inherit;False;Constant;_StartPos;StartPos;4;0;Create;True;0;0;0;False;0;False;0.5,0.5;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexCoordVertexDataNode;29;-1596.108,165.715;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;44;-1339.791,168.1455;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;25;-1419.172,-115.2032;Inherit;True;Property;_EdgeNoise;EdgeNoise;3;0;Create;True;0;0;0;False;0;False;-1;None;3442fe53105380044861c7ca6b9db4ee;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-1051.517,176.5218;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;52;-1010.732,477.6063;Inherit;False;Property;_EdgePos;EdgePos;6;0;Create;True;0;0;0;False;0;False;0,0;0.125,-0.2166667;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;10;-462.8437,634.6261;Inherit;False;Property;_MaskWidth;MaskWidth;2;0;Create;True;0;0;0;False;0;False;0;0.01;0;0.3;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-470.003,528.7423;Inherit;False;Property;_MaskPower;MaskPower;1;0;Create;True;0;0;0;False;0;False;0;0.877;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-788.0372,344.9472;Inherit;True;3;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;2;-636.3302,-121.9382;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenPosInputsNode;50;-700.8366,-1.871185;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-132.6909,619.9909;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-480.5884,314.1216;Inherit;True;Property;_Mask;Mask;0;0;Create;True;0;0;0;False;0;False;-1;None;cac8211a04e76634e8aac3e49efc0d07;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;12;56.94012,397.9288;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-386.7305,-80.3381;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;13;73.27445,624.8936;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;7;-254.4677,129.3656;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;51;372.9089,1131.008;Inherit;False;Property;_EdgeCol;EdgeCol;5;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;2,0.9469364,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;48;265.1231,871.189;Inherit;True;Property;_EdgeTex;EdgeTex;4;0;Create;True;0;0;0;False;0;False;-1;None;e6971b6bf5db543489a1ac67b416fcb6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;18.5323,11.0656;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;36;373.1032,279.351;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;15;347.6041,555.2946;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;694.5093,177.8852;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;674.9077,636.3002;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;49;1068.577,436.8392;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;1840.091,443.8699;Float;False;True;-1;2;ASEMaterialInspector;0;6;MianLevelElementMap;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;False;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;True;0;True;-9;False;False;False;False;False;False;True;True;True;1;False;-5;255;False;-8;255;False;-7;5;False;-4;0;False;-6;1;False;-1;1;False;-1;5;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;44;0;29;0
WireConnection;44;1;46;0
WireConnection;41;0;25;1
WireConnection;41;1;44;0
WireConnection;47;0;41;0
WireConnection;47;1;46;0
WireConnection;47;2;52;0
WireConnection;14;0;9;0
WireConnection;14;1;10;0
WireConnection;8;1;47;0
WireConnection;12;0;8;1
WireConnection;12;1;9;0
WireConnection;3;0;2;0
WireConnection;3;1;50;0
WireConnection;13;0;8;1
WireConnection;13;1;14;0
WireConnection;5;0;3;0
WireConnection;5;1;7;0
WireConnection;36;0;12;0
WireConnection;15;0;13;0
WireConnection;15;1;12;0
WireConnection;35;0;5;0
WireConnection;35;1;36;0
WireConnection;33;0;15;0
WireConnection;33;1;48;0
WireConnection;33;2;51;0
WireConnection;49;0;35;0
WireConnection;49;1;33;0
WireConnection;49;2;15;0
WireConnection;1;0;49;0
ASEEND*/
//CHKSM=2848EE83007606CF533C79F05D90AE95153DE541
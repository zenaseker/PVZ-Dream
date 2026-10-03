// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "鱼干/屛效/集成屛效"
{
	Properties
	{
		[KeywordEnum(ref,chack,move)] _Keyword0("渲染类型", Float) = 0
		[KeywordEnum(Camera,Color)] _Keyword2("UV影响", Float) = 0
		[Header(Ref)]_RAT("扰动贴图", 2D) = "white" {}
		_RAP("扰动强度", Float) = 0
		_RAS("扰动速度", Vector) = (0,0,0,0)
		[Header(Chack)]_Mask1("震动蒙版", 2D) = "white" {}
		RockVec1("震动速率及强度", Vector) = (1,1,1,1)
		[Header(Color)]_ColorMask("色彩蒙版", 2D) = "white" {}
		[Header(Color)]_ColorMask2("色彩蒙版2", 2D) = "white" {}
		[HDR][Gamma]_RefColor("屏幕色彩(W)", Color) = (1,1,1,1)
		[Header(Move)]_MoveXY("位移距离(Z)", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

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
			#include "UnityShaderVariables.cginc"
			#pragma shader_feature_local _KEYWORD2_CAMERA _KEYWORD2_COLOR
			#pragma shader_feature_local _KEYWORD0_REF _KEYWORD0_CHACK _KEYWORD0_MOVE


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
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
			uniform sampler2D _RAT;
			uniform float2 _RAS;
			uniform float _RAP;
			uniform float4 RockVec1;
			uniform sampler2D _Mask1;
			uniform float4 _Mask1_ST;
			uniform float2 _MoveXY;
			uniform float4 _RefColor;
			uniform sampler2D _ColorMask;
			uniform sampler2D _ColorMask2;
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
				o.ase_color = v.color;
				
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
				float2 texCoord39 = i.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner41 = ( 1.0 * _Time.y * _RAS + texCoord39);
				float4 ref44 = ( ase_grabScreenPosNorm + ( tex2D( _RAT, panner41 ) * _RAP ) );
				float2 uv_Mask1 = i.ase_texcoord2.xy * _Mask1_ST.xy + _Mask1_ST.zw;
				float4 tex2DNode52 = tex2D( _Mask1, uv_Mask1 );
				float2 appendResult58 = (float2(( ase_grabScreenPosNorm.r + ( sin( ( RockVec1.x * unity_DeltaTime.y ) ) * RockVec1.z * tex2DNode52.r ) ) , ( ase_grabScreenPosNorm.g + ( RockVec1.w * sin( ( unity_DeltaTime.y * RockVec1.y ) ) * tex2DNode52.r ) )));
				float2 chack60 = appendResult58;
				float2 appendResult83 = (float2(( ase_grabScreenPosNorm.r + _MoveXY.x ) , ( ase_grabScreenPosNorm.g + _MoveXY.y )));
				float4 texCoord145 = i.ase_texcoord3;
				texCoord145.xy = i.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 move79 = ( appendResult83 * texCoord145.z );
				#if defined(_KEYWORD0_REF)
				float4 staticSwitch43 = ref44;
				#elif defined(_KEYWORD0_CHACK)
				float4 staticSwitch43 = float4( chack60, 0.0 , 0.0 );
				#elif defined(_KEYWORD0_MOVE)
				float4 staticSwitch43 = float4( move79, 0.0 , 0.0 );
				#else
				float4 staticSwitch43 = ref44;
				#endif
				float4 screenColor163 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,staticSwitch43.xy);
				float4 screenColor66 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,ase_grabScreenPos.xy/ase_grabScreenPos.w);
				float4 texCoord166 = i.ase_texcoord3;
				texCoord166.xy = i.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float4 lerpResult77 = lerp( i.ase_color , ( _RefColor * texCoord166.w ) , _RefColor.a);
				float4 lerpResult78 = lerp( screenColor66 , lerpResult77 , ( tex2D( _ColorMask, staticSwitch43.xy ).r * tex2D( _ColorMask2, staticSwitch43.xy ).r ));
				#if defined(_KEYWORD2_CAMERA)
				float4 staticSwitch162 = screenColor163;
				#elif defined(_KEYWORD2_COLOR)
				float4 staticSwitch162 = lerpResult78;
				#else
				float4 staticSwitch162 = screenColor163;
				#endif
				
				
				finalColor = staticSwitch162;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18912
-18;391;1326;632;2778.714;-510.9697;1.161579;True;True
Node;AmplifyShaderEditor.CommentaryNode;159;-3103.113,491.6372;Inherit;False;2457.061;563.9897;震动;14;46;47;49;48;51;52;50;54;55;53;57;56;58;60;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector4Node;47;-3053.113,667.8433;Inherit;False;Property;RockVec1;震动速率及强度;6;0;Create;False;0;0;0;False;0;False;1,1,1,1;0.1,0.1,0.01,0.01;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DeltaTime;46;-2839.889,730.2454;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;156;-3103.078,-76.7352;Inherit;False;1759.182;557.8243;扰动;9;42;39;41;38;40;153;24;37;44;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-2572.168,887.232;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-2602.169,618.2316;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;158;-3084.953,1053.172;Inherit;False;1388.908;551.569;位移;8;81;86;87;88;83;145;146;79;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-3053.078,168.9321;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;42;-2973.024,318.8891;Inherit;False;Property;_RAS;扰动速度;4;0;Create;False;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;52;-2288.897,669.1024;Inherit;True;Property;_Mask1;震动蒙版;5;1;[Header];Create;False;1;Chack;0;0;False;0;False;-1;8a641c831f1da9a4dacada029b9cb5e5;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GrabScreenPosition;81;-2960.679,1103.172;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;50;-2404.282,860.0149;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;41;-2716.024,156.8891;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SinOpNode;51;-2392.785,586.4779;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;86;-3034.953,1277.817;Inherit;False;Property;_MoveXY;位移距离(Z);10;1;[Header];Create;False;1;Move;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;38;-2478.079,127.9321;Inherit;True;Property;_RAT;扰动贴图;2;1;[Header];Create;False;1;Ref;0;0;False;0;False;-1;9954ed6387d138d40b9432ee8a7c5d1a;9954ed6387d138d40b9432ee8a7c5d1a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;40;-2320.402,331.0146;Inherit;False;Property;_RAP;扰动强度;3;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;87;-2677.835,1165.88;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;55;-1954.959,684.1292;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1886.732,541.6373;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-1874.27,898.427;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;88;-2680.953,1290.617;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;153;-2165.849,218.7455;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GrabScreenPosition;24;-2891.62,-25.54537;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;56;-1655.223,618.2835;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;57;-1651.023,783.2835;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;83;-2547.11,1190.859;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;145;-2685.824,1400.74;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;146;-2413.642,1180.604;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;58;-1446.489,605.347;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;37;-1938.211,107.346;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;79;-1920.845,1117.495;Inherit;False;move;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;161;-570.7131,901.6918;Inherit;False;710.1307;474.1531;传出;4;62;80;45;43;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;60;-1172.4,594.1804;Inherit;False;chack;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;44;-1568.696,134.0011;Inherit;False;ref;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;45;-517.0601,951.6918;Inherit;False;44;ref;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;157;-344.8788,1247.56;Inherit;False;1166.342;971.1039;色彩;10;154;67;76;71;77;155;66;78;164;166;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;80;-525.1649,1099.174;Inherit;False;79;move;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;62;-520.7131,1030.051;Inherit;False;60;chack;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;43;-321.9466,1042.53;Inherit;False;Property;_Keyword0;渲染类型;0;0;Create;False;0;0;0;False;0;False;0;0;0;True;;KeywordEnum;3;ref;chack;move;Create;True;True;9;1;FLOAT4;0,0,0,0;False;0;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;5;FLOAT4;0,0,0,0;False;6;FLOAT4;0,0,0,0;False;7;FLOAT4;0,0,0,0;False;8;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;71;-63.25998,1584.643;Inherit;False;Property;_RefColor;屏幕色彩(W);9;2;[HDR];[Gamma];Create;False;1;;0;0;True;0;False;1,1,1,1;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;166;-262.2388,1586.75;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;154;-34.61589,1988.664;Inherit;True;Property;_ColorMask2;色彩蒙版2;8;1;[Header];Create;False;1;Color;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;67;-38.62403,1778.116;Inherit;True;Property;_ColorMask;色彩蒙版;7;1;[Header];Create;False;1;Color;0;0;False;0;False;-1;None;6de13cd297a141e4784d88dbf8f26973;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;76;-315.348,1388.683;Inherit;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;151.853,1579.771;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;66;277.463,1297.56;Inherit;False;Global;_GrabScreen2;Grab Screen 2;2;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;77;291.6333,1550.873;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;155;283.5315,1745.859;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;78;452.861,1535.736;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenColorNode;163;235.5891,1018.281;Inherit;False;Global;_GrabScreen0;Grab Screen 0;2;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;162;756.3445,1108.32;Inherit;False;Property;_Keyword2;UV影响;1;0;Create;False;0;0;0;False;0;False;0;0;0;True;;KeywordEnum;2;Camera;Color;Create;True;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;36;998.4188,1110.546;Float;False;True;-1;2;ASEMaterialInspector;100;1;鱼干/屛效/集成屛效;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Transparent=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;49;0;46;2
WireConnection;49;1;47;2
WireConnection;48;0;47;1
WireConnection;48;1;46;2
WireConnection;50;0;49;0
WireConnection;41;0;39;0
WireConnection;41;2;42;0
WireConnection;51;0;48;0
WireConnection;38;1;41;0
WireConnection;87;0;81;1
WireConnection;87;1;86;1
WireConnection;54;0;51;0
WireConnection;54;1;47;3
WireConnection;54;2;52;1
WireConnection;53;0;47;4
WireConnection;53;1;50;0
WireConnection;53;2;52;1
WireConnection;88;0;81;2
WireConnection;88;1;86;2
WireConnection;153;0;38;0
WireConnection;153;1;40;0
WireConnection;56;0;55;1
WireConnection;56;1;54;0
WireConnection;57;0;55;2
WireConnection;57;1;53;0
WireConnection;83;0;87;0
WireConnection;83;1;88;0
WireConnection;146;0;83;0
WireConnection;146;1;145;3
WireConnection;58;0;56;0
WireConnection;58;1;57;0
WireConnection;37;0;24;0
WireConnection;37;1;153;0
WireConnection;79;0;146;0
WireConnection;60;0;58;0
WireConnection;44;0;37;0
WireConnection;43;1;45;0
WireConnection;43;0;62;0
WireConnection;43;2;80;0
WireConnection;154;1;43;0
WireConnection;67;1;43;0
WireConnection;164;0;71;0
WireConnection;164;1;166;4
WireConnection;77;0;76;0
WireConnection;77;1;164;0
WireConnection;77;2;71;4
WireConnection;155;0;67;1
WireConnection;155;1;154;1
WireConnection;78;0;66;0
WireConnection;78;1;77;0
WireConnection;78;2;155;0
WireConnection;163;0;43;0
WireConnection;162;1;163;0
WireConnection;162;0;78;0
WireConnection;36;0;162;0
ASEEND*/
//CHKSM=60D902A43524F83CBF2280F40408CA70903FCA69
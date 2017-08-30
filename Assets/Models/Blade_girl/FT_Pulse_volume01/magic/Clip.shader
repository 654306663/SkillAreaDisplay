Shader "Custom/Clip"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

		_ClipY ("剔除 Y 值", float) = 0

		[Space(10)]_ClipObjPos	("遮罩位置",	Vector)	= ( 0, 0, 0, 1 )
		_ClipObjNormal	("遮罩法线向量",	Vector)	= ( 0, 1, 0, 1 )
	}
	SubShader
	{
		Cull Back

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
			};

			sampler2D _MainTex;
			float _ClipY;
			fixed4 _ClipObjPos;
			fixed4 _ClipObjNormal;

			float distanceToPlane(float3 pos, float3 objNormal, float3 pointInWorld)
			{
			  float3 w = - ( pos - pointInWorld );
			  //根据数学公式，用平面的法向量计算距离
			  float res = ( objNormal.x * w.x + objNormal.y * w.y + objNormal.z * w.z ) 
							/ sqrt( objNormal.x * objNormal.x +	objNormal.y * objNormal.y +	objNormal.z * objNormal.z );
			  return res;
			}

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				//计算模型的真正世界坐标
				o.worldPos = mul(_Object2World, v.vertex).xyz;

				o.uv = v.uv;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				clip(distanceToPlane(_ClipObjPos.xyz, _ClipObjNormal.xyz, i.worldPos));
				clip(i.worldPos.y - _ClipY);
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}


			ENDCG
		}
	}
}

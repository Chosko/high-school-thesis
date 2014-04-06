float4x4 matViewProjection;
float Time;
float TexDimension;
texture BaseTexture;
texture NoiseTexture;


struct VS_INPUT 
{
   float4 Position : POSITION0;
   float2 TexCoord : TEXCOORD0;
   
};

struct VS_OUTPUT 
{
   float4 Position : POSITION0;
   float2 TexCoord0 : TEXCOORD0;
   float2 TexCoord1 : TEXCOORD1;
};

VS_OUTPUT vs_main( VS_INPUT Input )
{
   VS_OUTPUT Output;

   Output.Position = mul( Input.Position, matViewProjection );
   Output.TexCoord1 = Input.TexCoord * 5 * TexDimension;
   Output.TexCoord0 = Input.TexCoord * 50 * TexDimension + float2(Time*0.13f,Time*0.17f);
   
   return( Output );
   
}

sampler2D BaseTex = sampler_state
{
   Texture = (BaseTexture);
   MINFILTER = LINEAR;
   MIPFILTER = LINEAR;
   MAGFILTER = LINEAR;
};

sampler2D NoiseTex = sampler_state
{
   Texture = (NoiseTexture);
   MAGFILTER = LINEAR;
   MINFILTER = LINEAR;
   MIPFILTER = LINEAR;
};

struct PS_INPUT
{
   float2 TexCoord0 : TEXCOORD0;
   float2 TexCoord1 : TEXCOORD1;
};

float4 ps_main(PS_INPUT Input) : COLOR0
{   
   float4 TotalColor;
   float4 Tex0 = tex2D(BaseTex, Input.TexCoord1);
   float4 TexNoise = tex2D(NoiseTex, Input.TexCoord0);
   TotalColor = Tex0;
   TotalColor-=TexNoise;
   //normalize(TotalColor);
   return( TotalColor);
   
}


technique t0
{
   pass Pass_0
   {
      VertexShader = compile vs_2_0 vs_main();
      PixelShader = compile ps_2_0 ps_main();
   }

}


float4x4 matViewProjection;
float4x4 matWorld;
texture colorTex;
texture alphaTex;

sampler2D ringTexture = sampler_state
{
   Texture = (colorTex);
   //MINFILTER = ANISOTROPIC;
   //MIPFILTER = ANISOTROPIC;
   //MAGFILTER = ANISOTROPIC;
};

sampler2D alphaTexture = sampler_state
{
   Texture = (alphaTex);
   MINFILTER = ANISOTROPIC;
   MIPFILTER = ANISOTROPIC;
   MAGFILTER = ANISOTROPIC;
};

struct VS_INPUT 
{
   float4 Position : POSITION0;
   float4 Normal : NORMAL0;
   float2 TexCoord : TEXCOORD0;   
};

struct VS_OUTPUT 
{
   float4 Position : POSITION0;
   float4 Normal : TEXCOORD0;
   float2 TexCoord : TEXCOORD1;   
};

struct PS_INPUT 
{
   float4 Position : POSITION0;
   float4 Normal : TEXCOORD0;
   float2 TexCoord : TEXCOORD1;
};

VS_OUTPUT vs_main( VS_INPUT Input )
{
   VS_OUTPUT Output;
   float4 pos = mul (Input.Position, matWorld);
   float4 norm = mul (Input.Normal, matWorld);
   pos = mul(pos, matViewProjection);
   norm = mul(norm, matViewProjection);
   Output.Position = pos;
   Output.Normal = norm;
   Output.TexCoord = Input.TexCoord;
   return( Output );
}

float4 ps_main(PS_INPUT Input) : COLOR0
{   
   float4 TexColor = float4(0,0,0,0); 
   TexColor.rgb = tex2D(ringTexture, Input.TexCoord);
   TexColor = TexColor/1.5f;
   TexColor.a = tex2D(alphaTexture, Input.TexCoord);
   return( TexColor );
}

technique t0
{
   pass Pass_0
   {
      VertexShader = compile vs_2_0 vs_main();
      PixelShader = compile ps_2_0 ps_main();
   }

}


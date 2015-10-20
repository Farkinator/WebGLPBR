precision mediump float;

varying vec2 vUV;
varying vec3 vLdir;
varying vec3 vNormal;
varying vec3 vEye;

//All the texture maps that we will need to use for various calculations.
uniform sampler2D baseMap;
uniform sampler2D metalMap;
uniform sampler2D roughMap;



constant float PI = 3.1415962;

float chiGGX(float v){
	//If v is greater than 0, return 1. Else, return 0. Known as the chi function in GGX.
	return v>0 ? 1:0;
}

//The Distribution function of GGX.
//What % of our microfacets are facing in the halfvector's direction?
float GGX_D(vec3 normal, vec3 half, float rough){
	ndh = dot(normal, half);
	rough2 = rough * rough;
	ndh2 = ndh * ndh;
	denom = ndh2 * rough2 + (1-ndh2);
	return (chiGGX(ndh) * rough2)/(PI * den * den);
}

//The Geometry function of GGX. Split into two partials.
//Attenuation based by the geometry of overlapping microfacets
float GGX_G(vec3 light, vec3 eye, vec3 half, vec3 normal, float rough){
	return (GGX_Gp(eye, half, normal, rough) * GGX_Gp(light, half, normal, rough));
}
//the partial geometry function.
float GGX_Gp(view, half, normal, rough){
	vdh2 = saturate(dot(view, half));
	chi = chiGGX(vdh2/saturate(dot(view, normal)));
	vdh2 = vdh2 * vdh2;
	tan2 = (1-vdh2)/vdh2;
	return (chi*2)/(1+sqrt(1+rough*rough*tan2));
}
//Calculate the Fresnel effect for GGX. The Schlick approximation.
float GGX_F(float cosT, float F0){
	return F0 + (1-F0) * pow(1-cosT, 5);
}


void main(void){
	vec4 reflectance; float roughness; float F0; vec4 albedo;

	vec4 white = (1.,1.,1.,1.); vec4 black = (0.0.,0.,0.);
	//Is our fragment metallic or dielectric? White (sRGB = 235-255) = metallic, black (sRGB = 0) = dielectric.
	vec4 metallic = texture2D(metalMap, vUV);



	//TODO: Look up reflectance/F0 values for all these metals/dielectrics.
	if(metallic == white){
		//If our fragment is metallic, then we can use our basecolor for reflectance. Basecolor is not needed to make an albedo color.
		reflectance = texture2D(baseMap, vUV);
	} else if (metallic == black) {
		//Set an albedo color according to the baseMap.
		albedo = texture2D(baseMap, vUV);
		F0 = .05;
	}

	//Find the roughness for this fragment.
	roughness = 

}
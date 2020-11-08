extern vec2 resolution;

float Circle(vec2 uv, vec2 pos, float radius, float step_size)
{
  float d = length(uv - pos);                           // distance from every pixel to pos
  float c = smoothstep(radius, radius - step_size, d);  // smooth edge of the circle (reversed to go from high to low!)
  
  return c;
}

vec4 effect(vec4 color, Image texture, vec2 texture_coords, vec2 screen_coords)
{
  vec2 uv = vec2(screen_coords.x / resolution.x, screen_coords.y / resolution.y);
  uv -= .5;                             // make uv range -0.5 to 0.5
  uv.x *= resolution.x / resolution.y;  // multiply x by aspect ratio to compensate for the width vs height difference
  
  vec3 col = vec3(1., 1., 0.);                    // yellow color
  float mask = Circle(uv, vec2(0.), .4, 0.05);    // circle shape
  mask -= Circle(uv, vec2(-.13, -.2), .07, .01);  // left eye
  mask -= Circle(uv, vec2(.13, -.2), .07, .01);   // right eye
  
  float mouth = Circle(uv, vec2(0.), .3, .02);
  mouth -= Circle(uv, vec2(0., -0.1), .3, .02);
  
  mask -= mouth;  
  col *= mask;
  
  return vec4(col, 1);              // use the same c value for r, g and b
}